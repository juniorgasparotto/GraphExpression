[
![Inglês](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/GraphExpression)
[
![Português](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md)

# <a name="implementation" />Expressão de grafos

Esse framework tem como objetivo implementar o conceito de expressão de grafos na linguagem .NET.

Resumidamente, o conceito de **expressão de grafos** tem como objetivo explorar os benefícios de uma expressão matemática trocando os números por entidades. Com isso, podemos criar uma nova maneira de transportar dados e principalmente criar um novo meio de pesquisa transversal em grafos circulares ou complexos.

Com relação a pesquisa em grafos, esse projeto se inspirou na implementação do `JQuery` para pesquisas de elementos HTML (DOM), unindo assim o conceito de expressão de grafos com a facilidade de uso do `JQuery` para pesquisas transversais.

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#concept) se você quiser conhecer mais sobre o conceito de expressão de grafos.

# <a name="index" />Índice

* [Instalação](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#install)
* [Doações](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#donate)
* [Licença](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md#license)

# Grafos circular

Um grafo circular é a maneira mais simples de implementar e entender como as coisas funcionam. Basicamente, é uma classe que faz referencia para ela mesma. A cardinalidade pouco importa aqui, pois isso quem vai determinar é quem implementa os operadores da expressão.

No exemplo a seguir, mostraremos uma forma de implementar o conceito de expressão de grafos sem nenhum framework, usando apenas `C#` e a matemática.

A ideia desse exemplo é criar um grafo circular da classe `Entity` onde o operador de soma vai incrementar a entidade da direita na entidade da esquerda da expressão.

[Clique aqui](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept-pt-br.md#intro) para saber mais...

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

# <a name="install" />Instalação

Via [NuGet](https://www.nuget.org/packages/GraphExpression/):

```
Install-Package GraphExpression
```

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