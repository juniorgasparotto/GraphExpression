//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExpressionGraph.Reflection
//{
//    /// <summary>
//    /// Reader all member for a class
//    /// </summary>
//    public class ClassReaderWithoutPrimitives : IClassReader
//    {
//        public bool CanRead(object obj, Type type)
//        {
//            return true;
//        }

//        public List<Type> GetInstanceTypes(object obj, Type type)
//        {
//            var typesParents = new List<Type>();
//            typesParents.Add(type);
//            typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
//            return typesParents;
//        }

//        public List<PropertyInfo> GetProperties(object obj, Type type)
//        {
//            var propsPublics = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
//            var propsPrivates = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.Name.Contains(".")).ToList();
//            var propsImplicits = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.Name.Contains(".")).ToList();
//            var propsPublicsStatics = type.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
//            var propsPrivatesStatics = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static).ToList();

//            var properties = new List<PropertyInfo>();
//            properties.AddRange(propsPublics);
//            properties.AddRange(propsPrivates);
//            properties.AddRange(propsImplicits);
//            properties.AddRange(propsPublicsStatics);
//            properties.AddRange(propsPrivatesStatics);

//            return properties;
//        }

//        public List<MethodInfo> GetMethods(object obj, Type type)
//        {
//            var propsPublics = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsSpecialName);
//            var propsPrivates = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && !f.Name.Contains(".")).ToList();
//            var propsImplicits = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && f.Name.Contains(".")).ToList();
//            var propsPublicsStatics = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(f => !f.IsSpecialName);
//            var propsPrivatesStatics = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(f => !f.IsSpecialName);

//            var methods = new List<MethodInfo>();
//            methods.AddRange(propsPublics);
//            methods.AddRange(propsPrivates);
//            methods.AddRange(propsImplicits);
//            methods.AddRange(propsPublicsStatics);
//            methods.AddRange(propsPrivatesStatics);

//            return methods;
//        }

//        public List<FieldInfo> GetFields(object obj, Type type)
//        {
//            var fieldsPublics = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
//            var fieldsPrivates = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
//            var fieldsPublicsStatics = type.GetFields(BindingFlags.Public | BindingFlags.Static);
//            var fieldsPrivatesStatics = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);

//            var fields = new List<FieldInfo>();
//            fields.AddRange(fieldsPublics);
//            fields.AddRange(fieldsPrivates);
//            fields.AddRange(fieldsPublicsStatics);
//            fields.AddRange(fieldsPrivatesStatics);

//            return fields;
//        }
//    }
//}