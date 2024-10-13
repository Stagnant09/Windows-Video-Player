using Microsoft.Win32; 
using System;          
using System.Windows;
using System.Windows.Threading;

namespace VideoPlayerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int time_;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        // Play button click handler
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                mediaElement.Play();
                timer.Start();  // Start the timer to update the slider
            }
        }

        // Pause button click handler
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        // Stop button click handler
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            timer.Stop();  // Stop the timer when video stops
            videoSlider.Value = 0;
        }

        // File selection handler
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.wmv;*.mkv";  // Set filter for video files
            if (openFileDialog.ShowDialog() == true)
            {
                mediaElement.Source = new Uri(openFileDialog.FileName);
                /*Change the dimensions of the window based on the height/width of the video to be displayed*/
                //double windowWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                //double windowHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                //double videoWidth = mediaElement.NaturalVideoWidth;
                //double videoHeight = mediaElement.NaturalVideoHeight;
                //double scaleFactor = Math.Min(windowWidth / videoWidth, windowHeight / videoHeight);
                //this.Width = videoWidth;
                //this.Height = videoHeight;
                mediaElement.Play();
                timer.Start();
            }
        }

        // Update slider as video plays
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                videoSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                videoSlider.Value = mediaElement.Position.TotalSeconds;
                /*Convert videoSlider value to time in the form HH:MM:SS*/
                time_ = (int)videoSlider.Value;
                int hours = time_ / 3600;
                int minutes = (time_ % 3600) / 60;
                int seconds = time_ % 60;
                TimeLabel.Content = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
        }

        // Slider value changed handler (seek functionality)
        private void VideoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement.Source != null && mediaElement.NaturalDuration.HasTimeSpan)
            {
                mediaElement.Position = TimeSpan.FromSeconds(videoSlider.Value);
                time_ = (int)videoSlider.Value;
                int hours = time_ / 3600;
                int minutes = (time_ % 3600) / 60;
                int seconds = time_ % 60;
                TimeLabel.Content = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
        }

        private void Plus_15_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(videoSlider.Value + 15);
            videoSlider.Value = videoSlider.Value + 15;
            if (videoSlider.Value > videoSlider.Maximum)
            {
                videoSlider.Value = videoSlider.Maximum;
            }
            else
            {
                mediaElement.Play();
            }
            time_ = (int)videoSlider.Value;
            int hours = time_ / 3600;
            int minutes = (time_ % 3600) / 60;
            int seconds = time_ % 60;
            TimeLabel.Content = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }

        private void Minus_15_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(videoSlider.Value - 15);
            videoSlider.Value = videoSlider.Value - 15;
            if (videoSlider.Value < 0)
            {
                videoSlider.Value = 0;              
            }
            mediaElement.Play();
            time_ = (int)videoSlider.Value;
            int hours = time_ / 3600;
            int minutes = (time_ % 3600) / 60;
            int seconds = time_ % 60;
            TimeLabel.Content = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement != null && volumeLabel != null)
            {
                // Set the volume of the media according to the value of the slider
                mediaElement.Volume = VolumeSlider.Value / 200;
                // Update the label content to reflect the current slider value
                volumeLabel.Content = string.Format("{0:P0}", VolumeSlider.Value / 200);
            }
        }

    }
}
