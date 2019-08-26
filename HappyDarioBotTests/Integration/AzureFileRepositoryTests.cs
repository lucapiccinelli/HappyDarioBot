using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyDarioBot;
using Microsoft.WindowsAzure.Storage.File;
using Xunit;

namespace HappyDarioBotTests.Integration
{
    public class AzureFileRepositoryTests
    {
        private readonly AzureFileRepository _azureFileRepository;
        private readonly AzureFileRepository _badAzureFileRepository;

        public AzureFileRepositoryTests()
        {
            _azureFileRepository = new AzureFileRepository(
                DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey),
                DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey),
                "happydariobottests");

            _badAzureFileRepository = new AzureFileRepository(
                "x",
                DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey),
                "xasxsaxsa");
        }

        [Fact]
        public void CanRetrieve_File_FromCloud()
        {
            Assert.True(_azureFileRepository.HasAnAudio("gesu", _ => true, () => false));
        }

        [Fact]
        public void IfFile_DoesNotExists_ReturnFalse()
        {
            Assert.False(_azureFileRepository.HasAnAudio("xaiuydeew", _ => true, () => false));
        }

        [Fact]
        public async void CanSet_AName()
        {
            bool result = false;
            await _azureFileRepository.SetCurrentAudioName("Luca", () => result = true, _ => result = false); 
            Assert.True(result);
        }

        [Fact]
        public async void OnError_ItReturns_AMessage()
        {
            string message = "";
            await _badAzureFileRepository.SetCurrentAudioName("Luca", () => message = "", error => message = error.Message); 
            Assert.NotEmpty(message);
        }
    }
}
