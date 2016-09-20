using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Brigit;

namespace BrigitVisualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoScroll = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void bufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(bufferedPanel1.AutoScrollPosition.X, bufferedPanel1.AutoScrollPosition.Y);
            bufferedPanel1.Height = 2000;
            bufferedPanel1.Width = 2000;
            Rectangle rect = new Rectangle(40, 40, 100, 1000);
            e.Graphics.DrawRectangle(Pens.Black, rect);
        }

        private GoodSortedList GetDomSortedList()
        {
            return null;
        }
    }
}
