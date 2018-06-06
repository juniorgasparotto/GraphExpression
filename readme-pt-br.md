[
![Inglês](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/documentation/img/en-us.png)
](https://github.com/juniorgasparotto/ExpressionGraph)
[
![Português](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/documentation/img/pt-br.png)
](https://github.com/juniorgasparotto/ExpressionGraph/blob/master/readme-pt-br.md)

# <a name="concept" />Expressão de grafos

O conceito expressão de grafos foi criado em 2015 por Glauber Donizeti Gasparotto Junior e tem como objetivos a representação de um grafo em forma de expressão.

A ideia de uma representação em forma de expressão é resumir um grafo em um texto que seja humanamente legivel e não apenas imagem.

Com o avanço do entendimento do conceito, você vai notar que ele pode ser útil para a criação de mecanismos de pesquisas complexas. Vale ressaltar que o foco não é performance, mas apenas uma nova forma de enchergar um grafo e suas informações.

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

* **Entidade:** É o elemento fundamental da expressão, determina uma unidade, um vértice no conceito de grafo.
* **Operador de soma `+`**: É o elemento que adiciona uma entidade em outra entidade.
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

## Grupos de expressão

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

### Grupo de expressão raiz

O primeiro grupo de expressão é chamado de "grupo de expressão raiz".

Não é obrigatório o uso dos parenteses no grupo de expressão raiz. Veremos que nos exemplos a seguir ambas as expressão estão corretas:

```
(A + B)
```

Ou

```
A + B
```

### Sub-grupos de expressão

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`(A + B + (C + D))`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C` e a entidade `C` será pai da entidade `D`.

### Repetições de grupo de expressão

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

### Entidade Raiz

A primeira entidade da expressão é a entidade raiz da expressão. Uma expressão só pode conter uma entidade raiz.

```
A + B + (C + A)
```

* A entidade `A` é a entidade raiz de toda expressão acima e será o topo do grafo.

### Entidade Pai

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo.

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.

### Entidade final

Uma entidade que não possui grupos de expressão em seu nível é chamada de "entidade final". Isso não significa que a entidade não tenha filhos, veja:

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

## Caminhos de entidades

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome, isso não faz sentido. Mas a mesma entidade pode aparecer em diversos pontos no grafo.

E para representar cada ocorrência usamos a notação de "caminhos de entidades" para determinar o caminho de inicio e fim até chegar na ocorrência da entidade. Abaixo temos um caminho de início e fim até chegar na entidade `D`.

```
A.B.C.D
```

Essa notação é o mesmo que:

```
A + (B + (C + D))
```

* A entidede `D` é filha da entidade `C`
* A entidede `C` é filha da entidade `B`
* A entidede `B` é filha da entidade `A`

O caractere `.` é usado entre a entidade pai e a entidade filha. A entidade da esquerda será a pai e a entidade da direita será a filha. Vejamos mais exemplos:

**Expressão:**

```
(A + A + (B + C) + (D + B))
```

**Caminho da entidade `A`:**

Ocorrência 1: `A` Ocorrência 2: `A.A`

Na "ocorrência 2" temos uma relação ciclica, portanto a notação é interrompida quando isso acontece, do contrário teriamos um caminho infinito.

**Caminho da entidade `B`:**

Ocorrência 1: `A.B` Ocorrência 2: `A.D.B`

### Caminhos cíclicos na expressão

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe uma soma ciclica entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação ciclica.

Note que o grafo contém duas referências ciclicas:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade `A` é pai dela mesma.
* Uma indireta (`C + A`): Onde `C` é pai de uma entidade ascendente, no caso a entidade `A`.

## Níveis

Uma expressão tem dois tipos de níveis: "Nível geral" e "Nível na expressão".

O "nível geral" determina em qual nível a entidade está com relação a hierarquia do grafo.

Por exemplo:

```
A (Nível 1)
----B (Nível 2)
    ----C (Nível 3)
    ----D (Nível 3)
        ----B (Nível 4)
----E (Nível 2)
    ----A (Nível 3)
```

O "nível na expressão" determina em qual nível a entidade está com relação a expressão.

Por exemplo:

```
                    A + B + C + ( D + E + ( F + G ) )
Nível na expressão: 1   1   1     2   2     3   3    
Nível geral:        1   2   2     2   3     3   4   
```

Note que o nível da expressão ignora o nível da entidade na hieraquia, é uma informação útil apenas para a expressão.

## Índices

Uma expressão tem dois tipos de índices: "Índice na expressão" e "Índice do nível".

O "Índice da expressão" determina em qual posição a entidade está na expressão. O índice inicia em zero e soma-se +1 até a última entidade da expressão.

Por exemplo:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

O "Índice do nível" determina em qual posição a entidade está com relação ao seu nível. O índice inicia em zero na primeira entidade do nível e soma-se +1 até a última entidade do mesmo nível. Por exemplo:

```
                 A + B + C + ( D + E + ( F + G ) )
Nível geral:     1   2   2     2   3     3   4   
Índice do nível: 0   0   1     2   0     1   0  
```

* A entidade `A` é a raiz da expressão e seu "índice no nível" será zero. Note que por ser a entidade raiz, ela não terá outras entidades em seu nível.
* A entidade `B` é a primeira do segundo nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` é a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` é a terceria do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` é a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` é a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` é a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.