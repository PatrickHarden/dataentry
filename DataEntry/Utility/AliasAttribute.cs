using System;

namespace dataentry.Utility
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// What type of alias this is. Each alias type has a separate set of aliases.
        /// </summary>
        public AliasType AliasType;

        /// <summary>
        /// Aliases to register with the alias type.
        /// </summary>
        public string[] Matches;

        /// <summary>
        /// If set to true, this attribute will override any previous aliases. 
        /// Useful if multiple enum values have the same alias and you want to control which value is retrieved from <see cref="EnumAliasExtensions.ToEnum"/>.
        /// </summary>
        public bool Primary = false;
        
        /// <summary>
        /// Define a set of aliases for an enum member for <see cref="AliasType.Default" />
        /// </summary>
        /// <param name="matches">Aliases to register with <see cref="AliasType.Default" />.</param>
        public AliasAttribute(params string[] matches) : this(AliasType.Default, matches) {}
        /// <summary>
        /// Define a set of aliases for an enum member
        /// </summary>
        /// <param name="aliasType">What type of alias this is. Each alias type has a separate set of aliases.</param>
        /// <param name="matches">Aliases to register with the alias type.</param>
        public AliasAttribute(AliasType aliasType, params string[] matches)
        {
            AliasType = aliasType;
            Matches = matches;
        }
    }
}