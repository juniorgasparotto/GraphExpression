## Pesquisa superficial <header-set anchor-name="search-surface" />

Na **Pesquisa superficial** não consideramos os caminhos que já foram declarados (ou percorridos), ou seja, não usamos a técnica da **desnormalização** para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas em alguns casos não terá a mesma precisão da _Pesquisa profunda_.

Por exemplo, se quisermos retornar todas as ocorrências da entidade `Y`, teríamos a seguinte diferença entre os tipos de pesquisas:

_Expressão de exemplo:_

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
```

**Pesquisa profunda:**

Primeiro, aplica-se a desnormalização:

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
```

* _Ocorrência 1_: A.C.Y
* _Ocorrência 2_: A.D.F.G.C.Y -> Novo caminho
* _Ocorrência 3_: A.D.F.G.Y

**Pesquisa superficial:**

Utiliza a expressão original:

* _Ocorrência 1_: A.C.Y
* _Ocorrência 2_: A.D.F.G.Y