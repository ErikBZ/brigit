using System.Collections.Generic;
using System.Collections;
using System.Text;
using Brigit;
using System.Windows.Forms;
using System.Drawing;

namespace BrigitVisualizer
{
    class Point
    {
        public const int Size = 100;
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
            get { return x / Size;  }
            set { x = Size * value; }
        }

        public int Y
        {
            get { return y / Size; }
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

        public Point()
        {
            x = 0;
            y = 0;
            text = string.Empty;
        }
        public Point(int x, int y, string text)
        {
            X = x;
            Y = y;
            this.text = text;
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

        public void SetDomNodeText(DomNode node)
        {
            if(node is Response)
            {
                this.SetResponseText((Response)node);
            }
            else if(node is Reply)
            {
                this.SetReplyText((Reply)node);
            }
            else
            {
                throw new System.Exception("Node type not recognized");
            }
        }

        // Differnt setters for different nodes
        private void SetResponseText(Response node)
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
        private void SetReplyText(Reply node)
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
        public void AddToChildList(Point p)
        {
            childPoints.Add(p);
        }

        public override string ToString()
        {
            return $"X: {this.x} Y: {this.y}";
        }
    }

    /// <summary>
    /// Static Class that helps draw Brigit Objects
    /// </summary>
    static class BrigitDrawer
    {
        /// <summary>
        /// Draws the tree into a nice picture
        /// </summary>
        /// <param name="dom"></param>
        public static void DrawTree(DomTree dom, PaintEventArgs e)
        {
            List<Point> list = CreatePointList(dom);
            DrawPiontList(list, e);
        }

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
            int j = 3;              // the cneter
            while(node != null)
            {
                if(node.Children.Length > 0)
                {
                    node = node.Children[0];
                    Point p = new Point();
                    p.X = j;
                    p.Y = i;
                    p.SetDomNodeText(node);
                    list.Add(p);
                    i++;
                }
                else
                {
                    node = null;
                }
            }
            return list;
        }

        /// <summary>
        /// Goes through a set of sets and creates a list of points
        /// to draw
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static List<Point> CreatePointList(StraightSet set)
        {
            List<Point> list = new List<Point>();
            int center = 0;

            // gets the first node IE the head of the graph
            Queue que = new Queue();
            que.Enqueue(set.GetObjAt(0));

            while (que.Count != 0)
            {

            }

            return list;
        }

        /// <summary>
        /// Draws the list of points to a panel
        /// </summary>
        /// <param name="list"></param>
        private static void DrawPiontList(List<Point> list, PaintEventArgs e)
        {
            foreach(Point p in list)
            {
                DrawPoint(p, e);
            }
        }

        /// <summary>
        /// Draws one single point to the screen
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        private static void DrawPoint(Point p, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(p.PixelX, p.PixelY, Point.Size, Point.Size);
            e.Graphics.DrawRectangle(Pens.Black, rect);

            // ohh fun using statement
            using (Font f = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
            {
                TextFormatFlags flags = TextFormatFlags.WordBreak;
                TextRenderer.DrawText(e.Graphics, p.Text, f, rect, Color.Black, flags);
                e.Graphics.DrawRectangle(Pens.Red, rect);
            }
        }
    }
}
