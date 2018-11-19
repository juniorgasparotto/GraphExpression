## Pesquisa profunda <header-set anchor-name="search-deep" />

A **pesquisa profunda** tem o objetivo de retornar a maior quantidade possíveis de resultados e para isso ela considera todos os caminhos que uma entidade percorre em um grafo.

Para poder criar uma _pesquisa profunda_, precisamos utilizar uma **expressão desnormalizada**. Isso é necessário, porque apenas a expressão desnormalizada contém todos os caminhos que uma entidade possui no grafo uma vez que a versão original da expressão não repete os grupos de expressão (e nem deve).

Vejamos a seguir o mesmo exemplo utilizado no tópico <anchor-get name="search-matrix-of-information" />, porém agora, a expressão foi desnormalizada:

**Expressão:**

```
Original:       A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
                          ^                                   ^
Denormalized:   A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
                                                                    ^
Level:          1   2     2   3       2   3     3     4   5     5   6       4     3 
Level Index:    0   0     1   0       2   0     1     0   0     1   0       1     2
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
                ----Y (Level Index: 0) *
        ----Y (Level Index: 1)
    ----Z (Level Index: 2)
```

* Foi aplicada a desnormalização e a entidade "C" teve seu grupo de expressão redeclarado dentro da entidade "G".
* Após a desnormalização um novo caminho foi criado para a entidade "Y":
    * Antes:
        * _Primeira ocorrência_: `A.C.Y`
        * _Segunda ocorrência_: `A.D.F.G.Y`
    * Depois:
        * _Primeira ocorrência_: `A.C.Y`
        * **_Segunda ocorrência_**: `A.D.F.G.C.Y`
        * _Terceira ocorrência_: `A.D.F.G.Y`

**<anchor-set name="sample-matrix-desnormalizated">Matriz desnormalizada</anchor-set>:**

Veja como ficou a expressão desnormalizada em forma de matriz:

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
#10     | Y *    | 6     | 0
#11     | Y      | 4     | 1 
#12     | Z      | 3     | 2 
```

* Foi criado uma nova linha com relação a versão original: A linha "#10" contém o novo caminho.