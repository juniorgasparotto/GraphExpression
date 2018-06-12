## Ocorrências de entidades <header-set anchor-name="entity-occurrence" />

Em um grafo, as entidades são únicas, porém elas podem estar em vários lugares ao mesmo tempo. Por exemplo, não existem duas entidades com o mesmo nome. Mas a mesma entidade pode aparecer em diversos pontos no grafo. 

```
(A + (B + C + A) + C)
```

Note que na expressão acima as entidades `A` e `C` estão repetidas. Elas representam a mesma entidade, porém em posições diferentes. Cada ocorrência contém algumas informações que são únicas daquela posição como: _Entidade_, _Índice_, _Nível_, _Navegação_.