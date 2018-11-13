# Criando entidades complexas com expressão de grafo e a matemática <header-set anchor-name="impl-factory-entity-complex" />

Criar entidades complexas com expressão de grafo não é uma tarefa simples como vimos nos grafos circulares. É necessário uma estrutura de classes robusta e o uso de reflexão para compor as entidades.

Vejamos um exemplo de como criar uma classe complexa do tipo `CircularEntity` usando apenas expressão de grafo. A ideia será atribuir um valor na propriedade `Name`.

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

Note que estamos usando expressões matemáticas e montamos a entidade do tipo `CircularEntity` sem usar o comando `new`.

A ordem da expressão é a mesma que vimos nas entidades circulares, ou seja, o item da esquerda da expressão é o item pai do item da direita da expressão. Por ser um tipo complexo, o item da esquerda é a instância e o item da direita é o membro. 

Na linguagem `C#` temos dois tipos de membros: **Propriedades** e **Campos** e ambas podem ser utilizadas na expressão independente da sua visibilidade.

## Entendendo a classe `Entity` <header-set anchor-name="impl-factory-entity-complex-class-entity" />

Essa classe representa uma entidade do grafo e o local onde ela pode estar. Cada local é representado por um construtor especifico e veremos isso a seguir:

**1)** Esse construtor representa um tipo complexo localizado na raiz da expressão, o parâmetro `complexEntityId` é obrigatório e é usado para atribuir uma identificação para a entidade. Essa identificação é importante pois você pode querer utilizar essa instância em outro local do grafo.

```csharp
Entity(int complexEntityId)
```

**2)** O segundo construtor cria uma entidade localizado em um membro, ou seja, pode ser uma propriedade ou um campo. O parâmetro `Name` vai definir o nome do membro. O parâmetro `complexEntityId` vai atribuir a esse membro a entidade que corresponde a esse ID. 

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

Notem também que o valor está em forma de texto e isso é importante porque o construtor que aceita um valor inteiro é exclusivo para atribuir referências e não valores primitivos.

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

**4)** O último construtor contém apenas o parâmetro: `string raw`. O valor desse parâmetro deve estar no formato de **entidades complexas em forma de texto**.

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
* `List<Operation> Operations`: Propriedade que contém todas as operações para que ocorra a re-execução nas fábricas de entidades. Essa propriedade é limpa em todas as outras entidades, exceto na entidade raiz.
* `string Raw`: Propriedade que determina a entidade em forma de texto.
* `Type Type`: Propriedade que determina o tipo da entidade.
* `MemberInfo MemberInfo`: Propriedade que determina o membro. 
* `string Name`: Propriedade que determina o nome do membro.
* `object Value`: Propriedade que determina o valor, ou seja, a própria entidade.
* `string ValueRaw`: Propriedade que determina o valor em forma de texto.
* `bool IsPrimitive`: Propriedade que indica se o valor é primitivo ou não.
* `string ComplexEntityId`: Propriedade que indica a identificação da entidade complexa.

## Entidades complexas em forma de texto - Tipos primitivos <header-set anchor-name="impl-factory-entity-complex-primitive" />

Para tipos primitivos, temos o seguinte formato:

```
[TypeName].[MemberName]: [Value]
```

Os colchetes indicam que a parte não é obrigatória, no formato acima vemos que a exibição do tipo não é obrigatória, contudo, quando ela estiver presente, devemos adicionar um `.`. 

A segunda parte não deve existir para a entidade que está localizada na raiz da expressão, afinal se ela está na raiz ela não tem pai. 

Da segunda entidade em diante, a segunda parte é obrigatória e indica o nome do membro que pode ser uma propriedade ou um campo.

A próxima parte é o separador `: ` que divide o nome do membro e seu valor. Se o valor do membro for nulo, o separador não será exibido.

**Vejamos alguns exemplos:**

_Exibe o tipo e o valor da entidade primitiva que está localizada na raiz. Por ser a entidade raiz então não existirá o nome do membro:_

```
System.String: Value
```

_Exibe o tipo, o membro e valor da entidade primitiva que está localizada da segunda posição em diante:_

```
System.String.StrValue: Value
```

_Exibe o tipo, o membro e um valor vazio da entidade primitiva que está localizada da segunda posição em diante:_

```
System.String.StrValue: 
```

_Exibe o tipo e o membro quando o valor for nulo. Não exibe o "separador", esse é o indicativo que o valor é nulo:_

```
System.String.StrValue
```

## Entidades complexas em forma de texto - Tipos complexos <header-set anchor-name="impl-factory-entity-complex-complex" />

Para tipos complexos, temos o seguinte formato:

```
[TypeName].[MemberName].EntityID
```

Os colchetes indicam que a parte não é obrigatória, no formato acima vemos que a exibição do tipo não é obrigatória, contudo, quando ela estiver presente, devemos adicionar um `.`. 

A segunda parte não deve existir para a entidade que está localizada na raiz da expressão, afinal se ela está na raiz ela não tem pai. 

Da segunda entidade em diante, a segunda parte é obrigatória e indica o nome do membro que pode ser uma propriedade ou um campo.

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

## Entidades complexas em forma de texto - Coleções e arrays <header-set anchor-name="impl-factory-entity-complex-collections" />

Para criar itens em uma coleção ou array, o nome do membro deve ser uma posição dentro dos colchetes: `[{position}]: Value`

No exemplo abaixo veremos como criar um array de inteiros usando apenas expressão de grafo. Note que no lugar do nome do membro, usamos os colchetes como indicativo de um item de coleção.

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

## Entendendo a classe `ComplexEntityFactory` <header-set anchor-name="impl-factory-entity-complex-class-complex-factory" />

Essa classe é a responsável por criar o grafo da entidade complexa com base na expressão. Internamente ela re-executa a expressão e gera cada entidade do grafo.

Essa classe contém o seguinte construtor:

```csharp
ComplexEntityFactory(Type type, Entity root = null)
```

* `type`: Esse parâmetro determina o tipo da entidade raiz.
* `root`: Esse parâmetro determina qual é a entidade raiz. Ele não é obrigatório, pois essa classe também é usada na desserialização onde a entidade raiz é obtida tardiamente.

Temos algumas propriedades que ajudarão na criação e customização das entidades:

* `IReadOnlyList<Entity> Entities`: Propriedade que armazena todas as entidades do grafo e sem repeti-las.
* `bool IsTyped`: Propriedade que indica se a criação tem um tipo definido.
* `IReadOnlyDictionary<Type, Type> MapTypes`: Propriedade que contém os mapas de criação para interfaces ou classes abstratas, ou até mesmo para classes concretas caso seja necessário.
* `IReadOnlyList<string> Errors`: Propriedade que armazena os erros.
* `bool IgnoreErrors`: Propriedade que indica se os erros serão ignorados. Do contrário será enviado uma exceção.
* `List<ITypeDiscovery> TypeDiscovery`: Propriedade que contém uma lista de classes de descoberta de tipos.
* `List<IValueLoader> ValueLoader`: Propriedade que contém uma lista de classes para inicializar valores.
* `List<IMemberInfoDiscovery> MemberInfoDiscovery`: Propriedade que contém uma lista de classes para descobrir membros.
* `List<ISetChild> SetChildAction`: Propriedade que contém uma lista de classes que fazem as atribuições das entidades filhas nas entidades pais.
* `Entity Root`: Propriedade que indica a entidade raiz.
* `Type RootType`: Propriedade que indica o tipo da entidade raiz.
* `object Value`: Valor da entidade raiz.

Por fim, temos alguns métodos que são usados durante a criação:

* `ComplexEntityFactory Build()`: Esse método é responsável por gerar o grafo. Esse método deve retornar a própria classe para manter a fluência.
* `void AddMapType<TFrom, TTo>()`: Esse método deve ser usado antes do método `Build` e ele determina o mapeamento dos tipos.
* `void AddError(string err)`

Existe também uma variação dessa classe que possibilita trabalhar de forma genérica e isso facilita o uso da propriedade `Value` que já estará com tipo definido.

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

## Descobridores de tipos <header-set anchor-name="impl-factory-entity-complex-discovery-types" />

Os "descobridores de tipos" tem como principal objetivo descobrir o tipo da entidade. A propriedade `TypeDiscovery` será usada para encontrar a melhor opção para cada entidade.

Os descobridores de tipos devem herdar da interface `ITypeDiscovery` e o método `CanDiscovery` é o responsável por determinar se a entidade pode ou não ser descoberta. Quando o método `CanDiscovery` retornar `true` então o método `GetEntityType` será chamado e é nesse momento que o tipo da entidade será retornado.

```csharp
public interface ITypeDiscovery
{
    bool CanDiscovery(Entity item);
    Type GetEntityType(Entity item);
}
```

A ordem dos descobridores é de extrema importância, uma vez que o último da lista será usado em caso de empate, ou seja, se três retornarem `true`, o último da lista será usado.

Por padrão, temos alguns descobridores de tipos definidos e todos eles já estão ordenados na propriedade `TypeDiscovery` para evitar erros.

1. `DictionaryItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item no dicionário. O método `CanDiscovery` verifica se o tipo pai é um dicionário, se for, então o método `GetEntityType` será chamado retornando o tipo `KeyValuePair<,>`.
2. `MemberInfoTypeDiscovery`: Essa classe é responsável por descobrir o tipo do membro. O método `CanDiscovery` verifica se a entidade contém um nome de membro, se tiver, então o método `GetEntityType` será chamado para obter o tipo do membro.
3. `ListItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item em uma lista. O método `CanDiscovery` verifica se o tipo pai é um `IList`, se for, então o método `GetEntityType` será chamado para obter o tipo da lista.
4. `ArrayItemTypeDiscovery`: Essa classe é responsável por descobrir o tipo de um item no array. O método `CanDiscovery` verifica se o tipo pai é um array, se for, então o método `GetEntityType` será chamado para obter o tipo do array.

## Descobridores de membros <header-set anchor-name="impl-factory-entity-complex-discovery-members" />

Os "descobridores de membros" tem o objetivo de descobrir o membro da entidade. A propriedade `MemberInfoDiscovery` será usada para encontrar a melhor opção de membros de cada entidade.

Os descobridores de membros devem herdar da interface `IMemberInfoDiscovery` e o método `CanDiscovery` é o responsável por determinar se a entidade pode ou não ter seu membro descoberto. Quando o método `CanDiscovery` retornar `true` então o método `GetMemberInfo` será chamado para retornar o `MemberInfo`.

```csharp
public interface IMemberInfoDiscovery
{
    bool CanDiscovery(Entity item);
    MemberInfo GetMemberInfo(Entity item);
}
```

Por padrão, temos apenas um classe definida em: `MemberInfoDiscovery`. 

```csharp
class MemberInfoDiscovery : IMemberInfoDiscovery
```

Caso queria substitui-lo, basta adicionar um nova classe na propriedade `MemberInfoDiscovery`. 

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
3. `item.Parent.Type != null`: Existe um tipo para a entidade pai. É com esse tipo e o nome do membro que obtemos o tipo do membro.

## Inicializadores de valores <header-set anchor-name="impl-factory-entity-complex-value-loaders" />

Os "inicializadores de valores" tem o objetivo de criar as entidades primitivas e complexas. A propriedade `ValueLoader` será usada para encontrar a melhor opção para cada entidade. Entenda o termo "valor" como sendo a entidade que será criada.

Os "inicializadores de valores" devem herdar da interface `IValueLoader` e o método `CanLoad` é o responsável por determinar se o tipo da entidade pode ou não ser inicializado. Quando o método `CanLoad` retornar `true` então o método `GetValue` será chamado para obter o valor que será a entidade.

```csharp
public interface IValueLoader
{
    bool CanLoad(Entity item);
    object GetValue(Entity item);
}
```

A ordem dos "inicializadores" é de extrema importância, uma vez que o último da lista será usado em caso de empate, ou seja, se três retornarem `true`, o último da lista será usado.

Por padrão, temos alguns "inicializadores de valores" definidos e todos eles já estão ordenados na propriedade `ValueLoader` para evitar erros.

1. `PrimitiveValueLoader`: Inicializa os tipos primitivos.
2. `ComplexEntityValueLoader`: Inicializa os tipos complexos. Caso o tipo tenha um construtor sem parâmetros então esse construtor será usado, do contrário a instância será criada sem chamar o construtor, ou seja, usando o método `FormatterServices.GetUninitializedObject(typeof(T))`. Esse inicializador só é utilizado quando a propriedade `ComplexEntityId` estiver preenchida, ou seja, se a entidade tiver identificação é porque ela é complexa.
3. `ArrayValueLoader`: Esse inicializador é utilizado quando a entidade é um array. Independente da quantidade de dimensões.
4. `ExpandoObjectValueLoader`: Esse inicializador é usado para tipos anônimos ou do tipo `ExpandoObject`.

**Importante:**

O tipo `ExpandoObject` será usado em todos os níveis quando a classe `ComplexEntityFactory` não tiver um tipo definido.

## Atribuição de filhos <header-set anchor-name="impl-factory-entity-complex-child-assign" />

A atribuição tem o objetivo de adicionar uma entidade filha em sua entidade pai, ou seja, atribuir um valor em um membro da instância pai, ou um item em uma lista por exemplo. A propriedade `SetChildAction` será usada para encontrar a melhor opção para cada entidade.

Uma classe que faz atribuição de filhos deve herdar da interface `ISetChild`, e o método `CanSet` é o responsável por determinar se o item filho pode ou não ser atribuído ao item pai. Quando o método `CanSet` retornar `true` então o método `SetChild` será chamado para fazer a atribuição.

```csharp
public interface ISetChild 
{
    bool CanSet(Entity item, Entity child);
    void SetChild(Entity item, Entity child);
}
```

A ordem é de extrema importância, uma vez que o último da lista será usado em caso de empate, ou seja, se três retornarem `true`, o último da lista será usado.

Por padrão, temos algumas classes de atribuição definidas e todas elas já estão ordenados na propriedade `SetChildAction` para evitar erros.

1. `MemberInfoSetChild`: Essa classe será utilizada quando a entidade filha tiver um membro definido.
2. `DictionarySetChild`: Essa classe será utilizada quando a entidade pai for um dicionário e a entidade filha tiver o nome do membro iniciado pelo caractere `[`. Isso significa que a entidade filha é um item e não uma propriedade da entidade pai.
3. `ExpandoObjectSetChild`: Essa classe é utilizada quando a entidade pai for do tipo `ExpandoObject`.
4. `ArraySetChild`: Essa classe é utilizada quando a entidade pai for do tipo `Array` e a entidade filha tiver o nome do membro iniciado pelo caractere `[`.
5. `ListSetChild`: Essa classe é utilizada quando a entidade pai for do tipo `IList` e a entidade filha tiver o nome do membro iniciado pelo caractere `[`.