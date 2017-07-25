using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    /*
         * End may not be necessary since if a set has
         * a startNode then the the Node after the last MUST
         * be an End and can be assumed, denoting this node
         * just makes it easier to understand in may opinion
         * I'm probably gonna put this somewhere else and I have
         * to create a good namespace structure
         */
    [Serializable]
    public enum NodeType { Start, End, Dual, Object, Empty };
}
