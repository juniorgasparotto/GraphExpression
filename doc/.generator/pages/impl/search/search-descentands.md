### Descendentes <header-set anchor-name="impl-search-descentands" />

A pesquisa de descendentes é útil para encontrar os filhos ou todos os descendentes de um item. Temos algumas sobrecargas que serão explicadas a seguir:

**1)** Essa é a sobrecarga padrão, caso nenhum parâmetro seja passado então nenhum filtro será aplicado e todos os descendentes serão retornados.

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

**2)** A segunda sobrecarga tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
```

**3)** A terceira sobrecarga filtra apenas pela profundidade de inicio e fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
```

**4)** A quarta sobrecarga filtra apenas pela profundidade de fim.

```csharp
IEnumerable<EntityItem<T>> Descendants(int depthEnd)
```

**5)** Esse método tem a mesma utilidade da sobrecarga padrão, contudo ele é um simplificador para recuperar todos os descendentes até que algum descendente retorne negativo no parâmetro `stop`. Do contrário será retornado todos os itens até chegar no último item. Ele utiliza o delegate `EntityItemFilterDelegate2`, ou seja, temos a informação da profundidade do item para usar na pesquisa.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
```

**6)** A segunda sobrecarga do método `DescendantsUntil` tem os mesmos filtros, contudo, utiliza o delegate `EntityItemFilterDelegate` que tem apenas o parâmetro `item` deixando mais rápido a escrita.

```csharp
IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
```