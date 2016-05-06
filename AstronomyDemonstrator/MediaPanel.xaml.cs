using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace AstronomyDemonstrator
{
    /// <summary>
    /// MediaPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MediaPanel : Window
    {
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetMaxVolume();
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetMinVolume();
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetVolume();
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool SetVolume(int volumeValue);
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool IsMute();
        [DllImport("SystemVolumeControl.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool SetMute(bool bMute);



        private bool isPlay = false;
        public bool IsPlay
        {
            get 
            {
                return isPlay;
            }
        }
        public MediaPanel()
        {
            InitializeComponent();
            mediaPlayer.LoadedBehavior = MediaState.Manual;
            mediaPlayer.UnloadedBehavior = MediaState.Stop;
        }
        public void PlayMovie(string moviePath)
        {
            mediaPlayer.Source = new Uri(moviePath,UriKind.Relative);
            mediaPlayer.Play();
            isPlay = true;
        }
        public void Play()
        {
            mediaPlayer.Play();
            isPlay = true;
        }
        public void Pause()
        {
            mediaPlayer.Pause();
            isPlay = false;
        }
        public void Stop()
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Close();
            isPlay = false;
        }
        public double GetMoviePosition()
        {
            TimeSpan timeSpan = mediaPlayer.Position;
            return timeSpan.TotalSeconds;
        }
        public double GetMovieTotalTime()
        {
            if (mediaPlayer.HasVideo && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                return mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
            else
            {
                return 0;
            }
        }
        public void SetMoviePosition(double positonValue)
        {
            if (mediaPlayer.HasVideo && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                if (positonValue<mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    mediaPlayer.Position = TimeSpan.FromSeconds(positonValue);
                } 
                else
                {
                    mediaPlayer.Position = TimeSpan.FromSeconds(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds-0.5);
                }
                
            }
        }
        public void Mute()
        {
            SetMute(true);
        }
        public void NoMute()
        {
            SetMute(false);
        }
        public void VolumeAdd()
        {
            if (GetVolume()<GetMaxVolume())
            {
                SetVolume(GetVolume() + 5);
            }
        }
        public void VolumeDecrease()
        {
            if (GetVolume()>GetMinVolume())
            {
                SetVolume(GetVolume() - 5);
            }
        }
        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private void mediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            try
            {
                mediaPlayer.Close();
                mediaPlayer.Play();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("视频错误");
            }

        }
    }
}
