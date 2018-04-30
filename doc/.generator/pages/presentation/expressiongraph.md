# Expression Graph <header-set anchor-name="presentation" />

O conceito `Expression graph` foi criado em 2015 por Glauber Donizeti Gasparotto Jr e tem como objetivo a representação de um grafo em forma de expressão.

Imagine o seguinte grafo:

```
A 
    B
        C
        D
            B
    E
        A
```

A sua representação em forma de expressão seria:

```
A + (B + C + (D + B)) + (E + A)
```

**Entidade Raiz**

A entidade raiz é a primeira da expressão, ela que dá origem a toda cadeia de conexões do grafo. Por exemplo:

`A + B`

Nesse exemplo, a entidade raiz é o `A`

**Entidades filhas**

As entidades filhas são somadas a entidade raiz usando o sinal `+` do grupo de conexão

**Sinal de `+`**

O sinal de `+` representa uma conexão entre as entidades. A entidade da esquerda determina que ela contém uma conexão com a entidade da direita. Por exemplo:

`A + B`

Isso significa que a entidade `A` faz conexão com a entidade `B`.

**Parenteses**

Os parenteses representam um novo grupo de conexões

**Referencias ciclicas:**

Note que o grafo contém duas referencias ciclicas:

`A -> E` e `E -> A`
`B -> D` e `D -> B`

Quando isso ocorre, basta repetir o item sem precisar escrever toda a sua cadeia novamente.



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