## Navegação para a direita (Próxima entidade) <header-set anchor-name="entity-next" />

Toda entidade, com exceção da última da expressão, tem conhecimento da próxima entidade na expressão. 

No exemplo abaixo, temos um mapa de conhecimento de todas as entidades a direita da entidade corrente:

```
A + B + C + ( D + E + ( F + G ) )
B   C   D     E   F     G
```

No exemplo, a entidade `A` tem conhecimento da entidade `B`. Note que a entidade `B` é filha de `A`, mas isso não influência, pois a ideia é conhecer a próxima entidade da expressão e não do seu nível.

