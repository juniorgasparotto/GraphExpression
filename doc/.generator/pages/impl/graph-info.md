# Informações do grafo de uma entidade <header-set anchor-name="impl-graph-info" />

As classes `Expression<T>` e `EntityItem<T>` trazem algumas informações da teoria de grafos que ajudam a compreender um pouco a relação entre as entidades.

A classe `Expression<T>` trás a propriedade `Graph` que isola as informações gerais do grafo, ela contém as seguintes propriedades e definições:

* `IReadOnlyList<Edge<T>> Edges`: Essa propriedade contém todas as arestas do grafo.
    * `class Edge<T>`: Essa classe representa uma conexão entre duas entidades (A e B), nela temos algumas propriedades e um método que ajudam a extrair algumas informações da ligação.
        * `decimal Weight`: Determina o peso da ligação, caso necessário, faça o preenchimento dela após a criação da expressão.
        * `EntityItem<T> Source`: Determina o item pai da ligação
        * `EntityItem<T> Target`: Determina o item filho da ligação
        * `IsLoop`: Determina se o `Source` é igual ao `Target`, se sim, essa ligação está em looping.
        * `bool IsAntiparallel(Edge<T> compare)`: Determina se duas arestas são antiparalelas, ou seja, se uma aresta comparada com a outra tem as mesmas entidades, porém, em ordem invertida:
            * `A -> B`
            * `B -> A`
* `IReadOnlyList<Vertex<T>> Vertexes`: Contém a lista de todas as entidades do grafo
    * `class Vertex<T>`: Representa um vértice, ou seja, uma entidade
        * `long Id`: Essa identificação é gerada automaticamente usando a classe estática `VertexContainer`.
        * `T Entity`: Representa a entidade do vértice.
        * `int CountVisited`: Determina quantas vezes o vértice foi utilizado no grafo.
        * `IReadOnlyList<EntityItem<T>> Parents`: Lista todos os pais de um vértice no grafo.
        * `IReadOnlyList<EntityItem<T>> Children`: Lista todos os filhos da entidade.
        * `int Indegrees`: Determina o grau de entrada (numero de pais)
        * `int Outdegrees`: Determina o grau de saída (numero de filhos)
        * `int Degrees`: Determina o grau do vértice (somatória de grau de entrada com grau de saída)
        * `bool IsSink`: Verifica se o vértice é uma folha, ou seja, não contém filhos.
        * `bool IsSource`: Verifica se o vértice é a raiz do grafo.
        * `bool IsIsolated`: Verifica se o vértice não contém pai e nem filhos, ou seja, é um item raiz sem filhos.
* `IReadOnlyList<Path<T>> Paths`: Contém a lista de todos os caminhos finais do grafo
    * `class Path<T>`: Representa um caminho que parte da raiz até chegar no vértice
        * `IEnumerable<EntityItem<T>> Items`: Lista todos os itens do caminho partindo do item raiz até o item da instância.
        *  `string Identity`: Essa é a identificação do caminho, essa identificação utiliza o `Id` de cada vértice e útiliza-se do seguinte padrão: 
            * Formato: [id-root].[id-parent].[id-instance]
            * Exemplo: [0].[1].[2]
        * `PathType PathType`: Determina o tipo do caminho
            * `Circuit`: Ocorre quando o vértice raiz é igual vértice da instância
            * `Circle`: Ocorre quando o vértice pai é igual ao vértice da instância
            * `Simple`: É tipo padrão, ou seja, quando não é circuito e nem circular
        * `bool ContainsPath(Path<T> pathTest)`: Verifica se um caminho existe dentro do caminho da instância. Basicamente, esse método faz uma comparação entre strings usado a propriedade `Identity`, ou seja, se um caminho conter a identificação de outro caminho é porque esse caminho está contido no outro.
            * Caminho 1: [0].[1].[2].[3]
            * Caminho 2: [2].[3]
            * O caminho 2 está contido no caminho 1 usando apenas uma verificação de strings: 
                * `"[0].[1].[2].[3]".Constains("[2].[3]") = true`

A classe `EntityItem<T>` trás as seguintes propriedades:

* `Vertex<T> Vertex`: Representa o vértice do item
* `Edge<T> Edge`: Representa a aresta do item
* `Path<T> Path`: Representa o caminho do item

No exemplo abaixo vamos exibir as principais informações do grafo. Não vamos demostrar todas as informações, pois muitas são auto explicativas:

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

Na primeira saída podemos ver todas as arestas do grafo, note que a primeira (raiz) não contém pai:

```
 , A
A, B
A, C
C, D
```

A segunda saída vemos todos os caminhos finais do grafo, ou seja, da raiz até as extremidades:

```
[A].[B]
[A].[C].[D]
```

A terceira saída vemos todos os caminhos de todos os itens:

```
A => [A]
B => [A].[B]
C => [A].[C]
D => [A].[C].[D]
```

## Removendo grafos duplicados

O método `IEnumerable<Graph<T>>.RemoveCoexistents` tem o objetivo de remover grafos que estão contidos dentro de outro grafo.

No exemplo abaixo temos os grafos `A` e `C`. O grafo `C` está contido no grafo `A` e vamos usar esse método para remover o menor grafo, no caso o grafo `C`.

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

A primeira saída mostra como ficou os caminhos do grafo `A`:

```
-> A: HashCode: 32854180
[A].[B]
[A].[C].[D]
```

A segunda saída mostra como ficou os caminhos do grafo `B`:

```
-> B: HashCode: 27252167
[C].[D]

```

A terceira saída mostra quais grafos sobraram após a remoção da coexistência. Como o grafo `A` continha o grafo `C`, então apenas ele sobrou: 

```
-> HashCode not duplicates: 32854180
```

Além disso, ainda temos o método `IEnumerable<Path<T>>.RemoveCoexistents()` que tem o mesmo objetivo, contudo, ele remove os caminhos repetidos em uma coleção de caminhos:

```csharp
IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source)
```

## Container de vértices

A classe `VertexContainer<T>` contém a propriedade `static ConcurrentBag<EntityId> Vertexes` que é responsável por armazenar todas os vértices da aplicação.

Manter uma propriedade estática não é a melhor prática, mas é a única maneira de fazer com que todos os vértices em diferentes instâncias de `Expression<object>` tenham a mesma identificação. Sem isso não é possível utilizar os métodos de extensão:

```csharp
IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source);
static IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source);
```

Planejo no futuro remover essa propriedade estática e troca-la por uma instância injetada. Isso vai reduzir o poder da funcionalidade, mas deixa as instâncias sob controle.

## Desativando as informações de grafos

A propriedade `GraphExpression.Expression<T>.EnableGraphInfo` determina se a coleta das informações de grafos estão ou não ligadas. Por padrão, ela está ligada, mas em caso de problemas de performance é possível desativa-la de forma global. 

Lembrando que ao fazer isso, todas as informações dos grafos estarão nulas.