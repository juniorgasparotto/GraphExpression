using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public interface IGraphExpressionPosition
    {
        int LevelInExpression { get; }
    }

    public class GraphExpressionItem<T> : IGraphExpressionPosition
    {
        public PathItem<T> PathItem { get; private set; }
        public int LevelInExpression { get; private set; }

        public GraphExpressionItem(PathItem<T> pathItem, int level)
        {
            this.PathItem = pathItem;
            this.LevelInExpression = level;
        }

        public override string ToString()
        {
            return this.PathItem.Edge.Target.ToString();
        }
    }

    public class GraphExpressionItemPlus : IGraphExpressionPosition
    {
        public int LevelInExpression { get; private set; }

        public GraphExpressionItemPlus(int level)
        {
            this.LevelInExpression = level;
        }

        public override string ToString()
        {
            return " + ";
        }
    }

    public class GraphExpressionItemOpenParenthesis : IGraphExpressionPosition
    {
        public int LevelInExpression { get; private set; }

        public GraphExpressionItemOpenParenthesis(int level)
        {
            this.LevelInExpression = level;
        }

        public override string ToString()
        {
            return "(";
        }
    }

    public class GraphExpressionItemCloseParenthesis : IGraphExpressionPosition
    {
        public int LevelInExpression { get; private set; }

        public GraphExpressionItemCloseParenthesis(int level)
        {
            this.LevelInExpression = level;
        }

        public override string ToString()
        {
            return ")";
        }
    }
}
