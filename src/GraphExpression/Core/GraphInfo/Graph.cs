using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Class used to store some graph information inside an expression
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class Graph<T>
    {
        #region Fields
        
        private List<Vertex<T>> vertexes;
        private List<Edge<T>> edges;
        private List<Path<T>> paths;
        //private bool? isHamiltonian = null;

        #endregion

        #region Public properties
       
        /// <summary>
        /// List of edges
        /// </summary>
        public IReadOnlyList<Edge<T>> Edges
        {
            get
            {
                return edges.AsReadOnly();
            }
        }

        /// <summary>
        /// List of vertexes
        /// </summary>
        public IReadOnlyList<Vertex<T>> Vertexes
        {
            get
            {
                return vertexes.AsReadOnly();
            }
        }

        /// <summary>
        /// Return only the last paths of the graph.
        /// </summary>
        public IReadOnlyList<Path<T>> Paths
        {
            get
            {
                return paths.AsReadOnly();
            }
        }

        #endregion

        /// <summary>
        /// Create a instance of graph info
        /// </summary>
        public Graph()
        {
            this.paths = new List<Path<T>>();
            this.vertexes = new List<Vertex<T>>();
            this.edges = new List<Edge<T>>();
        }

        /// <summary>
        /// Method used to create new graph information. Must be used during expression creation.
        /// </summary>
        /// <param name="entityItem">EntityItem to generate a new graph info</param>
        public void SetGraphInfo(EntityItem<T> entityItem)
        {
            Vertex<T> vertex = FindVertexFirstOccurrence(entityItem.Entity);
            if (vertex == null)
            {
                VertexContainer<T>.EntityId globalEntity = null;

                if (entityItem.Entity != null)
                    globalEntity = VertexContainer<T>.GetEntityId(entityItem.Entity);

                if (globalEntity == null)
                    globalEntity = VertexContainer<T>.Add(entityItem.Entity);

                vertex = new Vertex<T>(entityItem.Entity, globalEntity.Id);
                vertexes.Add(vertex);
            }

            vertex.CountVisited++;
            vertex.AddParent(entityItem.Parent); // Indegrees
            entityItem.Parent?.Vertex.AddChild(entityItem); // Outdegrees
            entityItem.Vertex = vertex;
            entityItem.Edge = new Edge<T>(entityItem.Parent, entityItem, 0);
            entityItem.Path = new Path<T>(entityItem);
            edges.Add(entityItem.Edge);

            //var edge = edges.LastOrDefault(f => f.Source?.AreEquals(vertexParent) == true && f.Target?.AreEquals(vertex) == true);
            //if (edge == null)
            //{
            //    edge = new Edge<T>
            //    {
            //        Source = vertexParent,
            //        Target = vertex
            //    };

            //    //if (graph.Configuration.AssignEdgeWeightCallback != null)
            //    //    graph.Configuration.AssignEdgeWeightCallback(entity, entityParent);

            //    edges.Add(edge);
            //}

            // create path item
            //this.AddInCurrentPath(entityItem);;            
        }

        /// <summary>
        /// A path must be created in the "SetGraphInfo" method and closed with this method when an entity reaches its limit within the graph. Must be used during expression creation
        /// </summary>
        /// <param name="lastPath">Path to end</param>
        public void EndPath(Path<T> lastPath)
        {
            this.paths.Add(lastPath);
        }

        /// <summary>
        /// Check if the parameter graph existis in current graph
        /// </summary>
        /// <param name="graph">Graph to verify</param>
        /// <returns>Return TRUE if contain</returns>
        public bool ContainsGraph(Graph<T> graph)
        {
            var countExist = 0;
            foreach (var pathTest in graph.paths)
            {
                if (this.paths.Exists(f => f.ContainsPath(pathTest)))
                    countExist++;
            }

            if (countExist == graph.paths.Count)
                return true;

            return false;
        }

        private Vertex<T> FindVertexFirstOccurrence(T entity)
        {
            return vertexes.FirstOrDefault(e => e.Entity?.Equals(entity) == true);
        }

        //public void EndPath(Path<T> lastPath)
        //{
            //this.paths.Add(currentPath);
            //this.lastPath = this.currentPath;
            //this.currentPath.SetType();
            //this.currentPath = null;
        //}

        //private void AddInCurrentPath(EntityItem<T> entityItem)
        //{
        //    if (this.currentPath == null)
        //    {
        //        this.currentPath = new Path<T>();

        //        if (lastPath != null && lastPath.Items.Where(f => f.ParentIterationRef == entityItem.ParentIterationRef).Any())
        //        {
        //            foreach (var item in lastPath.Items)
        //                if (item.ParentIterationRef == entityItem.ParentIterationRef)
        //                    break;
        //                else
        //                    this.currentPath.Add(item);
        //        }
        //    }

        //    this.currentPath.Add(entityItem);
        //}
    }
}