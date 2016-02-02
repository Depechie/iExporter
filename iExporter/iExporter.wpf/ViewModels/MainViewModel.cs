using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using iExporter.wpf.Extensions;
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
        private List<iTunesTrack> _iTunesTrackList;
        private List<TreeViewPlaylist> _localPlayLists = new List<TreeViewPlaylist>();

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

        private ObservableCollection<TreeViewArtist> _iTunesArtists = new ObservableCollection<TreeViewArtist>();
        public ObservableCollection<TreeViewArtist> iTunesArtists
        {
            get { return _iTunesArtists; }
            set { Set(() => iTunesArtists, ref _iTunesArtists, value); }
        }

        private ObservableCollection<TreeViewPlaylist> _iTunesPlaylists = new ObservableCollection<TreeViewPlaylist>();
        public ObservableCollection<TreeViewPlaylist> iTunesPlaylists
        {
            get { return _iTunesPlaylists; }
            set { Set(() => iTunesPlaylists, ref _iTunesPlaylists, value); }
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set { Set(() => SelectedTab, ref _selectedTab, value); }
        }

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
            iTunesArtists.Clear();

            string itunesLibraryContent = File.ReadAllText(iTunesLibraryFileLocation);

            List<iTunesTrack> parsedTracks = new List<iTunesTrack>();
            List<iTunesPlaylist> parsedPlaylists = new List<iTunesPlaylist>();
            _iTunesLibraryService.ParseLibrary(itunesLibraryContent, out parsedTracks, out parsedPlaylists);

            if (parsedTracks != null && parsedTracks.Any())
            {
                iTunesArtists.Clear();

                //Only add those tracks that actually have a given location
                //TODO: Or show in list greyed out with indication iCloud?
                _iTunesTrackList = parsedTracks.Where(item => !string.IsNullOrEmpty(item.Location)).ToList();

                var artists = (from track in _iTunesTrackList
                               where !string.IsNullOrEmpty(track.AlbumArtist)
                               orderby track.AlbumArtist
                               select track.AlbumArtist.ToLowerInvariant().ToTitleCase()).Distinct().ToList();

                foreach (string artist in artists)
                    iTunesArtists.Add(new TreeViewArtist() { Name = artist });
            }            

            if (parsedPlaylists != null && parsedPlaylists.Any())
            {
                _localPlayLists.Clear();
                iTunesPlaylists.Clear();

                var rootPlaylists = parsedPlaylists.Where(item => item.Parent == null);
                foreach (iTunesPlaylist playList in rootPlaylists)
                    _localPlayLists.Add(new TreeViewPlaylist() { Name = playList.Name, Id = playList.Id, PlaylistPersistentID = playList.PlaylistPersistentID });

                foreach (iTunesPlaylist playlist in rootPlaylists.Where(item => item.Children != null))
                    AddChildren(playlist);

                foreach (TreeViewPlaylist treeViewPlaylist in _localPlayLists)
                    iTunesPlaylists.Add(treeViewPlaylist);
            }
        }

        private void AddChildren(iTunesPlaylist playlist)
        {
            foreach (iTunesPlaylist child in playlist.Children)
            {
                var treeViewPlaylist = _localPlayLists.FirstOrDefault(item => item.PlaylistPersistentID == playlist.PlaylistPersistentID);
                treeViewPlaylist?.Children.Add(new TreeViewPlaylist() { Name = child.Name, Id = child.Id, PlaylistPersistentID = child.PlaylistPersistentID });

                AddChildren(child);
            }
        }

        private async Task ExportLibrary()
        {
            switch (SelectedTab)
            {
                case 0:
                    break;
                case 1:
                    List<TreeViewArtist> selectedArtists = iTunesArtists.Where(item => item.IsSelected).ToList();

                    var t = from iTunesTrack track in _iTunesTrackList
                            where selectedArtists.Exists(artist => artist.Name.Equals(track.AlbumArtist, StringComparison.OrdinalIgnoreCase))
                            select track;
                    //List < iTunesTrack > tracksToExport = (from iTunesTrack track in this._iTunesLibrary.iTunesTracks
                    //                                       where this._iTunesAlbumArtistsToExport.Exists(delegate (string argument)
                    //                                       {
                    //                                           return argument.Equals(track.AlbumArtist, StringComparison.OrdinalIgnoreCase);
                    //                                       })
                    //                                       select track).Distinct().ToList();
                    break;
            }
        }

        //TODO: Select / Deselect ALL option!

    }
}
