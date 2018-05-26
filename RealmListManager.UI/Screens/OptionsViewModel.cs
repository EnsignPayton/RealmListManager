using Caliburn.Micro;
using RealmListManager.UI.Core;

namespace RealmListManager.UI.Screens
{
    public class OptionsViewModel : Screen
    {
        public OptionsViewModel(IConfigurationManager configurationManager)
        {
            ConfigurationManager = configurationManager;
        }

        public IConfigurationManager ConfigurationManager { get; }
    }
}
