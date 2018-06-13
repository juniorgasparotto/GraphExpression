### Retornando a entidade anterior <header-set anchor-name="search-method-get-entity-previous" />

Para retornar a entidade anterior de uma determinada entidade, devemos subtrair o **índice geral** em `-1`.

Por exemplo:

Com base no exemplo modelo, para obter a entidade anterior da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`), e subtraímos `-1`. Com o resultado (`2`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `C`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
```

* Se o resultado for menor que zero, é porque estamos na **entidade raiz** e não existe entidade anterior. 