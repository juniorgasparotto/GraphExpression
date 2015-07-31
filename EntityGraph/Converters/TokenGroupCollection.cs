using NCalc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections;

namespace EntityGraph
{
    public class TokenGroupCollection : IEnumerable<TokenGroup>
    {
        private List<TokenGroup> results = new List<TokenGroup>();

        public TokenGroup this[HierarchicalEntity edo]
        {
            get
            {
                return results.FirstOrDefault(f => f.MainEdoObject == edo);
            }
        }

        public void Add(TokenGroup item)
        {
            this.results.Add(item);
        }

        public IEnumerator<TokenGroup> GetEnumerator()
        {
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return results.GetEnumerator();
        }
    }
}
