using System;
using System.IO;
using Core.Events.Data;
using UnityEngine;

namespace Core.Events.Services
{
    public class SaveService
    {
        private readonly string _folder;
        private readonly string _path;

        public SaveService()
        {
            _folder = GetSaveFolder();
            Directory.CreateDirectory(_folder);
            _path = Path.Combine(_folder, "save.json");
        }

        private static string GetSaveFolder()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "SaveData");
#else
            return Path.Combine(Application.persistentDataPath, "save.json");
#endif
        }

        public SaveData Load()
        {
            if (!File.Exists(_path))
                return new SaveData();

            try
            {
                return JsonUtility.FromJson<SaveData>(File.ReadAllText(_path)) ?? new SaveData();
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось прочитать сохранение: {e}");
                return new SaveData();
            }
        }

        public void Save(SaveData data)
        {
            File.WriteAllText(_path, JsonUtility.ToJson(data, true));

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}