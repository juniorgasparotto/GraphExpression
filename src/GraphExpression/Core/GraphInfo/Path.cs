using GraphExpression.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Represents a path within the graph
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class Path<T>
    {
        private readonly EntityItem<T> entityItem;
        private string identity;

        /// <summary>
        /// All items from this path
        /// </summary>
        public IEnumerable<EntityItem<T>> Items { get; }

        /// <summary>
        /// Path ID. The ID is generated using the entity ID of the first entity from the path to the last one in the format: [1].[2].[3]
        /// </summary>
        public string Identity
        {
            get
            {
                if (identity == null)
                {
                    var items = Items;
                    foreach (var item in items)
                    {
                        var separator = (string.IsNullOrWhiteSpace(this.identity) ? "" : Constants.IDENTIFIER_SEPARATOR);
                        this.identity += $"{separator}[{item.Vertex.Id}]";
                    }
                }
                return identity;
            }
        }

        /// <summary>
        /// Type of path
        /// </summary>
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

        /// <summary>
        /// Creates a path for a given entity
        /// </summary>
        /// <param name="entityItem">Entity that will be part of the path</param>
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

        /// <summary>
        /// Checks whether a path exists within this path.
        /// </summary>
        /// <param name="pathTest">Path to verify</param>
        /// <returns>Return TRUE if exists</returns>
        public bool ContainsPath(Path<T> pathTest)
        {
            if (this.Identity.Contains(pathTest.Identity))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if two paths are equal
        /// </summary>
        /// <param name="obj">Path to verify</param>
        /// <returns>Return TRUE if are equals</returns>
        public bool AreEquals(Path<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            return (this.Identity == obj.Identity);
        }

        #region Overrides

        /// <summary>
        /// Path to string
        /// </summary>
        /// <returns>Path to string</returns>
        public override string ToString()
        {
            var output = "";
            foreach (var item in Items)
            {
                var desc = $"[{item.ToString()}]";
                output += (output == "") ? desc : Constants.IDENTIFIER_SEPARATOR + desc;
            }
            return output;
        }

        #endregion
    }
}