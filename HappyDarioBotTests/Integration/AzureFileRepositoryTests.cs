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
        [Fact]
        public void CanRetrieve_File_FromCloud()
        {
            AzureFileRepository fileRepository = new AzureFileRepository(
                DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey),
                DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey));
            Assert.True(fileRepository.HasAnAudio("gesu", _ => true, () => false));
        }

        [Fact]
        public void IfFile_DoesNotExists_ReturnFalse()
        {
            AzureFileRepository fileRepository = new AzureFileRepository(
                DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey),
                DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey));
            Assert.False(fileRepository.HasAnAudio("xaiuydeew", _ => true, () => false));
        }
    }
}
