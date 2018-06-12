[
![Inglês](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/ExpressionGraph)
[
![Português](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md)

# <a name="concept" />Expressão de grafos

O conceito de **expressão de grafos** foi criado em 2015 por _Glauber Donizeti Gasparotto Junior_ e tem como objetivo a representação de um grafo em forma de expressão.

A ideia de uma representação em forma de expressão é resumir um grafo em um texto que seja humanamente legível e de fácil transporte ou a partir de um grafo, fazer a engenharia reversa para obter sua representação em forma de expressão.

A representação em forma de expressão é focada em grafos simples, modelos de negócios com pequenas quantidades de objetos e também para fins didáticos. Não espere ver um novo jeito de serializar ou deserealizar grafos complexos, embora isso seja possível, já existem soluções melhores e bem sólidas.

Outro conceito que trazemos é a **pesquisa em grafos**. Usando apenas as informações extraídas das expressões podemos criar uma matriz vertical que possibilita a criação de pesquisas em grafos simples ou complexos.

É importante destacar que o conceito como um todo não tem o objetivo de ser performático ou ser melhor ou pior que outros já existentes. O objetivo é ser apenas uma nova forma de enxergar um grafo e suas informações.

# <a name="concept" />Índice

* [Compreendendo uma expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro)
  * [Resolução da expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-execution-order)
  * [Entidade e ocorrências](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-and-occurrence)
  * [Operador de soma](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro-plus)
  * [Operador de subtração](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro-subtract)
  * [Grupos de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group)
    * [Grupo de expressão raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-root)
    * [Sub-grupos de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-sub-group)
    * [Repetições de grupo de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-repeat)
    * [Entidade Pai](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-parent)
  * [Entidade raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-root)
  * [Entidade final](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-final)
  * [Caminhos de entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths)
    * [Caminhos cíclicos na expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths-cyclic)
* [Informações das entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-info)
  * [Níveis](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#levels)
  * [Índices](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#indexes)
  * [Navegação para a direita (Próxima entidade)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-next)
  * [Navegação para a esquerda (Entidade anterior)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-previous)
* [Normalizando expressões](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-formatters)
  * [Normalização - tipo 1](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-1)
  * [Normalização - tipo 2](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-2)
  * [Normalização - tipo 3](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-3)
* [Desnormalizando expressões](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#desnormalization)
* [Pesquisas em expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search)
    * [Matriz de informação](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#matrix-of-information)
  * [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep)
    * [Pesquisando todas as ocorrências de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-occurrences)
    * [Retornando a entidade anterior](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-previous)
    * [Retornando a próxima entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-next)
    * [Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-is-first-at-group-expression)
    * [Verificando se entidade é a última do grupo de expressão (última dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-has-last-at-group-expression)
    * [Verificando se entidade é a entidade raiz da expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-is-root)
    * [Pesquisando todas as entidades que contenham filhos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-with-children)
    * [Pesquisando todos os descendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-descendants)
    * [Pesquisando os filhos de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-children)
    * [Pesquisando todos os ascendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-ascending)
    * [Pesquisando o pai de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-parent)
  * [Pesquisa superficial](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-surface)
* [Implementações](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation)
  * [Criando grafos com expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-graph)
  * [Convertendo uma matriz de informação para expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-expression)

# <a name="intro" />Compreendendo uma expressão de grafos

Uma expressão de grafos é composta por 4 elementos básicos e diversas informações que vamos detalhar nesse documento.

**Expressão de grafos - Exemplo:**

```
(A + B + C + D)
```

Os elementos que compõe uma expressão são:

* **Entidade**: É o elemento fundamental da expressão, determina uma unidade, um vértice na teoria de grafo.
  * São únicos, mas podem aparecer `N` vezes na expressão em diferentes posições.
  * São representados por um literal, no caso acima, as letras: `A`, `B`, `C` e `D`.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade.
  * Fazendo uma analogia com a teoria de grafos, o operador de `+` pode ser visto como uma **aresta**.
* **Operador de subtração `-`**: É o elemento que remove uma entidade de outra entidade.
* **Parenteses `(` e `)`**: São usados para determinar um grupo de entidades filhas de uma determina entidade.
  * Em expressão de grafos são denominados: **Grupo de expressão**.

Esses elementos, são os mesmos de uma expressão matemática, a diferença é que no lugar de números teremos entidades que vão ser adicionas ou removidas uma nas outras. Além disso, o objetivo do resultado tem suas diferenças.

Essa expressão representa o seguinte grafo:

```
A 
----B
----C
----D
```

## <a name="expression-execution-order" />Resolução da expressão

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona ou remove a entidade da direita e o resultado dessa soma é a própria entidade da esquerda e assim sucessivamente até chegar no final da expressão.

**Exemplo simples (Etapas simbólicas da resolução):**

1. `(A + B)`
2. Resultado final da expressão: `A`

_Grafo final da entidade `A`_

```
A 
----B
```

**Exemplo composto (Etapas simbólicas da resolução):**

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Resultado final da expressão: `A`

_Grafo final da entidade `A`_

```
A 
----B
----C
----D
```

Vimos que a cada etapa da resolução de uma expressão a entidade da direita desaparece e a entidade da esquerda prevalece até não restarem entidades a sua direita.

É óbvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita.

## <a name="entity-and-occurrence" />Entidade e ocorrências

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome. Mas a mesma entidade pode aparecer em diversos pontos no grafo.

```
(A + (B + C + A) + C)
```

Note que na expressão acima as entidades `A` e `C` estão repetidas. Elas representam a mesma entidade, porém em posições diferentes. Cada ocorrência contém algumas informações que são únicas daquela posição como:

* <error>The anchor 'indexex' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>
* [Níveis](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#levels)
* [Navegação para a esquerda (Entidade anterior)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-previous)
* [Navegação para a direita (Próxima entidade)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-next)

## <a name="intro-plus" />Operador de soma

A operação de soma usa o operador `+`, como dito, ela funciona como uma aresta que liga um vértice a outro vértice. Em expressão de grafos, dizemos que a entidade da esquerda adiciona a entidade da direita e sem limitações, por exemplo:

* A entidade da esquerda pode adicionar a sí mesma quantas vezes for preciso:

```
Expression: A + A + A + A
Graph:
            A 
            ----A
            ----A
            ----A
```

* A entidade `X` pode adicionar a entidade `Y` e a entidade `Y` pode adicionar a entidade `X` quantas vezes for necessário.

```
Expression: X + (Y + X + X) + Y
Graph:
            X 
            ----Y
                ----X
                ----X
            ----Y
```

## <a name="intro-subtract" />Operador de subtração

A operação de subtração usa o operador `-`. Em expressão de grafos, dizemos que a entidade da esquerda remove a entidade da direita fazendo com que a entidade da direita deixe de ser sua filha.

A cada operação de subtração apenas uma ocorrência será removida por vez, mesmo que a entidade da esquerda tenha mais de uma filha da mesma entidade. Por exemplo:

* A entidade da esquerda remove uma das filhas `B`

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B

Graph 2:
            A 
            ----B
            ----B
```

Note que uma das ocorrências da entidade `B` foi removida da entidade `A`. Com base no mesmo exemplo, se quiséssemos remover todas as ocorrências da entidade `B` teríamos que fazer a operação de subtração 3 vezes, que é equivalente a quantidade de vezes que entidade `B` existe dentro da entidade `A`.

Ainda é possível misturar as operações de soma e subtração.

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B - B - B + (C + Y)

Graph 2:
            A
            ----C    
                ----Y
```

Nesse exemplo, removemos todas as ocorrências da entidade `B` da entidade `A` e adicionamos uma nova filha `C` que contém a entidade `Y`.

## <a name="expression-group" />Grupos de expressão

Os grupos são delimitados pelo uso de parenteses: `(` para abrir e `)` para fechar.

A primeira entidade do grupo de expressão (após abrir parenteses) determina a entidade pai daquele grupo, ou seja, todas as entidades subsequentes serão suas filhas até que se feche os parenteses.

**Exemplo 1:**

```
(A + B + C)
```

* A entidade `A` é a entidade pai de seu grupo de expressão e a entidade `B` e `C` são suas filhas.

**Exemplo 2:**

```
(A + B + (C + D))
```

* A entidade `A` é a entidade pai de seu grupo de expressão e a entidade `B` e `C` são suas filhas.
* A entidade `C` é a entidade pai de seu grupo de expressão e a entidade `D` é sua filha.

### <a name="expression-group-root" />Grupo de expressão raiz

O primeiro grupo de expressão é chamado de **grupo de expressão raiz**.

Não é obrigatório o uso dos parenteses no grupo de expressão raiz. Veremos que nos exemplos a seguir ambas as expressão estão corretas:

```
(A + B)
```

Ou

```
A + B
```

### <a name="expression-sub-group" />Sub-grupos de expressão

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`(A + B + (C + D))`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C` e a entidade `C` será pai da entidade `D`.

### <a name="expression-group-repeat" />Repetições de grupo de expressão

Um grupo de expressão não pode ser redeclarado na próxima vez que a entidade pai do grupo for utilizada.

Por exemplo:

```
A + B + (C + D + E) + (I + C)
```

* A entidade `C` tem os filhos `D` e `E`
* A entidade `I` tem como filha a entidade `C`, porém não é necessário redeclarar as entidades filhas de `C`.

**Errado:**

```
A + B + (C + D + E) + (I + (C + D + E))
```

### <a name="entity-parent" />Entidade Pai

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo.

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.

## <a name="entity-root" />Entidade raiz

A primeira entidade da expressão é a **entidade raiz** da expressão. Uma expressão só pode conter uma entidade raiz.

```
A + B + (C + A)
```

* A entidade `A` é a entidade raiz de toda expressão acima e será o topo do grafo.

## <a name="entity-final" />Entidade final

Uma entidade que não possui grupos de expressão em seu nível é chamada de **entidade final**. Isso não significa que a entidade não tenha filhos, veja:

**Entidade final sem filhos:**

```
(A + B + C + (D + E))
```

* As entidades `B`, `C` e `E` são entidades finais.

**Entidade final com filhos:**

```
(A + (B + C) + (D + B))
```

* A entidade `C` é final e não contém filhos
* A entidade `B`, do grupo de expressão da entidade `D`, também é final, mas ela contém filhos.

## <a name="paths" />Caminhos de entidades

Toda entidade contém um caminho que deve ser percorrido até chegar em sua posição. Para representar esse caminho podemos usar a seguinte notação:

```
A.B.C.D
```

Essa notação indica a localização da entidade `D` dentro da expressão abaixo:

```
A + (B + (C + D))
```

* A entidade `D` é filha da entidade `C`
* A entidade `C` é filha da entidade `B`
* A entidade `B` é filha da entidade `A`

A notação utiliza o caractere `.` entre a entidade pai e a entidade filha. A entidade da esquerda será a pai e a entidade da direita será a filha.

**Outras exemplos:**

_Expressão:_

```
(A + A + (B + C) + (D + B))
```

_Caminhos da entidade `A`:_

* _Ocorrência 1_: `A`
* _Ocorrência 2_: `A.A`

Na "ocorrência 2" temos uma relação cíclica, portanto a notação é interrompida quando isso acontece, do contrário teríamos um caminho infinito.

_Caminhos da entidade `B`:_

* _Ocorrência 1_: `A.B`
* _Ocorrência 2_: `A.D.B`

### <a name="paths-cyclic" />Caminhos cíclicos na expressão

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe um caminho cíclico entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação cíclica.

Note que o grafo contém dois caminhos cíclicos:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade `A` é pai dela mesma.
* Uma indireta (`C + A`): Onde `C` é pai de uma entidade ascendente, no caso a entidade `A`.

# <a name="entity-info" />Informações das entidades

Uma entidade, ou melhor, cada ocorrência de uma entidade na expressão, contém informações que são de extrema importância, veremos isso no tópico [Pesquisas em expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search).

São elas:

* [Níveis](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#levels)
* [Índices](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#indexes)
* Entidades vizinhas:
  * [Navegação para a esquerda (Entidade anterior)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-previous)
  * [Navegação para a direita (Próxima entidade)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-next)

## <a name="levels" />Níveis

Uma expressão tem dois tipos de níveis: **Nível geral** e **Nível na expressão**.

O **nível geral** determina em qual nível a entidade está com relação a hierarquia do grafo.

Por exemplo:

```
A (Nível 1)
----B (Nível 2)
    ----C (Nível 3)
    ----D (Nível 3)
        ----B (Nível 4)
----E (Nível 2)
    ----A (Nível 3)
```

O **nível na expressão** determina em qual nível a entidade está com relação a expressão.

Por exemplo:

```
                    A + B + C + ( D + E + ( F + G ) )
Nível na expressão: 1   1   1     2   2     3   3    
Nível geral:        1   2   2     2   3     3   4   
```

Note que o nível da expressão ignora o nível da entidade na hieraquia, é uma informação útil apenas para a expressão.

## <a name="indexes" />Índices

Uma expressão tem dois tipos de índices: **Índice na expressão** e **Índice do nível**.

O **Índice da expressão** determina em qual posição a entidade está na expressão. O índice inicia em zero e soma-se +1 até a última entidade da expressão.

Por exemplo:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

O **Índice do nível** determina em qual posição a entidade está com relação ao seu nível. O índice inicia em zero na primeira entidade do nível e soma-se +1 até a última entidade do mesmo nível. Por exemplo:

```
                 A + B + C + ( D + E + ( F + G ) )
Nível geral:     1   2   2     2   3     3   4   
Índice do nível: 0   0   1     2   0     1   0  
```

* A entidade `A` é a raiz da expressão e seu "índice no nível" será zero. Note que por ser a entidade raiz, ela não terá outras entidades em seu nível.
* A entidade `B` é a primeira do segundo nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` é a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` é a terceria do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` é a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` é a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` é a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.

## <a name="entity-next" />Navegação para a direita (Próxima entidade)

Toda entidade, com exceção da última da expressão, tem conhecimento da próxima entidade na expressão.

No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a direita da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) )
B   C   D     E   F     G
```

No exemplo, a entidade `A` tem conhecimento da entidade `B`. Note que a entidade `B` é filha de `A`, mas isso não influência, pois a ideia é conhecer a próxima entidade da expressão e não do seu nível.

## <a name="entity-previous" />Navegação para a esquerda (Entidade anterior)

Toda entidade, com exceção da primeira da expressão (a entidade raiz), tem conhecimento da entidade anterior na expressão. No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a esquerda da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) ) 
    A   B     C   D     E   F
```

# <a name="entity-formatters" />Normalizando expressões

As normalizações foram criadas para melhorar a visualização das expressões.

## <a name="normalization-1" />Normalização - tipo 1

A **normalização de tipo 1** tem o objetivo de enxugar grupos de expressão que pertencem a mesma entidade pai e que estão em diferentes lugares na expressão.

Por exemplo:

```
A + (B + Y) + (D + (B + C))
     ^              ^
```

Note que na expressão acima, a entidade `B` tem dois grupos de expressão em lugares distintos. Na prática, isso não tem nenhum problema, mas será visualmente melhor se aplicarmos a normalização eliminando um dos grupos da entidade `B`, veja:

```
A + (B + Y + C) + (D + B)
```

É preciso dizer que nenhuma alteração na expressão deve modificar o seu grafo final. É perceptível que no exemplo isso não ocorreu, as entidades apenas foram reoganizadas.

Já no próximo exemplo, veremos uma expressão que pode gerar confusão no momento da normalização:

```
A + (B + Y) + (D + (B + Y))
     ^              ^
```

Nesse exemplo, é natural pensar que um dos grupos da entidade `B` pode ser eliminado por serem iguais, mas esse pensamento está errado. Se eliminarmos um dos grupos, estaremos modificando o grafo final e esse não é o objetivo.

**Errado:**

```
A + (B + Y) + (D + B)
```

**Correto:**

```
A + (B + Y + Y) + (D + B)
```

## <a name="normalization-2" />Normalização - tipo 2

A **normalização de tipo 2** tem o objetivo de organizar, quando possível, as **entidades finais** no começo do seu grupo de expressão para ajudar na visualização da expressão.

```
A + (B + (C + D) + E) + F + G
                   ^    ^   ^
```

Após a normalização ficaria assim:

```
A + F + G + (B + E + (C + D))
    ^   ^        ^    
```

* Note que as entidades `F` e `G` foram para o ínicio do seu grupo de expressão.
* A entidade `E` também foi reorganizada para o ínicio do seu grupo de expressão.

## <a name="normalization-3" />Normalização - tipo 3

A **normalização de tipo 3** tem o objetivo de declarar o mais rápido possível todos os **grupos de expressão**.

Exemplo:

```
A + B + (C + G + (B + F)) + (G + F)
    ^             ^    
             ^               ^
```

Note que as entidades `B` e `G` são utilizadas antes que seus grupos sejam declarados e após a normalização teremos:

```
A + (B + F) + (C + (G + F) + B) + G
```

* Após a normalização, os grupos das entidades `B` e `G` foram declarados no primeiro momento que foram utilizadas.
* A entidade `B`, dentro do grupo `C` e a entidade `G` que está solitária no final da expressão, se transformaram em entidades finais e devido a isso, podemos aplicar a [Normalização - tipo 2](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-2) para melhorar a visualização, veja:

```
A + G + (B + F) + (C + B + (G + F))
```

* Podemos aplicar novamente a [Normalização - tipo 3](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-3) para declarar o grupo de expressão da entidade `G` após a sua movimentação para o inicio da expressão:

```
A + (G + F) + (B + F) + (C + B + G)
```

Com isso concluímos a normalização e temos acima uma expressão muito mais legível.

# <a name="desnormalization" />Desnormalizando expressões

O objetivo da **desnormalização** é gerar uma nova expressão onde os grupos de expressões sejam escritos toda vez que a sua entidade pai for utilizada. Após a desnormalização será impossível voltar na expressão original, esse é um caminho sem volta.

Considere a seguinte expressão original:

```
A + (B + D) + (E + B)
```

* Note que a entidade `B` é filha da entidade `A` e filha da entidade `E`.
* Após a desnormalização teremos a seguinte expressão:

```
A + (B + D) + (E + (B + D))
                    ^
```

* Após a desnormalização a entidade `B` teve seu grupo de expressão reescrito por completo quando foi utilizada novamente como filha da entidade `D`.

Como dito, é impossível voltar na expressão original, pois não conseguimos distinguir quais grupos de expressões eram da expressão original. Sendo assim, não podemos dizer que uma _expressão original_ é igual a sua _expressão desnormalizada_. Vejam um exemplo de como elas são diferentes:

```
Original:       A + (B + D) + (E + B)
Grafo final:
                A
                ---B
                ------D
                ---E
                ------B
```

Se pegarmos a expressão desnormalizada e extrairmos o seu grafo, teremos um grafo diferente do grafo original:

```
Original:                    A + (B + D) + (E + (B + D))
Após normalização de tipo 1: A + (B + D + D) + (E + B)
Grafo final:
                             A
                             ---B
                             ------D
                             ------D
                             ---E
                             ------B
```

Portanto, não podemos considerar que uma expressão desnormalizada seja usada como uma expressão original, isso altera o grafo final. Além do mais, ela infringe a regra do tópico [Repetições de grupo de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-repeat).

# <a name="search" />Pesquisas em expressões de grafos

A pesquisa em expressão de grafos pode ser dividida em duas partes: **Pesquisa superficial** e **Pesquisa profunda**.

Ambas utilizam de uma **matriz de informação** que veremos adiante para poder obter ensumos para a pesquisa.

### <a name="matrix-of-information" />Matriz de informação

Podemos representar uma expressão de grafos em uma matriz vertical com todas as informações de uma expressão.

Com a visão em forma de matriz conseguimos uma melhor visualização do grafo e entendemos melhor como funciona a pesquisa em grafos complexos usando o conceito de expressão de grafos. Vejamos um exemplo:

**Expressão:**

```
Expressão:          A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
Nível geral:        1   2     2   3       2   3     3     4   5   5     4     3
Índice do nível:    0   0     1   0       2   0     1     0   0   1     1     2
```

**Hierarquia:**

```
A (Indice do nível: 0)
----B (Indice do nível: 0)
----C (Indice do nível: 1)
    ----Y (Indice do nível: 0)
----D (Indice do nível: 2)
    ----E (Indice do nível: 0)
    ----F (Indice do nível: 1)
        ----G (Indice do nível: 0)
            ----B (Indice do nível: 0)
            ----C (Indice do nível: 1)
        ----Y (Indice do nível: 1)
    ----Z (Indice do nível: 2)
```

**Matriz:**

```
Índice geral    | Entidade | Nível geral | Índice do nível
#00             | A        | 1           | 0 
#01             | B        | 2           | 0 
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
#05             | E        | 3           | 0 
#06             | F        | 3           | 1 
#07             | G        | 4           | 0 
#08             | B        | 5           | 0 
#09             | C        | 5           | 1 
#10             | Y        | 4           | 1 
#11             | Z        | 3           | 2
```

Com base nessa matriz de informação e ao fato das entidades conhecerem os seus _vizinhos_, ou seja, aquelas que estão posicionadas na sua esquerda ou na sua direita na expressão (independentemente do nível) podemos criar meios de navegação e pesquisa de entidades.

## <a name="search-deep" />Pesquisa profunda

A **pesquisa profunda** tem o objetivo de retornar a maior quantidade de resultados possíveis e para isso ela considera todos os caminhos que uma entidade percorre em um grafo.

Para poder criar uma pesquisa profunda, precisamos utilizar uma **expressão desnormalizada**. Isso é necessário, porque apenas a expressão desnormalizada contém todos os caminhos que uma entidade possui no grafo uma vez que a versão original da expressão não repete os grupos de expressão (e nem deve).

Vejamos a seguir o mesmo exemplo utilizado no tópico [Matriz de informação](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#matrix-of-information), porém agora, a expressão foi desnormalizada:

**Expressão:**

```
Original:            A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
                               ^                                   ^
Desnormalizada:      A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
                                                                         ^
Nível geral:         1   2     2   3       2   3     3     4   5     5   6       4     3 
Índice do nível:     0   0     1   0       2   0     1     0   0     1   0       1     2
```

**Hierarquia:**

```
A (Indice do nível: 0)
----B (Indice do nível: 0)
----C (Indice do nível: 1) 
    ----Y (Indice do nível: 0)
----D (Indice do nível: 2)
    ----E (Indice do nível: 0)
    ----F (Indice do nível: 1)
        ----G (Indice do nível: 0)
            ----B (Indice do nível: 0)
            ----C (Indice do nível: 1)
                ----Y (Indice do nível: 0) *
        ----Y (Indice do nível: 1)
    ----Z (Indice do nível: 2)
```

* Foi aplicada a desnormalização e a entidade `C` teve seu grupo de expressão reescrito dentro da entidade `G`.
* Após a desnormalização um novo caminho foi criado para a entidade `Y`:
  * Antes:
    * _Ocorrência 1_: A.C.Y
    * _Ocorrência 2_: A.D.F.G.Y
  * Depois:
    * _Ocorrência 1_: A.C.Y
    * **_Ocorrência 2_: A.D.F.G.C.Y**
    * _Ocorrência 3_: A.D.F.G.Y

**Matriz:**

```
Índice geral    | Entidade | Nível geral | Índice do nível
#00             | A        | 1           | 0 
#01             | B        | 2           | 0 
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
#05             | E        | 3           | 0 
#06             | F        | 3           | 1 
#07             | G        | 4           | 0 
#08             | B        | 5           | 0 
#09             | C        | 5           | 1 
#10             | Y *      | 6           | 0
#11             | Y        | 4           | 1 
#12             | Z        | 3           | 2 
```

### <a name="search-deep-occurrences" />Pesquisando todas as ocorrências de uma entidade

Uma entidade pode ter mais de uma ocorrência em um grafo, no exemplo acima, se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontrariamos as linhas `#3`, `#10` e `#11`.

* Note que sem a desnormalização não seria possível encontrar a linha `#10` e não seria possível obter o número correto de ocorrências dessa entidade.

### <a name="search-deep-get-entity-previous" />Retornando a entidade anterior

Para retornar a entidade anterior de uma determinada entidade, devemos subtrair o **índice geral** em `-1`. Por exemplo:

Para obter a entidade anterior da entidade `Y` da linha `#03`, pegamos seu índice geral (= `3`) e subtraímos `-1`. Com o resultado (= `2`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornariamos a entidade `C`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
```

* Se o resultado for menor que zero, é porque estamos na entidade raiz e não existe entidade anterior.

### <a name="search-deep-get-entity-next" />Retornando a próxima entidade

Para retornar a próxima entidade de uma determinada entidade, devemos somar o **índice geral** em `+1`. Por exemplo:

Para obter a próxima entidade da entidade `Y` da linha `#03`, pegamos seu índice geral (=`3`) e somamos `+1`. Com o resultado (= `4`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornariamos a entidade `D`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
```

* Se o resultado for maior que a quantidade máxima de itens na matriz é porque estamos na última entidade da expressão e não existe uma próxima entidade.

### <a name="search-deep-is-first-at-group-expression" />Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese)

Para descobrir se uma entidade é a primeira do seu grupo de expressão (primeira dentro do parênteses),verificamos se o seu **nível geral** é maior que o índice geral da **próxima entidade**, se for, essa entidade é a primeira de seu grupo de expressão.

```
                A + B + ( C + Y ) + (D + C)
Nível geral:    1   2     2   3      2   3
Index:          0   1     2   3      4   5
```

No exemplo acima, a entidade `C`, do índice `#02`, tem o nível geral igual á `2` e a sua próxima entidade `Y` tem o nível geral igual á `3`, sendo assim, ela é a primeira dentro de seu parênteses.

**Observação:**

Não confunda essa regra como sendo a solução para verificar se uma entidade contém filhos.

Note que a entidade `C` se repete no final da expressão e essa regra não se aplicaria nesse caso. E isso ocorre porque seu grupo de expressão não foi redeclarado.

Portanto, se o seu propósito é descobrir se a entidade `C`, que está no índice `#05`, contém filhos devemos aplicar essa mesma regra com alguns passos a mais: localizar a primeira ocorrência da entidade `C` e aplicar a regra.

1. Aplica a regra na entidade `C` do índice `#05`.
2. Se não tiver grupo de expressão: busca a primeira ocorrência da entidade `C`, nesse caso encontrariamos o índice `#02`. Se não existir uma ocorrência anterior é porque a ocorrência atual é a primeira ocorrência, sendo assim, essa entidade não teria filhos.
3. Usando a ocorrência do índice `#02`, aplicamos essa regra e pronto! Agora sabemos se a entidade contém filhos.

### <a name="search-deep-has-last-at-group-expression" />Verificando se entidade é a última do grupo de expressão (última dentro dos parêntese)

Para descobrir se uma entidade é a última do seu grupo de expressão (última dentro do parênteses), verificamos se seu **nível geral** é maior que o índice geral da **próxima entidade**, se for, essa entidade é a última do seu grupo de expressão.

```
                A + B + ( C + Y ) + (D + C) + U
Nível geral:    1   2     2   3      2   3    2
Index:          0   1     2   3      4   5    6
```

No exemplo acima, a entidade `Y`, do índice `#03`, tem o nível geral igual á `3` e a sua próxima entidade `D` tem o nível geral igual á `4`, sendo assim, ela é a última dentro de seu parênteses.

* A entidade `U` do índice `#06` não tem uma próxima entidade, portanto ela é a última de seu grupo de expressão, embora ele esteja omitido por estarmos no **grupo de expressão raiz**.

### <a name="search-deep-is-root" />Verificando se entidade é a entidade raiz da expressão

Para descobrir se a entidade é a raiz da expressão, verificamos se o seu **índice geral** é igual á `0`, se for, então ela será a entidade raiz.

```
        A + B
Index:  0   1
```

* A entidade `A` é a raiz.

### <a name="search-deep-with-children" />Pesquisando todas as entidades que contenham filhos

Para isso, basta recuperar as **entidades anteriores** de todas as entidades cujo o **índice do nível** seja igual a `0`.

Com base no exemplo, teremos:

1. Primeiro, encontramos todas as linhas com o índice do nível igual a zero:
  * `#00 (A)`
  * `#01 (B)`
  * `#03 (Y)`
  * `#05 (E)`
  * `#07 (G)`
  * `#08 (B)`
  * `#10 (Y)`
2. Para cada linha encontrada, retornamos a sua entidade anterior que será uma entidade pai:
  * `NULL` -> `#00 (A)`: Não contém entidade anterior, portanto não retorna nada.
  * `#00 (A)` -> `#01 (B)`: Retorna a entidade `A` como sendo sua anterior
  * `#02 (C)` -> `#03 (Y)`: Retorna a entidade `C` como sendo sua anterior
  * `#04 (D)` -> `#05 (E)`: Retorna a entidade `D` como sendo sua anterior
  * `#06 (F)` -> `#07 (G)`: Retorna a entidade `F` como sendo sua anterior
  * `#07 (G)` -> `#08 (B)`: Retorna a entidade `G` como sendo sua anterior
  * `#09 (C)` -> `#10 (Y)`: Retorna a entidade `C` como sendo sua anterior

Com isso, após removermos as repetições de entidades (no caso a entidade `C` que aparece nas linhas `#2` e `#09`), obtemos como resultado final as entidades `A`, `C`, `D`, `F` e `G` como sendo as únicas entidades com filhos na expressão.

### <a name="search-deep-descendants" />Pesquisando todos os descendentes de uma entidade

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se a próxima entidade tem seu **nível geral** maior que o **nível geral** da entidade desejada, se tiver, essa entidade é uma descendente.

Devemos continuar navegando para frente até quando a próxima entidade tiver o mesmo **nível geral** da entidade desejada ou se a expressão não tiver mais entidades.

Por exemplo, se quisermos pegar os descendentes da entidade `F`.

* A entidade `F` tem o nível geral igual a 3
* **A entidade `G` é a próxima entidade depois de `F` e o seu nível geral é 4, é descendente.**
* **A entidade `B` é a próxima entidade depois de `G` e o seu nível geral é 5, é descendente.**
* **A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é 5, é descendente.**
* **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é 6, é descendente.**
* **A entidade `Y` é a próxima entidade depois de `Y` e o seu nível geral é 4, é descendente.**
* A entidade `Z` é a próxima entidade depois de `Y`, porém o seu nível é 3, igual ao nível da entidade `F`, portanto não é descendente.

Após eliminarmos as repetições de entidades, obtemos como resultado final as seguintes entidades descendentes: `G`, `B`, `C` e `Y`

### <a name="search-deep-get-entity-children" />Pesquisando os filhos de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas os filhos da entidade `D`, precisariamos limitar o nível geral dos descendentes á: _[nível geral da entidade corrente] + 1_

* A entidade `D` tem o nível geral igual a 2
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`.**
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`.**
* As próximas entidades depois de `F` são: `G`, `B`, `C`, `Y` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`.**

Acabou a expressão e no final teremos as seguintes entidades descendentes: `E`, `F` e `Z`

### <a name="search-deep-get-entity-ascending" />Pesquisando todos os ascendentes de uma entidade

Se quisermos encontrar os ascendentes de uma entidade, devemos verificar se a entidade anterior tem seu **nível geral** menor que o **nível geral** da entidade desejada, se tiver, essa entidade é uma ascendente.

```
             A + B
Nível geral: 1   2
             ^   *
Parent of B: A
```

Se a entidade anterior for do mesmo nível da entidade deseja, deve-se ignora-la e continuar navegando para trás até encontrar a primeira entidade com o **nível geral** menor que o **nível geral** da entidade desejada.

```
             A + B + J
Nível geral: 1   2   2
             ^       *
Parent of J: A
```

Após encontrar a primeira ascendência, deve-se continuar navegando para trás, porém o **nível geral** a ser considerado agora será o da primeira ascendência e não mais da entidade desejada. Esse processo deve continuar até chegar na entidade raiz.

```
              A + B + (J + Y)
Nível geral:  1   2    2   3
              ^        ^   *
Parents of Y: J, A
```

Por exemplo, se quisermos pegar os ascendentes da entidade `C` considerando todas as suas ocorrências na matriz do primeiro exemplo, teremos:

**Ocorrência 1:**

* A entidade `C` da linha `#2` tem o nível geral igual a 2
* `#01`: A entidade `B` tem o nível geral igual a 2, não é ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a 1, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível 1 e não mais o nível 2.**

Acabou a expressão e teremos as seguintes entidades ascendentes: `A`

**Ocorrência 2:**

* A entidade `C` da linha `#09` tem o nível geral igual a 5
* `#08`: A entidade `B` tem o nível geral igual a 5, não é ascendente.
* `#07`: **A entidade `G` tem o nível geral igual a 4, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível 4 e não mais o nível 5.**
* `#06`: **A entidade `F` tem o nível geral igual a 3, é menor que o nível geral da entidade `G`, portanto é uma ascendente. Agora o nível a ser considerado será o nível 3 e não mais o nível 4.**
* `#05`: A entidade `E` tem o nível geral igual a 3, não é uma ascendente.
* `#04`: **A entidade `D` tem o nível geral igual a 2, é uma ascendente. Agora o nível a ser considerado será o nível 2 e não mais o nível 3.**
* `#03`: A entidade `Y` tem o nível geral igual a 3, não é uma ascendente.
* `#02`: A entidade `C` tem o nível geral igual a 2, não é uma ascendente.
* `#01`: A entidade `B` tem o nível geral igual a 2, não é uma ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a 1, é uma ascendente. Agora o nível a ser considerado será o nível 1 e não mais o nível 2.**

Acabou a expressão e no final teremos as seguintes entidades ascendentes: `G`, `F`, `D` e `A`

### <a name="search-deep-get-entity-parent" />Pesquisando o pai de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisariamos limitar o nível geral dos ascendentes á: _[nível geral da entidade corrente] - 1_; ou a primeira entidade com o nível geral menor que a entidade desejada.

Como existem 3 ocorrências da entidade `Y`, teremos um resultado por ocorrência:

**Ocorrência 1:**

* A entidade `Y` da linha `#3` tem o nível geral igual a 3
* `#02`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a 2, portanto, ela é pai da entidade `Y`.**

**Ocorrência 2:**

* A entidade `Y` da linha `#10` tem o nível geral igual a 6
* `#09`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a 5, portanto, ela é pai da entidade `Y`.**

**Ocorrência 3:**

* A entidade `Y` da linha `#11` tem o nível geral igual a 4
* `#10`: A entidade `Y` tem o nível geral igual a 6, não é uma ascendente.
* `#09`: A entidade `C` tem o nível geral igual a 5, não é uma ascendente.
* `#08`: A entidade `B` tem o nível geral igual a 5, não é uma ascendente.
* `#07`: A entidade `G` tem o nível geral igual a 4, não é uma ascendente.
* `#06`: **A entidade `F` é a entidade anterior a `G` e tem o nível geral igual a 3, portanto, ela é pai da entidade `Y`.**

## <a name="search-surface" />Pesquisa superficial

Na **Pesquisa superficial** a técnica usada é a mesma da **Pesquisa profunda**, á única deferença é que na pesquisa superficial não consideramos os caminhos que já foram escritos (ou percorridos). No caso, não usamos a técnica da desnormalização para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas não terá a mesma precisão da _Pesquisa profunda_.

Por exemplo, se quisermos retornar todas as ocorrências da entidade `Y`, teriamos a seguinte diferença entre os tipos de pesquisas:

**Expressão:**

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
```

**Pesquisa profunda:**

Primeiro, aplica-se a desnormalização:

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
```

* _Ocorrência 1_: A.C.Y
* _Ocorrência 2_: A.D.F.G.C.Y -> Novo caminho
* _Ocorrência 3_: A.D.F.G.Y

**Pesquisa superficial:**

Utiliza a expressão original:

* _Ocorrência 1_: A.C.Y
* _Ocorrência 2_: A.D.F.G.Y

# <a name="implementation" />Implementações

Esse tópico vai demostrar na prática alguns exemplos de implementações de alguns dos conceitos que estudamos.

* [Criando grafos com expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-graph)
* [Convertendo uma matriz de informação para expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-expression)
* <error>The anchor 'implementation-to-matrix' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>

Usaremos a linguagem de programação `C#` devido a sua capacidade de sobrecarregar operadores matemáticos.

## <a name="implementation-to-graph" />Criando grafos com expressão de grafos

Nesse exemplo vamos demostrar como criar um grafo usando apenas expressão de grafos da forma mais simples e objetiva possível.

Será usado uma **entidade hierárquicas**, ou seja, uma entidade que se relaciona com ela mesma.

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
  * Quando houver uma soma (`+`), a entidade da direita será adicionada na lista da entidade da esquerda, e a entidade da esquerda será devolvida como resultado. Essa é a base do conceito de expressão de grafos.
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

Note que a expressão é exatamente igual a todas as expressões que vimos durante esse estudo. Isso mostra que para entidades hierárquicas é possível usufruir desse conceito sem o uso de grandes blocos de código.

Para entidades de maior complexidade, não seria possível o uso dos operadores de forma tão simples, haveria a necessidade de criar mecanismos de reflexão e o uso de `strings` para a criação e processamento da expressão. Além do mais, não recomendamos esse esforço, não é o objetivo desse conceito criar mecanismo de serialização e deserialização de entidades, para isso existe meios melhores como: `XML` e `JSON`.

## <a name="implementation-to-expression" />Convertendo uma matriz de informação para expressões de grafos

Nesse exemplo veremos como converter uma matriz de informação de volta para expressão de grafos.

É importante destacar que esse código é simples e específico para o nosso exemplo. Embora ele possa ser útil para diversos propósitos devido a sua capacidade de identificar os momentos corretos de ínicio e fim de uma iteração de uma entidade.

```csharp
[DebuggerDisplay("{Entity.Name}")]
public class EntityItem
{
    private readonly Expression expression;

    public EntityItem(Expression expression)
    {
        this.expression = expression;
    }

    public int Index { get; set; }
    public int IndexAtLevel { get; set; }
    public int Level { get; set; }
    public int LevelAtExpression { get; set; }
    public Entity Entity { get; set; }

    public EntityItem Previous { get => expression.ElementAtOrDefault(Index - 1); }
    public EntityItem Next { get => expression.ElementAtOrDefault(Index + 1); }
    public EntityItem Parent
    {
        get
        {
            var previous = this.Previous;
            while(previous != null)
            {
                if (previous.Level < this.Level)
                    return previous;
                previous = previous.Previous;
            }
            return null;
        }
    }
}
```

* Essa classe será nossa representação de cada linha da matriz de informação, ou seja, cada ocorrência de uma entidade dentro da expressão. Nela teremos todas as propriedades que uma ocorrência de uma entidade pode ter.
* Nas propriedades `Previous`, `Next` e `Parent`, estamos implementando, respectivamente, as regras:
  * [Retornando a entidade anterior](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-previous)
  * [Retornando a próxima entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-next)
  * [Pesquisando o pai de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-parent)

```csharp
public class Expression : List<EntityItem>
{
    public string ToExpressionAsString()
    {
        var parenthesisToClose = new Stack<EntityItem>();
        var output = "";
        foreach (var item in this)
        {
            var next = item.Next;
            var hasChildren = next != null && item.Level < next.Level;
            var isLastInParenthesis = next == null || item.Level > next.Level;
            var isNotRoot = item.Index > 0;

            if (isNotRoot) 
                output += " + ";

            if (hasChildren) 
            {                
                output += "(";
                parenthesisToClose.Push(item);
            } 

            output += item.Entity.Name.ToString();

            if (isLastInParenthesis)
            {
                int countToClose;

                if (next == null)
                    countToClose = parenthesisToClose.Count;
                else
                    countToClose = item.Level - next.Level;

                for (var i = countToClose; i > 0; i--)
                {
                    output += ")";

                    // End iteration for entity: entityItemToClose
                    var entityItemToClose = parenthesisToClose.Pop();
                    // DO anything
                }
            }
        }

        return output;
    }
}

class Program 
{
    static void Main(string[] args)
    {
        var A = new Entity("A");
        var B = new Entity("B");
        var C = new Entity("C");
        var Y = new Entity("Y");
        var D = new Entity("D");
        var E = new Entity("E");
        var F = new Entity("F");
        var G = new Entity("G");
        var Z = new Entity("Z");

        var expression = new Expression();
        expression.Add(new EntityItem(expression) { Entity = A, Index = 0, IndexAtLevel = 0, Level = 1 });
        expression.Add(new EntityItem(expression) { Entity = B, Index = 1, IndexAtLevel = 0, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = C, Index = 2, IndexAtLevel = 1, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = Y, Index = 3, IndexAtLevel = 0, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = D, Index = 4, IndexAtLevel = 2, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = E, Index = 5, IndexAtLevel = 0, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = F, Index = 6, IndexAtLevel = 1, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = G, Index = 7, IndexAtLevel = 0, Level = 4 });
        expression.Add(new EntityItem(expression) { Entity = B, Index = 8, IndexAtLevel = 0, Level = 5 });
        expression.Add(new EntityItem(expression) { Entity = C, Index = 9, IndexAtLevel = 1, Level = 5 });
        expression.Add(new EntityItem(expression) { Entity = Y, Index = 10, IndexAtLevel = 1, Level = 4 });
        expression.Add(new EntityItem(expression) { Entity = Z, Index = 11, IndexAtLevel = 2, Level = 3 });
        var expressionString = expression.ToExpressionAsString();
    }
}
```

No método `Main` temos a chamada da nossa função, note que estamos criando a matriz de informação de forma manual. Essa matriz deve representar a seguinte expressão:

```
(A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z))
```

A função `ToExpressionAsString` será responsável por fazer toda a iteração e chegar em nosso objetivo que é devolver uma `string` contendo nossa expressão.

* A classe `Expression` representa uma expressão de grafos como um todo. Ela herda de uma lista do tipo `EntityItem` para fazer jus ao que ela é dentro do conceito: Um conjunto de ocorrências de entidades com suas informações.
* O método `ToExpressionAsString` retorna uma string que será a nossa expressão.
* A lista contendo todas as ocorrências das entidades será percorrida complementamente. Da posição 0 até o final da lista. Cada iteração pode conter diversos níveis da expressão.
* A variável `parenthesisToClose` armazena uma lista de todos os parênteses que foram abertos e precisam ser fechados. A lista tem que estar no formato: último a entrar, primeiro a sair.
* Para cada iteração:
  * Se a entidade for a entidade raiz, não adiciona o sinal de `+`.
    * [Verificando se entidade é a entidade raiz da expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-is-root)
  * Se a entidade tiver filhos, adiciona o caractere `(`
    * [Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-is-first-at-group-expression)
  * Se a entidade for a última do seu grupo de expressão (última dentro dos parênteses), então feche com o caractere `)`. Como diversos parênteses podem ter sido abertos nas iterações anteriores, então devemos calcular a quantidade de parênteses que precisam ser fechados e fecha-los. A variável `parenthesisToClose` contém a entidade que está sendo fechada, isso pode ser útil para alguma lógica.
    * [Verificando se entidade é a última do grupo de expressão (última dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-has-last-at-group-expression)

Com esses treixos de códigos vimos como é simples iterar em uma expressão de grafos e entender seus momentos. Além de abrir caminhos para implementações mais completas como: **pesquisa em expressão de grafos.**