using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Models
{
    public class MediaAddedEvent : EventArgs
    {
        public string MediaPath { get; set; }
    }
}
