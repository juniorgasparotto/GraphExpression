### Verificando se uma entidade contém filhos <header-set anchor-name="search-method-has-children" />

Para descobrir se uma entidade contém filhos, verificamos se o seu **nível geral** é maior que o nível geral da **próxima entidade**, se for, essa entidade contém filhos. Essa é a mesma técnica usada no tópico <anchor-get name="search-deep-is-first-at-group-expression" />.

Contudo, não podemos apenas verificar a **ocorrência da entidade**, pois não é garantido que o seu grupo de expressão foi declarado nesse momento. Nesse caso temos duas opções que serão explicadas adiante.

Essa limitação também serve para expressões **desnormalizadas**, isso por que ela não repete grupos de expressões que tem ascendências da entidade corrente.

Por exemplo, se quisermos descobrir se a entidade `A` , que está no índice `#05`, contém filhos:

```
                A + B + ( C + Y ) + (D + A)
General Level:  1   2     2   3      2   3
Index:          0   1     2   3      4   5
```

Essa expressão está **desnormalizada** e a entidade `A` que está no índice `#05` não foi redeclarado para evitar um **caminho cíclico**.

**Opção 1:**

1. Verificar se todas as ocorrências da entidade `A` contém filhos.

Com isso, teríamos um resultado negativo ao analisar a ocorrência da entidade `A` que está no índice `#05` e um resultado positivo ao analisar a ocorrência da entidade `A` que está no índice `#00`.

_Essa opção deve ser evitada se o seu propósito for retornar as entidades filhas, isso porque você retornaria as mesmas entidades em todas as ocorrências, nesse caso utilize a opção abaixo._

**Opção 2:**

1. Aplicar a <anchor-get name="normalization-3" /> para garantir que todas as entidades estão sendo declaradas logo na primeira utilização.
2. Localizar a primeira ocorrência da entidade `A`. Deve-se encontrar a ocorrência que está no índice `#00`.

Com isso, teríamos um resultado positivo ao analisar a ocorrência da entidade `A` que está no índice `#00` e não seria necessário verificar as outras ocorrências.

Esse tema também foi abordado, de forma superficial, no tópico <anchor-get name="entity-declaration" />.