## Convertendo uma matriz de informação para expressões de grafos <header-set anchor-name="implementation-to-expression" />

Nesse exemplo veremos como converter uma matriz de informação de volta para expressão de grafos.

É importante destacar que esse código é simples e específico para o nosso exemplo. Embora ele possa ser útil para diversos propósitos devido a sua capacidade de identificar os momentos corretos de ínicio e fim de uma iteração de uma entidade.

```csharp
[DebuggerDisplay("{Entity.Name}")]
public class EntityItem
{
    private readonly Expression expression;

    public EntityItem(Expression expression)
    {
        this.expression = expression;
    }

    public int Index { get; set; }
    public int IndexAtLevel { get; set; }
    public int Level { get; set; }
    public int LevelAtExpression { get; set; }
    public Entity Entity { get; set; }

    public EntityItem Previous { get => expression.ElementAtOrDefault(Index - 1); }
    public EntityItem Next { get => expression.ElementAtOrDefault(Index + 1); }
    public EntityItem Parent
    {
        get
        {
            var previous = this.Previous;
            while(previous != null)
            {
                if (previous.Level < this.Level)
                    return previous;
                previous = previous.Previous;
            }
            return null;
        }
    }
}
```

* Essa classe será nossa representação de cada linha da matriz de informação, ou seja, cada ocorrência de uma entidade dentro da expressão. Nela teremos todas as propriedades que uma ocorrência de uma entidade pode ter.
* Nas propriedades `Previous`, `Next` e `Parent`, estamos implementando, respectivamente, as técnicas:
    * <anchor-get name="search-deep-get-entity-previous" />
    * <anchor-get name="search-deep-get-entity-next" />
    * <anchor-get name="search-deep-get-entity-parent" />

```csharp
public class Expression : List<EntityItem>
{
    public string ToExpressionAsString()
    {
        var parenthesisToClose = new Stack<EntityItem>();
        var output = "";
        foreach (var item in this)
        {
            var next = item.Next;
            var isFirstInParenthesis = next != null && item.Level < next.Level;
            var isLastInParenthesis = next == null || item.Level > next.Level;
            var isNotRoot = item.Index > 0;

            if (isNotRoot) output += " + ";

            if (isFirstInParenthesis)
            {
                output += "(";
                parenthesisToClose.Push(item);
            }

            output += item.Entity.Name.ToString();

            if (isLastInParenthesis)
            {
                int countToClose;

                if (next == null)
                    countToClose = parenthesisToClose.Count;
                else
                    countToClose = item.Level - next.Level;

                for (var i = countToClose; i > 0; i--)
                {
                    parenthesisToClose.Pop();
                    output += ")";
                }
            }
        }

        return output;
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

        var expression = new Expression();
        expression.Add(new EntityItem(expression) { Entity = A, Index = 0, IndexAtLevel = 0, Level = 1 });
        expression.Add(new EntityItem(expression) { Entity = B, Index = 1, IndexAtLevel = 0, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = C, Index = 2, IndexAtLevel = 1, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = Y, Index = 3, IndexAtLevel = 0, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = D, Index = 4, IndexAtLevel = 2, Level = 2 });
        expression.Add(new EntityItem(expression) { Entity = E, Index = 5, IndexAtLevel = 0, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = F, Index = 6, IndexAtLevel = 1, Level = 3 });
        expression.Add(new EntityItem(expression) { Entity = G, Index = 7, IndexAtLevel = 0, Level = 4 });
        expression.Add(new EntityItem(expression) { Entity = B, Index = 8, IndexAtLevel = 0, Level = 5 });
        expression.Add(new EntityItem(expression) { Entity = C, Index = 9, IndexAtLevel = 1, Level = 5 });
        expression.Add(new EntityItem(expression) { Entity = Y, Index = 10, IndexAtLevel = 1, Level = 4 });
        expression.Add(new EntityItem(expression) { Entity = Z, Index = 11, IndexAtLevel = 2, Level = 3 });
        var expressionString = expression.ToExpressionAsString();
    }
}
```

No método `Main` temos a chamada da nossa função, note que estamos criando a matriz de informação de forma manual. Essa matriz deve representar a seguinte expressão:

```
(A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z))
```

A função `ToExpressionAsString` será responsável por fazer toda a iteração e chegar em nosso objetivo que é devolver uma `string` contendo nossa expressão.

* A classe `Expression` representa uma expressão de grafos como um todo. Ela herda de uma lista do tipo `EntityItem` para fazer jus ao que ela é dentro do conceito: Um conjunto de ocorrências de entidades com suas informações.
* O método `ToExpressionAsString` retorna uma string que será a nossa expressão.
* A lista contendo todas as ocorrências das entidades será percorrida complementamente. Da posição 0 até o final da lista. Cada iteração pode conter diversos níveis da expressão.
* A variável `parenthesisToClose` armazena uma lista de todos os parênteses que foram abertos e precisam ser fechados. A lista tem que estar no formato: último a entrar, primeiro a sair.
* Para cada iteração:
    * Se a entidade for a entidade raiz, não adiciona o sinal de `+`.
        * <anchor-get name="search-deep-is-root" />
    * Se a entidade for a primeira do grupo de expressão, adiciona o caractere `(`
        * <anchor-get name="search-deep-is-first-at-group-expression" />
    * Se a entidade for a última do seu grupo de expressão (última dentro dos parênteses), então feche com o caractere `)`. Como diversos parênteses podem ter sido abertos nas iterações anteriores, então devemos calcular a quantidade de parênteses que precisam ser fechados e fecha-los. A variável `parenthesisToClose` contém a entidade que está sendo fechada, isso pode ser útil para alguma lógica.
        * <anchor-get name="search-deep-has-last-at-group-expression" />

Com esses treixos de códigos vimos como é simples iterar em uma expressão de grafos e entender seus momentos. Além de abrir caminhos para implementações mais completas como: **pesquisa em expressão de grafos.**