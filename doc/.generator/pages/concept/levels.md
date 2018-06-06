## Níveis

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

