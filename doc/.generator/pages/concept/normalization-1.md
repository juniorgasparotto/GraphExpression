## Normalização - tipo 1 <header-set anchor-name="normalization-1" />

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

