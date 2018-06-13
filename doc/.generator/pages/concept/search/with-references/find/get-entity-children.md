### Encontrando os filhos de uma entidade <header-set anchor-name="search-method-get-entity-children" />

Seguindo a lógica da pesquisa acima, para encontrar apenas os filhos da entidade `D`, precisaríamos limitar o nível geral dos descendentes á: _[nível geral da entidade corrente] + 1_

Com base no _exemplo modelo_:

* A entidade `D` tem o nível geral igual a `2`.
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`**.
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`**.
* As próximas entidades depois de `F` são: `G`, `B`, `C`, `Y` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`**.

Acabou a expressão e no final teremos as seguintes entidades descendentes: `E`, `F` e `Z`

**Observação:**

Essa técnica deve sempre ser aplicada na **primeira ocorrência** da entidade e não na **ocorrência corrente**, e não importa se a expressão está ou não desnormalizada. Isso foi explicado em detalhes no tópico <anchor-get name="search-deep-has-children" />.