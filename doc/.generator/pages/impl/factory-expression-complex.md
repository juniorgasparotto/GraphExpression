# Customizando expressões complexas <header-set anchor-name="impl-factory-expression-complex" />

O método `object.AsExpression` é o meio mais rápido para criar uma expressão de grafos. Quando chamado sem nenhum parâmetro, ele criará uma instância do objeto `Expression<object>` que representa uma expressão de grafos complexa.

Esse método está localizado na classe estática `GraphExpression.ComplexExpressionExtensions` e contém os seguintes parâmetros:

* `this object entityRoot`: O objeto que será estendido para comportar o novo método e também para ser a raiz da expressão.
* `ComplexExpressionFactory factory = null`: Esse parâmetro deve ser utilizado quando for necessário trocar ou estender o comportamento padrão de criação de uma expressão de grafos complexa.
* `bool deep = false`: Quando `true`, a expressão será criada de forma profunda, ou seja, quando possível, vai repetir entidades que já foram navegadas.

A classe `GraphExpression.ComplexExpressionFactory` é a classe responsável pela criação/customização de uma expressão complexa. Ela dispõe de algumas propriedades que potencializam a criação da expressão:

* `List<IEntityReader> Readers`: Com essa lista é possível criar ou trocar a leitura de entidades para determinados tipos de objetos.
* `List<IMemberReader> MemberReaders`: Com essa lista é possível criar ou trocar os leitores de membros.
* `Func<object, IEnumerable<PropertyInfo>> GetProperties`: Método que delega ao usuário determinar quais propriedades serão carregadas. Por padrão, será usado a chamada: `entity.GetType().GetProperties()`.
* `Func<object, IEnumerable<FieldInfo>> GetFields`: Método que delega ao usuário determinar quais campos serão carregadas. Por padrão, será usado a chamada: `entity.GetType().GetFields()`.

Você pode usar a instância dessa classe no parâmetro `factory` do método `object.AsExpression` ou simplesmente chamar o método `Build` que retornará a instância da classe `Expression<object>` chegando no mesmo objetivo.

```csharp
Expression<object> Build(object entityRoot, bool deep = false)
```

* `entityRoot`: Entidade que será a raiz da expressão.
* `deep`: Quando `true`, a expressão será criada de forma profunda.

No exemplo abaixo, vamos criar um novo leitor de membros (`MethodReader`)  que terá como objetivo invocar o método `HelloWorld` da classe `Model`. Vamos criar também um novo tipo de entidade complexa (`MethodEntity`) para armazenar o resultado do método. 

Lembrando que os métodos não são suportados por padrão, apenas propriedades e campos são suportadas.

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

_A saída mostra como ficou a expressão com o novo tipo de entidade complexa. O primeiro item da saída é o item raiz e o segundo item é a nova entidade._

```
Model
MethodEntity.HelloWorld(value1, value2)
```

## Leitores de membros

Os leitores de membros herdam da classe `IMemberReader` e tem como principal objetivo ler os membros de uma instância e retornar para a expressão.

```csharp
public interface IMemberReader
{
    IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, Expression<object> expression, object entity);
}
```

Por padrão, temos dois leitores de membros:

* `FieldReader`: Esse leitor retorna os campos da instância usando a propriedade `factory.GetFields`
* `PropertyReader`: Esse leitor retorna as propriedades da instância usando a propriedade `factory.GetProperties`

_Atenção: Os leitores de membros devem ser usados pelos leitores de entidades. Se o seu leitor não fizer esse trabalho então a sua expressão não terá membros._

## Leitores de entidades

Os leitores de entidades são os principais responsáveis pela criação da expressão complexa e a propriedade `Readers` será usada para encontrar o melhor leitor para cada entidade da iteração.

Os leitores de entidades devem herdar da interface `IEntityReader` e o método `CanRead` é o responsável por determinar se a entidade pode ou não ser lida pelo leitor. Quando o método `CanRead` retornar `true` então o método `GetChildren` será chamado e é nesse momento que os itens da expressão serão efetivamente criados.

```csharp
public interface IEntityReader
{
    bool CanRead(ComplexExpressionFactory factory, object entity);
    IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory factory, Expression<object> expression, object entity);
}
```

A ordem dos leitores é de extrema importância, isso porque o último leitor será usado em caso de desempate, ou seja, se três leitores retornarem `true`, o último será usado.

Por padrão, temos alguns leitores de entidades definidos e todos eles já estão ordenados na propriedade `Readers` para que não aja erros de leitura.
 
1. `DefaultReader`: Esse é o leitor padrão de todas as entidades. Sua função é ler os membros de qualquer tipo, com exceção dos tipos que estão nos namespaces `System` e `Microsoft`. Ele é o primeiro leitor, ou seja, se houver algum outro que retorne `true` no método `CanRead`, então esse será ignorado.
2. `CollectionReader`: Esse leitor é responsável por ler objetos que herdam do tipo `ICollection`. Ele é o segundo leitor e sempre deve estar acima dos leitores `ArrayReader` e `DictionaryReader`. Isso é necessário pois objetos dos tipos `Array` e `IDictionary` também herdam de `ICollection` e esses leitores devem ter prioridade sobre este.
3. `ArrayReader`: Esse leitor é responsável por ler objetos do tipo `Array`
4. `DictionaryReader`: Esse leitor é responsável por ler objetos que herdam do tipo `IDictionary`
5. `DynamicReader`: Esse leitor é responsável por ler objetos que herdam do tipo `ExpandoObject`. Esse leitor deve ter prioridade sobre o leitor `DictionaryReader`, por isso ele está posicionado abaixo deste leitor.

Os leitores de entidades são os únicos responsáveis por lerem os membros, ou seja, se o seu leitor customizado quiser manter os membros da entidade, então não esqueça de iterar sobre a propriedade `MemberReaders` e fazer o devido retorno. 

Veja um exemplo de como usar a propriedade `MemberReaders` dentro dos leitores de entidades. Esse código pertence ao leitor: `CollectionReader`

Note que além da leitura da lista os membros também são criados e retornados. Isso é necessário pois podem existir classes de negócios que tenham propriedades publicas e ainda herdem de `ICollection`.

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
