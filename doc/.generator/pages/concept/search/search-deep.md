## Pesquisa profunda <header-set anchor-name="search-deep" />

A **pesquisa profunda** tem o objetivo de retornar a maior quantidade de resultados possíveis e para isso ela considera todos os caminhos que uma entidade percorre em um grafo.

Para poder criar uma pesquisa profunda, precisamos utilizar uma **expressão desnormalizada**. Isso é necessário, porque apenas a expressão desnormalizada contém todos os caminhos que uma entidade possui no grafo uma vez que a versão original da expressão não repete os grupos de expressão (e nem deve).

Vejamos a seguir o mesmo exemplo utilizado no tópico <anchor-get name="matrix-of-information" />, porém agora, a expressão foi desnormalizada:

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

**Matriz (Exemplo modelo):**

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