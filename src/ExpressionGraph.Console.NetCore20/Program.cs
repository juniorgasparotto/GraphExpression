using System.Linq;

namespace ExpressionGraph.ConsoleApp.NetCore20
{
    class Program
    {
        static void Main(string[] args)
        {
            var myClass = MyClass.Get();
            var expression = myClass
                .AsExpression();

            var str = expression.ToString();
            var a = myClass.AsExpression(c => c.SelectFields())
                .Where(f => f.Entity.Object is MySubClass).ToArray();
            var entities = expression.ToEntities();

            // retornar todos os elementos que iniciam com "Prop"
            var descendants = expression.Descendants().ToList();

            var allPropsNames = expression.Descendants(i => i.Entity.ContainerName.Contains("Prop")).ToList();
//            var allSubClass = expression.Descendants(i => i.Entity.o.ContainerName.Contains("Prop")).ToList();


                //.Descendants((a, b) => a.Entity.GetAllProperties().Where(p => p.Name.Contains("Prop")))
        }

        public class MyClass
        {
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
            private MySubClass subClass;

            public MyClass(MySubClass sub)
            {
                this.subClass = sub;
            }

            public static MyClass Get()
            {
                var subClass = new MySubClass
                {
                    Prop3 = "Test3",
                    Prop4 = "Test4"
                };

                var myClass = new MyClass(subClass)
                {
                    Prop1 = "Test",
                    Prop2 = "Test2"
                };

                return myClass;
            }
        }

        public class MySubClass
        {
            public string Prop3 { get; set; }
            public string Prop4 { get; set; }
        }
    }
}
