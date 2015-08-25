//// This program prints out basic information about the crash dump specified.
////
//// The platform must match what you are debugging, as we have to load the dac, a native dll.
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace EntityGraph
//{
//    // Or node
//    public class Entity<T>
//    {
//        private HashSet<Entity<T>> predecessors;
//        private HashSet<Entity<T>> successors;

//        public T Entity2 { get; private set; }

//        // parents
//        public IEnumerable<Entity<T>> Predecessors
//        { 
//            get 
//            {
//                return predecessors;
//            }
//        }

//        // children
//        public IEnumerable<Entity<T>> Successors
//        {
//            get
//            {
//                return successors;
//            }
//        }

//        internal Entity(T entity)
//        {
//            this.Entity2 = entity;
//            this.predecessors = new HashSet<Entity<T>>();
//            this.successors = new HashSet<Entity<T>>();
//        }

//        #region Overrides

//        public static bool operator ==(Entity<T> a, T b)
//        {
//            return Equals(a, b);
//        }

//        public static bool operator !=(Entity<T> a, T b)
//        {
//            return !Equals(a, b);
//        }

//        public static bool operator ==(Entity<T> a, Entity<T> b)
//        {
//            return Equals(a, b);
//        }

//        public static bool operator !=(Entity<T> a, Entity<T> b)
//        {
//            return !Equals(a, b);
//        }

//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(obj, null)) 
//                return false;

//            if (obj is T)
//                return this.Entity2.Equals(obj);

//            return false;
//        }

//        public override int GetHashCode()
//        {
//            return this.Entity2.GetHashCode();
//        }

//        public override string ToString()
//        {
//            return Entity2.ToString();
//        }

//        #endregion
//    }
//}