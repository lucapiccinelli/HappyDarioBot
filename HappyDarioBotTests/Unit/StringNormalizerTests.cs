using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyDarioBot;
using Xunit;

namespace HappyDarioBotTests.Unit
{
    public class StringNormalizerTests
    {
        [Theory]
        [InlineData("banana", "banana")]
        [InlineData("Banana", "banana")]
        [InlineData("bananà", "banana")]
        [InlineData("Bananà", "banana")]
        [InlineData("BéNèNà", "benena")]
        public void Strings_AreNormalized_RemovingAllAccents_AndLowerCasing(string input, string expected)
        {
            StringNormalizer normalizer = new StringNormalizer();
            string normalized = normalizer.Normalize(input);

            Assert.Equal(expected, normalized);
        }
    }
}
