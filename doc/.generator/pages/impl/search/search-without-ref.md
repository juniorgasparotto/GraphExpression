## Pesquisa sem referencia <header-set anchor-name="impl-search-without-ref" />

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