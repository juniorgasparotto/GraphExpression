## Criando uma matriz de informações a partir de um grafo <header-set anchor-name="implementation-to-matrix" />

No exemplo anterior vimos como gerar uma expressão de grafo a partir de uma matriz de informação manual e que foi representada pela classe `Expression`. 

Nesse exemplo, vamos abordar uma implementação que cria essa matriz de forma automática.

```csharp
public class Expression : List<EntityItem>
{
    public bool Deep { get; }

    public Expression(Entity root, bool deep = true)
    {
        Deep = deep;

        if (root != null)
            Build(root);
    }

    private void Build(Entity parent, int level = 1)
    {
        // only when is root entity
        if (Count == 0)
        {
            var rootItem = new EntityItem(this)
            {
                Entity = parent,
                Index = 0,
                IndexAtLevel = 0,
                LevelAtExpression = level,
                Level = level
            };

            Add(rootItem);
        }
        
        var indexLevel = 0;
        var parentItem = this.Last();

        level++;
        foreach (var child in parent.Children)
        {
            var previous = this.Last();
            var childItem = new EntityItem(this)
            {
                Entity = child,
                Index = Count,
                IndexAtLevel = indexLevel++,
                Level = level,
            };

            Add(childItem);

            // if:   IS 'deep' and the entity already declareted in expression, don't build the children of item.
            // else: if current entity exists in ancestors (to INFINITE LOOP), don't build the children of item.
            var continueBuild = true;
            if (Deep)
                continueBuild = !HasAncestorEqualsTo(childItem);
            else
                continueBuild = !IsEntityDeclared(childItem);

            if (continueBuild && child.Children.Count() > 0)
            {
                childItem.LevelAtExpression = parentItem.LevelAtExpression + 1;
                Build(child, level);
            }
            else
            {
                childItem.LevelAtExpression = parentItem.LevelAtExpression;
            }
        }
    }

    private bool HasAncestorEqualsTo(EntityItem entityItem)
    {
        var ancestor = entityItem.Parent;
        while (ancestor != null)
        {
            if (entityItem.Entity == ancestor.Entity)
                return true;

            ancestor = ancestor.Parent;
        }

        return false;
    }

    private bool IsEntityDeclared(EntityItem entityItem)
    {
        return this.Any(e => e != entityItem && e.Entity == entityItem.Entity);
    }

    public string ToMatrixAsString()
    {
        var s = "";
        s += "Index    | Entity  | Level    | Level Index     | LevelAtExpression \r\n";

        foreach (var i in this)
        {
            s += $"{i.Index.ToString("00")}       ";
            s += $"| {i.Entity.Name}       ";
            s += $"| {i.Level.ToString("00")}       ";
            s += $"| {i.IndexAtLevel.ToString("00")}              ";
            s += $"| {i.LevelAtExpression.ToString("00")} \r\n";
        }
        return s;
    }
}

class Program 
{
    static void Main(string[] args)
    {
        var A = new Entity("A");
        var B = new Entity("B");
        var C = new Entity("C");
        var Y = new Entity("Y");
        var D = new Entity("D");
        var E = new Entity("E");
        var F = new Entity("F");
        var G = new Entity("G");
        var Z = new Entity("Z");
        A = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
        var expression = new Expression(A, false);
        var matrix = expression.ToMatrixAsString();
    }
}
```

O método `ToMatrixAsString` será usado para verificarmos o resultado de nosso exemplo. E após o processamento do grafo da entidade `A`, teremos a seguinte matriz de informação:

```
Index    | Entity  | Level    | Level Index     | LevelAtExpression 
00       | A       | 01       | 00              | 01 
01       | B       | 02       | 00              | 02 
02       | C       | 03       | 00              | 03 
03       | A       | 04       | 00              | 03 
04       | A       | 03       | 01              | 02 
05       | D       | 02       | 01              | 02 
06       | D       | 03       | 00              | 02 
07       | E       | 03       | 01              | 02 
08       | F       | 03       | 02              | 03 
09       | G       | 04       | 00              | 04 
10       | A       | 05       | 00              | 04 
11       | C       | 05       | 01              | 04 
12       | Y       | 04       | 01              | 03 
13       | Z       | 03       | 03              | 02 
14       | G       | 02       | 02              | 01
```

* A classe recebe em seu construtor a **entidade raiz**. A partir dessa instância, vamos navegar em seu grafo por completo.
* O parâmetro `Deep` determina se a varredura será profunda ou não e que foi explicado no tópico <anchor-get name="search-deep" />
* O primeiro `if` dentro da função `Build` verifica se é a entidade raiz, se for, devemos criar o primeiro item. Nesse ponto, as informações são fixas, uma vez que por ser a entidade raiz, serão os valores inicias.
* Na segunda parte da função, iniciamos a leitura dos filhos da entidade `parent`. 
* Será incrementado `+1` no **nível geral** conforme se aprofunda na entidade. Esse valor é passado por parâmetro, pois ele transcende todo o grafo.
* Será incrementado `+1` no **índice do nível**. Esse valor está fechado apenas no escopo do `foreach`, ou seja, apenas para os filhos da entidade.
* Para cada interação, é verificado se a propriedade `Deep` é `true`, se for, devemos manter a navegação mesmo se entidade corrente já foi percorrida por completo em algum momento da expressão. Contudo, a única situação que limita a continuação é se a entidade corrente tiver relações com ela mesma em um de seus ascendentes. Se tiver, é interrompida a continuação.
* Se a propriedade `Deep` for `false`, então devemos apenas verificar se a entidade já foi percorrida em algum momento da expressão, se foi, então não continuamos.
* A propriedade `LevelAtExpression` (**nível da expressão**) é preenchida com o **nível de expressão** da entidade pai somando-se `+1` quando a entidade tiver filhos e não somando nada quando não tiver.

Com isso, concluímos os três principais exemplos do conceito e que podem ser base para implementações mais complexas como a **pesquisa em expressão de grafo**.