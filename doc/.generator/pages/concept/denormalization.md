## Desnormalização

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

