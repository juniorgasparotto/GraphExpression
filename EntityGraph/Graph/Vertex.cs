// This program prints out basic information about the crash dump specified.
//
// The platform must match what you are debugging, as we have to load the dac, a native dll.
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    // Or node
    public class Vertex<T>
    {
        private HashSet<Vertex<T>> predecessors;
        private HashSet<Vertex<T>> successors;

        public T Data { get; private set; }
        public int InternalId { get; private set; }
        public int CountVisited { get; internal set; }
        public List<TokenValue> Tokens { get; internal set; }

        // parents
        public IEnumerable<Vertex<T>> Predecessors
        { 
            get 
            {
                return predecessors;
            }
        }

        // children
        public IEnumerable<Vertex<T>> Successors
        {
            get
            {
                return successors;
            }
        }

        public IEnumerable<TokenValue> GetTokens()
        {
            var list = new List<TokenValue>();

            if (this.successors.Count > 0)
            { 
                var tokenValue = new TokenValue(this, null);
                list.Add(tokenValue);

                foreach(var successor in this.successors)
                {
                    list.Add(TokenValuePlus.Instance);
                    tokenValue = new TokenValue(successor, null);
                    list.Add(tokenValue);
                }
            }

            return list;
        }

        // parent and children
        public IEnumerable<Vertex<T>> PredecessorsAndSuccessors
        {
            get
            {
                var list = this.predecessors.ToList();
                list.AddRange(successors);
                return list;
            }
        }

        // Grau de entrada (numero de parents)
        public int Indegrees
        {
            get
            {
                return predecessors.Count;
            }
        }

        // Grau de saída (numero de filhos)
        public int Outdegrees
        {
            get
            {
                return successors.Count;
            }
        }

        // Grau
        public int Degrees
        {
            get
            {
                return this.Indegrees + this.Outdegrees;
            }
        }

        // Folha (Leaf), Sorvedouro (sem filhos)
        public bool IsSink
        {
            get
            {
                if (this.successors.Count == 0)
                    return true;
                return false;
            }
        }

        // folha = acima = (só tem um pai ou um filho)
        //public bool IsLeaf
        //{
        //    get
        //    {
        //        
        //    }
        //}

        // Fonte (sem pais)
        public bool IsSource
        {
            get
            {
                if (this.predecessors.Count == 0)
                    return true;
                return false;
            }
        }

        // Isolado (sem pais)
        public bool IsIsolated
        {
            get
            {
                if (this.predecessors.Count == 0 && this.successors.Count == 0)
                    return true;
                return false;
            }
        }

        internal Vertex(T data, int internalId)
        {
            this.Data = data;            
            this.InternalId = internalId;
            this.predecessors = new HashSet<Vertex<T>>();
            this.successors = new HashSet<Vertex<T>>();

            this.Tokens = new List<TokenValue>();
            this.Tokens.Add(new TokenValue(this, null));
        }

        internal void AddIndegree(Vertex<T> parent)
        {
            this.predecessors.Add(parent);
            parent.successors.Add(this);
            parent.Tokens.Add(TokenValuePlus.Instance);
            parent.Tokens.Add(new TokenValue(this, null));
        }

        #region Overrides

        public static bool operator ==(Vertex<T> a, T b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Vertex<T> a, T b)
        {
            return !Equals(a, b);
        }

        public static bool operator ==(Vertex<T> a, Vertex<T> b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Vertex<T> a, Vertex<T> b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) 
                return false;

            if (obj is T)
                return this.Data.Equals(obj);
            else if (obj is Vertex<T>)
                return this.Data.Equals((obj as Vertex<T>).Data);

            return false;
        }

        public override int GetHashCode()
        {
            return this.Data.GetHashCode();
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        #endregion
    }
}