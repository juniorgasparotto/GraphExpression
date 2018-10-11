using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class to configure a complex expression build
    /// </summary>
    public class ComplexExpressionFactory
    {
        /// <summary>
        /// Entity Readers, with this feature you can create a custom read for a particular entity.
        /// </summary>
        public List<IEntityReader> Readers { get; private set; }

        /// <summary>
        /// Readers of members of entities, with this feature it is possible to create a customized reading of the members of a certain entity.
        /// </summary>
        public List<IMemberReader> MemberReaders { get; private set; }

        /// <summary>
        /// It delegates to the user which properties he would like to load for a particular type or all.
        /// </summary>
        public Func<object, IEnumerable<PropertyInfo>> GetProperties { get; set; }

        /// <summary>
        /// It delegates to the user which fields he would like to load for a particular type or all.
        /// </summary>
        public Func<object, IEnumerable<FieldInfo>> GetFields { get; set; }

        /// <summary>
        /// Create a complex expression factory to customize entity readers
        /// </summary>
        /// <param name="addDefaultConfig">If TRUE, the default Readers/MemberReaders/GetProperties/GetFields are setted</param>
        public ComplexExpressionFactory(bool addDefaultConfig = true)
        {
            if (addDefaultConfig)
            {
                // This list should be used using "LastOrDefault", to give the user the chance to 
                // replace one "reader" with another one using "Reader.Add()"
                // that is, the last "reader" is the most important.
                // Important:
                // 1) Array inherit IList: Must be sorted below IList
                // 2) ExpandoObject inherit IDictionary: Must be sorted below IDictionary
                Readers = new List<IEntityReader>
                {
                    new DefaultReader(),
                    new CollectionReader(),
                    new ArrayReader(),
                    new DictionaryReader(),
                    new DynamicReader(),
                };

                MemberReaders = new List<IMemberReader>
                {
                    new PropertyReader(),
                    new FieldReader()
                };

                GetProperties = (entity) => entity.GetType().GetProperties();
                GetFields = (entity) => entity.GetType().GetFields();
            }
        }

        /// <summary>
        /// Method to build and create a complex expression using the current configurations
        /// </summary>
        /// <param name="entityRoot">Root entity of expression. All entity paths will be traversed via reflection and each entity in that path will be an item in the expression.</param>
        /// <param name="deep">Determines whether the expression constructed will be a deep one, that is, if it will navigate objects that have already been navigated, except for cyclic references.</param>
        /// <returns>Return a complex expression</returns>
        public Expression<object> Build(object entityRoot, bool deep = false)
        {
            var expression = new Expression<object>(expr => new ComplexEntity(expr, entityRoot), (expr, e) => GetChildren(expr, e), deep);
            expression.DefaultSerializer = new ComplexEntityExpressionSerializer(expression);
            return expression;
        }

        private IEnumerable<EntityItem<object>> GetChildren(Expression<object> expression, EntityItem<object> parent)
        {
            var entityParent = parent?.Entity;

            if (entityParent == null)
                yield break;

            // Find the reader, the last "reader" is the most important
            var instanceReader = 
                Readers
                .Where(f => f.CanRead(this, entityParent))
                .LastOrDefault();

            if (instanceReader != null)
                foreach (var item in instanceReader.GetChildren(this, expression, entityParent))
                    yield return item;
        }
    }
}
