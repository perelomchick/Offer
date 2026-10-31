using System;
using System.IO;
using System.Threading;
using Core.Infrastructure.SaveLoad.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Infrastructure.SaveLoad.Services
{
    public class SaveService
    {
        private readonly string _pathToMainFolder;

        public SaveService()
        {
            _pathToMainFolder = Application.persistentDataPath;
        }

        public async UniTask<T> Load<T>(string folderName,
            string fileName,
            CancellationToken ct = default)
            where T : ISaveData, new()
        {
            var pathToFile = Path.Combine(_pathToMainFolder,folderName, fileName);

            if (!File.Exists(pathToFile))
            {
                return new T();
            }

            try
            {
                var json = await File.ReadAllTextAsync(pathToFile,ct);
                await UniTask.SwitchToMainThread();
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async UniTask Save(string folderName,
            string fileName,
            ISaveData data,
            CancellationToken ct = default)
        {
            var pathToFolder = Path.Combine(_pathToMainFolder, folderName);
            var pathToFile = Path.Combine(pathToFolder, fileName);

            if (!Directory.Exists(pathToFile))
            {
                Directory.CreateDirectory(pathToFolder);
            }

            try
            {
                await File.WriteAllTextAsync(pathToFile, data.ToJson(),ct);
                await UniTask.SwitchToMainThread();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}