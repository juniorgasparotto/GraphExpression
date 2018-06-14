### Encontrando os filhos de uma entidade <header-set anchor-name="search-find-children" />

Para iniciar esse tópico é preciso entender por completo o tópico <anchor-get name="search-find-descendants" />.

A lógica é exatamente a mesma da pesquisa de descendentes, a única diferença é que o **nível geral** será limitado á: _[nível geral da entidade corrente] + 1_

Usaremos nesse exemplo a <anchor-get name="sample-matrix-desnormalizated">matriz desnormalizada</anchor-get> do tópico sobre <anchor-get name="search-deep" />.

Com base nessa matriz, se quisermos encontrar todas as filhas da entidade `D` da linha `#04`:

* A entidade `D` tem o nível geral igual a `2`.
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`**.
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`**.
* As próximas entidades depois de `F` são: `G`, `B`, `C`, `Y` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`**.

Acabou a expressão e no final teremos o resultado: `E, F, Z`