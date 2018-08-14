//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphExpression.Tests
//{
//    public class A
//    {
//        public B B_Prop { get; set; }
//        public B B_Field { get; set; }

//        internal static A Create()
//        {
//            var a = new A()
//            {
//                B_Prop = B.Create(),
//                B_Field = B.Create()
//            };
//            return a;
//        }
//    }

//    public class B
//    {
//        public C C_Prop { get; set; }
//        public C C_Field;

//        internal static B Create()
//        {
//            var b = new B()
//            {
//                C_Field = C.Create(),
//                C_Prop = C.Create()
//            };
//            return b;
//        }
//    }

//    public class C
//    {
//        public D D_Prop { get; set; }
//        public D D_Field;

//        internal static C Create()
//        {
//            var c = new C()
//            {
//                D_Field = D.Create(),
//                D_Prop = D.Create()
//            };
//            return c;
//        }
//    }

//    public class D
//    {
//        public F F_Prop { get; set; }
//        public F F_Field;

//        internal static D Create()
//        {
//            var d = new D()
//            {
//                F_Field = F.Create(),
//                F_Prop = F.Create()
//            };
//            return d;
//        }
//    }

//    public class F
//    {
//        public string Name { get; set; }
//        public int Age { get; set; }

//        internal static F Create()
//        {
//            var f = new F()
//            {
//                Age = 10,
//                Name = "Test"
//            };
//            return f;
//        }
//    }
//}
