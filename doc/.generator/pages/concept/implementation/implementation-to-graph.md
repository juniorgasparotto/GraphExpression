# Implementações <header-set anchor-name="implementation" />

Esse tópico vai demostrar na prática alguns exemplos de implementações de alguns dos conceitos que estudamos.

* <anchor-get name="implementation-to-graph" />
* <anchor-get name="implementation-to-expression" /> 
* <anchor-get name="implementation-to-matrix" /> 

Usaremos a linguagem de programação `C#` devido a sua capacidade de sobrecarregar operadores matemáticos.

## Criando grafos com expressão de grafo <header-set anchor-name="implementation-to-graph" />

Nesse exemplo vamos demostrar como criar um grafo usando apenas expressão de grafo da forma mais simples e objetiva possível.

Será usado uma **entidade circular**, ou seja, uma entidade que se relaciona com ela mesma. 

```csharp
[DebuggerDisplay("{Name}")]
public class Entity : List<Entity>
{
    public string Name { get; private set; }
    public Entity(string identity) => this.Name = identity;

    public static Entity operator +(Entity a, Entity b)
    {
        a.Add(b);
        return a;
    }

    public static Entity operator -(Entity a, Entity b)
    {
        a.Remove(b);
        return a;
    }
}
```

* A classe herda de uma lista genérica da própria classe, nossa intenção é criar uma instância hierárquica.
* A classe exige um nome como parâmetro de entrada, será o nome da entidade
* Os operadores `+` e `-` foram sobrescritos, agora essa entidade pode ser utilizada dentro de uma expressão.
    * Quando houver uma soma (`+`), a entidade da direita será adicionada na lista da entidade da esquerda, e a entidade da esquerda será devolvida como resultado. Essa é a base do conceito de expressão de grafo.
    * Quando houver uma subtração (`-`), a entidade da direita será removida na lista da entidade da esquerda, e a entidade da esquerda será devolvida como resultado.

Para usar é simples, basta pensar no conceito explicado e usar como se fosse uma expressão matemática dentro do `C#`:

```csharp
class Program
{
    static void Main(string[] args)
    {
        var A = new Entity("A");
        var B = new Entity("B");
        var C = new Entity("C");
        var D = new Entity("D");
        var E = new Entity("E");
        var F = new Entity("F");
        var Y = new Entity("Y");
        var H = new Entity("H");

        // expression1
        A = A + B + (C + (D + E + F)) + (Y + H);

        // expression2
        D = D - E;
    }
}
```

Após a execução da primeira expressão temos o seguinte grafo:

```
A
----B
----C
    ----D
        ----E
        ----F
----Y
    ----H
```

Após a execução da segunda expressão, vemos que a entidade `D` não tem mais a entidade `E` como filha, ela foi subtraída/removida:

```
A
----B
----C
    ----D
        ----F
----Y
    ----H
```

Note que a expressão é exatamente igual a todas as expressões que vimos durante esse estudo. Isso mostra que para entidades circulares é possível usufruir desse conceito sem o uso de grandes blocos de código.

Para entidades de maior complexidade, não seria possível o uso dos operadores de forma tão simples, haveria a necessidade de criar mecanismos de reflexão e o uso de `strings` para a criação e processamento da expressão. Além do mais, não recomendamos esse esforço, não é o objetivo desse conceito criar mecanismo de serialização e deserialização de entidades, para isso existe meios melhores como: `XML` e `JSON`.