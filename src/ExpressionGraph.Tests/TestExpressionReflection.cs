using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ExpressionGraph.Tests
{
    [TestClass]
    public partial class TestExpressionReflection
    {
        //private GraphConfiguration<HierarchicalEntity> GetConfig()
        //{
        //    return new GraphConfiguration<HierarchicalEntity>
        //    (
        //        assignEdgeWeightCallback: (current, parent) => 1,
        //        entityToStringCallback: f => f.ToString()
        //    );
        //}

        private string ToStringExpressionItems(IEnumerable<ExpressionItem<HierarchicalEntity>> items)
        {
            var str = "";
            foreach (var t in items)
                str += str == "" ? t.ToString() : "," + t.ToString();
            return str;
        }

        private Expression<HierarchicalEntity> GetExpression(string expressionIn, out IEnumerable<HierarchicalEntity> entities, bool usePlus = true, bool useParenthesis = true, bool awaysRepeatDefined = true)
        {
            entities = ExpressionUtils.FromString(expressionIn);
            return ExpressionBuilder<HierarchicalEntity>.Build(entities, f => f.Children, true, usePlus, useParenthesis).FirstOrDefault();
        }

        [TestMethod]
        public void TestReflection1()
        {
            var entities = ExpressionUtils.FromString("A+B");

            var t = new List<string>();
            t.Add("ABC");
            t.Add("DEF");
           
            var output = JsonConvert.SerializeObject(t);
            var type = t;

            var build = ExpressionBuilder<object>.Build(
                entities,
                f =>
                {
                    var properties2 =
                    from property in f.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    select property.GetValue(f, null);
                    //select new
                    //{
                    //    Name = property.Name,
                    //    Value = property.GetValue(f, null)
                    //};

                    return properties2;
                }
                , true, true, true).FirstOrDefault();
            
            //var fields = build.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //var properties = build.GetType().GetProperties();

        }
    }


    
}
