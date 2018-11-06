# Grafos circulares <header-set anchor-name="impl-graph-circular" />

Chamamos de grafos circulares aqueles que contém tipo definido, ou seja, todos os itens são definidos com o mesmo tipo `T`. 

Esse tipo de grafo é presentado pela classe:

```csharp
GraphExpression.Expression<T> : List<EntityItem<T>>
```

Essa classe herda de `List<EntityItem<T>>`, ou seja, ela também é uma coleção da classe `EntityItem<T>`.

No exemplo a seguir vamos converter o objeto para o tipo `Expression<CircularEntity>` e mostrar como ficou a estrutura convertida:

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

**1)** A primeira saída exibe os itens do objeto `expression` que representam como ficou a hierarquia do objeto `A` após a sua criação:

```
[0] => Item: A, Parent: , Previous: , Next: B, Level: 1
  [1] => Item: B, Parent: A, Previous: A, Next: C, Level: 2
  [2] => Item: C, Parent: A, Previous: B, Next: D, Level: 2
    [3] => Item: D, Parent: C, Previous: C, Next: , Level: 3
```

**2)** A segunda saída mostra como ficou a expressão de grafos do objeto `A`:

<anchor-get name="impl-serialization-circular">Clique aqui</anchor-get> para entender como funciona a serialiação de objetos circulares.

```
A + B + (C + D)
```

O método de extensão `AsExpression<T>` é o responsável pela criação da expressão circular. Esse método vai navegar por todos os nós partindo da raiz até o último descendente. Esse método contem os seguintes parâmetros:

* `Func<T, IEnumerable<T>> childrenCallback`: Esse parâmetro determina quais serão os filhos das entidades. É esse parâmetro que vai determinar a continuidade da execução. Todas as entidades do grafo chamarão esse método até que todos sejam navegados. A execução só será interrompida em caso de relações cíclicas.
* `Func<T, object> entityNameCallback`: Esse parâmetro é o responsável por determinar qual será o nome da entidade na serialização ou no modo de depuração. Em nosso exemplo, usamos a propriedade `Name`. Caso esse parâmetro não seja passado, será usado o método `ToString()`.
* `bool deep = false`: Quando `true`, a expressão será criada de forma profunda, ou seja, quando possível, vai repetir entidades que já foram navegadas.

Esse método está disponível em todos os objetos .NET, basta referenciar o namespace `using GraphExpression`.

**Conclusão:**

Nesse tópico vimos como é simples navegar em objetos circulares abrindo caminhos para outras funcionalidades como pesquisas e serializações.

Vejam também o tópico <anchor-get name="impl-factory-entity-circular" />, isso mostrará uma outra forma de criar objetos circulares sem a utilização do método `Add()`.