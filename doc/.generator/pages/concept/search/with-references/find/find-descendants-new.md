### Encontrando todos os descendentes de uma entidade <header-set anchor-name="search-method-get-descendants" />

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se a próxima entidade tem seu **nível geral** maior que o **nível geral** da entidade desejada, se tiver, essa entidade é sua descendente.

Devemos continuar navegando para frente até quando a próxima entidade tiver o mesmo **nível geral** da entidade desejada ou se a expressão não tiver mais entidades.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar entidades duplicadas em caso de grupos de expressões que foram redeclarados e será necessário remover as duplicações.

Sendo assim, é recomendado o uso da **pesquisa superficial** para evitar um processamento desnecessário.

**Pesquisa profunda**

Usaremos nesse exemplo a <anchor-get name="sample-matrix-desnormalizated">matriz desnormalizada</anchor-get> do tópico sobre <anchor-get name="search-deep" />.

1. Se quisermos pegar os descendentes da entidade `F`.

* A entidade `F` tem o nível geral igual a `3`.
* **A entidade `G` é a próxima entidade depois de `F` e o seu nível geral é `4`, é descendente**.
* **A entidade `B` é a próxima entidade depois de `G` e o seu nível geral é `5`, é descendente**.
* **A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é `5`, é descendente**.
* **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `6`, é descendente**.
* **A entidade `Y` é a próxima entidade depois de `Y` e o seu nível geral é `4`, é descendente**.
* A entidade `Z` é a próxima entidade depois de `Y`, porém o seu nível é `3`, igual ao nível da entidade `F`, portanto não é descendente.

Após eliminarmos as repetições de entidades, obtemos como resultado final as seguintes entidades descendentes: `G`, `B`, `C` e `Y`.

**Pesquisa superficial**

A lógica será a mesma da **pesquisa profunda**, contudo não teremos as ocorrências decorrentes das redeclarações dos grupos de expressão.

Usaremos nesse exemplo a <anchor-get name="sample-matrix">matriz original</anchor-get> do tópico sobre <anchor-get name="matrix-of-information" />.

1. Se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas:
    * `#03 (Y)`
    * `#10 (Y)`

* Note que foi encontrado uma ocorrência a menos que na _pesquisa profunda_.

### Encontrando todos os descendentes de uma entidade <header-set anchor-name="search-method-get-descendants" />

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se a próxima entidade tem seu **nível geral** maior que o **nível geral** da entidade desejada, se tiver, essa entidade é sua descendente.

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