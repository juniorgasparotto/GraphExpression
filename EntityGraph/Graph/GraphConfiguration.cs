using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public delegate decimal AssignEdgeWeightDelegate<T>(T entity, T entityParent);
    public delegate string EntityToStringDelegate<T>(T entity);

    public class GraphConfiguration<T>
    {
        public AssignEdgeWeightDelegate<T> AssignEdgeWeightCallback { get; private set; }
        public EntityToStringDelegate<T> EntityToStringCallback { get; private set; }
        public bool EncloseRootTokenInParenthesis { get; private set; }

        public GraphConfiguration
        (
            AssignEdgeWeightDelegate<T> assignEdgeWeightCallback = null,
            EntityToStringDelegate<T> entityToStringCallback = null,
            bool encloseRootTokenInParenthesis = false
        )
        {
            this.AssignEdgeWeightCallback = assignEdgeWeightCallback;
            this.EntityToStringCallback = entityToStringCallback;
            this.EncloseRootTokenInParenthesis = encloseRootTokenInParenthesis;
        }
    }
}