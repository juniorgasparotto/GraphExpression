## Normalização - tipo 3 <header-set anchor-name="normalization-3" />

A **normalização de tipo 3** tem o objetivo de declarar o mais rápido possível todos os <anchor-get name="expression-group">grupos de expressões</anchor-get>. Essa tema também foi abordado no tópico <anchor-get name="entity-declaration" />.

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
* A entidade `B`, dentro do grupo `C`, e a entidade `G` que está solitária no final da expressão, se transformaram em <anchor-get name="entity-final" /> e devido a isso, podemos aplicar a <anchor-get name="normalization-2" /> para melhorar a visualização, veja:

```
A + G + (B + F) + (C + B + (G + F))
```

* Note que agora a entidade `G` que estava no final da expressão foi movido para o início. Sendo assim, devemos aplicar novamente a <anchor-get name="normalization-3" />:

```
A + (G + F) + (B + F) + (C + B + G)
```

Com isso concluímos a normalização e temos acima uma expressão muito mais legível.