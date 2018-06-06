## Índices <header-set anchor-name="indexes" />

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