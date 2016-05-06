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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;

namespace AstronomyDemonstrator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
#if VEE_ENCRYPT
        //[DllImport("EnFileReadH.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void InitEnFileHook(bool p_bVeeFile);
        //[DllImport("EnFileReadH.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void StopEnFileHook();

        [DllImport("VeeFileReadH.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static void InitVeeFileHook(bool p_bVeeFile);
        [DllImport("VeeFileReadH.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static void StopVeeFileHook();
#endif


        MediaPanel media;
        XmlSetting xmlSetting;
        List<string> resultPlayList = new List<string>();
        List<string> playListBoxFileNames = new List<string>();
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int mParam, int IParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        DispatcherTimer processTimer;
        public MainWindow()
        {
            InitializeComponent();
            xmlSetting = new XmlSetting();
            
#if VEE_ENCRYPT
            //InitEnFileHook(false);
            InitVeeFileHook(false);
#endif
        }

        private void mediaControlPanel_Loaded(object sender, RoutedEventArgs e)
        {
            media = new MediaPanel();
            InitListBox();
            SetMediaPanelPositon();
            InitTimer();
        }
        private void InitListBox()
        {
            string moviesPath = xmlSetting.GetValueByName("MoviesPath");
            UpdateListBox(moviesPath);
        }
        private void SetMediaPanelPositon()
        {
            if (System.Windows.Forms.Screen.AllScreens.Length == 2)
            {
                for (int i=0;i<=1;i++)
                {
                    if (System.Windows.Forms.Screen.AllScreens[i].Primary==false)
                    {
                        media.Left = System.Windows.Forms.Screen.AllScreens[i].Bounds.Left;
                        media.Top = System.Windows.Forms.Screen.AllScreens[i].Bounds.Top;
                        media.Width = System.Windows.Forms.Screen.AllScreens[i].Bounds.Width;
                        media.Height = System.Windows.Forms.Screen.AllScreens[i].Bounds.Height;
                        media.Show();
                    }
                    else
                    {
                        this.Left = 0;
                        this.Top = 0;
                    }
                }
            }
            else
            {
                media.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Left;
                media.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Top;
                media.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                media.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                media.Show();
            }
        }
        private void InitTimer()
        {
            processTimer = new DispatcherTimer();
            processTimer.Interval = TimeSpan.FromMilliseconds(1000);
            processTimer.Start();
            processTimer.Tick += new EventHandler(processTimer_Tick);
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (media.IsPlay)
            {
                media.Pause();
                Style MyButtonStylePause = this.Resources["MyButtonStylePause"] as Style;
                this.playButton.Style = MyButtonStylePause;
            }
            else
            {
                media.Play();
                Style MyButtonStylePlay = this.Resources["MyButtonStylePlay"] as Style;
                this.playButton.Style = MyButtonStylePlay;
            }
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
            progressBar.Value = 0;
            Canvas.SetLeft(progressThum, 0);
            Style MyButtonStylePause = this.Resources["MyButtonStylePause"] as Style;
            this.playButton.Style = MyButtonStylePause;
        }

        private void setButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigForm configForm = new ConfigForm(this);
            configForm.ShowDialog();
        }
        public void UpdateListBox(string folderPath)
        {
            try
            {
                resultPlayList.Clear();
                playListBoxFileNames.Clear();
                playListView.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(folderPath);
                playListBoxFileNames = GetFiles(dir);
                playListBoxFileNames.Sort();
                foreach (string filePath in playListBoxFileNames)
                {
                    string moviename = filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf("\\") - 1);
                    if (!playListView.Items.Contains(moviename))
                    {
                        playListView.Items.Add(moviename);
                    }

                }
                playListView.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {

            }
        }
        public List<string> GetFiles(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                try
                {
                    foreach (FileInfo info in directory.GetFiles())
                    {
                        if (info.FullName.EndsWith(".avi") || 
                            info.FullName.EndsWith(".wmv") || 
                            info.FullName.EndsWith(".mp4") || 
                            info.FullName.EndsWith(".flv") || 
                            info.FullName.EndsWith(".rm") || 
                            info.FullName.EndsWith(".rmvb")
#if VEE_ENCRYPT
                             || info.FullName.EndsWith(".vee")
                             || info.FullName.EndsWith(".ven")
#endif
                            )
                        {
                            resultPlayList.Add(info.FullName.ToString());
                        }

                    }
                    foreach (DirectoryInfo info in directory.GetDirectories())
                    {
                        GetFiles(info);
                    }
                }
                catch
                {

                }
            }
            return resultPlayList;
        }

        private void playListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string str = playListView.SelectedItem.ToString();
                foreach (string selectString in playListBoxFileNames)
                {
                    if (selectString.Contains(str))
                    {
                        media.PlayMovie(selectString);
                        Style MyButtonStylePlay = this.Resources["MyButtonStylePlay"] as Style;
                        this.playButton.Style = MyButtonStylePlay;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Style MyButtonStylePause = this.Resources["MyButtonStylePause"] as Style;
                this.playButton.Style = MyButtonStylePause;
            }
        }
        private void mediaControlPanel_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null!=media)
            {
                media.Close();
            }
        }

        private void preMovie_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = playListView.SelectedIndex;
            if (selectedIndex>0)
            {
                playListView.SelectedIndex = selectedIndex - 1;
            }
            
        }

        private void nextMovie_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = playListView.SelectedIndex;
            if (selectedIndex < playListView.Items.Count - 1)
            {
                playListView.SelectedIndex = selectedIndex + 1;
            }
            
        }
        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.Left = 0;
            this.Top = 0;
        }
        private void VolumeDownButton_Click(object sender, RoutedEventArgs e)
        {
            media.VolumeDecrease();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPanel.IsMute())
            {
                media.NoMute();
                Style MyButtonStyleNoMute = this.Resources["MyButtonStyleNoMute"] as Style;
                this.MuteButton.Style = MyButtonStyleNoMute;
            } 
            else
            {
                media.Mute();
                Style MyButtonStyleMute = this.Resources["MyButtonStyleMute"] as Style;
                this.MuteButton.Style = MyButtonStyleMute;
            }
        }

        private void VolumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            media.VolumeAdd();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void processTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                progressBar.Value = (media.GetMoviePosition() / media.GetMovieTotalTime()) * 100;
                progressThum.SetValue(Canvas.LeftProperty, progressBar.Value*progressBar.Width/100);
            }
            catch (System.Exception ex)
            {

            }
        }
        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                double tempValue = progressBar.Value;
                double tempTime = media.GetMovieTotalTime();
                double currentTime = (tempValue /100) * tempTime;
                media.SetMoviePosition(currentTime);
            }
            catch (System.Exception ex)
            {

            }
        }
        private void progressBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            processTimer.Stop();
            progressBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(progressBar_ValueChanged);
            progressBar.Value = e.GetPosition(progressBar).X / progressBar.Width * 100;
        }
        private void progressBar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            progressBar.ValueChanged -= progressBar_ValueChanged;
            progressThum.SetValue(Canvas.LeftProperty, e.GetPosition(progressBar).X);
            processTimer.Start();
        }
        private void progressThum_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            processTimer.Stop();
            progressBar.ValueChanged += progressBar_ValueChanged;
        }
        private void progressThum_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double moveLength = Canvas.GetLeft(progressThum) + e.HorizontalChange;
            if (moveLength > progressBar.Width)
            {
                moveLength = progressBar.Width;
                Canvas.SetLeft(progressThum, moveLength);
            }
        }
        private void progressThum_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            double moveLength = Canvas.GetLeft(progressThum) + e.HorizontalChange;
            if (moveLength>progressBar.Width)
            {
                moveLength = progressBar.Width;
            }
            Canvas.SetLeft(progressThum, moveLength);
            progressBar.Value = moveLength / progressBar.Width * 100;
            progressBar.ValueChanged -= progressBar_ValueChanged;
            processTimer.Start();
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }



    }
}
