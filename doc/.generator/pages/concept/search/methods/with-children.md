### Encontrando todas as entidades que contenham filhos <header-set anchor-name="search-method-with-children" />

Para isso, basta recuperar as **entidades anteriores** de todas as entidades cujo o **índice do nível** seja igual a `0`.

Com base no _exemplo modelo_, teremos os seguintes passos:

1. Primeiro, encontramos todas as linhas com o índice do nível igual a zero: 
    * `#00 (A)`
    * `#01 (B)`
    * `#03 (Y)`
    * `#05 (E)`
    * `#07 (G)`
    * `#08 (B)`
    * `#10 (Y)`
2. Para cada linha encontrada, retornamos a sua entidade anterior que será uma entidade pai:
    * `NULL`  -> `#00 (A)`: Não contém entidade anterior, portanto não retorna nada.
    * `#00 (A)` -> `#01 (B)`: Retorna a entidade `A` como sendo sua anterior
    * `#02 (C)` -> `#03 (Y)`: Retorna a entidade `C` como sendo sua anterior
    * `#04 (D)` -> `#05 (E)`: Retorna a entidade `D` como sendo sua anterior
    * `#06 (F)` -> `#07 (G)`: Retorna a entidade `F` como sendo sua anterior
    * `#07 (G)` -> `#08 (B)`: Retorna a entidade `G` como sendo sua anterior
    * `#09 (C)` -> `#10 (Y)`: Retorna a entidade `C` como sendo sua anterior

Com isso, após removermos as repetições (no caso, a entidade `C` que aparece nas linhas `#2` e `#09`), obtemos como resultado final as entidades `A`, `C`, `D`, `F` e `G` como sendo as únicas entidades com filhos na expressão.