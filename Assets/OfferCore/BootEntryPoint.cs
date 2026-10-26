using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OfferCore.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace OfferCore
{
    public class BootEntryPoint : IInitializable, IDisposable
    {
        private const string GAME_SCENE_NAME = "GameScene";
        
        private readonly LoadingCurtainView _loadingCurtainView;
        private readonly AddressableContainer  _addressableContainer;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public BootEntryPoint(LoadingCurtainView loadingCurtainView, AddressableContainer addressableContainer)
        {
            _loadingCurtainView = loadingCurtainView;
            _addressableContainer = addressableContainer;
        }

        public void Initialize()
        {
            Debug.Log("[Test] Boot Initialize");
            Boot(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask Boot(CancellationToken ct)
        {
            try
            {
                Debug.Log("[Boot] Show curtain");
                await _loadingCurtainView.Show(ct);

                Debug.Log("[Boot] Load addressables");
                await _addressableContainer.LoadPrefabsAsync(new Progress<float>(_loadingCurtainView.SetProgress), ct);

                Debug.Log("[Boot] Load scene");
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