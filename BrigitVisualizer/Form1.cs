using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrigitVisualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        /// <summary>
        /// Draws an arc and rectanlge
        /// </summary>
        public void Draw()
        {
            Graphics graphics = this.CreateGraphics();
            Rectangle rect = new Rectangle(50, 100, 150, 150);
            graphics.DrawArc(Pens.Black, rect, 0.5f, 0f);
            graphics.DrawRectangle(Pens.Red, rect);
        }
    }
}
