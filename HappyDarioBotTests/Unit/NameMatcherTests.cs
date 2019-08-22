using System.Collections.Generic;
using System.Threading.Tasks;
using HappyDarioBot;
using Xunit;

namespace HappyDarioBotTests.Unit
{
    public class NameMatcherTests
    {
        [Theory]
        [InlineData("gesu", "Gesù.aac")]
        [InlineData("Gesù", "Gesù.aac")]
        [InlineData("gesú", "Gesù.aac")]
        [InlineData("geSú", "Gesù.aac")]
        [InlineData("banana", "baNana.wav")]
        [InlineData("ciao", "ciao.mp3")]
        public void MatchesNames(string name, string expectedFilename)
        {
            string[] namesList = {"ciao.mp3", "Gesù.aac", "baNana.wav"};

            Assert.Equal(expectedFilename, new NameMatcher(namesList).Match(name));

        }
    }
}
