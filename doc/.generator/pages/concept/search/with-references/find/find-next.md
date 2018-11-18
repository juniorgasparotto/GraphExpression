### Encontrando a próxima entidade <header-set anchor-name="search-find-next" />

Para retornar a próxima entidade de uma determinada entidade, devemos somar o seu **índice geral** em "+1". 

**Atenção:** Essa pesquisa não apresenta diferenças entre os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**.

Usaremos nesse exemplo a <anchor-get name="sample-matrix-desnormalizated">matriz desnormalizada</anchor-get> do tópico sobre <anchor-get name="search-deep" />.

1. Para obter a próxima entidade da entidade "Y" da linha "#03", pegamos seu índice geral ("3") e somamos "+1". Com o resultado ("4"), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade "D".

```
Index   | Entity | Level | Level Index
#03     | Y      | 3     | 0 
#04     | D      | 2     | 2 
```

* Se o resultado for maior que a quantidade máxima de itens na matriz é porque estamos na última entidade da expressão e não existe uma próxima entidade.