using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataentry.Extensions
{
    public static class JObjectExtensions
    {
        public static JToken SetToken(this JContainer container, string path, object value)
        {
            return TokenAction(container, path, true, (c, k) => SetKeyValue(c, k, value));
        }

        private static JToken SetKeyValue(JContainer container, string key, object value)
        {
            var stringValue = value as string;
            string keyToken = key;
            if (container.Type == JTokenType.Array || key.Contains('?'))
            {
                keyToken = $"[{key}]";
            }
            var existing = container.SelectToken(keyToken);
            if (existing != null && (
                    stringValue == "{object}" && existing.Type != JTokenType.Object
                    || stringValue == "{array}" && existing.Type != JTokenType.Array))
            {
                existing.Remove();
                existing = null;
            }

            if (existing == null)
            {
                var newValue = NewValue(value);

                if (container.Type == JTokenType.Array)
                {
                    var array = (JArray)container;
                    if (int.TryParse(key, out int i))
                    {
                        while (i > array.Count)
                        {
                            array.Add(null);
                        }
                    }

                    array.Add(newValue);
                } 
                else
                {
                    container[key] = newValue;
                }

                return newValue;
            }
            else
            {
                if (existing.Type == JTokenType.Object && stringValue == "{object}"
                    || existing.Type == JTokenType.Array && stringValue == "{array}")
                {
                    return existing;
                }

                var newValue = NewValue(value);
                existing.Replace(newValue);
                return newValue;
            }
        }

        private static JToken NewValue(object value)
        {
            if (value as string == "{object}")
            {
                return new JObject();
            }
            if (value as string == "{array}")
            {
                return new JArray();
            }
            return new JValue(value);
        }

        public static void MarkTokenForRemoval(this JContainer container, string path)
        {
            TokenAction(container, path, false, (c, n) =>
            {
                JToken existing = c.SelectToken(c is JArray ? $"[{n}]" : n);
                if (existing != null)
                {
                    existing.Replace("$rm");
                }
                return null;
            });
        }

        public static int RemoveMarkedTokens(this JContainer container)
        {
            int counter = 0;
            recurse(container);
            return counter;

            void recurse(JToken token)
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                    case JTokenType.Array:
                        foreach (var item in token.Values().ToList())
                        {
                            recurse(item);
                        }
                        break;
                    case JTokenType.String:
                        if (token.Value<string>() == "$rm")
                        {
                            token.Remove();
                        }
                        break;
                }
            }
        }

        private static JToken TokenAction(JContainer container, string path, bool buildTree, Func<JContainer, string, JToken> action)
        {
            IEnumerable<(string, JTokenType)> tokens = Tokenize(path);

            var target = container;
            var parentIsArray = container.Type == JTokenType.Array;
            foreach (var token in tokens)
            {
                JToken newJToken;

                if (token.Item1.StartsWith('?'))
                {
                    newJToken = target.SelectToken($"[{token.Item1}]");
                }
                else if (parentIsArray)
                {
                    var i = int.Parse(token.Item1);
                    if (i < ((JArray)target).Count)
                    {
                        newJToken = target[int.Parse(token.Item1)];
                    } 
                    else
                    {
                        newJToken = null;
                    }
                }
                else
                {
                    newJToken = target.SelectToken(token.Item1);
                }

                if (newJToken != null && newJToken.Type == token.Item2 && newJToken.Type != JTokenType.Undefined)
                {
                    target = newJToken.Value<JContainer>();
                }
                else
                {
                    switch (token.Item2)
                    {
                        case JTokenType.Object:
                            if (buildTree)
                            {
                                var newObject = new JObject();
                                if (parentIsArray)
                                {
                                    ((JArray)target).Add(newObject);
                                } 
                                else
                                {
                                    ((JObject)target).Add(token.Item1, newObject);
                                }
                                target = newObject;
                                break;
                            } 
                            else
                            {
                                return null;
                            }
                        case JTokenType.Array:
                            if (buildTree)
                            {
                                var newArray = new JArray();
                                if (parentIsArray)
                                {
                                    ((JArray)target).Add(newArray);
                                }
                                else
                                {
                                    ((JObject)target).Add(token.Item1, newArray);
                                }
                                target = newArray;
                                break;
                            } 
                            else
                            {
                                return null;
                            }
                        default:
                            return action(target, token.Item1);
                    }
                }

                parentIsArray = token.Item2 == JTokenType.Array;
            }

            throw new InvalidOperationException("JSON parsing error");
        }

        public static void Set(this JArray array, int index, JToken value)
        {
            while (array.Count < index)
            {
                array.Add(null);
            }

            if (array.Count > index)
            {
                array[index] = new JValue(value);
            }
            else
            {
                array.Add(value);
            }
        }

        public static IEnumerable<(string, JTokenType)> Tokenize(string path)
        {
            var lastSeparator = 0;
            var index = -1;
            int parentheses = 0;
            while (++index < path.Length)
            {
                var currentChar = path[index];
                if (parentheses < 1)
                {
                    if (currentChar == '.')
                    {
                        yield return (path.Substring(lastSeparator, index - lastSeparator).Trim().TrimEnd(']'), JTokenType.Object);
                        lastSeparator = index + 1;
                        continue;
                    }
                    else if (currentChar == '[')
                    {
                        yield return (path.Substring(lastSeparator, index - lastSeparator).Trim().TrimEnd(']'), JTokenType.Array);
                        lastSeparator = index + 1;
                        continue;
                    }
                }
                if (currentChar == '(')
                {
                    parentheses++;
                }
                else if (currentChar == ')')
                {
                    parentheses--;
                }
            }

            yield return (path.Substring(lastSeparator).Trim().TrimEnd(']'), JTokenType.Undefined);
        }

        public static string Detokenize(IEnumerable<(string, JTokenType)> tokens)
        {
            var result = new StringBuilder();
            var lastType = JTokenType.Undefined;
            foreach (var token in tokens)
            {
                if (lastType == JTokenType.Object)
                {
                    result.Append(".");
                    result.Append(token.Item1);
                }
                else if (lastType == JTokenType.Array)
                {
                    result.Append("[");
                    result.Append(token.Item1);
                    result.Append("]");
                }
                else 
                {
                    result.Append(token.Item1);
                }
                lastType = token.Item2;
            }

            return result.ToString();
        }
    }
}
