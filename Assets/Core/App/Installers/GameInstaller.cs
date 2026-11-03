using Core.Features.Economy.Factory;
using Core.Features.Economy.Logic;
using Core.Features.Economy.Presenters;
using Core.Features.Economy.Save;
using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using Core.Features.Events.Factories;
using Core.Features.Events.Presenters;
using Core.Features.Events.Save;
using Core.Features.Events.Services;
using Core.Features.Events.UI.Events;
using Core.Features.TimeClock.Services;
using UnityEngine;
using Zenject;

namespace Core.App.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private EventContainer _eventContainer;
        [SerializeField] private EventData[] _eventData;
        
        public override void InstallBindings()
        {
            BindLogicAndData();
            BindFromFactory();
            BindServices();

            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }
        
        private void BindLogicAndData()
        {            
            Debug.Log("BindLogic");
            Container.Bind<Inventory>().AsSingle();
            Container.Bind<EventContainer>().FromInstance(_eventContainer).AsSingle();
            Container.BindInstance(_eventData).AsSingle();
        }
        
        private void BindFromFactory()
        {
            Debug.Log("BindFromFactory");
            Container.BindInterfacesAndSelfTo<EventWindowPresenter>()
                .FromFactory<EventWindowPresenter,EventWindowFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryUI>()
                .FromFactory<InventoryUI, InventoryUIFactory>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindServices()
        {
            Debug.Log("BindServices");
            Container.BindInterfacesAndSelfTo<ServerTimeService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<InventoryUIPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventorySaver>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PurchaseProgressSaver>().AsSingle();     
            
            Container.BindInterfacesAndSelfTo<EventEntryFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
        }
    }
}