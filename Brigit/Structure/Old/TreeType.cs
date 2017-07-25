using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    /*
         * If a tree is an inner type tree then it only
         * needs to have tails and Head correctly tracked
         */
    [Serializable]
    public enum TreeType { Outer, Inner, Empty};
}
