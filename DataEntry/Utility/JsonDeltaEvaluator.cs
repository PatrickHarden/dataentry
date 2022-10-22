using dataentry.Extensions;
using dataentry.ViewModels.GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dataentry.Utility
{
    public class JsonDeltaEvaluator : IJsonDeltaEvaluator
    {
        private static readonly Dictionary<string, string> MatchingFields = new Dictionary<string, string>
        {
            { "Spaces", "MiqId" },
            { "Photos", "Url" },
            { "Floorplans","Url" },
            { "Brochures", "Url" },
            { "Aspects", "@" },
            { "PropertySizes", "SizeKind" }
        };

        public JsonDeltaEvaluator()
        {

        }

        public IEnumerable<ListingDeltaViewModel> Evaluate(string originalDocument, string newDocument)
        {
            var originalParsed = DeserializeAsJObject(originalDocument);
            var newParsed = DeserializeAsJObject(newDocument);

            IEnumerable<ListingDeltaViewModel> recurse(JToken current, string path, string matchingField, bool currentIsOriginal, bool forceCreate)
            {
                if (current == null) yield break;

                JToken oldToken = null;
                JToken newToken = null;

                var currentDocument = currentIsOriginal ? originalParsed : newParsed;

                oldToken = path == null || forceCreate ? null : originalParsed.SelectToken(path);
                newToken = path == null ? null : newParsed.SelectToken(Regex.Replace(path, @"\?\(new\((\d+)\)\)", @"$1"));

                string oldValue = EvaluateStringValue(oldToken);
                string newValue = EvaluateStringValue(newToken);

                if (oldValue != newValue)
                {
                    yield return new ListingDeltaViewModel
                    {
                        JsonPath = path,
                        OriginalDocumentPath = forceCreate ? null : oldToken?.Path,
                        OriginalValue = oldValue,
                        NewDocumentPath = newToken?.Path,
                        NewValue = newValue
                    };

                    if (oldToken == null)
                    {
                        forceCreate = true;
                    }
                }

                if (newToken != null) // only iterate on create/update, not delete
                {
                    if (current.Type == JTokenType.Object)
                    {
                        foreach (var kv in (JObject)current)
                        {
                            MatchingFields.TryGetValue(kv.Key, out string childmatchingField);

                            var childPath =  $"{path}.{kv.Key}";

                            foreach (var result in recurse(kv.Value, childPath, childmatchingField, currentIsOriginal, forceCreate))
                            {
                                yield return result;
                            }
                        }
                    }

                    else if (current.Type == JTokenType.Array)
                    {
                        var index = 0;
                        foreach (var value in (JArray)current)
                        {
                            string childPath;
                            var childForceCreate = forceCreate;

                            if (matchingField == null)
                            {
                                childPath = $"{path}[{index}]";
                            } 
                            else
                            {
                                string matchingValue;
                                if (matchingField == "@")
                                {
                                    matchingValue = value.ToString(Formatting.None);
                                } 
                                else
                                {
                                    matchingValue = (value as JObject)?.GetValue(matchingField)?.ToString(Formatting.None);
                                }


                                if (matchingValue == null || matchingValue == "0" || matchingValue == "null")
                                {
                                    if (currentIsOriginal)
                                    {
                                        //throw new InvalidOperationException($"Missing matching field in source document.\nField: {matchingField}\nPath: {path}[{index}]\nDocument:{originalDocument}");
                                        continue;
                                    }

                                    childForceCreate = true;
                                    childPath = $"{path}[?(new({index}))]";
                                } 
                                else
                                {
                                    var quoteMatch = Regex.Match(matchingValue, @"^""(.*)""$");
                                    if (quoteMatch.Success)
                                    {
                                        matchingValue = $"'{quoteMatch.Groups[1].Value.Replace("'", "\'")}'";
                                    }

                                    if (matchingField == "@")
                                    {
                                        childPath = $"{path}[?(@=={matchingValue})]";
                                    } 
                                    else
                                    {
                                        childPath = $"{path}[?(@.{matchingField}=={matchingValue})]";
                                    }
                                }
                            }
                            foreach (var result in recurse(value, childPath, null, currentIsOriginal, childForceCreate))
                            {
                                yield return result;
                            }

                            index++;
                        }
                    }
                }
            }

            var returnedFields = new HashSet<string>();

            foreach (var result in recurse(originalParsed, "$", null, true, false))
            {
                if (returnedFields.Add($"{result.OriginalDocumentPath}|{result.NewDocumentPath}")) yield return result;
            }

            foreach (var result in recurse(newParsed, "$", null, false, false))
            {
                if (returnedFields.Add($"{result.OriginalDocumentPath}|{result.NewDocumentPath}")) yield return result;
            }
        }

        public string Apply(string originalDocument, IEnumerable<ListingDeltaViewModel> deltas)
        {
            if (string.IsNullOrWhiteSpace(originalDocument))
            {
                return originalDocument;
            }

            if (deltas == null || !deltas.Any())
            {
                return originalDocument;
            }

            var originalParsed = DeserializeAsJObject(originalDocument);

            var newObjects = new Dictionary<int, JObject>();

            foreach (var delta in deltas.Where(d => d != null))
            {
                var target = originalParsed;
                var path = delta.JsonPath;
                var value = delta.NewValue;

                Match match;

                while ((match = Regex.Match(path, @"\[\s*\?\s*\(\s*new\s*\(\s*(\d+)\s*\)\s*\)\s*\]")).Success)
                {
                    var parent = target.SetToken(path.Substring(0, match.Index), "{array}").Value<JArray>();
                    var index = int.Parse(match.Groups[1].Value);

                    if (!newObjects.TryGetValue(index, out var newTarget))
                    {
                        newTarget = new JObject();
                        newObjects[index] = newTarget;
                        parent.Add(newTarget);
                    }

                    target = newTarget;
                    path = path.Substring(match.Index + match.Length);
                }

                if (value == "undefined")
                {
                    target.MarkTokenForRemoval(path);
                }
                else
                {
                    target.SetToken(path, value);
                }
            }
            originalParsed.RemoveMarkedTokens();

            return Serialize(originalParsed);
        }

        private JObject DeserializeAsJObject(string value)
        {
            var result = JObject.Parse(value);

            return result;
        }

        private string Serialize(JObject obj)
        {
            return obj.ToString(Formatting.None);
        }

        private string EvaluateStringValue(JToken token)
        {
            if (token == null) return "undefined";
            switch (token.Type)
            {
                case JTokenType.Array:
                    return "{array}";
                case JTokenType.Object:
                    return "{object}";
                default:
                    return token.ToString();
            }
        }

        private class FilterData
        {
            public bool Processed;
            public JContainer Container;
            public string StartPath;
            public string EndPath;
            public JToken Result;
        }
    }
}
