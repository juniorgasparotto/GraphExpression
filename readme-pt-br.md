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

# <a name="index" />Índice

* [Compreendendo uma expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro)
  * [Resolução da expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-execution-order)
  * [Entidade e suas ocorrências](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-and-occurrence)
  * [Operador de soma](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro-plus)
  * [Operador de subtração](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#intro-subtract)
  * [Grupo de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group)
    * [Grupo de expressão raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-root)
    * [Sub-grupos de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-sub-group)
    * [Declarações de entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-declaration)
    * [Repetições de grupo de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-repeat)
    * [Entidade pai](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-parent)
  * [Entidade raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-root)
  * [Entidade final](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-final)
  * [Caminhos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths)
    * [Caminhos cíclicos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths-cyclic)
* [Informações de uma ocorrência](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-info)
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
  * [Pesquisa superficial](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-surface)
  * [Pesquisas sem referência](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep)
    * [Encontrando a entidade raiz da expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-find-root)
    * [Encontrando as "entidades pais" de uma expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-find-parents)
  * [Pesquisas com referência](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-with-references)
    * [Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-is-first-at-group-expression)
    * [Verificando se uma entidade é a última do grupo de expressão (última dentro dos parêntese)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-is-last-at-group-expression)
    * [Encontrando a entidade anterior](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-entity-previous)
    * [Encontrando a próxima entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-entity-next)
    * [Encontrando todas as ocorrências de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-occurrences)
    * [Encontrando todos os descendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-find-descendants)
    * [Encontrando os filhos de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-entity-children)
    * [Encontrando todos os ascendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-entity-ascending)
    * [Encontrando os pais de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-method-get-entity-parent)
* [Implementações](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation)
  * [Criando grafos com expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-graph)
  * [Convertendo uma matriz de informação para expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-expression)
  * [Criando uma matriz de informações a partir de um grafo](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-matrix)

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

## <a name="entity-and-occurrence" />Entidade e suas ocorrências

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome. Mas a mesma entidade pode aparecer em diversos pontos no grafo.

```
(A + (B + C + A) + C)
```

Note que na expressão acima as entidades `A` e `C` estão repetidas. Elas representam a mesma entidade, porém em posições diferentes. Cada ocorrência contém algumas informações que são únicas daquela posição. Veremos isso no tópico [Informações de uma ocorrência](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-info).

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

## <a name="expression-group" />Grupo de expressão

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

### <a name="entity-declaration" />Declarações de entidades

Chamamos de "**declaração**" o primeiro momento em que uma entidade é escrita, ou seja, sua primeira ocorrência.

Caso essa entidade contenha filhos devemos declarar todo o seu grupo de expressão no mesmo momento, ou seja, adicionando seus filhos dentro dos parenteses.

Não existe uma obrigatoriedade para a declaração do grupo de expressão ser na primeira ocorrência, mas isso ajuda a simplificar a descoberta de algumas informações de uma maneira mais rápida.

Por exemplo, para descobrir se a entidade `B` contém filhos na expressão a seguir, será necessário verificar todas as suas ocorrências, pois não é possível dizer em qual das ocorrências o seu grupo de expressão foi declarado.

```
A + B + (C + (B + D)) + B
```

Agora, se soubermos que os grupos de expressões foram escritos sempre nas primeiras ocorrências, então podemos verificar apenas a primeira ocorrência da entidade `B` para saber se ela contém ou não filhos:

```
A + (B + D) + (C + B) + B
```

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

### <a name="entity-parent" />Entidade pai

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
* A última ocorrência da entidade `B`, do grupo de expressão da entidade `D`, também é final, mas ela contém filhos.

## <a name="paths" />Caminhos

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

Na segunda ocorrência temos uma relação cíclica, portanto a notação é interrompida quando isso acontece, do contrário teríamos um caminho infinito.

_Caminhos da entidade `B`:_

* _Ocorrência 1_: `A.B`
* _Ocorrência 2_: `A.D.B`

### <a name="paths-cyclic" />Caminhos cíclicos

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe um caminho cíclico entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação cíclica.

Note que o grafo contém dois caminhos cíclicos:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade `A` é pai dela mesma.
* Uma indireta (`C + A`): Onde `C` é pai de uma entidade ascendente, no caso a entidade `A`.

# <a name="entity-info" />Informações de uma ocorrência

Uma entidade, ou melhor, cada ocorrência de uma entidade na expressão, contém informações que são de extrema importância, veremos isso no tópico [Pesquisas em expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search).

São elas:

* [Níveis](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#levels)
* [Índices](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#indexes)
* Entidades vizinhas:
  * [Navegação para a esquerda (Entidade anterior)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-previous)
  * [Navegação para a direita (Próxima entidade)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-next)

## <a name="levels" />Níveis

Uma expressão tem dois tipos de níveis: **Nível geral** e **Nível na expressão**.

O **nível geral** determina em qual nível a entidade está com relação a hierarquia do grafo. O nível inicia-se em `1` e é incrementado `+1` até chegar no último nível.

Por exemplo:

```
A (Level 1)
----B (Level 2)
    ----C (Level 3)
    ----D (Level 3)
        ----B (Level 4)
----E (Level 2)
    ----A (Level 3)
```

O **nível na expressão** determina em qual nível a entidade está com relação a expressão. O nível inicia-se em `1` e é incrementado `+1` até chegar no último nível.

Por exemplo:

```
                    A + B + C + ( D + E + ( F + G ) )
Nível na expressão: 1   1   1     2   2     3   3    
Nível geral:        1   2   2     2   3     3   4   
```

Note que o _nível da expressão_ é bem similar ao _nível geral_. A única diferença está no valor da **entidade pai**, no nível geral esse número é sempre menor que o nível geral de seus filhos e no nível da expressão eles são iguais.

## <a name="indexes" />Índices

Uma expressão tem dois tipos de índices: **Índice na expressão** e **Índice do nível**.

O **Índice da expressão** determina em qual posição a entidade está com relação a expressão. O índice inicia-se em `0` e é incrementado `+1` até chegar na última entidade da expressão.

Por exemplo:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

O **Índice do nível** determina em qual posição a entidade está com relação ao seu nível. O índice inicia-se em `0` e é incrementado `+1` até chegar na última entidade do mesmo nível. Por exemplo:

```
                 A + B + C + ( D + E + ( F + G + Y) )
Nível geral:     1   2   2     2   3     3   4   4
Índice do nível: 0   0   1     2   0     1   0   1 
```

Por exemplo:

```
A (Level 0)
----B (Level 0)
----C (Level 1)
----D (Level 2)
    ----E (Level 0)
    ----F (Level 1)
        ----G (Level 0)
        ----Y (Level 1)
```

* A entidade `A` é a raiz da expressão e seu "índice no nível" será zero. Note que por ser a entidade raiz, ela não terá outras entidades em seu nível.
* A entidade `B` é a primeira do segundo nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` é a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` é a terceira do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` é a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` é a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` é a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.
* A entidade `Y` é a segunda do quarto nível e terá a posição 1. Ela é filha da entidade `F`.

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

A **normalização de tipo 2** tem o objetivo de organizar, quando possível, as [Entidade final](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-final) no começo do seu grupo de expressão para ajudar na visualização da expressão.

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

A **normalização de tipo 3** tem o objetivo de declarar o mais rápido possível todos os [grupos de expressões](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group). Essa tema também foi abordado no tópico [Declarações de entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-declaration).

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
* A entidade `B`, dentro do grupo `C`, e a entidade `G` que está solitária no final da expressão, se transformaram em [Entidade final](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-final) e devido a isso, podemos aplicar a [Normalização - tipo 2](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-2) para melhorar a visualização, veja:

```
A + G + (B + F) + (C + B + (G + F))
```

* Note que agora a entidade `G` que estava no final da expressão foi movido para o inicio. Sendo assim, devemos aplicar novamente a [Normalização - tipo 3](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-3):

```
A + (G + F) + (B + F) + (C + B + G)
```

Com isso concluímos a normalização e temos acima uma expressão muito mais legível.

# <a name="desnormalization" />Desnormalizando expressões

O objetivo da **desnormalização** é gerar uma nova expressão onde os [grupos de expressões](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group) sejam redeclarados toda vez que a sua entidade pai for utilizada.

Após a desnormalização será impossível voltar na expressão original, esse é um caminho sem volta.

Considere a seguinte expressão original:

```
A + (B + D) + (E + B)
```

* Note que a entidade `B` tem dois pais: `A` e `E`
* Após a desnormalização teremos a seguinte expressão:

```
A + (B + D) + (E + (B + D))
                    ^
```

* Após a desnormalização a entidade `B` teve seu grupo de expressão redeclarado por completo quando foi utilizada novamente como filha da entidade `D`.

Como dito, é impossível voltar na expressão original, pois não conseguimos distinguir quais grupos de expressões eram da expressão original. Sendo assim, não podemos dizer que uma _expressão original_ é igual a sua _expressão desnormalizada_.

Vejam um exemplo de como elas são diferentes:

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

A pesquisa em expressão de grafos pode ser dividida em dois tipos: **Pesquisa superficial** e **Pesquisa profunda**.

Nos próximos tópicos vamos abordar a diferença entre esses tipos de pesquisas, mas antes, será preciso entender o que é uma **matriz de informação** que é o tema comum entre os dois tipos de pesquisa.

### <a name="matrix-of-information" />Matriz de informação

Podemos representar uma expressão de grafos em uma matriz vertical com todas as informações de uma expressão.

Com a visão em forma de matriz conseguimos uma melhor visualização do grafo e entendemos melhor como funciona a pesquisa em grafos complexos usando o conceito de expressão de grafos.

Vejamos um exemplo:

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

**<a name="sample-matrix" />Matriz de informação:**

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

Perceba que a expressão mudou da _orientação horizontal_ para a _orientação vertical_ e todas as entidades foram empilhadas uma nas outras e respeitando a mesma ordem que elas tinha na expressão.

Inclusive, essa é uma regra importante: _Nunca alterar a ordem das linhas, isso altera completamente o grafo._

Os _elementos de soma_ e _parênteses_ foram removidos, eles não são necessários na matriz, pois somente com as informações de _índices_ e _níveis_, é possível identificar todos os _grupos de expressões_.

E é com base nessa matriz de informação e ao fato das entidades conhecerem os seus _vizinhos_, ou seja, aqueles que estão posicionados na sua esquerda ou na sua direita, independentemente do nível, que podemos criar meios de pesquisas e navegações.

## <a name="search-deep" />Pesquisa profunda

A **pesquisa profunda** tem o objetivo de retornar a maior quantidade possíveis de resultados e para isso ela considera todos os caminhos que uma entidade percorre em um grafo.

Para poder criar uma _pesquisa profunda_, precisamos utilizar uma **expressão desnormalizada**. Isso é necessário, porque apenas a expressão desnormalizada contém todos os caminhos que uma entidade possui no grafo uma vez que a versão original da expressão não repete os grupos de expressão (e nem deve).

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

* Foi aplicada a desnormalização e a entidade `C` teve seu grupo de expressão redeclarado dentro da entidade `G`.
* Após a desnormalização um novo caminho foi criado para a entidade `Y`:
  * Antes:
    * _Ocorrência 1_: A.C.Y
    * _Ocorrência 2_: A.D.F.G.Y
  * Depois:
    * _Ocorrência 1_: A.C.Y
    * **_Ocorrência 2_: A.D.F.G.C.Y**
    * _Ocorrência 3_: A.D.F.G.Y

**<a name="sample-matrix-desnormalizated" />Matriz desnormalizada:**

Veja como ficou a expressão desnormalizada em forma de matriz:

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

* Foi criado uma nova linha com relação a versão original: A linha `#10` contém o novo caminho.

## <a name="search-surface" />Pesquisa superficial

Na **Pesquisa superficial** não consideramos os caminhos que já foram declarados (ou percorridos), ou seja, não usamos a técnica da **desnormalização** para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas em alguns casos não terá a mesma precisão da _Pesquisa profunda_.

Por exemplo, se quisermos retornar todas as ocorrências da entidade `Y`, teríamos a seguinte diferença entre os tipos de pesquisas:

_Expressão de exemplo:_

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

## <a name="search-deep" />Pesquisas sem referência

A **pesquisa sem referência** busca encontrar entidades ou informações dentro da matriz de informação.

Nesse tipo de pesquisa não temos nenhuma entidade como referência e a busca será feita em todo a matriz de acordo com a necessidade.

Como existem infinitas opção de pesquisas dentro de um grafo, abordaremos apenas alguns exemplos de _pesquisa sem referência_.

### <a name="search-find-root" />Encontrando a entidade raiz da expressão

Para encontrar a **entidade raiz** da expressão, precisamos retornar a entidade que tem o **índice geral** igual `0`.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

Com base na expressão a seguir, podemos afirmar que a entidade `A` é a **entidade raiz** da expressão.

```
        A + B + C
Index:  0   1   2
```

### <a name="search-method-find-parents" />Encontrando as "entidades pais" de uma expressão

Para encontrar todas as **entidades pais** do grafo, devemos aplicar a seguinte técnica:

1. Recuperar as **entidades anteriores** de todas as entidades cujo o **índice do nível** seja igual a `0`.
2. Para cada linha encontrada, retornamos a sua **entidade anterior** que será sempre uma **entidade pai**.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar entidades duplicadas em caso de grupos de expressões que foram redeclarados e será necessário remover as duplicações.

Sendo assim, é recomendado o uso da **pesquisa superficial** para evitar um processamento desnecessário.

**Pesquisa profunda**

Usaremos nesse exemplo a [matriz desnormalizada](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix-desnormalizated) do tópico sobre [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep).

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

Com isso, após removermos as repetições (no caso, a entidade `C` que aparece nas linhas `#2` e `#09`), obtemos como resultado final as entidades `A`, `C`, `D`, `F` e `G` como sendo as únicas entidades com filhos na expressão.

**Pesquisa superficial**

A lógica será a mesma da **pesquisa profunda**, contudo não teremos as duplicações, pois na _pesquisa superficial_ não existem grupos de expressões repetidos.

## <a name="search-with-references" />Pesquisas com referência

A **pesquisa com referência** parte do princípio que a _entidade_ ou uma de suas _ocorrências_ já foi encontrada e com base nisso podemos tomar ações como: _verificações_, _navegações_ ou pesquisas em seus _ascendentes_ e _descendentes_.

Como existem infinitas opção de pesquisas usando uma entidade, abordaremos apenas alguns exemplos de _pesquisas com referências_.

### <a name="search-method-is-first-at-group-expression" />Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese)

Para descobrir se uma entidade é a primeira do seu grupo de expressão (primeira dentro do parênteses), verificamos se o seu **nível geral** é menor que o nível geral da **próxima entidade**, se for, essa entidade é a primeira de seu grupo de expressão.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

```
                A + B + ( C + Y ) + (D + C)
                          ^
Nível geral:    1   2     2   3      2   3
Index:          0   1     2   3      4   5
```

No exemplo acima, a entidade `C`, do índice `#02`, tem o nível geral igual á `2` e a sua próxima entidade `Y` tem o nível geral igual á `3`, sendo assim, ela é a primeira dentro de seu parênteses.

**Observação:**

Não confunda essa técnica como sendo a solução para verificar se uma entidade contém filhos. Veremos isso no tópico <error>The anchor 'search-deep-has-children' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>.

### <a name="search-method-is-last-at-group-expression" />Verificando se uma entidade é a última do grupo de expressão (última dentro dos parêntese)

Para descobrir se uma entidade é a última do seu grupo de expressão (última dentro do parênteses), verificamos se seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade é a última do seu grupo de expressão.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

```
                A + B + ( C + Y ) + (D + C) + U
                              ^
Nível geral:    1   2     2   3      2   3    2
Index:          0   1     2   3      4   5    6
```

No exemplo acima, a entidade `Y`, do índice `#03`, tem o nível geral igual á `3` e a sua próxima entidade `D` tem o nível geral igual á `4`, sendo assim, ela é a última dentro de seu parênteses.

* A entidade `U` do índice `#06` não tem uma próxima entidade, portanto ela é a última de seu grupo de expressão, embora ele esteja omitido por estarmos no **grupo de expressão raiz**.

### <a name="search-method-get-entity-previous" />Encontrando a entidade anterior

Para retornar a entidade anterior de uma determinada entidade, devemos subtrair o seu **índice geral** em `-1`.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

Usaremos nesse exemplo a [matriz desnormalizada](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix-desnormalizated) do tópico sobre [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep).

1. Para obter a entidade anterior da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`), e subtraímos `-1`. Com o resultado (`2`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `C`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
```

* Se o resultado for menor que zero, é porque estamos na **entidade raiz** e não existe entidade anterior.

### <a name="search-method-get-entity-next" />Encontrando a próxima entidade

Para retornar a próxima entidade de uma determinada entidade, devemos somar o seu **índice geral** em `+1`.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

Usaremos nesse exemplo a [matriz desnormalizada](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix-desnormalizated) do tópico sobre [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep).

1. Para obter a próxima entidade da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`) e somamos `+1`. Com o resultado (`4`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `D`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
```

* Se o resultado for maior que a quantidade máxima de itens na matriz é porque estamos na última entidade da expressão e não existe uma próxima entidade.

### <a name="search-method-get-occurrences" />Encontrando todas as ocorrências de uma entidade

Para encontrar todas as ocorrências de uma entidade, devemos percorrer toda a matriz partindo do índice `0` até última posição da matriz.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar uma quantidade maior de ocorrências. Isso ocorre por que nesse tipo de pesquisa os grupos de expressões são redeclarados.

Sendo assim, é recomendado o uso da **pesquisa profunda** caso a sua necessidade seja obter o maior número possível de caminhos.

**Pesquisa profunda**

Usaremos nesse exemplo a [matriz desnormalizada](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix-desnormalizated) do tópico sobre [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep).

1. Se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas:
  * `#03 (Y)`
  * `#10 (Y)`: Essa ocorrência é derivada da **desnormalização**.
  * `#11 (Y)`

**Pesquisa superficial**

A lógica será a mesma da **pesquisa profunda**, contudo não teremos as ocorrências decorrentes das redeclarações dos grupos de expressão.

Usaremos nesse exemplo a [matriz original](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix) do tópico sobre [Matriz de informação](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#matrix-of-information).

1. Se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas:
  * `#03 (Y)`
  * `#10 (Y)`
* Note que foi encontrado uma ocorrência a menos que na _pesquisa profunda_.

### <a name="search-find-descendants" />Encontrando todos os descendentes de uma entidade

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se o seu **nível geral** é menor que o nível geral da **próxima entidade**, se for, essa entidade é uma descendente da entidade corrente. Essa é a mesma técnica usada no tópico <error>The anchor 'search-deep-is-first-at-group-expression' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>.

Devemos continuar navegando para frente até quando a próxima entidade tiver o **nível geral** igual ou menor ao **nível geral** da entidade corrente ou se a expressão não tiver mais entidades.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, existem abordagens diferentes para cada uma delas. Além disso, devemos ter um tratamento especial para entidades que contenham uma ascendente da própria entidade, ou seja, um **caminho cíclico**.

**Entidade com caminho cíclico:**

Devemos ter alguns cuidados para encontrar os descendentes de entidades com caminhos cíclicos. Isso ocorre porque os grupos de expressão não podem ser redeclarados nessas situações.

Por exemplo, como podemos encontrar os descendentes da entidade `A` que está no índice `#05`?

```
                A + B + (C + Y) + (D + A + C)
                                       ^
General Level:  1   2    2   3     2   3   3
Index:          0   1    2   3     4   5   6
```

* A entidade `A` que está no índice `#05` não foi redeclarada para evitar um **caminho cíclico**.
* Note que a entidade `A` contém descendentes (é a entidade raiz), mas é impossível descobrir isso se analisarmos a sua ocorrência do índice `#05`.

A resposta seria:

* Encontrar todas as ocorrências da entidade `A`.
* Dentre as ocorrências encontradas, devemos encontrar e utilizar a primeira que tem descendentes e ignorar as demais.
  * _Ocorrência 1_:
    * `#00`: A entidade `A` tem o nível geral igual a `1`.
    * `#01`: **A entidade `B` é a próxima entidade depois de `A` e o seu nível geral é `2`, é descendente**.
    * Pronto! Encontramos a ocorrência que tem a declaração do grupo de expressão da entidade `A`.
  * _Ocorrência 2_:
    * `#05`: Não é preciso verificar a segunda ocorrência da entidade `A`, pois já encontramos a sua declaração.
* Retornar os descendentes da entidade `A` do índice `#00`:
  * `#00`: A entidade `A` tem o nível geral igual a `1`.
  * `#01`: **A entidade `B` é a próxima entidade depois de `A` e o seu nível geral é `2`, é descendente**.
  * `#02`: **A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é `2`, é descendente**.
  * `#03`: **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `3`, é descendente**.
  * `#04`: **A entidade `D` é a próxima entidade depois de `Y` e o seu nível geral é `2`, é descendente**.
  * `#05`: **A entidade `A` é a próxima entidade depois de `D` e o seu nível geral é `3`, é descendente**.
  * `#06`: **A entidade `C` é a próxima entidade depois de `A` e o seu nível geral é `3`, é descendente**.
  * A expressão terminou.
  * Foram encontradas as seguintes entidades: `A, B, C, Y, D, A, C`.
* Remover as ocorrências que estão duplicadas: `Y`
* Retornar o resultado: `A, B, C, Y, D, A`

**Pesquisa profunda**

Se uma entidade não tiver um **caminho cíclico**, podemos simplesmente continuar a pesquisa de descendentes da ocorrência corrente, pois é garantido que seu grupo de expressão foi redeclarado.

**Pesquisa superficial**

Na pesquisa superficial devemos ter alguns cuidados. Notem que na expressão abaixo chegamos em um cenário muito parecido com as **entidades com caminhos cíclicos**.

Por exemplo, como podemos retornar os descendentes da entidade `C` do índice `#02`?

```
                A + B + C + (D + A + (C + Y)) + Z
                        ^              
General Level:  1   2   2    2   3    3   4     2
Index:          0   1   2    3   4    5   6     7
```

* A entidade `C` que está no índice `#02` não foi redeclarada, pois estamos usando a pesquisa superficial.
* Essa expressão não esta **normalizada**, a entidade `C` deveria ter sido declarada o mais rápido possível, mas isso não ocorreu.
* A entidade `C` contém descendentes. Seu grupo de expressão é declarado no índice `#05`.

Nesse caso temos duas opções:

**Opção 1:**

Utilizar a mesma lógica que foi explicada para **entidades com caminhos cíclicos**. Com isso será avaliado todas as ocorrências da entidade `C` até encontrarmos a ocorrência que declara o seu grupo de expressão.

* Seria encontrado a ocorrência do índice `#05` e a ocorrência do índice `#02` seria descartada.
* Agora que achamos a ocorrência correta, devemos retornar os descendentes:
  * `#05`: A entidade `C` tem o nível geral igual a `3`.
  * `#06`:**A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `4`, é descendente**.
  * `#07`: A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral é `2`, ela não é descendente.
  * A expressão não terminou, mas foi interrompida depois do resultado negativo do índice `#07`.
  * Foram encontradas as seguintes entidades: `Y`.
* Remover as ocorrências que estão duplicadas, nesse caso não tivemos nenhuma.
* Retornar o resultado: `Y`

**Opção 2:**

A segunda opção pode apresentar uma melhor performance se a expressão nascer de forma normalizada, se isso estiver garantido, não precisamos executar o primeiro passo.

* Aplicar a [Normalização - tipo 3](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-3) para garantir que todas as entidades estão sendo declaradas logo na primeira utilização. Esse passo não é necessário se a expressão nascer normalizada.

```
                A + B + (C + Y) + (D + A + C) + Z
                         ^              
General Level:  1   2    2   3     2   3   3    2
Index:          0   1    2   3     4   5   6    7
```

* Localizar a primeira ocorrência da entidade `C`. Após a normalização, devemos encontrar a ocorrência que está no índice `#02`.
* Recuperar os descendentes da primeira ocorrência da entidade `C` do índice `#02`.
  * `#02`: A entidade `C` tem o nível geral igual a `2`.
  * `#03`: **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `3`, é descendente**.
  * `#04`: A entidade `D` é a próxima entidade depois de `Y` e o seu nível geral é `2`, ela não é descendente.
  * A expressão não terminou, mas foi interrompida depois do resultado negativo do índice `#04`.
  * Foram encontradas as seguintes entidades: `Y`.
* Remover as ocorrências que estão duplicadas, nesse caso não tivemos nenhuma.
* Retornar o resultado: `Y`

Por fim, é possível dizer que não precisamos atribuir um tratamento especial para **entidades com caminhos cíclicos** se estivemos usando uma _pesquisa superficial_. Vimos que a solução é a mesma nas duas situações.

Esse tema também foi abordado, de forma superficial, no tópico [Declarações de entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-declaration).

### <a name="search-method-get-entity-children" />Encontrando os filhos de uma entidade

Para iniciar esse tópico é preciso entender por completo o tópico [Encontrando todos os descendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-find-descendants).

A lógica é exatamente a mesma da pesquisa de descendentes, a única diferença é que o **nível geral** será limitado á: _[nível geral da entidade corrente] + 1_

Usaremos nesse exemplo a [matriz desnormalizada](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#sample-matrix-desnormalizated) do tópico sobre [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep).

Com base nessa matriz, se quisermos encontrar todas as filhas da entidade `D` da linha `#04`:

* A entidade `D` tem o nível geral igual a `2`.
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`**.
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`**.
* As próximas entidades depois de `F` são: `G`, `B`, `C`, `Y` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`**.

Acabou a expressão e no final teremos o resultado: `E, F, Z`

### <a name="search-method-get-entity-ascending" />Encontrando todos os ascendentes de uma entidade

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

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar uma quantidade maior de ocorrências. Isso ocorre por que nesse tipo de pesquisa os grupos de expressões são redeclarados.

Por exemplo, se quisermos pegar os ascendentes da entidade `C` considerando todas as suas ocorrências na matriz do primeiro exemplo, teremos:

**Ocorrência 1:**

* A entidade `C` da linha `#02` tem o nível geral igual a `2`.
* `#01`: A entidade `B` tem o nível geral igual a `2`, não é ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a `1`, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível `1` e não mais o nível `2`**.

Acabou a expressão e teremos as seguintes entidades ascendentes: `A`

**Ocorrência 2:**

* A entidade `C` da linha `#09` tem o nível geral igual a `5`.
* `#08`: A entidade `B` tem o nível geral igual a `5`, não é ascendente.
* `#07`: **A entidade `G` tem o nível geral igual a `4`, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível `4` e não mais o nível `5`**.
* `#06`: **A entidade `F` tem o nível geral igual a `3`, é menor que o nível geral da entidade `G`, portanto é uma ascendente. Agora o nível a ser considerado será o nível `3` e não mais o nível `4`**.
* `#05`: A entidade `E` tem o nível geral igual a `3`, não é uma ascendente.
* `#04`: **A entidade `D` tem o nível geral igual a `2`, é uma ascendente. Agora o nível a ser considerado será o nível `2` e não mais o nível `3`**.
* `#03`: A entidade `Y` tem o nível geral igual a `3`, não é uma ascendente.
* `#02`: A entidade `C` tem o nível geral igual a `2`, não é uma ascendente.
* `#01`: A entidade `B` tem o nível geral igual a `2`, não é uma ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a `1`, é uma ascendente. Agora o nível a ser considerado será o nível `1` e não mais o nível `2`**.

Acabou a expressão e no final teremos as seguintes entidades ascendentes: `G`, `F`, `D` e `A`.

### <a name="search-method-get-entity-parent" />Encontrando os pais de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisaríamos limitar o nível geral dos ascendentes á: _[nível geral da entidade corrente] - 1_; ou a primeira entidade com o nível geral menor que a entidade desejada.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar uma quantidade maior de ocorrências. Isso ocorre por que nesse tipo de pesquisa os grupos de expressões são redeclarados.

Como existem 3 ocorrências da entidade `Y`, teremos uma _entidade pai_ por ocorrência:

**Ocorrência 1:**

* A entidade `Y` da linha `#3` tem o nível geral igual a `3`.
* `#02`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a `2`, portanto, ela é pai da entidade `Y`**.

**Ocorrência 2:**

* A entidade `Y` da linha `#10` tem o nível geral igual a `6`.
* `#09`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a `5`, portanto, ela é pai da entidade `Y`**.

**Ocorrência 3:**

* A entidade `Y` da linha `#11` tem o nível geral igual a `4`.
* `#10`: A entidade `Y` tem o nível geral igual a `6`, não é uma ascendente.
* `#09`: A entidade `C` tem o nível geral igual a `5`, não é uma ascendente.
* `#08`: A entidade `B` tem o nível geral igual a `5`, não é uma ascendente.
* `#07`: A entidade `G` tem o nível geral igual a `4`, não é uma ascendente.
* `#06`: **A entidade `F` é a entidade anterior a `G` e tem o nível geral igual a `3`, portanto, ela é pai da entidade `Y`**.

# <a name="implementation" />Implementações

Esse tópico vai demostrar na prática alguns exemplos de implementações de alguns dos conceitos que estudamos.

* [Criando grafos com expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-graph)
* [Convertendo uma matriz de informação para expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-expression)
* [Criando uma matriz de informações a partir de um grafo](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#implementation-to-matrix)

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
* Nas propriedades `Previous`, `Next` e `Parent`, estamos implementando, respectivamente, as técnicas:
  * <error>The anchor 'search-deep-get-entity-previous' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>
  * <error>The anchor 'search-deep-get-entity-next' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>
  * <error>The anchor 'search-deep-get-entity-parent' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>

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
            var isFirstInParenthesis = next != null && item.Level < next.Level;
            var isLastInParenthesis = next == null || item.Level > next.Level;
            var isNotRoot = item.Index > 0;

            if (isNotRoot) output += " + ";

            if (isFirstInParenthesis)
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
                    parenthesisToClose.Pop();
                    output += ")";
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
    * <error>The anchor 'search-deep-is-root' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>
  * Se a entidade for a primeira do grupo de expressão, adiciona o caractere `(`
    * <error>The anchor 'search-deep-is-first-at-group-expression' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>
  * Se a entidade for a última do seu grupo de expressão (última dentro dos parênteses), então feche com o caractere `)`. Como diversos parênteses podem ter sido abertos nas iterações anteriores, então devemos calcular a quantidade de parênteses que precisam ser fechados e fecha-los. A variável `parenthesisToClose` contém a entidade que está sendo fechada, isso pode ser útil para alguma lógica.
    * <error>The anchor 'search-deep-has-last-at-group-expression' doesn't exist for language version pt-br: HtmlAgilityPack.HtmlNode</error>

Com esses treixos de códigos vimos como é simples iterar em uma expressão de grafos e entender seus momentos. Além de abrir caminhos para implementações mais completas como: **pesquisa em expressão de grafos.**

## <a name="implementation-to-matrix" />Criando uma matriz de informações a partir de um grafo

No exemplo anterior vimos como gerar uma expressão de grafos a partir de uma matriz de informação manual e que foi representada pela classe `Expression`.

Nesse exemplo, vamos abordar uma implementação que cria essa matriz de forma automática.

```csharp
public class Expression : List<EntityItem>
{
    public bool Deep { get; }

    public Expression(Entity root, bool deep = true)
    {
        Deep = deep;

        if (root != null)
            Build(root);
    }

    private void Build(Entity parent, int level = 1)
    {
        // only when is root entity
        if (Count == 0)
        {
            var rootItem = new EntityItem(this)
            {
                Entity = parent,
                Index = 0,
                IndexAtLevel = 0,
                LevelAtExpression = level,
                Level = level
            };

            Add(rootItem);
        }

        var indexLevel = 0;
        var parentItem = this.Last();

        level++;
        foreach (var child in parent.Children)
        {
            var previous = this.Last();
            var childItem = new EntityItem(this)
            {
                Entity = child,
                Index = Count,
                IndexAtLevel = indexLevel++,
                Level = level,
            };

            Add(childItem);

            // if:   IS 'deep' and the entity already declareted in expression, don't build the children of item.
            // else: if current entity exists in ancestors (to INFINITE LOOP), don't build the children of item.
            var continueBuild = true;
            if (Deep)
                continueBuild = !HasAncestorEqualsTo(childItem);
            else
                continueBuild = !IsEntityDeclared(childItem);

            if (continueBuild && child.Children.Count() > 0)
            {
                childItem.LevelAtExpression = parentItem.LevelAtExpression + 1;
                Build(child, level);
            }
            else
            {
                childItem.LevelAtExpression = parentItem.LevelAtExpression;
            }
        }
    }

    private bool HasAncestorEqualsTo(EntityItem entityItem)
    {
        var ancestor = entityItem.Parent;
        while (ancestor != null)
        {
            if (entityItem.Entity == ancestor.Entity)
                return true;

            ancestor = ancestor.Parent;
        }

        return false;
    }

    private bool IsEntityDeclared(EntityItem entityItem)
    {
        return this.Any(e => e != entityItem && e.Entity == entityItem.Entity);
    }

    public string ToMatrixAsString()
    {
        var s = "";
        s += "Index    | Name    | Level    | IndexAtLevel    | LevelAtExpression \r\n";

        foreach (var i in this)
        {
            s += $"{i.Index.ToString("00")}       ";
            s += $"| {i.Entity.Name}       ";
            s += $"| {i.Level.ToString("00")}       ";
            s += $"| {i.IndexAtLevel.ToString("00")}              ";
            s += $"| {i.LevelAtExpression.ToString("00")} \r\n";
        }
        return s;
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
        A = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
        var expression = new Expression(A, false);
        var matrix = expression.ToMatrixAsString();
    }
}
```

O método `ToMatrixAsString` será usado para verificarmos o resultado de nosso exemplo. E após o processamento do grafo da entidade `A`, teremos a seguinte matriz de informação:

```
Index    | Name    | Level    | IndexAtLevel    | LevelAtExpression 
00       | A       | 01       | 00              | 01 
01       | B       | 02       | 00              | 02 
02       | C       | 03       | 00              | 03 
03       | A       | 04       | 00              | 03 
04       | A       | 03       | 01              | 02 
05       | D       | 02       | 01              | 02 
06       | D       | 03       | 00              | 02 
07       | E       | 03       | 01              | 02 
08       | F       | 03       | 02              | 03 
09       | G       | 04       | 00              | 04 
10       | A       | 05       | 00              | 04 
11       | C       | 05       | 01              | 04 
12       | Y       | 04       | 01              | 03 
13       | Z       | 03       | 03              | 02 
14       | G       | 02       | 02              | 01
```

* A classe recebe em seu construtor a **entidade raiz**. A partir dessa instância, vamos navegar em seu grafo por completo.
* O parâmetro `Deep` determina se a varredura será profunda ou não e que foi explicado no tópico [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep)
* O primeiro `if` dentro da função `Build` verifica se é a entidade raiz, se for, devemos criar o primeiro item. Nesse ponto, as informações são fixas, uma vez que por ser a entidade raiz, serão os valores inicias.
* Na segunda parte da função, iniciamos a leitura dos filhos da entidade `parent`.
* Será incrementado `+1` no **nível geral** conforme se aprofunda na entidade. Esse valor é passado por parâmetro, pois ele transcende todo o grafo.
* Será incrementado `+1` no **índice do nível**. Esse valor está fechado apenas no escopo do `foreach`, ou seja, apenas para os filhos da entidade.
* Para cada interação, é verificado se a propriedade `Deep` é `true`, se for, devemos manter a navegação mesmo se entidade corrente já foi percorrida por completo em algum momento da expressão. Contudo, a única situação que limita a continuação é se a entidade corrente tiver relações com ela mesma em um de seus ascendentes. Se tiver, é interrompida a continuação.
* Se a propriedade `Deep` for `false`, então devemos apenas verificar se a entidade já foi percorrida em algum momento da expressão, se foi, então não continuamos.
* A propriedade `LevelAtExpression` (**nível da expressão**) é preenchida com o **nível de expressão** da entidade pai somando-se `+1` quando a entidade tiver filhos e não somando nada quando não tiver.

Com isso, concluímos os três principais exemplos do conceito e que podem ser base para implementações mais complexas como a **pesquisa em expressão de grafos**.