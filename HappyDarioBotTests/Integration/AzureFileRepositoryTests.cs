﻿using System.Collections.Generic;
using System.IO;
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
        public void CanSet_AName()
        {
            bool result = false;
            _azureFileRepository.SetCurrentAudioName("Luca", () => result = true, _ => result = false); 
            Assert.True(result);
        }

        [Fact]
        public void OnError_OnSetName_ItReturns_AMessage()
        {
            string message = "";
            _badAzureFileRepository.SetCurrentAudioName("Luca", () => message = "", error => message = error.Message); 
            Assert.NotEmpty(message);
        }

        [Fact]
        public void Can_Upload_AFile()
        {
            string savedNameOut = "";
            byte[] filetoUpload = File.ReadAllBytes(Path.Combine("resources", "Audio.mp3"));
            _azureFileRepository.Save(filetoUpload, savedName => savedNameOut = savedName, error => {}); 
            Assert.NotEmpty(savedNameOut);
        }

        [Fact]
        public void OnError_FileUpload_ItReturns_AMessage()
        {
            string message = "";
            byte[] filetoUpload = File.ReadAllBytes(Path.Combine("resources", "Audio.mp3"));
            _badAzureFileRepository.Save(filetoUpload, _ => { }, error => message = error.Message);
            Assert.NotEmpty(message);
        }

        [Fact]
        public void Can_Push_InWaitingList()
        {
            _azureFileRepository.PushInWaitingList(494523457, "Luca");
        }

        [Fact]
        public void Can_Create_And_Retrieve_WaitingList()
        {
            int id1 = 123456789;
            int id2 = 00000;
            List<int> expectedWaitingList = new List<int> { id1,  id2 };

            _azureFileRepository.SetCurrentAudioName("bananà", () => { }, error => { });
            _azureFileRepository.ClearWaitingList("Banana");

            _azureFileRepository.PushInWaitingList(id1, "Banana");
            _azureFileRepository.PushInWaitingList(id2, "banana");

            List<int> waitingList = _azureFileRepository.GetWaitingList();
            Assert.Equal(expectedWaitingList, waitingList);
        }

        [Fact]
        public void GetAn_EmptyWatingList_IfNoOne_IsWaiting()
        {
            List<int> expectedWaitingList = new List<int>();

            _azureFileRepository.SetCurrentAudioName("Banana", () => { }, error => { });
            _azureFileRepository.ClearWaitingList("Banana");

            List<int> waitingList = _azureFileRepository.GetWaitingList();
            Assert.Equal(expectedWaitingList, waitingList);
        }
    }
}
