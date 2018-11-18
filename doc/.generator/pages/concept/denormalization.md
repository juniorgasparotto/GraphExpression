# Desnormalizando expressões <header-set anchor-name="desnormalization" />

O objetivo da **desnormalização** é gerar uma nova expressão onde os <anchor-get name="expression-group">grupos de expressões</anchor-get> sejam redeclarados toda vez que a sua entidade pai for utilizada. 

Após a desnormalização será impossível voltar na expressão original, esse é um caminho sem volta.

Considere a seguinte expressão original:

```
A + (B + D) + (E + B)
```

* Note que a entidade "B" tem dois pais: "A" e "E"
* Após a desnormalização teremos a seguinte expressão:

```
A + (B + D) + (E + (B + D))
                    ^
```

* Após a desnormalização a entidade "B" teve seu grupo de expressão redeclarado por completo quando foi utilizada novamente como filha da entidade "D".

Como dito, é impossível voltar na expressão original, pois não conseguimos distinguir quais grupos de expressões eram da expressão original. Sendo assim, não podemos dizer que uma _expressão original_ é igual a sua _expressão desnormalizada_. 

Vejam um exemplo de como elas são diferentes:

```
Original:       A + (B + D) + (E + B)
Final Graph:
                A
                ---B
                ------D
                ---E
                ------B
```

Se pegarmos a expressão desnormalizada e extrairmos o seu grafo, teremos um grafo diferente do grafo original:

```
Original:                       A + (B + D) + (E + (B + D))
After normalization of type 1:  A + (B + D + D) + (E + B)
Final Graph:
                                A
                                ---B
                                ------D
                                ------D
                                ---E
                                ------B
```

Portanto, não podemos considerar que uma expressão desnormalizada seja usada como uma expressão original, isso altera o grafo final. Além do mais, ela infringe a regra do tópico <anchor-get name="expression-group-repeat" />.

