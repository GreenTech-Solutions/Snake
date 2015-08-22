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
            Main_WF main = new Main_WF();
            main.Init(inf);
            main.Run();
            main.ResizeWindow(new Size(640, 480));
            main.ToggleFullscreen();
            Thread.Sleep(500);
            main.ToggleFullscreen();
            //main.ShutDown();
        }
    }
}
