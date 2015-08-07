using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class Graph<T>
    {
        private List<Vertex<T>> vertexes;
        private List<Edge<T>> edges;
        private List<Path<T>> paths;
        private Path<T> currentPath;
        private Path<T> lastPath;

        public int CountIteration { get; private set; }
        public string SequenceIteration { get; private set; }
        public List<TokenValue> Tokens = new List<TokenValue>();

        //private bool? isHamiltonian = null;
        public bool IsHamiltonian
        {
            get
            { 
                //if (isHamiltonian == null)
                //{
                //    if (this.vertexes.Count == this.vertexes.Sum(f => f.CountVisited))
                //        isHamiltonian = true;
                //    else
                //        isHamiltonian = false;
                //}

                //return isHamiltonian.Value;

                /// TODO
                return false;
            }
        }
        
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

        private Graph()
        {
            this.paths = new List<Path<T>>();
            this.currentPath = new Path<T>();
            this.vertexes = new List<Vertex<T>>();
            this.edges = new List<Edge<T>>();
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

        // Not accept multigraph
        public static List<Graph<T>> ToGraphs(IEnumerable<T> source, Func<T, List<T>> nextVertexCallback, Action<Edge<T>> edgeCreatedCallback = null, bool encloseRootTokenInParenthesis = false)
        {
            Graph<T> graph = new Graph<T>();
            var graphs = new List<Graph<T>>();
            //graphs.Add(graph);

            var iteration = new Iteration<T>()
            {
                Enumerator = source.Distinct().GetEnumerator(),
                Level = 0,
                CountIteration = 0,
                //Finished = false,
                //CountItems = source.Count()
            };

            var iterations = new List<Iteration<T>>();
            iterations.Add(iteration);
            
            //var countIterationForAll = 0;
            //var allCountAlreadyFinishedIterationForAll = 0;

            while (true)
            {
                while (iteration.Enumerator.MoveNext())
                {
                    // New graph
                    if (iteration.Level == 0)
                    {
                        graph = new Graph<T>();
                        graphs.Add(graph);
                    }

                    // Count iterations
                    iteration.CountIteration++;
                    graph.CountIteration++;

                    var data = iteration.Enumerator.Current;
                    var dataParent = iteration.IterationParent != null ? iteration.IterationParent.Enumerator.Current : default(T);

                    // Create or get current vertex
                    Vertex<T> vertex = graph.vertexes.Where(f=> f.Data.Equals(data)).FirstOrDefault();
                    if (vertex == null)
                    {
                        vertex = new Vertex<T>(data, graph.vertexes.Count + 1);
                        graph.vertexes.Add(vertex);
                    }

                    vertex.CountVisited++;

                    // Get parent vertex
                    Vertex<T> vertexParent = null;
                    if (dataParent != null)
                    {
                        vertexParent = graph.vertexes.Where(f => f.Data.Equals(dataParent)).FirstOrDefault();
                        vertex.AddIndegree(vertexParent);
                    }

                    var edge = graph.edges.LastOrDefault(f => f.Source == vertexParent && f.Target == vertex);
                    if (edge == null)
                    {
                        edge = new Edge<T>();
                        edge.Source = vertexParent;
                        edge.Target = vertex;
                        
                        if (edgeCreatedCallback != null)
                            edgeCreatedCallback(edge);

                        graph.edges.Add(edge);
                    }
                    
                    graph.AddInCurrentPath(iteration, edge);
                     
                    // Prevent recursion, infinite loop. eg: "A + B + [A]" where [A] already exists in path
                    var exists = graph.ExistsVertexInPrevious(iteration, vertex);

                    List<T> nexts = null;

                    // Get nexts vertexes if vertex does not exists in current path
                    if (!exists)
                        nexts = nextVertexCallback(data);

                    var hasNext = nexts != null && nexts.Count > 0;

                    // Specify sequencial iteration
                    graph.SequenceIteration += (string.IsNullOrWhiteSpace(graph.SequenceIteration) ? "" : ".") + "[" + vertex.ToString() + "]";

                    // if exists any token, add "+" in sequence
                    if (graph.Tokens.Count > 0)
                        graph.Tokens.Add(TokenValuePlus.Instance);

                    if (hasNext)
                    {
                        // add tokens "(A" with parenthesis (when encloseRootTokenInParenthesis = true)
                        // because exists children
                        var addParenthesis = true;

                        // can't add in root when encloseRootTokenInParenthesis = false
                        if (!encloseRootTokenInParenthesis && iteration.Level == 0)
                            addParenthesis = false;

                        if (addParenthesis)
                            graph.Tokens.Add(TokenValueOpenParenthesis.Instance);

                        graph.Tokens.Add(new TokenValue(vertex, null));

                        iteration = new Iteration<T>()
                        {
                            Enumerator = nexts.GetEnumerator(),
                            Level = iteration.Level + 1,
                            DataIterationForDebug = iteration.Enumerator.Current,
                            IterationParent = iteration,
                            HasOpenParenthesis = addParenthesis
                        };

                        iterations.Add(iteration);
                    }
                    else
                    {
                        // add single token "A"
                        graph.Tokens.Add(new TokenValue(vertex, null));
                        graph.ClosePath();
                    }
                }

                if (iteration.HasOpenParenthesis)
                    graph.Tokens.Add(TokenValueCloseParenthesis.Instance);

                // Remove iteration because is empty
                iterations.Remove(iteration);

                if (iterations.Count == 0)
                    break;

                iteration = iterations.LastOrDefault();
            }

            return graphs;
        }

        private void EndIteration(Vertex<T> vertex, List<T> nexts)
        {
            this.Tokens.Add(TokenValueCloseParenthesis.Instance);
        }

        private void AddInCurrentPath(Iteration<T> iteration, Edge<T> edge)
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

            this.currentPath.Add(new PathItem<T>(iteration, edge, iteration.Level));
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
            return this.SequenceIteration;
        }
    }
}