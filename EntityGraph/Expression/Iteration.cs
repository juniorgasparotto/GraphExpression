//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace EntityGraph 
//{
//    internal class Iteration<T>
//    {
//        public IEnumerator<T> Enumerator { get; set; }
//        public T EntityRootOfTheIterationForDebug { get; set; }
//        public int Level { get; set; }
//        public bool HasOpenParenthesis { get; set; }
//        public int CountIteration { get; set; }

//        public override string ToString()
//        {
//            if (EntityRootOfTheIterationForDebug == null)
//                return "";

//            return EntityRootOfTheIterationForDebug.ToString();
//        }
//    }
//}