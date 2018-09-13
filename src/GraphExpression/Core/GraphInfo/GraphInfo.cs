using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class GraphInfo<T>
    {
        private static List<T> globalIds = new List<T>();

        #region Fields
        
        private List<Vertex<T>> vertexes;
        private List<Edge<T>> edges;
        private List<Path<T>> paths;
        private Path<T> currentPath;
        private Path<T> lastPath;
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
            this.currentPath = new Path<T>();
            this.vertexes = new List<Vertex<T>>();
            this.edges = new List<Edge<T>>();
        }

        public PathItem<T> CreatePath(object parentIterationRef, int level, T entity, T entityParent)
        {
            Vertex<T> vertex = FindVertexFirstOccurrence(entity);
            if (vertex == null)
            {
                long globalEntity = -1;
                
                if (entity != null)
                    globalEntity = globalIds.IndexOf(entity);

                if (globalEntity == -1)
                {
                    globalEntity = globalIds.Count;
                    globalIds.Add(entity);
                }

                vertex = new Vertex<T>(entity, globalEntity);
                vertexes.Add(vertex);
            }

            vertex.CountVisited++;

            // Get parent vertex
            Vertex<T> vertexParent = null;
            if (entityParent != null)
            {
                vertexParent = vertexes.Where(f => f.Entity?.Equals(entityParent) == true).FirstOrDefault();
                vertex.AddIndegree(vertexParent);
            }

            var edge = edges.LastOrDefault(f => f.Source?.AreEquals(vertexParent) == true && f.Target?.AreEquals(vertex) == true);
            if (edge == null)
            {
                edge = new Edge<T>
                {
                    Source = vertexParent,
                    Target = vertex
                };

                //if (graph.Configuration.AssignEdgeWeightCallback != null)
                //    graph.Configuration.AssignEdgeWeightCallback(entity, entityParent);

                edges.Add(edge);
            }

            // create path item
            return AddInCurrentPath(parentIterationRef, level, edge);
        }

        public void EndPath()
        {
            this.paths.Add(currentPath);
            this.lastPath = this.currentPath;
            this.currentPath.SetType();
            this.currentPath = null;
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

        private PathItem<T> AddInCurrentPath(object parentIterationRef, int level, Edge<T> edge)
        {
            if (this.currentPath == null)
            {
                this.currentPath = new Path<T>();

                if (lastPath != null && lastPath.Items.Where(f => f.ParentIterationRef == parentIterationRef).Any())
                {
                    foreach (var item in lastPath.Items)
                        if (item.ParentIterationRef == parentIterationRef)
                            break;
                        else
                            this.currentPath.Add(item);
                }
            }

            var pathItem = new PathItem<T>(this.currentPath, parentIterationRef, edge, level);
            this.currentPath.Add(pathItem);
            return pathItem;
        }

        //private bool ExistsVertexInPrevious(Iteration<T> endpoint, Vertex<T> vertex)
        //{
        //    if (this.currentPath != null)
        //        return this.currentPath.GetPrevious(endpoint).Where(f => f.Edge.Target?.AreEquals(vertex) == true).Any();

        //    return false;
        //}
    }
}