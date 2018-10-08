using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public interface IEntityFactory
    {
        IReadOnlyList<Entity> Entities { get; }
        IReadOnlyList<string> Errors { get; }
        bool IgnoreErrors { get; set; }
        bool IsTyped { get; }        
        IReadOnlyDictionary<Type, Type> MapTypes { get; }
        Type RootType { get; }
        void AddError(string err);

        List<ITypeDiscovery> TypeDiscovery { get; }
        List<IValueLoader> ValueLoader { get; }
        List<IMemberInfoDiscovery> MemberInfoDiscovery { get; }
        List<ISetChild> SetChildAction { get; }
    }
}