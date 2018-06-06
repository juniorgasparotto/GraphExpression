# Grupos de expressão <header-set anchor-name="expression-group" />

Os grupos são delimitados pelo uso de parenteses: `(` para abrir e `)` para fechar. 

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

## Grupo de expressão raiz <header-set anchor-name="expression-group-root" />

O primeiro grupo de expressão é chamado de **grupo de expressão raiz**.

Não é obrigatório o uso dos parenteses no grupo de expressão raiz. Veremos que nos exemplos a seguir ambas as expressão estão corretas:

``` 
(A + B)
```

Ou

``` 
A + B
```

## Sub-grupos de expressão <header-set anchor-name="expression-sub-group" />

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`(A + B + (C + D))`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C` e a entidade `C` será pai da entidade `D`.

## Repetições de grupo de expressão <header-set anchor-name="expression-group-repeat" />

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

## Entidade Raiz <header-set anchor-name="entity-root" />

A primeira entidade da expressão é a **entidade raiz** da expressão. Uma expressão só pode conter uma entidade raiz.

```
A + B + (C + A)
```

* A entidade `A` é a entidade raiz de toda expressão acima e será o topo do grafo.

## Entidade Pai <header-set anchor-name="entity-parent" />

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo. 

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.

## Entidade final <header-set anchor-name="entity-final" />

Uma entidade que não possui grupos de expressão em seu nível é chamada de **entidade final**. Isso não significa que a entidade não tenha filhos, veja:

**Entidade final sem filhos:**

```
(A + B + C + (D + E))
```

* As entidades `B`, `C` e `E` são entidades finais.

**Entidade final com filhos:**

```
(A + (B + C) + (D + B))
```

* A entidade `C` é final e não contém filhos
* A entidade `B`, do grupo de expressão da entidade `D`, também é final, mas ela contém filhos.