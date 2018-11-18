## Desserialização complexa <header-set anchor-name="impl-deserialization-complex" />

A desserialização de entidades complexas é feita pela classe `ComplexEntityExpressionDeserializer`. O método `Deserialize` é o responsável pela desserialização. Existem algumas variações desse método:

**1)** O primeiro método necessita apenas da expressão em forma de texto. Com base nessa expressão e no tipo inferido no método `Deserialize` é possível fazer a desserialização. Existem duas variantes desse método, uma síncrona e outra assíncrona.

```csharp
public T Deserialize<T>(string expression);
public async Task<T> DeserializeAsync<T>(string expression);
```

No exemplo abaixo vamos deserializar uma expressão para um array de inteiros. Note que o tipo `int[]` está sendo inferido no método `Deserialize<int[]>`.

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

**2)** O segundo método tem o mesmo objetivo do primeiro método, a única diferença é que o tipo não será inferido no método e sim no parâmetro `type`:

```csharp
public object Deserialize(string expression, Type type = null);
public async Task<object> DeserializeAsync(string expression, Type type = null);
```

**3)** O terceiro método recebe o parâmetro `factory`, esse parâmetro deve ser usado se for necessário alguma customização na criação das entidades complexas. Em resumo, esse processo é exatamente igual ao processo do tópico <anchor-get name="impl-factory-entity-complex" />. Internamente, o compilador transformará cada item da expressão na classe `Entity` e depois seguirá os mesmos passos que já vimos nesse tópico:

```csharp
public T Deserialize<T>(string expression, ComplexEntityFactory factory);
public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactory factory);
```

**4)** Essa última sobrecarga tem a mesma função da sobrecarga acima, a única diferença é que não temos um tipo inferido, ou seja, o tipo estará definido na classe `ComplexEntityFactory` ou se não tiver, o resultado será uma classe do tipo `ExpandObject`:

```csharp
public object Deserialize(string expression, ComplexEntityFactory factory);
public async Task<object> DeserializeAsync(string expression, ComplexEntityFactory factory);
```