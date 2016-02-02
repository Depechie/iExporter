using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iExporter.wpf.Models
{
    public class iTunesPlaylist : BaseClass
    {
        private string _invalidFileChars = new string(Path.GetInvalidFileNameChars());
        private string _invalidPathChars = new string(Path.GetInvalidPathChars());

        private string _id;
        private string _playlistPersistentId;
        private string _parentPersistentId;
        private string _name;
        private IEnumerable<iTunesTrack> _iTunesTracks;
        private iTunesPlaylist _parent;
        private List<iTunesPlaylist> _children = new List<iTunesPlaylist>();

        public string Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistPersistentID
        {
            get { return _playlistPersistentId; }
            set
            {
                if (value == _playlistPersistentId) return;
                _playlistPersistentId = value;
                OnPropertyChanged();
            }
        }

        public string ParentPersistentID
        {
            get { return _parentPersistentId; }
            set
            {
                if (value == _parentPersistentId) return;
                _parentPersistentId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValidPlaylistName));
            }
        }

        public IEnumerable<iTunesTrack> iTunesTracks
        {
            get { return _iTunesTracks; }
            set
            {
                if (Equals(value, _iTunesTracks)) return;
                _iTunesTracks = value;
                OnPropertyChanged();
            }
        }

        public iTunesPlaylist Parent
        {
            get { return _parent; }
            set
            {
                if (Equals(value, _parent)) return;
                _parent = value;
                OnPropertyChanged();
            }
        }

        public List<iTunesPlaylist> Children
        {
            get { return _children; }
            set
            {
                if (Equals(value, _children)) return;
                _children = value;
                OnPropertyChanged();
            }
        }

        public string ValidPlaylistName
        {
            get
            {
                return new string((from char c in this.Name
                                   select (this._invalidFileChars.Contains(c) ? '-' : c)).ToArray());
            }
        }
    }
}
