using NCalc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace ExpressionGraph
{
    public class HierarchicalEntity
    {
        #region Fields

        private List<HierarchicalEntity> parents;
        private List<HierarchicalEntity> children;

        #endregion

        #region Properties

        public object Identity { get; private set; }

        public IEnumerable<HierarchicalEntity> Parents
        {
            get
            {
                return parents;
            }
        }

        public IEnumerable<HierarchicalEntity> Children
        {
            get
            {
                return children;
            }
        }

        #endregion

        #region Constructor

        public HierarchicalEntity(object identity)
        {
            this.Identity = identity;
            this.children = new List<HierarchicalEntity>();
            this.parents = new List<HierarchicalEntity>();
        }

        #endregion

        #region Control children

        public void Add(HierarchicalEntity obj)
        {
            this.children.Add(obj);
            obj.parents.Add(this);
        }

        public void Remove(HierarchicalEntity obj)
        {
            if (this.children.Remove(obj))
                obj.parents.Remove(this);
        }

        #endregion

        public bool IdentityIsEquals(object name)
        {
            if (this.Identity.Equals(name))
                return true;

            return false;
        }

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
