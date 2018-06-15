### Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese) <header-set anchor-name="search-check-is-first-at-group-expression" />

Para descobrir se uma entidade é a primeira do seu grupo de expressão (primeira dentro do parênteses), verificamos se o seu **nível geral** é menor que o nível geral da **próxima entidade**, se for, essa entidade é a primeira de seu grupo de expressão.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

```
        A + B + ( C + Y ) + (D + C)
                  ^
Level:  1   2     2   3      2   3
Index:  0   1     2   3      4   5
```

No exemplo acima, a entidade `C`, do índice `#02`, tem o nível geral igual á `2` e a sua próxima entidade `Y` tem o nível geral igual á `3`, sendo assim, ela é a primeira dentro de seu parênteses.

**Observação:**

Não confunda essa técnica como sendo a solução para verificar se uma entidade contém filhos. Veremos isso no tópico <anchor-get name="search-find-descendants" />.