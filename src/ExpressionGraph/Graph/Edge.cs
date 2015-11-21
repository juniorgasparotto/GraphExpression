using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Graph
{
    public class Edge<T>
    {
        private bool? isLoop = false;

        public decimal Weight { get; set; }
        public Vertex<T> Source { get; internal set; }
        public Vertex<T> Target { get; internal set; }

        // Laço
        public bool IsLoop
        {
            get
            {
                if (isLoop != null)
                    isLoop = this.Source == this.Target;

                return isLoop.Value;
            }
        }

        internal Edge(Vertex<T> source = null, Vertex<T> target = null, decimal weight = 0)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        // Anti-paralelo
        public bool IsAntiparallel(Edge<T> compare)
        {
            if (this.Source == compare.Target && this.Target == compare.Source)
                return true;
            return false;
        }

        #region Overrides

        public static bool operator ==(Edge<T> a, Edge<T> b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Edge<T> a, Edge<T> b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var converted = (Edge<T>)obj;
            return (this.Source == converted.Source && this.Target == converted.Target);
        }

        /// <summary>
        /// Como descrito por Jon Skeet neste SO resposta, é melhor prática para escolher 
        /// alguns números primos e multiplique-os com os códigos de hash 
        /// individuais, em seguida, resumir tudo.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                
                if (this.Source != null)
                    result = (result * 397) ^ this.Source.GetHashCode();

                if (this.Target != null)
                    result = (result * 397) ^ this.Target.GetHashCode();

                return result;
            }
        }

        public override string ToString()
        {
            return (Source == null ? "" : Source.ToString()) + ", " + (Target == null ? "" : Target.ToString());
        }

        #endregion

    }
}