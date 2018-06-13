## Pesquisa superficial <header-set anchor-name="search-surface" />

Na **Pesquisa superficial** a técnica usada é a mesma da **Pesquisa profunda**, á única deferença é que na pesquisa superficial não consideramos os caminhos que já foram escritos (ou percorridos). No caso, não usamos a técnica da **desnormalização** para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas não terá a mesma precisão da _Pesquisa profunda_.

Por exemplo, se quisermos retornar todas as ocorrências da entidade `Y`, teriamos a seguinte diferença entre os tipos de pesquisas:

**Expressão:**

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