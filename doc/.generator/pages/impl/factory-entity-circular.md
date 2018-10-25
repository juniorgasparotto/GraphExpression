# Criando entidades circulares com expressão de grafos e a matemática <header-set anchor-name="impl-factory-entity-circular" />

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