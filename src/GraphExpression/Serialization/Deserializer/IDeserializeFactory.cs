namespace GraphExpression
{
    /// <summary>
    /// Interfaces to generate a Entity in deserialization moment
    /// </summary>
    /// <typeparam name="T">Determines the Type that necessarily override the operators '+'.</typeparam>
    public interface IDeserializeFactory<T>
    {
        /// <summary>
        /// This method is exclusive for deserialization moment and should create a new entity to compose a expression
        /// new Entity("A") + new Entity("B");
        /// </summary>
        /// <param name="name">Name that is used as Raw</param>
        /// <param name="index">Index at expression</param>
        /// <returns>Return a new Entity to compose the expression</returns>
        T GetEntity(string name, int index);
    }
}