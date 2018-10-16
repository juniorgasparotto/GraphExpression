# <anchor-set name="implementation">Expressão de grafos</anchor-set>

Esse framework tem como objetivo implementar o conceito de expressão de grafos na linguagem .NET.

Resumidamente, o conceito de **expressão de grafos** tem como objetivo explorar os benefícios de uma expressão matemática trocando os números por entidades. Com isso, podemos criar uma nova maneira de transportar dados e principalmente criar um novo meio de pesquisa transversal em grafos circulares ou complexos.

Com relação a pesquisa em grafos, esse projeto se inspirou na implementação do `JQuery`  para pesquisas de elementos HTML (DOM), unindo assim o conceito de expressão de grafos com a facilidade de uso do `JQuery` para pesquisas transversais.

<anchor-get name="concept">Clique aqui</anchor-get> se você quiser conhecer mais sobre o conceito de expressão de grafos.

# <anchor-set name="index">Índice</anchor-set>

<table-of-contents />

# Grafos circular

Um grafo circular é a maneira mais simples de implementar e entender como as coisas funcionam. Basicamente, é uma classe que faz referencia para ela mesma. A cardinalidade pouco importa aqui, pois isso quem vai determinar é quem implementa os operadores da expressão.

No exemplo a seguir, mostraremos uma forma de implementar o conceito de expressão de grafos sem nenhum framework, usando apenas `C#` e a matemática.

A ideia desse exemplo é criar um grafo circular da classe `Entity` onde o operador de soma vai incrementar a entidade da direita na entidade da esquerda da expressão.

<anchor-get name="intro">Clique aqui</anchor-get> para saber mais...

```csharp
class Program
{
    static void Main(string[] args)
    {
        var A = new Entity("A");
        var B = new Entity("B");
        var C = new Entity("C");
        var D = new Entity("D");
        
        A = A + B + (C + D);  // ACTION: ADD

        /*
        **** PRINT *****
        * A 
        * ---B
        * ---C 
        * ----- D
        ****************
        */
        
        C = C - D; // ACTION: REMOVE

        /*
        **** PRINT *****
        * A 
        * ---B
        * ---C
        ****************
        */
    }

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
}
```