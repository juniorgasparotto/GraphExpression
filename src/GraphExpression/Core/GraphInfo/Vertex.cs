using System.Collections.Generic;

namespace GraphExpression
{
    public class Vertex<T>
    {
        private HashSet<Vertex<T>> parents;
        private HashSet<Vertex<T>> children;

        public T Entity { get; private set; }
        public int CountVisited { get; internal set; }

        public IEnumerable<Vertex<T>> Parents
        { 
            get 
            {
                return parents;
            }
        }

        public IEnumerable<Vertex<T>> Children
        {
            get
            {
                return children;
            }
        }

        // Indegrees (number of parents) - (Grau de entrada/numero de pais)
        public int Indegrees
        {
            get
            {
                return parents.Count;
            }
        }

        // Outdegrees (number of children) - (Grau de saída/numero de filhos)
        public int Outdegrees
        {
            get
            {
                return children.Count;
            }
        }

        // Degrees (Grau em português)
        public int Degrees
        {
            get
            {
                return this.Indegrees + this.Outdegrees;
            }
        }

        // Leaf (Folha em português), Sorvedouro (no children)
        public bool IsSink
        {
            get
            {
                if (this.children.Count == 0)
                    return true;
                return false;
            }
        }

        // Source (no parents/root)
        public bool IsSource
        {
            get
            {
                if (this.parents.Count == 0)
                    return true;
                return false;
            }
        }

        // Isolated (no parents and no children)
        public bool IsIsolated
        {
            get
            {
                if (this.parents.Count == 0 && this.children.Count == 0)
                    return true;
                return false;
            }
        }

        internal Vertex(T entity)
        {
            this.Entity = entity;            
            this.parents = new HashSet<Vertex<T>>();
            this.children = new HashSet<Vertex<T>>();
        }

        public bool AreEquals(Vertex<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return this.Entity?.Equals(obj.Entity) == true;
        }

        internal void AddIndegree(Vertex<T> parent)
        {
            // if is Root parent is null
            if (parent != null)
            {
                this.parents.Add(parent);
                parent.children.Add(this);
            }
        }

        #region Overrides

        public override string ToString()
        {
            return Entity?.ToString();
        }

        #endregion
    }
}