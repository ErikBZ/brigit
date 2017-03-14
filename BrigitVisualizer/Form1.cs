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
using Brigit.Structure;

namespace BrigitVisualizer
{
    public partial class Form1 : Form
    {
        DomTree tree;
        List<Point> pointList;

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
                bufferedPanel1.Height = 700;
                BrigitDrawer.DrawTree(tree, e);
            }
            if(pointList != null)
            {
                int maxWidth = BrigitDrawer.DrawPointList(pointList, e);
                bufferedPanel1.Width = (maxWidth + 3) * 100;
            }
        }

        // will be using this but once i'm able to convert graphs
        // to sets correctly. that is my next task
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

        // for now i'm just testing the drawing of sets
        private void button1_Click(object sender, EventArgs e)
        {
            //tree = OpenTreeFromDialog();
            ListAndDepth pointsAndDepth = BrigitDrawer.CreatePointList(Tester.GetTestBaseTest());
            pointList = pointsAndDepth.Points;
            bufferedPanel1.Height = pointsAndDepth.MaxDepths + 200;
            bufferedPanel1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
   }
}
