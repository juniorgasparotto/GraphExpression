using System;
using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Interface that should be used as a fabrication of complex entities 
    /// </summary>
    public interface IEntityFactory
    {
        /// <summary>
        /// All entities that was loaded during build process
        /// </summary>
        IReadOnlyList<Entity> Entities { get; }

        /// <summary>
        /// List of errors if exists
        /// </summary>
        IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// If TRUE, not throw in some situations
        /// </summary>
        bool IgnoreErrors { get; set; }

        /// <summary>
        /// TRUE when build a Entity with a specify type
        /// </summary>
        bool IsTyped { get; }

        /// <summary>
        /// Can be used to create a concrete instances from a interfaces or abstract classes
        /// </summary>
        IReadOnlyDictionary<Type, Type> MapTypes { get; }

        /// <summary>
        /// Root Type
        /// </summary>
        Type RootType { get; }

        /// <summary>
        /// List of TypeDiscovery
        /// </summary>
        List<ITypeDiscovery> TypeDiscovery { get; }

        /// <summary>
        /// List of ValueLoader
        /// </summary>
        List<IValueLoader> ValueLoader { get; }

        /// <summary>
        /// List of MemberInfoDiscovery
        /// </summary>
        List<IMemberInfoDiscovery> MemberInfoDiscovery { get; }

        /// <summary>
        ///  List of SetChildAction
        /// </summary>
        List<ISetChild> SetChildAction { get; }

        /// <summary>
        /// Method to add a error
        /// </summary>
        /// <param name="err">Error description</param>
        void AddError(string err);
    }
}