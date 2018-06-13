## Tipos de pesquisas <header-set anchor-name="search-deep" />

### Encontrando todas as ocorrências de uma entidade <header-set anchor-name="search-deep-occurrences" />

Uma entidade pode ter mais de uma ocorrência em um grafo, no _exemplo modelo_, se quisermos buscar todas as ocorrências da entidade `Y` dentro do grafo, encontraríamos as linhas `#3`, `#10` e `#11`.

* Note que sem a desnormalização não seria possível encontrar a linha `#10` e não seria possível obter o número correto de ocorrências dessa entidade.

### Retornando a entidade anterior <header-set anchor-name="search-deep-get-entity-previous" />

Para retornar a entidade anterior de uma determinada entidade, devemos subtrair o **índice geral** em `-1`.

Por exemplo:

Para obter a entidade anterior da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`), e subtraímos `-1`. Com o resultado (`2`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `C`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#02             | C        | 2           | 1 
#03             | Y        | 3           | 0 
```

* Se o resultado for menor que zero, é porque estamos na **entidade raiz** e não existe entidade anterior. 

### Retornando a próxima entidade <header-set anchor-name="search-deep-get-entity-next" />

Para retornar a próxima entidade de uma determinada entidade, devemos somar o **índice geral** em `+1`. Por exemplo:

Com base no _exemplo modelo_, para obter a próxima entidade da entidade `Y` da linha `#03`, pegamos seu índice geral (`3`) e somamos `+1`. Com o resultado (`4`), encontramos na matriz a entidade que está nessa posição, nesse caso, retornaríamos a entidade `D`.

```
Índice geral    | Entidade | Nível geral | Índice do nível
#03             | Y        | 3           | 0 
#04             | D        | 2           | 2 
```

* Se o resultado for maior que a quantidade máxima de itens na matriz é porque estamos na última entidade da expressão e não existe uma próxima entidade. 

### Verificando se uma entidade é a primeira do grupo de expressão (primeira dentro dos parêntese) <header-set anchor-name="search-deep-is-first-at-group-expression" />

Para descobrir se uma entidade é a primeira do seu grupo de expressão (primeira dentro do parênteses), verificamos se o seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade é a primeira de seu grupo de expressão.

```
                A + B + ( C + Y ) + (D + C)
Nível geral:    1   2     2   3      2   3
Index:          0   1     2   3      4   5
```

No exemplo acima, a entidade `C`, do índice `#02`, tem o nível geral igual á `2` e a sua próxima entidade `Y` tem o nível geral igual á `3`, sendo assim, ela é a primeira dentro de seu parênteses.

**Observação:**

Não confunda essa técnica como sendo a solução para verificar se uma entidade contém filhos. Veremos isso no tópico <anchor-get name="search-deep-has-children" />.

### Verificando se entidade é a última do grupo de expressão (última dentro dos parêntese) <header-set anchor-name="search-deep-has-last-at-group-expression" />

Para descobrir se uma entidade é a última do seu grupo de expressão (última dentro do parênteses), verificamos se seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade é a última do seu grupo de expressão.

```
                A + B + ( C + Y ) + (D + C) + U
Nível geral:    1   2     2   3      2   3    2
Index:          0   1     2   3      4   5    6
```

No exemplo acima, a entidade `Y`, do índice `#03`, tem o nível geral igual á `3` e a sua próxima entidade `D` tem o nível geral igual á `4`, sendo assim, ela é a última dentro de seu parênteses.

* A entidade `U` do índice `#06` não tem uma próxima entidade, portanto ela é a última de seu grupo de expressão, embora ele esteja omitido por estarmos no **grupo de expressão raiz**.

### Verificando se a entidade é a raiz da expressão <header-set anchor-name="search-deep-is-root" />

Para descobrir se uma entidade é a raiz da expressão, verificamos se o seu **índice geral** é igual á `0`, se for, então ela será a entidade raiz.

```
        A + B
Index:  0   1
```

* A entidade `A` é a raiz.

### Verificando se uma entidade contém filhos <header-set anchor-name="search-deep-has-children" />

Para descobrir se uma entidade contém filhos, verificamos se o seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade contém filhos. Essa é a mesma técnica usada no tópico <anchor-get name="search-deep-is-first-at-group-expression" />.

Contudo, não podemos apenas verificar a **ocorrência da entidade**, pois não é garantido que o seu grupo de expressão foi declarado nesse momento. Nesse caso temos duas opções que serão explicadas adiante.

Essa limitação também serve para expressões **desnormalizadas**, isso por que ela não repete grupos de expressões que tem ascendências da entidade corrente.

Por exemplo, se quisermos descobrir se a entidade `A` , que está no índice `#05`, contém filhos:

```
                A + B + ( C + Y ) + (D + A)
General Level:  1   2     2   3      2   3
Index:          0   1     2   3      4   5
```

Essa expressão está **desnormalizada** e a entidade `A` que está no índice `#05` não foi redeclarado para evitar um **caminho cíclico**.

**Opção 1:**

1. Verificar se todas as ocorrências da entidade `A` contém filhos.

Com isso, teríamos um resultado negativo ao analisar a ocorrência da entidade `A` que está no índice `#05` e um resultado positivo ao analisar a ocorrência da entidade `A` que está no índice `#00`.

_Essa opção deve ser evitada se o seu propósito for retornar as entidades filhas, isso porque você retornaria as mesmas entidades em todas as ocorrências, nesse caso utilize a opção abaixo._

**Opção 2:**

1. Aplicar a <anchor-get name="normalization-3" /> para garantir que todas as entidades estão sendo declaradas logo na primeira utilização.
2. Localizar a primeira ocorrência da entidade `A`. Deve-se encontrar a ocorrência que está no índice `#00`.

Com isso, teríamos um resultado positivo ao analisar a ocorrência da entidade `A` que está no índice `#00` e não seria necessário verificar as outras ocorrências.

Esse tema também foi abordado, de forma superficial, no tópico <anchor-get name="entity-declaration" />.

### Encontrando todas as entidades que contenham filhos <header-set anchor-name="search-deep-with-children" />

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

### Encontrando todos os descendentes de uma entidade <header-set anchor-name="search-deep-descendants" />

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

### Encontrando os filhos de uma entidade <header-set anchor-name="search-deep-get-entity-children" />

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

### Encontrando todos os ascendentes de uma entidade <header-set anchor-name="search-deep-get-entity-ascending" />

Se quisermos encontrar os ascendentes de uma entidade, devemos verificar se a entidade anterior tem seu **nível geral** menor que o **nível geral** da entidade desejada, se tiver, essa entidade é uma ascendente.

```
             A + B
Nível geral: 1   2
             ^   *
Parent of B: A
```

Se a entidade anterior for do mesmo nível da entidade deseja, deve-se ignora-la e continuar navegando para trás até encontrar a primeira entidade com o **nível geral** menor que o **nível geral** da entidade desejada. 

```
             A + B + J
Nível geral: 1   2   2
             ^       *
Parent of J: A
```

Após encontrar a primeira ascendência, deve-se continuar navegando para trás, porém o **nível geral** a ser considerado agora será o da primeira ascendência e não mais da entidade desejada. Esse processo deve continuar até chegar na entidade raiz.

```
              A + B + (J + Y)
Nível geral:  1   2    2   3
              ^        ^   *
Parents of Y: J, A
```

Por exemplo, se quisermos pegar os ascendentes da entidade `C` considerando todas as suas ocorrências na matriz do primeiro exemplo, teremos:

**Ocorrência 1:**

* A entidade `C` da linha `#02` tem o nível geral igual a `2`.
* `#01`: A entidade `B` tem o nível geral igual a `2`, não é ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a `1`, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível `1` e não mais o nível `2`**.

Acabou a expressão e teremos as seguintes entidades ascendentes: `A`

**Ocorrência 2:**

* A entidade `C` da linha `#09` tem o nível geral igual a `5`.
* `#08`: A entidade `B` tem o nível geral igual a `5`, não é ascendente.
* `#07`: **A entidade `G` tem o nível geral igual a `4`, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível `4` e não mais o nível `5`**.
* `#06`: **A entidade `F` tem o nível geral igual a `3`, é menor que o nível geral da entidade `G`, portanto é uma ascendente. Agora o nível a ser considerado será o nível `3` e não mais o nível `4`**.
* `#05`: A entidade `E` tem o nível geral igual a `3`, não é uma ascendente.
* `#04`: **A entidade `D` tem o nível geral igual a `2`, é uma ascendente. Agora o nível a ser considerado será o nível `2` e não mais o nível `3`**.
* `#03`: A entidade `Y` tem o nível geral igual a `3`, não é uma ascendente.
* `#02`: A entidade `C` tem o nível geral igual a `2`, não é uma ascendente.
* `#01`: A entidade `B` tem o nível geral igual a `2`, não é uma ascendente.
* `#00`: **A entidade `A` tem o nível geral igual a `1`, é uma ascendente. Agora o nível a ser considerado será o nível `1` e não mais o nível `2`**.

Acabou a expressão e no final teremos as seguintes entidades ascendentes: `G`, `F`, `D` e `A`

### Encontrando os pais de uma entidade <header-set anchor-name="search-deep-get-entity-parent" />

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisaríamos limitar o nível geral dos ascendentes á: _[nível geral da entidade corrente] - 1_; ou a primeira entidade com o nível geral menor que a entidade desejada.

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