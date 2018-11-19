## Caminhos <header-set anchor-name="paths" />

Toda entidade contém um caminho que deve ser percorrido até chegar em sua posição. Para representar esse caminho podemos usar a seguinte notação:

```
A.B.C.D
```

Essa notação indica a localização da entidade "D" dentro da expressão abaixo:

```
A + (B + (C + D))
```

* A entidade "D" é filha da entidade "C"
* A entidade "C" é filha da entidade "B"
* A entidade "B" é filha da entidade "A"

A notação utiliza o caractere "." entre a entidade pai e a entidade filho. A entidade da esquerda será a pai e a entidade da direita será o filho.

**Outras exemplos:**

_Expressão:_

```
(A + A + (B + C) + (D + B))
```

_Caminhos da entidade `A`:_

* _Ocorrência 1_: `A`
* _Ocorrência 2_: `A.A`

Na segunda ocorrência temos uma relação cíclica, portanto a notação é interrompida quando isso acontece, do contrário teríamos um caminho infinito.

_Caminhos da entidade `B`:_

* _Ocorrência 1_: `A.B`
* _Ocorrência 2_: `A.D.B`

### Caminhos cíclicos <header-set anchor-name="paths-cyclic" />

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe um caminho cíclico entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é o suficiente para descobrir que existe uma situação cíclica.

Note que o grafo contém dois caminhos cíclicos:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade "A" é pai dela mesma.
* Uma indireta (`C + A`): Onde "C" é pai de uma entidade ascendente, no caso a entidade "A".