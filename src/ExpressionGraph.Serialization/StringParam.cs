using NCalc;
using System;
using System.Collections.Generic;

namespace ExpressionGraph.Serialization
{
    internal class StringParam : IOperations
    {
        private HierarchicalEntity Object;
        public StringParam(HierarchicalEntity Object)
        {
            this.Object = Object;
        }

        #region Implements IOperations

        object IOperations.Add(object b)
        {
            var p = (StringParam)b;
            this.Object.Add(p.Object);
            return this;
        }

        object IOperations.Soustract(object b)
        {
            var p = (StringParam)b;
            this.Object.Remove(p.Object);
            return this;
        }

        object IOperations.Multiply(object b)
        {
            throw new NotImplementedException();
        }

        object IOperations.Divide(object b)
        {
            throw new NotImplementedException();
        }

        object IOperations.Modulo(object b)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
