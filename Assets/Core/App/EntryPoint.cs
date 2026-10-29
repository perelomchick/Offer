using System;
using System.Threading;
using Core.Infrastructure.AssetManager.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.App
{
    public class EntryPoint : IInitializable, IDisposable
    {
        private const string GAME_SCENE_NAME = "GameScene";
        
        private readonly LoadingCurtainUI _loadingCurtainUI;
        private readonly AddressableContainer  _addressableContainer;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public EntryPoint(LoadingCurtainUI loadingCurtainUI, AddressableContainer addressableContainer)
        {
            _loadingCurtainUI = loadingCurtainUI;
            _addressableContainer = addressableContainer;
        }

        public void Initialize()
        {
            Boot(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask Boot(CancellationToken ct)
        {
            try
            {
                await _loadingCurtainUI.Show(ct); ;
                await _addressableContainer.LoadPrefabsAsync(new Progress<float>(_loadingCurtainUI.SetProgress), ct);
                await SceneManager.LoadSceneAsync(GAME_SCENE_NAME).ToUniTask(cancellationToken: ct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}