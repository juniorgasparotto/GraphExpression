# Normalização - tipo 2 <header-set anchor-name="normalization-2" />

A **normalização de tipo 2** tem o objetivo de organizar, quando possível, as **entidades finais** no começo do seu grupo de expressão para ajudar na visualização da expressão.

```
A + (B + (C + D) + E) + F + G
                   ^    ^   ^
```

Após a normalização ficaria assim:

```
A + F + G + (B + E + (C + D))
    ^   ^        ^    
```

* Note que as entidades `F` e `G` foram para o ínicio do seu grupo de expressão.
* A entidade `E` também foi reorganizada para o ínicio do seu grupo de expressão.

