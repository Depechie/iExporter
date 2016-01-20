using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using iExporter.wpf.Extensions;
using iExporter.wpf.Models;
using iExporter.wpf.Services.Interfaces;

namespace iExporter.wpf.Services
{
    public class iTunesLibraryService : IiTunesLibraryService
    {
        private XDocument _itunesLibraryXDocument;

        /// <summary>
        /// Parse the given iTunes Library file
        /// It will go through all the available playlists and tracks
        /// </summary>
        /// <param name="iTunesLibraryContent"></param>
        public List<iTunesTrack> ParseLibrary(string iTunesLibraryContent)
        {
            //Precondition checks
            if (string.IsNullOrEmpty(iTunesLibraryContent))
                throw new NullReferenceException("Please provide valid iTunes library content");

            _itunesLibraryXDocument = XDocument.Parse(iTunesLibraryContent);

            //TODO: perform parsing in multiple threads
            return InitiTunesTracks();
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

            return (trackDictionary.Nodes().Select(track => new iTunesTrack()
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
        }
    }
}
