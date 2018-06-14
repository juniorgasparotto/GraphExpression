### Encontrando a entidade anterior <header-set anchor-name="search-find-previous" />

Para retornar a entidade anterior de uma determinada entidade, devemos subtrair o seu **índice geral** em `-1`.

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

Usaremos nesse exemplo a <anchor-get name="sample-matrix-desnormalizated">matriz desnormalizada</anchor-get> do tópico sobre <anchor-get name="search-deep" />.

1. Para obter a entidade anterior da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`), e subtraímos `-1`. Com o resultado (`2`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `C`.

```
Index   | Entity | Level | Level Index
#02     | C      | 2     | 1 
#03     | Y      | 3     | 0 
```

* Se o resultado for menor que zero, é porque estamos na **entidade raiz** e não existe entidade anterior. 