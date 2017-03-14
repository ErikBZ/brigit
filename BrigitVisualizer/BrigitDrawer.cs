using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using Brigit;
using System.Windows.Forms;
using System.Drawing;
using Brigit.Structure;

namespace BrigitVisualizer
{
    struct ListAndDepth
    {
        public List<Point> Points;
        public int MaxDepths;
    }
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

        // points to the parent to draw a line to
        Point[] parents;

        // properties to easily use i, j "block" ints into
        // actual pixel values
        public int X
        {
            get { return x / Size; }
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

        // the parents of this point
        public Point[] Parents
        {
            get { return parents; }
            set { parents = value; }
        }

        public void SetDomNodeText(DomNode node)
        {
            if (node is Dialog)
            {
                this.SetResponseText((Dialog)node);
            }
            else if (node is Choice)
            {
                this.SetReplyText((Choice)node);
            }
            else
            {
                throw new System.Exception("Node type not recognized");
            }
        }

        public void SetNodeText(Node node)
        {
            text = node.ToString();
        }

        // Differnt setters for different nodes
        private void SetResponseText(Dialog node)
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
        private void SetReplyText(Choice node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(node.ToString());
            sb.Append('\n');
            int i = 1;
            foreach(string s in node.Choices)
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
            return $"X: {this.x} Y: {this.y}, {this.text}";
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
            DrawPointList(list, e);
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
        
        // this will need to be a recursive function
        public static ListAndDepth CreatePointList(StraightSet set)
        {
            ListAndDepth lAndDepth;
            StraightSet.CenterSet(set);
            lAndDepth.Points = new List<Point>();
            Point[] parents = null;

            Point[] lastNodes = AddToList(parents, lAndDepth.Points, set);

            // list should not be a list of points that can draw a set
            // Time for testing yay!
            lAndDepth.MaxDepths = lastNodes[0].PixelY;
            return lAndDepth;
        }

        // recrusive to add things to a point list
        /// <summary>
        /// Recursively adds points to the point list while traversing the graph
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="list"></param>
        /// <param name="set"></param>
        /// <returns>The parent nodes for the next element</returns>
        private static Point[] AddToList(Point[] parents, List<Point> list, StraightSet set)
        {
            for (int i = 0; i < set.Count; i++)
            {
                object elem = set.GetObjAt(i);
                if (elem is BranchSet)
                {
                    // will get cast to an array
                    List<Point> nextParents = new List<Point>();
                    BranchSet branch = (BranchSet)elem;
                    for(int j=0;j<branch.Count;j++)
                    {
                        Point[] points = AddToList(parents, list, branch.GetObjAt(j));
                        foreach(Point p in points)
                        {
                            nextParents.Add(p);
                        }
                    }
                    parents = nextParents.ToArray();
                }
                // converts Node to a point, adds it to the list, and sets the point 
                // as "old" or "parent" point to be the parent of the next node
                else if(elem is Node)
                {
                    Point newPoint = null;
                    if(parents == null)
                    {
                        newPoint = NodeToPoint((Node)elem, 0, set.Center);
                    }
                    else
                    {
                        int maxDepth = GetMaxDepth(parents);
                        if(parents.Length > 1)
                        {
                            foreach(Point p in parents)
                            {
                                p.Y = maxDepth;
                            }
                        }
                        else if(parents.Length == 0)
                        {
                            throw new Exception("Parent list is somehow empty but not null");
                        }
                        newPoint = NodeToPoint((Node)elem, maxDepth, set.Center);
                        newPoint.Parents = parents;
                    }
                    parents = new Point[]{ newPoint };
                    list.Add(newPoint);
                }
                else
                {
                    throw new Exception("Straight set cannot have non Node or BranchSet elements");
                }
            }
            return parents;
        }

        /// <summary>
        /// Creates a point out of node. Needs the parent with the max depth to 
        /// calculate its depth
        /// </summary>
        /// <param name="n"></param>
        /// <param name="maxParent"></param>
        /// <returns></returns>
        private static Point NodeToPoint(Node n, int depth, int center)
        {
            Point p = new Point();
            p.X = center;
            p.Y = depth+ 1;
            p.SetNodeText(n);
            return p;
        }

        /// <summary>
        /// Gets the max depths from an array of points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static int GetMaxDepth(Point[] points)
        {
            int max = 0;
            if(points == null)
            {
                max = 0;
            }
            else
            {
                foreach(Point p in points)
                {
                    if(p.Y > max)
                    {
                        max = p.Y;
                    }
                }
            }
            return max;
        }

        /// <summary>
        /// Draws the list of points to a panel and returns
        /// the furtherest node to the right
        /// </summary>
        /// <param name="list"></param>
        public static int DrawPointList(List<Point> list, PaintEventArgs e)
        {
            int max = 0;
            foreach(Point p in list)
            {
                int num = DrawPoint(p, e);
                if(num > max)
                {
                    max = num;
                }
            }
            return max;
        }

        /// <summary>
        /// Draws one single point to the screen
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        private static int DrawPoint(Point p, PaintEventArgs e)
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
            return p.X;
        }
    }
}
