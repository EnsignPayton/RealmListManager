using Caliburn.Micro;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Events;

namespace RealmListManager.UI.Screens
{
    public class OptionsViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public OptionsViewModel(IConfigurationManager configurationManager, IEventAggregator eventAggregator)
        {
            ConfigurationManager = configurationManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public IConfigurationManager ConfigurationManager { get; }

        public void GoBack()
        {
            _eventAggregator.PublishOnUIThread(new OptionsClosed());
        }
    }
}
