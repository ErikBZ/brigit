using System.Collections.Generic;
using System.Text;
using Brigit;
using System.Windows.Forms;
using System.Drawing;

namespace BrigitVisualizer
{
    class Point
    {
        public const int Size = 50;
        // the point in pixels
        private int x = 0;
        private int y = 0;

        // The text of a res, rep, or whatever node
        string text = "empty";

        // points to draw lines to
        List<Point> childPoints = new List<Point>();

        // properties to easily use i, j "block" ints into
        // actual pixel values
        public int X
        {
            get { return x * Size;  }
            set { x = Size * value; }
        }

        public int Y
        {
            get { return y * Size; }
            set { y = Size * value; }
        }

        public int PixelX
        {
            get { return x; }
        }

        public int PixelY
        {
            get { return y; }
        }

        // the setter changes depending the type of
        // dom node being drawn
        public string Text
        {
            get { return text; }
        }

        public List<Point> PointList
        {
            get { return childPoints; }
        }

        // Differnt setters for different nodes
        public void SetResponseText(Response node)
        {
            // for now i'm just going to print the
            // type of node it is with the text that is all
            StringBuilder sb = new StringBuilder();
            sb.Append(node.ToString());
            sb.Append('\n');
            sb.Append(node.Text);
            this.text = sb.ToString();
        }

        // settings a reply node
        public void SetReplyText(Reply node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(node.ToString());
            sb.Append('\n');
            int i = 1;
            foreach(string s in node.Replies)
            {
                sb.Append(i);
                i++;
                sb.Append(s);
                sb.Append('\n');
            }
            this.text = sb.ToString();
        }

        /// <summary>
        /// adds a point to a list. These points are points
        /// that need the drawer needs to draw lines to
        /// IE this point is the parent of these points
        /// </summary>
        /// <param name="p"></param>
        public void AddToPointList(Point p)
        {
            childPoints.Add(p);
        }
    }

    /// <summary>
    /// Static Class that helps draw Brigit Objects
    /// </summary>
    static class BrigitDrawer
    {
        // this is the actual structure that will be used to draw
        /// <summary>
        /// Creates a list of points which are properly formatted for drawing
        /// </summary>
        /// <param name="tree"></param>
        public static List<Point> CreatePointList(DomTree tree)
        {
            // i'm only testings this right now so it'll be
            // the algorithm that doesn't work 
            List<Point> list = new List<Point>();
            DomNode node = tree.Head;
            int i = 1;              // the depth tracker
            while(node != null)
            {
                Point p = new Point();
                node = node.Children[0];
                i++;
            }
            return list;
        }

        /// <summary>
        /// Draws the list of points to a panel
        /// </summary>
        /// <param name="list"></param>
        public static void DrawPiontList(List<Point> list, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle();
        }

        /// <summary>
        /// Draws one single point to the screen
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        public static void DrawPoint(Point p, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(p.PixelX, p.PixelX, Point.Size, Point.Size);
            e.Graphics.DrawRectangle(Pens.Black, rect);
        }
    }
}
