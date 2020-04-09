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
        public MainWindow()
        {
            InitializeComponent();
            playList.Added += new Playlist.MediaAdded(OnMediaAdded);
            MediaPlayer.Finished += MediaPlayer_Finished;
            jumplist = new JumpList();
            CreateJumpTask();
            timer.Tick += Timer_Tick;
            taskBarItemInfo1 = new TaskbarItemInfo();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(MediaPlayer.Duration.HasTimeSpan)
            {
                if (counter < MediaPlayer.Duration.TimeSpan.TotalSeconds)
                {
                    this.taskBarItemInfo1.ProgressState = TaskbarItemProgressState.Normal;
                    taskBarItemInfo1.ProgressValue += counter;
                    counter++;
                    //if(MediaPlayer.FileName.EndsWith(".mp3"))
                    //{
                    //    TskBarProgress.Overlay = new BitmapImage(new Uri(@"pack://application:,,,/Dummy;component/Resources/m.png", UriKind.Absolute));
                    //}
                    //else
                    //{
                    //    TskBarProgress.Overlay = new BitmapImage(new Uri(@"pack://application:,,,/Dummy;component/Resources/v.png", UriKind.Absolute));
                    //}
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
            bool noNext = playList.Next();
            if(!noNext)
            {
                this.Title = "XPlayer - " + System.IO.Path.GetFileName(playList.CurrSongName);
                MediaPlayer.FileName = playList.CurrSongName;
                MediaPlayer.PlayMedia();
                timer.Stop();
                timer.Start();
                isStarted = true;
                playList.ClearAll();
            }
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

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            this.MediaPlayer.PlayMedia();
            this.taskBarItemInfo1.Overlay = (DrawingImage)this.FindResource("StopImage");
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MediaPlayer.PlayMedia();
            this.taskBarItemInfo1.Overlay = (DrawingImage)this.FindResource("PlayImage");
        }

        private void ThumbButtonInfo_Click(object sender, EventArgs e)
        {
            this.MediaPlayer.PlayMedia();
            this.taskBarItemInfo1.Overlay = (DrawingImage)this.FindResource("PlayImage");
        }

        private void ThumbButtonInfo_Click_1(object sender, EventArgs e)
        {
            this.MediaPlayer.PlayMedia();
            this.taskBarItemInfo1.Overlay = (DrawingImage)this.FindResource("StopImage");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
