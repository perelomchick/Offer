using System;
using Zenject;
using System.IO;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using Core.Infrastructure.SaveLoad.Data;

namespace Core.Infrastructure.SaveLoad.Services
{
    public class SaveService
    {
        private readonly string _pathToMainFolder;

        [Inject]
        public SaveService() : this(Application.persistentDataPath) { }

        public SaveService(string pathToMainFolder)
        {
            _pathToMainFolder = pathToMainFolder;
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
                Debug.LogException(e);
                throw;
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
                Debug.LogException(e);
                throw;
            }
        }
    }
}