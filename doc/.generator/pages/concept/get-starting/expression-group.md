## Grupo de expressão <header-set anchor-name="expression-group" />

O **grupos de expressão** é delimitado pelo uso de parenteses: `(` para abrir e `)` para fechar. 

A primeira entidade do grupo de expressão (após abrir parenteses) determina a entidade pai daquele grupo, ou seja, todas as entidades subsequentes serão suas filhas até que se feche os parenteses.

**Exemplo 1:**

``` 
(A + B + C)
```

* A entidade `A` é a entidade pai de seu grupo de expressão e a entidade `B` e `C` são suas filhas.

**Exemplo 2:**

``` 
(A + B + (C + D))
```

* A entidade `A` é a entidade pai de seu grupo de expressão e a entidade `B` e `C` são suas filhas.
* A entidade `C` é a entidade pai de seu grupo de expressão e a entidade `D` é sua filha.

### Grupo de expressão raiz <header-set anchor-name="expression-group-root" />

O primeiro grupo de expressão é chamado de **grupo de expressão raiz**.

Não é obrigatório o uso dos parenteses no grupo de expressão raiz. Veremos que nos exemplos a seguir ambas as expressão estão corretas:

``` 
(A + B)
```

Ou

``` 
A + B
```

### Sub-grupos de expressão <header-set anchor-name="expression-sub-group" />

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`(A + B + (C + D))`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C` e a entidade `C` será pai da entidade `D`.

### Declarações de entidades <header-set anchor-name="entity-declaration" />

Chamamos de "**declaração**" o primeiro momento em que uma entidade é escrita, ou seja, sua primeira ocorrência.

Caso essa entidade contenha filhos devemos declarar todo o seu grupo de expressão no mesmo momento, ou seja, adicionando seus filhos dentro dos parenteses.

Não existe uma obrigatoriedade para a declaração do grupo de expressão ser na primeira ocorrência, mas isso ajuda a simplificar a descoberta de algumas informações de uma maneira mais rápida. 

Por exemplo, para descobrir se a entidade `B` contém filhos na expressão a seguir, será necessário verificar todas as suas ocorrências, pois não é possível dizer em qual das ocorrências o seu grupo de expressão foi declarado.

```
A + B + (C + (B + D)) + B
```

Agora, se soubermos que os grupos de expressões foram escritos sempre nas primeiras ocorrências, então podemos verificar apenas a primeira ocorrência da entidade `B` para saber se ela contém ou não filhos:

```
A + (B + D) + (C + B) + B
```

### Repetições de grupo de expressão <header-set anchor-name="expression-group-repeat" />

Um grupo de expressão não pode ser redeclarado na próxima vez que a entidade pai do grupo for utilizada.

Por exemplo: 

```
A + B + (C + D + E) + (I + C)
```

* A entidade `C` tem os filhos `D` e `E`
* A entidade `I` tem como filha a entidade `C`, porém não é necessário redeclarar as entidades filhas de `C`.

**Errado:**

```
A + B + (C + D + E) + (I + (C + D + E))
```

### Entidade pai <header-set anchor-name="entity-parent" />

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo. 

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.
