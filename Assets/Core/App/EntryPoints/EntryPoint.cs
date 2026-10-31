using System;
using Zenject;
using UnityEngine;
using System.Threading;
using Core.App.Data;
using Core.Features.Economy.ResourceData;
using Core.Features.Economy.Save;
using Core.Features.Events.Save;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using Core.Infrastructure.SaveLoad.Data;
using Core.Infrastructure.SaveLoad.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.App.EntryPoints
{
    public class EntryPoint : IInitializable, IDisposable
    {
        private readonly LoadingCurtainUI _loadingCurtainUI;
        private readonly AddressableContainer  _addressableContainer;
        private readonly ZenjectSceneLoader  _sceneLoader;
        private readonly SaveService _saveService;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public EntryPoint(LoadingCurtainUI loadingCurtainUI,
            AddressableContainer addressableContainer,
            SaveService saveService, 
            ZenjectSceneLoader sceneLoader)
        {
            _loadingCurtainUI = loadingCurtainUI;
            _addressableContainer = addressableContainer;
            _saveService = saveService;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            Boot(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask Boot(CancellationToken ct)
        {
            try
            {
                await _loadingCurtainUI.Show(ct);
                await RegisterAddressableContainer(ct);
                var eventSaveData = await LoadingData<EventSaveData>(ConstName.NAME_EVENT_SAVE_FILE,ct);
                var inventorySaveData = await LoadingData<InventorySaveData>(ConstName.NAME_INVENTORY_SAVE_FILE,ct);
                
                await _sceneLoader.LoadSceneAsync(
                    ConstName.GAME_SCENE_NAME,
                    LoadSceneMode.Single,
                    container =>
                    {
                        container.BindInstance(eventSaveData);
                        container.BindInstance(inventorySaveData);
                    })
                    .ToUniTask(cancellationToken:ct);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async UniTask<T> LoadingData<T>(string fileName, CancellationToken ct)
        where T : ISaveData, new()
        {
            var result =  await _saveService.Load<T>(ConstName.NAME_FOLDER_SAVE,fileName,ct);
            
            if (result == null)
            {
                result = new T();
            }

            return result;
        }


        private async UniTask<EventSaveData> LoadEventData(CancellationToken ct)
        {
            var result = await _saveService.Load<EventSaveData>(ConstName.NAME_FOLDER_SAVE, ConstName.NAME_EVENT_SAVE_FILE,ct);

            if (result == null)
            {
                result = new EventSaveData();
            }

            return result;
        }

        private async UniTask RegisterAddressableContainer(CancellationToken ct)
        {
            _addressableContainer.Register<TypeResource, Sprite>(ConstName.PATH_TO_SPRITE_RES);
            _addressableContainer.Register<UiElements, GameObject>(ConstName.PATH_TO_VIEW_PREF);
            await _addressableContainer.LoadAll(new Progress<float>(_loadingCurtainUI.SetProgress), ct);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}