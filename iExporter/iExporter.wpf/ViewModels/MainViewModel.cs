using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using iExporter.wpf.Models;
using iExporter.wpf.Services.Interfaces;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace iExporter.wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _canLoadLibrary = false;
        private bool _canExportLibrary = false;

        private IiTunesLibraryService _iTunesLibraryService;

        private string _iTunesLibraryFileLocation;
        public string iTunesLibraryFileLocation
        {
            get { return _iTunesLibraryFileLocation; }
            set { Set(() => iTunesLibraryFileLocation, ref _iTunesLibraryFileLocation, value); }
        }

        private string _iTunesLibraryFolderLocation;
        public string iTunesLibraryFolderLocation
        {
            get { return _iTunesLibraryFolderLocation; }
            set { Set(() => iTunesLibraryFolderLocation, ref _iTunesLibraryFolderLocation, value); }
        }

        //private ObservableCollection<iTunesTrack> _iTunesTracks = new ObservableCollection<iTunesTrack>();
        //public ObservableCollection<iTunesTrack> iTunesTracks
        //{
        //    get { return _iTunesTracks; }
        //    set { Set(() => iTunesTracks, ref _iTunesTracks, value); }
        //}

        private RelayCommand _loadCommand;        
        public RelayCommand LoadCommand => _loadCommand ?? (_loadCommand = new RelayCommand(async () => await LoadLibrary(), () => _canLoadLibrary));

        private RelayCommand _exportCommand;        
        public RelayCommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(async () => await ExportLibrary(), () => _canExportLibrary));

        private RelayCommand _selectLibraryFileCommand;        
        public RelayCommand SelectLibraryFileCommand => _selectLibraryFileCommand ?? (_selectLibraryFileCommand = new RelayCommand(SelectLibrary));

        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand => _selectFolderCommand ?? (_selectFolderCommand = new RelayCommand(SelectFolder));

        public MainViewModel(IMessenger messenger, IiTunesLibraryService iTunesLibraryService) : base(messenger)
        {
            _iTunesLibraryService = iTunesLibraryService;
        }

        private void SelectLibrary()
        {
            OpenFileDialog selectLibraryDialog = new OpenFileDialog();
            selectLibraryDialog.Filter = "iTunes library (*.xml)|*.xml";

            if (selectLibraryDialog.ShowDialog() == true)
                iTunesLibraryFileLocation = selectLibraryDialog.FileName;

            _canLoadLibrary = !string.IsNullOrEmpty(iTunesLibraryFileLocation);
            LoadCommand.RaiseCanExecuteChanged();
        }

        private void SelectFolder()
        {
            FolderBrowserDialog selectFolderDialog = new FolderBrowserDialog();
            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
                iTunesLibraryFolderLocation = selectFolderDialog.SelectedPath;

            _canExportLibrary = !string.IsNullOrEmpty(iTunesLibraryFolderLocation);
            ExportCommand.RaiseCanExecuteChanged();
        }

        private async Task LoadLibrary()
        {
            //iTunesTracks.Clear();

            string itunesLibraryContent = File.ReadAllText(iTunesLibraryFileLocation);
            List<iTunesTrack> iTunesTrackList = _iTunesLibraryService.ParseLibrary(itunesLibraryContent);

            //foreach(iTunesTrack track in iTunesTrackList)
            //    iTunesTracks.Add(track);
        }

        private async Task ExportLibrary()
        {
            throw new System.NotImplementedException();
        }

    }
}
