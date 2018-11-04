using GraphExpression.Utils;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Standard class for creating expression serializers
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public abstract class ExpressionSerializerBase<T> : ISerialize<T>
    {
        private IValueFormatter valueFormatter;
        private readonly Expression<T> expression;

        /// <summary>
        /// Determines whether there will be parentheses involving the root entity
        /// </summary>
        public bool EncloseParenthesisInRoot { get; set; }

        /// <summary>
        /// Determines whether entities with valid names will always be enclosed in quote. Otherwise, entities with valid names will not be enclosed in quote.
        /// </summary>
        public bool ForceQuoteEvenWhenValidIdentified { get; set; }

        /// <summary>
        /// Default formatter
        /// </summary>
        public IValueFormatter ValueFormatter
        {
            get
            {
                if (valueFormatter == null)
                    valueFormatter = new DefaultValueFormatter();
                return valueFormatter;
            }
            set
            {
                valueFormatter = value;
            }
        }

        /// <summary>
        /// Constructor obrigatory for child classes 
        /// </summary>
        /// <param name="expression">Expression to serialize</param>
        public ExpressionSerializerBase(Expression<T> expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Serialize a expression to string
        /// </summary>
        /// <returns>Expression as string</returns>
        public virtual string Serialize()
        {
            var output = "";
            expression.IterationAll
                (
                    itemBeginGroupExpression =>
                    {
                        if (itemBeginGroupExpression.IsRoot)
                        {
                            if (EncloseParenthesisInRoot)
                                output = "(";

                            output += GetEnclosedValue(SerializeItem(itemBeginGroupExpression));
                        }
                        else
                        {
                            output += " + ";
                            if (itemBeginGroupExpression.IsFirstInParent)
                                output += "(";

                            output += GetEnclosedValue(SerializeItem(itemBeginGroupExpression));
                        }
                    },
                    itemEndGroupExpression =>
                    {
                        if (!itemEndGroupExpression.IsRoot || (itemEndGroupExpression.IsRoot && EncloseParenthesisInRoot))
                            output += ")";
                    }
                );

            return output;
        }

        /// <summary>
        /// Serialize a unique EntityItem
        /// </summary>
        /// <param name="item">EntityItem to serialize</param>
        /// <returns>Entity item as string</returns>
        public abstract string SerializeItem(EntityItem<T> item);

        private string GetEnclosedValue(string value)
        {
            if (   ForceQuoteEvenWhenValidIdentified
                || !ReflectionUtils.IsValidIdentifier(value)
                || ReflectionUtils.IsCSharpKeyword(value))
                value = ReflectionUtils.ToVerbatim(value);

            if (value == null)
                value = Constants.NULL_VALUE;

            return value;
        }
    }
}
