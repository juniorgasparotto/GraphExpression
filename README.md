[
![Inglês](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/GraphExpression)
[
![Português](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md)

# <a name="implementation" />Graph Expression

This framework aims to implement the concept of graph expression on .NET language.

In short, the concept of **graph expression** aims to explore the benefits of a mathematical expression swapping numbers for entities. With this, we can create a new way to carry data and especially create a new kind of research in complex or circular graphs.

With respect to research, this project was inspired by the `JQuery` implementation for HTML elements, so the concept of graph expression with the `JQuery` ease-of-use for research in graph theory.

_Attention: This document will explain the concept of graph expression, he will focus only on the framework `GraphExpression` ._

**[Click here](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#concept) if you want to know more about the concept of expression of graph.**

# Installation

Via [NuGet](https://www.nuget.org/packages/GraphExpression/):

```
Install-Package GraphExpression
```

# <a name="index" />Index

* [Complex graphs](https://github.com/juniorgasparotto/GraphExpression#impl-graph-complex)
* [Circular graphs](https://github.com/juniorgasparotto/GraphExpression#impl-graph-circular)
* [Searching](https://github.com/juniorgasparotto/GraphExpression#impl-search)
  * [Unreferenced research](https://github.com/juniorgasparotto/GraphExpression#impl-search-without-ref)
  * [Search with reference](https://github.com/juniorgasparotto/GraphExpression#impl-search-with-ref)
  * [Types of searches](https://github.com/juniorgasparotto/GraphExpression#impl-search-kind)
    * [`Ancestors`](https://github.com/juniorgasparotto/GraphExpression#impl-search-ancertors)
    * [`Descendants`](https://github.com/juniorgasparotto/GraphExpression#impl-search-descentands)
    * [`Children`](https://github.com/juniorgasparotto/GraphExpression#impl-search-children)
    * [`Siblings`](https://github.com/juniorgasparotto/GraphExpression#impl-search-siblings)
* [Customizing complex expressions](https://github.com/juniorgasparotto/GraphExpression#impl-factory-expression-complex)
* [Creating circular entities with graph and mathematical expression](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-circular)
* [Creating complex entities with graph and mathematical expression](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex)
  * [Understanding `Entity` class](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-class-entity)
  * [Complex entities in text form-primitive types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-primitive)
  * [Complex entities in text-form complex types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-complex)
  * [Complex entities in text form-Collections and arrays](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-collections)
  * [Understanding `ComplexEntityFactory` class](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-class-complex-factory)
  * [Discoverers of types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-discovery-types)
  * [Discoverers of members](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-discovery-members)
  * [Value initializers](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-value-loaders)
  * [Assignment of children](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-child-assign)
* [Serialization](https://github.com/juniorgasparotto/GraphExpression#impl-serialization)
  * [Circular serialization](https://github.com/juniorgasparotto/GraphExpression#impl-serialization-circular)
  * [Complex Serialization](https://github.com/juniorgasparotto/GraphExpression#impl-serialization-complex)
    * [Customizing the serialization of the items](https://github.com/juniorgasparotto/GraphExpression#impl-serialization-complex-itens-serialize)
* [Deserialization](https://github.com/juniorgasparotto/GraphExpression#impl-deserialization)
  * [Deserialization circular](https://github.com/juniorgasparotto/GraphExpression#impl-deserialization-circular)
  * [Deserialization complex](https://github.com/juniorgasparotto/GraphExpression#impl-deserialization-complex)
* [Information from the graph of an entity](https://github.com/juniorgasparotto/GraphExpression#impl-graph-info)
* [Donations](https://github.com/juniorgasparotto/GraphExpression#donate)
* [License](https://github.com/juniorgasparotto/GraphExpression#license)

# <a name="impl-graph-complex" />Complex graphs

We call complex graphs those containing no type defined, that is, all items are defined as `object` .

This type of graph is presented by class:

```csharp
GraphExpression.Expression<object> : List<EntityItem<object>>
```

This class inherits from `List<EntityItem<object>>` , that is, it is also a collection `EntityItem<object>` class. The class `EntityItem<object>` represents an item in the list, this is the class that there are all the information of the entity in the graph.

In the following example we will convert an object of the `Class1` type for the `Expression<object>` object and display all `EntityItem<object>` of the structure `Class1` type. In last exit, we display how would this object in the form of expression of graph:

```csharp
public void GraphComplex()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Field1 = 1000,
            Class2_Prop2 = "Value2"
        }
    };

    // transversal navigation
    Expression<object> expression = model.AsExpression();
    foreach (EntityItem<object> item in expression)
    {
        var ident = new string(' ', item.Level * 2);
        var output = $"{ident}[{item.Index}] => Item: {GetEntity(item)}, Parent: {GetEntity(item.Parent)}, Previous: {GetEntity(item.Previous)}, Next: {GetEntity(item.Next)}, Level: {item.Level}";
        System.Console.WriteLine(output);
    }

    // Serialize to expression
    System.Console.WriteLine(expression.DefaultSerializer.Serialize());
}

// Get entity as String to example
private string GetEntity(EntityItem<object> item)
{
    if (item is PropertyEntity prop)
        return $"Property.{prop.Property.Name}";

    if (item is FieldEntity field)
        return $"Field.{field.Field.Name}";

    if (item is ComplexEntity root)
        return root.Entity.GetType().Name;

    return null;
}

public class Class1
{
    public string Class1_Prop1 { get; set; }
    public Class2 Class1_Prop2 { get; set; }
}

public class Class2
{
    public int Class2_Prop1 = int.MaxValue;
    public string Class2_Prop2 { get; set; }
}
```

**1)** at the first exit, we can visualize all the type information `Class1` and also information from the graph: expression `Index` , `Parent` , `Next` , `Previous` and `Level` :

```
  [0] => Item: Class1, Parent: , Previous: , Next: Property.Class1_Prop1, Level: 1
    [1] => Item: Property.Class1_Prop1, Parent: Class1, Previous: Class1, Next: Property.Class1_Prop2, Level: 2
    [2] => Item: Property.Class1_Prop2, Parent: Class1, Previous: Property.Class1_Prop1, Next: Property.Class2_Prop2, Level: 2
      [3] => Item: Property.Class2_Prop2, Parent: Property.Class1_Prop2, Previous: Property.Class1_Prop2, Next: Field.Class2_Field1, Level: 3
      [4] => Item: Field.Class2_Field1, Parent: Property.Class1_Prop2, Previous: Property.Class2_Prop2, Next: , Level: 3
```

* The property `Level` is responsible for reporting on what level of the graph is each item, making it possible to create a identada output that represents the hierarchy of the `model` object.
* The method `GetEntity` is just a helper that prints the item type and the name of the Member that can be a property or a field. We could also return the value of the Member, but to leave cleaner output, we eliminate this information.

**2)** On second output, see how was the expression of this graph object:

[Click here](https://github.com/juniorgasparotto/GraphExpression#impl-serialization-complex) to understand how the serialization of complex objects.

```
"Class1.32854180" + "Class1_Prop1: Value1" + ("Class1_Prop2.36849274" + "Class2_Prop2: Value2" + "Class2_Field1: 1000")
```

The extension method `AsExpression` is responsible for creating the complex expression. This method will browse through all the nodes from the root to the last descendant. This method contains the following parameters:

* `ComplexExpressionFactory factory = null`: This parameter must be used when you need to change or extend the default behavior of creating a complex graph. The topic [Customizing complex expressions](https://github.com/juniorgasparotto/GraphExpression#impl-factory-expression-complex) back all the information of how to extend the default behavior.
* `bool deep = false`: When `true` the expression will be deep, that is, when possible, will repeat entities that have already been navigated. See the topic [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep) to understand the purpose of this functionality.

This method is available in all .NET objects, simply add the namespace reference: `using GraphExpression` .

**Conclusion:**

In this topic we saw how simple it is to navigate complex objects, opening paths to research into and.

See also the topic [Creating complex entities with graph and mathematical expression](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex), it will show another way to create complex objects.

## Standard elements of a graph for complex types

The elements of a complex expression ( `Expression<object>` ) can vary between the following types:

* `ComplexEntity`: This type is the basis for all other types of a complex expression. It is also the root entity type, i.e. the first entity of the expression.
* `PropertyEntity`: Determines that the item is a property.
* `FieldEntity`: Determines that the item is a field.
* `ArrayItemEntity`: Determines that the item is an item of a `array` , i.e., the parent class will be of type `Array` .
* `CollectionItemEntity`: Determines that the item is an item in a collection, that is, the parent class will be of type `ICollection` .
* `DynamicItemEntity`: Determines that the item is a dynamic property, i.e., the parent class will be of type `dynamic` .

All of these types inherit from `ComplexEntity` that for your time inherits from `EntityItem<object>` , so in addition to its specific properties still have the item information in the expression.

It is still possible to extend to create a complex expression. To know more see the topic[Customizing complex expressions](https://github.com/juniorgasparotto/GraphExpression#impl-factory-expression-complex)

# <a name="impl-graph-circular" />Circular graphs

We define the circular graphs as being those that contain a defined type, i.e. all items are set to the same `T` type.

This type of graph is presented by class:

```csharp
GraphExpression.Expression<T> : List<EntityItem<T>>
```

This class inherits from `List<EntityItem<T>>` , that is, it is also a collection of class: `EntityItem<T>` .

In the following example we will convert the object to the `Expression<CircularEntity>` type and show you how was the converted structure:

```csharp
public void GraphCircular()
{
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    // populate A
    A.Children.Add(B);
    A.Children.Add(C);

    // populate C
    C.Children.Add(D);

    // Create circular expression
    Expression<CircularEntity> expression = A.AsExpression(e => e.Children, entityNameCallback: o => o.Name);

    // print 'A'
    foreach (EntityItem<CircularEntity> item in expression)
    {
        var ident = new string(' ', item.Level * 2);
        var output = $"{ident}[{item.Index}] => Item: {item.Entity.Name}, Parent: {item.Parent?.Entity.Name}, Previous: {item.Previous?.Entity.Name}, Next: {item.Next?.Entity.Name}, Level: {item.Level}";
        System.Console.WriteLine(output);
    }

    // Serialize to graph expression
    System.Console.WriteLine(expression.DefaultSerializer.Serialize());
}

public class CircularEntity
{
    public string Name { get; private set; }
    public List<CircularEntity> Children { get; } = new List<CircularEntity>();
    public CircularEntity(string identity) => this.Name = identity;
}
```

**1)** at the first exit, we see the items `expression` object, representing the object hierarchy `A` :

```
[0] => Item: A, Parent: , Previous: , Next: B, Level: 1
  [1] => Item: B, Parent: A, Previous: A, Next: C, Level: 2
  [2] => Item: C, Parent: A, Previous: B, Next: D, Level: 2
    [3] => Item: D, Parent: C, Previous: C, Next: , Level: 3
```

**2)** On second output, we can see the expression of the graph `A` object:

[Click here](https://github.com/juniorgasparotto/GraphExpression#impl-serialization-circular) to understand how the serialization of circular objects.

```
A + B + (C + D)
```

The extension method `AsExpression<T>` is responsible for creating the circular expression. This method will browse through all the nodes from the root to the last descendant. This method contains the following parameters:

* `Func<T, IEnumerable<T>> childrenCallback`: This parameter determines which are the children of the entities. Is this parameter that will determine the continuity of the implementation. All entities of the graph will call this method until all are navigated. The execution will only be interrupted in case of cyclic relations.
* `Func<T, object> entityNameCallback`: This parameter is responsible for determining what will be the name of the entity in the serialization or in debug mode. In our example, we use the property `Name` . If this parameter is not passed, the `ToString()` method will be used.
* `bool deep = false`: When `true` the expression will be deep, that is, when possible, will repeat entities that have already been navigated.

This method is available in all .NET objects, simply add the namespace reference: `using GraphExpression` .

**Conclusion:**

In this topic we saw how simple it is to navigate in circular objects, opening paths to research into and.

See also the topic [Creating circular entities with graph and mathematical expression](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-circular), it will show another way to create circular objects without the use of the method `Add()` .

# <a name="impl-search" />Searching

There are two types of searches in the concept of graph expression: **unreferenced Research** and **reference search**.

_Attention: in this topic, we will use the model of complex graphs due to your increased complexity._

**[Click here](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search) to better understand how search works in expression of graph**

## <a name="impl-search-without-ref" />Unreferenced research

The "unreferenced" will be made into a collection of entities, i.e., each item in the collection will be tested. By repeating the same search in all items in the list, this kind of research can bring duplicates.

[Click here](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-without-references) to learn more about this type of research.

Whereas the same models of the example `GraphComplex` , let's create a search to return all descendants of all entities that are a property and children of the class `Class2` .

```csharp
public void Search1()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Prop2 = "ValueChild",
            Class2_Field1 = 1000
        }
    };

    // filter
    Expression<object> expression = model.AsExpression();
    IEnumerable<EntityItem<object>> result = expression.Descendants(e => e is PropertyEntity && e.Parent.Entity is Class2);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(GetEntity(item));
}
```

_As expected, the result returned two lines:_

```
Property.Class2_Prop2
Property.Class2_Prop2
```

* The first entity (root) had all his descendants tested by filter and following descendant had positive feedback: `Property.Class2_Prop2` .
* The second `Property.Class1_Prop1` principal was also tested, but she has no descendants.
* The third `Property.Class1_Prop2` entity had all of its descendants tested and also returned the item: `Property.Class2_Prop2` .
* The fourth entity on, none returned positive.

If you want to eliminate repetitions in this type of search (with collections), use the `Linq` function:

```csharp
Distinct();
```

## <a name="impl-search-with-ref" />Search with reference

The "reference search" will be made using a specific item, that is, first you need to locate the desired item and from it will be done the research you want.

[Click here](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-with-references) to learn more about this type of research.

Whereas the same models of the example `GraphComplex` , let's create a search to return all descendants of the root item to be a property and children `Class2` class.

```csharp
public void Search2()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Prop2 = "ValueChild",
            Class2_Field1 = 1000
        }
    };

    // filter
    Expression<object> expression = model.AsExpression();
    EntityItem<object> root = expression.First();
    IEnumerable<EntityItem<object>> result = root.Descendants(e => e is PropertyEntity && e.Parent.Entity is Class2);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(GetEntity(item));
}
```

As expected, the result returned one row:

```
Property.Class2_Prop2
```

* Note that the only change was to use the root item as reference ( `First()` ) and it did eliminate duplicates without the need of using the `Distinct` method.
* This was because only one item was tested (the root item). On "research without reference", all items were reviewed and the `Property.Class1_Prop2` item also returns the same result as the root item.
* When possible, use this type of search, this will make the search faster.
* The entity root is the best option for this.

## <a name="impl-search-kind" />Types of searches

By default, this project brings the following types of searches:

* `Ancestors`: Returns all ancestors of a given item.
* `AncestorsUntil`: Returns all ancestors of a given item until the specified filter returns positive.
* `Descendants`: Returns all descendants of a given item.
* `DescendantsUntil`: Returns all descendants of a given item until the specified filter returns positive.
* `Children`: Returns the children of an item.
* `Siblings`: Returns the siblings of an item.
* `SiblingsUntil`: Returns the siblings of an item until the specified filter returns positive.

All these types of searches are available for any of the object types:

* `GraphExpression.EntityItem<T>`: Search with reference
* `IEnumerable<GraphExpression.EntityItem<T>>`: Search without reference

You can also create custom searches using C # Extensions methods.

**Without references:**

```csharp
public static IEnumerable<EntityItem<T>> Custom<T>(this IEnumerable<EntityItem<T>> references)
```

**With references:**

```csharp
public static IEnumerable<EntityItem<T>> Custom<T>(this EntityItem<T> references)
```

### Delegates of the research

All research methods use the following delegates:

```csharp
public delegate bool EntityItemFilterDelegate<T>(EntityItem<T> item);
public delegate bool EntityItemFilterDelegate2<T>(EntityItem<T> item, int depth);
```

* `EntityItem<T> item`: This parameter means the current item during the search.
* `int depth`: Determines the depth of the current item in relation to your position.

You can use the classes `Func<EntityItem<T>>` and `Func<EntityItem<T>, int>` to simplify use.

### <a name="impl-search-ancertors" />`Ancestors`

The search for ancestors is useful to find the father or the parents of an item. We have a few overloads that will be explained below:

**1)** This is the default overload, if no parameter is passed, then no filter is applied and all ancestors will be returned.

```csharp
IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

* `filter`: Does not return items when the filter return negative but continues searching until you reach the root item. The research uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.
* `stop`: Determines when the navigation must stop, otherwise the navigation should go to the root item.
* `depthStart`: Determines the depth to the research begin
* `depthEnd`: Determines the depth that the search should stop

In this example, we will return all the ancestors of the last item of expression, noting that the structure is the same as the example `GraphComplex` :

```
  [0] => Item: Class1, Parent: , Previous: , Next: Property.Class1_Prop1, Level: 1
    [1] => Item: Property.Class1_Prop1, Parent: Class1, Previous: Class1, Next: Property.Class1_Prop2, Level: 2
    [2] => Item: Property.Class1_Prop2, Parent: Class1, Previous: Property.Class1_Prop1, Next: Property.Class2_Prop2, Level: 2
      [3] => Item: Property.Class2_Prop2, Parent: Property.Class1_Prop2, Previous: Property.Class1_Prop2, Next: Field.Class2_Field1, Level: 3
      [4] => Item: Field.Class2_Field1, Parent: Property.Class1_Prop2, Previous: Property.Class2_Prop2, Next: , Level: 3
```

```csharp
public void Ancertor1()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Prop2 = "ValueChild",
            Class2_Field1 = 1000
        }
    };

    // transversal navigation
    Expression<object> expression = model.AsExpression();
    EntityItem<object> lastItem = expression.Last();
    IEnumerable<EntityItem<object>> result = lastItem.Ancestors();

    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(GetEntity(item));

    System.Console.WriteLine("-> Parent");

    // Get first ancertos (parent)
    result = lastItem.Ancestors((item, depth) => depth == 1);

    foreach (var item in result)
        System.Console.WriteLine(GetEntity(item));
}
```

_The first output displays all parents of the item reference._

```
Property.Class1_Prop2
Class1
```

* Return order will always be the nearest ancestor, that is, the first item in the return list will always be the father of the item reference.

_The second output displays only the ancestor whose depth is equal to `1` , or is, in this case would be the parent item of the item reference:_

```
Property.Class1_Prop2
```

**2)** the second overload has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

**3)** the third overload filters only by the depth of beginning and end.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthStart, int depthEnd)
```

**4)** the fourth overload only the final depth filters.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthEnd)
```

**5)** this method has the same usefulness of standard overload, however he is a simplifier to retrieve all ancestors until any negative `stop` parameter return ancestor. Otherwise it returns all items until you reach the root. He uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

**6)** the second method overload `AncestorsUntil` has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

### <a name="impl-search-descentands" />`Descendants`

The descendants search is useful to find the children or all descendants of an item. We have a few overloads that will be explained below:

**1)** This is the default overload, if no parameter is passed, then no filter is applied and all descendants will be returned.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

* `filter`: Does not return items when the filter return negative but continues searching until you get to the last item. The research uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.
* `stop`: Determines when the navigation must stop, otherwise the navigation should go to the last item.
* `depthStart`: Determines the depth to the research begin
* `depthEnd`: Determines the depth that the search should stop

In this example, we will return all descendants of the root item whose initial and final depth is equal to `2` , we will use the same structure of the example `GraphComplex` :

```
  Class1                            // ***** ROOT *****
    PropertyEntity.Class1_Prop1     // Deph = 1
    PropertyEntity.Class1_Prop2     // Deph = 1
      PropertyEntity.Class2_Prop2   // Deph = 2
      FieldEntity.Class2_Field1     // Deph = 2
```

```csharp
public void Descendants1()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Prop2 = "ValueChild",
            Class2_Field1 = 1000
        }
    };

    // filter
    Expression<object> expression = model.AsExpression();
    EntityItem<object> root = expression.First();
    IEnumerable<EntityItem<object>> result = root.Descendants(2, 2);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(GetEntity(item));
}
```

_The output will be:_

```
Property.Class2_Prop2
Field.Class2_Field1
```

**2)** the second overload has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

**3)** the third overload filters only by the depth of beginning and end.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
```

**4)** the fourth overload only the final depth filters.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthEnd)
```

**5)** this method has the same usefulness of standard overload, however he is a simplifier to retrieve all descendants until some descendant return negative in the parameter "stop". Otherwise it returns all items until you get to the last item. He uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

**6)** the second method overload `DescendantsUntil` has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

### <a name="impl-search-children" />`Children`

To return the children of an item just use the method:

```csharp
IEnumerable<EntityItem<T>> Children()
```

In this example we return root item's children:

```csharp
public void Children()
{
    // create a simple object
    var model = new Class1
    {
        Class1_Prop1 = "Value1",
        Class1_Prop2 = new Class2()
        {
            Class2_Prop2 = "ValueChild",
            Class2_Field1 = 1000
        }
    };

    // filter
    Expression<object> expression = model.AsExpression();
    EntityItem<object> root = expression.First();
    IEnumerable<EntityItem<object>> result = root.Children();
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(GetEntity(item));
}
```

_The output will display the two properties that are children of the root item:_

```
Property.Class1_Prop1
Property.Class1_Prop2
```

* This method has no parameters, simply use the `Linq` functions if you need some filtering.
* This method is a `Descendants(int depthStart, int depthEnd)` method alias, which will be passed the fixed values `Descendants(1, 1)` .

### <a name="impl-search-siblings" />`Siblings`

This search finds the brothers of a given item. We have a few overloads that will be explained below:

**1)** This is the default overload, if no parameter is passed, then no filter is applied and all descendants will be returned.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

* `filter`: Does not return items when the filter return negative but continues searching until the last brother or first (depends on the `direction` parameter). The research uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.
* `stop`: Determines when the navigation must stop, otherwise the navigation should go until the last brother or first (depends on the `direction` parameter).
* `direction`: This parameter determines in which direction navigation should go:
  * `Start`: Determines which navigation should start on the first brother positioned to the left of the reference item and go to the last brother to the right.
  * `Next`: Determines which navigation should start next brother positioned to the right of the item and go to the last brother to the right.
  * `Previous`: Determines which navigation should start next brother positioned to the left of the reference item and go to the first brother left.
* `positionStart`: Determines the initial position that the search should start.
  * When the direction is equal to `Start` , the position `1` will be the first sibling to the left of the reference item.
  * When the direction is equal to `Next` , the position `1` will be the next sibling to the right of the item reference.
  * When the direction is equal to `Previous` , the position `1` will be the next sibling to the left of the reference item.
* `positionEnd`: Determines the end position that the search should stop.

In this example we will return the item whose brothers value is equal `C` in all directions.

```csharp
public void Siblings1()
{
    // create a simple object
    var model = new
    {
        A = "A",
        B = "B",
        C = "C",
        D = "D",
        E = "E",
    };

    // Get Siblings1 from C - Start direction
    System.Console.WriteLine("-> Start direction");
    Expression<object> expression = model.AsExpression();
    var C = expression.Where(f => f.Entity as string == "C");
    IEnumerable<EntityItem<object>> result = C.Siblings(direction: SiblingDirection.Start);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(item.ToString());

    // Get Siblings1 from C - Next direction            
    System.Console.WriteLine("-> Next direction");
    result = C.Siblings(direction: SiblingDirection.Next);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(item.ToString());

    // Get Siblings1 from C - Previous direction
    System.Console.WriteLine("-> Previous direction");
    result = C.Siblings(direction: SiblingDirection.Previous);
    foreach (EntityItem<object> item in result)
        System.Console.WriteLine(item.ToString());
}
```

_The first exit returns all the brothers of the `C` entity initiating the first sibling to the left until the last sibling to the right. It is important to highlight that the item itself is not returned, after all he's not brother himself._

```
-> Start direction
A: A
B: B
D: D
E: E
```

_The second exit returns all the brothers of the `C` entity initiating the next sibling to the right till the last sibling to the right._

```
-> Next direction
D: D
E: E
```

_The third exit returns all the brothers of the `C` entity initiating the previous sibling to the first brother left._

```
-> Previous direction
B: B
A: A
```

**2)** the second overload has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

**3)** the third overload filters only by the depth of beginning and end in the specified direction.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

**4)** the fourth overload only the final depth of filters specified direction.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

**5)** this method has the same usefulness of standard overload, however he is a simplifier to retrieve all the brothers until a brother return negative in the parameter "stop". Otherwise be returned all the brothers until the last or the first (depends on the `direction` parameter). He uses the delegate `EntityItemFilterDelegate2` , that is, we have the depth information of the item to use in the search.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

**6)** the second method overload `SiblingsUntil` has the same filters, however, uses the delegate `EntityItemFilterDelegate` that has only the `item` parameter leaving faster writing.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

# <a name="impl-factory-expression-complex" />Customizing complex expressions

The method `object.AsExpression` is the fastest way to create a graph. When called without any parameters, it will create an instance of the object `Expression<object>` that represents an expression of complex graph.

This method is located in the static class `GraphExpression.ComplexExpressionExtensions` and contains the following parameters:

* `this object entityRoot`: The object that will be extended to accommodate the new method and also to be the root of the expression.
* `ComplexExpressionFactory factory = null`: This parameter must be used when you need to change or extend the default behavior of creating a complex graph.
* `bool deep = false`: When `true` the expression will be deep, that is, when possible, will repeat entities that have already been navigated.

The class `GraphExpression.ComplexExpressionFactory` is the class responsible for creating/customizing a complex expression. This class has some properties that increase the power of creation of the expression:

* `List<IEntityReader> Readers`: With this list you can create or change the reading of entities for certain types of objects.
* `List<IMemberReader> MemberReaders`: With this list you can create or change the readers of members.
* `Func<object, IEnumerable<PropertyInfo>> GetProperties`: Method that delegates to the user the option to determine which properties will be loaded. By default, will be used: `entity.GetType().GetProperties()` .
* `Func<object, IEnumerable<FieldInfo>> GetFields`: Method that delegates to the user the option to determine which fields are loaded. By default, will be used: `entity.GetType().GetFields()` .

You can use the instance of this class in the parameter `factory` of the method `object.AsExpression` or simply call the method `Build` that returns the instance of the `Expression<object>` class, arriving at the same goal.

```csharp
Expression<object> Build(object entityRoot, bool deep = false)
```

* `entityRoot`: Entity that will be the root of the expression.
* `deep`: When `true` the expression will be profound.

In the example below, we will create a new reader of members ( `MethodReader` ) that have as objective to invoke the method `HelloWorld` of the class `Model` . We will create a new entity type complex ( `MethodEntity` ) to store the result of the method.

Noting that the methods are not supported by default, only fields and properties are supported.

```csharp
public void ComplexExpressionFactory()
{
    var factory = new ComplexExpressionFactory();
    factory.MemberReaders.Add(new MethodReader());
    var model = new Model();
    var expression = model.AsExpression(factory);

    foreach(ComplexEntity item in expression)
    {
        var output = GetEntity(item);
        if (item is MethodEntity method)
            output = $"MethodEntity.{method.MethodInfo.Name}({method.Parameters[0]}, {method.Parameters[1]})";
        System.Console.WriteLine(output);
    }
}

public class MethodReader : IMemberReader
{
    public IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, GraphExpression.Expression<object> expression, object entity)
    {
        if (entity is Model)
        {
            var method = entity
                .GetType()
                .GetMethods().Where(f => f.Name == "HelloWorld")
                .First();

            var parameters = new object[] { "value1", "value2" };
            var methodValue = method.Invoke(entity, parameters);
            yield return new MethodEntity(expression, method, parameters, methodValue);
        }
    }
}

private class MethodEntity : ComplexEntity
{
    public MethodInfo MethodInfo { get; }
    public object[] Parameters { get; }

    public MethodEntity(Expression<object> expression, MethodInfo methodInfo, object[] parameters, object value)
        : base(expression)
    {
        this.MethodInfo = methodInfo;
        this.Parameters = parameters;
        this.Entity = value;
    }
}

private class Model
{
    public string HelloWorld(string val1, string val2)
    {
        return $"{val1}-{val2}";
    }
}
```

_The output shows as was the expression with the new type of complex entity. The first item of the output is the root item and the second item is the new entity._

```
Model
MethodEntity.HelloWorld(value1, value2)
```

## Readers of members

The readers of members "inherit from the `IMemberReader` class. Its main purpose is to read the members of an instance and return to the expression.

```csharp
public interface IMemberReader
{
    IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, Expression<object> expression, object entity);
}
```

By default, we have two readers of members:

* `FieldReader`: This reader returns the fields of the instance using the`factory.GetFields`
* `PropertyReader`: This reader returns the instance properties using the property`factory.GetProperties`

_Attention: readers of members must be used by the readers of entities. If your reader doesn't do this job then your expression does not have members._

## Readers of entities

The "principal players" are mainly responsible for the creation of complex expression and the property `Readers` will be used to find the best reader for each entity of the iteration.

Readers of entities must inherit from `IEntityReader` interface and the method `CanRead` is responsible for determining if the entity may or may not be read by the reader. When the method `CanRead` returns `true` then the method `GetChildren` will be called and it is at this point that the items of the expression will be effectively created.

```csharp
public interface IEntityReader
{
    bool CanRead(ComplexExpressionFactory factory, object entity);
    IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory factory, Expression<object> expression, object entity);
}
```

The order of the readers is of the utmost importance, since the last reader of the list will be used in the event of a tie, in other words, if three readers return `true` , the last on the list will be used.

By default, we have some readers of defined entities and all of them are already sorted on the property `Readers` to prevent reading errors.

1. `DefaultReader`: This is the default class for all entities. Its function is to read the members of any type, with the exception of types that are in `System` and `Microsoft` . He is the first reader, i.e. If there is some other that returns `true` in the method `CanRead` , then this class is ignored.
2. `CollectionReader`: This reader is responsible for reading objects that inherit from the `ICollection` type. He is the second and must always be above `ArrayReader` and `DictionaryReader` readers. This is necessary because the objects `Array` types and `IDictionary` also inherit from `ICollection` and these readers must have priority over this.
3. `ArrayReader`: This reader is responsible for reading objects of type`Array`
4. `DictionaryReader`: This reader is responsible for reading objects that inherit from the type`IDictionary`
5. `DynamicReader`: This reader is responsible for reading objects that inherit from the `ExpandoObject` type. This reader must have priority over the reader `DictionaryReader` , so it is positioned below this reader.

Readers of entities are solely responsible for reading the members, that is, if your "principal player" customized you want to keep the members of the entity, so don't forget to iterate over the property `MemberReaders` and do the due return.

Here is an example of how to use the property `MemberReaders` within the readers of entities. This code belongs to the reader:`CollectionReader`

Note that in addition to the reading of the list members are also created and returned. This is necessary because there may be business classes that have public properties and still inherit from `ICollection` .

```csharp
public class CollectionReader : IEntityReader
{
    public bool CanRead(ComplexExpressionFactory factory, object entity)
    {
        return entity is System.Collections.ICollection;
    }

    public IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory factory, Expression<object> expression, object entity)
    {
        var list = (System.Collections.ICollection)entity;
        var enumerator = list.GetEnumerator();
        var count = 0;
        while (enumerator.MoveNext())
            yield return new CollectionItemEntity(expression, count++, enumerator.Current);

        // read members, it may happen to be an instance of the 
        // user that inherits from IList, so you need to read the members.
        foreach (var memberReader in factory.MemberReaders)
        {
            var items = memberReader.GetMembers(factory, expression, entity);
            foreach (var item in items)
                yield return item;
        }
    }
}
```

# <a name="impl-factory-entity-circular" />Creating circular entities with graph and mathematical expression

One of the advantages of `C#` language is that it allows you to override mathematical operators leaving the operation to the programmer. With this action delegated to the programmer it is possible to use the concept of graph expression to insert or remove an entity from the other.

If you have read the documentation on the expression of concept graph then you already know that the entity of the left of the operation is the parent entity and the entity of the right side of the operation is the daughter.

With that in mind, we will show a way to create **circular graphs** using only C # and mathematics.

In the sample code, we will overwrite `+` and operators `-` and delegate to them the following actions:

* `+`: Add the "right" entity as being the daughter of "the left".
* `-`: Remove the "right-hand entity" sons of "entity list of the left".

To the left of the operation entity is represented by the parameter `a` and the entity of the right side of the operation is represented by the parameter `b` .

```csharp
public class CircularEntity
{
    public static CircularEntity operator +(CircularEntity a, CircularEntity b)
    {
        a.Children.Add(b);
        return a;
    }

    public static CircularEntity operator -(CircularEntity a, CircularEntity b)
    {
        a.Children.Remove(b);
        return a;
    }

    public string Name { get; private set; }
    public List<CircularEntity> Children { get; } = new List<CircularEntity>();
    public CircularEntity(string identity) => this.Name = identity;
}
```

Note that the class `CircularEntity` can now be used in any mathematical expression:

```csharp
public void GraphCircular()
{
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    // ACTION: ADD
    A = A + B + (C + D);
}
```

_This code will create the graph of the entity `A` generating the following structure:_

```
[0] => A
    [1] => B
    [2] => C
         [3] => D
```

_We now remove the entity `D` from `C` entity:_

```csharp
public void GraphCircular()
{
    C = C - D;
}
```

_The final structure of the entity `A` will be:_

```
[0] => A
    [1] => B
    [2] => C
```

_The final graph expression, after removal, would look like this:_

```
A + B + C
```

# <a name="impl-factory-entity-complex" />Creating complex entities with graph and mathematical expression

Create complex entities with graph expression is not a simple task as we have seen in circular graphs. You need a robust class structure and the use of reflection to compose the entities.

Let's look at an example of how to create a complex class of type `CircularEntity` using only graph expression. The idea is to assign a value to the property `Name` .

```csharp
public void EntityFactory()
{
    var root = new Entity(0) + new Entity("Name: Entity name ;)");
    var factory = new ComplexEntityFactory<CircularEntity>(root);

    // Build entity
    factory.Build();

    var entity = factory.Value;
    System.Console.WriteLine(entity.Name);
}
```

The output shows that the property `Name` has been filled:

```
Entity name ;)
```

Note that we're using mathematical expressions and assemble the entity `CircularEntity` type without using the command `new` .

The order of the expression is the same as we saw in the circular bodies, namely, the item on the left of the expression is the parent item of the item on the right of expression. Because it is a complex type, the item on the left is the instance and the item on the right is the Member.

In the language `C#` we have two types of members: **Properties** and **Fields** and both can be used in the expression independent of your visibility.

## <a name="impl-factory-entity-complex-class-entity" />Understanding `Entity` class

This class represents an entity of the graph and the place where she might be. Each location is represented by a specific constructor and see it below:

**1)** this constructor represents a complex type located at the root of the expression, the `complexEntityId` parameter is required and is used to assign an ID for the entity. This ID is important because you may want to use this instance to another location of the graph.

```csharp
Entity(int complexEntityId)
```

**2)** the second constructor creates an entity located in a member, that is, can be a property or a field. The `Name` parameter will set the name of the Member. The `complexEntityId` parameter will assign this member the entity which corresponds to that ID.

```csharp
Entity(string name, int complexEntityId)
```

In the example below, we will see the use of the first two constructors where we create an object of type `MyClass` with ID equal to `0` and we will assign `Child` the entity property whose identification is also equal to `0` , that is, the root entity itself.

```csharp
public void EntityFactory2()
{
    var root = new Entity(0) + new Entity("Child", 0);
    var factory = new ComplexEntityFactory<MyClass>(root);

    // Build entity
    factory.Build();

    var entity = factory.Value;
    System.Console.WriteLine(entity == entity.Child);
}

private class MyClass
{
    private int _intValue;
    public MyClass Child { get; set; }
    public int IntValue => _intValue;
}
```

The output shows that the entities are exactly the same:

```
True
```

**3)** the third constructor should be used when you need to assign a value that is not a reference, i.e. any primitive type. These values should be passed in text form to be assigned correctly.

```csharp
Entity(string name, string value)
```

It is important to note that non-public members can also have values assigned. And in our next example we will demonstrate how to assign a value in the private field `_intValue` .

Note also that the value is in the form of text, and this is important because the constructor that accepts an integer value is unique to assign references and not primitive values.

```csharp
public void EntityFactory3()
{
    var root = new Entity(0) + new Entity("_intValue", "1000");
    var factory = new ComplexEntityFactory<MyClass>(root);

    // Build entity
    factory.Build();

    var entity = factory.Value;
    System.Console.WriteLine(typeof(MyClass).GetField("_intValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(entity));
}
```

This output will produce the value below and it was necessary to obtain the value via reflection due to fields ' visibility `_intValue` :

```
1000
```

**4)** the last contains only the constructor parameter: `string raw` . The value of this parameter must be in the form of **complex entities in text form**.

```csharp
Entity(string raw)
```

In the following example we will see the creation of an entity where we populate the private field `_intValue` using the **format of complex entities in text form**.

```csharp
public void EntityFactory4()
{
    var root = new Entity(0) + new Entity("System.Int32._intValue: 1000");
    var factory = new ComplexEntityFactory<MyClass>(root);

    // Build entity
    factory.Build();

    var entity = factory.Value;
    System.Console.WriteLine(typeof(MyClass).GetField("_intValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(entity));
}
```

This output will produce the value below and it was necessary to obtain the value via reflection due to fields ' visibility `_intValue` :

```
1000
```

This format is divided into two: **primitive types** and **complex types** and see that in the next topics.

**Class properties:**

* `IEntityFactory Factory`: Property that contains the instance of the entities.
* `Entity Parent`: Property that determines the parent entity in the expression.
* `IReadOnlyCollection<Entity> Children`: Property that contains all the children.
* `Entity this[int index]`: Property that returns a child by index.
* `Entity this[string key]`: Property that returns a son by the name of the Member.
* `List<Operation> Operations`: Property that contains all the operations for the execution in the factories of entities. This property is clean in all other entities, except for the root entity.
* `string Raw`: Property that determines the text entity.
* `Type Type`: Property that determines the type of the entity.
* `MemberInfo MemberInfo`: Property that determines the Member.
* `string Name`: Property that determines the name of the Member.
* `object Value`: Property that determines the value, that is, the entity itself.
* `string ValueRaw`: Property that determines the value in text form.
* `bool IsPrimitive`: Property that indicates whether the value is primitive or not.
* `string ComplexEntityId`: Property that indicates the ID of the entity.

## <a name="impl-factory-entity-complex-primitive" />Complex entities in text form-primitive types

For primitive types, we have the following format:

```
[TypeName].[MemberName]: [Value]
```

The square brackets indicate that the part is not mandatory, in the format above we see that the type is not required, however, when she is present, we should add a `.` .

The second part should not exist for the entity that is located at the root of the expression, after all if she is at the root she has no father.

The second entity in the second part is mandatory and indicates the name of the Member that can be a property or a field.

The next part is the `:` separator that divides the member name and your value. If the value of the Member is null, the tab will not be displayed.

**Let's see some examples:**

_Displays the type and the value of primitive entity that is located in the root. For being the root entity so there will be no member name:_

```
System.String: Value
```

_Displays the type, the value of the entity and primitive Member that is in the second position on:_

```
System.String.StrValue: Value
```

_Displays the type, Member, and an empty value of primitive entity that is in the second position on:_

```
System.String.StrValue: 
```

_Displays the type and the Member when the value is null. Does not display the "separator", this is indicative that the value is null:_

```
System.String.StrValue
```

## <a name="impl-factory-entity-complex-complex" />Complex entities in text-form complex types

For complex types, we have the following format:

```
[TypeName].[MemberName].EntityID
```

The square brackets indicate that the part is not mandatory, in the format above we see that the type is not required, however, when she is present, we should add a `.` .

The second part should not exist for the entity that is located at the root of the expression, after all if she is at the root she has no father.

The second entity in the second part is mandatory and indicates the name of the Member that can be a property or a field.

The next part is the ID of the entity in the graph. This ID must be an integer and it is she who ensures the possibility to use the same entity references in other parts of the graph.

**Let's see some examples:**

_Displays the type and ID of the complex entity that is located in the root. We'll use the ID equal to `0` . For being the root entity so there will be no member name:_

```
Namespace.MyClass.0
```

_Displays the type, Member, and the identification of the complex entity that is located in second position on. We'll use the ID equal to `1` :_

```
Namespace.MyClass.MyProperty.1
```

_Displays the type and the Member when the value is null. Displays no identification, because there is no entity. This is indicative that the value is null:_

```
Namespace.MyClass.MyProperty
```

## <a name="impl-factory-entity-complex-collections" />Complex entities in text form-Collections and arrays

To create items in a collection or array, the name of the Member must be a position within the brackets:`[{position}]: Value`

In the example below we will see how to create an array of integers using only graph expression. Note that instead of the member name, use the brackets as indicative of a collection item.

```csharp
public void EntityFactory5()
{
    var root = new Entity(0) + new Entity("[0]", "10") + new Entity("[1]: 11");
    var factory = new ComplexEntityFactory<int[]>(root);

    // Build entity and get typed value
    var entity = factory.Build().Value;
    System.Console.WriteLine(entity[0]);
    System.Console.WriteLine(entity[1]);
}
```

The output will be:

```
10
11
```

## <a name="impl-factory-entity-complex-class-complex-factory" />Understanding `ComplexEntityFactory` class

This class is responsible for creating the graph of the complex entity based in the expression. Internally she re-executes the expression and generates each entity of the graph.

This class contains the following constructor:

```csharp
ComplexEntityFactory(Type type, Entity root = null)
```

* `type`: This parameter determines the type of the root entity.
* `root`: This parameter determines which is the root entity. It is not mandatory, because this class is also used in deserialization where the root entity is obtained.

We have some properties that will help in the creation and customization of the entities:

* `IReadOnlyList<Entity> Entities`: Property that stores all the entities of the graph and without repeating them.
* `bool IsTyped`: Property that indicates whether the creation has a defined type.
* `IReadOnlyDictionary<Type, Type> MapTypes`: Property that contains the maps for interfaces or abstract classes, or even to concrete classes if necessary.
* `IReadOnlyList<string> Errors`: Property that stores the errors.
* `bool IgnoreErrors`: Property that indicates whether errors are ignored. Otherwise an exception will be sent.
* `List<ITypeDiscovery> TypeDiscovery`: Property that contains a list of discovery classes.
* `List<IValueLoader> ValueLoader`: Property that contains a list of classes to initialize values.
* `List<IMemberInfoDiscovery> MemberInfoDiscovery`: Property that contains a list of classes to find members.
* `List<ISetChild> SetChildAction`: Property that contains a list of classes that make the assignments of the daughters in the parent entities.
* `Entity Root`: Property that indicates the root entity.
* `Type RootType`: Property that indicates the type of the root entity.
* `object Value`: Value of the root entity.

Finally, we have some methods that are used when creating:

* `ComplexEntityFactory Build()`: This method is responsible for generating the graph. This method should return the class itself to maintain fluency.
* `void AddMapType<TFrom, TTo>()`: This method must be used before the method `Build` and it determines the mapping of types.
* `void AddError(string err)`

There is also a variation of this class that allows you to work in a generic way and this facilitates the use of `Value` property which will already be with type set.

```csharp
ComplexEntityFactory<T>()
```

In the next example, we will see the creation of an entity that contains no properties with concrete types, being necessary the creation of a map of types:

```csharp
public void EntityFactory6()
{
    var root = new Entity(0) 
        + (new Entity("A", 1) + new Entity("MyProp", "10"))
        + (new Entity("B", 2) + new Entity("MyProp", "20"));

    var factory = new ComplexEntityFactory<ClassWithAbstractAndInterface>(root);
    factory.AddMapType<Interface, ImplementAbstractAndInterface>();
    factory.AddMapType<AbstractClass, ImplementAbstractAndInterface>();

    // Build entity and get typed value
    var entity = factory.Build().Value;
    System.Console.WriteLine(entity.A.MyProp);
    System.Console.WriteLine(entity.A.GetType().Name);
    System.Console.WriteLine(entity.B.MyProp);
    System.Console.WriteLine(entity.B.GetType().Name);
}
```

The output shows that the properties `A` and `B` were created with the `ImplementAbstractAndInterface` type and the properties `A.MyProp` and `B.MyProp` have also been populated:

```
10
ImplementAbstractAndInterface
20
ImplementAbstractAndInterface
```

## <a name="impl-factory-entity-complex-discovery-types" />Discoverers of types

The "discoverers of types ' main objective is to find out the type of the entity. The property `TypeDiscovery` will be used to find the best option for each entity.

The discoverers of types must inherit from `ITypeDiscovery` interface and the method `CanDiscovery` is responsible for determining if the entity may or may not be discovered. When the method `CanDiscovery` returns `true` then the method `GetEntityType` is called and this is where the type of the entity is returned.

```csharp
public interface ITypeDiscovery
{
    bool CanDiscovery(Entity item);
    Type GetEntityType(Entity item);
}
```

The order of discovery is of the utmost importance, since the last of the list will be used in the event of a tie, in other words, if three return `true` , the last on the list will be used.

By default, we have some discoverers of types defined and all of them are already sorted on the property `TypeDiscovery` to prevent errors.

1. `DictionaryItemTypeDiscovery`: This class is responsible for finding out the type of an item in the dictionary. The method `CanDiscovery` checks whether the parent type is a dictionary, if it is, then the method `GetEntityType` will be called by returning the `KeyValuePair<,>` type.
2. `MemberInfoTypeDiscovery`: This class is responsible for finding out the type of the Member. The method `CanDiscovery` checks if the entity contains a member name, if it does, then the method `GetEntityType` will be called to get the type of the Member.
3. `ListItemTypeDiscovery`: This class is responsible for finding out the type of an item in a list. The method `CanDiscovery` checks whether the father is a type `IList` , if it is, then the method `GetEntityType` will be invoked to get the type of the list.
4. `ArrayItemTypeDiscovery`: This class is responsible for finding out the type of an item in the array. The method `CanDiscovery` checks whether the parent type is an array, if it is, then the method `GetEntityType` will be invoked to get the type of the array.

## <a name="impl-factory-entity-complex-discovery-members" />Discoverers of members

The "discoverers of members" aims to discover the entity member. The property `MemberInfoDiscovery` will be used to find the best option for members of each entity.

The discoverers of members must inherit from `IMemberInfoDiscovery` interface and the method `CanDiscovery` is responsible for determining if the entity may or may not have your Member discovered. When the method `CanDiscovery` returns `true` then the method `GetMemberInfo` is called to return the `MemberInfo` .

```csharp
public interface IMemberInfoDiscovery
{
    bool CanDiscovery(Entity item);
    MemberInfo GetMemberInfo(Entity item);
}
```

By default, we only have one class defined in: `MemberInfoDiscovery` .

```csharp
class MemberInfoDiscovery : IMemberInfoDiscovery
```

If you want to replace it, just add a new class in the property `MemberInfoDiscovery` .

Just make sure that the method `CanDiscovery` has the following implementation:

```csharp
public bool CanDiscovery(Entity item)
{
    return item.Factory.IsTyped
            && item.Name != null
            && !item.Name.StartsWith(Constants.INDEXER_START) // ignore [0] members
            && item.Parent.Type != null;
}
```

This code ensures that:

1. `item.Factory.IsTyped`: There is a type defined for the root entity. It is from her that we found all kinds of graph.
2. `item.Name != null`: There is a name for the Member
3. `!item.Name.StartsWith(Constants.INDEXER_START)`: The name of the Member cannot be a positional representation of collections, that is, cannot start with `[` .
4. `item.Parent.Type != null`: There is a type for the parent entity. It is with this type and member name that we obtain the type of the Member.

## <a name="impl-factory-entity-complex-value-loaders" />Value initializers

The "values initializers" aims to create the primitive and complex entities. The property `ValueLoader` will be used to find the best option for each entity. Understand the term "value" as being the entity that will be created.

The "values initializers" must inherit from `IValueLoader` interface and the method `CanLoad` is responsible for determining if the entity type may or may not be initialized. When the method `CanLoad` returns `true` then the method `GetValue` is called to get the value that will be the entity.

```csharp
public interface IValueLoader
{
    bool CanLoad(Entity item);
    object GetValue(Entity item);
}
```

The order of the "initializer" is of the utmost importance, since the last of the list will be used in the event of a tie, in other words, if three return `true` , the last on the list will be used.

By default, we have some "initializers of values" defined and all of them are already sorted on the property `ValueLoader` to prevent errors.

1. `PrimitiveValueLoader`: Initializes the primitive types.
2. `ComplexEntityValueLoader`: Initializes the complex types. If the type has a constructor that takes no parameters, then this constructor will be used, otherwise the instance will be created without calling the constructor, that is, using the `FormatterServices.GetUninitializedObject(typeof(T))` method. This initializer is only used when the property `ComplexEntityId` is populated, i.e. If the entity has ID is because she is complex.
3. `ArrayValueLoader`: This initializer is used when the entity is an array. Independent of the number of dimensions.
4. `ExpandoObjectValueLoader`: This initializer is used for anonymous types or `ExpandoObject` type.

**Important:**

The `ExpandoObject` type will be used at all levels when the class `ComplexEntityFactory` does not have a defined type.

## <a name="impl-factory-entity-complex-child-assign" />Assignment of children

The award is intended to add a child entity in your parent entity, i.e., assign a value in a parent instance member, or an item in a list for example. The property `SetChildAction` will be used to find the best option for each entity.

A class that makes assignment of children must inherit from the `ISetChild` interface, and the method `CanSet` is responsible for determining whether the child item may or may not be attributed to the parent item. When the method `CanSet` returns `true` then the method `SetChild` will be called to do the assignment.

```csharp
public interface ISetChild 
{
    bool CanSet(Entity item, Entity child);
    void SetChild(Entity item, Entity child);
}
```

The order is of extreme importance, once the last of the list will be used in the event of a tie, in other words, if three return `true` , the last on the list will be used.

By default, we have some classes of attribution set and all of them are already sorted on the property `SetChildAction` to prevent errors.

1. `MemberInfoSetChild`: This class will be used when the child entity has a member set.
2. `DictionarySetChild`: This class will be used when the parent entity is a dictionary and daughter have the member name started by the character `[` . This means that the child is an item and not a property of the parent entity.
3. `ExpandoObjectSetChild`: This class is used when the parent entity is of type `ExpandoObject` .
4. `ArraySetChild`: This class is used when the parent entity is of type `Array` and daughter have the member name started by the character `[` .
5. `ListSetChild`: This class is used when the parent entity is of type `IList` and daughter have the member name started by the character `[` .

# <a name="impl-serialization" />Serialization

Serialization is the process of transformation of entity to text. We split the serialization into two kinds: **circular entity serialization** and **serialization of complex entities**.

That's interesting, because circular entities are simpler and require only a name to represent them, unlike complex entities that can contain several properties.

## <a name="impl-serialization-circular" />Circular serialization

Serialization of circular entities is made by `CircularEntityExpressionSerializer` class.

This class inherits from the abstract class `ExpressionSerializerBase<T>` which has the responsibility to write the mathematical basis of a graph. This is done by the method `Serialize()` . He is responsible for creating the parentheses, add "+" characters among other things.

The class `CircularEntityExpressionSerializer` overrides the method `SerializeItem` that is responsible for serializing each item of expression.

The constructor requires two parameters:

```csharp
CircularEntityExpressionSerializer(Expression<T> expression, Func<T, object> entityNameCallback)
```

1. `expression`: Indicates which will be a circular expression that should be serialized.
2. `entityNameCallback`: Indicates which is the text used in each item of expression. If it is passed `null` then the method `ToString()` of each `EntityItem` will be used.

In the example below we will see a way to serialize a circular expression

```csharp
public void SerializationCircular1()
{
    // create a simple object
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    A = A + B + (C + D);

    var expression = A.AsExpression(c => c.Children);
    var serialization = new CircularEntityExpressionSerializer<CircularEntity>(expression, f => f.Name);
    var expressionAsString = serialization.Serialize();
    System.Console.WriteLine(expressionAsString);
}
```

The output is a circular expression whose name of each item will be the property `Name` of the class `CircularEntity` :

```
A + B + (C + D)
```

Some properties of customizations can be used before the serialization. All these properties are in the base class, that is, they apply to complex expressions too.

* `bool EncloseParenthesisInRoot`: This property determines if there is a root entity encompassing parentheses, the default is to not exist.
* `bool ForceQuoteEvenWhenValidIdentified`: This property forces the use of quotation marks even when the name of the entity is a valid name. A valid name cannot contain spaces or special characters that the language `C#` does not support in variable names. Names that reference terms reserved the `C#` are also considered invalid, for example: `bool` , `while` and etc. If a name is invalid then the use of quotation marks will be used, if a name is valid then the use of quotation marks will depend on the value of this property.
  * `true`: Forces the use of quotation marks even for valid names
  * `false`: Displays the quotes only for invalid names. This is the default value of this property.
* `IValueFormatter ValueFormatter`: This property indicates what will be the value formatter for each item of expression, by default we only have two, but is it possible to create a custom formatter using the interface `IValueFormatter` .
  * `DefaultValueFormatter`: This formatter is used as the default for any primitive type.
    * The types of "date" will have the following format:`yyyy-MM-ddTHH:mm:ss.fffzzz`
    * For Boolean types the default will be`true|false`
    * Other types will be converted to text using the culture: `CultureInfo.InvariantCulture` .
  * `TruncateFormatter`: This formatter can be used when the name of the entity is too large and need to truncate it. This means that many big names will be reduced according to the specified size. This formatter can only be applied to types of texts ( `string` ).

In the following example we will force the use of parentheses in the root item, forcing the use of quotation marks for valid names and also truncate names that pass of 3 characters:

```csharp
public void SerializationCircular2()
{
    // create a simple object
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("BigName");

    A = A + B + (C + D);

    var expression = A.AsExpression(c => c.Children);
    var serialization = new CircularEntityExpressionSerializer<CircularEntity>(expression, f => f.Name);
    serialization.EncloseParenthesisInRoot = true;
    serialization.ForceQuoteEvenWhenValidIdentified = true;
    serialization.ValueFormatter = new TruncateFormatter(3);

    var expressionAsString = serialization.Serialize();
    System.Console.WriteLine(expressionAsString);
}
```

The output below shows how was our customization, note that the name `BigName` was truncated, all items now have quotation marks and there is a parentheses including the root item.

```
("A" + "B" + ("C" + "Big"))
```

Finally, we highlight that when an expression is created using the method `AsExpression(c => c.Children)` , we will have on `DefaultSerialize` property a pre-configured instance of class: `CircularEntityExpressionSerializer<T>` .

However, this property returns the type of interface: `ISerialize<T>` . Therefore, you must cast to the circular or serializer using the method below that will do the conversion for you:

```csharp
expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
```

## <a name="impl-serialization-complex" />Complex Serialization

Serialization of complex entities is made by the class `ComplexEntityExpressionSerializer` . This class must abide by the rules that we have seen in the topics [Complex entities in text form-primitive types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-primitive), [Complex entities in text-form complex types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-complex) and [Complex entities in text form-Collections and arrays](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-collections).

This class inherits from the abstract class `ExpressionSerializerBase<object>` which has the responsibility to write the mathematical basis of a graph expression either circular or complex. This is done by the method `Serialize()` . He is responsible for creating the parentheses, adding the characters of sum and etc.

The class `ComplexEntityExpressionSerializer` overrides the method `SerializeItem` that is responsible for serializing each item of expression.

The constructor requires to be passed an instance of an expression.

```csharp
ComplexEntityExpressionSerializer(Expression<object> expression)
```

1. `expression`: Indicates which is the complex expression that should be serialized.

In the example below we will see a way to serialize a complex expression:

```csharp
public void SerializationComplex1()
{
    // create a simple object
    var model = new
    {
        A = "A",
        B = "B",
        C = "C",
        D = "D",
        E = "E",
    };

    var expression = model.AsExpression();
    var serialization = new ComplexEntityExpressionSerializer(expression);
    var expressionAsString = serialization.Serialize();
    System.Console.WriteLine(expressionAsString);
}
```

The output is a complex expression containing all properties with their respective values of anonymous entity:

```
"<>f__AnonymousType0`5.845912156" + "A: A" + "B: B" + "C: C" + "D: D" + "E: E"
```

Some properties of customizations can be used before the serialization:

* `bool EncloseParenthesisInRoot`: Has the same function of the circular expressions.
* `bool ForceQuoteEvenWhenValidIdentified`: Has the same function of the circular expressions.
* `IValueFormatter ValueFormatter`: Has the same function of the circular expressions.
* `GetEntityIdCallback`: Property that returns the ID of an entity, by default, use the method `GetHashCode()` .
* `ItemsSerialize`: Property that contains a list of the interface `IEntitySerialize` . This property is the main alternative to customize the serialization and see that in the next topics.
* `ShowType`: Determines how will be the display of type in each item of expression
  * `None`: Does not display the type for any item.
  * `TypeNameOnlyInRoot`: Displays the name of the type (in short form) for the root item.
  * `TypeName`: Displays the name of the type (in short form) for all items.
  * `FullTypeName`: Displays the full name of the type for all items in the expression.

In the following example we will force the use of parentheses in the root item, force the display type in all items of the expression and also truncate values that pass of 3 characters:

```csharp
public void SerializationComplex2()
{
    // create a simple object
    var model = new
    {
        A = "A",
        B = "B",
        C = "C",
        D = "D",
        E = "BIG VALUE",
    };

    var expression = model.AsExpression();
    var serialization = new ComplexEntityExpressionSerializer(expression);
    serialization.EncloseParenthesisInRoot = true;
    serialization.ValueFormatter = new TruncateFormatter(3);
    serialization.ShowType = ShowTypeOptions.TypeName;
    var expressionAsString = serialization.Serialize();
    System.Console.WriteLine(expressionAsString);
}
```

The output below shows how was our customization, note that the value of the `E` property has been truncated and is there a parentheses including the root item. In addition, all items are displaying the name of the type in short form:

```
("<>f__AnonymousType0`5.-438126044" + "String.A: A" + "String.B: B" + "String.C: C" + "String.D: D" + "String.E: BIG")
```

Finally, we highlight that when an expression is created using the method `AsExpression()` , we will have on `DefaultSerialize` property a pre-configured instance of class: `ComplexEntityExpressionSerializer<T>` .

### <a name="impl-serialization-complex-itens-serialize" />Customizing the serialization of the items

The serialization items are responsible for the serialization of the member name and obtaining the item type in the expression. The returned type will be used by the class `ValueFormatter` when it is primitive. For complex types will be the display of identification as we saw in the topic [Complex entities in text-form complex types](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex-complex).

The property `ItemsSerialize` will be used to find the best serializer for each item of expression.

The serialization items must inherit from `IEntitySerialize` interface and the method `CanSerialize` is responsible for determining if the item of expression may or may not be serialized. When the method `CanSerialize` returns `true` then the method `GetSerializeInfo` is called to obtain the serialization information for the item.

```csharp
public interface IEntitySerialize
{
    bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
    (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
}
```

The order of serialization items is of the utmost importance, since the last of the list will be used in the event of a tie, in other words, if three return `true` , the last on the list will be used.

By default, we have some serializers are defined and all of them are already sorted on the property `ItemsSerialize` to prevent errors.

1. `ObjectSerialize`: Is the default serialization class, if no other is found, this will be used. It returns the entity type and the value `null` for the property `ContainerName` .
2. `PropertySerialize`: This class is used for items that derive from the properties. The type of the property and your name will be used.
3. `FieldSerialize`: This class is used for items that are derived from the fields. The type of the field and your name will be used.
4. `ArrayItemSerialize`: This class is used for items that derive from `array` . She returns in `ContainerName` the position property of the item in the form:`[{position1},{position2}]`
5. `DynamicItemSerialize`: This class is used for items that derive from dynamic classes.
6. `CollectionItemSerialize`: This class is used for items that derive from Collections. She returns in `ContainerName` the position property in the format:`[{position1},{position2}]`

In the following example we will see the creation of a new member named reader `MethodReader` that will be responsible for reading the method `HelloWorld` and create a new entity type in the expression called `MethodEntity` . Based on this new type of entity we create a serializer called `MethodSerialize` which will have the function to serialize the new type to the following format:

```
MethodName(parameters)
```

```csharp
public void SerializationComplex3()
{
    var factory = new ComplexExpressionFactory();
    factory.MemberReaders.Add(new MethodReader());

    var model = new Model();
    var expression = model.AsExpression(factory);
    var serialization = expression.GetSerializer<ComplexEntityExpressionSerializer>();
    serialization.ItemsSerialize.Add(new MethodSerialize());
    System.Console.WriteLine(serialization.Serialize());
}

private class Model
{
    public string HelloWorld(string val1, string val2)
    {
        return $"{val1}-{val2}";
    }
}

public class MethodReader : IMemberReader
{
    public IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, GraphExpression.Expression<object> expression, object entity)
    {
        if (entity is Model)
        {
            var method = entity
                .GetType()
                .GetMethods().Where(f => f.Name == "HelloWorld")
                .First();

            var parameters = new object[] { "value1", "value2" };
            var methodValue = method.Invoke(entity, parameters);
            yield return new MethodEntity(expression, method, parameters, methodValue);
        }
    }
}

public class MethodSerialize : IEntitySerialize
{
    public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
    {
        return item is MethodEntity;
    }

    public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
    {
        var cast = (MethodEntity)item;
        return (
            item.Entity?.GetType(),
            $"{cast.MethodInfo.Name}({string.Join(",", cast.Parameters)})"
        );
    }
}

private class MethodEntity : ComplexEntity
{
    public MethodInfo MethodInfo { get; }
    public object[] Parameters { get; }

    public MethodEntity(Expression<object> expression, MethodInfo methodInfo, object[] parameters, object value)
        : base(expression)
    {
        this.MethodInfo = methodInfo;
        this.Parameters = parameters;
        this.Entity = value;
    }
}
```

The output shows that the new methods serializer displayed the name of the method in the expected format and also the values of the parameters inside the parentheses.

```
"Model.43942917" + "HelloWorld(value1,value2): value1-value2"
```

# <a name="impl-deserialization" />Deserialization

Deserialization is the process of transformation of a text to a specified entity. We split it up deserialization into two types: **deserialization circular entity** and **deserialization of complex entities**.

The deserialization process uses as a base the compiler `Roslyn` . This has pros and cons.

The good part is that we don't have to reimplement the reading of mathematical expressions, because with the Roslyn is possible convert `string` , which is a mathematical expression, in a `SystaxTree` and do the build for a mathematical expression.

The class `RoslynExpressionDeserializer<T>` is responsible for making the `string` conversion to a mathematical expression.

The inferred type `T` must contain an `+` operator overload.

The downside of this approach is that there is a slowness in this process in your first run. For now, we don't have a solution to this problem, but we are following the evolution of the compiler Roslyn.

## <a name="impl-deserialization-circular" />Deserialization circular

Deserialization of circular entities is made by the class `CircularEntityExpressionDeserializer<T>` . The method `Deserialize` is responsible for deserializing. There are a few variations of this method:

The inferred type `T` must contain an `+` operator overload, because that is the operator that the logic of the sum or subtraction will be contained.

**1)** the first method requires only the expression in text form. Based on this expression and the inferred type in `CircularEntityExpressionDeserializer` class it is possible to make deserialization. There are two variants of this method, a synchronous and another asynchronously.

The inferred type must have (necessarily) a parameter in your constructor of type: `string` . This parameter is the name of the entity.

```csharp
public T Deserialize(string expression);
public async Task<T> DeserializeAsync(string expression);
```

In the following example we will see how simple deserialization of circular entities:

```csharp
public void DeserializationCircular1()
{
    var expressionAsString = "A + B + (C + D)";

    var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
    var A = serializer.Deserialize(expressionAsString);

    // A
    System.Console.WriteLine(A.Name);

    // B
    System.Console.WriteLine(A.Children[0].Name);

    // C
    System.Console.WriteLine(A.Children[1].Name);

    // C - children 
    System.Console.WriteLine(A.Children[1].Children[0].Name);
}

public class CircularEntity
{
    public string Name { get; private set; }
    public CircularEntity(string identity) 
    { 
        this.Name = identity;
    }

    ... continue
}
```

The output of this example shows that all levels have been created and populated with the name used in the expression. Note that the class `CircularEntity` contains a parameter `string` type. This is necessary so that these methods work:

```
A
B
C
D
```

**2)** the second overload requires the expression and a method that is the "circular entity factory", that is, with this overload to your circular class does not need to be forced to have a constructor with a parameter, since the creation of the class will be made in this method. There are two variants of this method, a synchronous and another asynchronously.

```csharp
public T Deserialize(string expression, Func<string, T> createEntityCallback);
public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback);
```

**3)** the third overload of this method requires the expression and an instance of the class `CircularEntityFactory<T>` . This class allows custom functions are used in the expression in text form. There are two variants of this method, a synchronous and another asynchronously.

```csharp
public T Deserialize(string expression, CircularEntityFactory<T> factory);
public async Task<T> DeserializeAsync(string expression, CircularEntityFactory<T> factory);
```

In the following example we will show how to use custom functions in the expression. We will create a class `CircularEntityFactoryExtend` that inherits from the call `CircularEntityFactory<CircularEntity>` . In this class we will create the `NewEntity(string name)` method that will be used in the expression in text form.

```csharp
public void DeserializationCircular2()
{
    var strExp = "NewEntity('my entity name1') + NewEntity('my entity name2')";
    var factory = new CircularEntityFactoryExtend();
    var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
    var root = serializer.Deserialize(strExp, factory);
    var entities = factory.Entities.Values.ToList();

    System.Console.WriteLine(root.Name);
    System.Console.WriteLine(root.Children[0].Name);
}

public class CircularEntityFactoryExtend : CircularEntityFactory<CircularEntity>
{
    public CircularEntity NewEntity(string name)
    {
        return new CircularEntity(name);
    }
}
```

The output is the name of the circular entities that have been created:

```
my entity name1
my entity name2
```

## <a name="impl-deserialization-complex" />Deserialization complex

Deserialization of complex entities is made by the class `ComplexEntityExpressionDeserializer` . The method `Deserialize` is responsible for deserializing. There are a few variations of this method:

**1)** the first method requires only the expression in text form. Based on this expression and the inferred type in the method `Deserialize` it is possible to make deserialization. There are two variants of this method, a synchronous and another asynchronously.

```csharp
public T Deserialize<T>(string expression);
public async Task<T> DeserializeAsync<T>(string expression);
```

In the example below we will deserialize an expression to an array of integers. Note that the `int[]` type is being inferred in the method `Deserialize<int[]>` .

```csharp
public void DeserializationComplex1()
{
    var expressionAsString = "\"Int32[].1\" + \"[0]: 1\" + \"[1]: 2\" + \"[2]: 3\"";
    var deserializer = new ComplexEntityExpressionDeserializer();
    var array = deserializer.Deserialize<int[]>(expressionAsString);
    System.Console.WriteLine(array[0]);
    System.Console.WriteLine(array[1]);
    System.Console.WriteLine(array[2]);
}
```

**2)** the second method has the same purpose of the first method, the only difference is that the guy won't be inferred in the method and the `type` parameter:

```csharp
public object Deserialize(string expression, Type type = null);
public async Task<object> DeserializeAsync(string expression, Type type = null);
```

**3)** the third method receives the parameter `factory` , this parameter should be used if you need some customization in the creation of complex entities. In short, this process is exactly the same as the process of the topic [Creating complex entities with graph and mathematical expression](https://github.com/juniorgasparotto/GraphExpression#impl-factory-entity-complex). Internally, the compiler will transform each item of expression in the class `Entity` and then follow the same steps that we've seen in this topic:

```csharp
public T Deserialize<T>(string expression, ComplexEntityFactory factory);
public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactory factory);
```

**4)** This last has the same overload overload function above, the only difference is that we don't have an inferred type, that is, the type will be defined in the class `ComplexEntityFactory` or if he has not, the result will be a class `ExpandObject` -type:

```csharp
public object Deserialize(string expression, ComplexEntityFactory factory);
public async Task<object> DeserializeAsync(string expression, ComplexEntityFactory factory);
```

# <a name="impl-graph-info" />Information from the graph of an entity

Classes: `Expression<T>` and `EntityItem<T>` bring some information of graph theory to help understand a little the relationship between the entities.

**1)** `Expression<T>` : Contains the property `Graph` that isolates the General information of the graph, it contains the following properties and definitions:

* `IReadOnlyList<Edge<T>> Edges`: This property contains all the edges of the graph.
  * `class Edge<T>`: This class represents a connection between two entities (A and B), we have some properties and a method that help extract some information.
    * `decimal Weight`: Determines the weight of the edge, if necessary, do the padding after the creation of the expression.
    * `EntityItem<T> Source`: Determines the parent item of the edge
    * `EntityItem<T> Target`: Determines the child item of the edge
    * `IsLoop`: Determines if the `Source` equals `Target` , if yes, this edge is in looping.
    * `bool IsAntiparallel(Edge<T> compare)`: Determines if two edges are antiparallel, meaning if an edge compared to the other has the same entities, however, in reverse order:
      * `A -> B`
      * `B -> A`
* `IReadOnlyList<Vertex<T>> Vertexes`: Contains the list of all entities of the graph
  * `class Vertex<T>`: Represents a vertex, i.e. an entity
    * `long Id`: This ID is generated automatically using the static class `VertexContainer` .
    * `T Entity`: Represents the entity of the vertex.
    * `int CountVisited`: Determines how many times the vertex was used in the graph.
    * `IReadOnlyList<EntityItem<T>> Parents`: Lists all parents of a vertex in the graph.
    * `IReadOnlyList<EntityItem<T>> Children`: Lists all the children of the entity.
    * `int Indegrees`: Determines the indegree (number of parents)
    * `int Outdegrees`: Determines the outdegree (number of children)
    * `int Degrees`: Determines the degree of the vertex (sum of indegree with outdegree)
    * `bool IsSink`: Checks if the vertex is a leaf, i.e. does not contain children.
    * `bool IsSource`: Checks if the vertex is the root of the graph.
    * `bool IsIsolated`: Checks if the vertex does not contain father and no children, that is, it is a root item.
* `IReadOnlyList<Path<T>> Paths`: Contains a list of all the paths end of graph
  * `class Path<T>`: Represents a path that starts at the root and goes up to the vertex.
    * `IEnumerable<EntityItem<T>> Items`: Lists all the items in the path that begins at the root to the vertex.
    * `string Identity`: This is the identification of the way, this ID uses the `Id` of each vertex and use the following pattern:
      * Format:`[id-root].[id-parent].[id-instance]`
      * Example:`[0].[1].[2]`
    * `PathType PathType`: Determines the type of the path
      * `Circuit`: Occurs when the root vertex is equal to the current vertex.
      * `Circle`: Occurs when the parent vertex is equal to the current vertex.
      * `Simple`: Is default type, i.e. when it is not and not circular circuit
    * `bool ContainsPath(Path<T> pathTest)`: Checks whether a path exists within the path of the instance. Basically, this method makes a comparison at the property `Identity` of the two paths, i.e. If a path contain the ID of the other path is because this path is contained in another. Example:
      * `Path 1`:`[0].[1].[2].[3]`
      * `Path 2`:`[2].[3]`
      * Using text comparison, we see that the second path is contained in the first path:
        * `"[0].[1].[2].[3]".Constains("[2].[3]") = true`

**2)** `EntityItem<T>` : Contains the following properties:

* `Vertex<T> Vertex`: Represents the apex of the item of expression.
* `Edge<T> Edge`: Represents the edge of the item of expression.
* `Path<T> Path`: Represents the path of the item in the item of expression.

In the example below we display the main information of the graph. We're not going to show all the information, since many are self explanatory:

```csharp
public void GraphInfo()
{
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    A = A + B + (C + D);
    var expressionA = A.AsExpression(f => f.Children, e => e.Name);

    foreach (Edge<CircularEntity> edge in expressionA.Graph.Edges)
        System.Console.WriteLine(edge.ToString());

    foreach (Path<CircularEntity> path in expressionA.Graph.Paths)
        System.Console.WriteLine(path.ToString());

    foreach (EntityItem<CircularEntity> item in expressionA)
        System.Console.WriteLine($"{item.ToString()} => {item.Path}");
}
```

The first exit we can see all the edges of the graph, note that the first (root) contains no Dad:

```
 , A
A, B
A, C
C, D
```

The second output, we see all the paths end of graph, i.e. from root to ends:

```
[A].[B]
[A].[C].[D]
```

At the third exit, we see all the paths of all items:

```
A => [A]
B => [A].[B]
C => [A].[C]
D => [A].[C].[D]
```

## Removing duplicate graphs

The method `IEnumerable<Graph<T>>.RemoveCoexistents` aims to remove graphs that are contained within another graph.

In the example below we have graphs `A` and `C` . The `C` graph is contained in the graph `A` and we will use this method to remove the smaller graph, in case the `C` graph.

```csharp
public void GraphRemoveCoexistents()
{
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    A = A + B + (C + D);

    var graphs = new List<Graph<CircularEntity>>
    {
        A.AsExpression(f=>f.Children, e => e.Name).Graph,
        C.AsExpression(f=>f.Children, e => e.Name).Graph
    };

    System.Console.WriteLine($"-> A: HashCode: {graphs[0].GetHashCode()}");
    foreach (Path<CircularEntity> path in graphs[0].Paths)
        System.Console.WriteLine(path.ToString());

    System.Console.WriteLine($"-> B: HashCode: {graphs[1].GetHashCode()}");
    foreach (Path<CircularEntity> path in graphs[1].Paths)
        System.Console.WriteLine(path.ToString());

    var graphsNonDuplicates = graphs.RemoveCoexistents();
    foreach(var graph in graphsNonDuplicates)
    {
        System.Console.WriteLine($"-> A: HashCode: {graph.GetHashCode()}");
        foreach (Path<CircularEntity> path in graph.Paths)
            System.Console.WriteLine(path.ToString());
    }
}
```

The first exit, we see the `A` graph:

```
-> A: HashCode: 32854180
[A].[B]
[A].[C].[D]
```

The second output, see the `B` graph:

```
-> B: HashCode: 27252167
[C].[D]

```

The third output shows which graph were maintained after removal of coexistence. As the `A` graph contained the `C` graph, then only the `A` graph was maintained.

```
-> HashCode not duplicates: 32854180
```

In addition, we still have the method `IEnumerable<Path<T>>.RemoveCoexistents()` that has the same purpose, however, it removes the repeated paths in a collection of paths:

```csharp
IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source)
```

## Container of vertices

The class `VertexContainer<T>` contains a `static ConcurrentBag<EntityId> Vertexes` property that is responsible for storing all the vertices of the application.

Keep a static property is not the best practice, but it's the only way to make all vertices in different `Expression<object>` instances have the same ID. Without it you cannot use extension methods:

```csharp
IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source);
static IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source);
```

I plan in the future to remove this static property and exchange it for an instance injected. This will reduce the power of the functionality, but leaves the instances under control.

## Disabling the graph information

The property `GraphExpression.Expression<T>.EnableGraphInfo` determines whether the graph information collection is enabled. By default it is on, but performance issues, you can disable it.

Recalling that by doing this, all the information of the graphs will be nil.

# <a name="donate" />Donations

GraphExpression is an open source project. Starting in 2017, many hours have been invested in the creation and evolution of this project.

If the GraphExpression was useful for you, or if you want to see it evolve increasingly, consider making a small donation (any amount). Help us also with suggestions and possible problems.

Anyway, we thank you for have come up here;)

**Bitcoin:**

_19DmxWBNcaUGjm2PQAuMBD4Y8ZbrGyMLzK_

![bitcoinkey](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/bitcoinkey.png)

# <a name="license" />License

The MIT License (MIT)

Copyright (c) 2018 Glauber Donizeti Gasparotto Junior

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN THE EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

* * *

<sub>This text was translated by a machine</sub>

https://github.com/juniorgasparotto/MarkdownGenerator