using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using Win32API;
using StrawberryGameEngine.Core;
using System.Threading;

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
            DrawLine();
        }

        private void DrawLine()
        {
            Graphics g = this.CreateGraphics();
            g.DrawLine(new Pen(Color.Red), 10, 10, 100, 100);
            g.DrawRectangle(new Pen(Color.Blue), new Rectangle(10,10,100,250));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawLine();
        }
    }
}
