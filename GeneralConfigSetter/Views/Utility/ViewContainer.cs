using GeneralConfigSetter.Views.Pages;

namespace GeneralConfigSetter.Views.Utility
{
    public class ViewContainer
    {
        public ViewContainer(ConfigUpdateView configUpdateView)
        {
            ConfigUpdateView = configUpdateView;
        }

        public ConfigUpdateView ConfigUpdateView { get; }
    }
}
