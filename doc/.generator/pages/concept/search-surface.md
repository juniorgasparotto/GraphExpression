### Pesquisa superficial

Na "Pesquisa superficial" a técnica usada é a mesma da "Pesquisa profunda", á única deferença é que na pesquisa superficial não consideramos os caminhos que já foram escritos (ou percorridos). No caso, não usamos a técnica da desnormalização para criar esses novos caminhos. Isso reduz muito o tempo da pesquisa, mas não terá a mesma precisão da "Pesquisa profunda".

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

* Ocorrência 1: A.C.Y
* Ocorrência 2: A.D.F.G.C.Y -> Novo caminho
* Ocorrência 3: A.D.F.G.Y

**Pesquisa superficial:**

Utiliza a expressão original:

* Ocorrência 1: A.C.Y
* Ocorrência 2: A.D.F.G.Y