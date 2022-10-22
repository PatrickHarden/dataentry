using dataentry.Extensions;
using dataentry.Utility;
using Xunit;

namespace dataentry.Test.Utility
{
    public class EnumAliasUtilityTest
    {
        public enum TestEnum
        {
            [Alias(AliasType.MIQ, "Alias1", "Alias2", "Alias3")]
            TestValue1, [Alias(AliasType.StoreApi, "Alias1", "Alias2")]
            TestValue2, [Alias(AliasType.StoreApi, "StoreApiAlias")]
            [Alias(AliasType.MIQ, "MIQAlias")]
            TestValue3,
            TestValue4, [Alias(AliasType.MIQ, "Alias3", Primary = true)]
            TestValue5
        }

        public EnumAliasUtilityTest()
        {
            EnumAliasExtensions.Init<TestEnum>();
        }

        [Theory]
        [InlineData(AliasType.MIQ, "Alias1", TestEnum.TestValue1)]
        [InlineData(AliasType.MIQ, "Alias2", TestEnum.TestValue1)]
        [InlineData(AliasType.StoreApi, "Alias1", TestEnum.TestValue2)]
        [InlineData(AliasType.StoreApi, "Alias2", TestEnum.TestValue2)]
        [InlineData(AliasType.StoreApi, "StoreApiAlias", TestEnum.TestValue3)]
        [InlineData(AliasType.MIQ, "MIQAlias", TestEnum.TestValue3)]
        [InlineData(AliasType.MIQ, "Alias3", TestEnum.TestValue5)]
        [InlineData(AliasType.MIQ, "Alias4", null)]
        public void EnumAliasUtility_GetEnum(AliasType aliasType, string alias, TestEnum? expected)
        {
            Assert.Equal(expected, alias.ToEnum<TestEnum>(aliasType));
        }

        [Theory]
        [InlineData(AliasType.MIQ, TestEnum.TestValue1, "Alias1")]
        [InlineData(AliasType.MIQ, TestEnum.TestValue2, "TestValue2")]
        [InlineData(AliasType.MIQ, TestEnum.TestValue3, "MIQAlias")]
        [InlineData(AliasType.MIQ, TestEnum.TestValue4, "TestValue4")]
        [InlineData(AliasType.MIQ, TestEnum.TestValue5, "Alias3")]
        [InlineData(AliasType.StoreApi, TestEnum.TestValue1, "TestValue1")]
        [InlineData(AliasType.StoreApi, TestEnum.TestValue2, "Alias1")]
        [InlineData(AliasType.StoreApi, TestEnum.TestValue3, "StoreApiAlias")]
        [InlineData(AliasType.StoreApi, TestEnum.TestValue4, "TestValue4")]
        [InlineData(AliasType.StoreApi, TestEnum.TestValue5, "TestValue5")]
        public void EnumAliasUtility_GetAlias(AliasType aliasType, TestEnum value, string expected)
        {
            Assert.Equal(expected, value.ToAlias<TestEnum>(aliasType));
        }
    }
}