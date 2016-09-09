using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  fuck i figured out a better way to this
///  fuck my lifeeee
/// </summary>
namespace Brigit
{
    class GoodSortedList
    {
        public int Count {get; private set;}
        ArrayList[] array;

        public GoodSortedList()
        {
            Count = 0;
            // there can never be a structure that skips a level
            // if there is a 5 there must be a 4th level
            array = new ArrayList[4];
        }

        public void Add(Object data, int depth)
        {
            if (depth > Count) { Count = depth; }

            if(depth >= array.Length)
            {
                array = ExpandArray(array);
            }

            if(array[depth] == null)
            {
                array[depth] = new ArrayList();
            }

            array[depth].Add(data);
        }

        public ArrayList GetListAtDepth(int depth)
        {
            return array[depth];
        }

        /// <summary>
        /// Shifts everything after index to the right
        /// </summary>
        /// <param name="index"></param>
        private void ShiftRight(int index)
        {
            for(int i=Count-1;i>=index;i--)
            {
                if (array.Length <= Count)
                {
                    array = ExpandArray(array);
                }
                array[i + 1] = array[i];
            }
        }

        private ArrayList[] ExpandArray(ArrayList[] array)
        {
            ArrayList[] longerArray = new ArrayList[array.Length * 2];
            array.CopyTo(longerArray, 0);
            return longerArray;
        }
    }
}
