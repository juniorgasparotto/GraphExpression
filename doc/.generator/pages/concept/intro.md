# Compreendendo uma expressão de grafos <header-set anchor-name="intro" />

Uma expressão de grafos é composta por 4 elementos básicos e diversas informações que vamos detalhar nesse documento.

**Expressão de grafos - Exemplo:**

```
(A + B + C + D)
```

Os elementos que compõe uma expressão são:

* **Entidade:** É o elemento fundamental da expressão, determina uma unidade, um vértice na teória de grafo. São representados por um literal, no caso acima, as letras: `A`, `B` e etc.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade, uma aresta em teória de grafos.
* **Operador de subtração `-`**: É o elemento que remove uma entidade de outra entidade.
* **Parenteses `(` e `)`**: São usados para determinar um grupo de entidades filhas de uma determina entidade.

Esses elementos, são os mesmos de uma expressão matemática, a diferença é que no lugar de números teremos entidades que vão ser adicionas ou removidas uma nas outras. Além disso, o objetivo do resultado tem suas diferenças.

Essa expressão representa o seguinte grafo:

```
A 
----B
----C
----D
```

## Resolução da expressão <header-set anchor-name="expression-execution-order" />

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona ou remove a entidade da direita e o resultado dessa soma é a propria entidade da esquerda e assim sucessivamente até chegar no final da expressão.

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

É obvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita.

## Operador de soma

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

## Operador de subtração

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

Note que uma das ocorrências da entidade `B` foi removida da entidade `A`. Com base no mesmo exemplo, se quisessemos remover todas as ocorrências da entidade `B` teriamos que fazer a operação de subtração 3 vezes, que é equivalente a quantidade de vezes que entidade `B` existe dentro da entidade `A`.

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
