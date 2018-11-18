## Desserialização circular <header-set anchor-name="impl-deserialization-circular" />

A desserialização de entidades circulares é feita pela classe `CircularEntityExpressionDeserializer<T>`. O método `Deserialize` é o responsável pela desserialização. Existem algumas variações desse método:

O tipo inferido `T` deve obrigatoriamente conter uma sobrecarga do operador `+`, pois é nesse operador que a lógica da soma ou subtração estará contida.

**1)**  O primeiro método necessita apenas da expressão em forma de texto. Com base nessa expressão e no tipo inferido na classe `CircularEntityExpressionDeserializer` é possível fazer a desserialização. Existem duas variantes desse método, uma síncrona e outra assíncrona.

O tipo inferido deve ter (obrigatoriamente) um parâmetro em seu construtor do tipo: `string`. Esse parâmetro será o nome da entidade.

```csharp
public T Deserialize(string expression);
public async Task<T> DeserializeAsync(string expression);
```

No exemplo a seguir veremos como é simples a desserialização de entidades circulares:

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

A saída desse exemplo mostra que todos os níveis foram criados e populados com o nome usado na expressão. Note que a classe `CircularEntity` contém um parâmetro do tipo `string`. Isso é necessário para que esses métodos funcionem:

```
A
B
C
D
```

**2)**  A segunda sobrecarga necessita da expressão e de um método que será a "fábrica da entidade circular", ou seja, com essa sobrecarga a sua classe circular não precisa ser obrigada a ter um construtor com um parâmetro, pois a criação da classe será feita nesse método. Existem duas variantes desse método, uma síncrona e outra assíncrona.

```csharp
public T Deserialize(string expression, Func<string, T> createEntityCallback);
public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback);
```

**3)**  A terceira sobrecarga desse método necessita da expressão e de uma instância da classe `CircularEntityFactory<T>`. Essa classe possibilita que funções customizadas sejam utilizadas na expressão em forma de texto. Existem duas variantes desse método, uma síncrona e outra assíncrona.

```csharp
public T Deserialize(string expression, CircularEntityFactory<T> factory);
public async Task<T> DeserializeAsync(string expression, CircularEntityFactory<T> factory);
```

No exemplo a seguir veremos como utilizar funções customizadas na expressão. Vamos criar uma classe chamada `CircularEntityFactoryExtend` que herda da classe `CircularEntityFactory<CircularEntity>`. Nessa nova classe vamos criar o método `NewEntity(string name)` que será utilizada na expressão em forma de texto.

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

A saída será o nome das entidades circulares que foram criadas:

```
my entity name1
my entity name2
```

