using Core.Common;

namespace Core.Events.View.Resources
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