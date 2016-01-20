using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using iExporter.wpf.Services;
using iExporter.wpf.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace iExporter.wpf.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel Main => Get<MainViewModel>();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register<IiTunesLibraryService, iTunesLibraryService>();

            Register<MainViewModel>();
        }

        internal static T Get<T>() where T : class
        {
            return SimpleIoc.Default.GetInstance<T>();
        }

        private static void Register<T>(bool createImmediately = false) where T : class
        {
            SimpleIoc.Default.Register<T>(createImmediately);
        }
    }
}