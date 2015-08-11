// This program prints out basic information about the crash dump specified.
//
// The platform must match what you are debugging, as we have to load the dac, a native dll.
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    // Or node
    public class Vertex<T>
    {
        private HashSet<Vertex<T>> predecessors;
        private HashSet<Vertex<T>> successors;

        public T Entity { get; private set; }
        public int InternalId { get; private set; }
        public Graph<T> Graph { get; private set; }

        public int CountVisited { get; internal set; }

        // parents
        public IEnumerable<Vertex<T>> Predecessors
        { 
            get 
            {
                return predecessors;
            }
        }

        // children
        public IEnumerable<Vertex<T>> Successors
        {
            get
            {
                return successors;
            }
        }

        // parent and children
        public IEnumerable<Vertex<T>> PredecessorsAndSuccessors
        {
            get
            {
                var list = this.predecessors.ToList();
                list.AddRange(successors);
                return list;
            }
        }

        // Grau de entrada (numero de parents)
        public int Indegrees
        {
            get
            {
                return predecessors.Count;
            }
        }

        // Grau de saída (numero de filhos)
        public int Outdegrees
        {
            get
            {
                return successors.Count;
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

        // Folha (Leaf), Sorvedouro (sem filhos)
        public bool IsSink
        {
            get
            {
                if (this.successors.Count == 0)
                    return true;
                return false;
            }
        }

        // Fonte (sem pais)
        public bool IsSource
        {
            get
            {
                if (this.predecessors.Count == 0)
                    return true;
                return false;
            }
        }

        // Isolado (sem pais)
        public bool IsIsolated
        {
            get
            {
                if (this.predecessors.Count == 0 && this.successors.Count == 0)
                    return true;
                return false;
            }
        }

        internal Vertex(Graph<T> graph, T entity, int internalId)
        {
            this.Entity = entity;            
            this.InternalId = internalId;
            this.predecessors = new HashSet<Vertex<T>>();
            this.successors = new HashSet<Vertex<T>>();
            this.Graph = graph;
        }

        internal void AddIndegree(Vertex<T> parent)
        {
            this.predecessors.Add(parent);
            parent.successors.Add(this);
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