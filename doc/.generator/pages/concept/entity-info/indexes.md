## Índices <header-set anchor-name="indexes" />

Uma expressão tem dois tipos de índices: **Índice na expressão** e **Índice do nível**.

O **Índice da expressão** determina em qual posição a entidade está com relação a expressão. O índice inicia-se em `0` e é incrementado `+1` até chegar na última entidade da expressão.

Por exemplo:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

O **Índice do nível** determina em qual posição a entidade está com relação ao seu nível. O índice inicia-se em `0` e é incrementado `+1` até chegar na última entidade do mesmo nível.

Por exemplo:

```        
                A + B + C + ( D + E + ( F + G + Y ) )
Level:          1   2   2     2   3     3   4   4
Level Index:    0   0   1     2   0     1   0   1

Graph:

A (Level Index: 0)
----B (Level Index: 0)
----C (Level Index: 1)
----D (Level Index: 2)
    ----E (Level Index: 0)
    ----F (Level Index: 1)
        ----G (Level Index: 0)
        ----Y (Level Index: 1)
```

* A entidade `A` é a raiz da expressão e seu "índice no nível" será zero. Note que por ser a entidade raiz, ela não terá outras entidades em seu nível.
* A entidade `B` é a primeira do segundo nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` é a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` é a terceira do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` é a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` é a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` é a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.
* A entidade `Y` é a segunda do quarto nível e terá a posição 1. Ela é filha da entidade `F`.