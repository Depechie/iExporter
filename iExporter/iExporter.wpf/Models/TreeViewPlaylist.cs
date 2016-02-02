using System.Collections.Generic;

namespace iExporter.wpf.Models
{
    public class TreeViewPlaylist : TreeViewItem
    {
        public string Id { get; set; }

        public string PlaylistPersistentID { get; set; }

        public List<TreeViewPlaylist> Children { get; set; } = new List<TreeViewPlaylist>();
    }
}