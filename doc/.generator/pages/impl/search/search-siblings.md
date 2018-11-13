### `Siblings` <header-set anchor-name="impl-search-siblings" />

Essa pesquisa encontra os irmãos de um determinado item. Temos algumas sobrecargas que serão explicadas a seguir:

**1)** Essa é a sobrecarga padrão, caso nenhum parâmetro seja passado então nenhum filtro será aplicado e todos os descendentes serão retornados.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

* `filter`: Não retorna itens quando o filtro retornar negativo, mas continua a busca até chegar no último irmão ou no primeiro (depende do parâmetro `direction`). A pesquisa utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.
* `stop`: Determina quando a navegação deve parar, do contrário a navegação deverá ir até chegar no último irmão ou no primeiro (depende do parâmetro `direction`).
* `direction`: Esse parâmetro determina em qual direção a navegação deverá ir:
    * `Start`: Determina que a navegação deve começar no primeiro irmão posicionado à esquerda do item de referência e ir até o último irmão à direita.
    * `Next`: Determina que a navegação deve começar no próximo irmão posicionado à direita do item de referência e ir até o último irmão à direita.
    * `Previous`: Determina que a navegação deve começar no próximo irmão posicionado à esquerda do item de referência e ir até o primeiro irmão à esquerda.
* `positionStart`: Determina a posição de início que a pesquisa deve começar. 
    * Quando a direção for igual a `Start`, a posição `1` será do primeiro irmão à esquerda do item referência.
    * Quando a direção for igual a `Next`, a posição `1` será do próximo irmão à direita do item referência.
    * Quando a direção for igual a `Previous`, a posição `1` será do próximo irmão à esquerda do item referência.
* `positionEnd`: Determina a posição de fim que a pesquisa deve parar.

Nesse exemplo vamos retornar os irmãos do item cujo o valor é igual a `C` em todas as direções.

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

_A primeira saída retorna todos os irmãos da entidade `C` iniciando do primeiro irmão à esquerda até o último irmão à direita. É importante destacar que o próprio item não é retornado, afinal ele não é irmão de si mesmo._

```
-> Start direction
A: A
B: B
D: D
E: E
```

_A segunda saída retorna todos os irmãos da entidade `C` iniciando do próximo irmão à direita até o último irmão à direita._

```
-> Next direction
D: D
E: E
```

_A terceira saída retorna todos os irmãos da entidade `C` iniciando do irmão anterior até o primeiro irmão à esquerda._

```
-> Previous direction
B: B
A: A
```

**2)** A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
```

**3)** A terceira sobrecarga filtra apenas pela profundidade de início e fim na direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

**4)** A quarta sobrecarga filtra apenas pela profundidade final da direção especificada.

```csharp
IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Start)
```

**5)** Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os irmãos até que algum irmão retorne negativo no parâmetro "stop". Do contrário será retornado todos os irmãos até chegar no último ou no primeiro (depende do parâmetro `direction`). Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```

**6)** A segunda sobrecarga do método `SiblingsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
```