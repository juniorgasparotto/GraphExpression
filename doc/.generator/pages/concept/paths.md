## Caminhos de entidades <header-set anchor-name="paths" />

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

Ocorrência 1: `A`
Ocorrência 2: `A.A`

Na "ocorrência 2" temos uma relação ciclica, portanto a notação é interrompida quando isso acontece, do contrário teriamos um caminho infinito.

**Caminho da entidade `B`:**

Ocorrência 1: `A.B`
Ocorrência 2: `A.D.B`

### Caminhos cíclicos na expressão <header-set anchor-name="paths-cyclic" />

Quando uma entidade é pai de si mesma, ou uma entidade descendente é pai de alguma entidade ascendente, isso determina que existe uma soma ciclica entre as entidades. Nesse caso, a expressão deve apenas repetir o nome da entidade ascendente, isso é suficiente para saber que existe uma situação ciclica.

Note que o grafo contém duas referências ciclicas:

```
A + A + B + (C + A)
```

* Uma direta (`A + A`): onde a entidade `A` é pai dela mesma.
* Uma indireta (`C + A`): Onde `C` é pai de uma entidade ascendente, no caso a entidade `A`.