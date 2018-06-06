[
![Inglês](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/documentation/img/en-us.png)
](https://github.com/juniorgasparotto/ExpressionGraph)
[
![Português](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/documentation/img/pt-br.png)
](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md)
* [Expressão de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#concept)
  * [Grupos de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group)
    * [Grupo de expressão raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-root)
    * [Sub-grupos de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-sub-group)
    * [Repetições de grupo de expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#expression-group-repeat)
    * [Entidade Raiz](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-root)
    * [Entidade Pai](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-parent)
    * [Entidade final](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-final)
  * [Caminhos de entidades](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths)
    * [Caminhos cíclicos na expressão](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#paths-cyclic)
  * [Níveis](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#levels)
  * [Índices](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#indexes)
  * [Navegação para a direita (Próxima entidade)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-next)
  * [Navegação para a esquerda (Entidade anterior)](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#entity-previous)
  * [Normalização - tipo 1](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-1)
  * [Normalização - tipo 2](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-2)
  * [Normalização - tipo 3](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#normalization-3)
  * [Desnormalização](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#desnormalization)
  * [Pesquisas em expressões de grafos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search)
    * [Matriz de informação](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#matrix-of-information)
    * [Pesquisa profunda](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep)
      * [Pesquisando todas as ocorrências de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-occurrences)
      * [Pesquisando todas as entidades que contenham filhos](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-with-children)
      * [Pesquisando todos os descendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-descendants)
      * [Pesquisando os filhos de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-children)
      * [Pesquisando todos os ascendentes de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-ascending)
      * [Pesquisando o pai de uma entidade](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-deep-get-entity-parent)
    * [Pesquisa superficial](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md#search-surface)

# <a name="concept" />Expressão de grafos

O conceito expressão de grafos foi criado em 2015 por Glauber Donizeti Gasparotto Junior e tem como objetivos a representação de um grafo em forma de expressão.

A ideia de uma representação em forma de expressão é resumir um grafo em um texto que seja humanamente legível e de fácil transporte.

Com o avanço do entendimento do conceito, você vai notar que ele pode ser útil para a criação de mecanismos de pesquisas complexas. Vale ressaltar que o foco não é performance, mas apenas uma nova forma de enxergar um grafo e suas informações.

Imagine o seguinte grafo:

```
A 
----B
    ----C
    ----D
        ----B
----E
    ----A
```

A sua representação em forma de expressão seria:

```
(A + (B + C + (D + B)) + (E + A))
```

Note que essa representação se parece com uma expressão matemática, porém a resolução da expressão é bem peculiar.

## Elementos de uma expressão de grafos

Primeiro, vamos listar os elementos de uma expressão:

* **Entidade:** É o elemento fundamental da expressão, determina uma unidade, um vértice no teória de grafo.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade, uma aresta em teória de grafos.
* **Parenteses `(` e `)`**: São usados para determinar um grupo de entidades filhas de uma determina entidade.

## Objetivo do resultado final

O objetivo de uma expressão de grafos é bem diferente do objetivo de uma expressão matemática. Na matemática, uma expressão nesses moldes teria números como sendo as entidades e após o processamento da expressão o resultado final seria outro número com a soma de todos os outros.

Em expressão de grafos, o objetivo final é gerar um grafo completo a partir de uma expressão, ou a partir de um grafo, fazer a engenharia reversa para obter sua representação em forma de expressão.

### Ordem de resolução

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona a entidade da direita e o resultado dessa soma é a propria entidade da esquerda e assim sucessivamente até chegar no final.

**Exemplo simples (Etapas simbólicas da resolução):**

1. `(A + B)`
2. Resultado final da expressão: `A`

_Composição final da entidade `A`_

```
A 
----B
```

**Exemplo composto (Etapas simbólicas da resolução):**

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Resultado final da expressão: `A`

_Composição final da entidade `A`_

```
A 
----B
----C
----D
```

**Conclusão**

Vimos que a cada etapa da resolução de uma expressão a entidade da direita desaparece e a entidade da esquerda prevalece até não restarem entidades a sua direita. É obvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita, mas o que importa aqui é entender a ordem de resolução e o objetivo final do resultado.

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

O primeiro grupo de expressão é chamado de "grupo de expressão raiz".

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

### <a name="entity-root" />Entidade Raiz

A primeira entidade da expressão é a entidade raiz da expressão. Uma expressão só pode conter uma entidade raiz.

```
A + B + (C + A)
```

* A entidade `A` é a entidade raiz de toda expressão acima e será o topo do grafo.

### <a name="entity-parent" />Entidade Pai

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo.

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.

### <a name="entity-final" />Entidade final

Uma entidade que não possui grupos de expressão em seu nível é chamada de "entidade final". Isso não significa que a entidade não tenha filhos, veja:

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

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome, isso não faz sentido. Mas a mesma entidade pode aparecer em diversos pontos no grafo.

E para representar cada ocorrência usamos a notação de "caminhos de entidades" para determinar o caminho de inicio e fim até chegar na ocorrência da entidade. Abaixo temos um caminho de início e fim até chegar na entidade `D`.

```
A.B.C.D
```

Essa notação é o mesmo que:

```
A + (B + (C + D))
```

* A entidede `D` é filha da entidade `C`
* A entidede `C` é filha da entidade `B`
* A entidede `B` é filha da entidade `A`

O caractere `.` é usado entre a entidade pai e a entidade filha. A entidade da esquerda será a pai e a entidade da direita será a filha. Vejamos mais exemplos:

**Expressão:**

```
(A + A + (B + C) + (D + B))
```

**Caminho da entidade `A`:**

Ocorrência 1: `A` Ocorrência 2: `A.A`

Na "ocorrência 2" temos uma relação ciclica, portanto a notação é interrompida quando isso acontece, do contrário teriamos um caminho infinito.

**Caminho da entidade `B`:**

Ocorrência 1: `A.B` Ocorrência 2: `A.D.B`

### <a name="paths-cyclic" />Caminhos cíclicos na expressão

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe uma soma ciclica entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação ciclica.

Note que o grafo contém duas referências ciclicas:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade `A` é pai dela mesma.
* Uma indireta (`C + A`): Onde `C` é pai de uma entidade ascendente, no caso a entidade `A`.

## <a name="levels" />Níveis

Uma expressão tem dois tipos de níveis: "Nível geral" e "Nível na expressão".

O "nível geral" determina em qual nível a entidade está com relação a hierarquia do grafo.

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

O "nível na expressão" determina em qual nível a entidade está com relação a expressão.

Por exemplo:

```
                    A + B + C + ( D + E + ( F + G ) )
Nível na expressão: 1   1   1     2   2     3   3    
Nível geral:        1   2   2     2   3     3   4   
```

Note que o nível da expressão ignora o nível da entidade na hieraquia, é uma informação útil apenas para a expressão.

## <a name="indexes" />Índices

Uma expressão tem dois tipos de índices: "Índice na expressão" e "Índice do nível".

O "Índice da expressão" determina em qual posição a entidade está na expressão. O índice inicia em zero e soma-se +1 até a última entidade da expressão.

Por exemplo:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

O "Índice do nível" determina em qual posição a entidade está com relação ao seu nível. O índice inicia em zero na primeira entidade do nível e soma-se +1 até a última entidade do mesmo nível. Por exemplo:

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

## <a name="normalization-1" />Normalização - tipo 1

A normalização tem o objetivo de enxugar grupos de expressão que pertencem a mesma entidade pai e que estão em diferentes lugares na expressão.

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

A normalização de tipo 2 tem o objetivo de organizar, quando possível, as "entidades finais" no começo do seu grupo de expressão para ajudar na visualização da expressão.

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

A normalização de tipo 3 tem o objetivo de declarar o mais rápido possível todos os "grupos de expressão".

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

1. Após a normalização, os grupos das entidades `B` e `G` foram declarados no primeiro momento que foram utilizadas.
2. A entidade `B`, dentro do grupo `C` e a entidade `G` que está solitária no final da expressão, se transformaram em entidades finais e devido a isso, podemos aplicar a "normalização de tipo 2" para melhorar a visualização, veja:

```
A + G + (B + F) + (C + B + (G + F))
```

1. Podemos aplicar novamente a normalização de tipo 3 para declarar o grupo de expressão da entidade `G` após a sua movimentação para o inicio da expressão:

```
A + (G + F) + (B + F) + (C + B + G)
```

Com isso concluímos a normalização e temos acima uma expressão muito mais legível.

## <a name="desnormalization" />Desnormalização

O objetivo da desnormalização é gerar uma nova expressão onde os grupos de expressões sejam escritos toda vez que a sua entidade pai for utilizada. Após a desnormalização será impossível voltar na expressão original, esse é um caminho sem volta.

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

Como dito, é impossível voltar na expressão original, pois não conseguimos distinguir quais grupos de expressões eram da expressão original. Sendo assim, não podemos dizer que uma expressão original é igual a sua expressão desnormalizada. Vejam um exemplo de como elas são diferentes:

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

Portanto, não podemos considerar que uma expressão desnormalizada seja usada como uma expressão original, isso altera o grafo final. Além do mais, ela infringe a regra do tópico "Repetições de grupo de expressão".

## <a name="search" />Pesquisas em expressões de grafos

A pesquisa em expressão de grafos pode ser dividida em duas partes: "Pesquisa superficial" e "Pesquisa profunda".

Ambas utilizam de uma matriz de informação que veremos adiante para poder obter ensumos para a pesquisa.

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
#01             | A        | 1           | 0 
#02             | B        | 2           | 0 
#03             | C        | 2           | 1 
#04             | Y        | 3           | 0 
#05             | D        | 2           | 2 
#06             | E        | 3           | 0 
#07             | F        | 3           | 1 
#08             | G        | 4           | 0 
#09             | B        | 5           | 0 
#10             | C        | 5           | 1 
#11             | Y        | 4           | 1 
#12             | Z        | 3           | 2
```

Com base nessa matriz de informação e ao fato das entidades conhecerem os seus "vizinhos", ou seja, aquelas que estão posicionadas na sua esquerda ou na sua direita na expressão (independentemente do nível) podemos criar meios de navegação e pesquisa de entidades.

### <a name="search-deep" />Pesquisa profunda

A pesquisa profunda tem o objetivo de retornar a maior quantidade de resultados possíveis e para isso ela considera todos os caminhos que uma entidade percorre em um grafo.

Para poder criar uma pesquisa profunda, precisamos utilizar uma **expressão desnormalizada**. Isso é necessário, porque apenas a expressão desnormalizada contém todos os caminhos que uma entidade possui no grafo uma vez que a versão original da expressão não repete os grupos de expressão (e nem deve).

Vejamos a seguir o mesmo exemplo utilizado no tópico "Matriz de informação", porém agora, a expressão foi desnormalizada:

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
    * Ocorrência 1: A.C.Y
    * Ocorrência 2: A.D.F.G.Y
  * Depois:
    * Ocorrência 1: A.C.Y
    * Ocorrência 2: A.D.F.G.C.Y -> Novo caminho
    * Ocorrência 3: A.D.F.G.Y

**Matriz:**

```
Índice geral    | Entidade | Nível geral | Índice do nível
#01             | A        | 1           | 0 
#02             | B        | 2           | 0 
#03             | C        | 2           | 1 
#04             | Y        | 3           | 0 
#05             | D        | 2           | 2 
#06             | E        | 3           | 0 
#07             | F        | 3           | 1 
#08             | G        | 4           | 0 
#09             | B        | 5           | 0 
#10             | C        | 5           | 1 
#11             | Y *      | 6           | 0
#12             | Y        | 4           | 1 
#13             | Z        | 3           | 2 
```

#### <a name="search-deep-occurrences" />Pesquisando todas as ocorrências de uma entidade

Uma entidade pode ter mais de uma ocorrência em um grafo, no exemplo acima, se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontrariamos as linhas `#4`, `#11` e `#12`.

* Note que sem a desnormalização não seria possível encontrar a linha `#11` e não seria possível obter o número correto de ocorrências dessa entidade.

#### <a name="search-deep-with-children" />Pesquisando todas as entidades que contenham filhos

Para isso, basta recuperar as **entidades anteriores** de todas as entidades cujo o **índice do nível** seja igual a `0`.

Com base no exemplo, teremos:

1. Primeiro, encontramos todas as linhas com o índice do nível igual a zero:
  * `#01 (A)`
  * `#02 (B)`
  * `#04 (Y)`
  * `#06 (E)`
  * `#08 (G)`
  * `#09 (B)`
  * `#11 (Y)`
2. Para cada linha encontrada, retornamos a sua entidade anterior que será uma entidade pai:
  * `NULL` -> `#01 (A)`: Não contém entidade anterior, portanto não retorna nada.
  * `#01 (A)` -> `#02 (B)`: Retorna a entidade `A` como sendo sua anterior
  * `#03 (C)` -> `#04 (Y)`: Retorna a entidade `C` como sendo sua anterior
  * `#05 (D)` -> `#06 (E)`: Retorna a entidade `D` como sendo sua anterior
  * `#07 (F)` -> `#08 (G)`: Retorna a entidade `F` como sendo sua anterior
  * `#08 (G)` -> `#09 (B)`: Retorna a entidade `G` como sendo sua anterior
  * `#10 (C)` -> `#11 (Y)`: Retorna a entidade `C` como sendo sua anterior

Com isso, após removermos as repetições de entidades (no caso a entidade `C` que aparece nas linhas `#3` e `#10`), obtemos como resultado final as entidades `A`, `C`, `D`, `F` e `G` como sendo as únicas entidades com filhos na expressão.

#### <a name="search-deep-descendants" />Pesquisando todos os descendentes de uma entidade

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

#### <a name="search-deep-get-entity-children" />Pesquisando os filhos de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas os filhos da entidade `D`, precisariamos limitar o nível geral dos descendentes á: [nível geral da entidade corrente] + 1

* A entidade `D` tem o nível geral igual a 2
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`.**
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`.**
* As próximas entidades depois de `F` são: `G`, `B`, `C`, `Y` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`.**

Acabou a expressão e no final teremos as seguintes entidades descendentes: `E`, `F` e `Z`

#### <a name="search-deep-get-entity-ascending" />Pesquisando todos os ascendentes de uma entidade

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

* A entidade `C` da linha `#3` tem o nível geral igual a 2
* `#02`: A entidade `B` tem o nível geral igual a 2, não é ascendente.
* `#01`: **A entidade `A` tem o nível geral igual a 1, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível 1 e não mais o nível 2.**

Acabou a expressão e teremos as seguintes entidades ascendentes: `A`

**Ocorrência 2:**

* A entidade `C` da linha `#10` tem o nível geral igual a 5
* `#09`: A entidade `B` tem o nível geral igual a 5, não é ascendente.
* `#08`: **A entidade `G` tem o nível geral igual a 4, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível 4 e não mais o nível 5.**
* `#07`: **A entidade `F` tem o nível geral igual a 3, é menor que o nível geral da entidade `G`, portanto é uma ascendente. Agora o nível a ser considerado será o nível 3 e não mais o nível 4.**
* `#06`: A entidade `E` tem o nível geral igual a 3, não é uma ascendente.
* `#05`: **A entidade `D` tem o nível geral igual a 2, é uma ascendente. Agora o nível a ser considerado será o nível 2 e não mais o nível 3.**
* `#04`: A entidade `Y` tem o nível geral igual a 3, não é uma ascendente.
* `#03`: A entidade `C` tem o nível geral igual a 2, não é uma ascendente.
* `#02`: A entidade `B` tem o nível geral igual a 2, não é uma ascendente.
* `#01`: **A entidade `A` tem o nível geral igual a 1, é uma ascendente. Agora o nível a ser considerado será o nível 1 e não mais o nível 2.**

Acabou a expressão e no final teremos as seguintes entidades ascendentes: `G`, `F`, `D` e `A`

#### <a name="search-deep-get-entity-parent" />Pesquisando o pai de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisariamos limitar o nível geral dos ascendentes á: [nível geral da entidade corrente] - 1; ou a primeira entidade com o nível geral menor que a entidade desejada.

Como existem 3 ocorrências da entidade `Y`, teremos um resultado por ocorrência:

**Ocorrência 1:**

* A entidade `Y` da linha `#4` tem o nível geral igual a 3
* `#3`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a 2, portanto, ela é pai da entidade `Y`.**

**Ocorrência 2:**

* A entidade `Y` da linha `#11` tem o nível geral igual a 6
* `#10`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a 5, portanto, ela é pai da entidade `Y`.**

**Ocorrência 3:**

* A entidade `Y` da linha `#12` tem o nível geral igual a 4
* `#11`: A entidade `Y` tem o nível geral igual a 6, não é uma ascendente.
* `#10`: A entidade `C` tem o nível geral igual a 5, não é uma ascendente.
* `#09`: A entidade `B` tem o nível geral igual a 5, não é uma ascendente.
* `#08`: A entidade `G` tem o nível geral igual a 4, não é uma ascendente.
* `#07`: **A entidade `F` é a entidade anterior a `G` e tem o nível geral igual a 3, portanto, ela é pai da entidade `Y`.**

### <a name="search-surface" />Pesquisa superficial

Na "Pesquisa superficial" a técnica usada é a mesma da "Pesquisa profunda", á única deferença é que na pesquisa superficial não consideramos os caminhos que já foram escritos (ou percorridos). No caso, não usamos a técnica da desnormalização para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas não terá a mesma precisão da "Pesquisa profunda".

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

* Ocorrência 1: A.C.Y
* Ocorrência 2: A.D.F.G.C.Y -> Novo caminho
* Ocorrência 3: A.D.F.G.Y

**Pesquisa superficial:**

Utiliza a expressão original:

* Ocorrência 1: A.C.Y
* Ocorrência 2: A.D.F.G.Y