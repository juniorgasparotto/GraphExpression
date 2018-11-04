[
![Inglês](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/GraphExpression)
[
![Português](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md)

# <a name="implementation" />Expressão de grafos

Esse framework tem como objetivo implementar o conceito de expressão de grafos na linguagem .NET.

Resumidamente, o conceito de **expressão de grafos** tem como objetivo explorar os benefícios de uma expressão matemática trocando os números por entidades. Com isso, podemos criar uma nova maneira de transportar dados e principalmente criar um novo meio de pesquisa transversal em grafos complexos ou circulares.

Com relação a pesquisa em grafos, esse projeto se inspirou na implementação do `JQuery` para pesquisas de elementos HTML (DOM), unindo assim o conceito de expressão de grafos com a facilidade de uso do `JQuery` para pesquisas transversais.

_Atenção: Esse documento não vai explicar o conceito de expressão de grafos, ele terá como foco apenas no framework `GraphExpression`._

**[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#concept) se você quiser conhecer mais sobre o conceito de expressão de grafos.**

# Instalação

Via [NuGet](https://www.nuget.org/packages/GraphExpression/):

```
Install-Package GraphExpression
```

# <a name="index" />Índice

* [Grafos complexos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-graph-complex)
* [Grafos circulares](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-graph-circular)
* [Pesquisando](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search)
  * [Pesquisa sem referencia](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-without-ref)
  * [Pesquisa com referência](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-with-ref)
  * [Tipos de pesquisas](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-kind)
    * [Antepassados](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-ancertors)
    * [Descendentes](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-descentands)
    * [Filhos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-children)
    * [Irmãos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-search-siblings)
* [Customizando expressões complexas](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-expression-complex)
* [Criando entidades circulares com expressão de grafos e a matemática](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-circular)
* [Criando entidades complexas com expressão de grafos e a matemática](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex)
  * [Entendendo a classe `Entity`](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-class-entity)
  * [Entidades complexas em forma de texto - Tipos primitivos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-primitive)
  * [Entidades complexas em forma de texto - Tipos complexos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-complex)
  * [Entidades complexas em forma de texto - Coleções e arrays](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-collections)
  * [Entendendo a classe `ComplexEntityFactory`](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-class-complex-factory)
  * [Descobridores de tipos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-discovery-types)
  * [Descobridores de membros](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-discovery-members)
  * [Carregadores de valores](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-value-loaders)
  * [Atribuidores de filhos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-child-assign)
* [Serialização](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization)
  * [Serialização Complexa](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization-complex)
    * [Customizando a serialização dos itens](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization-complex-itens-serialize)
  * [Serialização circular](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization-circular)
* [Desserialização](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-deserialization)
  * [Desserialização complexa](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-deserialization-complex)
  * [Desserialização circular](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-deserialization-circular)
* [Informações do grafo de uma entidade](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-graph-info)
* [Doações](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#donate)
* [Licença](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#license)

# <a name="impl-graph-complex" />Grafos complexos

Chamamos de grafos complexos aqueles que não contém tipo definido, ou seja, todos os itens são definidos como `object`.

Esse tipo de grafo é presentado pela classe:

```csharp
GraphExpression.Expression<object> : List<EntityItem<object>>
```

Essa classe herda de `List<EntityItem<object>>`, ou seja, ela também é uma coleção da classe `EntityItem<object>`. A class `EntityItem<object>` representa um item dentro da lista, é nela que existem todas as informações da entidade no grafo.

No exemplo a seguir vamos converter um objeto do tipo `Class1` para o objeto `Expression<object>` e exibir todos os `EntityItem<object>` da estrutura do tipo `Class1`. Na última saída, vamos exibir como ficaria esse objeto no formato de expressão de grafos:

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

1. Na primeira saída podemos visualizar todas as informações da estrutura do tipo `Class1` e também as informações: `Index`, `Parent`, `Next`, `Previous` e `Level` que compõem uma expressão de grafos:

```
  [0] => Item: Class1, Parent: , Previous: , Next: Property.Class1_Prop1, Level: 1
    [1] => Item: Property.Class1_Prop1, Parent: Class1, Previous: Class1, Next: Property.Class1_Prop2, Level: 2
    [2] => Item: Property.Class1_Prop2, Parent: Class1, Previous: Property.Class1_Prop1, Next: Property.Class2_Prop2, Level: 2
      [3] => Item: Property.Class2_Prop2, Parent: Property.Class1_Prop2, Previous: Property.Class1_Prop2, Next: Field.Class2_Field1, Level: 3
      [4] => Item: Field.Class2_Field1, Parent: Property.Class1_Prop2, Previous: Property.Class2_Prop2, Next: , Level: 3
```

* A propriedade `Level` é a responsável por informar em qual nível do grafo está cada item da iteração, possibilitando criar uma saída identada que representa a hierarquia do objeto `model`.
* O método `GetEntity` é apenas um ajudante que imprime o tipo do item e o nome do membro que pode ser uma propriedade ou um campo. Poderíamos também retornar o valor do membro, mas para deixar mais limpo a saída, eliminamos essa informação.
1. Na segunda saída podemos ver como ficou a representação desse objeto em expressão de grafos:

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization-complex) para entender como funciona a serialiação de objetos complexos.

```
"Class1.32854180" + "Class1_Prop1: Value1" + ("Class1_Prop2.36849274" + "Class2_Prop2: Value2" + "Class2_Field1: 1000")
```

O método de extensão `AsExpression` é o responsável pela criação da expressão complexa. Esse método vai navegar por todos os nós partindo da raiz até o último descendente. Esse método contem os seguintes parâmetros:

* `ComplexExpressionFactory factory = null`: Esse parâmetro deve ser utilizado quando for necessário trocar ou estender o comportamento padrão de criação de uma expressão de grafos complexa. O tópico [Customizando expressões complexas](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-expression-complex) trás todas as informações de como estender o comportamento padrão.
* `bool deep = false`: Quando `true`, a expressão será criada de forma profunda, ou seja, quando possível, vai repetir entidades que já foram navegadas.

Esse método está disponível em todos os objetos .NET, basta referenciar o namespace `using GraphExpression`.

**Conclusão:**

Nesse tópico vimos como é simples navegar em objetos complexos abrindo caminhos para outras funcionalidades como pesquisas e serializações.

Vejam também o tópico [Criando entidades complexas com expressão de grafos e a matemática](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex), isso mostrará uma outra forma de criar objetos complexos.

## Elementos padrão de uma expressão de grafos para tipos complexos

Os elementos de uma expressão complexa (`Expression<object>`) podem variar entre os seguintes tipos:

* `ComplexEntity`: Esse tipo é a base de todos os outros tipos de uma expressão complexa. É também o tipo da entidade raiz, ou seja, da primeira entidade da expressão.
* `PropertyEntity`: Determina que o item é uma propriedade.
* `FieldEntity`: Determina que o item é um campo.
* `ArrayItemEntity`: Determina que o item é um item de um `array`, ou seja, a classe pai será do tipo `Array`.
* `CollectionItemEntity`: Determina que o item é um item de uma coleção, ou seja, a classe pai será do tipo `ICollection`.
* `DynamicItemEntity`: Determina que o item é uma propriedade dinâmica, ou seja, a classe pai será do tipo `dynamic`.

Todos esses tipos herdam de `ComplexEntity` que por sua vez herda de `EntityItem<object>`, portanto, além de suas propriedades especificas ainda terão as informações do item na expressão.

Ainda é possível estender a criação de uma expressões complexas, para sabe mais veja o tópico [Customizando expressões complexas](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-expression-complex)

# <a name="impl-graph-circular" />Grafos circulares

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
    public CircularEntity(string identity) => this.Name = identity;
}
```

1. A primeira saída exibe os itens do objeto `expression` que representam como ficou a hierarquia do objeto `A` após a sua criação:

```
[0] => Item: A, Parent: , Previous: , Next: B, Level: 1
  [1] => Item: B, Parent: A, Previous: A, Next: C, Level: 2
  [2] => Item: C, Parent: A, Previous: B, Next: D, Level: 2
    [3] => Item: D, Parent: C, Previous: C, Next: , Level: 3
```

1. A segunda saída mostra como ficou a expressão de grafos do objeto `A`:

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-serialization-circular) para entender como funciona a serialiação de objetos circulares.

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

Vejam também o tópico [Criando entidades circulares com expressão de grafos e a matemática](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-circular), isso mostrará uma outra forma de criar objetos circulares sem a utilização do método `Add()`.

# <a name="impl-search" />Pesquisando

Existem dois tipos de pesquisas no conceito de expressão de grafos: **Pesquisa sem referencia** e **pesquisa com referencia** e que serão abordadas nesse tópico.

_Atenção: Nesse tópico, usaremos o modelo de grafos complexos devido a sua maior complexidade._

**[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#search) para saber mais.**

## <a name="impl-search-without-ref" />Pesquisa sem referencia

A pesquisa sem referencia será feita em uma coleção de entidades, ou seja, cada item da coleção será testado e retornado em caso de sucesso. Por repetir a mesma pesquisa em todos os itens da lista, esse tipo de pesquisa pode trazer duplicidades.

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#search-without-references) para saber mais sobre esse tipo de pesquisa.

Considerando os mesmos modelos do exemplo `GraphComplex`, vamos criar uma pesquisa para retornar todos os descendentes de todas as entidades que sejam uma propriedade e filhos da class `Class2`.

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

_Como esperado, o resultado retornou duas linhas:_

```
Property.Class2_Prop2
Property.Class2_Prop2
```

* Isso ocorreu por que a primeira entidade (raiz) teve todos os seus descendentes testados pelo `filter` e obteve o item: `Property.Class2_Prop2`.
* Depois a segunda entidade `Property.Class1_Prop1` foi testada também, mas ela não tem descendentes.
* A terceira entidade `Property.Class1_Prop2` teve todos os seus descendentes testados e também obteve o item: `Property.Class2_Prop2`.
* Da quarta entidade em diante nenhuma outra retornou positivo.

Caso você queira eliminar as repetições nesse tipo de pesquisa (com coleções), utilize a função do `Linq`:

```csharp
Distinct();
```

## <a name="impl-search-with-ref" />Pesquisa com referência

A pesquisa com referencia será feita usando um item especifico, ou seja, primeiro você precisa localizar o item desejado e a partir dele será feito a pesquisa desejada.

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#search-with-references) para saber mais sobre esse tipo de pesquisa.

Considerando os mesmos modelos do exemplo `GraphComplex`, vamos criar uma pesquisa para retornar todos os descendentes do item raiz que sejam uma propriedade e filhos da class `Class2`.

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

Como esperado, o resultado retornou uma linha:

```
Property.Class2_Prop2
```

* Note que a única mudança foi utilizar o item raiz como referência (`First()`) e isso fez eliminar as duplicidades sem a necessidade do uso do método `Distinct`.
* Isso ocorreu porque apenas um item foi analisado (o item raiz), na pesquisa sem referencias, todos os itens foram analisados fazendo com que o item `Property.Class1_Prop2` também retornasse o mesmo resultado do item raiz.
* De preferência para esse tipo de pesquisa, isso tornará a pesquisa mais rápida.
* A entidade raiz é a melhor opção para isso.

## <a name="impl-search-kind" />Tipos de pesquisas

Por padrão, esse projeto trás os seguintes tipos de pesquisas:

* `Ancestors`: Retorna todos os antepassados de um determinado item.
* `AncestorsUntil`: Retorna todos os antepassados de um determinado item até que o filtro especificado retorne positivo.
* `Descendants`: Retorna todos os descendentes de um determinado item.
* `DescendantsUntil`: Retorna todos os descendentes de um determinado item até que o filtro especificado retorne positivo.
* `Children`: Retorna os filhos de um item.
* `Siblings`: Retorna os irmãos de um item.
* `SiblingsUntil`: Retorna os irmãos de um item até que o filtro especificado retorne positivo.

Todos esses tipos de pesquisas estão disponíveis para qualquer objeto dos tipos:

* `GraphExpression.EntityItem<T>`: Pesquisa com referencia
* `IEnumerable<GraphExpression.EntityItem<T>>`: Pesquisa sem referencia

Também é possível criar pesquisas customizadas usando os métodos de extensões do C#.

**Sem referencias:**

```csharp
public static IEnumerable<EntityItem<T>> Custom<T>(this IEnumerable<EntityItem<T>> references)
```

**Com referencias:**

```csharp
public static IEnumerable<EntityItem<T>> Custom<T>(this EntityItem<T> references)
```

### Delegates das pesquisa

Todos os métodos de pesquisa utilizam os delegates abaixo e que podem ser utilizados usando a classe `Func`

```csharp
public delegate bool EntityItemFilterDelegate<T>(EntityItem<T> item);
public delegate bool EntityItemFilterDelegate2<T>(EntityItem<T> item, int depth);
```

* `EntityItem<T> item`: Esse parâmetro significa o item corrente durante a pesquisa.
* `int depth`: Determina a profundidade do item corrente com relação a sua posição.

### <a name="impl-search-ancertors" />Antepassados

A pesquisa de antepassados é útil para encontrar o pai ou os pais de um item. Temos algumas sobrecargas que serão explicadas a seguir:

1. Essa é a sobrecarga padrão, caso nenhuma parâmetro seja passado então nenhum filtro será aplicado e todos os antepassados serão retornados.

```csharp
IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

* `filter`: Não retorna itens quando o filtro retornar negativo, mas continua a busca até chegar no item raiz. A pesquisa utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.
* `stop`: Determina quando a navegação deve parar, do contrário a navegação deverá ir até o item raiz.
* `depthStart`: Determina a profundidade de inicio que a pesquisa deve começar
* `depthEnd`: Determina a profundidade de fim que a pesquisa deve parar

Nesse exemplo, vamos retornar todos os antepassados do último item da expressão, lembrando que a estrutura é a mesma do exemplo `GraphComplex`:

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

_A primeira saída exibe todos os pais do item referencia._

```
Property.Class1_Prop2
Class1
```

* A ordem de retorno será sempre do antepassado mais próximo, ou seja, o primeiro item da lista de retorno será sempre o pai do item referência.

_A segunda saída exibe apenas o antepassado cujo a profundidade é igual a `1`, ou seja, nesse caso seria o item pai do item referencia:_

```
Property.Class1_Prop2
```

1. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

1. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthStart, int depthEnd)
```

1. A quarta sobrecarga filtra profundidade de fim.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthEnd)
```

1. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os antepassados até que algum antepassado retorne negativo no parâmetro `stop`. Do contrário será retornado todos os itens até a raiz. Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

1. A segunda sobrecarga do método `AncestorsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

### <a name="impl-search-descentands" />Descendentes

A pesquisa de descendentes é útil para encontrar os filhos ou todos os descendentes de um item. Temos algumas sobrecargas que serão explicadas a seguir:

1. Essa é a sobrecarga padrão, caso nenhum parâmetro seja passado então nenhum filtro será aplicado e todos os descendentes serão retornados.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

* `filter`: Não retorna itens quando o filtro retornar negativo, mas continua a busca até chegar no último item. A pesquisa utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.
* `stop`: Determina quando a navegação deve parar, do contrário a navegação deverá ir até o último item.
* `depthStart`: Determina a profundidade de inicio que a pesquisa deve começar
* `depthEnd`: Determina a profundidade de fim que a pesquisa deve parar

Nesse exemplo, vamos retornar todos os descendentes do item raiz cujo a profundidade inicial e final seja igual a `2`, vamos utilizar a mesma estrutura do exemplo `GraphComplex`:

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

_A saída será:_

```
Property.Class2_Prop2
Field.Class2_Field1
```

1. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

1. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
```

1. A quarta sobrecarga filtra profundidade de fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthEnd)
```

1. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os descendentes até que algum descendente retorne negativo no parâmetro `stop`. Do contrário será retornado todos os itens até chegar no último item. Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

1. A segunda sobrecarga do método `DescendantsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

### <a name="impl-search-children" />Filhos

Para retornar os filhos de um item basta usar o método:

```csharp
IEnumerable<EntityItem<T>> Children()
```

Nesse exemplo vamos retornar os filhos do item raiz:

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

_A saída exibirá as duas propriedades que são filhas do item raiz:_

```
Property.Class1_Prop1
Property.Class1_Prop2
```

* Esse método não tem parâmetros, basta utilizar as funções do `Linq` caso necessite de alguma filtragem.
* Esse método é um alias do método `Descendants(int depthStart, int depthEnd)`, no qual será passado os valores fixos `Descendants(1, 1)`.

### <a name="impl-search-siblings" />Irmãos

Essa pesquisa encontra os irmãos de um determinado item. Temos algumas sobrecargas que serão explicadas a seguir:

1. Essa é a sobrecarga padrão, caso nenhum parâmetro seja passado então nenhum filtro será aplicado e todos os descendentes serão retornados.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

* `filter`: Não retorna itens quando o filtro retornar negativo, mas continua a busca até chegar no último irmão ou no primeiro (depende do parâmetro `direction`). A pesquisa utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.
* `stop`: Determina quando a navegação deve parar, do contrário a navegação deverá ir até chegar no último irmão ou no primeiro (depende do parâmetro `direction`).
* `direction`: Esse parâmetro determina em qual direção a navegação deverá ir:
  * `Start`: Determina que a navegação deve iniciar no primeiro irmão á esquerda do item referencia e ir até o último irmão á direita.
  * `Next`: Determina que a navegação deve iniciar no próximo item e seguir até o último irmão á direita.
  * `Previous`: Determina que a navegação deve iniciar no item anterior e seguir até o primeiro irmão á esquerda.
* `positionStart`: Determina a posição de inicio que a pesquisa deve começar.
  * Quando a direção for igual a `Start`, a posição `1` será do primeiro irmão á esquerda do item referencia.
  * Quando a direção for igual a `Next`, a posição `1` será do próximo irmão á direita do item referencia.
  * Quando a direção for igual a `Previous`, a posição `1` será do próximo irmão á esquerda do item referencia.
* `positionEnd`: Determina a posição de fim que a pesquisa deve parar.

Nesse exemplo vamos retornar os irmãos do item cujo o valor é igual a `C` em todas variando as direções.

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

_A primeira saída será retorna todos os irmãos da entidade `C` iniciando do primeiro irmão á esquerda até o último irmão á direita. É importante destacar que o próprio item não é retornado, afinal ele não é irmão dele mesmo._

```
-> Start direction
A: A
B: B
D: D
E: E
```

_A segunda saída retorna todos os irmãos da entidade `C` iniciando do próximo irmão á direita até o último irmão á direita._

```
-> Next direction
D: D
E: E
```

_A terceira saída retorna todos os irmãos da entidade `C` iniciando do irmão anterior até o primeiro irmão á esquerda._

```
-> Previous direction
B: B
A: A
```

1. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

1. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim na direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

1. A quarta sobrecarga filtra profundidade de fim na direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

1. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os irmãos até que algum irmão retorne negativo no parâmetro `stop`. Do contrário será retornado todos os irmãos até chegar no último ou no primeiro (depende do parâmetro `direction`). Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

1. A segunda sobrecarga do método `SiblingsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

# <a name="impl-factory-expression-complex" />Customizando expressões complexas

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

No exemplo abaixo, vamos criar um novo leitor de membros (`MethodReader`) que terá como objetivo invocar o método `HelloWorld` da classe `Model`. Vamos criar também um novo tipo de entidade complexa (`MethodEntity`) para armazenar o resultado do método.

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

# <a name="impl-factory-entity-circular" />Criando entidades circulares com expressão de grafos e a matemática

Uma das vantagens da linguagem `C#` é que ela permite sobrescrever os operadores da matemática deixando a ação da operação para o programador. Com essa ação delegada ao programador é possível utilizar o conceito de expressão de grafos para inserir ou remover uma entidade da outra.

Se você já leu a documentação sobre o conceito de expressão de grafos então você já sabe que a entidade da esquerda da operação é a entidade pai e a entidade da direita da operação é a entidade filha.

Com isso em mente, vamos demostrar uma forma de criar **grafos circulares** usando apenas CSharp e a matemática.

No código de exemplo, vamos sobrescrever os operadores `+` e `-` e delegar a eles as seguintes ações:

* `+`: Adicionar a entidade da direita como sendo filha da entidade da esquerda.
* `-`: Remover a entidade da direita da lista filhos da entidade da esquerda.

A entidade da esquerda da operação é representada pelo parâmetro `a` e a entidade da direita da operação é representada pelo parâmetro `b`.

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

Note que a classe `CircularEntity` agora pode ser usada em qualquer expressão matemática:

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

_Esse código vai criar o grafo da entidade `A` gerando a seguinte estrutura:_

```
[0] => A
    [1] => B
    [2] => C
         [3] => D
```

_Vamos agora remover a entidade `D` da entidade `C`:_

```csharp
public void GraphCircular()
{
    C = C - D;
}
```

_A estrutura final da entidade `A` será:_

```
[0] => A
    [1] => B
    [2] => C
```

_A expressão de grafos final, após a remoção, ficaria assim:_

```
A + B + C
```

# <a name="impl-factory-entity-complex" />Criando entidades complexas com expressão de grafos e a matemática

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

Na linguagem `C#` temos dois tipos de membros: **Propriedades** e **Campos** e ambas podem ser utilizadas na expressão independente da sua visibilidade.

## <a name="impl-factory-entity-complex-class-entity" />Entendendo a classe `Entity`

Essa classe representa uma entidade do grafo e o local onde ela pode estar. Cada local é representado por um construtor especifico e veremos isso a seguir:

**1)** Esse construtor representa um tipo complexo localizado na raiz da expressão, o parâmetro `complexEntityId` é obrigatório e é usado para atribuir uma identificação para a entidade. Essa identificação é importante pois você pode querer utilizar essa instância em outro local do grafo.

```csharp
Entity(int complexEntityId)
```

**2)** O segundo construtor cria uma entidade no qual o seu local será um membro, ou seja, pode ser uma propriedade ou um campo. O parâmetro `Name` vai definir o nome do membro. O parâmetro `complexEntityId` vai atribuir a esse membro a entidade que corresponde a esse ID.

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

A saída mostra que as entidades são exatamente as mesmas:

```
True
```

**3)** O terceiro construtor deve ser usado quando você precisa atribuir um valor que não seja uma referência, ou seja, qualquer tipo primitivo. Esses valores devem ser passados em forma de texto para serem atribuídos corretamente.

```csharp
Entity(string name, string value)
```

É importante destacar que membros que não são públicos também podem ter valores atribuídos. E no nosso próximo exemplo vamos demostrar como atribuir um valor no campo privado `_intValue`.

Notem também que o valor está em forma de texto e isso é importante porque o construtor que aceita um valor inteiro é exclusivo para atribuir referencias e não valores primitivos.

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

**4)** O último construtor contém apenas o parâmetro `string raw`. O valor desse parâmetro deve estar no formato de **entidades complexas em forma de texto**.

```csharp
Entity(string raw)
```

No exemplo a seguir veremos a criação de uma entidade onde vamos popular o campo privado `_intValue` usando o **formato de entidades complexas em forma de texto**.

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

Essa saída vai produzir o valor abaixo e foi necessário obter o valor via reflexão devido a visibilidade do campo `_intValue`:

```
1000
```

Esse formato é dividido em dois: **Tipos primitivos** e **Tipos complexos** e veremos isso nos próximos tópicos.

**Propriedades da classe:**

* `IEntityFactory Factory`: Propriedade que contém a instância da fábrica de entidades.
* `Entity Parent`: Propriedade que determina a entidade pai na expressão.
* `IReadOnlyCollection<Entity> Children`: Propriedade que contém todos os filhos.
* `Entity this[int index]`: Propriedade que retorna um filho pelo índice.
* `Entity this[string key]`: Propriedade que retorna um filho pelo nome do membro.
* `List<Operation> Operations`: Propriedade que contém todas as operações para que ocorra a re-execução nas fábricas de entidades. Essa propriedade é limpa em todas as outras entidades, exceto a entidade raiz.
* `string Raw`: Propriedade que determina a entidade em forma de texto.
* `Type Type`: Propriedade que determina o tipo da entidade.
* `MemberInfo MemberInfo`: Propriedade que determina o membro.
* `string Name`: Propriedade que determina o nome do membro.
* `object Value`: Propriedade que determina o valor, ou seja, a própria entidade.
* `string ValueRaw`: Propriedade que determina o valor em forma de texto.
* `bool IsPrimitive`: Propriedade que indica se o valor é primitivo ou não.
* `string ComplexEntityId`: Propriedade que indica a identificação da entidade complexa.

## <a name="impl-factory-entity-complex-primitive" />Entidades complexas em forma de texto - Tipos primitivos

Para tipos primitivos temos o seguinte formato:

```
[TypeName].[MemberName]: [Value]
```

Os colchetes indicam que a parte não é obrigatória, no formato acima vemos que a exibição do tipo não é obrigatória, contudo, quando ela estiver presente, devemos adicionar um `.`.

A segunda parte não deve existir para a entidade que está localizada na raiz da expressão, afinal se ela está na raiz ela não tem pai.

A partir da segunda entidade da expressão, a segunda parte é obrigatória e indica o nome do membro que pode ser uma propriedade ou um campo.

A próxima parte é o separador `:` que separa o membro do valor. Se o valor do membro for nulo, então o separador não será exibido.

**Vejamos alguns exemplos:**

_Exibe o tipo e o valor da entidade primitiva que está localizada na raiz. Por ser a entidade raiz então não existirá o nome do membro:_

```
System.String: Value
```

_Exibe o tipo, o membro e valor da entidade primitiva que está localizada na segunda posição em diante:_

```
System.String.StrValue: Value
```

_Exibe o tipo, o membro e um valor vazio da entidade primitiva que está localizada na segunda posição em diante:_

```
System.String.StrValue: 
```

_Exibe o tipo e o membro quando o valor for nulo. Não exibe o separador, esse é o indicativo que o valor é nulo:_

```
System.String.StrValue
```

## <a name="impl-factory-entity-complex-complex" />Entidades complexas em forma de texto - Tipos complexos

Para tipos complexos temos o seguinte formato:

```
[TypeName].[MemberName].EntityID
```

Os colchetes indicam que a parte não é obrigatória, no formato acima vemos que a exibição do tipo não é obrigatória, contudo, quando ela estiver presente, devemos adicionar um `.`.

A segunda parte não deve existir para a entidade que está localizada na raiz da expressão, afinal se ela está na raiz ela não tem pai.

A partir da segunda entidade da expressão, a segunda parte é obrigatória e indica o nome do membro que pode ser uma propriedade ou um campo.

A próxima parte é a identificação da entidade no grafo. Essa identificação deve ser um número inteiro e é ela que garante a possibilidade de usar referências da mesma entidade em outros pontos do grafo.

**Vejamos alguns exemplos:**

_Exibe o tipo e a identificação da entidade complexa que está localizada na raiz. Usaremos o ID igual a `0`. Por ser a entidade raiz então não existirá o nome do membro:_

```
Namespace.MyClass.0
```

_Exibe o tipo, o membro e a identificação da entidade complexa que está localizada na segunda posição em diante. Usaremos o ID igual a `1`:_

```
Namespace.MyClass.MyProperty.1
```

_Exibe o tipo e o membro quando o valor for nulo. Não exibe nenhuma identificação, pois não existe entidade. Esse é o indicativo que o valor é nulo:_

```
Namespace.MyClass.MyProperty
```

## <a name="impl-factory-entity-complex-collections" />Entidades complexas em forma de texto - Coleções e arrays

Para criar itens em uma coleção ou array é necessário que o nome do membro indique a posição do item dentro de colchetes: `[{position}]: Value`

No exemplo abaixo veremos como criar um array de inteiro usando expressão de grafos. Note que no lugar do nome do membro, usamos os colchetes como indicativo de um item de coleção.

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

A saída será:

```
10
11
```

## <a name="impl-factory-entity-complex-class-complex-factory" />Entendendo a classe `ComplexEntityFactory`

Essa classe é a responsável por criar o grafo da entidade complexa com base na expressão. Internamente ela re-executa a expressão e gera cada entidade do grafo.

Essa classe contém o seguinte construtor:

```csharp
ComplexEntityFactory(Type type, Entity root = null)
```

* `type`: Esse parâmetro determina o tipo da entidade raiz.
* `root`: Esse parâmetro determina qual é a entidade raiz. Ele não é obrigatório, pois essa classe também é usada na desserialização e lá a entidade raiz é obtida tardiamente.

Temos algumas propriedades que ajudarão na criação e customização das entidades:

* `IReadOnlyList<Entity> Entities`: Propriedade que armazena todas as entidades do grafo e sem repeti-las.
* `bool IsTyped`: Propriedade que indica se a criação tem um tipo definido.
* `IReadOnlyDictionary<Type, Type> MapTypes`: Propriedade que contém os mapas de criação para interfaces ou classes abstratas, ou até mesmo para classes concretas caso seja necessário trocar um tipo por outro.
* `IReadOnlyList<string> Errors`: Propriedade que armazena os erros.
* `bool IgnoreErrors`: Propriedade que indica se os erros serão ignorados. Do contrário será enviado uma exceção.
* `List<ITypeDiscovery> TypeDiscovery`: Propriedade que contém uma lista de classes de descoberta de tipos.
* `List<IValueLoader> ValueLoader`: Propriedade que contém uma lista de classes para carregar valores.
* `List<IMemberInfoDiscovery> MemberInfoDiscovery`: Propriedade que contém uma lista de classes para descoberta de membros.
* `List<ISetChild> SetChildAction`: Propriedade que contém uma lista de classes que fazem as atribuições entidades filhas nas entidades pais.
* `Entity Root`: Propriedade que indica a entidade raiz.
* `Type RootType`: Propriedade que indica o tipo da entidade raiz.
* `object Value`: Valor da entidade raiz.

Por fim, temos alguns métodos que são usados durante a criação:

* `ComplexEntityFactory Build()`: Esse método é responsável por gerar o grafo. Ele retorna a propria classe para manter a fluência.
* `void AddMapType<TFrom, TTo>()`: Esse método deve ser usado antes do método `Build` e ele determina o mapeamento dos tipos.
* `void AddError(string err)`

Existe também uma variação dessa classe que possibilita trabalhar de forma genérica e isso facilita o uso da propriedade `Value` que já estará tipada.

```csharp
ComplexEntityFactory<T>()
```

No próximo exemplo, veremos a criação de uma entidade que não contém propriedades com tipos concretos, sendo necessário a criação de um mapa de tipos:

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

A saída mostra que as propriedades `A` e `B` foram criadas com o tipo `ImplementAbstractAndInterface` e as propriedades `A.MyProp` e `B.MyProp` também foram populadas:

```
10
ImplementAbstractAndInterface
20
ImplementAbstractAndInterface
```

## <a name="impl-factory-entity-complex-discovery-types" />Descobridores de tipos

Os descobridores de tipos tem como principal objetivo descobrir o tipo da entidade. A propriedade `TypeDiscovery` será usada para encontrar o melhor descobridor para cada entidade.

Os descobridores de tipos devem herdar da interface `ITypeDiscovery` e o método `CanDiscovery` é o responsável por determinar se a entidade pode ou não ser descoberta. Quando o método `CanDiscovery` retornar `true` então o método `GetEntityType` será chamado e é nesse momento que o tipo da entidade será retornado.

```csharp
public interface ITypeDiscovery
{
    bool CanDiscovery(Entity item);
    Type GetEntityType(Entity item);
}
```

A ordem dos descobridores é de extrema importância, isso porque o último será usado em caso de desempate, ou seja, se três retornarem `true`, o último será usado.

Por padrão, temos alguns descobridores de tipos definidos e todos eles já estão ordenados na propriedade `TypeDiscovery` para que não aja erros.

1. `DictionaryItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item no dicionário. O método `CanDiscovery` verifica se o tipo pai é um dicionário, se for, então o método `GetEntityType` será chamado retornando o tipo `KeyValuePair<,>`.
2. `MemberInfoTypeDiscovery`: Essa classe é responsável por descobrir o tipo do membro. O método `CanDiscovery` verifica se a entidade tem um nome de membro definido, se tiver, então o método `GetEntityType` será chamado para obter o tipo do membro.
3. `ListItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item em uma lista. O método `CanDiscovery` verifica se o tipo pai é um `IList`, se for, então o método `GetEntityType` será chamado para obter o tipo da lista.
4. `ArrayItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item no array. O método `CanDiscovery` verifica se o tipo pai é um array, se for, então o método `GetEntityType` será chamado para obter o tipo do array.

## <a name="impl-factory-entity-complex-discovery-members" />Descobridores de membros

Os descobridores de membros tem o objetivo de descobrir o membro da entidade. A propriedade `MemberInfoDiscovery` será usada para encontrar o melhor descobridor de membros de cada entidade.

Os descobridores de membros devem herdar da interface `IMemberInfoDiscovery` e o método `CanDiscovery` é o responsável por determinar se a entidade pode ou não ter seu membro descoberto. Quando o método `CanDiscovery` retornar `true` então o método `GetMemberInfo` será chamado para retornar o `MemberInfo`.

```csharp
public interface IMemberInfoDiscovery
{
    bool CanDiscovery(Entity item);
    MemberInfo GetMemberInfo(Entity item);
}
```

Por padrão, temos apenas um descobridor de membro definido na propriedade `MemberInfoDiscovery`.

```csharp
class MemberInfoDiscovery : IMemberInfoDiscovery
```

Caso queria substitui-lo, basta adicionar um novo descobridor de membro na propriedade `MemberInfoDiscovery`.

Apenas garanta que o método `CanDiscovery` tenha a seguinte implementação:

```csharp
public bool CanDiscovery(Entity item)
{
    return item.Factory.IsTyped
            && item.Name != null
            && !item.Name.StartsWith(Constants.INDEXER_START) // ignore [0] members
            && item.Parent.Type != null;
}
```

Esse código garante que:

1. `item.Factory.IsTyped`: Exista um tipo definido para a entidade raiz. É a partir dela que encontramos todos os tipos do grafo.
2. `item.Name != null`: Exista um nome para o membro
3. `!item.Name.StartsWith(Constants.INDEXER_START)`: O nome do membro não pode ser uma representação de posição de coleções, ou seja, não pode iniciar com `[`.
4. `item.Parent.Type != null`: Exista um tipo para a entidade pai, é com esse tipo mais o nome do membro que obtemos o tipo do membro.

## <a name="impl-factory-entity-complex-value-loaders" />Carregadores de valores

Os carregadores de valores tem o objetivo de criar as entidades primitivas e complexas. A propriedade `ValueLoader` será usada para encontrar o melhor carregador de valor para cada entidade. Entenda o termo "valor" como sendo a entidade que será criada.

Os carregadores de valores devem herdar da interface `IValueLoader` e o método `CanLoad` é o responsável por determinar se o tipo da entidade pode ou não ser carregado. Quando o método `CanLoad` retornar `true` então o método `GetValue` será chamado para obter o valor que será a entidade.

```csharp
public interface IValueLoader
{
    bool CanLoad(Entity item);
    object GetValue(Entity item);
}
```

A ordem dos carregadores é de extrema importância, isso porque o último será usado em caso de desempate, ou seja, se três retornarem `true`, o último será usado.

Por padrão, temos alguns carregadores de valores definidos e todos eles já estão ordenados na propriedade `ValueLoader` para que não aja erros.

1. `PrimitiveValueLoader`: Esse carregador inicializa os tipos primitivos.
2. `ComplexEntityValueLoader`: Esse inicializador carrega os tipos complexos. Caso o tipo tenha um construtor sem parâmetros então esse construtor será usado, do contrário a instância será criada sem chamar o construtor, ou seja, usando o método `FormatterServices.GetUninitializedObject(typeof(T))`. Esse carregador só é utilizado quando a propriedade `ComplexEntityId` da classe `Entity` estiver preenchida, ou seja, se a entidade tiver identificação é porque ela é complexa.
3. `ArrayValueLoader`: Esse carregador é utilizado quando a entidade é um array independente da quantidade de dimensões.
4. `ExpandoObjectValueLoader`: Esse carregador é usado para tipos anônimos ou do tipo `ExpandoObject`.

**Importante:**

O tipo `ExpandoObject` será usado em todos os níveis quando a classe `ComplexEntityFactory` não tiver um tipo definido.

## <a name="impl-factory-entity-complex-child-assign" />Atribuidores de filhos

Os atribuidores de filhos tem o objetivo de adicionar uma entidade filha em sua entidade pai, ou seja, atribuir um valor em um membro da instância pai, ou um item em uma lista por exemplo. A propriedade `SetChildAction` será usada para encontrar o melhor atribuidor para cada entidade.

Os atribuidores de filhos devem herdar da interface `ISetChild` e o método `CanSet` é o responsável por determinar se o item filho por ou não ser atribuído ao item pai. Quando o método `CanSet` retornar `true` então o método `SetChild` será chamado para fazer a atribuição.

```csharp
public interface ISetChild 
{
    bool CanSet(Entity item, Entity child);
    void SetChild(Entity item, Entity child);
}
```

A ordem dos atribuidores é de extrema importância, isso porque o último será usado em caso de desempate, ou seja, se três retornarem `true`, o último será usado.

Por padrão, temos alguns atribuidores definidos e todos eles já estão ordenados na propriedade `SetChildAction` para que não aja erros.

1. `MemberInfoSetChild`: Esse atribuidor será utilizado quando a entidade filha tiver um membro definido.
2. `DictionarySetChild`: Esse atribuidor será utilizado quando a entidade pai for um dicionário e a entidade filha tiver o nome do membro iniciado pelo caractere `[`. Isso significa que a entidade filha é um item e não uma propriedade da entidade pai.
3. `ExpandoObjectSetChild`: Esse atribuidor é utilizado quando a entidade pai for do tipo `ExpandoObject`.
4. `ArraySetChild`: Esse atribuidor é utilizado quando a entidade pai for do tipo `Array` e a entidade filha tiver o nome do membro iniciado pelo caractere `[`.
5. `ListSetChild`: Esse atribuidor é utilizado quando a entidade pai for do tipo `IList` e a entidade filha tiver o nome do membro iniciado pelo caractere `[`.

# <a name="impl-serialization" />Serialização

A serialização é o processo de transformação de um grafo para expressão de grafos em forma de texto. Dividimos a serialização em dois tipos: **serialização de entidades circulares** e **serialização de entidades complexas**. Isso é interessante, pois entidades circulares são mais simples e precisam apenas de um nome para representa-las, ao contrário de entidades complexas que podem conter diversas propriedades para diversos fins.

## <a name="impl-serialization-complex" />Serialização Complexa

A serialização de entidades complexas é feita pela classe `ComplexEntityExpressionSerializer`. Essa classe deve respeitar as regras que vimos nos tópicos [Entidades complexas em forma de texto - Tipos primitivos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-primitive), [Entidades complexas em forma de texto - Tipos complexos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-complex) e [Entidades complexas em forma de texto - Coleções e arrays](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-collections).

Essa classe herda da classe abstrata `ExpressionSerializerBase<object>` que tem como responsabilidade compor a base matemática de uma expressão de grafos seja ela circular ou complexa. Essa composição é feita pelo método `Serialize()`. Ele é o responsável por criar os parenteses, adicionar os caracteres de soma e etc.

A classe `ComplexEntityExpressionSerializer` ao implementar essa classe base deve obrigatoriamente sobrescrever o método `SerializeItem` que será o responsável por serializar cada item da expressão.

O construtor obriga que seja passado a expressão a ser serializada.

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
* `bool ForceQuoteEvenWhenValidIdentified`: Tem a mesma função das expressões circulares.
* `IValueFormatter ValueFormatter`: Tem a mesma função das expressões circulares.
* `GetEntityIdCallback`: Propriedade que retorna a identificação de uma entidade, por padrão, usamos o método `GetHashCode()` do próprio `C#`.
* `ItemsSerialize`: Propriedade que contém uma lista da interface `IEntitySerialize`. Essa propriedade é a principal alternativa para customizar a serialização e veremos isso nos próximos tópicos.
* `ShowType`: Determina como será a exibição do tipo em cada item da expressão
  * `None`: Não exibe o tipo para nenhum item.
  * `TypeNameOnlyInRoot`: Exibe o nome do tipo (na forma curta) apenas para o item raiz.
  * `TypeName`: Exibe o nome do tipo (na forma curta) para todos os itens da expressão.
  * `FullTypeName`: Exibe o nome completo do tipo para todos os itens da expressão.

No exemplo a seguir vamos forçar o uso de parenteses no item raiz, forçar a exibir o tipo em todos os itens da expressão e também trucar valores que passem de 3 caracteres:

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

### <a name="impl-serialization-complex-itens-serialize" />Customizando a serialização dos itens

Os itens de serialização são os responsáveis pela serialização do nome do membro e obtenção do tipo do item na expressão. O tipo retornado será usado pela classe `ValueFormatter` quando for primitivo. Para tipos complexos será mantida a exibição da identificação como vimos no tópico [Entidades complexas em forma de texto - Tipos complexos](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex-complex).

A propriedade `ItemsSerialize` será usada para encontrar o melhor serializador para item da expressão.

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
2. `PropertySerialize`: Esse classe é usada para itens que derivam de propriedades. Será usado o tipo da propriedade e o seu nome como retorno.
3. `FieldSerialize`: Esse classe é usada para itens que derivam de campos. Será usado o tipo do campo e o seu nome como retorno.
4. `ArrayItemSerialize`: Esse item classe é usada para itens que derivam de arrays. Ela retorna na propriedade `ContainerName` a posição do item no array no formato: `[{position1},{position2}]`
5. `DynamicItemSerialize`: Esse classe é usada para itens que derivam de classes dinâmicas.
6. `CollectionItemSerialize`: Esse item classe é usada para itens que derivam de coleções. Ela retorna na propriedade `ContainerName` a posição do item na coleção no formato: `[{position1},{position2}]`

No exemplo a seguir veremos a criação de um novo leitor de membro chamado `MethodReader` que será responsável por ler o método `HelloWorld` e criar um novo tipo de entidade na expressão chamado `MethodEntity`. Com base nesse novo tipo de entidade, vamos criar um serializador chamado `MethodSerialize` que terá a função de serializar os itens dos tipos `MethodEntity` para o formato: `MethodName(parameters)`:

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

## <a name="impl-serialization-circular" />Serialização circular

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

A saída será uma expressão circular cujo o nome de cada item foi a propriedade `Name` da classe `CircularEntity`:

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

No exemplo a seguir vamos forçar o uso de parenteses no item raiz, forçar o uso de aspas para nomes válidos e também trucar nomes que passem de 3 caracteres:

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

# <a name="impl-deserialization" />Desserialização

A desserialização é o processo de transformação de expressão de grafos em forma de texto para uma entidade especificada. Dividimos a desserialização em dois tipos: **desserialização de entidades circulares** e **desserialização de entidades complexas**.

O processo de desserialização utiliza como base o compilador `Roslyn` da linguagem `C#`. Isso tem prós e contras.

A parte boa é que não precisamos reimplementar a leitura de expressões matemáticas, pois com o Roslyn é possível converter a `string`, que é uma expressão matemática, em uma `SystaxTree` e com isso fazer a compilação para uma expressão matemática.

A classe `RoslynExpressionDeserializer<T>` é a responsável por fazer a conversão da `string` para uma expressão matemática.

O tipo inferido `T` deve obrigatoriamente conter uma sobrecarga do operador `+`.

A parte ruim dessa abordagem é que existe uma lentidão nesse processo em sua primeira execução. Por hora, não temos solução para esse problema, mas estamos acompanhando a evolução do compilador Roslyn.

## <a name="impl-deserialization-complex" />Desserialização complexa

A desserialização de entidades complexas é feita pela classe `ComplexEntityExpressionDeserializer`. O método `Deserialize` é o responsável pela desserialização. Existem algumas variações desse método e cada uma tem sua utilidade:

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

**2)** O segundo método tem o mesmo objetivo do primeiro, a única diferença é que o tipo não será inferido no método e sim no parâmetro `type`:

```csharp
public object Deserialize(string expression, Type type = null);
public async Task<object> DeserializeAsync(string expression, Type type = null);
```

**3)** O terceiro método recebe o parâmetro `factory`, esse parâmetro deve ser usado se for necessário alguma customização na criação das entidades complexas. Em resumo, esse processo é exatamente igual ao tópico [Criando entidades complexas com expressão de grafos e a matemática](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#impl-factory-entity-complex). Internamente, o compilador transformará cada item da expressão na classe `Entity` e depois seguirá os mesmos passos que já vimos nesse tópico:

```csharp
public T Deserialize<T>(string expression, ComplexEntityFactory factory);
public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactory factory);
```

**4)** Essa última sobrecarga tem a mesma função da sobrecarga acima, a única diferença é que não temos um tipo inferido, ou seja, o tipo estará definido na classe `ComplexEntityFactory` ou se não tiver, o resultado será uma classe do tipo `ExpandObject`:

```csharp
public object Deserialize(string expression, ComplexEntityFactory factory);
public async Task<object> DeserializeAsync(string expression, ComplexEntityFactory factory);
```

## <a name="impl-deserialization-circular" />Desserialização circular

A desserialização de entidades circulares é feita pela classe `CircularEntityExpressionDeserializer<T>`. O método `Deserialize` é o responsável pela desserialização. Existem algumas variações desse método e cada uma tem sua utilidade:

O tipo inferido `T` deve obrigatoriamente conter uma sobrecarga do operador `+`, pois é nesse operador que ficará a sua lógica de soma ou subtração.

**1)** O primeiro método necessita apenas da expressão em forma de texto. Com base nessa expressão e no tipo inferido na classe `CircularEntityExpressionDeserializer` é possível fazer a desserialização. Existem duas variantes desse método, uma síncrona e outra assíncrona.

O tipo inferido deve ter obrigatoriamente um parâmetro em seu construtor do tipo `string`. Esse parâmetro será o nome da entidade.

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

**2)** A segunda sobrecarga desse método necessita da expressão e de um método que será a fábrica da entidade circular, ou seja, com essa sobrecarga a sua classe circular não precisa necessariamente ter um construtor com um parâmetro do tipo `string`, pois quem fará a criação da classe será esse método. Existem duas variantes desse método, uma síncrona e outra assíncrona.

```csharp
public T Deserialize(string expression, Func<string, T> createEntityCallback);
public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback);
```

**3)** A terceira sobrecarga desse método necessita da expressão e de uma instância da classe `CircularEntityFactory<T>`. Essa classe pode ser utilizada para potencializar uma expressão em forma de `string`. Isso quer dizer que você pode criar funções nessa classe e utiliza-la na expressão em forma de texto. Existem duas variantes desse método, uma síncrona e outra assíncrona.

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

# <a name="impl-graph-info" />Informações do grafo de uma entidade

As classes `Expression<T>` e `EntityItem<T>` trazem algumas informações da teoria de grafos que ajudam a compreender um pouco a relação entre as entidades.

A classe `Expression<T>` trás a propriedade `Graph` que isola as informações gerais do grafo, ela contém as seguintes propriedades e definições:

* `IReadOnlyList<Edge<T>> Edges`: Essa propriedade contém todas as arestas do grafo.
  * `class Edge<T>`: Essa classe representa uma conexão entre duas entidades (A e B), nela temos alguns propriedades e um método que ajudam a extrair algumas informações da ligação.
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
    * `string Identity`: Essa é a identificação do caminho, essa identificação utiliza o `Id` de cada vértice e útiliza-se do seguinte padrão:
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

# <a name="donate" />Doações

GraphExpression é um projeto de código aberto. Iniciado em 2017, muitas horas foram investidos na criação e evolução deste projeto.

Se o GraphExpression foi útil pra você, ou se você deseja ve-lo evoluir cada vez mais, considere fazer uma pequena doação (qualquer valor). Ajude-nos também com sugestões e possíveis problemas.

De qualquer forma, agradecemos você por ter chego até aqui ;)

**BitCoin:**

_19DmxWBNcaUGjm2PQAuMBD4Y8ZbrGyMLzK_

![bitcoinkey](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/bitcoinkey.png)

# <a name="license" />Licença

The MIT License (MIT)

Copyright (c) 2018 Glauber Donizeti Gasparotto Junior

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.