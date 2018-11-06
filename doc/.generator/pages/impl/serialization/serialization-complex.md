## Serialização Complexa <header-set anchor-name="impl-serialization-complex" />

A serialização de entidades complexas é feita pela classe `ComplexEntityExpressionSerializer`. Essa classe deve respeitar as regras que vimos nos tópicos <anchor-get name="impl-factory-entity-complex-primitive" />, <anchor-get name="impl-factory-entity-complex-complex" /> e <anchor-get name="impl-factory-entity-complex-collections" />.

Essa classe herda da classe abstrata `ExpressionSerializerBase<object>` que tem como responsabilidade compor a base matemática de uma expressão de grafos seja ela circular ou complexa. Essa composição é feita pelo método `Serialize()`. Ele é o responsável por criar os parenteses, adicionar os caracteres de soma e etc.

A classe `ComplexEntityExpressionSerializer` ao implementar essa classe base deve obrigatoriamente sobrescrever o método `SerializeItem` que será o responsável por serializar cada item da expressão.

O construtor obriga que seja passado uma instância de uma expressão.

```csharp
ComplexEntityExpressionSerializer(Expression<object> expression)
```

1. `expression`: Indica qual será a expressão complexa que deve ser serializada.

No exemplo abaixo veremos uma forma de serializar uma expressão complexa:

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

A saída será uma expressão complexa contendo todas as propriedades com seus respectivos valores da entidade anônima:

```
"<>f__AnonymousType0`5.845912156" + "A: A" + "B: B" + "C: C" + "D: D" + "E: E"
```

Algumas propriedades de customizações podem ser utilizadas antes da serialização:

* `bool EncloseParenthesisInRoot`: Tem a mesma função das expressões circulares.
* `bool ForceQuoteEvenWhenValidIdentified`:  Tem a mesma função das expressões circulares.
* `IValueFormatter ValueFormatter`: Tem a mesma função das expressões circulares.
* `GetEntityIdCallback`: Propriedade que retorna a identificação de uma entidade, por padrão, usamos o método `GetHashCode()` do próprio `C#`.
* `ItemsSerialize`: Propriedade que contém uma lista da interface `IEntitySerialize`. Essa propriedade é a principal alternativa para customizar a serialização e veremos isso nos próximos tópicos.
* `ShowType`: Determina como será a exibição do tipo em cada item da expressão
    * `None`: Não exibe o tipo para nenhum item.
    * `TypeNameOnlyInRoot`: Exibe o nome do tipo (na forma curta) apenas para o item raiz.
    * `TypeName`: Exibe o nome do tipo (na forma curta) para todos os itens da expressão.
    * `FullTypeName`: Exibe o nome completo do tipo para todos os itens da expressão.

No exemplo a seguir vamos forçar o uso de parenteses no item raiz, forçar a exibir o tipo em todos os itens da expressão e também truncar valores que passem de 3 caracteres:

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

A saída abaixo mostra como ficou nossa customização, note que o valor da propriedade `E` foi truncada e existe um parenteses englobando o item raiz. Além disso, todos os itens estão exibindo o nome do tipo na forma curta:

```
("<>f__AnonymousType0`5.-438126044" + "String.A: A" + "String.B: B" + "String.C: C" + "String.D: D" + "String.E: BIG")
```

Destacamos que quando uma expressão é criada usando o método `AsExpression()`, teremos na propriedade `DefaultSerialize` uma instância da classe `ComplexEntityExpressionSerializer<T>` pré-configurada.

### Customizando a serialização dos itens <header-set anchor-name="impl-serialization-complex-itens-serialize" />

Os itens de serialização são os responsáveis pela serialização do nome do membro e obtenção do tipo do item na expressão. O tipo retornado será usado pela classe `ValueFormatter` quando for primitivo. Para tipos complexos será mantida a exibição da identificação como vimos no tópico <anchor-get name="impl-factory-entity-complex-complex" />.

A propriedade `ItemsSerialize` será usada para encontrar o melhor serializador para cada item da expressão.

Os itens de serialização devem herdar da interface `IEntitySerialize` e o método `CanSerialize` é o responsável por determinar se o item da expressão pode ou não ser serializado. Quando o método `CanSerialize` retornar `true` então o método `GetSerializeInfo` será chamado para obter as informações da serialização do item.


```csharp
public interface IEntitySerialize
{
    bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
    (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
}
```

A ordem dos itens de serialização é de extrema importância, isso porque o último serializador será usado em caso de desempate, ou seja, se três serializadores retornarem `true`, o último será usado.

Por padrão, temos alguns serializadores de itens definidos e todos eles já estão ordenados na propriedade `ItemsSerialize` para que não aja erros de serialização.

1. `ObjectSerialize`: É a classe de serialização padrão, caso nenhum outro seja encontrado, este será usado. Ele retorna o tipo da entidade e o valor `null` para a propriedade `ContainerName`.
1. `PropertySerialize`: Esse classe é usada para itens que derivam de propriedades. Será usado o tipo da propriedade e o seu nome como retorno.
1. `FieldSerialize`: Esse classe é usada para itens que derivam de campos. Será usado o tipo do campo e o seu nome como retorno.
1. `ArrayItemSerialize`: Esse classe é usada para itens que derivam de arrays. Ela retorna na propriedade `ContainerName` a posição do item no array no formato: `[{position1},{position2}]`
1. `DynamicItemSerialize`: Essa classe é usada para itens que derivam de classes dinâmicas.
1. `CollectionItemSerialize`: Essa classe é usada para itens que derivam de coleções. Ela retorna na propriedade `ContainerName` a posição do item na coleção no formato: `[{position1},{position2}]`

No exemplo a seguir veremos a criação de um novo leitor de membro chamado `MethodReader` que será responsável por ler o método `HelloWorld` e criar um novo tipo de entidade na expressão chamado `MethodEntity`. Com base nesse novo tipo de entidade vamos criar um serializador chamado `MethodSerialize` que terá a função de serializar os itens dos tipos `MethodEntity` para o formato: `MethodName(parameters)`:

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

A saída mostra que o novo serializador de métodos exibiu o nome do método no formato esperado e também os valores dos parâmetros dentro dos parenteses.

```
"Model.43942917" + "HelloWorld(value1,value2): value1-value2"
```