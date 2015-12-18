using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public class ArrayKey : IEnumerable<int>
    {
        private List<int> _indexes;

        public int this[int index]
        {
            get
            {
                return _indexes[index];
            }
        }

        public int Length
        {
            get
            {
                return _indexes.Count;
            }
        }

        public ArrayKey(params int[] indexes)
        {
            this._indexes = indexes.ToList();
        }

        public static bool operator ==(ArrayKey a, ArrayKey b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(ArrayKey a, ArrayKey b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var toCompare = obj as ArrayKey;
            
            if (this.Length != toCompare.Length)
                return false;

            for (var i = 0; i < _indexes.Count; i++)
            {
                if (_indexes[i] != toCompare[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this._indexes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._indexes.GetEnumerator();
        }

        public override string ToString()
        {
            var strIndexes = "";
            for (var i = 0; i < _indexes.Count; i++)
            {
                strIndexes += strIndexes == "" ? "" : ",";
                strIndexes += _indexes[i];
            }
            return strIndexes;
        }
    }
}
