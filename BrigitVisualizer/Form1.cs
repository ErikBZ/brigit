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
        DomTree tree;
        public Form1()
        {
            InitializeComponent();
            this.AutoScroll = true;
            tree = DomAdmin.ReadDomTree(@"..\..\..\Brigit\doms\test.ctom");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void bufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(bufferedPanel1.AutoScrollPosition.X, bufferedPanel1.AutoScrollPosition.Y);
            float midX = bufferedPanel1.Width / 2;
            e.Graphics.DrawString(tree.Name, this.Font, Brushes.Black, midX, 0);
        }

        private GoodSortedList GetDomSortedList()
        {
            return null;
        }
    }
}
