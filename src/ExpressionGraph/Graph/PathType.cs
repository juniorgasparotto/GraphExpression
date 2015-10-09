using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public enum PathType
    {
        // Not repeat and not circular
        Simple,
        // Repeat first and last (circular)
        Circuit,
        // Repeat in middle (A,B,B)
        Circle,
    }
}