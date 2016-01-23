using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
            Animation a = new Animation(this);
            string Path;
            for (int i = 0; i < 4; i++)
            {
                Path = "D:\\Media\\Photo\\Work\\Sprites\\" + (i + 2) + ".png";
                a.textures.LoadTextureFromFile(Path);
            }
            a.Delay = new int[5];
            for (int i = 0; i < 5; i++)
            {
                a.Delay[i] = 0;
            }
            //TextureManager manager = new TextureManager(this);
            a.Play();

            for (int i = 0; i < 4; i++)
            {
                Path = "D:\\Media\\Photo\\Work\\Sprites\\" + (i + 2) + ".png";
                a.textures.LoadTextureFromFile(Path);
            }

            //Image image1 = Image.FromFile("D:\\Media\\Photo\\Work\\Sprites\\2.png");
            //Image image2 = Image.FromFile("D:\\Media\\Photo\\Work\\Sprites\\3.png");
            //Image image3 = Image.FromFile("D:\\Media\\Photo\\Work\\Sprites\\4.png");
            //Image image4 = Image.FromFile("D:\\Media\\Photo\\Work\\Sprites\\5.png");
            //Image image5 = Image.FromFile("D:\\Media\\Photo\\Work\\Sprites\\6.png");

            //Graphics g = this.CreateGraphics();
            //g.Clear(Color.Blue);
            //Point p = new Point(0,0);
            //g.DrawImageUnscaled(image1, p);
            //GraphicsState state = g.Save();
            //g.DrawImageUnscaled(image2, p);
            //g.Restore(state);
            //g.Clear(Color.Blue);
            //state = g.Save();
            //g.Clear(Color.Red);
            //g.Restore(state);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawLine();
        }
    }
}
