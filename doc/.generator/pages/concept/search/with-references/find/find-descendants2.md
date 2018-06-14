### Encontrando todos os descendentes de uma entidade <header-set anchor-name="search-method-get-descendants" />

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se o seu **nível geral** é menor que o nível geral da **próxima entidade**, se for, essa entidade é uma descendente da entidade corrente. Essa é a mesma técnica usada no tópico <anchor-get name="search-deep-is-first-at-group-expression" />.

Devemos continuar navegando para frente até quando a próxima entidade tiver o **nível geral** igual ou menor ao **nível geral** da entidade corrente ou se a expressão não tiver mais entidades.

<!-- **Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, não podemos apenas verificar a **ocorrência da entidade**, pois não é garantido que o seu grupo de expressão foi declarado nesse momento.  -->

**Atenção:** Essa pesquisa pode ser feita usando os dois tipos de pesquisa: **Pesquisa profunda** e **Pesquisa superficial**. Contudo, existem abordagens diferentes para cada uma delas. Além disso, devemos ter um tratamento especial para entidades que contenham uma ascendente da própria entidade, ou seja, um **caminho cíclico**.

**Entidade com caminho cíclico:**

Devemos ter alguns cuidados para encontrar os descendentes de entidades com caminhos cíclicos. Isso ocorre porque os grupos de expressão não podem ser redeclarados nessas situações.

Por exemplo, como podemos encontrar os descendentes da entidade `A` que está no índice `#05`?

```
                A + B + (C + Y) + (D + A + C)
                                       ^
General Level:  1   2    2   3     2   3   3
Index:          0   1    2   3     4   5   6
```

* A entidade `A` que está no índice `#05` não foi redeclarada para evitar um **caminho cíclico**. 
* Note que a entidade `A` contém descendentes (é a entidade raiz), mas é impossível descobrir isso se analisarmos a sua ocorrência do índice `#05`.

A resposta seria:

* Encontrar todas as ocorrências da entidade `A`.
* Dentre as ocorrências encontradas, devemos encontrar e utilizar a primeira que tem descendentes e ignorar as demais.
    * _Ocorrência 1_:
        * `#00`: A entidade `A` tem o nível geral igual a `1`.
        * `#01`:**A entidade `B` é a próxima entidade depois de `A` e o seu nível geral é `2`, é descendente**.
        * Pronto! Encontramos a ocorrência que tem a declaração do grupo de expressão da entidade `A`.
    * _Ocorrência 2_:
        * `#05`: Não é preciso verificar a segunda ocorrência da entidade `A`, pois já encontramos a sua declaração.
* Retornar os descendentes da entidade `A` do índice `#00`:
    * `#00`: A entidade `A` tem o nível geral igual a `1`.
    * `#01`:**A entidade `B` é a próxima entidade depois de `A` e o seu nível geral é `2`, é descendente**.
    * `#02`:**A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é `2`, é descendente**.
    * `#03`:**A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `3`, é descendente**.
    * `#04`:**A entidade `D` é a próxima entidade depois de `Y` e o seu nível geral é `2`, é descendente**.
    * `#05`:**A entidade `A` é a próxima entidade depois de `D` e o seu nível geral é `3`, é descendente**.
    * `#06`:**A entidade `C` é a próxima entidade depois de `A` e o seu nível geral é `3`, é descendente**.
    * `#07`:**A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `4`, é descendente**.
    * A expressão terminou.
    * Foram encontradas as seguintes entidades: `A, B, C, Y, D, A, C, Y`.
* Remover as ocorrências que estão duplicadas: `Y, C`
* Retornar o resultado: `A, B, C, Y, D, A`

**Pesquisa profunda**

Se uma entidade não tiver um **caminho cíclico**, podemos simplesmente continuar a pesquisa de descendentes da ocorrência corrente, pois é garantido que seu grupo de expressão foi redeclarado.

**Pesquisa superficial**

Na pesquisa superficial devemos ter alguns cuidados. Notem que na expressão abaixo chegamos em um cenário muito parecido com as **entidades com caminhos cíclicos**.

Por exemplo, como podemos retornar os descendentes da entidade `C` do índice `#02`?

```
                A + B + C + (D + A + (C + Y)) + Z
                        ^              
General Level:  1   2   2    2   3    3   4     2
Index:          0   1   2    3   4    5   6     7
```

* A entidade `C` que está no índice `#02` não foi redeclarada, pois estamos usando a pesquisa superficial.
* Essa expressão não esta **normalizada**, a entidade `C` deveria ter sido declarada o mais rápido possível, mas isso não ocorreu.
* A entidade `C` contém descendentes. Seu grupo de expressão é declarado no índice `#05`.

Nesse caso temos duas opções:

**Opção 1:**

Utilizar a mesma lógica que foi explicada para **entidades com caminhos cíclicos**. Com isso será avaliado todas as ocorrências da entidade `C` até encontrarmos a ocorrência que declara o seu grupo de expressão.

* Seria encontrado a ocorrência do índice `#05` e a ocorrência do índice `#02` seria descartada.
* Agora que achamos a ocorrência correta, devemos retornar os descendentes:
    * `#05`: A entidade `C` tem o nível geral igual a `3`.
    * `#06`:**A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `4`, é descendente**.
    * `#07`: A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral é `2`, ela não é descendente.
    * A expressão não terminou, mas foi interrompida depois do resultado negativo do índice `#07`.
    * Foram encontradas as seguintes entidades: `Y`.
* Remover as ocorrências que estão duplicadas, nesse caso não tivemos nenhuma.
* Retornar o resultado: `Y`

**Opção 2:**

A segunda opção pode apresentar uma melhor performance se a expressão nascer de forma normalizada, se isso estiver garantido, não precisamos executar o primeiro passo.

* Aplicar a <anchor-get name="normalization-3" /> para garantir que todas as entidades estão sendo declaradas logo na primeira utilização. Esse passo não é necessário se a expressão nascer normalizada.

```
                A + B + (C + Y) + (D + A + C) + Z
                         ^              
General Level:  1   2    2   3     2   3   3    2
Index:          0   1    2   3     4   5   6    7
```

* Localizar a primeira ocorrência da entidade `C`. Após a normalização, devemos encontrar a ocorrência que está no índice `#02`.
* Recuperar os descendentes da primeira ocorrência da entidade `C` do índice `#02`.
    * `#02`: A entidade `C` tem o nível geral igual a `2`.
    * `#03`:**A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é `3`, é descendente**.
    * `#04`: A entidade `D` é a próxima entidade depois de `Y` e o seu nível geral é `2`, ela não é descendente.
    * A expressão não terminou, mas foi interrompida depois do resultado negativo do índice `#04`.
    * Foram encontradas as seguintes entidades: `Y`.
* Remover as ocorrências que estão duplicadas, nesse caso não tivemos nenhuma.
* Retornar o resultado: `Y`

Esse tema também foi abordado, de forma superficial, no tópico <anchor-get name="entity-declaration" />.