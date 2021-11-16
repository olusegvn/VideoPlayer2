﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using System.Threading;

namespace VideoPlayer
{
    [Serializable]
    public partial class Form1 : Form
    {
        [Serializable]
        public class LimitedQueue<T> : Queue<T>
        {
            public int Limit { get; set; }


            public LimitedQueue(int limit) : base(limit)
            {
                Limit = limit;

            }

            public new void Enqueue(T item)
            {
                while (Count >= 80)
                {
                    Dequeue();

                }
                base.Enqueue(item);
            }
        }



        public Image menuVertical = VideoPlayer.Properties.Resources.icons8_menu_vertical_16;
        public Image menuImage = VideoPlayer.Properties.Resources.icons8_menu_16;
        public string[] extentions = { ".MOV", ".MP4", ".M4V", ".3GP", ".MPG", ".TS", ".VOB", ".ASF", ".AVI", ".WMV", ".MKV", ".RM", ".DV", ".FLV", ".WEBM" };
        public static IFormatter formatter = new BinaryFormatter();
        public static Stream stream, stream1, stream2, stream3;
        public LimitedQueue<string> recentlyPlayedFiles;
<<<<<<< HEAD
<<<<<<< HEAD
        public static Dictionary<string, double[]> timeDictionary = new Dictionary<string, double[]>();
=======
        public static Dictionary<string, long[]> timeDictionary = new Dictionary<string, long[]>();
>>>>>>> - Full LibVLC update
=======
        public static Dictionary<string, long[]> timeDictionary = new Dictionary<string, long[]>();
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        public static Dictionary<string, int[]> skipperData = new Dictionary<string, int[]>();

        List<PictureBox> previewBoxes = new List<PictureBox>();
        private string DATABASE = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%/AIVideo_Player/data/images/");
        private string STORAGEBASE = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%/AIVideo_Player/data/");
        NReco.VideoConverter.FFMpegConverter ffMpeg = new NReco.VideoConverter.FFMpegConverter();
        List<string> playlist = new List<string>();
        public bool viewerFullscreen = false;
        public bool fullscreenClicked = false;
        private string directory = "C:/";
        private string clickedDirectory = "";
        private string clickedFilePath = "";
        int carouselCounter = 0;
        public Panel previewUnderPanel;
        private List<string> carouselVideos = new List<string>();
        Dictionary<string, long> favouriteTimes = new Dictionary<string, long>();
        bool loop = false;
        bool directoryRendered = false;
        bool textFromUser = true;
        bool incognito = false;
        bool random = false;
        bool renderedFavourites = false;
        Panel p = new Panel();
        int X = 30; int Y = 30;
        int x = 3, y = 1;
        int nPreviewFiles = 10;
        int goTo = 0;
        int skipper = 0;
        int goToEnd;
        bool useCustomSkipper;
        int customSkipper;
        string output;
        string clickedFilePathDirectory;
        bool firstFull = true;
        int currentPlaylistIndex = -1;
        Panel gotoPanel = new Panel();
        Panel gotoEndPanel = new Panel();
        public MediaPlayer media_player;
        public Media media;
        public MediaPlayer carousel_media_player;
        public Media carousel_media;
        LibVLC libVLC;
        bool pause;
        LibVLC carouselLibVLC;

        public Form1(bool fromIncognito)
        {

            InitializeComponent();
            Core.Initialize();
            resizeEverything();

            object sender = new object();
            EventArgs e = new EventArgs();

            Panel[] buttonPanels = { B1, U2, B3 };
            foreach (Panel panel in buttonPanels)
            {
                panel.Visible = false;
            }

            IFormatter formatter = new BinaryFormatter();

            try
            {
                stream = new FileStream(STORAGEBASE + "Queue.txt", FileMode.Open, FileAccess.Read);
                recentlyPlayedFiles = (LimitedQueue<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch {
                recentlyPlayedFiles = new LimitedQueue<string>(80);
                stream.Close();
            }
            try
            {
                stream1 = new FileStream(STORAGEBASE + "time.txt", FileMode.Open, FileAccess.Read);
<<<<<<< HEAD
<<<<<<< HEAD
                timeDictionary = (Dictionary<string, double[]>)formatter.Deserialize(stream1);
                stream1.Close();
            }
            catch { 
                stream1.Close();
            }
            try
            {
                stream2 = new FileStream(STORAGEBASE + "Favourites.txt", FileMode.Open, FileAccess.Read);
                favouriteTimes = (Dictionary<string, int>)formatter.Deserialize(stream2);
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                timeDictionary = (Dictionary<string, long[]>)formatter.Deserialize(stream1);
                stream1.Close();
            }
            catch { 
                stream1.Close();
            }
            try
            {
                stream2 = new FileStream(STORAGEBASE + "Favourites.txt", FileMode.Open, FileAccess.Read);
                favouriteTimes = (Dictionary<string, long>)formatter.Deserialize(stream2);
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                stream2.Close();
            }
            catch { 
                stream2.Close();
            }
            try
            {
                stream3 = new FileStream(STORAGEBASE + "skipperData.txt", FileMode.Open, FileAccess.Read);
                skipperData = (Dictionary<string, int[]>)formatter.Deserialize(stream3);
                stream3.Close();
            }
            catch {
                stream3.Close();
            }
<<<<<<< HEAD
<<<<<<< HEAD

            homeButton_Click(sender, e);
=======
            searchTextBox_TextChanged_1(sender, e);

>>>>>>> - Full LibVLC update
=======
            searchTextBox_TextChanged_1(sender, e);

>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            gotoPanel.Size = new Size(10, 14);
            gotoPanel.BackColor = Color.Black;
            gotoEndPanel.Size = new Size(10, 14);
            gotoEndPanel.BackColor = Color.Black;
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            libVLC = new LibVLC();
            carouselLibVLC = new LibVLC();
            media_player = new MediaPlayer(libVLC);
            mainMediaPlayer.MediaPlayer = media_player;
            carousel_media_player = new MediaPlayer(carouselLibVLC);
            carousel.MediaPlayer = carousel_media_player;
            media_player.EndReached += media_player_MediaPlayerEndReached;
            homeButton_Click(sender, e);

<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            if (fromIncognito)
            {
                foreach (string videoFile in recentlyPlayedFiles.Reverse())
                {
                    clickedFilePath = videoFile; break;
                }
                file_Click();
                firstFull = false;
<<<<<<< HEAD
<<<<<<< HEAD
                viewerButton_Click(sender, e);
                axVLCPlugin21.playlist.pause();
=======
                media_player.Pause();
>>>>>>> - Full LibVLC update
=======
                media_player.Pause();
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            }
            timer1_Tick(sender, e);
            timer1.Start();
        }



        public void renderDirectory()
        {
            foreach (DirectoryInfo _ in new DirectoryInfo(directory).GetDirectories("*").Where(s => s.Name.ToLower().Contains(searchTextBox.Text.ToLower())))
            {
                FlowLayoutPanel subpanel = new FlowLayoutPanel();
                Panel dockPanel = new Panel();
                dockPanel.Dock = DockStyle.Bottom;
                dockPanel.Height = 1;
                dockPanel.BackColor = Color.Black;
                subpanel.BackColor = Color.Beige;
                subpanel.MouseEnter += new EventHandler(subPanel_MouseEnter);
                subpanel.MouseLeave += new EventHandler(dirSubPanel_MouseLeave);
                Label label = new Label();
                label.Text = _.Name;
                subpanel.Name = _.Name;
                label.Font = new Font("Microsoft JhengHei Light", 10);
                label.AutoSize = true;
                subpanel.Location = new Point(X, Y);
                subpanel.Margin = new Padding(20);
                X += subpanel.Width;
                label.Click += new EventHandler(directory_Click);
                subpanel.Click += new EventHandler(directory_Click);
                subpanel.Controls.Add(label);
                subpanel.Controls.Add(dockPanel);
                foldersFlowLayoutPanel.Controls.Add(subpanel);
                directoryRendered = true;
            }
        }

        public void playlistAdd(object sender, EventArgs e)
        {

            clickedFilePath = getControlName(sender).Replace("/", "\\");
            playlist.Add(clickedFilePath);
            ((Button)sender).Text = "Added";
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist();
        }

        public void renderFile()
        {
            directoryLabel.Text = directory;
            foldersFlowLayoutPanel.Controls.Clear();
            foreach (FileInfo _ in new DirectoryInfo(directory).GetFiles().Where(s => extentions.Contains(s.Extension.ToUpper()) && s.Name.ToLower().Contains(searchTextBox.Text.ToLower())))
            {
                FlowLayoutPanel subpanel = new FlowLayoutPanel
                {
                    Name = _.FullName,
                    Location = new Point(X, Y),
                    Margin = new Padding(20),
                    Height = 350,
                    BorderStyle = BorderStyle.FixedSingle,
                };
                PictureBox pictureBox = new PictureBox
                {
                    Width = subpanel.Width,
                    Height = subpanel.Height / 2,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Top,
                    Name = _.FullName,
                };
                Panel dockPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 1,
                    BackColor = Color.Black
                };
                Button pButton = new Button
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Bottom,
                    Text = "Add to Playlist",
                    Name = _.FullName,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    FlatStyle = FlatStyle.Flat,
                    Width = subpanel.Width - 8,
                };
                Label progressLabel = new Label
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    Anchor = AnchorStyles.Right,
                    Font = new Font("Microsoft JhengHei Light", 12),

=======
                    Font = new Font("Microsoft JhengHei Light", 12),
                    Dock = DockStyle.Top,
                    Width = subpanel.Width,
>>>>>>> - Full LibVLC update
=======
                    Font = new Font("Microsoft JhengHei Light", 12),
                    Dock = DockStyle.Top,
                    Width = subpanel.Width,
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                };
                Panel progressPanel = new Panel
                {
                    Height = 2,
                    BackColor = Color.Black,
                    Width = 0,
<<<<<<< HEAD
<<<<<<< HEAD
                    Dock = DockStyle.Bottom,
                };
                string formattedName = _.FullName.Replace("\\", "/");

                if (timeDictionary.ContainsKey(formattedName))
                {
                    int progress = (int)timeDictionary[formattedName][1];
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                    Dock = DockStyle.Top,
                    Anchor = AnchorStyles.Left,

                };

                if (timeDictionary.ContainsKey(_.FullName))
                {
                    int progress = (int)timeDictionary[_.FullName][1];
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                    progressLabel.Text = progress.ToString()+ "%";
                    progressPanel.Width = progress * subpanel.Width / 100;
                }
                pButton.FlatAppearance.BorderColor = Color.Black;
                pButton.FlatAppearance.BorderSize = 1;
                pButton.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
                pButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(252, 250, 250);
                pButton.Click += new EventHandler(playlistAdd);

                Button button = new Button
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Bottom,
                    Text = "Delete",
                    Name = _.FullName,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    FlatStyle = FlatStyle.Flat,
                    Width = subpanel.Width - 8,

                };
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(252, 250, 250);
                button.Click += new EventHandler(deleteFile);

                Button rButton = new Button
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Bottom,
                    Text = "Rename",
                    Name = _.FullName,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    FlatStyle = FlatStyle.Flat,
                    Width = subpanel.Width - 8,

                };
                rButton.FlatAppearance.BorderColor = Color.Black;
                rButton.FlatAppearance.BorderSize = 1;
                rButton.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
                rButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(252, 250, 250);
                rButton.Click += new EventHandler(renameFile);


                subpanel.BackColor = Color.FromArgb(252, 250, 250);
                subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);



                Label label = new Label
                {
                    Text = _.Name,
                    Name = _.FullName,
                    AutoSize = true,
                    Font = new Font("Microsoft JhengHei Light", 8)
                };


                output = DATABASE + Path.GetFileNameWithoutExtension(_.Name) + ".png";
                try { ffMpeg.GetVideoThumbnail(_.FullName, output, 35); } catch { }
                try { pictureBox.Image = Image.FromFile(output); } catch { }

                pictureBox.Click += new EventHandler(file_Click);
                subpanel.Click += new EventHandler(file_Click);
<<<<<<< HEAD
<<<<<<< HEAD
                label.MouseEnter += new EventHandler(subPanel_MouseEnter);
                label.MouseLeave += new EventHandler(subPanel_MouseLeave);
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                label.Click += new EventHandler(file_Click);
                label.MouseEnter += new EventHandler(childLabelTogglePanelWhiteSmoke);
                label.MouseLeave += new EventHandler(childLabelTogglePanelWhiteSmoke);
                pictureBox.MouseEnter += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                pictureBox.MouseLeave += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);



<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                subpanel.Controls.Add(progressLabel);
                subpanel.Controls.Add(progressPanel);
                subpanel.Controls.Add(pictureBox);
                subpanel.Controls.Add(label);
                subpanel.Controls.Add(dockPanel);
                subpanel.Controls.Add(pButton);
                subpanel.Controls.Add(rButton);
                subpanel.Controls.Add(button);
                foldersFlowLayoutPanel.Controls.Add(subpanel);
                X += subpanel.Width + 5;
            }

        }

        private void file_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            clickedFilePath = getControlName(sender);
            file_Click();
            viewerButton_Click(sender, e);
        }


        private void file_Click()
        {
            carousel.volume = 0;
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            clickedFilePath = getControlName(sender).Replace("/", "\\");
            file_Click();
        }

        private void file_Click()
        {
            carousel.MediaPlayer.Volume = 0;
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            directoryRendered = true;
            textFromUser = false;
            searchTextBox.Text = "";
            textFromUser = true;
<<<<<<< HEAD
<<<<<<< HEAD

            clickedFilePathDirectory = Path.GetDirectoryName(clickedFilePath);
            try
            {
                if (skipperData[clickedFilePathDirectory][1] == 0)
                {
                    skipper = skipperData[clickedFilePathDirectory][0];
                    useCustomSkipper = false;
                }
                else
                {
                    customSkipper = skipperData[clickedFilePathDirectory][0];
                    useCustomSkipper = true;
                }
            }
            catch { }

            axVLCPlugin21.playlist.stop();
            axVLCPlugin21.playlist.items.clear();
            playlist.Clear();
            string fileUri = new Uri(clickedFilePath).AbsoluteUri;

            var directoryFiles = new DirectoryInfo(Path.GetDirectoryName(clickedFilePath)).GetFiles().Where(s => extentions.Contains(s.Extension.ToUpper()));
            string convertedUri;
            string fullname;

            foreach (FileInfo path in directoryFiles)
=======
            clickedFilePathDirectory = Path.GetDirectoryName(clickedFilePath);
            try
>>>>>>> - Full LibVLC update
=======
            clickedFilePathDirectory = Path.GetDirectoryName(clickedFilePath);
            try
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            {
                if (skipperData[clickedFilePathDirectory][1] == 0)
                {
                    skipper = skipperData[clickedFilePathDirectory][0];
                    useCustomSkipper = false;
                }
                else
                {
                    customSkipper = skipperData[clickedFilePathDirectory][0];
                    useCustomSkipper = true;
                }
            }
            catch { }
            if (!random)
            {
                playlist.Clear();
                var directoryFiles = new DirectoryInfo(Path.GetDirectoryName(clickedFilePath)).GetFiles().Where(s => extentions.Contains(s.Extension.ToUpper()));
                foreach (FileInfo path in directoryFiles)
                    playlist.Add(path.FullName.Replace("/", "\\"));
            }
            media_player.Play(new Media(libVLC, clickedFilePath));
            if (timeDictionary.ContainsKey(clickedFilePath))
<<<<<<< HEAD
<<<<<<< HEAD
                axVLCPlugin21.input.time = (int)timeDictionary[clickedFilePath][0] - 2500;
=======
                media_player.Time = (int)timeDictionary[clickedFilePath][0] - 2500;
>>>>>>> - Full LibVLC update
=======
                media_player.Time = (int)timeDictionary[clickedFilePath][0] - 2500;
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            renderPlaylist();
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            changeFile();
            viewerButton_Click();

        }


        public void renderPlaylist()
        {
            y = 100;
            playlistPanel.Controls.Clear();
            Label playlistLabel = new Label
            {
                Font = new Font("Microsoft JhengHei UI Light", 16),

                Anchor = AnchorStyles.Top,
                Text = "Playlist",
                Padding = new Padding(30, 19, 0, 0),
                Size = new Size(237, 74)
            };
            playlistPanel.Controls.Add(playlistLabel);
            {
            }
            for (int _ = 0; _ < playlist.Count; _++)
            {
                string video = playlist[_];
                Panel subpanel = new Panel
                {
                    Cursor = Cursors.Hand,
                    Width = 300,
                    Height = 70,
                    Name = video,
                    Padding = new Padding(1),
                    Location = new Point(x, y),
                };
                subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                Panel dockPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 1,
                    BackColor = Color.Gray
                };
                Label label = new Label
                {
                    BackColor = Color.Transparent,
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft JhengHei Light", 9),
                    Text = Uri.UnescapeDataString(Path.GetFileNameWithoutExtension(video)),
                    Name = video,
                    AutoSize = true,
                    Padding = new Padding(0, 0, 70, 0)

                };

                Button button = new Button
                {
                    Dock = DockStyle.Bottom,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Text = "Remove",
                    Name = video,
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    Width = 40,
                };
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 244, 245);
                button.FlatAppearance.MouseDownBackColor = Color.WhiteSmoke;
                if (video == clickedFilePath)
                {
                    currentPlaylistIndex = _;
                    label.Margin = new Padding(0, 0, 50, 0);
                    label.ForeColor = Color.Black;
                    subpanel.BackColor = Color.FromArgb(240, 244, 245);
                    dockPanel.Height = 10;
                    dockPanel.BackColor = Color.Black;
                    label.Font = new Font("Microsoft JhengHei Light", 9);
                }
                else
                {
                    label.ForeColor = Color.Gray;
                }

                y += subpanel.Height;
                label.Click += new EventHandler(playlistClick);
                subpanel.Click += new EventHandler(playlistClick);
                button.Click += new EventHandler(playlistRemove);
                subpanel.Controls.Add(label);
                subpanel.Controls.Add(dockPanel);
                subpanel.Controls.Add(button);
                playlistPanel.Controls.Add(subpanel);
            }
        }

        public string getControlName(object sender)
        {
            if (sender is Panel)
                return ((Panel)sender).Name.Replace("\\", "/");
            else if (sender is PictureBox)
                return ((PictureBox)sender).Name.Replace("\\", "/");
            else if (sender is FlowLayoutPanel)
                return ((FlowLayoutPanel)sender).Name.Replace("\\", "/");
            else if (sender is Button)
                return ((Button)sender).Name.Replace("\\", "/");
            else if (sender is Label)
                return ((Label)sender).Name.Replace("\\", "/");
            return "";
        }

        public void playlistClick(object sender, EventArgs e)
        {
            changeFile();
            string active = getControlName(sender);
            clickedFilePath = active.Replace("/", "\\");
            media_player.Play(new Media(libVLC, clickedFilePath));
            if (timeDictionary.ContainsKey(clickedFilePath))
<<<<<<< HEAD
<<<<<<< HEAD
                axVLCPlugin21.input.time = timeDictionary[clickedFilePath][0] - 2500;
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(timeDictionary[clickedFilePath][0] - 2500)));
>>>>>>> - Full LibVLC update
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(timeDictionary[clickedFilePath][0] - 2500)));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist();

        }
        public void playlistRemove(object sender, EventArgs e)
        {
            string filename = getControlName(sender).Replace("/", "\\");
            playlist.Remove(filename);
            renderPlaylist();
        }
        private void changeFile()
        {
            goTo = 0;
            goToEnd = -1;
<<<<<<< HEAD
<<<<<<< HEAD
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)(media_player.Length) * 100) };
>>>>>>> - Full LibVLC update
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)(media_player.Length) * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            if (seekBar.Controls.Contains(gotoPanel))
                seekBar.Controls.Remove(gotoPanel);
            if (seekBar.Controls.Contains(gotoEndPanel))
                seekBar.Controls.Remove(gotoEndPanel);
        }



        public void queueRemove(String item)
        {
            LimitedQueue<string> temp = new LimitedQueue<string>(80);
            foreach (string _item in recentlyPlayedFiles)
            {
                if (_item != item)
                    temp.Enqueue(_item);
            }
            recentlyPlayedFiles = temp;
        }
        public void deleteFile(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete File ?", "Delete (in Administrative Mode)", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string filename = getControlName(sender);
                File.SetAttributes(filename, FileAttributes.Normal);
                File.Delete(filename);
                renderFile();
                renderDirectory();
            }
        }

        public void renameFile(object sender, EventArgs e)
        {
            string filename = getControlName(sender);
            File.SetAttributes(filename, FileAttributes.Normal);
            File.Move(filename, Path.GetDirectoryName(filename) + Microsoft.VisualBasic.Interaction.InputBox("Rename", "New name (in Administrative Mode) ", "", -1, -1) + Path.GetExtension(filename));
            renderFile();
            renderDirectory();

        }

        private void nextFile(bool next)
        {
            string currentItem;
            if (next)
                currentItem = playlist.ElementAt(currentPlaylistIndex + 1);
            else
                currentItem = playlist.ElementAt(currentPlaylistIndex - 1);

            media_player.Play(new Media(libVLC, currentItem));
            clickedFilePath = currentItem.Replace("/", "\\");
            if (timeDictionary.ContainsKey(clickedFilePath))
<<<<<<< HEAD
<<<<<<< HEAD
                axVLCPlugin21.input.time = (double)timeDictionary[clickedFilePath][0] - 2500;
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist(currentItem);
        }

        private void prevFile()
        {
            axVLCPlugin21.playlist.prev();
            string currentItem = playlist.ElementAt(axVLCPlugin21.playlist.currentItem);
            clickedFilePath = Uri.UnescapeDataString(currentItem).Replace("file:///", "");

            if (timeDictionary.ContainsKey(clickedFilePath))
                axVLCPlugin21.input.time = (double)timeDictionary[clickedFilePath][0] - 2500;
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist(currentItem);
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, (int)timeDictionary[clickedFilePath][0] - 2500));
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist();
>>>>>>> - Full LibVLC update
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, (int)timeDictionary[clickedFilePath][0] - 2500));
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist();
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        }

        private void directory_Click(object sender, EventArgs e)
        {
            directoryRendered = true;
            textFromUser = false;
            searchTextBox.Text = "";
            textFromUser = true;
            if (sender is FlowLayoutPanel)
                clickedDirectory = ((FlowLayoutPanel)sender).Name;
            else if (sender is Label)
                clickedDirectory = ((Label)sender).Text;
            directory += clickedDirectory + "/";
            renderFile();
            renderDirectory();
        }

        public void renderHomePanel()
        {
            int x = 1;
            carouselVideos.Clear();
            int count = 0;
            List<string> videoFiles = new List<string>();
            List<string> fileDirectories = new List<string>();
            string fVideoFile;
            string vidirectory;
            recentFolders.Controls.Clear();
            favoritesFlowLayoutPanel.Controls.Clear();
            if (recentlyPlayedFiles != null)
            {
                recentFlowLayoutPanel2.Controls.Clear();
                Label recentsLabel = new Label
                {
                    Text = "Recents",
                    Font = new Font("Microsoft JhengHei Light", 17),
                    //Location = new Point(pPictureBox.Location.X, 0),
                    Padding = new Padding(0, 15, 0, 0),
                    Size = new Size(265, 70),
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                };
                recentFlowLayoutPanel2.Controls.Add(recentsLabel);
                foreach (string videoFile in recentlyPlayedFiles.Reverse())
                {
                    vidirectory = Path.GetDirectoryName(videoFile);
                    if (count < nPreviewFiles && count <= recentlyPlayedFiles.Count)
                    {

                        if (!fileDirectories.Contains(vidirectory))
                        {
                            Button pButton = new Button
                            {
                                BackColor = Color.White,
                                Text = Path.GetFileName(vidirectory),
                                Name = vidirectory,
                                Font = new Font("Microsoft JhengHei Light", 7),
                                FlatStyle = FlatStyle.Flat,
                                Width = 320,
                                Height = 50,
                                Location = new Point(x)
                            };
                            pButton.FlatAppearance.BorderColor = Color.Black;
                            pButton.FlatAppearance.BorderSize = 1;
                            pButton.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
                            pButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(252, 250, 250);
                            pButton.Click += new EventHandler(changeDirectory);
                            recentFolders.Controls.Add(pButton);
                            X += pButton.Width;



                            output = DATABASE + Path.GetFileNameWithoutExtension(videoFile) + ".png";
                            carouselVideos.Add(videoFile);

                            try { ffMpeg.GetVideoThumbnail(videoFile, output, 35); } catch { }




                            Panel pPanel = new Panel
                            {
                                Dock = DockStyle.Top,
                                Anchor = AnchorStyles.Top,
<<<<<<< HEAD
<<<<<<< HEAD
                                Size = new Size(225, 140),
=======
                                Size = new Size(225, 122),
>>>>>>> - Full LibVLC update
=======
                                Size = new Size(225, 122),
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                                Padding = new Padding(18, 28, 3, 5),
                                Cursor = Cursors.Hand,

                            };
                            PictureBox pPictureBox = new PictureBox
                            {
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Size = new Size(210, 95),
                                Dock = DockStyle.Top,
                                Anchor = AnchorStyles.Top,
                                Cursor = Cursors.Hand,

                            };
                            try { pPictureBox.Image = Image.FromFile(output); } catch { }

                            Label pLabel = new Label
                            {
                                Text = Path.GetFileNameWithoutExtension(videoFile),
                                Cursor = Cursors.Hand,
                                Font = new Font("Microsoft JhengHei Light", 7),
                                Location = new Point(pPictureBox.Location.X, pPictureBox.Location.Y + pPictureBox.Height + 1),
                                Size = new Size(265, 140),

                            };

                            pPanel.Controls.Add(pPictureBox);
                            pPanel.Controls.Add(pLabel);
                            recentFlowLayoutPanel2.Controls.Add(pPanel);

<<<<<<< HEAD
<<<<<<< HEAD





=======
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                            pPictureBox.Name = videoFile;
                            pPanel.Name = videoFile;
                            pLabel.Name = videoFile;
                            pPictureBox.Click += new EventHandler(file_Click);
                            pPanel.Click += new EventHandler(file_Click);
                            pLabel.Click += new EventHandler(file_Click);
                            pLabel.MouseEnter += new EventHandler(childLabelTogglePanelWhiteSmoke);
                            pLabel.MouseLeave += new EventHandler(childLabelTogglePanelWhiteSmoke);
                            pPictureBox.MouseEnter += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                            pPictureBox.MouseLeave += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                            pPanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                            pPanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                            fileDirectories.Add(Path.GetDirectoryName(videoFile));
                            count += 1;
                        }
                    }
                    else
                        break;
                }
            }

            count = 0;
            List<int> randoms = new List<int>();
<<<<<<< HEAD
<<<<<<< HEAD
            PictureBox[] PreviewBoxes1 = { previewBox5, previewBox6, previewBox7, previewBox8 };
            Label[] previewLabels1 = { previewLabel5, previewLabel6, previewLabel7, previewLabel8 };
            Panel[] previewPanels1 = { previewPanel5, previewPanel6, previewPanel7, previewPanel8 };


            try
            {
                for (int i = 0; i < 4;)
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            try
            {
                for (int i = 0; i < nPreviewFiles;)
>>>>>>> - Full LibVLC update
                {
                    var r = new Random().Next(0, favouriteTimes.Count());
                    if (!randoms.Contains(r))
                    {
                        fVideoFile = favouriteTimes.ElementAt(r).Key;
                        ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        output = DATABASE + Path.GetFileNameWithoutExtension(fVideoFile) + ".png";
                        carouselVideos.Add(fVideoFile);
                        try { ffMpeg.GetVideoThumbnail(fVideoFile, output, 50); } catch { }
                        Panel pPanel = new Panel
                        {
                            Dock = DockStyle.Top,
                            Anchor = AnchorStyles.Top,
                            Size = new Size(225, 130),
                            Padding = new Padding(18, 28, 3, 5),
                            Cursor = Cursors.Hand,

                        };
                        PictureBox pPictureBox = new PictureBox
                        {
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Size = new Size(215, 100),
                            Dock = DockStyle.Top,
                            Anchor = AnchorStyles.Top,
                            Cursor = Cursors.Hand,

                        };
                        try { pPictureBox.Image = Image.FromFile(output); } catch { }

                        Label pLabel = new Label
                        {
                            Text = Path.GetFileNameWithoutExtension(fVideoFile),
                            Cursor = Cursors.Hand,
                            Font = new Font("Microsoft JhengHei Light", 7),
                            Location = new Point(pPictureBox.Location.X, pPictureBox.Location.Y + pPictureBox.Height + 1),
                            Size = new Size(265, 140),

                        };

                        pPanel.Controls.Add(pPictureBox);
                        pPanel.Controls.Add(pLabel);
                        favoritesFlowLayoutPanel.Controls.Add(pPanel);

                        pPictureBox.Name = fVideoFile;
                        pPanel.Name = fVideoFile;
                        pLabel.Name = fVideoFile;
                        pPictureBox.Click += new EventHandler(file_Click);
                        pPanel.Click += new EventHandler(file_Click);
                        pLabel.Click += new EventHandler(file_Click);
                        pLabel.MouseEnter += new EventHandler(childLabelTogglePanelWhiteSmoke);
                        pLabel.MouseLeave += new EventHandler(childLabelTogglePanelWhiteSmoke);
                        pPictureBox.MouseEnter += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                        pPictureBox.MouseLeave += new EventHandler(childPictureBoxTogglePanelWhiteSmoke);
                        pPanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                        pPanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                        fileDirectories.Add(Path.GetDirectoryName(fVideoFile));




                        randoms.Add(r);
                        i += 1;
                    }
                    if (randoms.Count == favouriteTimes.Count)
                        break;

                }
            }
            catch { }

        }

        private void toggleActivePreviewBox(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Parent.BackColor == Color.FromArgb(240, 244, 245))
            {
                ((PictureBox)sender).Parent.BackColor = Color.White;
            }
            else
            {
                ((PictureBox)sender).Parent.BackColor = Color.FromArgb(240, 244, 245);
            }
        }

        private void toggleActivePanel(object sender, EventArgs e)
        {
            if (((Panel)sender).BackColor == Color.FromArgb(240, 244, 245))
            {
                ((Panel)sender).BackColor = Color.White;
            }
            else
            {
                ((Panel)sender).BackColor = Color.FromArgb(240, 244, 245);
            }
        }

        public void changeDirectory(object sender, EventArgs e)
        {
            directory = getControlName(sender) + "/";
            renderFile();
            renderDirectory();
        }

        private void toggleActiveLabel(object sender, EventArgs e)
        {
            if (((Label)sender).Parent.BackColor == Color.FromArgb(240, 244, 245))
            {
                ((Label)sender).Parent.BackColor = Color.White;
            }
            else
            {
                ((Label)sender).Parent.BackColor = Color.FromArgb(240, 244, 245);
            }
        }
        public void resizeEverything()
        {
            recentFlowLayoutPanel2.Width = Convert.ToInt32((22 * ClientRectangle.Width) / 100);
<<<<<<< HEAD
<<<<<<< HEAD
            favouritesPanel.Height = Convert.ToInt32((13 * ClientRectangle.Width) / 100);
=======
            favoritesFlowLayoutPanel.Height = Convert.ToInt32((21 * ClientRectangle.Height) / 100);
>>>>>>> - Full LibVLC update
=======
            favoritesFlowLayoutPanel.Height = Convert.ToInt32((21 * ClientRectangle.Height) / 100);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        }

        public void toggleMenuicon()
        {
            if (toggleMenuButton.Image == menuImage)
                toggleMenuButton.Image = menuVertical;
            else
                toggleMenuButton.Image = menuImage;
        }

        private void subPanel_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                ((Panel)sender).BackColor = Color.Transparent;
            }
            catch
            {
                ((Label)sender).BackColor = Color.Transparent;
            }
        }



        private void subPanel_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                ((Panel)sender).BackColor = Color.FromArgb(252, 250, 250);
            }
            catch
            {
                ((Label)sender).BackColor = Color.FromArgb(252, 250, 250);
            }


        }
        private void subPanel_MouseLeaveGray(object sender, EventArgs e)
        {
            try
            {
                ((Panel)sender).BackColor = Color.Gray;
            }
            catch
            {
                ((Label)sender).BackColor = Color.Gray;
            }


        }


        private void dirSubPanel_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                ((Panel)sender).BackColor = Color.Beige;
            }
            catch
            {
                ((Label)sender).BackColor = Color.Beige;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = .9;

        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            fullscreenExit();
            renderHomePanel();
            carousel_media_player.Volume = 70;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = .9;

            media_player.Pause();
            homePanel.BringToFront();

<<<<<<< HEAD
<<<<<<< HEAD
            carousel.playlist.items.clear();
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> - Full LibVLC update
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        }

        private void homeButton_MouseEnter(object sender, EventArgs e)
        {
            B1.Visible = true;
        }

        private void homeButton_MouseLeave(object sender, EventArgs e)
        {
            B1.Visible = false;
        }

        private void recntlyPlayedButton_MouseEnter(object sender, EventArgs e)
        {
            U2.Visible = true;
        }

        private void recntlyPlayedButton_MouseLeave(object sender, EventArgs e)
        {
            U2.Visible = false;
        }

        private void foldersButton_MouseEnter(object sender, EventArgs e)
        {
            B3.Visible = true;
        }

        private void foldersButton_MouseLeave(object sender, EventArgs e)
        {
            B3.Visible = false;
        }

        private void toggleMenuButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button[] buttons = { homeButton, recntlyPlayedButton, foldersButton, viewerButton, appExitButton };
            string[] buttonTexts = { "Home", "Recently Played", "Folders", "Viewer", "Exit" };
            if (mainSidePanel.Width > 43)
            {
                foreach (System.Windows.Forms.Button button in buttons)
                {
                    button.Text = "";
                }
                mainSidePanel.Width = 42;
                mainSidePanel.BackColor = Color.FromArgb(252, 250, 250);
                toggleMenuButton.ImageAlign = ContentAlignment.MiddleLeft;
                hiddenPanel.Visible = false;

            }
            else
            {
                for (int _ = 0; _ < buttons.Length; _++)
                {
                    buttons[_].Text = buttonTexts[_];
                }
                mainSidePanel.Width = 173;
                mainSidePanel.BackColor = Color.White;
                toggleMenuButton.ImageAlign = ContentAlignment.MiddleCenter;
                hiddenPanel.Visible = true;
            }
            toggleMenuicon();
        }

        private void toggleMenuButton_MouseEnter(object sender, EventArgs e)
        {
            toggleMenuicon();
        }

        private void toggleMenuButton_MouseLeave(object sender, EventArgs e)
        {
            toggleMenuicon();
        }


        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            resizeEverything();


        }

        public string video;
        private void slider()
        {

            if (carouselCounter > carouselVideos.Count-1 || carouselCounter < 0)
                carouselCounter = 0;
            carousel_media_player.Play(new Media(libVLC, carouselVideos[carouselCounter].Replace("/", "\\")));
            try
            {
                video = carouselVideos[carouselCounter];
                carouselLabel.Text = Path.GetFileNameWithoutExtension(video);
                if (favouriteTimes.ContainsKey(video))
<<<<<<< HEAD
<<<<<<< HEAD
                    carousel.input.time = favouriteTimes[video] - 4200.0;
                else if (timeDictionary.ContainsKey(video))
                    carousel.input.time = timeDictionary[video][0] - 4200.0;
=======
                    carousel.MediaPlayer.Time = (favouriteTimes[video] - 4200);
                else if (timeDictionary.ContainsKey(video))
                    carousel.MediaPlayer.Time = (timeDictionary[video][0] - 4200);
>>>>>>> - Full LibVLC update
=======
                    carousel.MediaPlayer.Time = (favouriteTimes[video] - 4200);
                else if (timeDictionary.ContainsKey(video))
                    carousel.MediaPlayer.Time = (timeDictionary[video][0] - 4200);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                else
                    carousel.MediaPlayer.Time = 0;
            }
            catch { }
            carouselCounter++;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            slider();
            if (carouselCounter < 5)
                timer1.Interval = 5000;
            else
                timer1.Interval = 12000;
        }




        private void foldersButton_Click(object sender, EventArgs e)
        {
            media_player.Pause();
            carousel.MediaPlayer.Volume = 0;
            fullscreenExit();
            if (!directoryRendered)
            {
                renderFile();
                renderDirectory();
            }
            foldersPanel.BringToFront();
            this.FormBorderStyle = FormBorderStyle.Sizable;
<<<<<<< HEAD
<<<<<<< HEAD
            this.Opacity = .9;
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            this.Opacity = .97;
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> - Full LibVLC update
=======
            this.Opacity = .97;
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010

        }

        private void viewerButton_Click(object sender, EventArgs e)
        {
            viewerButton_Click();
        }
        private void viewerButton_Click()
        {
            filenameLabel.Text = Path.GetFileNameWithoutExtension(clickedFilePath);
            carousel.MediaPlayer.Volume = 0;
            media_player.Play();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 1;
            viewerPanel.BringToFront();
            isFavourite();
<<<<<<< HEAD
<<<<<<< HEAD
            axVLCPlugin21.BringToFront();
            axVLCPlugin21.Focus();

=======
            fullscreenEnter();
            mainMediaPlayer.Focus();
>>>>>>> - Full LibVLC update
=======
            fullscreenEnter();
            mainMediaPlayer.Focus();
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        }

        public void isFavourite()
        {
            if (favouriteTimes.ContainsKey(clickedFilePath))
            {
                pictureBox5.BackColor = Color.White;
                pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                pictureBox5.BackColor = Color.WhiteSmoke;
                pictureBox5.BorderStyle = BorderStyle.None;
            }
        }

        private void recntlyPlayedButton_Click(object sender, EventArgs e)
        {
            carousel.MediaPlayer.Volume = 0;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = .9;

            media_player.Pause();
            fullscreenExit();
            recentlyPlayedFlowLayoutPanel.Controls.Clear();
            foreach (string _ in recentlyPlayedFiles.Reverse())
            {
                try
                {
                    FlowLayoutPanel subpanel = new FlowLayoutPanel();
                    subpanel.BackColor = Color.FromArgb(252, 250, 250);
                    subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                    subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                    subpanel.Width = 200;
                    subpanel.Height = 230;
                    Panel dockPanel = new Panel(), dockPanel1 = new Panel();

                    dockPanel1.Height = 1;
                    dockPanel1.BackColor = Color.Black;
                    dockPanel.Dock = DockStyle.Bottom;
                    dockPanel.Height = 1;
                    dockPanel.BackColor = Color.Black;

                    Label label = new Label(), label1 = new Label();
                    label.Text = Path.GetFileNameWithoutExtension(_);
                    label1.Text = Path.GetDirectoryName(_);
                    label.Name = _;
                    label1.Name = _;
                    subpanel.Name = _;
                    label.AutoSize = true;
                    label.Font = new Font("Microsoft JhengHei Light", 7);
                    label1.Font = new Font("Microsoft JhengHei Light", 6);
                    Point location = new Point(X, Y);
                    subpanel.Location = location;
                    /*subpanel.Padding = new Padding(16);*/
                    subpanel.Margin = new Padding(2);
                    X += subpanel.Width;
                    label.Visible = true;
                    label.Click += new EventHandler(file_Click);
                    label1.Dock = DockStyle.Bottom;
                    subpanel.Click += new EventHandler(file_Click);
                    label.MouseEnter += new EventHandler(subPanel_MouseEnter);
                    label.MouseLeave += new EventHandler(subPanel_MouseLeave);
                    label1.MouseEnter += new EventHandler(subPanel_MouseEnter);
                    label1.MouseLeave += new EventHandler(subPanel_MouseLeave);
                    label1.Padding = new Padding(3);
                    label.Padding = new Padding(3);
                    subpanel.Padding = new Padding(3);
                    dockPanel1.Dock = DockStyle.Bottom;

                    PictureBox pictureBox = new PictureBox
                    {
                        Image = Image.FromFile(DATABASE + Path.GetFileNameWithoutExtension(_) + ".png"),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = subpanel.Width,
                        Height = subpanel.Height - 130,
                        Name = _
                    };
                    pictureBox.Click += new EventHandler(file_Click);
                    output = DATABASE + Path.GetFileNameWithoutExtension(_) + ".png";
                    try { ffMpeg.GetVideoThumbnail(_, output, 35); } catch { }

                    try { pictureBox.Image = Image.FromFile(output); } catch { }
                    subpanel.Location = new Point(x, y);
                    subpanel.Controls.Add(pictureBox);
                    subpanel.Controls.Add(label);
                    subpanel.Controls.Add(dockPanel);
                    subpanel.Controls.Add(label1);
                    subpanel.Controls.Add(dockPanel1);
                    recentlyPlayedFlowLayoutPanel.Controls.Add(subpanel);
                }
                catch { }
            }


            recentlyPlayedFlowLayoutPanel.BringToFront();
<<<<<<< HEAD
<<<<<<< HEAD
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> - Full LibVLC update
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            nextFile(false);
        }
        private void toggleControlsAndViewerOptionssPanel()
        {
            filenameFlowLayoutPanel.Visible = mainSidePanel.Visible = controlsPanel.Visible = !controlsPanel.Visible;
        }

        public void TogglePause()
        {
            if (media_player.IsPlaying)
            {
                pause = true;
                media_player.Pause();
                playButton.Image = Properties.Resources.icons8_pause_50;
                filenameFlowLayoutPanel.Visible = mainSidePanel.Visible = controlsPanel.Visible = !controlsPanel.Visible;

            }
            else
            {
                media_player.Play();
                pause = false;
                playButton.Image = Properties.Resources.icons8_play_50;
                filenameFlowLayoutPanel.Visible = mainSidePanel.Visible = controlsPanel.Visible = !controlsPanel.Visible;
            }
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            TogglePause();

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            nextFile(true);
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.WhiteSmoke ? Color.White : Color.WhiteSmoke;
        }


        private void loopPictureBox(object sender, EventArgs e)
        {
            loop = loop == true ? false : true;
            if (loop)
            {
                pictureBox7.BackColor = Color.White;
                pictureBox7.BorderStyle = BorderStyle.FixedSingle;
                describe("Loop Video");
            }
            else
            {
                pictureBox7.BackColor = Color.WhiteSmoke;
                pictureBox7.BorderStyle = BorderStyle.None;
                describe("Remove Loop");
            }
        }


        public void togglePanelWhiteSmoke(object sender, EventArgs e)
        {

            if (((Panel)sender).BackColor == Color.WhiteSmoke)
            {
                ((Panel)sender).BackColor = Color.White;
            }
            else
            {
                ((Panel)sender).BackColor = Color.WhiteSmoke;

<<<<<<< HEAD
=======
            }
        }
        public void childPictureBoxTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Parent.BackColor == Color.WhiteSmoke)
            {
                ((PictureBox)sender).Parent.BackColor = Color.White;

            }
            else
            {
                ((PictureBox)sender).Parent.BackColor = Color.WhiteSmoke;

            }
        }
        public void childLabelTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((Label)sender).Parent.BackColor == Color.WhiteSmoke)
            {
                ((Label)sender).Parent.BackColor = Color.White;

            }
            else
            {
                ((Label)sender).Parent.BackColor = Color.WhiteSmoke;

>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            }
        }
        public void childPictureBoxTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Parent.BackColor == Color.WhiteSmoke)
            {
                ((PictureBox)sender).Parent.BackColor = Color.White;

            }
            else
            {
                ((PictureBox)sender).Parent.BackColor = Color.WhiteSmoke;

            }
        }
        public void childLabelTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((Label)sender).Parent.BackColor == Color.WhiteSmoke)
            {
                ((Label)sender).Parent.BackColor = Color.White;

            }
            else
            {
                ((Label)sender).Parent.BackColor = Color.WhiteSmoke;

            }
        }
        public void togglePanelGrey(object sender, EventArgs e)
        {
            if (((Panel)sender).BackColor == Color.Gray)
            {
                ((Panel)sender).BackColor = Color.Transparent;
                ((Panel)sender).Controls[0].ForeColor = Color.Black;
            }
            else
            {
                ((Panel)sender).BackColor = Color.Gray;
                ((Panel)sender).Controls[0].ForeColor = Color.White;
            }
        }

        private void pictureBox1_MouseEnter_1(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.WhiteSmoke ? Color.White : Color.WhiteSmoke;

        }

        private void pictureBox1_MouseLeave_1(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.WhiteSmoke ? Color.White : Color.WhiteSmoke;
        }

        private void appExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fullscreenButton_Click(object sender, EventArgs e)
        {
            if (!viewerFullscreen && !firstFull)
            {
                fullscreenEnter();
            }
            else
            {
                fullscreenExit();
            }
            firstFull = false;
        }

        private void fullscreenExit()
        {
            this.WindowState = FormWindowState.Normal;
            mainSidePanel.Visible = true;
            controlsPanel.Visible = true;
            filenameFlowLayoutPanel.Visible = true;
            viewerPanel.Padding = new Padding(3);
            viewerFullscreen = false;

        }

        private void fullscreenEnter()
        {
            this.WindowState = FormWindowState.Maximized;
            mainSidePanel.Visible = false;
            controlsPanel.Visible = false;
            filenameFlowLayoutPanel.Visible = false;
            viewerPanel.Padding = new Padding(0);
            viewerFullscreen = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 1;
        }


        private void sliderBackButton_Click_1(object sender, EventArgs e)
        {
            carouselCounter -= 2;
            slider();
            timer1.Interval = 1;
            timer1.Interval = 12000;
        }

        private void searchTextBox_TextChanged_1(object sender, EventArgs e)
        {
            if (textFromUser)
            {
                renderFile();
                renderDirectory();
            }
        }

        private void foldersBackButton_Click(object sender, EventArgs e)
        {
            directory = Path.GetDirectoryName(directory.TrimEnd('/')).Replace('\\', '/') + "/";
            directoryRendered = false;
            foldersButton.PerformClick();
        }

        private void axVLCPlugin21_DblClick(object sender, EventArgs e)
        {
            fullscreenButton_Click(sender, e);
        }

        private void foldersFlowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axVLCPlugin21_KeyPressEvent_1(object sender, AxAXVLC.DVLCEvents_KeyPressEvent e)
        {
        }

        private void axVLCPlugin21_KeyDownEvent(object sender, AxAXVLC.DVLCEvents_KeyDownEvent e)
        {
        }

        private void describe(string text)
        {
            descriptionTimer_Tick(new object(), new EventArgs());
            descriptionTimer.Start();
            descriptionLabel.Text = text;
            descriptionLabel.Visible = true;

        }



        private void axVLCPlugin21_KeyUpEvent(object sender, AxAXVLC.DVLCEvents_KeyUpEvent e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());
        }

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveQueues();
            if (!incognito)
                Application.Exit();
            incognito = false;

        }

        private void saveQueues()
        {
<<<<<<< HEAD
<<<<<<< HEAD
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> - Full LibVLC update
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(STORAGEBASE + "Queue.txt", FileMode.Create, FileAccess.Write);
                Stream stream1 = new FileStream(STORAGEBASE + "time.txt", FileMode.Create, FileAccess.Write);
                Stream stream2 = new FileStream(STORAGEBASE + "Favourites.txt", FileMode.Create, FileAccess.Write);
                Stream stream3 = new FileStream(STORAGEBASE + "skipperData.txt", FileMode.Create, FileAccess.Write);

                formatter.Serialize(stream, recentlyPlayedFiles);
                formatter.Serialize(stream1, timeDictionary);
                formatter.Serialize(stream2, favouriteTimes);
                formatter.Serialize(stream3, skipperData);

                stream.Close();
                stream1.Close();
                stream2.Close();
                stream3.Close();
            }
            catch { }
          
        }

        private void foldersBackButton_MouseEnter(object sender, EventArgs e)
        {
            foldersBackButton.BackColor = Color.FromArgb(252, 250, 250);
        }

        private void foldersBackButton_MouseLeave(object sender, EventArgs e)
        {
            foldersBackButton.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit and shutdown ?", "Shutdown", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                Application.Exit();
            }


        }

        private void secondsTimer_Tick(object sender, EventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            //if (!controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).Y > 1010 && this.WindowState == FormWindowState.Maximized)
            //{
            //    controlsPanel.Visible = true;
            //    fullscreenClicked = false;
            //}
            //else if (!controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).X < 40 && this.WindowState == FormWindowState.Maximized)
            //{
            //    mainSidePanel.Visible = true;
            //    fullscreenClicked = false;
            //}



=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            if (!pause && this.WindowState == FormWindowState.Maximized)
            {
                if (!controlsPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).Y > 1010 )
                    controlsPanel.Visible = true;
                else if (controlsPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).Y < 920 )
                    controlsPanel.Visible = false;
                else if (!mainSidePanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).X < 40 )
                    mainSidePanel.Visible = true;
                else if (mainSidePanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).X > 60 )
                    mainSidePanel.Visible = false;
                else if (!filenameFlowLayoutPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).Y < 40)
                    filenameFlowLayoutPanel.Visible = true;
                else if (filenameFlowLayoutPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).Y > 40 )
                    filenameFlowLayoutPanel.Visible = false;
                else if (!playlistPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).X > 1600)
                    playlistPanel.Visible = true;
                else if (playlistPanel.Visible && mainMediaPlayer.PointToClient(Cursor.Position).X < 1600)
                    playlistPanel.Visible = false;
                fullscreenClicked = false;
            }
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            TimeSpan time;
            systemTime.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            try
            {
                time = TimeSpan.FromSeconds(media_player.Time * 0.001);
                currentTime.Text = time.Hours + " : " + time.Minutes + "." + time.Seconds;
                time = TimeSpan.FromSeconds(media_player.Length * 0.001);
                lengthTime.Text = time.Hours + " : " + time.Minutes + "." + time.Seconds;
                seekpin.Location = new Point((int)(((double)media_player.Time / (double)media_player.Length) * seekBar.Width), 1);
                if (goToEnd > 0 && media_player.Time > goToEnd)
                {
                    media_player.Time = goTo;
                }
            }
            catch { }
            batteryPercent.Text = (SystemInformation.PowerStatus.BatteryLifePercent * 100) + "%";
            if (SystemInformation.PowerStatus.BatteryChargeStatus.ToString().Contains("Charging"))
            {
                batteryTimeRemaining.Text = "  Battery Status: Charging";
            }
            else if (SystemInformation.PowerStatus.BatteryLifePercent == 1) 
            {
                batteryTimeRemaining.Text = "   Battery Status: Charged";
            }
            else
            {

                time = TimeSpan.FromSeconds(SystemInformation.PowerStatus.BatteryLifeRemaining);
                batteryTimeRemaining.Text = SystemInformation.PowerStatus.BatteryChargeStatus.ToString();
            }

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            media_player.Stop();
        }

        private void playlistPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void seekBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            if (favouriteTimes.ContainsKey(clickedFilePath))
            {
                if (MessageBox.Show("Remove from Favourites ?", "Favourites", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    favouriteTimes.Remove(clickedFilePath);
                    pictureBox5.BackColor = Color.WhiteSmoke;
                    pictureBox5.BorderStyle = BorderStyle.None;
                }
            }
            else
            {
                if (MessageBox.Show("Add to Favourites ?", "Favourites", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    favouriteTimes[clickedFilePath] = media_player.Time;
                    pictureBox5.BackColor = Color.White;
                    pictureBox5.BorderStyle = BorderStyle.FixedSingle;
                }
            }

        }

    

        private void descriptionTimer_Tick(object sender, EventArgs e)
        {
            descriptionLabel.Visible = false;
            descriptionTimer.Stop();
        }

        private void seekBar_Click(object sender, EventArgs e)
        {
            int x = seekBar.PointToClient(Cursor.Position).X;
            media_player.SeekTo(new TimeSpan(0, 0, 0, 0, (int)(x * (double)media_player.Length / seekBar.Width)));
            //MessageBox.Show(((int)((int)media_player.Time / (int)media_player.Length * 100 * seekBar.Width / 100)).ToString());

        }

        private void seekBar_MouseEnter(object sender, EventArgs e)
        {
            seekBar.Height += 10;
            seekpin.Height += 10;
            axVLCPlugin21.playlist.pause();
            
        }

        private void seekBar_MouseLeave(object sender, EventArgs e)
        {
            seekBar.Height -= 10;
            seekpin.Height -= 10;
            axVLCPlugin21.playlist.play();

        }

        private void incognitoButton_Click(object sender, EventArgs e)
        {
            if (Interaction.InputBox("Password", "Incognito", "password", -1, -1).Equals("1033"))
            {
                incognito = true;
                baseForm.switchIncognito();
            }
        }

        public void returnMode()
        {
            carousel_media_player.Play();
            timer1.Start();
        }

        private void axVLCPlugin21_ClickEvent(object sender, EventArgs e)
        {
            if (fullscreenClicked)
            {
                mainSidePanel.Visible = true;
                controlsPanel.Visible = true;
                viewerPanel.Padding = new Padding(3);
                fullscreenClicked = false;
            }
            else
            {
                mainSidePanel.Visible = false;
                controlsPanel.Visible = false;
                playlistPanel.Visible = false;
                viewerPanel.Padding = new Padding(0);
                fullscreenClicked = true;
            }
        }

        private void media_player_MediaPlayerEndReached(object sender, EventArgs e)
        {
            if (!loop)
<<<<<<< HEAD
            {
                ThreadPool.QueueUserWorkItem(_ => nextFile(true));
            }
            else
            {
                ThreadPool.QueueUserWorkItem(_ => nextFile(true));
                ThreadPool.QueueUserWorkItem(_ => nextFile(false));
            }
            else
            {
                axVLCPlugin21.playlist.prev();
                nextFile();
=======
            {
                ThreadPool.QueueUserWorkItem(_ => nextFile(true));
            }
            else
            {
                ThreadPool.QueueUserWorkItem(_ => nextFile(true));
                ThreadPool.QueueUserWorkItem(_ => nextFile(false));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            }
        }

        private void playlistButton_Click(object sender, EventArgs e)
        {
            if (playlistPanel.Visible == false)
            {
                playlistPanel.Visible = true;

            }
            else
            {
                playlistPanel.Visible = false;
            }
        }

        private void randomPictureBox_Click(object sender, EventArgs e)
        {
            if (random)
            {
                random = false;
                describe("Random: off");
                randomPictureBox.BackColor = Color.WhiteSmoke;
                randomPictureBox.BorderStyle = BorderStyle.None;
                file_Click();
            }
            else
            {
                random = true;
                describe("Random: on");
                randomPictureBox.BackColor = Color.White;
                randomPictureBox.BorderStyle = BorderStyle.FixedSingle;
                Random r = new Random();
                playlist = playlist.OrderBy(_ => r.Next()).ToList();
                clickedFilePath = playlist[0];
                file_Click();
            }
        }

        private void axVLCPlugin21_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //descriptionLabel.BringToFront();
            //MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.volume += 10;
                        describe("Volume: " + axVLCPlugin21.volume);
=======
                        media_player.Volume += 10;
                        describe("Volume: " + media_player.Volume);
>>>>>>> - Full LibVLC update
=======
                        media_player.Volume += 10;
                        describe("Volume: " + media_player.Volume);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.R:
                    {
                        randomPictureBox_Click(sender, e);
                        break;
                    }
                case Keys.D:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time = axVLCPlugin21.input.time + 10;
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(media_player.Time+10)));
>>>>>>> - Full LibVLC update
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(media_player.Time+10)));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("Foreward: 10ms");
                        break;
                    }
                case Keys.W:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.volume = axVLCPlugin21.volume + 5;
                        describe("Volume: " + axVLCPlugin21.volume.ToString());
=======
                        media_player.Volume += 5;
                        describe("Volume: " + media_player.Volume.ToString());
>>>>>>> - Full LibVLC update
=======
                        media_player.Volume += 5;
                        describe("Volume: " + media_player.Volume.ToString());
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.S:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.volume = axVLCPlugin21.volume - 5;
                        describe("Volume: " + axVLCPlugin21.volume.ToString());
=======
                        media_player.Volume -= 5;
                        describe("Volume: " + media_player.Volume.ToString());
>>>>>>> - Full LibVLC update
=======
                        media_player.Volume -= 5;
                        describe("Volume: " + media_player.Volume.ToString());
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.A:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time = axVLCPlugin21.input.time - 10;
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(media_player.Time - 10)));
>>>>>>> - Full LibVLC update
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, (int)(media_player.Time - 10)));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("Backward: 10ms");
                        break;
                    }
                case Keys.Space: case Keys.MediaPlayPause:
                    {
                        TogglePause();
                        break;
                    }
                case Keys.Return:
                    {
                        fullscreenButton_Click(sender, e);
                        break;
                    }
                case Keys.Next:
                case Keys.MediaNextTrack:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        nextFile();
=======
                        nextFile(true);
>>>>>>> - Full LibVLC update
=======
                        nextFile(true);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.P:
                    {
                        playlistButton_Click(sender, e);
                        break;
                    }
                case Keys.PageUp:
                case Keys.MediaPreviousTrack:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        prevFile();
=======
                        nextFile(false);
>>>>>>> - Full LibVLC update
=======
                        nextFile(false);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.T:
                    {
                        describe(DateTime.Now.ToString());
                        break;
                    }
                case Keys.F:
                    {
                        pictureBox5_Click_1(sender, e);
                        break;
                    }
                case Keys.Home:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time = 0;
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, 0, 0));

>>>>>>> - Full LibVLC update
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, 0, 0));

>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("Restart");
                        break;
                    }
                case Keys.End:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time = axVLCPlugin21.input.length;
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, 0, (int)media_player.Length-100));
>>>>>>> - Full LibVLC update
=======
                        media_player.SeekTo(new TimeSpan(0, 0, 0, 0, (int)media_player.Length-100));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("End");
                        break;
                    }
                case Keys.Z:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        skipper = (int)axVLCPlugin21.input.time;
                        describe("Set skipper");
                        skipperData[clickedFilePathDirectory] = new int[] { customSkipper, 1 }; ;
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        skipper = (int)media_player.Time;
                        MessageBox.Show(skipper.ToString());
                        describe("Set skipper");
                        skipperData[clickedFilePathDirectory] = new int[] { skipper, 0 }; ;
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.L:
                    {
                        loopPictureBox(sender, e);
                        break;
                    }
                case Keys.OemOpenBrackets:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        goTo = (int)axVLCPlugin21.input.time;
                        gotoPanel.Location = new Point(Convert.ToInt32((((goTo / axVLCPlugin21.input.length) * 100) * seekBar.Width) / 100), 1);
=======
                        goTo = (int)media_player.Time;
                        gotoPanel.Location = new Point(Convert.ToInt32((((goTo / media_player.Length) * 100) * seekBar.Width) / 100), 1);
>>>>>>> - Full LibVLC update
=======
                        goTo = (int)media_player.Time;
                        gotoPanel.Location = new Point(Convert.ToInt32((((goTo / media_player.Length) * 100) * seekBar.Width) / 100), 1);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        seekBar.Controls.Add(gotoPanel);
                        describe("Set GoTO");
                        break;
                    }
                case Keys.Oem6:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        goToEnd = (int)axVLCPlugin21.input.time;
                        gotoEndPanel.Location = new Point(Convert.ToInt32((((goToEnd / axVLCPlugin21.input.length) * 100) * seekBar.Width) / 100), 1);
=======
                        goToEnd = (int)media_player.Time;
                        gotoEndPanel.Location = new Point(Convert.ToInt32((((goToEnd / media_player.Length) * 100) * seekBar.Width) / 100), 1);
>>>>>>> - Full LibVLC update
=======
                        goToEnd = (int)media_player.Time;
                        gotoEndPanel.Location = new Point(Convert.ToInt32((((goToEnd / media_player.Length) * 100) * seekBar.Width) / 100), 1);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        seekBar.Controls.Add(gotoEndPanel);
                        describe("Set A-B Repeat");
                        break;
                    }
                case Keys.LWin:
                    {
                        appExitButton_Click(sender, e);
                        break;
                    }
                case Keys.Oem5:
                    {
                        goTo = 0;
<<<<<<< HEAD
<<<<<<< HEAD
                        goToEnd = (int)axVLCPlugin21.input.length;
=======
                        goToEnd = (int)media_player.Length;
>>>>>>> - Full LibVLC update
=======
                        goToEnd = (int)media_player.Length;
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        if (seekBar.Controls.Contains(gotoPanel))
                            seekBar.Controls.Remove(gotoPanel);
                        if (seekBar.Controls.Contains(gotoEndPanel))
                            seekBar.Controls.Remove(gotoEndPanel);
                        describe("Clear GoTO");
                        break;
                    }
                case Keys.Down:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.volume -= 10;
                        describe("Volume: " + axVLCPlugin21.volume);
=======
                        media_player.Volume -= 10;
                        describe("Volume: " + media_player.Volume);
>>>>>>> - Full LibVLC update
=======
                        media_player.Volume -= 10;
                        describe("Volume: " + media_player.Volume);
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        break;
                    }
                case Keys.Right:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time += 5000;
=======
                        media_player.Position += 0.001f;
>>>>>>> - Full LibVLC update
=======
                        media_player.Position += 0.001f;
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("Forward 5s");
                        break;
                    }
                case Keys.Left:
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        axVLCPlugin21.input.time -= 5000;
=======
                        media_player.Position -= 0.001f;
>>>>>>> - Full LibVLC update
=======
                        media_player.Position -= 0.001f;
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                        describe("Backwards 5s");
                        break;
                    }
                case Keys.X:
                    {
                        customSkipper = Int32.Parse(Interaction.InputBox("Skip by how many seconds?", "Skipper", "3", -1, -1)) * 1000;
                        useCustomSkipper = true;
                        skipperData[clickedFilePathDirectory] = new int[] { customSkipper, 1};

                        break;
                    }
            }
            try
            {

<<<<<<< HEAD
<<<<<<< HEAD
                axVLCPlugin21.input.time = (float.Parse(e.KeyCode.ToString()[1].ToString()) / 9) * axVLCPlugin21.input.length;
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, 0, (int)((float.Parse(e.KeyCode.ToString()[1].ToString()) / 10) * media_player.Length)));
>>>>>>> - Full LibVLC update
=======
                media_player.SeekTo(new TimeSpan(0, 0, 0, 0, (int)((float.Parse(e.KeyCode.ToString()[1].ToString()) / 10) * media_player.Length)));
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            }
            catch { }
            if (e.KeyCode.ToString() == "P" && e.Shift)
            {
                if (MessageBox.Show("Clear Playlist ?", "Playlist", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    media_player.Stop();
                    playlist.Clear();
                    renderPlaylist();
                }
            }

            else if (e.KeyCode == Keys.Right && e.Shift)
            {
                media_player.Position += 0.003f;
                describe("Forward 1s");
            }
            else if (e.KeyCode == Keys.Left && e.Shift)
            {
                media_player.Position -= 0.003f;
                describe("Backwards 1s");
            }

<<<<<<< HEAD
<<<<<<< HEAD
            else if (e.KeyCode == Keys.Add && (skipper != 0 || useCustomSkipper.Equals(true)))
            {
                if (useCustomSkipper)
                    axVLCPlugin21.input.time += customSkipper;
                else
                    axVLCPlugin21.input.time = skipper;
=======
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
            else if (e.KeyCode == Keys.Add && (skipper > 0 || useCustomSkipper.Equals(true)))
            {
                if (useCustomSkipper)
                    media_player.Time += customSkipper;
                else
                    media_player.SeekTo(new TimeSpan(0, 0, 0, 0, skipper));
<<<<<<< HEAD
>>>>>>> - Full LibVLC update
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
                describe("Skipping");
            }

            else if (e.KeyCode.ToString() == "G" && goTo != 0)
            {
                media_player.SeekTo(new TimeSpan(0, 0, 0, goTo));
                describe("GoTO");
            }       

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            carousel_media_player.Volume = 0;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = .9;

            media_player.Pause();
            fullscreenExit();
            favouritesFlowLayoutPanel.Controls.Clear();
            if (!renderedFavourites)
            {
                foreach (string _ in favouriteTimes.Keys)
                {
                    try
                    {
                        FlowLayoutPanel subpanel = new FlowLayoutPanel();
                        Panel dockPanel = new Panel(), dockPanel1 = new Panel();

                        dockPanel1.Height = 1;
                        dockPanel1.BackColor = Color.Black;
                        dockPanel.Dock = DockStyle.Bottom;
                        dockPanel.Height = 1;
                        dockPanel.BackColor = Color.Black;

                        subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                        subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                        subpanel.Width = 200;
                        subpanel.Height = 230;
                        Label label = new Label(), label1 = new Label();
                        label.Text = Path.GetFileNameWithoutExtension(_);
                        label1.Text = Path.GetDirectoryName(_);
                        label.Name = _;
                        label1.Name = _;
                        subpanel.Name = _;
                        label.AutoSize = true;
                        label.Font = new Font("Microsoft JhengHei Light", 7);
                        label1.Font = new Font("Microsoft JhengHei Light", 6);
                        Point location = new Point(X, Y);
                        subpanel.Location = location;
                        /*subpanel.Padding = new Padding(16);*/
                        subpanel.Margin = new Padding(2);
                        X += subpanel.Width + 5;
                        label.Visible = true;
                        label.Click += new EventHandler(file_Click);
                        label1.Dock = DockStyle.Bottom;
                        subpanel.Click += new EventHandler(file_Click);
                        label.MouseEnter += new EventHandler(subPanel_MouseEnter);
                        label.MouseLeave += new EventHandler(subPanel_MouseLeave);
                        label1.MouseEnter += new EventHandler(subPanel_MouseEnter);
                        label1.MouseLeave += new EventHandler(subPanel_MouseLeave);
                        label1.Padding = new Padding(3);
                        label.Padding = new Padding(3);
                        label.BackColor = Color.Transparent;
                        label1.BackColor = Color.Transparent;
                        subpanel.Padding = new Padding(3);
                        dockPanel1.Dock = DockStyle.Bottom;

                        PictureBox pictureBox = new PictureBox
                        {
                            Image = Image.FromFile(DATABASE + Path.GetFileNameWithoutExtension(_) + ".png"),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Width = subpanel.Width,
                            Height = subpanel.Height - 130,
                            Name = _,
                            BackColor = Color.Transparent,
                        };
                        pictureBox.Click += new EventHandler(file_Click);
                        output = DATABASE + Path.GetFileNameWithoutExtension(_) + ".png";
                        try { ffMpeg.GetVideoThumbnail(_, output, 35); } catch { }

                        try { pictureBox.Image = Image.FromFile(output); } catch { }
                        subpanel.BackColor = Color.FromArgb(252, 250, 250);
                        subpanel.Location = new Point(x, y);
                        subpanel.Controls.Add(pictureBox);
                        subpanel.Controls.Add(label);
                        subpanel.Controls.Add(dockPanel);
                        subpanel.Controls.Add(label1);
                        subpanel.Controls.Add(dockPanel1);
                        favouritesFlowLayoutPanel.Controls.Add(subpanel);
                    }
                    catch { }
                }
            }


            favouritesFlowLayoutPanel.BringToFront();
<<<<<<< HEAD
<<<<<<< HEAD
            timeDictionary[clickedFilePath] = new double[] { axVLCPlugin21.input.time, (axVLCPlugin21.input.time / axVLCPlugin21.input.length * 100) };
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> - Full LibVLC update
=======
            timeDictionary[clickedFilePath] = new long[] { media_player.Time, (long)((double)media_player.Time / (double)media_player.Length * 100) };
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010

        }

        private void Form1_Click(object sender, EventArgs e)
        {
        }
        private void controlsPanel_MouseLeave(object sender, EventArgs e)
        {
            //controlsPanel.Visible = !viewerFullscreen;
<<<<<<< HEAD
        }

        private void mainSidePanel_MouseLeave(object sender, EventArgs e)
=======
        }

        private void mainSidePanel_MouseLeave(object sender, EventArgs e)
        {
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            clickedFilePath = files[0];
            MessageBox.Show("real: " + clickedFilePath);
            file_Click();
            firstFull = false;
    }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

<<<<<<< HEAD
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
<<<<<<< HEAD
            MessageBox.Show("");
=======
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            clickedFilePath = files[0];
            MessageBox.Show("real: " + clickedFilePath);
            file_Click();
            firstFull = false;
    }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
>>>>>>> - Full LibVLC update
        }

        private void axVLCPlugin21_MouseMoveEvent(object sender, EventArgs e)
=======
        private void axVLCPlugin21_MouseMoveEvent(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void axVLCPlugin21_MouseMoveEvent(object sender, MouseEventArgs e)
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        {
            MessageBox.Show("");
        }

<<<<<<< HEAD
        private void axVLCPlugin21_MouseMoveEvent(object sender, MouseEventArgs e)
        {
<<<<<<< HEAD
            //controlsPanel.Visible = !viewerFullscreen;
=======
            MessageBox.Show("");
>>>>>>> - Full LibVLC update
        }

        private void controlsPanel_MouseHover(object sender, EventArgs e)
        {
=======
        private void controlsPanel_MouseHover(object sender, EventArgs e)
        {
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010

        }

        private void videoView1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void videoView1_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("Dropped");
        }

        private void videoView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        }

        private void mainMediaPlayer_Click(object sender, EventArgs e)
        {
            filenameFlowLayoutPanel.Visible = mainSidePanel.Visible = controlsPanel.Visible = !controlsPanel.Visible;
            pause = true;
        }

        private void controlsPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void controlsPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void seekBar_MouseHover(object sender, EventArgs e)
        {

        }

        private void seekBar_MouseMove(object sender, MouseEventArgs e)
        {
            
<<<<<<< HEAD
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            clickedFilePath = files[0]; 
            file_Click();
            firstFull = false;
            viewerButton_Click(sender, e);
    }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void axVLCPlugin21_MouseMoveEvent(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void axVLCPlugin21_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            MessageBox.Show("");
        }

        private void controlsPanel_MouseHover(object sender, EventArgs e)
        {

        }

        private void controlsPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void seekBar_MouseHover(object sender, EventArgs e)
        {

        }

        private void seekBar_MouseMove(object sender, MouseEventArgs e)
        {
            
=======
>>>>>>> 6c67bf427f89bc5a72b8a129a61109cb454ab010
        }

        private void axVLCPlugin21_MouseDownEvent(object sender, AxAXVLC.DVLCEvents_MouseDownEvent e)
        {
            axVLCPlugin21_ClickEvent(sender, new EventArgs());
        }
    }
}
