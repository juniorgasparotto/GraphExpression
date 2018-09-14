using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class GraphInfo<T>
    {
        #region Fields
        
        private List<Vertex<T>> vertexes;
        private List<Edge<T>> edges;
        private List<Path<T>> paths;
        //private bool? isHamiltonian = null;

        #endregion

        #region Public properties

        public IReadOnlyList<Edge<T>> Edges
        {
            get
            {
                return edges.AsReadOnly();
            }
        }

        public IReadOnlyList<Vertex<T>> Vertexes
        {
            get
            {
                return vertexes.AsReadOnly();
            }
        }

        public IReadOnlyList<Path<T>> Paths
        {
            get
            {
                return paths.AsReadOnly();
            }
        }

        #endregion

        public GraphInfo()
        {
            this.paths = new List<Path<T>>();
            this.vertexes = new List<Vertex<T>>();
            this.edges = new List<Edge<T>>();
        }

        public void SetGraphInfo(EntityItem<T> entityItem)
        {
            Vertex<T> vertex = FindVertexFirstOccurrence(entityItem.Entity);
            if (vertex == null)
            {
                long globalEntity = -1;

                if (entityItem.Entity != null)
                    globalEntity = VertexContainer<T>.Vertexes.IndexOf(entityItem.Entity);

                if (globalEntity == -1)
                {
                    globalEntity = VertexContainer<T>.Vertexes.Count;
                    VertexContainer<T>.Vertexes.Add(entityItem.Entity);
                }

                vertex = new Vertex<T>(entityItem.Entity, globalEntity);
                vertexes.Add(vertex);
            }

            vertex.CountVisited++;
            vertex.AddParent(entityItem.Parent); // Indegrees
            entityItem.Parent?.Vertex.AddChild(entityItem); // Outdegrees
            entityItem.Vertex = vertex;
            entityItem.Path = new Path<T>(entityItem);
            edges.Add(entityItem.Path.Edge);

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

        public void EndPath(Path<T> lastPath)
        {
            this.paths.Add(lastPath);
        }

        public bool ContainsGraph(GraphInfo<T> graph)
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

        //private bool ExistsVertexInPrevious(Iteration<T> endpoint, Vertex<T> vertex)
        //{
        //    if (this.currentPath != null)
        //        return this.currentPath.GetPrevious(endpoint).Where(f => f.Edge.Target?.AreEquals(vertex) == true).Any();

        //    return false;
        //}
    }
}