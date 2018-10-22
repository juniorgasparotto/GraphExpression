using System.Collections.Generic;

namespace GraphExpression.Examples
{
    public class CircularEntity
    {
        public string Name { get; private set; }
        public List<CircularEntity> Children { get; } = new List<CircularEntity>();

        public CircularEntity(string identity) => this.Name = identity;

        public static CircularEntity operator +(CircularEntity a, CircularEntity b)
        {
            a.Children.Add(b);
            return a;
        }

        public static CircularEntity operator -(CircularEntity a, CircularEntity b)
        {
            a.Children.Remove(b);
            return a;
        }
    }
}
