### Filhos <header-set anchor-name="impl-search-children" />

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