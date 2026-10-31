using Core.Features.Economy.Logic;
using Core.Features.Economy.Save;
using Core.Features.Events.Data;
using Core.Features.Events.Factories;
using Core.Features.Events.Save;
using Core.Features.Events.Services;
using Core.Features.Events.View.Events;
using UnityEngine;
using Zenject;

namespace Core.App.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private  EventContainer _eventContainer;
        [SerializeField] private EventData[] _eventData;
        
        public override void InstallBindings()
        {
            Container.Bind<EventContainer>().FromInstance(_eventContainer).AsSingle();
            
            Container.BindInstance(_eventData).AsSingle();
            Container.BindInterfacesAndSelfTo<EventWindowPresenter>()
                .FromFactory<EventWindowPresenter,EventWindowFactory>()
                .AsSingle();
            
            BindServices();

            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<Inventory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<InventorySaver>().AsSingle();
            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PurchaseProgressSaver>().AsSingle();            
            Container.BindInterfacesAndSelfTo<EventEntryFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
        }
    }
}