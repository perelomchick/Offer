using Core.App.EntryPoints;
using Core.Infrastructure.AssetManager.Services;
using Core.Infrastructure.SaveLoad.Services;
using UnityEngine;
using Zenject;

namespace Core.App.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtainUI loadingCurtainUI;

        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtainUI>().FromInstance(loadingCurtainUI).AsSingle();
            Container.BindInterfacesAndSelfTo<AddressableContainer>().AsSingle(); ;
            Container.Bind<SaveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
        }
    }
}