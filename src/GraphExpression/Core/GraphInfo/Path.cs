using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphExpression
{
    public class Path<T>
    {
        private readonly EntityItem<T> entityItem;
        private IEnumerable<EntityItem<T>> items;
        private string identity;

        public Edge<T> Edge { get; private set; }

        public IEnumerable<EntityItem<T>> Items
        {
            get
            {
                if (items == null)
                {
                    var items = new Stack<EntityItem<T>>();
                    items.Push(entityItem);

                    var parent = entityItem.Parent;
                    while (parent != null)
                    {
                        items.Push(parent);
                        parent = parent.Parent;
                    }
                    this.items = items;
                }
                return items;
            }
        }

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
                
                if (items.First().AreEntityEquals(items.Last()) == true)
                {
                    return PathType.Circuit;
                }
                else
                {
                    EntityItem<T> last = null;
                    foreach (var current in this.items)
                    {
                        if (last != null && current.AreEntityEquals(last) == true)
                            return PathType.Circle;
                        last = current;
                    }

                    return PathType.Simple;
                }
            }
        }

        public Path(EntityItem<T> entityItem)
        {
            this.Edge = new Edge<T>(entityItem.Parent, entityItem, 0);
            this.entityItem = entityItem;
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

        public string ToString(bool showEntityDesc)
        {
            if (showEntityDesc)
            {
                var output = "";
                foreach (var item in Items)
                {
                    var desc = $"[{item.ToString()}]";
                    output += (output == "") ? desc : "." + desc;
                }
                return output;
            }
            else
            {
                return this.Identity;
            }
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        #endregion
    }
}