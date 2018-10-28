# Criando entidades complexas com expressão de grafos e a matemática <header-set anchor-name="impl-factory-entity-complex" />

Criar entidades complexas com expressão de grafos não é uma tarefa simples como vimos nos grafos circulares. É necessário uma estrutura de classes robusta e o uso de reflexão para compor as entidades.

Vejamos um exemplo de como criar uma classe complexa do tipo `CircularEntity` usando apenas expressão de grafos. A ideia será atribuir um valor na propriedade `Name`.

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

A saída mostra que a propriedade `Name` foi preenchida:

```
Entity name ;)
```

Note que estamos usando expressões matemáticas e montamos a entidade do tipo `CircularEntity` sem usar o comando `new` do `C#`.

A ordem da expressão é a mesma das entidades circulares, ou seja, o item da esquerda da expressão é o item pai do item da direita da expressão. Por ser um tipo complexo, o item da esquerda é a instância e o item da direita é o membro. 

Na linguagem `C#` temos dois tipos de membros: **Propriedades** e **Campos** e ambos podem receber valores independente da sua visibilidade.

## Entendendo a classe `Entity`

Essa classe representa uma entidade do grafo e o local onde ela pode estar. Cada local é representado por um construtor especifico e veremos isso a seguir:

1. Esse construtor representa um tipo complexo localizado na raiz da expressão, o parâmetro `complexEntityId` é obrigatório e é usado para atribuir uma identificação para a entidade. Essa identificação é importante pois você pode querer utilizar essa instância em outro local do grafo.

```csharp
Entity(int complexEntityId) 
```

2. O segundo construtor cria uma entidade no qual o seu local será um membro, ou seja, pode ser uma propriedade ou um campo. O parâmetro `Name` vai definir o nome do membro. O parâmetro `complexEntityId` vai atribuir a esse membro a entidade que corresponde a esse ID. 

```csharp
Entity(string name, int complexEntityId)
```

No exemplo abaixo, veremos o uso dos dois primeiros construtores onde vamos criar um objeto do tipo `MyClass` com o ID igual a `0` e vamos atribuir na propriedade `Child` a entidade cuja a identificação também é igual a `0`, ou seja, a própria entidade raiz.

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

A saída compara se as entidades são exatamente as mesmas e o resultado será positivo:

```
True
```

3. O terceiro construtor deve ser usado quando você precisa atribuir um valor que não seja uma referência, ou seja, qualquer valor de tipos primitivos devem ser passados em forma de string para serem atribuídos corretamente.

```csharp
Entity(string name, string value)
```

É importante lembrar que membros que não são públicos também podem ter valores atribuídos. E no nosso próximo exemplo vamos demostrar como atribuir um valor na propriedade `_intValue` que é privada.

Notem também que o valor está em forma de string e isso é importante porque o construtor que aceita um valor inteiro é exclusivo para atribuir referencias e não valores primitivos.

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

Essa saída vai produzir o valor abaixo e foi necessário obter o valor via reflexão devido a visibilidade do campo `_intValue`:

```
1000
```

4. O último construtor é o meio mais básico para criar uma instância. 

```csharp
Entity(string raw)
```








Os locais variam entre: **Raiz do grafo**, **Propriedades** e **Campos**



* Criar entidades complexas usando expressão de grafos
* Atribuir valores via reflexão para entidades que já existem
* Desserialização


É importante dizer esse recurso não foi pensado para criar objetos complexos, para isso nós temos a sintaxe: `new Class()`



 ainda assim a sua usabilidade muito mais código e o uso de reflexão


A criação de grafos complexos com expressões matemáticas não é 

