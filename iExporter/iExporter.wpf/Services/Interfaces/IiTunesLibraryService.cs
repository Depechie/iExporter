using System.Collections.Generic;
using System.Threading.Tasks;
using iExporter.wpf.Models;

namespace iExporter.wpf.Services.Interfaces
{
    public interface IiTunesLibraryService
    {
        /// <summary>
        /// Parse the given iTunes library
        /// It will go through all the available playlists and tracks
        /// </summary>
        /// <param name="iTunesLibraryContent"></param>
        /// <param name="iTunesTracks"></param>
        /// <param name="iTunesPlaylists"></param>
        void ParseLibrary(string iTunesLibraryContent, out List<iTunesTrack> iTunesTracks, out List<iTunesPlaylist> iTunesPlaylists);

        void Export(List<iTunesPlaylist> playlists);
    }
}
