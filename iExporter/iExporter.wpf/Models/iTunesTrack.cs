using System;
using System.IO;

namespace iExporter.wpf.Models
{
    public class iTunesTrack : BaseClass
    {
        private int _id;
        private string _album;
        private string _artist;
        private string _albumArtist;
        private int _bitRate;
        private string _comments;
        private string _composer;
        private string _genre;
        private string _kind;
        private string _location;
        private string _name;
        private int _playCount;
        private int _sampleRate;
        private Int64 _size;
        private Int64 _totalTime;
        private int _trackNumber;

        public string Album
        {
            get { return _album; }
            set
            {
                if (value == _album) return;
                _album = value;
                OnPropertyChanged();
            }
        }

        public string AlbumArtist
        {
            get { return _albumArtist; }
            set
            {
                if (value == _albumArtist) return;
                _albumArtist = value;
                OnPropertyChanged();
            }
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                if (value == _artist) return;
                _artist = value;
                OnPropertyChanged();
            }
        }

        public int BitRate
        {
            get { return _bitRate; }
            set
            {
                if (value == _bitRate) return;
                _bitRate = value;
                OnPropertyChanged();
            }
        }

        public string Comments
        {
            get { return _comments; }
            set
            {
                if (value == _comments) return;
                _comments = value;
                OnPropertyChanged();
            }
        }

        public string Composer
        {
            get { return _composer; }
            set
            {
                if (value == _composer) return;
                _composer = value;
                OnPropertyChanged();
            }
        }

        public string Genre
        {
            get { return _genre; }
            set
            {
                if (value == _genre) return;
                _genre = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Kind
        {
            get { return _kind; }
            set
            {
                if (value == _kind) return;
                _kind = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (value == _location) return;
                _location = value;
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
            }
        }

        public int PlayCount
        {
            get { return _playCount; }
            set
            {
                if (value == _playCount) return;
                _playCount = value;
                OnPropertyChanged();
            }
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (value == _sampleRate) return;
                _sampleRate = value;
                OnPropertyChanged();
            }
        }

        public long Size
        {
            get { return _size; }
            set
            {
                if (value == _size) return;
                _size = value;
                OnPropertyChanged();
            }
        }

        public long TotalTime
        {
            get { return _totalTime; }
            set
            {
                if (value == _totalTime) return;
                _totalTime = value;
                OnPropertyChanged();
            }
        }

        public int TrackNumber
        {
            get { return _trackNumber; }
            set
            {
                if (value == _trackNumber) return;
                _trackNumber = value;
                OnPropertyChanged();
            }
        }

        public string FileName
        {
            get
            {
                int index = this.Location.LastIndexOf(Path.DirectorySeparatorChar);
                if (index == -1)
                    return this.Location;
                else
                    return this.Location.Substring(index + 1);
            }
        }
    }
}
