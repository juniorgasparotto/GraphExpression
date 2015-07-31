using NCalc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntityGraph
{
    public class ListOfHierarchicalEntity : List<HierarchicalEntity>
    {
        private bool ValidateAdd(HierarchicalEntity obj)
        {
            var exists = this.Exists(f => f == obj);
            if (!exists)
            {
                if (this.Exists(f => f.Identity == obj.Identity))
                    throw new EntityAlreadyExistsException(string.Format("Object '{0}' already exists in the list", obj.Identity));
            }

            return !exists;
        }

        public bool ContainsIdentity(object id)
        {
            return this.Exists(f => f.IdentityIsEquals(id));
        }

        public HierarchicalEntity GetByIdentity(object id)
        {
            return this.FirstOrDefault(f => f.IdentityIsEquals(id));
        }
        
        public ListOfHierarchicalEntity DescendantsOfAll()
        {
            var list = new ListOfHierarchicalEntity();
            foreach (var item in this)
                list.AddRange(item.DescendantsAndSelf());

            return list;
        }

        public ListOfHierarchicalEntity Roots()
        {
            var list = new ListOfHierarchicalEntity();
            var isChild = false;
            foreach (var parent in this)
            {
                foreach (var child in this)
                {
                    if (child.Parents.Contains(parent))
                    {
                        isChild = true;
                        break;
                    }
                }

                if (isChild)
                    list.Add(parent);
            }

            return list;
        }

        #region List override methods

        public new void Add(HierarchicalEntity obj)
        {
            if (this.ValidateAdd(obj))
                base.Add(obj);
        }

        public new void AddRange(IEnumerable<HierarchicalEntity> list)
        {
            foreach (var item in list)
                this.Add(item);
        }

        public new void Insert(int index, HierarchicalEntity item)
        {
            if (this.ValidateAdd(item))
                base.Insert(index, item);
        }

        public new void InsertRange(int index, IEnumerable<HierarchicalEntity> list)
        {
            foreach (var item in list)
                this.Insert(index, item);
        }

        public new HierarchicalEntity this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                this.Add(value);
            }
        }

        public new void CopyTo(HierarchicalEntity[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length; i++)
                this.Add(array[i]);
        }

        #endregion
    }
}
