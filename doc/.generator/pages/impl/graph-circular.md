## Grafos circulares

Chamamos de grafos circulares aqueles que contém tipo definido, ou seja, todos os itens são definidos com o mesmo tipo `T`. Esse tipo de grafo é presentado pela classe:

```csharp
GraphExpression.Expression<T> : List<EntityItem<T>>
```

Essa classe herda de `List<EntityItem<T>>`, ou seja, ela também é uma coleção da classe `EntityItem<T>`.

Um grafo circular é a maneira mais simples de entender como as coisas funcionam. Basicamente, é uma classe que faz referencia para ela mesma. No exemplo a seguir, mostraremos uma forma de implementar o conceito de expressão de grafos sem nenhum framework, usando apenas `C#` e a matemática.

A ideia desse exemplo é criar um grafo circular da classe `CircularEntity` onde o operador de soma vai incrementar a entidade da direita na entidade da esquerda como sendo seu filho. Após a criação, vamos converter o objeto para o tipo `Expression<CircularEntity>` e mostrar como ficou a estrutura convertida:

Note que a entidade `A`, que será a raiz da expressão, será criada de uma forma rápida e simples. E tudo isso usando apenas `C#`.

```csharp
public void GraphCircular()
{
    var A = new CircularEntity("A");
    var B = new CircularEntity("B");
    var C = new CircularEntity("C");
    var D = new CircularEntity("D");

    // ACTION: ADD
    A = A + B + (C + D);

    // PRINT 'A'
    Expression<CircularEntity> expression = A.AsExpression(e => e.Children, entityNameCallback: o => o.Name);
    foreach (EntityItem<CircularEntity> item in expression)
    {
        var ident = new string(' ', item.Level * 2);
        var output = $"{ident}[{item.Index}] => Item: {item.Entity.Name}, Parent: {item.Parent?.Entity.Name}, Previous: {item.Previous?.Entity.Name}, Next: {item.Next?.Entity.Name}, Level: {item.Level}";
        System.Console.WriteLine(output);
    }

    System.Console.WriteLine(expression.DefaultSerializer.Serialize());


    // ACTION: REMOVE
    C = C - D;

    // PRINT 'A' AGAIN
    expression = A.AsExpression(e => e.Children, entityNameCallback: o => o.Name);
    foreach (EntityItem<CircularEntity> item in expression)
    {
        var ident = new string(' ', item.Level * 2);
        var output = $"{ident}[{item.Index}] => Item: {item.Entity.Name}, Parent: {item.Parent?.Entity.Name}, Previous: {item.Previous?.Entity.Name}, Next: {item.Next?.Entity.Name}, Level: {item.Level}";
        System.Console.WriteLine(output);
    }

    // PRINT EXPRESSION
    System.Console.WriteLine(expression.DefaultSerializer.Serialize());
}

public class CircularEntity
{
    public string Name { get; private set; }
    public List<CircularEntity> Children { get; } = new List<CircularEntity>();

    public CircularEntity(string identity) => this.Name = identity;

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
}
```

1. A primeira saída exibe os itens do objeto `expression` que representam como está a hierarquia do objeto `A` após a sua criação:

```
[0] => Item: A, Parent: , Previous: , Next: B, Level: 1
  [1] => Item: B, Parent: A, Previous: A, Next: C, Level: 2
  [2] => Item: C, Parent: A, Previous: B, Next: D, Level: 2
    [3] => Item: D, Parent: C, Previous: C, Next: , Level: 3
```

2. A segunda saída mostra como ficou a expressão de grafos do objeto `A`:

```
A + B + (C + D)
```

* O parâmetro `entityNameCallback` é o responsável por determinar qual será o nome a ser exibido na expressão, nesse exemplo usamos a propriedade `Name`, assim a expressão abaixo exibirá o nome de cada entidade em cada posição da expressão.
* Caso esse parâmetro não seja passado, será usado o método `ToString` que existe em qualquer objeto .NET.

3. A terceira saída mostra como ficou a estrutura da expressão após a remoção do objeto filho `D` no objeto pai `C`:

```
[0] => Item: A, Parent: , Previous: , Next: B, Level: 1
  [1] => Item: B, Parent: A, Previous: A, Next: C, Level: 2
  [2] => Item: C, Parent: A, Previous: B, Next: , Level: 2
```

A quarta saída mostra como ficou a expressão após a remoção do objeto filho `D` no objeto pai `C`:

```
A + B + C
```

<anchor-get name="serialization-circular">Clique aqui</anchor-get> para entender como funciona a serialiação de objetos circulares.