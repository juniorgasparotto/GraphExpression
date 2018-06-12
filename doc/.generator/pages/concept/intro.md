# Compreendendo uma expressão de grafos <header-set anchor-name="intro" />

Uma expressão de grafos é composta por 4 elementos básicos e diversas informações que vamos detalhar nesse documento.

**Expressão de grafos - Exemplo:**

```
(A + B + C + D)
```

Os elementos que compõe uma expressão são:

* **Entidade**: É o elemento fundamental da expressão, determina uma unidade, um vértice na teoria de grafo. 
    * São únicos, mas podem aparecer `N` vezes na expressão em diferentes posições.
    * São representados por um literal, no caso acima, as letras: `A`, `B`, `C` e `D`.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade.
    * Fazendo uma analogia com a teoria de grafos, o operador de `+` pode ser visto como uma **aresta**.
* **Operador de subtração `-`**: É o elemento que remove uma entidade de outra entidade.
* **Parenteses `(` e `)`**: São usados para determinar um grupo de entidades filhas de uma determina entidade. 
    * Em expressão de grafos são denominados: **Grupo de expressão**.

Esses elementos, são os mesmos de uma expressão matemática, a diferença é que no lugar de números teremos entidades que vão ser adicionas ou removidas uma nas outras. Além disso, o objetivo do resultado tem suas diferenças.

Essa expressão representa o seguinte grafo:

```
A 
----B
----C
----D
```

## Resolução da expressão <header-set anchor-name="expression-execution-order" />

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona ou remove a entidade da direita e o resultado dessa soma é a própria entidade da esquerda e assim sucessivamente até chegar no final da expressão.

**Exemplo simples (Etapas simbólicas da resolução):**

1. `(A + B)`
2. Resultado final da expressão: `A`

_Grafo final da entidade `A`_

```
A 
----B
```

**Exemplo composto (Etapas simbólicas da resolução):**

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Resultado final da expressão: `A`

_Grafo final da entidade `A`_

```
A 
----B
----C
----D
```

Vimos que a cada etapa da resolução de uma expressão a entidade da direita desaparece e a entidade da esquerda prevalece até não restarem entidades a sua direita. 

É óbvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita.

## Entidade e suas ocorrências <header-set anchor-name="entity-and-occurrence" />

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome. Mas a mesma entidade pode aparecer em diversos pontos no grafo. 

```
(A + (B + C + A) + C)
```

Note que na expressão acima as entidades `A` e `C` estão repetidas. Elas representam a mesma entidade, porém em posições diferentes. Cada ocorrência contém algumas informações que são únicas daquela posição como:

* <anchor-get name="indexes" />
* <anchor-get name="levels" />
* <anchor-get name="entity-previous" />
* <anchor-get name="entity-next" />

## Operador de soma <header-set anchor-name="intro-plus" />

A operação de soma usa o operador `+`, como dito, ela funciona como uma aresta que liga um vértice a outro vértice. Em expressão de grafos, dizemos que a entidade da esquerda adiciona a entidade da direita e sem limitações, por exemplo:

* A entidade da esquerda pode adicionar a sí mesma quantas vezes for preciso:

```
Expression: A + A + A + A
Graph:
            A 
            ----A
            ----A
            ----A
```

* A entidade `X` pode adicionar a entidade `Y` e a entidade `Y` pode adicionar a entidade `X` quantas vezes for necessário.

```
Expression: X + (Y + X + X) + Y
Graph:
            X 
            ----Y
                ----X
                ----X
            ----Y
```

## Operador de subtração <header-set anchor-name="intro-subtract" />

A operação de subtração usa o operador `-`. Em expressão de grafos, dizemos que a entidade da esquerda remove a entidade da direita fazendo com que a entidade da direita deixe de ser sua filha. 

A cada operação de subtração apenas uma ocorrência será removida por vez, mesmo que a entidade da esquerda tenha mais de uma filha da mesma entidade. Por exemplo:

* A entidade da esquerda remove uma das filhas `B`

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B

Graph 2:
            A 
            ----B
            ----B
```

Note que uma das ocorrências da entidade `B` foi removida da entidade `A`. Com base no mesmo exemplo, se quiséssemos remover todas as ocorrências da entidade `B` teríamos que fazer a operação de subtração 3 vezes, que é equivalente a quantidade de vezes que entidade `B` existe dentro da entidade `A`.

Ainda é possível misturar as operações de soma e subtração.

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B - B - B + (C + Y)

Graph 2:
            A
            ----C    
                ----Y
```

Nesse exemplo, removemos todas as ocorrências da entidade `B` da entidade `A` e adicionamos uma nova filha `C` que contém a entidade `Y`.
