using System;
using System.IO;
using System.Threading.Tasks;
using Core.Common.Data;
using Core.Infrastructure.SaveLoad.Data;
using Core.Infrastructure.SaveLoad.Services;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

namespace Core.Tests
{
    public class SaveLoadTest
    {
        [Test]
        public void WhenLoad_AndFileIsEmpty_ThenReturnNewData()
        {
            // Arrange.
            string folderSave = ConstName.NAME_FOLDER_SAVE;
            string fileName = "TestFile.json";
            SaveService  saveService = new SaveService();
            
            // Act.
            var result = saveService.Load<TestSaveData>(folderSave, fileName);
            
            // Assert.
            result.Should().NotBeNull();
        }
        
        private class TestSaveData : ISaveData
        {
            public int Number;
            
            public string ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }
    }
}