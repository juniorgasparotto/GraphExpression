using System.Collections.Generic;

namespace GraphExpression
{
    public class Vertex<T>
    {
        private List<EntityItem<T>> parents;
        private List<EntityItem<T>> children;

        public long Id { get; private set; }
        public T Entity { get; private set; }
        public int CountVisited { get; set; }

        public IReadOnlyList<EntityItem<T>> Parents
        { 
            get 
            {
                return parents;
            }
        }

        public IReadOnlyList<EntityItem<T>> Children
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

        public Vertex(T entity, long id)
        {
            this.Id = id;
            this.Entity = entity;
            this.parents = new List<EntityItem<T>>();
            this.children = new List<EntityItem<T>>();
        }

        public bool AreEquals(Vertex<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return this.Entity?.Equals(obj.Entity) == true;
        }

        public void AddParent(EntityItem<T> parent)
        {
            // if is Root parent is null
            if (parent != null)
                this.parents.Add(parent);
        }

        public void AddChild(EntityItem<T> child)
        {
            if (child != null)
                this.children.Add(child);
        }

        #region Overrides

        public override string ToString()
        {
            return Entity?.ToString();
        }

        #endregion
    }
}