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

        public PathItem<T> CreatePath(object parentIterationRef, int level, EntityItem<T> entityItem, EntityItem<T> entityItemParent)
        {
            Vertex<T> vertex = FindVertexFirstOccurrence(entityItem.Entity);
            if (vertex == null)
            {
                long globalEntity = -1;
                
                if (entityItem.Entity != null)
                    globalEntity = globalIds.IndexOf(entityItem.Entity);

                if (globalEntity == -1)
                {
                    globalEntity = globalIds.Count;
                    globalIds.Add(entityItem.Entity);
                }

                vertex = new Vertex<T>(entityItem.Entity, globalEntity);
                vertexes.Add(vertex);
            }

            vertex.CountVisited++;

            // Set parent (indegree) to current entity
            if (entityItemParent != null)
                vertex.AddParent(entityItem, entityItemParent);

            entityItem.Vertex = vertex;
            entityItem.Edge = new Edge<T>(entityItemParent, entityItem, 0);            
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
            return AddInCurrentPath(parentIterationRef, level, entityItem, vertex.Id);
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

        private PathItem<T> AddInCurrentPath(object parentIterationRef, int level, EntityItem<T> entityItem, long id)
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

            var pathItem = new PathItem<T>(this.currentPath, id, parentIterationRef, entityItem, level);
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