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
        private Path<T> currentPath;
        private Path<T> lastPath;
        //private bool? isHamiltonian = null;

        #endregion

        #region Public properties

        public IEnumerable<Edge<T>> Edges
        {
            get
            {
                return edges.AsEnumerable();
            }
        }

        public IEnumerable<Vertex<T>> Vertexes
        {
            get
            {
                return vertexes.AsEnumerable();
            }
        }

        public IEnumerable<Path<T>> Paths
        {
            get
            {
                return paths.AsEnumerable();
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

        public PathItem<T> CreatePath(object iteration, int level, T entity, T entityParent)
        {
            Vertex<T> vertex = FindVertexFirstOccurrence(entity);
            if (vertex == null)
            {
                vertex = new Vertex<T>(entity);
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
            return AddInCurrentPath(iteration, level, edge);
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
            return this.vertexes.FirstOrDefault(e => e.Entity?.Equals(entity) == true);
        }

        private PathItem<T> AddInCurrentPath(object iteration, int level, Edge<T> edge)
        {
            if (this.currentPath == null)
            {
                this.currentPath = new Path<T>();

                if (lastPath != null && lastPath.Where(f => f.Iteration == iteration).Any())
                {
                    foreach (var path in lastPath)
                        if (path.Iteration == iteration)
                            break;
                        else
                            this.currentPath.Add(path);
                }
            }

            var pathItem = new PathItem<T>(iteration, edge, level);
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