using OfferCore.Common;
using OfferCore.Events.Services;
using Zenject;

namespace OfferCore
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