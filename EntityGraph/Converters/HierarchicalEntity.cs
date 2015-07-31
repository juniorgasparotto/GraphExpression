using NCalc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace EntityGraph
{
    public class HierarchicalEntity
    {
        #region Fields

        private ListOfHierarchicalEntity parents;
        private ListOfHierarchicalEntity children;

        #endregion

        #region Properties

        public object Identity { get; private set; }

        public ListOfHierarchicalEntity Parents
        {
            get
            {
                return parents.ToListOfHierarchicalEntity();
            }
        }

        public ListOfHierarchicalEntity Children
        {
            get
            {
                return children.ToListOfHierarchicalEntity();
            }
        }

        #endregion

        #region Constructor

        public HierarchicalEntity(object identity)
        {
            this.Identity = identity;
            this.children = new ListOfHierarchicalEntity();
            this.parents = new ListOfHierarchicalEntity();
        }

        #endregion

        #region Control children

        public void Add(HierarchicalEntity obj)
        {
            //this.ValidateAdd(obj);
            this.children.Add(obj);
            obj.parents.Add(this);
        }

        public void Remove(HierarchicalEntity obj)
        {
            if (this.children.Remove(obj))
                obj.parents.Remove(this);
        }

        #endregion

        #region Methods

        public ListOfHierarchicalEntity Descendants()
        {
            var list = new ListOfHierarchicalEntity();
            var enumerable = this.children.Traverse(f => f.Children);
            foreach (var item in enumerable)
                list.Add(item);

            return list;
        }

        public ListOfHierarchicalEntity DescendantsAndSelf()
        {
            var list = new ListOfHierarchicalEntity();
            list.Add(this);
            list.AddRange(this.Descendants());
            return list;
        }

        public ListOfHierarchicalEntity Ancestors()
        {
            var list = new ListOfHierarchicalEntity();
            var enumerable = this.Parents.Traverse(f => f.Parents);
            foreach (var item in enumerable)
                list.Add(item);

            return list;
        }

        public ListOfHierarchicalEntity AncestorsAndSelf()
        {
            var list = new ListOfHierarchicalEntity();
            list.Add(this);
            list.AddRange(this.Ancestors());
            return list;
        }

        public void Validate()
        {
            // If any error occur (duplicate ID) the exception is fired
            this.DescendantsAndSelf();
        }

        public bool IdentityIsEquals(object name)
        {
            if (this.Identity.Equals(name))
                return true;

            return false;
        }
        
        #endregion

        #region Operators

        public static HierarchicalEntity operator +(HierarchicalEntity a, HierarchicalEntity b)
        {
            a.Add(b);
            return a;
        }

        public static HierarchicalEntity operator -(HierarchicalEntity a, HierarchicalEntity b)
        {
            a.Remove(b);
            return a;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return this.Identity.ToString();
        }

        #endregion
    }
}
