using Core.Infrastructure.AssetManager.Services;

namespace Core.Features.Economy.UI
{
    public class ResourceViewPresenter
    {
        private readonly ResourceView _resourceView;

        public ResourceViewPresenter(UnityEngine.Resources resources
            ,ResourceView resourceView
            , AddressableContainer addressableContainer)
        {
            _resourceView = resourceView;
        }
    }
}