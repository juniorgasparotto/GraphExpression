using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class Graph<T>
    {
        #region Fields
        
        private List<Vertex<T>> vertexes;
        private List<Edge<T>> edges;
        private List<Path<T>> paths;
        private Path<T> currentPath;
        private Path<T> lastPath;

        #endregion

        #region Public properties

        public Expression<T> Expression { get; private set; }
        public GraphConfiguration<T> Configuration { get; private set; }

        //public int CountIteration { get; private set; }
        //public string SequenceIteration { get; private set; }
        //private bool? isHamiltonian = null;
        //public bool IsHamiltonian
        //{
        //    get
        //    { 
        //        /// TODO
        //        return false;
        //    }
        //}
        
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

        private Graph(GraphConfiguration<T> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.paths = new List<Path<T>>();
            this.currentPath = new Path<T>();
            this.vertexes = new List<Vertex<T>>();
            this.edges = new List<Edge<T>>();
            this.Expression = new Expression<T>();
            this.Configuration = configuration;
        }

        // Not accept multigraph
        public static List<Graph<T>> ToGraphs(IEnumerable<T> source, Func<T, List<T>> childrenCallback, GraphConfiguration<T> configuration = null)
        {
            // set configuration or default
            configuration = configuration ?? new GraphConfiguration<T>();

            Graph<T> graph = null;
            var graphs = new List<Graph<T>>();

            var iteration = new Iteration<T>()
            {
                Enumerator = source.Distinct().GetEnumerator(),
                Level = 1,
                CountIteration = 0,
            };

            var iterations = new List<Iteration<T>>();
            iterations.Add(iteration);

            while (true)
            {
                while (iteration.Enumerator.MoveNext())
                {
                    // New graph
                    if (iteration.Level == 1)
                    {
                        graph = new Graph<T>(configuration);
                        graphs.Add(graph);
                    }

                    // Count iterations
                    iteration.CountIteration++;

                    var entity = iteration.Enumerator.Current;
                    var entityParent = iteration.IterationParent != null ? iteration.IterationParent.Enumerator.Current : default(T);

                    // Create or get current vertex
                    Vertex<T> vertex = graph.vertexes.Where(f=> f.Entity.Equals(entity)).FirstOrDefault();
                    if (vertex == null)
                    {
                        vertex = new Vertex<T>(graph, entity, graph.vertexes.Count + 1);
                        graph.vertexes.Add(vertex);
                    }

                    vertex.CountVisited++;

                    // Get parent vertex
                    Vertex<T> vertexParent = null;
                    if (entityParent != null)
                    {
                        vertexParent = graph.vertexes.Where(f => f.Entity.Equals(entityParent)).FirstOrDefault();
                        vertex.AddIndegree(vertexParent);
                    }

                    var edge = graph.edges.LastOrDefault(f => f.Source == vertexParent && f.Target == vertex);
                    if (edge == null)
                    {
                        edge = new Edge<T>();
                        edge.Source = vertexParent;
                        edge.Target = vertex;

                        if (graph.Configuration.AssignEdgeWeightCallback != null)
                            graph.Configuration.AssignEdgeWeightCallback(entity, entityParent);

                        graph.edges.Add(edge);
                    }
                    
                    // create path item
                    var pathItem = graph.AddInCurrentPath(iteration, edge);
                     
                    // Prevent recursion, infinite loop. eg: "A + B + [A]" where [A] already exists in path
                    var exists = graph.ExistsVertexInPrevious(iteration, vertex);

                    List<T> children = null;

                    // Get vertexes children if vertex does not exists in current path
                    if (!exists)
                        children = childrenCallback(entity);

                    var hasChildren = children != null && children.Count > 0;

                    // Specify sequencial iteration
                    //graph.SequenceIteration += (string.IsNullOrWhiteSpace(graph.SequenceIteration) ? "" : ".") + "[" + vertex.ToString() + "]";

                    // if exists any token, add "+" in sequence
                    //var parentLevel = iteration.IterationParent != null ? iteration.IterationParent.Level : 1;

                    //if (iteration.IterationParent != null)
                    //    graph.Expression.Add(new GraphExpressionItemPlus<T>(parentLevel));

                    if (hasChildren)
                    {
                        // add parenthesis "(A" because exists children or when is the root level and encloseRootTokenInParenthesis = true
                        var addParenthesis = iteration.Level > 1;

                        if (addParenthesis)
                            graph.Expression.OpenParenthesis();

                        graph.Expression.AddItem(vertex.Entity);

                        iteration = new Iteration<T>()
                        {
                            Enumerator = children.GetEnumerator(),
                            Level = iteration.Level + 1,
                            EntityRootOfTheIterationForDebug = iteration.Enumerator.Current,
                            IterationParent = iteration,
                            HasOpenParenthesis = addParenthesis
                        };

                        iterations.Add(iteration);
                    }
                    else
                    {
                        graph.Expression.AddItem(vertex.Entity);
                        graph.ClosePath();
                    }
                }

                if (iteration.HasOpenParenthesis)
                    graph.Expression.CloseParenthesis();

                // Remove iteration because is empty
                iterations.Remove(iteration);

                if (iterations.Count == 0)
                    break;

                iteration = iterations.LastOrDefault();
            }

            return graphs;
        }

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

        private PathItem<T> AddInCurrentPath(Iteration<T> iteration, Edge<T> edge)
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

            var pathItem = new PathItem<T>(iteration, edge, iteration.Level);
            this.currentPath.Add(pathItem);
            return pathItem;
        }

        private void ClosePath()
        {
            this.paths.Add(currentPath);
            this.lastPath = this.currentPath;
            this.currentPath.SetType();
            this.currentPath = null;
        }

        private bool ExistsVertexInPrevious(Iteration<T> endpoint, Vertex<T> vertex)
        {
            if (this.currentPath != null)
                return this.currentPath.GetPrevious(endpoint).Where(f => f.Edge.Target == vertex).Any();

            return false;
        }

        public override string ToString()
        {
            return this.Expression.ToString();
        }
    }
}