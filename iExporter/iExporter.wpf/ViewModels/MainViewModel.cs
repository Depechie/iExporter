using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace iExporter.wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _canLoadLibrary = false;
        private bool _canExportLibrary = false;

        private string _iTunesLibraryFileLocation;
        public string iTunesLibraryFileLocation
        {
            get { return _iTunesLibraryFileLocation; }
            set { Set(() => iTunesLibraryFileLocation, ref _iTunesLibraryFileLocation, value); }
        }

        private RelayCommand _loadCommand;        
        public RelayCommand LoadCommand => _loadCommand ?? (_loadCommand = new RelayCommand(async () => await LoadLibrary(), () => _canLoadLibrary));

        private RelayCommand _exportCommand;        
        public RelayCommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(async () => await ExportLibrary(), () => _canExportLibrary));

        private RelayCommand _selectLibraryFileCommand;
        public RelayCommand SelectLibraryFileCommand => _selectLibraryFileCommand ?? (_selectLibraryFileCommand = new RelayCommand(SelectLibrary));

        public MainViewModel(IMessenger messenger) : base(messenger)
        {
        }

        private void SelectLibrary()
        {
            OpenFileDialog openLibraryDialog = new OpenFileDialog();
            openLibraryDialog.Filter = "iTunes library (*.xml)|*.xml";

            if (openLibraryDialog.ShowDialog() == true)
                iTunesLibraryFileLocation = openLibraryDialog.FileName;
        }

        private void SelectFolder()
        {
        }

        private async Task LoadLibrary()
        {
            throw new System.NotImplementedException();
        }

        private async Task ExportLibrary()
        {
            throw new System.NotImplementedException();
        }

    }
}
