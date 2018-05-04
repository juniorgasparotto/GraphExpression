# Expression Graph <header-set anchor-name="presentation" />

# Conceito <header-set anchor-name="concept" />

O conceito `Expression graph` foi criado em 2015 por Glauber Donizeti Gasparotto Jr e tem como objetivos a representação de um grafo em forma de expressão e a pesquisa e navegação de uma entidade dentro da expressão.

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

**Elementos de uma expressão de grafos**

Primeiro, vamos listar os elementos de uma expressão:

* Entidade: É o elemento fundamental da expressão, determina uma unidade, um vértice no conceito de grafo.
* Operador de soma `+`: É o elemento que adiciona uma entidade em outra entidade.
* Parenteses `(` e `)`: São usados para determinar um grupo de uma determina entidade.

**Objetivo do resultado final**

O objetivo de uma expressão de grafos é bem diferente do objetivo de uma expressão matemática. Na matemática, uma expressão nesses moldes teria números como sendo as entidades e após o processamento da expressão o resultado final seria outro número com a soma de todos os outros.

Em expressão de grafos, o objetivo final é gerar um grafo completo a partir de uma expressão, ou a partir de um grafo, fazer a engenharia reversa para obter sua representação em forma de expressão. 

**Ordem de resolução**

A resolução é sempre da esquerda para a direita, onde a entidade da esquerda adiciona a entidade da direita e o resultado dessa soma é a propria entidade da esquerda e assim sucessivamente até chegar no final. Por exemplo:

Exemplo simples (Etapas simbólicas da resolução):

1. `(A + B)`
2. Resultado final: `A`

Exemplo composto (Etapas simbólicas da resolução):

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Resultado final: `A`

É obvio que a cada etapa da resolução a entidade da esquerda é alterada internamente, ela adiciona a entidade da direita, o que importa aqui é entender a ordem de resolução.

**Grupos de expressão**

Os grupos são delimitados pelo uso de parenteses: `(` para abrir e `)` para fechar. 

A primeira entidade do grupo determina a entidade pai daquele grupo de expressão. Ou seja, todas as entidades subsequentes serão filhas da entidade pai do grupo.

Não é obrigatório o uso dos parenteses no primeiro grupo de expressão. Ou seja, no exemplo acima, a expressão abaixo também é considerada correta:

``` 
A + (B + C + (D + B)) + (E + A)
```

**Entidade Pai** e **Entidades filhas**

A entidade pai é a primeira do grupo de expressão, ela que dá origem ao grafo daquele grupo. Por exemplo:

`(A + B + C + D)`

Nesse exemplo, a entidade pai é o `A` e as entidades subsequentes serão suas filhas: `B`,`C` e `D`.

As entidades filhas são adicionadas a entidade pai usando o sinal `+`.

**Sub-grupos de expressão**

Um grupo de expressão pode conter outros grupos de expressão dentro dele e a lógica será a mesma para o sub-grupo:

`A + B + (C + D)`

Nesse exemplo a entidade `A` será pai das entidades `B` e `C`. A entidade `C` será pai da entidade `D`.

**Somas ciclicas:**

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe uma soma ciclica entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação ciclica.

Note que o grafo contém duas referencias ciclicas: 

```
A + A + B + (C + A)
```

Uma direta: `A + A` onde a entidade `A` é pai dela mesma.
Uma indireta: Onde `C` é pai de uma entidade ascendente, no caso a entidade: `A`.

**Repetições de grupo de expressão**

Um grupo de expressão não precisa ser redeclarado na próxima vez que a entidade pai do grupo for utilizada. Por exemplo: 

```
A + B + (C + D + E) + (I + C)
```

* Note que a entidade `C` foi declarada após a entidade `B` e ambas são filhas da entidade `A`. 
* A entidade `C` tem os filhos `D` e `E`
* A entidade `I` tem como filha a entidade `C`, porém não é necessário redeclarar as entidades filhas de `C`.
* Contudo, não existe problemas conceituais se existir a repetição, apenas uma poluição visual, por exemplo:
    
```
    A + B + (C + D + E) + (I + (C + D + E))
```

**Entidade Raiz**

A primeira entidade do primeiro grupo de expressão é a entidade raiz da expressão. Uma expressão só pode conter uma entidade raiz.

```
(A + A + B + (C + A))
```

A entidade `A` é a entidade raiz de toda expressão acima.

**Níveis**

Uma expressão tem dois tipos de níveis: "Nível geral" e "Nível na expressão".

O "nível geral" determina em qual nível está com relação a hierarquia do grafo. Por exemplo:

```
A (Nível 1)
----B (Nível 2)
    ----C (Nível 3)
    ----D (Nível 3)
        ----B (Nível 4)
----E (Nível 2)
    ----A (Nível 3)
```

O "nível na expressão" determina em qual nível a entidade está com relação a expressão. Por exemplo:

```
                    A + B + C + ( D + E + ( F + G ) )
Nível na expressão: 1   1   1     2   2     3   3    
Nível geral:        1   2   2     2   3     3   4   
```

Note que o nível da expressão ignora o nível da entidade na hieraquia, é uma informação útil apenas para a expressão.

**Índices**

Uma expressão tem dois tipos de índices: "Índice na expressão" e "Índice do nível".

O "Índice da expressão" determina em qual posição a entidade está na expressão. O índice inicia em zero e soma-se +1 até a última entidade da expressão. Por exemplo:

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

* A entidade `A` é a raiz da expressão, sendo assim, ela será o primeiro nível e seu "índice no nível" terá somente a posição zero que é a propria entidade `A`.
* A entidade `B` é a primeira do segunda nível e terá a posição zero. Ela é filha da entidade `A`.
* A entidade `C` e a segunda do segundo nível e terá a posição 1. Ela é filha da entidade `A`.
* A entidade `D` e a terceria do segundo nível e terá a posição 2. Ela é filha da entidade `A`.
* A entidade `E` e a primeira do terceiro nível e terá a posição 0. Ela é filha da entidade `D`.
* A entidade `F` e a segunda do terceiro nível e terá a posição 1. Ela é filha da entidade `D`.
* A entidade `G` e a primeira do quarto nível e terá a posição 0. Ela é filha da entidade `F`.

**Pesquisa e navegação**

Uma entidade conhece as entidades que estão ao seu redor, ou seja, aquelas que estão posicionadas na sua esquerda ou na sua direita na expressão independentemente do nível. Essas ligações possibilitam a pesquisa e navegação de entidades.

Navegação para a direita da entidade (Próxima):

Toda entidade, com exceção da última da expressão, tem conhecimento da próxima entidade na expressão. No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a direita da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) )
B   C   D     E   F     G
```

No exemplo, a entidade `A` tem conhecimento da entidade `B`. Note que a entidade `B` é filha de `A`, mas isso não influência, pois a ideia é conhecer a próxima entidade da expressão e não do seu nível.

Navegação para a esquerda (Anterior)

Toda entidade, com exceção da primeira da expressão (a entidade raiz), tem conhecimento da entidade anterior na expressão. No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a esquerda da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) ) 
    A   B     C   D     E   F
```

## Build Status

<table>
    <tr><th>netstandard2.0+</th><th>net461+</th></tr>
    <tr>
        <td>

[![Build status](https://ci.appveyor.com/api/projects/status/6hb2sox6y6g5pwmt/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoSanches/syscommand-bg4ki/branch/master)
        </td>
        <td>
        
[![Build status](https://ci.appveyor.com/api/projects/status/36vajwj2n93f4u21/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoSanches/syscommand/branch/master)
        </td>
    </tr>
</table>

A partir da versão 2.0.0, apenas os novos frameworks serão suportados, veja abaixo a tabela de suporte:

<table>
<tr>
<th>Frameworks</th>
<th>Versão compatível</th>
<th>Notas da versão</th>
</tr>
<tr>
<td>netstandard2.0, net461</td>
<td>

[2.0.0-preview2](https://www.nuget.org/packages/SysCommand/2.0.0-preview2)
</td>
<td>

[notes](https://github.com/juniorgasparotto/SysCommand/releases/tag/2.0.0)
</td>
</tr>  
<tr>
<td>netstandard1.6, net452</td>
<td>

[1.0.9](https://www.nuget.org/packages/SysCommand/1.0.9)
</td>
<td>

[notes](https://github.com/juniorgasparotto/SysCommand/releases/tag/1.0.9)
</td>
</tr>
</table>

## Canais

* [Reportar um erro](https://github.com/juniorgasparotto/SysCommand/issues/new)
* [Mandar uma mensagem](https://syscommand.slack.com/)
* <anchor-get name="donate" />