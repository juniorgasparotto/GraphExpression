# <anchor-set name="implementation">Expressão de grafos</anchor-set>

Esse framework tem como objetivo implementar o conceito de expressão de grafos na linguagem .NET.

Resumidamente, o conceito de **expressão de grafos** tem como objetivo explorar os benefícios de uma expressão matemática trocando os números por entidades. Com isso, podemos criar uma nova maneira de transportar dados e principalmente criar um novo meio de pesquisa transversal em grafos complexos ou circulares.

Com relação a pesquisa em grafos, esse projeto se inspirou na implementação do `JQuery` para pesquisas de elementos HTML (DOM), unindo assim o conceito de expressão de grafos com a facilidade de uso do `JQuery` para pesquisas transversais.

**Atenção:** Esse documento não vai explicar o conceito de expressão de grafos, ele terá como foco apenas o framework `GraphExpression`.

<anchor-get name="concept">Clique aqui</anchor-get> se você quiser conhecer mais sobre o conceito de expressão de grafos.

## Grafos complexos

Chamamos de grafos complexos aqueles que não contém tipo definido, ou seja, todos os itens são definidos como `object`. Esse tipo de grafo é presentado pela classe:

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

* O método de extensão `AsExpression` é o responsável pela criação da expressão. Esse método vai navegar por todos os nós do objeto partindo da raiz até o último descendente e produzirá um resultado semelhante a isso:
* O método de extensão `AsExpression` está disponível em todos os objetos .NET, basta referenciar o namespace `using GraphExpression`.
* A propriedade `Level` é a responsável por informar em qual nível do grafo está cada item da iteração, possibilitando criar uma saída identada que representa a hierarquia do objeto `model`.
* O método `GetEntity` é apenas um ajudante que imprime o tipo do item e o nome do membro que pode ser uma propriedade ou um campo. Poderíamos também retornar o valor do membro, mas para deixar mais limpo a saída, eliminamos essa informação.

2. Na segunda saída podemos ver como ficou a representação desse objeto em expressão de grafos:

```
"Class1.32854180" + "Class1_Prop1: Value1" + ("Class1_Prop2.36849274" + "Class2_Prop2: Value2" + "Class2_Field1: 1000")
```

<anchor-get name="serialization-complex">Clique aqui</anchor-get> para entender como funciona a serialiação de objetos complexos.

### Elementos padrão de uma expressão de grafos para tipos complexos

Os elementos de uma expressão complexa (`Expression<object>`) podem variar entre os seguintes tipos:

* `ComplexEntity`: Esse tipo é a base de todos os outros tipos de uma expressão complexa. É também o tipo da entidade raiz, ou seja, da primeira entidade da expressão.
* `PropertyEntity`: Determina que o item é uma propriedade.
* `FieldEntity`: Determina que o item é um campo.
* `ArrayItemEntity`: Determina que o item é um item de um `array`, ou seja, a classe pai será do tipo `Array`.
* `CollectionItemEntity`: Determina que o item é um item de uma coleção, ou seja, a classe pai será do tipo `ICollection`.
* `DynamicItemEntity`: Determina que o item é uma propriedade dinâmica, ou seja, a classe pai será do tipo `dynamic`.

Todos esses tipos herdam de `EntityItem<object>`, portanto, além de suas propriedades especificas ainda terão as informações do item na expressão.

Ainda é possível extender a criação de uma expressões complexas, para sabe mais veja o tópico <anchor-get name="entity-complex-factory" />

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

# Pesquisas

Existem dois tipos de pesquisas no conceito de expressão de grafos: **Pesquisa sem referencia** e **pesquisa com referencia** e que serão abordadas nesse tópico.

**Atenção:** Nesse tópico, usaremos o modelo de grafos complexos devido a sua maior complexidade.

<anchor-get name="search">Clique aqui</anchor-get> para saber mais.

## Pesquisa sem referencia

A pesquisa sem referencia será feita em uma coleção de entidades, ou seja, cada item da coleção será testado e retornado em caso de sucesso. Por repetir a mesma pesquisa em todos os itens da lista, esse tipo de pesquisa pode trazer duplicidades.

<anchor-get name="search-without-references">Clique aqui</anchor-get> para saber mais sobre esse tipo de pesquisa.

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

Como esperado, o resultado retornou duas linhas:

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

## Pesquisa com referência

A pesquisa com referencia será feita usando um item especifico, ou seja, primeiro você precisa localizar o item desejado e a partir dele será feito a pesquisa desejada.

<anchor-get name="search-with-references">Clique aqui</anchor-get> para saber mais sobre esse tipo de pesquisa.

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

# Tipos de pesquisas

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

## Delegates das pesquisa:

Todos os métodos de pesquisa utilizam os delegates abaixo e que podem ser utilizados usando a classe `Func`

```csharp
public delegate bool EntityItemFilterDelegate<T>(EntityItem<T> item);
public delegate bool EntityItemFilterDelegate2<T>(EntityItem<T> item, int depth);
```

* `EntityItem<T> item`: Esse parâmetro significa o item corrente durante a pesquisa.
* `int depth`: Determina a profundidade do item corrente com relação a sua posição.

## Antepassados

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

1.1. A primeira saída exibe todos os pais do item referencia.

```
Property.Class1_Prop2
Class1
```

* A ordem de retorno será sempre do antepassado mais próximo, ou seja, o primeiro item da lista de retorno será sempre o pai do item referência.

1.2. A segunda saída exibe apenas o antepassado cujo a profundidade é igual a `1`, ou seja, nesse caso seria o item pai do item referencia:

```
Property.Class1_Prop2
```

2. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

3. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthStart, int depthEnd)
```

4. A quarta sobrecarga filtra profundidade de fim.

```csharp
IEnumerable<EntityItem<T>> Ancestors(int depthEnd)
```

5. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os antepassados até que algum antepassado retorne negativo no parâmetro `stop`. Do contrário será retornado todos os itens até a raiz. Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

6. A segunda sobrecarga do método `AncestorsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

## Descendentes

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

A saída será:

```
Property.Class2_Prop2
Field.Class2_Field1
```

2. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

3. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
```

4. A quarta sobrecarga filtra profundidade de fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthEnd)
```

5. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os descendentes até que algum descendente retorne negativo no parâmetro `stop`. Do contrário será retornado todos os itens até chegar no último item. Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

6. A segunda sobrecarga do método `DescendantsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```

## Filhos

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

A saída exibirá as duas propriedades que são filhas do item raiz:

```
Property.Class1_Prop1
Property.Class1_Prop2
```

* Esse método não tem parâmetros, basta utilizar as funções do `Linq` caso necessite de alguma filtragem.
* Esse método é um alias do método `Descendants(int depthStart, int depthEnd)`, no qual será passado os valores fixos `Descendants(1, 1)`.

## Irmãos

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

1.1. A primeira saída será retorna todos os irmãos da entidade `C` iniciando do primeiro irmão á esquerda até o último irmão á direita. É importante destacar que o próprio item não é retornado, afinal ele não é irmão dele mesmo.

```
-> Start direction
A: A
B: B
D: D
E: E
```

1.1. A segunda saída retorna todos os irmãos da entidade `C` iniciando do próximo irmão á direita até o último irmão á direita.

```
-> Next direction
D: D
E: E
```

1.2. A terceira saída retorna todos os irmãos da entidade `C` iniciando do irmão anterior até o primeiro irmão á esquerda.

```
-> Previous direction
B: B
A: A
```

2. A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

3. A terceira sobrecarga filtra apenas pela profundidade de inicio e fim na direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

4. A quarta sobrecarga filtra profundidade de fim na direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

5. Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os irmãos até que algum irmão retorne negativo no parâmetro `stop`. Do contrário será retornado todos os irmãos até chegar no último ou no primeiro (depende do parâmetro `direction`). Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

6. A segunda sobrecarga do método `SiblingsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

# Representação em forma de expressão

A segunda saída desse exemplo mostra como fica o objeto `model` em forma de expressão:

```
"Class1.32854180" + "Class1_Prop1: Value1" + ("Class1_Prop2.27252167" + "Class2_Prop2: ValueChild" + "Class2_Prop1: 2147483647")
```

Com essa representação podemos usar o conceito e abstrair as informações de cada item do grafo e com isso fazer o que quisermos

<anchor-get name="intro">Clique aqui</anchor-get> para saber mais sobre essas informações.

# Estrutura transversal

Esse tipo de estrutura é a base do conceito de expressão de grafos, ou seja, remover as dificuldades das estruturas hierarquicas empilhando todas as entidades do grafo uma em baixo da outra. É claro que é possível identificar a posição da entidade no grafo hirarquico, o conceito de expressão de grafos trás um conjunto de informações que possibilita essa descoberta.


<anchor-get name="entity-info">Clique aqui</anchor-get> para saber mais sobre essas informações.

## Serializando para expressão de grafos

```csharp
public void Example1()
{
    System.Console.WriteLine(expression.DefaultSerializer.Serialize());
}
```

A segunda saída mostra o objeto serializado para expressão de grafos. Notem que os parenteses correspondem a criação de um novo nível seguindo o conceito de expressão de grafos para objetos complexos.

```
"Class1.32854180" + "Class1_Prop1: Value1" + ("Class1_Prop2.27252167" + "Class2_Prop2: ValueChild" + "Class2_Prop1: 2147483647")
```

Essa classe contém uma lista da classe `EntityItem<object>` na qual contém todas as informações da ocorrência da entidade na expressão.

* 

# <anchor-set name="index">Índice</anchor-set>

<table-of-contents />