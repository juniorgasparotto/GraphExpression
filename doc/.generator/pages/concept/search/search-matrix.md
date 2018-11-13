### Matriz de informação <header-set anchor-name="search-matrix-of-information" />

Podemos representar uma expressão de grafo em uma matriz vertical com todas as informações de uma expressão. 

Com a visão em forma de matriz conseguimos uma melhor visualização do grafo e entendemos melhor como funciona a pesquisa em grafos complexos usando o conceito de expressão de grafo. 

Vejamos um exemplo:

**Expressão:**

```
Expression:     A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
Level:          1   2     2   3       2   3     3     4   5   5     4     3
Level Index:    0   0     1   0       2   0     1     0   0   1     1     2
```

**Hierarquia:**

```
A (Level Index: 0)
----B (Level Index: 0)
----C (Level Index: 1)
    ----Y (Level Index: 0)
----D (Level Index: 2)
    ----E (Level Index: 0)
    ----F (Level Index: 1)
        ----G (Level Index: 0)
            ----B (Level Index: 0)
            ----C (Level Index: 1)
        ----Y (Level Index: 1)
    ----Z (Level Index: 2)
```

**<anchor-set name="sample-matrix">Matriz de informação</anchor-set>:**

```
Index   | Entity | Level | Level Index
#00     | A      | 1     | 0
#01     | B      | 2     | 0
#02     | C      | 2     | 1
#03     | Y      | 3     | 0
#04     | D      | 2     | 2
#05     | E      | 3     | 0
#06     | F      | 3     | 1
#07     | G      | 4     | 0
#08     | B      | 5     | 0
#09     | C      | 5     | 1
#10     | Y      | 4     | 1
#11     | Z      | 3     | 2
```

Perceba que a expressão mudou da _orientação horizontal_ para a _orientação vertical_ e todas as entidades foram empilhadas uma nas outras e respeitando a mesma ordem que elas tinha na expressão. 

Inclusive, essa é uma regra importante: _Nunca alterar a ordem das linhas, isso altera completamente o grafo._

Os _elementos de soma_ e _parênteses_ foram removidos, eles não são necessários na matriz, pois somente com as informações de _índices_ e _níveis_, é possível identificar todos os _grupos de expressões_.

E é com base nessa matriz de informação e ao fato das entidades conhecerem os seus _vizinhos_, ou seja, aqueles que estão posicionados na sua esquerda ou na sua direita, independentemente do nível, que podemos criar meios de pesquisas e navegações.