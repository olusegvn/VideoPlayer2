using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsfMojo.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Shell;


namespace VideoPlayer
{
    [Serializable]
    public partial class incognitoForm : Form
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
        public static Stream stream;
        public static Dictionary<string, double> timeDictionary = new Dictionary<string, double>();
        List<PictureBox> previewBoxes = new List<PictureBox>();
        public LimitedQueue<string> recentlyPlayedFiles;
        private string DATABASE = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%/AppData/Local/Microsoft/Windows/1033/1/");
        private string STORAGEBASE = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%/AppData/Local/Microsoft/Windows/1033/");
        List<Panel> previewPanels = new List<Panel>(), previewUnderPanels = new List<Panel>();
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
        Dictionary<string, int> favouriteTimes = new Dictionary<string, int>();
        bool loop = false;
        bool directoryRendered = false;
        bool textFromUser = true;
        bool random = false;
        bool renderedFavourites = false;
        bool exit = true;
        Panel p = new Panel();
        int X = 30; int Y = 30;
        int x = 3, y = 1;
        int nPreviewFiles = 4;
        int goTo = 0;
        int skipper = 0;
        bool useCustomSkipper;
        int customSkipper;
        int goToEnd;
        string output;
        int currentPlaylistIndex = -1;
        Panel gotoPanel = new Panel();
        Panel gotoEndPanel = new Panel();

        public incognitoForm()
        {
            InitializeComponent();
            resizeEverything();

            object sender = new object();
            EventArgs e = new EventArgs();

            searchTextBox_TextChanged_1(sender, e);
            System.Windows.Forms.Panel[] buttonPanels = { B1, U2, B3 };
            foreach (System.Windows.Forms.Panel panel in buttonPanels)
            {
                panel.Visible = false;
            }
            IFormatter formatter = new BinaryFormatter();
            
            //try
            //{
                stream = new FileStream(STORAGEBASE + "iQueue.txt", FileMode.Open, FileAccess.Read);
                recentlyPlayedFiles = (LimitedQueue<string>)formatter.Deserialize(stream);
                Stream stream1 = new FileStream(STORAGEBASE + "iTime.txt", FileMode.Open, FileAccess.Read);
                Stream stream2 = new FileStream(STORAGEBASE + "iFavourites.txt", FileMode.Open, FileAccess.Read);

                timeDictionary = (Dictionary<string, double>)formatter.Deserialize(stream1);
                favouriteTimes = (Dictionary<string, int>)formatter.Deserialize(stream2);
                stream.Close();
                stream1.Close();
                stream2.Close();
            //}
            //catch { recentlyPlayedFiles = new LimitedQueue<string>(80); }
            
            homeButton_Click(sender, e);
            gotoPanel.Size = new Size(10, 14);
            gotoPanel.BackColor = Color.Black;
            gotoEndPanel.Size = new Size(10, 14);
            gotoEndPanel.BackColor = Color.Black;
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
                subpanel.Location = new Point(X, Y);
                subpanel.Margin = new Padding(20);
                label.AutoSize = true;
                X += subpanel.Width;
                label.Click += new EventHandler(directory_Click);
                subpanel.Click += new EventHandler(directory_Click);
                subpanel.Controls.Add(label);
                subpanel.Controls.Add(dockPanel);
                foldersFlowLayoutPanel.Controls.Add(subpanel);
            }
        }

        public void renderFile()
        {
            directoryLabel.Text = directory;
            foldersFlowLayoutPanel.Controls.Clear();
            foreach (FileInfo _ in new DirectoryInfo(directory).GetFiles().Where(s => extentions.Contains(s.Extension.ToUpper()) && s.Name.ToLower().Contains(searchTextBox.Text.ToLower())))
            {

                FlowLayoutPanel subpanel = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(70, 70, 70),
                    Height = 350,
                    Location = new Point(X, Y),
                    Margin = new Padding(20),
                    Name = _.FullName,
                    BorderStyle = BorderStyle.FixedSingle,
                };

                subpanel.MouseEnter += new EventHandler(togglePanelWhiteSmoke);
                subpanel.MouseLeave += new EventHandler(togglePanelWhiteSmoke);
                PictureBox pictureBox = new PictureBox
                {
                    Width = subpanel.Width,
                    Height = subpanel.Height/2,
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                Panel dockPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 1,
                    BackColor = Color.Black
                };
                Button pButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(40, 40, 40),
                    Text = "Add to Playlist",
                    ForeColor = Color.White,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    Name = _.FullName,
                    Width = subpanel.Width - 8,
                };
                pButton.FlatAppearance.BorderColor = Color.Black;
                pButton.FlatAppearance.BorderSize = 1;
                pButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
                pButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);

                pButton.Click += new EventHandler(playlistAdd);
                Button button = new Button
                {
                    Dock = DockStyle.Bottom,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(40, 40, 40),
                    Text = "Delete",
                    Name = _.FullName,
                    ForeColor = Color.White,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    Width = subpanel.Width - 8,
            };
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
                button.Click += new EventHandler(deleteFile);

                Button rButton = new Button
                {
                    Dock = DockStyle.Bottom,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(40, 40, 40),
                    Text = "Rename",
                    Name = _.FullName,
                    ForeColor = Color.White,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    Width = subpanel.Width - 8,
                };
                rButton.FlatAppearance.BorderColor = Color.Black;
                rButton.FlatAppearance.BorderSize = 1;
                rButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
                rButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
                rButton.Click += new EventHandler(renameFile);

                output = DATABASE + Path.GetFileNameWithoutExtension(_.Name) + ".png";
                
                Label label = new Label
                {
                    Text = _.Name,
                    Name = _.FullName,

                    AutoSize = true,
                    Font = new Font("Microsoft JhengHei Light", 8),
                    BackColor = Color.Transparent,
                    ForeColor = Color.White
                };


                label.Click += new EventHandler(file_Click);
                subpanel.Click += new EventHandler(file_Click);
                try { ffMpeg.GetVideoThumbnail(_.FullName, output, 35); } catch { }
                try { pictureBox.Image = Image.FromFile(output); } catch { }
                subpanel.Controls.Add(pictureBox);
                subpanel.Controls.Add(label);
                subpanel.Controls.Add(dockPanel);
                subpanel.Controls.Add(pButton);
                subpanel.Controls.Add(rButton);
                subpanel.Controls.Add(button);
                foldersFlowLayoutPanel.Controls.Add(subpanel);
                X += subpanel.Width;

            }

        }

        private void file_Click(object sender, EventArgs e)
        {
            carousel.volume = 0;
            textFromUser = false;
            searchTextBox.Text = "";
            textFromUser = true;
            clickedFilePath = getControlName(sender);
            

            axVLCPlugin21.playlist.stop();
            axVLCPlugin21.playlist.items.clear();
            playlist.Clear();
            string fileUri = new Uri(clickedFilePath).AbsoluteUri;

            var directoryFiles = new DirectoryInfo(Path.GetDirectoryName(clickedFilePath)).GetFiles().Where(s => extentions.Contains(s.Extension.ToUpper()));
            string convertedUri;
            string fullname;
            
            foreach (FileInfo path in directoryFiles)
            {
                fullname = path.FullName.Replace("\\", "/");
                Uri uri = new Uri(fullname);
                convertedUri = uri.AbsoluteUri;
                axVLCPlugin21.playlist.add(convertedUri);
                playlist.Add(convertedUri);
                
            }
            

            currentPlaylistIndex = playlist.IndexOf(fileUri);
            axVLCPlugin21.playlist.playItem(currentPlaylistIndex);
            if (timeDictionary.ContainsKey(clickedFilePath))
                axVLCPlugin21.input.time = (int)timeDictionary[clickedFilePath] - 2500;
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            renderPlaylist(fileUri);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            changeFile();

            viewerButton_Click(sender, e);
        }

        public void changeDirectory(object sender, EventArgs e)
        {
            directory = getControlName(sender) + "/";
            renderFile();
            renderDirectory();
        }

        public void playlistAdd(object sender, EventArgs e)
        {
            string filename = new Uri(getControlName(sender)).AbsoluteUri;
            playlist.Add(filename);
            axVLCPlugin21.playlist.add(filename);
            ((Button)sender).Text = "Added";
            clickedFilePath = Uri.UnescapeDataString(filename).Replace("file:///", "");
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist(filename);
        }
        
        public void playlistRemove(object sender, EventArgs e)
        {
            string filename = getControlName(sender);
            axVLCPlugin21.playlist.items.remove(playlist.IndexOf(filename));
            playlist.Remove(filename);
            renderPlaylist(new Uri(clickedFilePath).AbsoluteUri);
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
        private void changeFile()
        {
            goTo = 0;
            goToEnd = -1;
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;
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

        public void renderPlaylist(string active)
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
            foreach (string video in playlist)
            {
                Panel subpanel = new Panel
                {
                    Cursor = Cursors.Hand,
                    Width = 300,
                    Height = 70,
                    Name = video,
                    Padding = new Padding(1),
                    Location = new Point(x, y),
                    BackColor = Color.FromArgb(43, 43, 43),
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
                    ForeColor = Color.WhiteSmoke,
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
                    BackColor = Color.FromArgb(40, 40, 40),
                    Text = "Remove",
                    Name = video,
                    ForeColor = Color.White,
                    Font = new Font("Microsoft JhengHei Light", 7),
                    Width = 40,
                };
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
                if (video == active)
                {

                    label.Margin = new Padding(0, 0, 50, 0);
                    label.ForeColor = Color.White;
                    subpanel.BackColor = Color.FromArgb(70, 70, 70);
                    dockPanel.Height = 10;
                    dockPanel.BackColor = Color.WhiteSmoke;
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
                return((PictureBox)sender).Name.Replace("\\", "/");
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
            axVLCPlugin21.playlist.playItem(playlist.IndexOf(active));
            clickedFilePath = Uri.UnescapeDataString(active).Replace("file:///", "");
            if (timeDictionary.ContainsKey(clickedFilePath))
                axVLCPlugin21.input.time = timeDictionary[clickedFilePath] - 2500;
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist(active);

        }

        private void nextFile()
        {
            if (random)
            {
                int r = new Random().Next(0, playlist.Count);
                axVLCPlugin21.playlist.playItem(r);
            }
            else
                axVLCPlugin21.playlist.next();
            string currentItem = playlist.ElementAt(axVLCPlugin21.playlist.currentItem);
            clickedFilePath = Uri.UnescapeDataString(currentItem).Replace("file:///", "");

            if (timeDictionary.ContainsKey(clickedFilePath))
                axVLCPlugin21.input.time = (int)timeDictionary[clickedFilePath] - 2500;
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
                axVLCPlugin21.input.time = (int)timeDictionary[clickedFilePath] - 2500;
            if (recentlyPlayedFiles.Contains(clickedFilePath))
                queueRemove(clickedFilePath);
            recentlyPlayedFiles.Enqueue(clickedFilePath);
            renderPlaylist(currentItem);
        }

        private void directory_Click(object sender, EventArgs e)
        {
            directoryRendered = true;
            textFromUser = false;
            searchTextBox.Text = "";
            textFromUser = true;
            clickedDirectory = getControlName(sender);
            directory += clickedDirectory + "/";
            renderFile();
            renderDirectory();

        }

        private int imageNumber = 0;

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
            if (recentlyPlayedFiles != null)
            {
                recentFlowLayoutPanel2.Controls.Clear();
                Label recentsLabel = new Label
                {
                    Text = "Recents",
                    ForeColor = Color.WhiteSmoke,
                    Font = new Font("Microsoft JhengHei Light", 17),
                    //Location = new Point(pPictureBox.Location.X, 0),
                    Padding = new Padding(0, 15, 0, 0),
                    Size = new Size(265, 70),

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
                                Size = new Size(265, 140),
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
                                Text = Path.GetFileNameWithoutExtension(videoFile),
                                Cursor = Cursors.Hand,
                                Font = new Font("Microsoft JhengHei Light", 7),
                                Location = new Point(pPictureBox.Location.X, pPictureBox.Location.Y + pPictureBox.Height + 1),
                                Size = new Size(265, 140),
                                ForeColor = Color.WhiteSmoke,


                            };

                            pPanel.Controls.Add(pPictureBox);
                            pPanel.Controls.Add(pLabel);
                            recentFlowLayoutPanel2.Controls.Add(pPanel);






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
            PictureBox[] PreviewBoxes1 = { previewBox5, previewBox6, previewBox7, previewBox8 };
            Label[] previewLabels1 = { previewLabel5, previewLabel6, previewLabel7, previewLabel8 };
            Panel[] previewPanels1 = { previewPanel5, previewPanel6, previewPanel7, previewPanel8 };


            try
            {
                for (int i = 0; i < 4;)
                {
                    var r = new Random().Next(0, favouriteTimes.Count());
                    if (!randoms.Contains(r))
                    {
                        fVideoFile = favouriteTimes.ElementAt(r).Key;
                        ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        output = DATABASE + Path.GetFileNameWithoutExtension(fVideoFile) + ".png";
                        carouselVideos.Add(fVideoFile);
                        previewLabels1[i].Text = Path.GetFileNameWithoutExtension(fVideoFile);
                        try { ffMpeg.GetVideoThumbnail(fVideoFile, output, 50); } catch { }
                        try { PreviewBoxes1[i].Image = Image.FromFile(output); } catch { }
                        PreviewBoxes1[i].Name = fVideoFile;
                        previewPanels1[i].Name = fVideoFile;
                        previewLabels1[i].Name = fVideoFile;
                        PreviewBoxes1[i].Click += new EventHandler(file_Click);
                        previewPanels1[i].Click += new EventHandler(file_Click);
                        previewLabels1[i].Click += new EventHandler(file_Click);
                        previewPanels1[i].Visible = true;
                        if (!randoms.Contains(r))
                            randoms.Add(r);
                        i += 1;
                    }
                    if (randoms.Count == favouriteTimes.Count)
                        break;

                }
            }
            catch { }

        }
      
        public void resizeEverything()
        {

            int newwidth = Convert.ToInt32((6 * ClientRectangle.Width / 100));
            int newheight = Convert.ToInt32((6 * ClientRectangle.Height / 100));
            //PictureBox[] PreviewBoxes = { previewBox1, previewBox2, previewBox3, pictureBox4 };
            //Panel[] previewPanels = { previewPanel1, previewPanel2, previewPanel3, previewPanel4 };
            //PictureBox[] PreviewBoxes1 = { previewBox5, previewBox6, previewBox7, previewBox8 };
            //Panel[] previewPanels1 = { previewPanel5, previewPanel6, previewPanel7, previewPanel8 };
            //for (int _ = 0; _ < previewPanels.Count(); _++)
            //{
            //    PreviewBoxes[_].Size = new Size(newheight, newwidth);
            //    previewPanels[_].Size = new Size(newheight + 50, newwidth + 60);
            //    PreviewBoxes1[_].Size = new Size(newheight, newwidth + 40);
            //    previewPanels1[_].Size = new Size(newheight + 220, newwidth + 80);
            //}
            recentFlowLayoutPanel2.Width = Convert.ToInt32((22 * ClientRectangle.Width) / 100);
            favouritesPanel.Height = Convert.ToInt32((13 * ClientRectangle.Width) / 100);
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

        }



        private void subPanel_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                ((Panel)sender).BackColor = Color.FromArgb(43, 43, 43);
            }
            catch
            {
                ((Label)sender).BackColor = Color.FromArgb(43, 43, 43);
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

        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            fullscreenExit();
            renderHomePanel();
            carousel.volume = 40;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            axVLCPlugin21.playlist.pause();
            homePanel.BringToFront();

            carousel.playlist.items.clear();
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;
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
                mainSidePanel.BackColor = Color.FromArgb(43, 43, 43);
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
                mainSidePanel.BackColor = Color.FromArgb(70, 70, 70);
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
            if (carouselCounter > carouselVideos.Count || carouselCounter < 0)
                carouselCounter = 0;
            if (carousel.playlist.items.count == 0)
            {
                carousel.playlist.items.clear();
                foreach (string item in carouselVideos)
                    carousel.playlist.add(new Uri(item).AbsoluteUri);
            }
            carousel.playlist.playItem(carouselCounter);

            try
            {
                video = carouselVideos[carouselCounter];
                carouselLabel.Text = Path.GetFileNameWithoutExtension(video);
                if (favouriteTimes.ContainsKey(video))
                    carousel.input.time = favouriteTimes[video] - 4200;
                else if (timeDictionary.ContainsKey(video))
                    carousel.input.time = timeDictionary[video] - 4200;
                else
                    carousel.input.time = 0;
            }
            catch { }
            carouselCounter++;


        }




        private void foldersButton_Click(object sender, EventArgs e)
        {
            axVLCPlugin21.playlist.pause();
            carousel.volume = 0;
            fullscreenExit();
            if (!directoryRendered)
            {
                renderFile();
                renderDirectory();
            }
            foldersPanel.BringToFront();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;

        }

        private void viewerButton_Click(object sender, EventArgs e)
        {
            carousel.volume = 0;
            axVLCPlugin21.playlist.play();
            this.FormBorderStyle = FormBorderStyle.None;
            viewerPanel.BringToFront();
            isFavourite();
            axVLCPlugin21.Focus();
        }

        public void isFavourite()
        {
            if (favouriteTimes.ContainsKey(clickedFilePath))
            {
                pictureBox5.BackColor = Color.FromArgb(70, 70, 70);
                pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                pictureBox5.BackColor = Color.FromArgb(40, 40, 40);
                pictureBox5.BorderStyle = BorderStyle.None;
            }
        }

        private void recntlyPlayedButton_Click(object sender, EventArgs e)
        {
            carousel.volume = 0;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            axVLCPlugin21.playlist.pause();
            fullscreenExit();
            recentlyPlayedFlowLayoutPanel.Controls.Clear();
            foreach (string _ in recentlyPlayedFiles.Reverse())
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
                    subpanel.BackColor = Color.FromArgb(43, 43, 43);
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
                    X += subpanel.Width;
                    label.Visible = true;
                    label.Click += new EventHandler(file_Click);
                    label1.Dock = DockStyle.Bottom;
                    subpanel.Click += new EventHandler(file_Click);
                    label.BackColor = Color.Transparent;
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
                    pictureBox.Image = Image.FromFile(DATABASE + Path.GetFileNameWithoutExtension(_) + ".png");
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Width = subpanel.Width;
                    pictureBox.Height = subpanel.Height - 130;
                    pictureBox.Name = _;
                    pictureBox.Click += new EventHandler(file_Click);
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
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            prevFile();
        }

        public void TogglePause()
        {
            if (axVLCPlugin21.playlist.isPlaying)
            {
                axVLCPlugin21.playlist.pause();
                playButton.Image = Properties.Resources.icons8_pause_50;
                controlsPanel.Visible = true;
                mainSidePanel.Visible = true;

            }
            else
            {
                axVLCPlugin21.playlist.play();
                playButton.Image = Properties.Resources.icons8_play_50;
                controlsPanel.Visible = false;
                mainSidePanel.Visible = false;
            }
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox5_Click_1(sender, e);

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            nextFile();
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.FromArgb(40, 40, 40) ? Color.FromArgb(70, 70, 70) : Color.FromArgb(40, 40, 40);
        }


        private void loopPictureBox(object sender, EventArgs e)
        {
            loop = loop? false : true;
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
            if (((Panel)sender).BackColor == Color.FromArgb(40, 40, 40))
            {
                ((Panel)sender).BackColor = Color.FromArgb(70, 70, 70);
            }
            else
            {
                ((Panel)sender).BackColor = Color.FromArgb(40, 40, 40);
            }
        }
        public void childPictureBoxTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Parent.BackColor == Color.FromArgb(40, 40, 40))
            {
                ((PictureBox)sender).Parent.BackColor = Color.FromArgb(70, 70, 70);

            }
            else
            {
                ((PictureBox)sender).Parent.BackColor = Color.FromArgb(40, 40, 40);

            }
        }
        public void childLabelTogglePanelWhiteSmoke(object sender, EventArgs e)
        {
            if (((Label)sender).Parent.BackColor == Color.FromArgb(40, 40, 40))
            {
                ((Label)sender).Parent.BackColor = Color.FromArgb(70, 70, 70);

            }
            else
            {
                ((Label)sender).Parent.BackColor = Color.FromArgb(40, 40, 40);

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
                ((Panel)sender).Controls[0].ForeColor = Color.FromArgb(70, 70, 70);
            }
        }

        private void pictureBox1_MouseEnter_1(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.FromArgb(40, 40, 40) ? Color.FromArgb(70, 70, 70) : Color.FromArgb(40, 40, 40);

        }

        private void pictureBox1_MouseLeave_1(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = ((PictureBox)sender).BackColor == Color.FromArgb(40, 40, 40) ? Color.FromArgb(70, 70, 70) : Color.FromArgb(40, 40, 40);
        }

        private void appExitButton_Click(object sender, EventArgs e)
        {
            saveQueues();
            Application.Exit();
        }

        private void fullscreenButton_Click(object sender, EventArgs e)
        {
            if (viewerFullscreen)
            {
                fullscreenExit();

            }
            else
            {
                fullscreenEnter();
            }
        }

        private void fullscreenExit()
        {
            this.WindowState = FormWindowState.Normal;
            mainSidePanel.Visible = true;
            controlsPanel.Visible = true;
            viewerPanel.Padding = new Padding(3);
            viewerFullscreen = false;

        }

        private void fullscreenEnter()
        {
            this.WindowState = FormWindowState.Maximized;
            mainSidePanel.Visible = false;
            controlsPanel.Visible = false;
            viewerPanel.Padding = new Padding(0);
            viewerFullscreen = true;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void previewLabel1_Click(object sender, EventArgs e)
        {

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
            if (e.KeyCode == Keys.P && e.Shift)
            {
                axVLCPlugin21.playlist.stop();
                playlist.Clear();
                axVLCPlugin21.playlist.items.clear();
            }

            else if (viewerPanel.Parent.Controls.GetChildIndex(viewerPanel) == 0)
            {
                


            }
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveQueues();
            if (exit)
                Application.Exit();
        }
        private void saveQueues()
        {
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(STORAGEBASE + "iQueue.txt", FileMode.Create, FileAccess.Write);
                Stream stream1 = new FileStream(STORAGEBASE + "itime.txt", FileMode.Create, FileAccess.Write);
                Stream stream2 = new FileStream(STORAGEBASE + "iFavourites.txt", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, recentlyPlayedFiles);
                formatter.Serialize(stream1, timeDictionary);
                formatter.Serialize(stream2, favouriteTimes);
                stream.Close();
                stream1.Close();
                stream2.Close();
            }
            catch { }
        }

        private void foldersBackButton_MouseEnter(object sender, EventArgs e)
        {
            foldersBackButton.BackColor = Color.FromArgb(43, 43, 43);
        }

        private void foldersBackButton_MouseLeave(object sender, EventArgs e)
        {
            foldersBackButton.BackColor = Color.FromArgb(70, 70, 70);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveQueues();
            if (MessageBox.Show("Exit and shutdown ?", "Shutdown", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                Application.Exit();
            }


        }

        private void secondsTimer_Tick(object sender, EventArgs e)
        {
            if (!controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).Y > 800 && this.WindowState == FormWindowState.Normal || !controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).Y > 1010 && this.WindowState == FormWindowState.Maximized)
            {
                controlsPanel.Visible = true;
                fullscreenClicked = false;
            }
            else if (!controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).X < 40 && this.WindowState == FormWindowState.Normal || !controlsPanel.Visible && axVLCPlugin21.PointToClient(Cursor.Position).X < 40 && this.WindowState == FormWindowState.Maximized)
            {
                mainSidePanel.Visible = true;
                fullscreenClicked = false;
            }
            TimeSpan time;
            systemTime.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            try
            {
                time = TimeSpan.FromSeconds(axVLCPlugin21.input.time * 0.001);
                currentTime.Text = time.Hours + " : " + time.Minutes + "." + time.Seconds;
                time = TimeSpan.FromSeconds(axVLCPlugin21.input.length * 0.001);
                lengthTime.Text = time.Hours + " : " + time.Minutes + "." + time.Seconds;
                seekpin.Location = new Point(Convert.ToInt32((((axVLCPlugin21.input.time / axVLCPlugin21.input.length) * 100) * seekBar.Width) / 100), 1);
                if (goToEnd > 0 && axVLCPlugin21.input.time > goToEnd)
                {
                    axVLCPlugin21.input.time = goTo;
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
                batteryTimeRemaining.Text = time.Hours + " hours " + time.Minutes + " mins remaining";
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            axVLCPlugin21.playlist.stop();
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
                    pictureBox5.BackColor = Color.FromArgb(40, 40, 40);
                    pictureBox5.BorderStyle = BorderStyle.None;
                }
            }
            else
            {
                if (MessageBox.Show("Add to Favourites ?", "Favourites", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    favouriteTimes[clickedFilePath] = (int)axVLCPlugin21.input.time;
                    pictureBox5.BackColor = Color.FromArgb(70, 70, 70);
                    pictureBox5.BorderStyle = BorderStyle.FixedSingle;
                }
            }

        }

        private void previewPanel5_MouseEnter(object sender, EventArgs e)
        {
            previewLabel5.BackColor = Color.FromArgb(70, 70, 70);
            previewBox5.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel5.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel5.Visible = true;

        }

        private void previewBox5_MouseEnter(object sender, EventArgs e)
        {
            previewLabel5.BackColor = Color.FromArgb(70, 70, 70);
            previewBox5.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel5.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel5.Visible = true;
        }

        private void previewLabel5_MouseEnter(object sender, EventArgs e)
        {
            previewLabel5.BackColor = Color.FromArgb(70, 70, 70);
            previewBox5.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel5.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel5.Visible = true;
        }

        private void previewLabel5_MouseLeave(object sender, EventArgs e)
        {

        }

        private void previewPanel5_MouseLeave(object sender, EventArgs e)
        {
            previewLabel5.BackColor = Color.FromArgb(43, 43, 43);
            previewBox5.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel5.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel5.Visible = false;
        }

        private void previewBox5_MouseLeave(object sender, EventArgs e)
        {
            previewLabel5.BackColor = Color.FromArgb(43, 43, 43);
            previewBox5.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel5.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel5.Visible = false;
        }

        private void previewPanel6_MouseLeave(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(43, 43, 43);
            previewBox6.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel6.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel6.Visible = false;
        }

        private void previewBox6_MouseLeave(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(43, 43, 43);
            previewBox6.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel6.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel6.Visible = false;
        }

        private void previewLabel6_MouseLeave(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(43, 43, 43);
            previewBox6.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel6.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel6.Visible = false;
        }

        private void previewLabel6_MouseEnter(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(70, 70, 70);
            previewBox6.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel6.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel6.Visible = true;
        }

        private void previewPanel6_MouseEnter(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(70, 70, 70);
            previewBox6.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel6.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel6.Visible = true;
        }

        private void previewBox6_MouseEnter(object sender, EventArgs e)
        {
            previewLabel6.BackColor = Color.FromArgb(70, 70, 70);
            previewBox6.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel6.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel6.Visible = true;
        }

        private void previewPanel7_MouseEnter(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(70, 70, 70);
            previewBox7.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel7.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel7.Visible = true;
        }

        private void previewBox7_MouseEnter(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(70, 70, 70);
            previewBox7.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel7.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel7.Visible = true;
        }

        private void previewLabel7_MouseEnter(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(70, 70, 70);
            previewBox7.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel7.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel7.Visible = true;
        }

        private void previewPanel7_MouseLeave(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(43, 43, 43);
            previewBox7.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel7.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel7.Visible = false;
        }

        private void previewBox7_MouseLeave(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(43, 43, 43);
            previewBox7.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel7.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel7.Visible = false;
        }

        private void previewLabel7_MouseLeave(object sender, EventArgs e)
        {
            previewLabel7.BackColor = Color.FromArgb(43, 43, 43);
            previewBox7.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel7.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel7.Visible = false;
        }

        private void panel16_MouseLeave(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(43, 43, 43);
            previewBox8.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel8.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel8.Visible = false;
        }

        private void previewBox8_MouseLeave(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(43, 43, 43);
            previewBox8.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel8.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel8.Visible = false;
        }

        private void previewLabel8_MouseLeave(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(43, 43, 43);
            previewBox8.BackColor = Color.FromArgb(43, 43, 43);
            previewPanel8.BackColor = Color.FromArgb(43, 43, 43);
            previewUnderPanel8.Visible = false;
        }

        private void previewPanel8_MouseEnter(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(70, 70, 70);
            previewBox8.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel8.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel8.Visible = true;
        }

        private void previewBox8_MouseEnter(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(70, 70, 70);
            previewBox8.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel8.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel8.Visible = true;
        }

        private void previewLabel8_MouseEnter(object sender, EventArgs e)
        {
            previewLabel8.BackColor = Color.FromArgb(70, 70, 70);
            previewBox8.BackColor = Color.FromArgb(70, 70, 70);
            previewPanel8.BackColor = Color.FromArgb(70, 70, 70);
            previewUnderPanel8.Visible = true;
        }

        private void descriptionTimer_Tick(object sender, EventArgs e)
        {
            descriptionLabel.Visible = false;
            descriptionTimer.Stop();
        }

        private void seekBar_Click(object sender, EventArgs e)
        {
            int x = seekBar.PointToClient(Cursor.Position).X;
            seekpin.Location = new Point(seekBar.PointToClient(Cursor.Position).X, 1);
            axVLCPlugin21.input.time = (x * axVLCPlugin21.input.length) / seekBar.Width;

        }

        private void seekBar_MouseEnter(object sender, EventArgs e)
        {
            seekBar.Height += 10;
            seekpin.Height += 10;
        }

        private void seekBar_MouseLeave(object sender, EventArgs e)
        {
            seekBar.Height -= 10;
            seekpin.Height -= 10;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Return to Normal Mode ?", "Incognito Mode", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                exit = false;
                baseForm.switchNormal();
                exit = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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

        private void axVLCPlugin21_ClickEvent(object sender, EventArgs e)
        {
            axVLCPlugin21.Focus();
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

        private void axVLCPlugin21_MediaPlayerEndReached(object sender, EventArgs e)
        {
            if (loop)
                axVLCPlugin21.playlist.prev();
        }

        private void randomPictureBox_Click(object sender, EventArgs e)
        {
            if (random)
            {
                
                describe("Random: off");
                randomPictureBox.BackColor = Color.FromArgb(40, 40, 40);
                randomPictureBox.BorderStyle = BorderStyle.None;
                random = false;
            }
            else
            {
                random = true;
                describe("Random: on");
                randomPictureBox.BackColor = Color.FromArgb(70, 70, 70);
                randomPictureBox.BorderStyle = BorderStyle.FixedSingle;
            }

        }

        private void viewerButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                axVLCPlugin21.volume += 10;
                describe("Volume: " + axVLCPlugin21.volume);
            }

            else if (e.KeyCode == Keys.Down)
            {
                axVLCPlugin21.volume -= 10;
                describe("Volume: " + axVLCPlugin21.volume);
            }

            else if (e.KeyCode == Keys.Right)
            {
                axVLCPlugin21.input.time += 5000;
                describe("Forward 5s");
            }
            else if (e.KeyCode == Keys.Left)
            {
                axVLCPlugin21.input.time -= 5000;
                describe("Backwards 5s");
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void axVLCPlugin21_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        axVLCPlugin21.volume += 10;
                        describe("Volume: " + axVLCPlugin21.volume);
                        break;
                    }
                case Keys.R:
                    {
                        randomPictureBox_Click(sender, e);
                        break;
                    }
                case Keys.D:
                    {
                        axVLCPlugin21.input.time = axVLCPlugin21.input.time + 10;
                        describe("Foreward: 10ms");
                        break;
                    }
                case Keys.W:
                    {
                        axVLCPlugin21.volume = axVLCPlugin21.volume + 5;
                        describe("Volume: " + axVLCPlugin21.volume.ToString());
                        break;
                    }
                case Keys.S:
                    {
                        axVLCPlugin21.volume = axVLCPlugin21.volume - 5;
                        describe("Volume: " + axVLCPlugin21.volume.ToString());
                        break;
                    }
                case Keys.A:
                    {
                        axVLCPlugin21.input.time = axVLCPlugin21.input.time - 10;
                        describe("Backward: 10ms");
                        break;
                    }
                case Keys.Space:
                case Keys.MediaPlayPause:
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
                        nextFile();
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
                        prevFile();
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
                        axVLCPlugin21.input.time = 0;
                        describe("Restart");
                        break;
                    }
                case Keys.End:
                    {
                        axVLCPlugin21.input.time = axVLCPlugin21.input.length;
                        describe("End");
                        break;
                    }
                case Keys.Z:
                    {
                        skipper = (int)axVLCPlugin21.input.time;
                        describe("Set skipper");
                        break;
                    }
                case Keys.L:
                    {
                        loopPictureBox(sender, e);
                        break;
                    }
                case Keys.OemOpenBrackets:
                    {
                        goTo = (int)axVLCPlugin21.input.time;
                        gotoPanel.Location = new Point(Convert.ToInt32((((goTo / axVLCPlugin21.input.length) * 100) * seekBar.Width) / 100), 1);
                        seekBar.Controls.Add(gotoPanel);
                        describe("Set GoTO");
                        break;
                    }
                case Keys.Oem6:
                    {
                        goToEnd = (int)axVLCPlugin21.input.time;
                        gotoEndPanel.Location = new Point(Convert.ToInt32((((goToEnd / axVLCPlugin21.input.length) * 100) * seekBar.Width) / 100), 1);
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
                        goToEnd = (int)axVLCPlugin21.input.length;
                        if (seekBar.Controls.Contains(gotoPanel))
                            seekBar.Controls.Remove(gotoPanel);
                        if (seekBar.Controls.Contains(gotoEndPanel))
                            seekBar.Controls.Remove(gotoEndPanel);
                        describe("Clear GoTO");
                        break;
                    }
                case Keys.Down:
                    {
                        axVLCPlugin21.volume -= 10;
                        describe("Volume: " + axVLCPlugin21.volume);
                        break;
                    }
                case Keys.Right:
                    {
                        axVLCPlugin21.input.time += 5000;
                        describe("Forward 5s");
                        break;
                    }
                case Keys.Left:
                    {
                        axVLCPlugin21.input.time -= 5000;
                        describe("Backwards 5s");
                        break;
                    }
                case Keys.X:
                    {
                        customSkipper = Int32.Parse(Interaction.InputBox("Skip by how many seconds?", "Skipper", "3", -1, -1)) * 1000;
                        useCustomSkipper = true;
                        break;
                    }
            }
            try
            {

                axVLCPlugin21.input.time = (float.Parse(e.KeyCode.ToString()[1].ToString()) / 9) * axVLCPlugin21.input.length;
            }
            catch { }
            if (e.KeyCode.ToString() == "P" && e.Shift)
            {
                if (MessageBox.Show("Clear Playlist ?", "Playlist", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    axVLCPlugin21.playlist.stop();
                    playlist.Clear();
                    axVLCPlugin21.playlist.items.clear();
                    renderPlaylist("");
                }
            }

            else if (e.KeyCode == Keys.Right && e.Shift)
            {
                axVLCPlugin21.input.time += 1000;
                describe("Forward 1s");
            }
            else if (e.KeyCode == Keys.Left && e.Shift)
            {
                axVLCPlugin21.input.time -= 1000;
                describe("Backwards 1s");
            }

            else if (e.KeyCode == Keys.Add && (skipper != 0 || useCustomSkipper.Equals(true)))
            {
                if (useCustomSkipper)
                    axVLCPlugin21.input.time += customSkipper;
                else
                    axVLCPlugin21.input.time = skipper;
                describe("Skipping");
            }

            else if (e.KeyCode.ToString() == "G" && goTo != 0)
            {
                axVLCPlugin21.input.time = goTo;
                describe("GoTO");
            }
        }

        public void favouritesButton_Click(object sender, EventArgs e)
        {
            carousel.volume = 0;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            axVLCPlugin21.playlist.pause();
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
                        subpanel.BackColor = Color.FromArgb(70, 70, 70);
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
            renderedFavourites = true;
            timeDictionary[clickedFilePath] = axVLCPlugin21.input.time;
            favouritesFlowLayoutPanel.BringToFront();
        }

        private void recentlyPlayedFlowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void controlsPanel_MouseLeave(object sender, EventArgs e)
        {
            controlsPanel.Visible = false;
        }

        private void axVLCPlugin21_Enter(object sender, EventArgs e)
        {

        }

        private void carouselLabel_Click(object sender, EventArgs e)
        {

        }


        private void axVLCPlugin21_MouseDownEvent(object sender, AxAXVLC.DVLCEvents_MouseDownEvent e)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            slider();
            timer1.Interval = 1;
            if (carouselCounter < 5)
                timer1.Interval = 5000;
            else
                timer1.Interval = 12000;
        }
    }
}
