using Microsoft.Win32;
using Playlist;
using Playlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shell;
using System.Windows.Threading;

namespace Dummy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JumpList jumplist;
        private bool isStarted = false;
        private DispatcherTimer timer = new DispatcherTimer();
        private int counter = 0;
        public System.Windows.Shell.TaskbarItemInfo TskBarProgress { get; set; }        
        public MainWindow()
        {
            InitializeComponent();
            playList.Added += new Playlist.MediaAdded(OnMediaAdded);
            MediaPlayer.Finished += MediaPlayer_Finished;
            jumplist = new JumpList();
            CreateJumpTask();
            timer.Tick += Timer_Tick;
            TskBarProgress = new TaskbarItemInfo();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(MediaPlayer.Duration.HasTimeSpan)
            {
                if (counter < MediaPlayer.Duration.TimeSpan.TotalSeconds)
                {
                    TskBarProgress.ProgressValue += counter;
                    counter++;
                    if(MediaPlayer.FileName.EndsWith(".mp3"))
                    {
                        TskBarProgress.Overlay = new BitmapImage(new Uri(@"pack://application:,,,/Dummy;component/Images/m.png", UriKind.Absolute));
                    }
                    else
                    {
                        TskBarProgress.Overlay = new BitmapImage(new Uri(@"pack://application:,,,/Dummy;component/Images/v.png", UriKind.Absolute));
                    }
                }
            }            
        }

        private void CreateJumpTask()
        {
            JumpTask task = new JumpTask();
            task.Arguments = MediaHistoryManager.GetMediaPath();
            task.Title = MediaHistoryManager.GetMediaPath();
            task.ApplicationPath = Application.ResourceAssembly.Location;
            task.IconResourcePath = MediaHistoryManager.GetMediaPath();            
            jumplist.JumpItems.Add(task);
            jumplist.Apply();
        }

        public void CustomPlay(string filepath)
        {
            this.Title = "XPlayer - " + System.IO.Path.GetFileName(playList.CurrSongName);
            MediaPlayer.FileName = filepath;
            MediaPlayer.PlayMedia();
            timer.Start();
            isStarted = true;            
        }

        private void MediaPlayer_Finished()
        {
            playList.Next();
            this.Title = "XPlayer - " + System.IO.Path.GetFileName(playList.CurrSongName);
            MediaPlayer.FileName = playList.CurrSongName;
            MediaPlayer.PlayMedia();
            timer.Stop();
            timer.Start();
            isStarted = true;
        }

        private void OnMediaAdded(MediaAddedEvent e)
        {
            this.Title = "XPlayer - " + System.IO.Path.GetFileName(e.MediaPath);
            MediaPlayer.FileName = e.MediaPath;
            MediaPlayer.PlayMedia();
            MediaHistoryManager.SaveMediaPath(MediaPlayer.FileName);
            if(!isStarted)
            {
                timer.Start();
                isStarted = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Width = this.Width - 237 - 15;
            MediaPlayer.Height = this.Height - 55;
            toolMenu.Width = this.Width;
            playList.Width = this.Width - MediaPlayer.Width;
            playList.Margin = new Thickness(MediaPlayer.Width, playList.Margin.Top, 0, playList.Margin.Bottom);
        }        

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {            
            MediaPlayer.Width = this.Width - 237 - 15;
            MediaPlayer.Height = this.Height - 55;
            toolMenu.Width = this.Width;
            playList.Width = this.Width - MediaPlayer.Width;
            playList.Margin = new Thickness(MediaPlayer.Width, playList.Margin.Top, 0, playList.Margin.Bottom);
        }        

        protected override void OnStateChanged(EventArgs e)
        {
            MediaPlayer.Width = this.Width - 237 - 15;
            MediaPlayer.Height = this.Height - 55;
            playList.Width = this.Width - MediaPlayer.Width;
            playList.Margin = new Thickness(MediaPlayer.Width, playList.Margin.Top, 0, playList.Margin.Bottom);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            /// on the client
            OpenFileDialog opf = new OpenFileDialog
            {
                Title = "Choose a Media File",
                Filter = "Songs, Videos|*.mp3;*.mp4;*.wav;*.avi"
            };
            bool? result = opf.ShowDialog();
            if(result == true)
            {
                this.Title = "XPlayer - " + System.IO.Path.GetFileName(opf.FileName);
                MediaPlayer.FileName = opf.FileName;
                MediaPlayer.PlayMedia();                
                if(!isStarted)
                {
                    timer.Start();
                    isStarted = true;
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        

        //private void TimeFunction()
        //{
        //    if(secs < 59)
        //    {
        //        secs++;
        //    }
        //    else
        //    {
        //        secs = 0;
        //        if(mins < 59)
        //        {
        //            mins++;
        //        }
        //        else
        //        {
        //            mins = 0;
        //            hours++;
        //        }
        //    }
        //}
    }
}
