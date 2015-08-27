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
            var inf = new WindowCreationInfo(new Size(1366, 768), "Hello From c#", 32, true, false);
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

        TextureManager _m;

        private void DrawLine()
        {
            _m = new TextureManager(this);
            var t = _m.LoadTextureFromMemory(Resources.SnakeMain);
            _m.ChangeTextureInfo(t, new TextureInfo(0, 0, 100));
            _m.SetTextureSize(t, SystemInformation.PrimaryMonitorSize);
            //m.DrawTexture(t);
            var textureSize = new Size((int)_m.GetTextureWidth(t), (int)_m.GetTextureHeight(t));
            var f = _m.CreateTextureSection(t, new Section(textureSize.Width/2,0,textureSize.Width,textureSize.Height/2));
            _m.DrawTextureSection(f);
            _m.RemoveAllTextureSections();
            _m.DrawTexture(t);
            _m.ReloadTextures();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawLine();
        }
    }
}
