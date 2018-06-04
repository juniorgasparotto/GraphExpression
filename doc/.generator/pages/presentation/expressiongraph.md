# Expressão de grafos <header-set anchor-name="concept" />

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

Note que essa representação se parece com uma expressão matemática, porém a resolução da expressão é bem peculiar. Vamos lá:

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

Não é obrigatório o uso dos parenteses no primeiro grupo de expressão. Veremos que nos exemplos a seguir ambas as expressão estão corretas:

``` 
(A + B)
```

Ou

``` 
A + B
```

## Sub-grupos de expressão

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`(A + B + (C + D))`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C` e a entidade `C` será pai da entidade `D`.

## Entidade Raiz

A primeira entidade da expressão é a entidade raiz da expressão. Uma expressão só pode conter uma entidade raiz.

```
(A + A + B + (C + A))
```

* A entidade `A` é a entidade raiz de toda expressão acima e será o topo do grafo.

## Entidade Pai

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo. 

Por exemplo:

`(A + B + (C + D))`

* Nesse exemplo, temos duas entidades pai: `A` e `C`.
* O elemento `+` é utilizado como simbolo de atribuição de uma entidade filha em seu pai.

## Entidade final

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

## Somas ciclicas

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe uma soma ciclica entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação ciclica.

Note que o grafo contém duas referencias ciclicas: 

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
* A entidade `B` é a primeira do segunda nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` e a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` e a terceria do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` e a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` e a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` e a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.

## Navegação para a direita (Próxima entidade)

Toda entidade, com exceção da última da expressão, tem conhecimento da próxima entidade na expressão. 

No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a direita da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) )
B   C   D     E   F     G
```

No exemplo, a entidade `A` tem conhecimento da entidade `B`. Note que a entidade `B` é filha de `A`, mas isso não influência, pois a ideia é conhecer a próxima entidade da expressão e não do seu nível.

## Navegação para a esquerda (Entidade anterior)

Toda entidade, com exceção da primeira da expressão (a entidade raiz), tem conhecimento da entidade anterior na expressão. No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a esquerda da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) ) 
    A   B     C   D     E   F
```

## Repetições de grupo de expressão

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

## Normalização - tipo 1

A normalização tem o objetivo de enxugar grupos de expressão que pertencem a mesma entidade pai e que estão em diferentes lugares na expressão.

Por exemplo:

```
A + (B + Y) + (D + (B + C))
     ^              ^
```

Note que na expressão acima, a entidade `B` tem dois grupos de expressão em lugares distintos. Na prática, isso não tem nenhum problema, mas será visualmente melhor se aplicarmos a normalização eliminando um dos grupos da entidade `B`, veja:


```
A + (B + Y + C) + (D + B)
```

É preciso dizer que nenhuma alteração na expressão deve modificar o seu grafo final. É perceptível que no exemplo isso não ocorreu, as entidades apenas foram reoganizadas.

Já no próximo exemplo, veremos uma expressão que pode gerar confusão no momento da normalização:

```
A + (B + Y) + (D + (B + Y))
     ^              ^
```

Nesse exemplo, é natural pensar que um dos grupos da entidade `B` pode ser eliminado por serem iguais, mas esse pensamento está errado. Se eliminarmos um dos grupos, estaremos modificando o grafo final e esse não é o objetivo. 

**Errado:**

```
A + (B + Y) + (D + B)
```

**Correto:**

```
A + (B + Y + Y) + (D + B)
```

## Normalização - tipo 2

A normalização de tipo 2 tem o objetivo de organizar, quando possível, as "entidades finais" no começo do seu grupo de expressão para ajudar na visualização da expressão.

```
A + (B + (C + D) + E) + F + G
                   ^    ^   ^
```

Após a normalização ficaria assim:

```
A + F + G + (B + E + (C + D))
    ^   ^        ^    
```

* Note que as entidades `F` e `G` foram para o ínicio do seu grupo de expressão.
* A entidade `E` também foi reorganizada para o ínicio do seu grupo de expressão.

## Normalização - tipo 3

A normalização de tipo 3 tem o objetivo de declarar o mais rápido possível todos os "grupos de expressão".

Exemplo:

```
A + B + (C + G + (B + F)) + (G + F)
    ^             ^    
             ^               ^
```

Note que as entidades `B` e `G` são utilizadas antes que seus grupos sejam declarados e após a normalização teremos:

```
A + (B + F) + (C + (G + F) + B)
```

* Após a normalização, os grupos das entidades `B` e `G` foram declarados no primeiro momento que foram utilizadas.
* A entidade `B`, dentro do grupo `C`, se transformou em uma entidade final e devido a isso, podemos aplicar a "normalização de tipo 2" para melhorar ainda mais a visualização, veja:

```
A + (B + F) + (C + (G + F) + B)
```

```
A + (B + D) + K + (D + B)
```

```
A + (B + (D + B)) + K + (D + (B + D))
```

## Desnormalização

O objetivo da desnormalização é gerar uma nova expressão onde os grupos de expressões sejam escritos toda vez que a sua entidade pai for utilizada. Após a desnormalização será impossível voltar na expressão original, esse é um caminho sem volta. 

Considere a seguinte expressão original:

```
A + (B + D) + (E + B)
```

* Note que a entidade `B` é filha da entidade `A` e filha da entidade `E`.
* Após a desnormalização teremos a seguinte expressão:

```
A + (B + D) + (E + (B + D))
                    ^
```

* Após a desnormalização a entidade `B` teve seu grupo de expressão reescrito por completo quando foi utilizada novamente como filha da entidade `D`.

Como dito, é impossível voltar na expressão original, pois não conseguimos distinguir quais grupos de expressões eram da expressão original. Sendo assim, não podemos dizer que uma expressão original é igual a sua expressão desnormalizada. Vejam um exemplo de como elas são diferentes:

```
Original:       A + (B + D) + (E + B)
Grafo final:
                A
                ---B
                ------D
                ---E
                ------B
```

Se pegarmos a expressão desnormalizada e extrairmos o seu grafo, teremos um grafo diferente do grafo original:

```
Original:                    A + (B + D) + (E + (B + D))
Após normalização de tipo 1: A + (B + D + D) + (E + B)
Grafo final:
                             A
                             ---B
                             ------D
                             ------D
                             ---E
                             ------B
```

Portanto, não podemos considerar que uma expressão desnormalizada seja usada como uma expressão original, isso altera o grafo final. Além do mais, ela infringe a regra do tópico "Repetições de grupo de expressão".

## Representação em forma de matriz

Podemos representar uma expressão de grafos em uma matriz vertical com todas as informações de uma expressão. Isso facilita a visualização e pesquisa de grafos complexos e de multiplos níveis. Exemplo:

_Expressão:_

```
                    (A + B + C + ( D + E + ( F + ( G + B + C ) ) ) )
Nível geral:         1   2   2     2   3     3     4   5   5                     
Índice do nível:     0   0   1     2   0     1     0   0   1
```

_Hierarquia:_

```
A (Indice do nível: 0)
----B (Indice do nível: 0)
----C (Indice do nível: 1)
----D (Indice do nível: 2)
    ----E (Indice do nível: 0)
    ----F (Indice do nível: 1)
        ----G (Indice do nível: 0)
            ----B (Indice do nível: 0)
            ----C (Indice do nível: 1)
```

_Matriz de informações:_

```
Índice geral   | Entidade | Nível geral | Índice do nível
#1             | A        | 1           | 0
#2             | B        | 2           | 0
#3             | C        | 2           | 1
#4             | D        | 2           | 2
#5             | E        | 3           | 0
#6             | F        | 3           | 1
#7             | G        | 4           | 0
#8             | B        | 5           | 0
#9             | C        | 5           | 1
```

* Note que as entidades `B` e `C` tem dois pais: `A` e `G`, porém com níveis diferentes.

## Pesquisa e navegação

Com base nessa matriz de informações e ao fato das entidades conhecerem os seus "vizinhos", ou seja, aquelas que estão posicionadas na sua esquerda ou na sua direita na expressão (independentemente do nível) podemos enfim criar meios de navegação e pesquisa de entidades. 

Vejamos alguns exemplos de pesquisas:

### Pesquisando todas as ocorrências de uma entidade

Uma entidade pode ter mais de uma ocorrência em um grafo, no exemplo acima, se quisermos buscar todas as entidades `B` no grafo, encontrariamos as linhas `#2` e `#8`.

### Pesquisando todas as entidades que contenham filhos

Para isso, basta recuperar as **entidades anteriores** de todas as entidades cujo o **índice do nível** seja igual a `0`.

No exemplo, encontrariamos:

1. Encontramos todas as entidades com o índice do nível igual a zero: `A`,`B`, `E`, `G` e `B`
2. Para cada entidade encontrada, retornamos a sua entidade anterior que será uma entidade pai:
    * `A`: Não contém entidade anterior, portanto não retorna nada.
    * `B`: Retorna a entidade `A` como sendo sua anterior
    * `E`: Retorna a entidade `D` como sendo sua anterior
    * `G`: Retorna a entidade `F` como sendo sua anterior
    * `B`: Retorna a entidade `G` como sendo sua anterior

Com isso, obtemos as entidades `A`, `D`, `F` e `G` como sendo as únicas entidades com filhos na expressão.

### Pesquisando todos os descendentes de uma entidade

Se quisermos encontrar os descendentes de uma entidade, devemos verificar se a próxima entidade da entidade desejada tem seu **nível geral** maior que o **nível geral** da entidade desejada, se tiver, essa entidade é uma descendente.

Devemos continuar navegando para frente até quando a próxima entidade tiver o mesmo **nível geral** da entidade desejada ou se a expressão não tiver mais entidades. 

Por exemplo, se quisermos pegar os descendentes da entidade `F`.

```
                    (A + B + C + ( D + E + ( F + ( G + B + C ) + Y ) + Z ) )
Nível geral:         1   2   2     2   3     3     4   5   5     4     3
```

* A entidade `F` tem o nível geral igual a 3
* **A entidade `G` é a próxima entidade depois de `F` e o seu nível geral é 4, é descendente.**
* **A entidade `B` é a próxima entidade depois de `G` e o seu nível geral é 5, é descendente.**
* **A entidade `C` é a próxima entidade depois de `B` e o seu nível geral é 5, é descendente.**
* **A entidade `Y` é a próxima entidade depois de `C` e o seu nível geral é 4, é descendente.** 
* A entidade `Z` é a próxima entidade depois de `Y`, porém o seu nível é 3, igual ao nível da entidade `F`, portanto não é descendente.

Acabou a expressão e no final teremos as seguintes entidades descendentes: `G`, `B`, `C` e `Y`

### Pesquisando os filhos de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas os filhos da entidade `D`, precisariamos limitar o nível geral dos descendentes á: [nível geral da entidade corrente] + 1

* A entidade `D` tem o nível geral igual a 2
* **A entidade `E` é a próxima entidade depois de `D` e o seu nível geral é 3, é filha de `D`.**
* **A entidade `F` é a próxima entidade depois de `E` e o seu nível geral também é 3, é filha de `D`.**
* As próximas entidades depois de `F` são: `G`, `B`, `C` e `Y`, todas tem níveis maiores que 3, então serão ignoradas.
* **A entidade `Z` é a próxima entidade depois de `Y` e o seu nível geral também é 3, é filha de `D`.**

Acabou a expressão e no final teremos as seguintes entidades descendentes: `E`, `F` e `Z`

### Pesquisando todos os ascendentes de uma entidade

Se quisermos encontrar os ascendentes de uma entidade, devemos verificar se a entidade anterior da entidade desejada tem seu **nível geral** menor que o **nível geral** da entidade desejada, se tiver, essa entidade é uma ascendente. 

Se a entidade anterior for do mesmo nível da entidade deseja, deve-se ignora-la e continuar navegando para trás até encontrar a primeira entidade com o **nível geral** menor que o **nível geral** da entidade desejada. 

Após encontrar a primeira ascendência, deve-se continuar navegando para trás, porém o **nível geral** a ser considerado agora será o da primeira ascendência e não mais da entidade desejada. Esse processo deve continuar até chegar na entidade raiz. 

Por exemplo, se quisermos pegar os ascendentes da entidade `C`.

```
                    (A + B + C + ( D + E + ( F + ( G + B + C ) + Y ) + Z ) )
Nível geral:         1   2   2     2   3     3     4   5   5     4     3
```

* A entidade `C` tem o nível geral igual a 5
* A entidade `B` tem o nível geral igual a 5, não é ascendente.
* **A entidade `G` tem o nível geral igual a 4, é menor, portanto é a primeira ascendente, nesse caso a entidade pai. Agora o nível a ser considerado será o nível 4 e não mais o nível 5.**
* **A entidade `F` tem o nível geral igual a 3, é menor que o nível geral da entidade `G`, portanto é uma ascendente. Agora o nível a ser considerado será o nível 3 e não mais o nível 4.**
* A entidade `E` tem o nível geral igual a 3, não é uma ascendente.
* **A entidade `D` tem o nível geral igual a 2, é uma ascendente. Agora o nível a ser considerado será o nível 2 e não mais o nível 3.**
* A entidade `C` tem o nível geral igual a 2, não é uma ascendente.
* A entidade `B` tem o nível geral igual a 2, não é uma ascendente.
* **A entidade `A` tem o nível geral igual a 1, é uma ascendente. Agora o nível a ser considerado será o nível 1 e não mais o nível 2.**

Acabou a expressão e no final teremos as seguintes entidades ascendentes: `G`, `F`, `D` e `A`

### Pesquisando o pai de uma entidade

Seguindo a lógica da pesquisa acima, para encontrar apenas o pai da entidade `Y`, precisariamos limitar o nível geral dos ascendentes á: [nível geral da entidade corrente] - 1; ou a primeira entidade com o nível geral menor que a entidade desejada.

* A entidade `Y` tem o nível geral igual a 4
* A entidade `C` é a entidade anterior a `Y` e tem o nível geral igual a 5, será ignorada.
* A entidade `B` é a entidade anterior a `C` e tem o nível geral igual a 5, será ignorada.
* A entidade `G` é a entidade anterior a `B` e tem o nível geral igual a 4, será ignorada.
* **A entidade `F` é a entidade anterior a `G` e tem o nível geral igual a 3, portanto, ela é pai da entidade `Y`.**