using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using iExporter.wpf.Extensions;
using iExporter.wpf.Models;
using iExporter.wpf.Services.Interfaces;

namespace iExporter.wpf.Services
{
    public class iTunesLibraryService : IiTunesLibraryService
    {
        private XDocument _itunesLibraryXDocument;
        private List<string> _defaultiTunesPlayLists = new List<string> { Constants.PLAYLIST_AUDIOBOOKS,
                                                                          Constants.PLAYLIST_GENIUS,
                                                                          Constants.PLAYLIST_LIBRARY,
                                                                          Constants.PLAYLIST_MOVIES,
                                                                          Constants.PLAYLIST_MUSIC,
                                                                          Constants.PLAYLIST_TVSHOWS };
        private List<iTunesTrack> _iTunesTracks;
        private string _iTunesLibraryLocation;

        /// <summary>
        /// Parse the given iTunes Library file
        /// It will go through all the available playlists and tracks
        /// </summary>
        /// <param name="iTunesLibraryContent"></param>
        /// <param name="iTunesTracks"></param>
        /// <param name="iTunesPlaylists"></param>
        public void ParseLibrary(string iTunesLibraryContent, out List<iTunesTrack> iTunesTracks, out List<iTunesPlaylist> iTunesPlaylists)
        {
            //Precondition checks
            if (string.IsNullOrEmpty(iTunesLibraryContent))
                throw new NullReferenceException("Please provide valid iTunes library content");

            _itunesLibraryXDocument = XDocument.Parse(iTunesLibraryContent);

            string iTunesLibraryLocation = (from iTunesLibNode in _itunesLibraryXDocument.Descendants("plist").Elements("dict")
                                            from key in iTunesLibNode.Descendants("key")
                                            where key.Value.Replace(" ", "").Equals("MusicFolder")
                                            select ((XElement)key.NextNode).Value).FirstOrDefault();

            _iTunesLibraryLocation = HttpUtility.UrlDecode(iTunesLibraryLocation.Replace(Constants.URI_LOCALHOST, string.Empty)).Replace('/', Path.DirectorySeparatorChar);

            //TODO: perform parsing in multiple threads
            iTunesTracks = InitiTunesTracks();
            iTunesPlaylists = InitiTunesPlaylists();
        }

        public void Export(List<iTunesPlaylist> playlists)
        {
            //TODO: Clear destination? Configuration option?
            //TODO: Sync with destination? Configuration option? ( means that we will be adding files not found and deleting files not in playlist )

            List<iTunesTrack> tracksToExport = new List<iTunesTrack>();

            foreach (iTunesPlaylist playlist in playlists)
                tracksToExport.AddRange(playlist.iTunesTracks);

            ExportTracks(tracksToExport);
        }

        /// <summary>
        /// Get all available tracks from the given library
        /// </summary>
        private List<iTunesTrack> InitiTunesTracks()
        {
            XElement trackDictionary = new XElement("Tracks", from track in _itunesLibraryXDocument.Descendants("plist").Elements("dict").Elements("dict").Elements("dict")
                                                              select new XElement("track",
                                                                from key in track.Descendants("key")
                                                                select new XElement(((string)key).Replace(" ", ""), (string)(XElement)key.NextNode)));

            _iTunesTracks = (trackDictionary.Nodes().Select(track => new iTunesTrack()
            {
                Id = ((XElement)track).Element("TrackID").ToInt(0),
                Album = ((XElement)track).Element("Album").ToString(string.Empty),
                Artist = ((XElement)track).Element("Artist").ToString(string.Empty),
                AlbumArtist = ((XElement)track).Element("AlbumArtist").ToString(string.Empty),
                BitRate = ((XElement)track).Element("BitRate").ToInt(0),
                Comments = ((XElement)track).Element("Comments").ToString(string.Empty),
                Composer = ((XElement)track).Element("Composer").ToString(string.Empty),
                Genre = ((XElement)track).Element("Genre").ToString(string.Empty),
                Kind = ((XElement)track).Element("Kind").ToString(string.Empty),
                //To be able to have a + signs in the track location, we need to convert it to the correct HTML code before we use the UrlDecode
                Location = WebUtility.UrlDecode(((XElement)track).Element("Location").ToString(string.Empty).Replace(Constants.URI_LOCALHOST, string.Empty).Replace("+", "%2B")).Replace('/', Path.DirectorySeparatorChar),
                Name = ((XElement)track).Element("Name").ToString(string.Empty),
                PlayCount = ((XElement)track).Element("PlayCount").ToInt(0),
                SampleRate = ((XElement)track).Element("SampleRate").ToInt(0),
                Size = ((XElement)track).Element("Size").ToInt64(0),
                //Get the track time in total seconds
                TotalTime = ((XElement)track).Element("TotalTime").ToInt64(0) / 1000,
                TrackNumber = ((XElement)track).Element("TrackNumber").ToInt(0)
            })).ToList();

            return _iTunesTracks;
        }

        /// <summary>
        /// Get all available playlists from the given library
        /// </summary>
        /// <returns></returns>
        private List<iTunesPlaylist> InitiTunesPlaylists()
        {
            //Get all the available playlists, but don't add the PlaylistItems Node
            XElement playListDictionary = new XElement("PlayLists", from iTunesPlayList in _itunesLibraryXDocument.Descendants("plist").Elements("dict").Elements("array").Elements("dict")
                                                                    select new XElement("playList",
                                                                      from key in iTunesPlayList.Descendants("key")
                                                                      where key.Value.Replace(" ", "") != "PlaylistItems" // && key.Value.Replace(" ","") != "TrackID"
                                                                      select new XElement(key.Value.Replace(" ", ""), ((XElement)key.NextNode).Value)));

            //From all available playlists, get a IEnumerable list that only contains user playlists and at least one track!
            //Load the corresponding tracks from the IEnumerable track list
            List<iTunesPlaylist> iTunesPlaylists = (from XElement playList in playListDictionary.Nodes()
                                                    let playListTracks = from trackField in playList.Elements("TrackID")
                                                                         select trackField
                                                                         where !this._defaultiTunesPlayLists.Contains(playList.Element("Name").Value) && playListTracks.Any()
                                    select new iTunesPlaylist()
                                    {
                                        Id = playList.Element("PlaylistID").ToString(string.Empty),
                                        PlaylistPersistentID = playList.Element("PlaylistPersistentID").ToString(string.Empty),
                                        ParentPersistentID = playList.Element("ParentPersistentID").ToString(string.Empty),
                                        Name = playList.Element("Name").ToString(string.Empty),
                                        iTunesTracks = from iTunesTrack track in _iTunesTracks
                                                       where (from trackID in playList.Elements("TrackID")
                                                              select trackID.Value).Contains(track.Id.ToString())
                                                       select track
                                    }).ToList();

            //Add playlist relationships
            foreach (iTunesPlaylist playList in iTunesPlaylists)
            {
                //Set the parent of each playlist
                if (!string.IsNullOrEmpty(playList.ParentPersistentID))
                {
                    playList.Parent = iTunesPlaylists.FirstOrDefault(item => !string.IsNullOrEmpty(item.PlaylistPersistentID) && item.PlaylistPersistentID.Equals(playList.ParentPersistentID, StringComparison.OrdinalIgnoreCase));
                    //Set the children playlists
                    if (!ReferenceEquals(playList.Parent, null))
                    {
                        if (ReferenceEquals(playList.Parent.Children, null))
                            playList.Parent.Children = new List<iTunesPlaylist>();

                        playList.Parent.Children.Add(playList);
                    }
                }
            }

            return iTunesPlaylists;
        }

        private void ExportTracks(List<iTunesTrack> tracks)
        {
            foreach (iTunesTrack track in tracks.Where(item => !string.IsNullOrEmpty(item.Location)).Distinct())
            {
                var t = track.Location;
            }
        }
    }
}
