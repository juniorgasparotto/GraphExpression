## Pesquisas em expressões de grafos

A pesquisa em expressão de grafos pode ser dividida em duas partes: "Pesquisa superficial" e "Pesquisa profunda".

Ambas utilizam de uma matriz de informação que veremos adiante para poder obter ensumos para a pesquisa.

### Matriz de informação

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