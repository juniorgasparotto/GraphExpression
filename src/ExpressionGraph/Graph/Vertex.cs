// This program prints out basic information about the crash dump specified.
//
// The platform must match what you are debugging, as we have to load the dac, a native dll.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    // Or node
    public class Vertex<T>
    {
        private HashSet<Vertex<T>> parents;
        private HashSet<Vertex<T>> children;

        public T Entity { get; private set; }
        public int InternalId { get; private set; }
        public Graph<T> Graph { get; private set; }

        public int CountVisited { get; internal set; }

        // parents
        public IEnumerable<Vertex<T>> Parents
        { 
            get 
            {
                return parents;
            }
        }

        // children
        public IEnumerable<Vertex<T>> Children
        {
            get
            {
                return children;
            }
        }

        // parent and children
        //public IEnumerable<Vertex<T>> ParentsAndChildren
        //{
        //    get
        //    {
        //        var list = this.parents.ToList();
        //        list.AddRange(successors);
        //        return list;
        //    }
        //}

        // Grau de entrada (numero de parents)
        public int Indegrees
        {
            get
            {
                return parents.Count;
            }
        }

        // Grau de saída (numero de filhos)
        public int Outdegrees
        {
            get
            {
                return children.Count;
            }
        }

        // Grau
        public int Degrees
        {
            get
            {
                return this.Indegrees + this.Outdegrees;
            }
        }

        // Folha (Leaf), Sorvedouro (no children)
        public bool IsSink
        {
            get
            {
                if (this.children.Count == 0)
                    return true;
                return false;
            }
        }

        // Fonte (sem pais)
        public bool IsSource
        {
            get
            {
                if (this.parents.Count == 0)
                    return true;
                return false;
            }
        }

        // Isolado (sem pais)
        public bool IsIsolated
        {
            get
            {
                if (this.parents.Count == 0 && this.children.Count == 0)
                    return true;
                return false;
            }
        }

        internal Vertex(Graph<T> graph, T entity, int internalId)
        {
            this.Entity = entity;            
            this.InternalId = internalId;
            this.parents = new HashSet<Vertex<T>>();
            this.children = new HashSet<Vertex<T>>();
            this.Graph = graph;
        }

        internal void AddIndegree(Vertex<T> parent)
        {
            this.parents.Add(parent);
            parent.children.Add(this);
        }

        #region Overrides

        public static bool operator ==(Vertex<T> a, T b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Vertex<T> a, T b)
        {
            return !Equals(a, b);
        }

        public static bool operator ==(Vertex<T> a, Vertex<T> b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Vertex<T> a, Vertex<T> b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) 
                return false;

            if (obj is T)
                return this.Entity.Equals(obj);
            else if (obj is Vertex<T>)
                return this.Entity.Equals((obj as Vertex<T>).Entity);

            return false;
        }

        public override int GetHashCode()
        {
            return this.Entity.GetHashCode();
        }

        public override string ToString()
        {
            if (Graph.Configuration.EntityToStringCallback != null)
                return Graph.Configuration.EntityToStringCallback(this.Entity);
            return Entity.ToString();
        }

        #endregion
    }
}