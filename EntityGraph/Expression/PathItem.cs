//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace EntityGraph
//{
//    private class ExpressionPathItem<T>
//    {
//        public int Level { get; private set; }
//        public T Edge { get; private set; }
//        internal Iteration<T> Iteration { get; private set; }

//        internal PathItem(Iteration<T> Iteration, T edge, int level)
//        {
//            this.Edge = edge;
//            this.Level = level;
//            this.Iteration = Iteration;
//        }

//        public override string ToString()
//        {
//            return "[" + this.Edge.ToString() + "]";
//        }
//    }
//}