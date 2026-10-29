using Core.Features.Events.Services;
using Core.Infrastructure.AssetManager.Services;
using Core.Infrastructure.SaveLoad.Services;
using Core.Resources;
using UnityEngine;
using Zenject;

namespace Core.App
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtainUI loadingCurtainUI;

        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtainUI>().FromInstance(loadingCurtainUI).AsSingle();
            
            Container.BindInterfacesAndSelfTo<AddressableContainer>().AsSingle();
            Container.Bind<Inventory>().AsSingle();
            Container.Bind<SaveService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
        }
    }
}