# Expressão de grafos <header-set anchor-name="concept" />

O conceito expressão de grafos foi criado em 2015 por _Glauber Donizeti Gasparotto Junior_ e tem como objetivo a representação de um grafo em forma de expressão.

A ideia de uma representação em forma de expressão é resumir um grafo em um texto que seja humanamente legível e de fácil transporte.

Com o avanço do entendimento do conceito, você vai notar que ele pode ser útil para a criação de mecanismos de pesquisas complexas. Vale ressaltar que o foco não é performance, mas apenas uma nova forma de enxergar um grafo e suas informações.

Imagine o seguinte grafo:

```
A 
----B
    ----C
    ----D
        ----B
----E
    ----A
```

A sua representação em forma de expressão seria:

```
(A + (B + C + (D + B)) + (E + A))
```

Note que essa representação se parece com uma expressão matemática, porém a resolução da expressão é bem peculiar.

## Elementos de uma expressão de grafos

Primeiro, vamos listar os elementos de uma expressão:

* **Entidade:** É o elemento fundamental da expressão, determina uma unidade, um vértice no teória de grafo.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade, uma aresta em teória de grafos.
* **Parenteses `(` e `)`**: São usados para determinar um grupo de entidades filhas de uma determina entidade.

## Objetivo do resultado final

O objetivo de uma expressão de grafos é bem diferente do objetivo de uma expressão matemática. Na matemática, uma expressão nesses moldes teria números como sendo as entidades e após o processamento da expressão o resultado final seria outro número com a soma de todos os outros.

Em expressão de grafos, o objetivo final é gerar um grafo completo a partir de uma expressão, ou a partir de um grafo, fazer a engenharia reversa para obter sua representação em forma de expressão. 

### Ordem de resolução

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona a entidade da direita e o resultado dessa soma é a propria entidade da esquerda e assim sucessivamente até chegar no final. 

**Exemplo simples (Etapas simbólicas da resolução):**

1. `(A + B)`
2. Resultado final da expressão: `A`

_Composição final da entidade `A`_

```
A 
----B
```

**Exemplo composto (Etapas simbólicas da resolução):**

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Resultado final da expressão: `A`

_Composição final da entidade `A`_

```
A 
----B
----C
----D
```

**Conclusão**

Vimos que a cada etapa da resolução de uma expressão a entidade da direita desaparece e a entidade da esquerda prevalece até não restarem entidades a sua direita. É obvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita, mas o que importa aqui é entender a ordem de resolução e o objetivo final do resultado.