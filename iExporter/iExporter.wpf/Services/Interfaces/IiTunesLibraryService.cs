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
        /// <returns></returns>
        List<iTunesTrack> ParseLibrary(string iTunesLibraryContent);
    }
}
