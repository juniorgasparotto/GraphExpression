using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Examples
{
    public partial class Examples
    {
        [Action(Help = "Remove graph coexistents")]
        public void GraphRemoveCoexistents()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");

            A = A + B + (C + D);

            var graphs = new List<Graph<CircularEntity>>
            {
                A.AsExpression(f=>f.Children, e => e.Name).Graph,
                C.AsExpression(f=>f.Children, e => e.Name).Graph
            };

            System.Console.WriteLine($"-> A: HashCode: {graphs[0].GetHashCode()}");
            foreach (Path<CircularEntity> path in graphs[0].Paths)
                System.Console.WriteLine(path.ToString());

            System.Console.WriteLine($"-> B: HashCode: {graphs[1].GetHashCode()}");
            foreach (Path<CircularEntity> path in graphs[1].Paths)
                System.Console.WriteLine(path.ToString());

            var graphsNonDuplicates = graphs.RemoveCoexistents();
            foreach(var graph in graphsNonDuplicates)
            {
                System.Console.WriteLine($"-> HashCode not duplicates: {graph.GetHashCode()}");
            }
        }
    }
}
