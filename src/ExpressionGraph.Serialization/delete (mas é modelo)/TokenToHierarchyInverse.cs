/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EntityGraph.Converter;

namespace EntityGraph.Converter
{
    public class TokenToHierarchyInverse
    {
        private string delimiterMainTokens;
        private const string noReferenceLabel = "[no reference]";
        private Func<HierarchicalEntity, string> viewFunc;

        public TokenToHierarchyInverse(Func<HierarchicalEntity, string> viewFunc, string delimiterMainTokens = null)
        {
            delimiterMainTokens = string.IsNullOrEmpty(delimiterMainTokens) ? "\r\n-----\r\n" : delimiterMainTokens;
            this.delimiterMainTokens = delimiterMainTokens;
            this.viewFunc = viewFunc;
        }
        
        public string Convert(TokenGroupCollection collection, HierarchicalEntity edoObject = null)
        {
            var organizedChilds = new Dictionary<HierarchicalEntity, List<HierarchicalEntity>>();
            foreach (var keyPair in collection)
            {
                foreach (var tokenResult in keyPair)
                    this.OrganizeChildrensAndParents(tokenResult, organizedChilds, edoObject);
            }

            return this.GetString(organizedChilds);
        }

        private void OrganizeChildrensAndParents(List<Token> tokens, Dictionary<HierarchicalEntity, List<HierarchicalEntity>> organizedChilds, HierarchicalEntity onlyThis)
        {
            var onlyValues = tokens.Where(f => f.TokenValue.Value is HierarchicalEntity).ToList();
            foreach (var token in onlyValues)
            {
                var value = (HierarchicalEntity)token.TokenValue.Value;

                if (onlyThis != null && onlyThis != value)
                    continue;

                if (!organizedChilds.ContainsKey(value))
                    organizedChilds.Add(value, new List<HierarchicalEntity>());

                var parent = token.Parent != null ? (HierarchicalEntity)token.Parent.TokenValue.Value : null;
                if (parent != null && !organizedChilds[value].Contains(parent))
                    organizedChilds[value].Add(parent);
            }
        }

        private string GetString(Dictionary<HierarchicalEntity, List<HierarchicalEntity>> organizedChilds)
        {
            StringBuilder strBuilder = new StringBuilder();
            var last = organizedChilds.LastOrDefault();
            foreach (var child in organizedChilds)
            {
                if (child.Value.Count == 0)
                    strBuilder.AppendLine(noReferenceLabel);
                else
                    foreach (var parent in child.Value)
                        strBuilder.AppendLine(viewFunc(parent));

                strBuilder.Append("..." + viewFunc(child.Key));
                if (child.GetHashCode() != last.GetHashCode())
                    strBuilder.Append(delimiterMainTokens);
            }

            return Helper.TrimAll(strBuilder.ToString());
        }
    }
}

*/