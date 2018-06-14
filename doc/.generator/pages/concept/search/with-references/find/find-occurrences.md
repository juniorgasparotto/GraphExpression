### Encontrando todas as ocorrências de uma entidade <header-set anchor-name="search-find-occurrences"/>

Para encontrar todas as ocorrências de uma entidade, devemos percorrer toda a matriz partindo do índice `0` até última posição da matriz.

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, a _pesquisa profunda_ pode retornar uma quantidade maior de ocorrências. Isso ocorre por que nesse tipo de pesquisa os grupos de expressões são redeclarados.

Sendo assim, é recomendado o uso da **pesquisa profunda** caso a sua necessidade seja obter o maior número possível de caminhos.

**Pesquisa profunda**

Usaremos nesse exemplo a <anchor-get name="sample-matrix-desnormalizated">matriz desnormalizada</anchor-get> do tópico sobre <anchor-get name="search-deep" />.

1. Se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas:
    * `#03 (Y)`
    * `#10 (Y)`: Essa ocorrência é derivada da **desnormalização**.
    * `#11 (Y)`

**Pesquisa superficial**

A lógica será a mesma da **pesquisa profunda**, contudo não teremos as ocorrências decorrentes das redeclarações dos grupos de expressão.

Usaremos nesse exemplo a <anchor-get name="sample-matrix">matriz original</anchor-get> do tópico sobre <anchor-get name="matrix-of-information" />.

1. Se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas:
    * `#03 (Y)`
    * `#10 (Y)`

* Note que foi encontrado uma ocorrência a menos que na _pesquisa profunda_.