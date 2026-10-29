using Core.Common;
using Core.Events.Services;
using Zenject;

namespace Core
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AddressableContainer>().AsSingle();
            Container.Bind<Inventory>().AsSingle();
            Container.Bind<SaveService>().AsSingle();

            Container.Bind<PurchaseService>().AsSingle();
            Container.Bind<EventService>().AsSingle();
        }
    }
}