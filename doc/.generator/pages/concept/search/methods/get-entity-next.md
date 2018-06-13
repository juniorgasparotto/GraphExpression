### Retornando a próxima entidade <header-set anchor-name="search-method-get-entity-next" />

Para retornar a próxima entidade de uma determinada entidade, devemos somar o **índice geral** em `+1`. Por exemplo:

Com base no _exemplo modelo_, para obter a próxima entidade da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`) e somamos `+1`. Com o resultado (`4`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `D`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
```

* Se o resultado for maior que a quantidade máxima de itens na matriz é porque estamos na última entidade da expressão e não existe uma próxima entidade.