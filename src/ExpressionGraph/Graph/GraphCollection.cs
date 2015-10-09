//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace EntityGraph
//{
//    public class GraphCollection<T> : IEnumerable<GraphCollection<T>>
//    {
//        private List<GraphCollection<T>> graphs;

//        public GraphCollection<T> this[int i]
//        {
//            get
//            {
//                return graphs[i];
//            }
//        }

//        public GraphCollection<T> this[Vertex<T> vertex]
//        {
//            get
//            {
//                return graphs[i];
//            }
//        }

//        internal GraphCollection()
//        {
//            this.graphs = new List<GraphCollection<T>>();
//        }

//        internal void Add(GraphCollection<T> item)
//        {   
//            this.graphs.Add(item);
//        }

//        public IEnumerator<GraphCollection<T>> GetEnumerator()
//        {
//            return graphs.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return graphs.GetEnumerator();
//        }
//    }
//}