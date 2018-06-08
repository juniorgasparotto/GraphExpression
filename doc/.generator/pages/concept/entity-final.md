## Entidade final <header-set anchor-name="entity-final" />

Uma entidade que não possui grupos de expressão em seu nível é chamada de **entidade final**. Isso não significa que a entidade não tenha filhos, veja:

**Entidade final sem filhos:**

```
(A + B + C + (D + E))
```

* As entidades `B`, `C` e `E` são entidades finais.

**Entidade final com filhos:**

```
(A + (B + C) + (D + B))
```

* A entidade `C` é final e não contém filhos
* A entidade `B`, do grupo de expressão da entidade `D`, também é final, mas ela contém filhos.