using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class HelpFormShowCurrentMousePosition : Form
    {
        public HelpFormShowCurrentMousePosition()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public void HelpFormShowCurrentMousePosition_Load(object sender, EventArgs e)
        {

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            Point cursorPosition = Cursor.Position;
            int mouseX = cursorPosition.X;
            int mouseY = cursorPosition.Y;
            lbl_x.Text = mouseX.ToString();
            lbl_y.Text = mouseY.ToString();
        }
    }
}
