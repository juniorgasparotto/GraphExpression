using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class PropertyReaderArray : IPropertyReader
    {
        public bool CanRead(object obj, PropertyInfo property)
        {
            return obj is Array;
        }

        public IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length > 0)
            { 
                var array = obj as Array;
                long[][] allParameters = new long[array.Length][];
                var l = new List<long[]>();

                for (int dimension = 0; dimension < array.Rank; dimension++)
                {
                    long[] p = new long[array.Rank];
                    for (int i = array.GetLowerBound(dimension); i <= array.GetUpperBound(dimension); i++)
                    {
                        //for (int j = array.GetLowerBound(1); j <= array.GetUpperBound(1); j++)
                        {
                            //for (int k = array.GetLowerBound(2); k <= array.GetUpperBound(2); k++)
                            {
                                p[dimension] = i;
                            }
                        }
                    }

                    l.Add(p);
                }


                for (int i = 0; i < array.Length; i++)
                {
                    allParameters[i] = new long[array.Rank];
                    for (int dimension = 0; dimension < array.Rank; dimension++)
                    {
                        var len = array.GetLength(dimension);
                        for (int iDin = 0; iDin < len; iDin++)
                        {
                            allParameters[i][dimension] = iDin;
                        }
                    }
                }

                for (int dimension = 0; dimension < array.Rank; dimension++)
                {
                    var len = array.GetLength(dimension);
                    for (int i = 0; i < len; i++)
                    {
                        //parameters[dimension] = dimension;

                        //var value = property.GetValue(obj, new object[] { i });
                        //var parameter = new MethodValueParam(dimension.ToString(), null, dimension);
                        //yield return new MethodValue(value, parameter);
                    }
                }
            }
            yield return new MethodValue(0, null);
        }
    }
}