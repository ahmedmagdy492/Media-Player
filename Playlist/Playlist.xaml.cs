using Microsoft.Win32;
using Playlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Playlist
{
    public delegate void MediaAdded(MediaAddedEvent e);    
    /// <summary>
    /// MediaPlayer PlayList
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private readonly List<string> Songs;
        private int currentSong;
        public event MediaAdded Added;

        public string CurrSongName { get; set; }
        public UserControl1()
        {
            InitializeComponent();
            currentSong = 0;
            Songs = new List<string>();
            Added += delegate{};
        }

        private MLabel CreateMLabel(string content)
        {            
            MLabel lbl = new MLabel
            {
                Content = System.IO.Path.GetFileName(content),
                Value = content,
                Foreground = Brushes.Black,
                FontSize = 18,
                Width = 200,
                ToolTip = new ToolTip
                {
                    Content = System.IO.Path.GetFileName(content)
                }
            };
            lbl.MouseDoubleClick += Lbl_MouseDoubleClick;
            return lbl;
        }

        private void Lbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MLabel thisLbl = (MLabel)sender;
            thisLbl.Foreground = Brushes.Blue;
            CurrSongName = thisLbl.Value;
            currentSong = Songs.IndexOf(CurrSongName);
            ColorAllBack(currentSong);
            // calling the added event
            Added(new MediaAddedEvent { MediaPath = thisLbl.Value });
        }

        private void ColorAllBack(int except)
        {
            for (int i = 0; i < PlayList.Children.Count; i++)
            {
                if(i != except)
                {
                    MLabel MLabel = ((MLabel)PlayList.Children[i]);
                    MLabel.Foreground = Brushes.Black;
                }
            }
        }

        public bool Next()
        {
            bool noNext = true;
            if(currentSong < (Songs.Count - 1))
            {
                currentSong++;
                CurrSongName = Songs[currentSong];
                var lbl = (MLabel)PlayList.Children[currentSong];
                lbl.Foreground = Brushes.Blue;
                ColorAllBack(currentSong);
                noNext = false;
            }
            return noNext;
        }

        public void Prev()
        {           
            if(currentSong > 0)
            {
                currentSong--;
                CurrSongName = Songs[currentSong];
            }
        }        

        // Adding new Item
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog
            {
                Filter = "Songs, Videos|*.mp3;*.mp4;*.wav;*.avi",
                //Multiselect = true,
                ValidateNames = true
            };
            
            if(opf.ShowDialog() == true)
            {
                if (opf.FileNames.Count() > 1)
                {
                    //Songs.AddRange(opf.FileNames);
                    //foreach (var song in Songs)
                    //{
                    //    MLabel lbl = CreateMLabel(song);
                    //    lbl.Foreground = Brushes.Blue;
                    //    PlayList.Children.Add(lbl);
                    //    /// tososs
                    //}
                }
                else
                {
                    CurrSongName = opf.FileName;
                    Songs.Add(CurrSongName);
                    currentSong = Songs.Count - 1;

                    // adding MLabel
                    MLabel lbl = CreateMLabel(CurrSongName);
                    lbl.Foreground = Brushes.Blue;
                    PlayList.Children.Add(lbl);

                    // calling the added event
                    Added(new MediaAddedEvent { MediaPath = lbl.Value });
                }
                ColorAllBack(currentSong);
                
            }
        }

        // clear all songs

        public void ClearAll()
        {
            PlayList.Children.Clear();
            Songs.Clear();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Songs.Count; i++)
            {
                if (Songs[i] != CurrSongName)
                {
                    Songs.Remove(Songs[i]);
                    var ele = PlayList.Children[i];
                    PlayList.Children.Remove(ele);
                }
            }
            CurrSongName = "";
            currentSong = 0;
        }
    }
}
