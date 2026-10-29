using Core.Common;
using Core.Events.Services;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtainView loadingCurtainView;

        public override void InstallBindings()
        {
            Debug.Log("[Test] Installer runs");
            Container.Bind<LoadingCurtainView>().FromInstance(loadingCurtainView).AsSingle();
            
            Container.Bind<AddressableContainer>().AsSingle();
            Container.Bind<Inventory>().AsSingle();
            Container.Bind<SaveService>().AsSingle();

            Container.Bind<PurchaseService>().AsSingle();
            Container.Bind<EventService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BootEntryPoint>().AsSingle();
            
            DontDestroyOnLoad(this);
        }
    }
}