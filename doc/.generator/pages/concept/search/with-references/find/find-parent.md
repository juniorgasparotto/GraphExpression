### Encontrando os pais de uma entidade <header-set anchor-name="search-method-get-entity-parent" />

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisaríamos limitar o nível geral dos ascendentes á: _[nível geral da entidade corrente] - 1_; ou a primeira entidade com o nível geral menor que a entidade desejada.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar uma quantidade maior de ocorrências. Isso ocorre por que nesse tipo de pesquisa os grupos de expressões são redeclarados.

Como existem 3 ocorrências da entidade `Y`, teremos uma _entidade pai_ por ocorrência:

**Ocorrência 1:**

* A entidade `Y` da linha `#3` tem o nível geral igual a `3`.
* `#02`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a `2`, portanto, ela é pai da entidade `Y`**.

**Ocorrência 2:**

* A entidade `Y` da linha `#10` tem o nível geral igual a `6`.
* `#09`: **A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a `5`, portanto, ela é pai da entidade `Y`**.

**Ocorrência 3:**

* A entidade `Y` da linha `#11` tem o nível geral igual a `4`.
* `#10`: A entidade `Y` tem o nível geral igual a `6`, não é uma ascendente.
* `#09`: A entidade `C` tem o nível geral igual a `5`, não é uma ascendente.
* `#08`: A entidade `B` tem o nível geral igual a `5`, não é uma ascendente.
* `#07`: A entidade `G` tem o nível geral igual a `4`, não é uma ascendente.
* `#06`: **A entidade `F` é a entidade anterior a `G` e tem o nível geral igual a `3`, portanto, ela é pai da entidade `Y`**.