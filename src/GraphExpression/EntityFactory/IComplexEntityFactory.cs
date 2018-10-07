using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public interface IEntityFactory
    {
        IEnumerable<Entity> Entities { get; }
        IReadOnlyList<string> Errors { get; }
        bool IgnoreErrors { get; set; }
        bool IsTyped { get; }
        List<object> ItemsDeserialize { get; set; }
        IReadOnlyDictionary<Type, Type> MapTypes { get; }
        Type RootType { get; }

        void AddError(string err);
    }
}