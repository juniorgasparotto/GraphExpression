namespace GraphExpression
{
    public enum PathType
    {
        // Not repeat and not circular
        Simple,
        // Repeat first and last (circular)
        Circuit,
        // Repeat in middle (A,B,B)
        Circle,
    }
}