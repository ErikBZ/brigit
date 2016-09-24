using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Brigit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BrigitVisualizer
{
    public partial class Form1 : Form
    {
        DomTree tree;
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
            float midX = bufferedPanel1.Width / 2;
            if(tree != null)
            {
                e.Graphics.DrawString(tree.Name, this.Font, Brushes.Black, midX, 0);
            }
        }

        private GoodSortedList GetDomSortedList()
        {
            return null;
        }

        private DomTree OpenTreeFromDialog()
        {
            DomTree tree = null;
            Stream stream = null;
            IFormatter iformat = new BinaryFormatter();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"..\..\Brigit\doms";
            openFile.Filter = "ctom files (*.ctom)|*.ctom|All files (*.*)|*.*";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFile.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            tree = (DomTree)iformat.Deserialize(stream);
                            return tree;
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error could not open file from disk. {0}", e.ToString());
                }
            }
            return tree;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tree = OpenTreeFromDialog();
            bufferedPanel1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
   }
}
