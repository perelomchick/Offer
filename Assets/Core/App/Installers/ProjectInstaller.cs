using Core.App.EntryPoints;
using Core.Features.TimeClock.Services;
using Core.Infrastructure.AssetManager.Services;
using Core.Infrastructure.SaveLoad.Services;
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
            Container.BindInterfacesAndSelfTo<ServerTimeService>().AsSingle();
            
            Container.Bind<SaveService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
        }
    }
}