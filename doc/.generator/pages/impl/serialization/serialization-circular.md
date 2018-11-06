## Serialização circular <header-set anchor-name="impl-serialization-circular" />

A serialização de entidades circulares é feita pela classe `CircularEntityExpressionSerializer`. 

Essa classe herda da classe abstrata `ExpressionSerializerBase<T>` que tem como responsabilidade compor a base matemática de uma expressão de grafos seja ela circular ou complexa. Essa composição é feita pelo método `Serialize()`. Ele é o responsável por criar os parenteses, adicionar os caracteres de soma e etc.

A classe `CircularEntityExpressionSerializer` ao implementar essa classe base deve obrigatoriamente sobrescrever o método `SerializeItem` que será o responsável por serializar cada item da expressão.

O construtor obriga que sejam passados 2 parâmetros:

```csharp
CircularEntityExpressionSerializer(Expression<T> expression, Func<T, object> entityNameCallback)
```

1. `expression`: Indica qual será a expressão circular que deve ser serializada.
2. `entityNameCallback`: Indica qual será o texto usado em cada item da expressão. Se for passado `null` então o método `ToString()` de cada `EntityItem` será utilizado.

No exemplo abaixo veremos uma forma de serializar uma expressão circular

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

A saída será uma expressão circular cujo o nome de cada item será a propriedade `Name` da classe `CircularEntity`:

```
A + B + (C + D)
```

Algumas propriedades de customizações podem ser utilizadas antes da serialização. Todas essas propriedades estão na classe base, ou seja, elas valem para expressões complexas também.

* `bool EncloseParenthesisInRoot`: Essa propriedade determina se existirá parenteses englobando a entidade raiz, o padrão é não existir.
* `bool ForceQuoteEvenWhenValidIdentified`: Essa propriedade força o uso de aspas mesmo quando o nome da entidade for um nome válido. Consideramos um nome válido aquele que não contém espaços e nem caracteres especiais que a linguagem `C#` não suporta em nomes de variáveis. Nomes que fazem referência a termos reservados do `C#` também são considerados inválidos, por exemplo: `bool`, `while` e etc. Caso um nome seja inválido então o uso das aspas será usado, caso um nome seja válido então o uso das aspas dependerá do valor dessa propriedade. 
    * `true`: Força o uso de aspas até para nomes válidos
    * `false`: Exibe as aspas apenas para nomes inválidos. Esse é o valor padrão dessa propriedade.
* `IValueFormatter ValueFormatter`: Essa propriedade indica qual será o formatador de valor para cada item da expressão, por padrão temos apenas dois, mas é possível a criação de um formatador customizado usando a interface `IValueFormatter`.
    * `DefaultValueFormatter`: Esse formatador é usado como padrão para qualquer tipo primitivo. 
        * Para tipos de data o padrão usado será `yyyy-MM-ddTHH:mm:ss.fffzzz`
        * Para tipos booleanos o padrão será `true|false`
        * Os demais tipos serão convertidos em texto usando a cultura: `CultureInfo.InvariantCulture`.
    * `TruncateFormatter`: Esse formatador pode ser usado quando o nome da entidade é muito grande e seja necessário trunca-lo. Isso significa que nomes muitos grandes serão reduzidos de acordo com o tamanho especificado. Esse formatador só será aplicado para tipos de textos (`string`).

No exemplo a seguir vamos forçar o uso de parenteses no item raiz, forçar o uso de aspas para nomes válidos e também truncar nomes que passem de 3 caracteres:

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

A saída abaixo mostra como ficou nossa customização, note que o nome `BigName` foi truncado, todos os itens agora tem aspas e existe um parenteses englobando o item raiz.

```
("A" + "B" + ("C" + "Big"))
```

Por fim, destacamos que quando uma expressão é criada usando o método `AsExpression(c => c.Children)`, teremos na propriedade `DefaultSerialize` uma instância da classe `CircularEntityExpressionSerializer<T>` pré-configurada. 

Contudo, essa propriedade retornará o tipo da interface `ISerialize<T>` sendo necessário fazer a conversão para o serializador circular ou utilizar o método abaixo que fará a conversão por você: 

```csharp
expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
```