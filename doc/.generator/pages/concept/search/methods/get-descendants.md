### Encontrando todos os descendentes de uma entidade <header-set anchor-name="search-method-get-descendants" />

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se a próxima entidade tem seu **nível geral** maior que o **nível geral** da entidade desejada, se tiver, essa entidade é uma descendente.

Devemos continuar navegando para frente até quando a próxima entidade tiver o mesmo **nível geral** da entidade desejada ou se a expressão não tiver mais entidades.

Com base no _exemplo modelo_, se quisermos pegar os descendentes da entidade `F`.

* A entidade `F` tem o nível geral igual a `3`.
* **A entidade `G` é a próxima entidade depois de `F` e o seu nível geral é `4`, é descendente**.
* **A entidade `B` é a próxima entidade depois de `G` e o seu nível geral é `5`, é descendente**.
* **A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é `5`, é descendente**.
* **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `6`, é descendente**.
* **A entidade `Y` é a próxima entidade depois de `Y` e o seu nível geral é `4`, é descendente**.
* A entidade `Z` é a próxima entidade depois de `Y`, porém o seu nível é `3`, igual ao nível da entidade `F`, portanto não é descendente.

Após eliminarmos as repetições de entidades, obtemos como resultado final as seguintes entidades descendentes: `G`, `B`, `C` e `Y`.

**Observação:**

Essa técnica deve sempre ser aplicada na **primeira ocorrência** da entidade e não na **ocorrência corrente**, e não importa se a expressão está ou não desnormalizada. Isso foi explicado em detalhes no tópico <anchor-get name="search-deep-has-children" />.