using System;
using System.Drawing;
using System.Windows.Forms;
using StrawberryGameEngine.Core;
using StrawberryGameEngine.Video;

namespace Snake
{
    public partial class SandForm : Form
    {
        public SandForm()
        {
            InitializeComponent();
        }

        private void SandForm_Load(object sender, EventArgs e)
        {
            WindowCreationInfo inf = new WindowCreationInfo(new Size(1366, 768), "Hello From c#", 32, true, false);
            //Main_WF main = new Main_WF();
            //main.Init(inf);
            //main.Run();
            //main.ResizeWindow(new Size(640, 480));
            //main.ToggleFullscreen();
            //Thread.Sleep(500);
            //main.ToggleFullscreen();

            //main.ShutDown();
        }

        private void SandForm_Shown(object sender, EventArgs args)
        {
            
        }

        TextureManager m;

        private void DrawLine()
        {
            m = new TextureManager(this);
            int t = m.LoadTextureFromMemory(Resources.SnakeMain);
            m.ChangeTextureInfo(t, new TextureInfo(0, 0, 100));
            m.SetTextureSize(t, SystemInformation.PrimaryMonitorSize);
            //m.DrawTexture(t);
            Size textureSize = new Size((int)m.GetTextureWidth(t), (int)m.GetTextureHeight(t));
            int f = m.CreateTextureSection(t, new Section(textureSize.Width/2,0,textureSize.Width,textureSize.Height/2));
            m.DrawTextureSection(f);
            m.RemoveAllTextureSections();
            m.DrawTexture(t);
            m.DrawTextureSection(f);
            m.ReloadTextures();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawLine();
        }
    }
}
