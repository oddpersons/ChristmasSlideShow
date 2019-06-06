using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SlideShow
{
    public partial class Form1 : Form
    {

        public string SourceFilePath = "C:\\SourceImages";
        public string WatchDirectoryPath = "C:\\Users\\Brandon\\OneDrive\\SkyDrive camera roll";
        public DateTime LastFileCreate = DateTime.Now;
        public DirectoryInfo sourceDir;
        public DirectoryInfo watchDir;
        public List<string> ImageHistory = new List<string>();
        public enum Mode {Play,Pause }
        public Mode PlayMode = Mode.Play;
        public int ImageIndex = 0;

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            pictureBox1.Size = this.Size;
            sourceDir = new DirectoryInfo(SourceFilePath);
            watchDir = new DirectoryInfo(WatchDirectoryPath);
            SetNextRandomImage();
        }

        private void onMouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = this.Size;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    TogglePlayMode();
                    break;
                case Keys.Right:
                    ShowNextImage();
                    break;
                    case Keys.Left:
                    ShowPreviousImage();
                break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
            return;
        }

        public void SetNextRandomImage()
        {
            bool addedfile = false;
            FileInfo[] Watchlist = watchDir.GetFiles("*.jpg");
            foreach (FileInfo file in Watchlist)
            {
                if(file.CreationTime > LastFileCreate)
                {
                    try {
                        file.CopyTo(SourceFilePath + '\\' + file.Name);
                    } catch (Exception)
                    {

                    }
                    addedfile = true;
                }
            }

            if(addedfile == true)
                LastFileCreate = DateTime.Now;

            FileInfo[] list = sourceDir.GetFiles();
            if (list.Length > 0)
            {
                System.Random filepicker = new Random();
                int index = filepicker.Next(list.Length);
                string nextfile = list[index].FullName;
                try {
                    pictureBox1.Image = System.Drawing.Image.FromFile(nextfile);
                }catch (Exception ){

                }
                ImageHistory.Add(nextfile);
                ImageIndex = ImageHistory.Count;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(PlayMode == Mode.Play)
                SetNextRandomImage();
            
        }

        public void TogglePlayMode()
        {
            if (PlayMode == Mode.Play)
            {
                timer1.Stop();
                PlayMode = Mode.Pause;
            }
            else
            {
                timer1.Start();
                PlayMode = Mode.Play;
            }
        }

        public void SetImageFromIndex(int index)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(ImageHistory[index]);
        }

        public bool ShowPreviousImage()
        {
            if (PlayMode == Mode.Play)
                PlayMode = Mode.Pause;
            if(ImageIndex > 1)
            {
                ImageIndex--;
                SetImageFromIndex(ImageIndex);
                return true;
            }
            return false;
        }

        public bool ShowNextImage()
        {
            if(ImageIndex < ImageHistory.Count-1)
            {
                ImageIndex++;
                SetImageFromIndex(ImageIndex);
                return true;
            }
            PlayMode = Mode.Play;
            return false;
        }
    }
}
