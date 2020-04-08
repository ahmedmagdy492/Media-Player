using MediaPlayer.Models;
using Microsoft.Win32;
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
using System.Windows.Threading;

namespace MediaPlayer
{
    public delegate void MediaFinished();

    /// <summary>
    /// Media Player Element
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public event MediaFinished Finished;
        private MediaElement mediaElement;
        private bool isPlaying;
        private DispatcherTimer timer;
        private Time countUpTimer;
        private Time countDownTimer;
        private int elapsedTime;
        private int secCounter = 0;
        private int totalSecs;
        private int remainingTime;        
        private bool isPaused;
        private bool isStoppedPressed;

        public string FileName { get; set; }

        public Duration Duration { 
            get {
                return mediaElement.NaturalDuration;
            } 
        }
        public UserControl1()
        {
            InitializeComponent();
            mediaElement = new MediaElement();            
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Close;
            mediaElement.MediaOpened += MediaElement_MediaOpened;
            soundSlider.Value = mediaElement.Volume;
            isPlaying = false;            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            elapsedTime = 0;
            timer.Tick += Timer_Tick;
            videoPreview.HorizontalAlignment = HorizontalAlignment.Center;
            videoPreview.VerticalAlignment = VerticalAlignment.Center;
            Finished += delegate{};
        }        

        private int ConvertToSecs(Time mTime)
        {
            mTime.Seconds += (mTime.Mintues * 60) + (mTime.Hours * 60 * 60);
            return mTime.Seconds;
        }                

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(remainingTime > 0)
            {
                remainingTime--;
                countDownTimer.CountDown();
                RemaindTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
            }

            if (secCounter < totalSecs)
            {
                countUpTimer.TimeFunction();
                ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
                secCounter++;
                MediaProgress.Value = secCounter;
            }
            else
            {
                timer.Stop();
                isPlaying = false;
                playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/play.png", UriKind.Absolute));
                secCounter = 0;
                MediaProgress.Value = secCounter;
                ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
                Finished();
            }
            #region start commented code
            //if(remainingTime > 0)
            //{
            //    remainingTime--;
            //    CountDown();
            //}            

            //if (secCounter <= totalSecs)
            //{
            //    MediaProgress.Value = elapsedTime;                
            //    secCounter++;
            //    elapsedTime++;                                
            //    ChangeMediaTimer();                
            //}
            //else
            //{
            //    timer.Stop();
            //    elapsedTime = 0;
            //    remainingTime = totalSecs;
            //    playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/play.png", UriKind.Absolute));
            //    // stop the media
            //    MediaProgress.Value = 0;
            //}
            //ElapsedTime.Content = $"{mediaTime.Hours}:{mediaTime.Mintues}:{mediaTime.Seconds}";
            //RemaindTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
            #endregion
        }

        public UserControl1(string fileName) : base()
        {
            this.FileName = fileName;
        }
        
        private void PlayOrPause()
        {
            if (isPlaying == false)
            {
                // play the media
                mediaElement.Play();
                // change the image to pause
                playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/pause.png", UriKind.Absolute));
                // do all of the heavy working
                isPlaying = true;                
                // getting the total secs of the media
                if(isStoppedPressed)
                {
                    timer.Start();

                    isStoppedPressed = false;
                }
                if(isPaused)
                {
                    timer.Start();
                    isPaused = false;
                    isPlaying = false;
                }                
            }
            else
            {
                mediaElement.Pause();
                playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/play.png", UriKind.Absolute));
                // do all of the heavy working
                isPlaying = false;
                /// stop the timer temperarailly
                timer.Stop();
                isPaused = true;
            }
        }

        private void SetButtonsPostion()
        {
            btnForward.Width = btnPlay.Width = btnStop.Width = btnRewind.Width = 70;
            btnRewind.Margin = new Thickness(btnRewind.Margin.Left, btnRewind.Margin.Top, PlayBar.Width - this.btnRewind.Width, btnRewind.Margin.Bottom);
            btnPlay.Margin = new Thickness(btnPlay.Margin.Left, btnPlay.Margin.Top, PlayBar.Width - this.btnPlay.Width - this.btnRewind.Width, btnPlay.Margin.Bottom);
            btnForward.Margin = new Thickness(btnForward.Margin.Left, btnForward.Margin.Top, PlayBar.Width - this.btnForward.Width - btnPlay.Width - btnRewind.Width, btnForward.Margin.Bottom);
            btnStop.Margin = new Thickness(btnStop.Margin.Left, btnStop.Margin.Top, PlayBar.Width - this.btnStop.Width - btnRewind.Width - btnPlay.Width - btnForward.Width, btnStop.Margin.Bottom);            
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            soundSlider.Margin = new Thickness(this.Width - soundSlider.Width - 30, soundSlider.Margin.Top, soundSlider.Margin.Right, soundSlider.Margin.Bottom);
            soundValue.Width = 25;
            soundValue.Margin = new Thickness(this.Width - soundValue.Width - 40, soundValue.Margin.Top, soundValue.Margin.Right, soundValue.Margin.Bottom);            
            PlayBar.Width = this.Width - 20;
            PlayBar.Margin = new Thickness(PlayBar.Margin.Left, this.Height - PlayBar.Height, PlayBar.Margin.Right, 10);
            MediaProgress.Width = this.Width - 20;
            MediaProgress.Margin = new Thickness(MediaProgress.Margin.Left, MediaProgress.Margin.Top, 10, MediaProgress.Margin.Bottom);
            RemaindTime.Margin = new Thickness(PlayBar.Width - RemaindTime.Width - 20, RemaindTime.Margin.Top, 1, RemaindTime.Margin.Bottom);
            SetButtonsPostion();
            videoPreview.Width = this.Width;
            videoPreview.Height = this.Height;
        }

        public void PlayMedia()
        {            
            if(!string.IsNullOrEmpty(this.FileName))
            {
                mediaElement.LoadedBehavior = MediaState.Manual;
                mediaElement.Source = new Uri(this.FileName);                
                /// playing the media
                ///               
                countUpTimer = new Time();
                countDownTimer = new Time();                
                
                mediaElement.Width = this.Width;
                mediaElement.Height = this.Height;                

                Label noPreviewLabel = new Label
                {
                    Content = "No Preview",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontSize = 20
                };

                if (!videoPreview.Children.Contains(mediaElement))
                    videoPreview.Children.Add(mediaElement);

                if (this.FileName.EndsWith(".mp3"))
                {
                    videoPreview.Children.Add(noPreviewLabel);
                }  
                else
                {
                    if (videoPreview.Children.Count == 2)
                        videoPreview.Children.RemoveAt(videoPreview.Children.Count - 1);
                }

                /// 2- setting the volume value to the slider value
                /// 
                soundSlider.Value = mediaElement.Volume;
                soundValue.Content = soundSlider.Value;

                //// TODO: do all the required work after playing                                
                PlayOrPause();                
            }
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            totalSecs = (int)mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            remainingTime = totalSecs;
            countDownTimer.Seconds = mediaElement.NaturalDuration.TimeSpan.Seconds;
            countDownTimer.Mintues = mediaElement.NaturalDuration.TimeSpan.Minutes;
            countDownTimer.Hours = mediaElement.NaturalDuration.TimeSpan.Hours;
            ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
            RemaindTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
            if (isPaused)
            {
                isPaused = false;
            }
            elapsedTime = 0;
            secCounter = 0;
            MediaProgress.Value = secCounter;
            MediaProgress.Maximum = totalSecs;
            remainingTime = totalSecs;
            if(isPlaying)
            {
                isPlaying = false;
            }
            timer.Start();
            Label noPreviewLabel = new Label
            {
                Content = "No Preview",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.White,
                FontSize = 20
            };
            if (this.FileName.EndsWith(".mp3"))
            {
                videoPreview.Children.Add(noPreviewLabel);
            }
            else
            {
                if (videoPreview.Children.Count == 2)
                    videoPreview.Children.RemoveAt(videoPreview.Children.Count - 1);
            }
        }

        private void soundSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = soundSlider.Value;
            soundValue.Content = soundSlider.Value;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            // checking whether a media is loaded or not
            if(mediaElement.Source != null)
            {
                PlayOrPause();
                PlayBar.Visibility = Visibility.Collapsed;
            }            
        }

        private void StopTimer()
        {
            timer.Stop();
            secCounter = 0;
            countUpTimer = new Time();
            countDownTimer = new Time();
            ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
            if(mediaElement.NaturalDuration.HasTimeSpan)
            {
                countDownTimer.Seconds = mediaElement.NaturalDuration.TimeSpan.Seconds;
                countDownTimer.Mintues = mediaElement.NaturalDuration.TimeSpan.Minutes;
                countDownTimer.Hours = mediaElement.NaturalDuration.TimeSpan.Hours;
            }
            RemaindTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
            MediaProgress.Value = 0;
            remainingTime = totalSecs;
            elapsedTime = 0;
            Finished();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(mediaElement.Source != null)
            {
                if(isStoppedPressed == false)
                {
                    mediaElement.Stop();
                    isPlaying = false;
                    playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/play.png", UriKind.Absolute));
                    // then stop the timer     
                    StopTimer();
                    // stop the progress bar
                    isStoppedPressed = true;
                }
            }
        }                

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(this).X <= PlayBar.Width && e.GetPosition(this).Y <= (this.Height -PlayBar.Height))
            {
                PlayBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                PlayBar.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MediaProgress.HorizontalAlignment = HorizontalAlignment.Center;            
            MediaProgress.Width = PlayBar.Width - 20;
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if(mediaElement.Source != null)
            {
                if(secCounter < totalSecs)
                {
                    secCounter += 5;
                    countUpTimer.Seconds += 5;
                    remainingTime -= 5;
                    countDownTimer.Seconds -= 5;
                    ElapsedTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
                    ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
                    MediaProgress.Value = secCounter;
                    mediaElement.Position = new TimeSpan(countUpTimer.Hours, countUpTimer.Mintues, countUpTimer.Seconds);
                }
                else
                {
                    // stop the media
                    timer.Stop();
                    secCounter = 0;
                    countUpTimer = new Time();
                    countDownTimer = new Time();
                    ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
                    RemaindTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
                    MediaProgress.Value = 0;
                    remainingTime = totalSecs;
                    elapsedTime = 0;
                    playImg.Source = new BitmapImage(new Uri(@"pack://application:,,,/MediaPlayer;component/Images/play.png", UriKind.Absolute));
                    Finished();
                }
            }
        }

        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                if (secCounter > 0)
                {
                    secCounter -= 5;
                    countUpTimer.Seconds -= 5;
                    remainingTime += 5;
                    countDownTimer.Seconds += 5;
                    ElapsedTime.Content = $"{countDownTimer.Hours}:{countDownTimer.Mintues}:{countDownTimer.Seconds}";
                    ElapsedTime.Content = $"{countUpTimer.Hours}:{countUpTimer.Mintues}:{countUpTimer.Seconds}";
                    MediaProgress.Value = secCounter;
                    mediaElement.Position = new TimeSpan(countUpTimer.Hours, countUpTimer.Mintues, countUpTimer.Seconds);
                }                
            }

        }
    }
}
