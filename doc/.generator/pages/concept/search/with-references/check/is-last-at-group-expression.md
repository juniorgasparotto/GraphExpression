### Verificando se uma entidade é a última do grupo de expressão (última dentro dos parêntese) <header-set anchor-name="search-method-is-last-at-group-expression" />

Para descobrir se uma entidade é a última do seu grupo de expressão (última dentro do parênteses), verificamos se seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade é a última do seu grupo de expressão.

```
                A + B + ( C + Y ) + (D + C) + U
                              ^
Nível geral:    1   2     2   3      2   3    2
Index:          0   1     2   3      4   5    6
```

No exemplo acima, a entidade `Y`, do índice `#03`, tem o nível geral igual á `3` e a sua próxima entidade `D` tem o nível geral igual á `4`, sendo assim, ela é a última dentro de seu parênteses.

* A entidade `U` do índice `#06` não tem uma próxima entidade, portanto ela é a última de seu grupo de expressão, embora ele esteja omitido por estarmos no **grupo de expressão raiz**.