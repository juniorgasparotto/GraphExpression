using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphExpression
{
    public class Path<T>
    {
        private readonly EntityItem<T> entityItem;
        private string identity;

        public IEnumerable<EntityItem<T>> Items { get; }

        public string Identity
        {
            get
            {
                if (identity == null)
                {
                    var items = Items;
                    foreach (var item in items)
                    {
                        var separator = (string.IsNullOrWhiteSpace(this.identity) ? "" : ".");
                        this.identity += $"{separator}[{item.Vertex.Id}]";
                    }
                }
                return identity;
            }
        }

        public PathType PathType
        {
            get
            {
                var items = Items;

                var first = items.First();
                var last = items.Last();
                if (first != last && first.AreEntityEquals(last) == true)
                {
                    return PathType.Circuit;
                }
                else
                {
                    EntityItem<T> lastIteration = null;
                    foreach (var current in this.Items)
                    {
                        if (lastIteration != null && current.AreEntityEquals(lastIteration) == true)
                            return PathType.Circle;
                        lastIteration = current;
                    }

                    return PathType.Simple;
                }
            }
        }

        public Path(EntityItem<T> entityItem)
        {            
            this.entityItem = entityItem;

            var parent = this.entityItem.Parent;
            if (parent != null)
            {
                this.Items = new List<EntityItem<T>>(parent.Path.Items)
                {
                    entityItem
                };
            }
            else
            {
                this.Items = new EntityItem<T>[] { entityItem };
            }
        }

        public bool ContainsPath(Path<T> pathTest)
        {
            if (this.Identity.Contains(pathTest.Identity))
                return true;
            return false;
        }

        public bool AreEquals(Path<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            return (this.Identity == obj.Identity);
        }

        #region Overrides

        public override string ToString()
        {
            var output = "";
            foreach (var item in Items)
            {
                var desc = $"[{item.ToString()}]";
                output += (output == "") ? desc : "." + desc;
            }
            return output;
        }

        #endregion
    }
}