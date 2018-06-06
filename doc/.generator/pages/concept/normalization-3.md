## Normalização - tipo 3 <header-set anchor-name="normalization-3" />

A **normalização de tipo 3** tem o objetivo de declarar o mais rápido possível todos os **grupos de expressão**.

Exemplo:

```
A + B + (C + G + (B + F)) + (G + F)
    ^             ^    
             ^               ^
```

Note que as entidades `B` e `G` são utilizadas antes que seus grupos sejam declarados e após a normalização teremos:

```
A + (B + F) + (C + (G + F) + B) + G
```

* Após a normalização, os grupos das entidades `B` e `G` foram declarados no primeiro momento que foram utilizadas.
* A entidade `B`, dentro do grupo `C` e a entidade `G` que está solitária no final da expressão, se transformaram em entidades finais e devido a isso, podemos aplicar a <anchor-get name="normalization-2" /> para melhorar a visualização, veja:

```
A + G + (B + F) + (C + B + (G + F))
```

* Podemos aplicar novamente a <anchor-get name="normalization-3" /> para declarar o grupo de expressão da entidade `G` após a sua movimentação para o inicio da expressão:

```
A + (G + F) + (B + F) + (C + B + G)
```

Com isso concluímos a normalização e temos acima uma expressão muito mais legível.