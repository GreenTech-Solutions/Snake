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
            WindowCreationInfo inf = new WindowCreationInfo(new Size(640, 480), "Test", 32, true, false, false, true);
            Main_SDL main = new Main_SDL();
            main.Init(inf);
            main.ResizeWindow(1,1);
        }
    }
}
