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
        public void GraphInfo()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");

            A = A + B + (C + D);
            var expressionA = A.AsExpression(f => f.Children, e => e.Name);

            foreach (Edge<CircularEntity> edge in expressionA.Graph.Edges)
                System.Console.WriteLine(edge.ToString());

            foreach (Path<CircularEntity> path in expressionA.Graph.Paths)
                System.Console.WriteLine(path.ToString());

            foreach (EntityItem<CircularEntity> item in expressionA)
                System.Console.WriteLine($"{item.ToString()} => {item.Path}");
        }
    }
}
