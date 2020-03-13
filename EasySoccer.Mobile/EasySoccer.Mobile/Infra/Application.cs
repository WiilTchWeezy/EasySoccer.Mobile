using EasySoccer.Mobile.API.Session;
using Prism.Navigation;

namespace EasySoccer.Mobile.Infra
{
    public class Application
    {
        private static Application _application;
        public static Application Instance
        {
            get
            {
                if (_application == null)
                    _application = new Application();
                return _application;
            }
        }

        public void LogOff(INavigationService navigationService)
        {
            CurrentUser.Instance.LogOff();
        }
    }
}
