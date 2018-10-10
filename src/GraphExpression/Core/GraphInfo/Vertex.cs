using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// It represents a vertex in the concept of graphs.
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class Vertex<T>
    {
        private List<EntityItem<T>> parents;
        private List<EntityItem<T>> children;

        /// <summary>
        /// Vertex ID
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Real entity
        /// </summary>
        public T Entity { get; private set; }

        /// <summary>
        /// Determines the number of occurrences of an entity within the graph
        /// </summary>
        public int CountVisited { get; set; }

        /// <summary>
        /// Contains all the parents of an entity within the graph
        /// </summary>
        public IReadOnlyList<EntityItem<T>> Parents
        { 
            get 
            {
                return parents;
            }
        }

        /// <summary>
        /// Contains all the child of an entity within the graph
        /// </summary>
        public IReadOnlyList<EntityItem<T>> Children
        {
            get
            {
                return children;
            }
        }

        /// <summary>
        /// Indegrees (number of parents) - (Grau de entrada/numero de pais)
        /// </summary>
        public int Indegrees
        {
            get
            {
                return parents.Count;
            }
        }

        /// <summary>
        /// Outdegrees (number of children) - (Grau de saída/numero de filhos) 
        /// </summary>
        public int Outdegrees
        {
            get
            {
                return children.Count;
            }
        }

        /// <summary>
        /// Degrees (Grau em português) 
        /// </summary>
        public int Degrees
        {
            get
            {
                return this.Indegrees + this.Outdegrees;
            }
        }

        /// <summary>
        /// Leaf (Folha em português), Sorvedouro (no children) 
        /// </summary>
        public bool IsSink
        {
            get
            {
                if (this.children.Count == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Source (no parents/root)  
        /// </summary>
        public bool IsSource
        {
            get
            {
                if (this.parents.Count == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Isolated (no parents and no children)  
        /// </summary>
        public bool IsIsolated
        {
            get
            {
                if (this.parents.Count == 0 && this.children.Count == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Creates a new vertex for an entity
        /// </summary>
        /// <param name="entity">Real entity</param>
        /// <param name="id">Entity ID</param>
        public Vertex(T entity, long id)
        {
            this.Id = id;
            this.Entity = entity;
            this.parents = new List<EntityItem<T>>();
            this.children = new List<EntityItem<T>>();
        }

        /// <summary>
        /// Verify if two vertex are equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AreEquals(Vertex<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return this.Entity?.Equals(obj.Entity) == true;
        }

        /// <summary>
        /// Add a new parent in this vertex
        /// </summary>
        /// <param name="parent">Parent to add</param>
        public void AddParent(EntityItem<T> parent)
        {
            // if is Root parent is null
            if (parent != null)
                this.parents.Add(parent);
        }

        /// <summary>
        /// Add a new child in this vertex
        /// </summary>
        /// <param name="child">Child to add</param>
        public void AddChild(EntityItem<T> child)
        {
            if (child != null)
                this.children.Add(child);
        }

        #region Overrides

        /// <summary>
        ///  Vertex to string
        /// </summary>
        /// <returns>Vertex to string</returns>
        public override string ToString()
        {
            return Entity?.ToString();
        }

        #endregion
    }
}