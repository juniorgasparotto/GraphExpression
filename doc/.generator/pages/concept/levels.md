## Níveis <header-set anchor-name="levels" />

Uma expressão tem dois tipos de níveis: **Nível geral** e **Nível na expressão**.

O **nível geral** determina em qual nível a entidade está com relação a hierarquia do grafo. O nível inicia-se em `1` e é incrementado `+1` até chegar no último nível.

Por exemplo:

```
A (Level: 1)
----B (Level: 2)
    ----C (Level: 3)
    ----D (Level: 3)
        ----B (Level: 4)
----E (Level: 2)
    ----A (Level: 3)
```

O **nível na expressão** determina em qual nível a entidade está com relação a expressão. O nível inicia-se em `1` e é incrementado `+1` até chegar no último nível.

Por exemplo:

```
                        A + B + C + ( D + E + ( F + G ) )
Level in expression:    1   1   1     2   2     3   3    
General Level:          1   2   2     2   3     3   4   
```

Note que o _nível da expressão_ é bem similar ao _nível geral_. A única diferença está no valor da **entidade pai**, no nível geral esse número é sempre menor que o nível geral de seus filhos e no nível da expressão eles são iguais.

