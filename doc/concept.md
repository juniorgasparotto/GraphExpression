[
![Inglês](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/GraphExpression)
[
![Português](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/GraphExpression/blob/master/readme-pt-br.md)

# <a name="concept" />Expression of graphs

The concept of **graph expression** was created in 2015 by _Glauber Donizeti Gasparotto Junior_ and aims at the representation of a graph in the form of a mathematical expression.

The concept aims to explore the benefits of a mathematical expression swapping numbers for entities. With this, we can create a new way to carry data and, above all, create a new means of cross research in complex or circular graphs.

It is important to highlight that the concept as a whole is intended to be or performance be better or worse than other existing ones. The goal is to be just a new way to see a graph and its information.

# <a name="index" />Index

* [Comprising a graph expression](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#intro)
  * [Expression resolution](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-execution-order)
  * [Entity and its occurrences](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-and-occurrence)
  * [Sum operator](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#intro-plus)
  * [Subtraction operator](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#intro-subtract)
  * [Expression group](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group)
    * [Root expression group](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group-root)
    * [Sub-expression groups](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-sub-group)
    * [Entity declarations](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-declaration)
    * [Repetitions of expression group](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group-repeat)
    * [Parent entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-parent)
  * [Root entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-root)
  * [End entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-final)
  * [Paths](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#paths)
    * [Cyclic paths](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#paths-cyclic)
* [Information of an occurrence](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-info)
  * [Levels](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#levels)
  * [Indexes](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#indexes)
  * [Navigation to the right (Next)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-next)
  * [Navigation for the left (former Principal)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-previous)
* [Normalizing expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-formatters)
  * [Standardization-1 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-1)
  * [Standardisation-2 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-2)
  * [Standardization-3 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-3)
* [Denormalizing expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#desnormalization)
* [Research on graph expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search)
    * [Array of information](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-matrix-of-information)
  * [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep)
  * [Superficial research](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-surface)
  * [Unreferenced research](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-without-references)
    * [Finding the root of the expression entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-root)
    * [Finding the "parent entities" of an expression](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-parents)
  * [Search with reference](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-with-references)
    * [Verifying that an entity is the first expression Group (first within the parenthesis)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-check-is-first-at-group-expression)
    * [Verifying that an entity is the last expression Group (last within the parenthesis)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-check-is-last-at-group-expression)
    * [Finding the previous entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-previous)
    * [Finding the next entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-next)
    * [Finding all occurrences of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-occurrences)
    * [Finding all the descendants of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-descendants)
    * [Finding the children of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-children)
    * [Finding all the ascendants of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-ascending)
    * [Meeting the parents of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-parent)
* [Implementations](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation)
  * [Creating graphs with graph expression](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-graph)
  * [Converting an array of information to graph expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-expression)
  * [Creating an array of information from a graph](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-matrix)

# <a name="intro" />Comprising a graph expression

A graph expression consists of 4 basic elements and various information that we detail in this document.

**Expression of graphs-Example:**

```
(A + B + C + D)
```

The elements that make up an expression are:

* **Entity**: it is the fundamental element of expression, determines a unit, a vertex in graph theory.
  * Are unique, but may appear `N` often in expression in different positions.
  * Are represented by a literal, in the above case, the letters: `A` , `B` , `C` and `D` .
* ** `+` Sum operator **: is the element that adds an entity in another entity.
  * Making an analogy with the theory of graphs, the `+` operator can be seen as an **edge**.
* **Subtraction operator `-` **: is the element that removes an entity to another entity.
* **Parentheses `(` and `)` **: are used to determine a group of daughters of entities an entity determines.
  * In graphs are called expression: **expression Group**.

These elements are the same as a mathematical expression, the difference is that in place of numbers we have entities that will be added or removed a. In addition, the goal of the result has its differences.

This expression represents the following graph:

```
A 
----B
----C
----D
```

## <a name="expression-execution-order" />Expression resolution

The resolution is always from left to right, where the entity left adds or removes the entity from the right and the result of this sum is the entity itself on the left, and so on until you reach the end of the expression.

**Simple example (symbolic of the resolution Steps):**

1. `(A + B)`
2. Final result of the expression:`A`

_Final graph of the entity`A`_

```
A 
----B
```

**Example compound (symbolic of the resolution Steps):**

1. `(A + B + C + D)`
2. `(A + C + D)`
3. `(A + D)`
4. Final result of the expression:`A`

_Final graph of the entity`A`_

```
A 
----B
----C
----D
```

We saw that each step resolution of an expression to the right entity disappears and the entity of the left prevails until the entities remain your right.

It is obvious that each step resolution left entity is changed internally, it adds the right entity.

## <a name="entity-and-occurrence" />Entity and its occurrences

In a graph, the entities are unique, but they can be in several places at once. For example, there are no two entities with the same name. But the same entity may appear at various points in the graph.

```
(A + (B + C + A) + C)
```

Note that in the above `A` entities and `C` are repeated. They represent the same entity, but in different positions. Each instance contains some information that are unique to that position. We will see this in the topic [Information of an occurrence](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-info).

## <a name="intro-plus" />Sum operator

The sum operation uses the `+` operator, as stated, it works as an edge that connects a vertex to any other vertex. In graph expression, we say that the left right entity adds and without limitations, for example:

* The entity of the left can add yourself as many times as it takes:

```
Expression: A + A + A + A
Graph:
            A 
            ----A
            ----A
            ----A
```

* The entity `X` can add the entity `Y` and the entity `Y` can add the entity `X` as many times as needed.

```
Expression: X + (Y + X + X) + Y
Graph:
            X 
            ----Y
                ----X
                ----X
            ----Y
```

## <a name="intro-subtract" />Subtraction operator

The subtraction operation uses the `-` operator. In graph expression, we say that the entity of the left removes the entity from the right causing the right entity ceases to be your daughter.

Each subtraction operation only one occurrence will be removed at a time, even if the entity has left more of a daughter of the same entity. For example:

* The entity of the left removes one of the daughters`B`

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B

Graph 2:
            A 
            ----B
            ----B
```

Note that one of the occurrences of the `B` entity was removed from the `A` entity. Based on the same example, if we wanted to remove all occurrences of the `B` entity would have to do the subtraction operation 3 times, which is equivalent to the amount of times that `B` entity exists within the `A` entity.

It is still possible to mix the operations of addition and subtraction.

```
Graph 1:
            A 
            ----B
            ----B
            ----B

Expression: A - B - B - B + (C + Y)

Graph 2:
            A
            ----C    
                ----Y
```

In this example, remove all occurrences of the `B` entity the entity `A` and add a new child `C` that contains the entity `Y` .

## <a name="expression-group" />Expression group

The **groups of expression** is delimited by parentheses: `(` to open and `)` to close.

The first entity of the Group (after opening parentheses) determines the parent entity of that group, that is, all subsequent entities will be their daughters until they close the parentheses.

**Example 1:**

```
(A + B + C)
```

* The entity `A` is the parent entity of your expression and group `B` entity and `C` are their daughters.

**Example 2:**

```
(A + B + (C + D))
```

* The entity `A` is the parent entity of your expression and group `B` entity and `C` are their daughters.
* The entity `C` is the parent entity of your expression and the entity group `D` is your daughter.

### <a name="expression-group-root" />Root expression group

The first group of expression is called the **root expression group**.

It is not mandatory the use of parentheses in the expression. We will see that in the following examples both expression are correct:

```
(A + B)
```

Or

```
A + B
```

### <a name="expression-sub-group" />Sub-expression groups

A group of expression can contain other groups of expression within it and the logic is the same for the Sub-group:

`(A + B + (C + D))`

In this example the entity `A` will be father of `B` entities and `C` `C` entity and will be father of the `D` entity.

### <a name="entity-declaration" />Entity declarations

We call it "**statement**" the first moment in which an entity is written, i.e. your first occurrence.

If this entity contains children should declare all your group of expression at the same time, namely, adding his children within the parentheses.

There is no obligation for the Declaration of the Group of speech be on first occurrence, but this helps to simplify the discovery of some information in a faster way.

For example, to find out if the entity `B` contains children in the following expression, you will need to check all of its instances, as it is not possible to say which events your group of expression was declared.

```
A + B + (C + (B + D)) + B
```

Now, if we know that groups of expressions were written in the first instances, so we can check only the first occurrence of the `B` entity to determine whether it contains or not children:

```
A + (B + D) + (C + B) + B
```

### <a name="expression-group-repeat" />Repetitions of expression group

A group of expression cannot be redeclared the next time the parent entity of the group.

For example:

```
A + B + (C + D + E) + (I + C)
```

* The entity `C` has the children `D` and`E`
* The entity `I` has as the entity `C` , but it is not necessary to redeclare the daughters of entities `C` .

**Wrong:**

```
A + B + (C + D + E) + (I + (C + D + E))
```

### <a name="entity-parent" />Parent entity

The parent entity is the first expression group, she who gives rise to the graph of that group.

For example:

`(A + B + (C + D))`

* In this example, we have two entities father: `A` and `C` .
* The `+` element is used as a symbol for an entity in your father's daughter.

## <a name="entity-root" />Root entity

The first entity of the expression is the **root entity** . An expression can only contain a root entity.

```
A + B + (C + A)
```

* The entity `A` is the entity root of all expression above and will be the top of the graph.

## <a name="entity-final" />End entity

An entity that does not have expression groups in your level is called **the end-entity**. This does not mean that the entity does not have children, see:

**End entity without children:**

```
(A + B + C + (D + E))
```

* `B`Entities, `C` and `E` are final entities.

**End entity with children:**

```
(A + (B + C) + (D + B))
```

* The entity `C` is final and does not contain children
* The last occurrence of the entity `B` , the entity's expression group `D` , also is final, but it contains children.

## <a name="paths" />Paths

All entity contains a path that must be traversed to get to your position. To represent this way we can use the following notation:

```
A.B.C.D
```

This notation indicates the location of the `D` entity within the expression below:

```
A + (B + (C + D))
```

* The entity `D` is the daughter of the entity`C`
* The entity `C` is the daughter of the entity`B`
* The entity `B` is the daughter of the entity`A`

The notation uses the `.` character between the parent entity and the entity daughter. The entity of the left will be the father and the entity of the right will be the daughter.

**Other examples:**

_Expression:_

```
(A + A + (B + C) + (D + B))
```

_Paths of `A` entity:_

* _1 Occurrence_:`A`
* _2 Occurrence_:`A.A`

In the second instance we have a cyclic relationship, so the notation is interrupted when this happens, otherwise we'd have an infinite path.

_Paths of `B` entity:_

* _1 Occurrence_:`A.B`
* _2 Occurrence_:`A.D.B`

### <a name="paths-cyclic" />Cyclic paths

When an entity is father of herself, or a descendant entity is the father of some entity ascendant, that determines that there is a cyclic path between the entities. In this case, the expression must only repeat the name of the ascendant, it is enough to know that there is a cyclical situation.

Note that the graph contains two cyclic paths:

```
A + A + B + (C + A)
```

* A ( `A + A` ): where the entity `A` is the parent herself.
* A hint ( `C + A` ): Where `C` a rising entity in case the entity `A` .

# <a name="entity-info" />Information of an occurrence

An entity may appear several times within an expression and for each of these events have a set of information that will be seen in this topic.

This information is of the utmost importance and we will see examples of this in the topic [Research on graph expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search).

* [Levels](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#levels)
* [Indexes](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#indexes)
* Neighboring entities:
  * [Navigation for the left (former Principal)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-previous)
  * [Navigation to the right (Next)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-next)

## <a name="levels" />Levels

An expression has two types of levels: **General level** and **Level in the expression**.

The **General level** determines what the entity is level with respect the hierarchy of the graph. The level starts at `1` and is incremented `+1` until you get to the last level.

For example:

```
A (Level: 1)
----B (Level: 2)
    ----C (Level: 3)
    ----D (Level: 3)
        ----B (Level: 4)
----E (Level: 2)
    ----A (Level: 3)
```

The **level in the expression** determines in which the entity is level with respect to expression. The level starts at `1` and is incremented `+1` until you get to the last level.

For example:

```
                        A + B + C + ( D + E + ( F + G ) )
Level in expression:    1   1   1     2   2     3   3    
Level:                  1   2   2     2   3     3   4   
```

Note that the _level of expression_ is very similar to _General level_. The only difference is the value of the **parent entity**, the General level this number is always less than the General level of their children and at the level of the expression they are equal.

## <a name="indexes" />Indexes

An expression has two types of indexes: **Index on expression** and **the Index level**.

The **index of the expression** determines what position the entity is about expression. The index starts at `0` and is incremented `+1` until you reach the last entity of the expression.

For example:

```
A + B + C + ( D + E + ( F  + G ) ) 
0   1   2     3   4     5    6
```

The **index of the level** determines what position the entity is in relation to your level. The index starts at `0` and is incremented `+1` until you reach the last peer entity.

For example:

```
                A + B + C + ( D + E + ( F + G + Y ) )
Level:          1   2   2     2   3     3   4   4
Level Index:    0   0   1     2   0     1   0   1

Graph:

A (Level Index: 0)
----B (Level Index: 0)
----C (Level Index: 1)
----D (Level Index: 2)
    ----E (Level Index: 0)
    ----F (Level Index: 1)
        ----G (Level Index: 0)
        ----Y (Level Index: 1)
```

* The entity `A` is the root of the expression and your "index in the level" will be zero. Note that the root entity, she will have no other entities in your level.
* The entity `B` is the first of the second level and have zero position. She is the daughter of the entity `A` .
* The entity `C` is the second of the second level and have the position 1. She is the daughter of the entity `A` .
* The entity `D` is the third of the second level and have the position 2. She is the daughter of the entity `A` .
* The entity `E` is the first of the third level, and will have the position 0. She is the daughter of the entity `D` .
* The entity `F` is the second of the third level, and will have the position 1. She is the daughter of the entity `D` .
* The entity `G` is the first of the fourth level and have the 0 position. She is the daughter of the entity `F` .
* The entity `Y` is the second of the fourth level and will have the position 1. She is the daughter of the entity `F` .

## <a name="entity-next" />Navigation to the right (Next)

All entity, with the exception of the last expression, is aware of the next entity in the expression.

In the example below, we have a map of knowledge to all entities to the right of the current entity:

```
A + B + C + ( D + E + ( F + G ) )
B   C   D     E   F     G
```

In the example, the entity `A` is aware of the entity `B` . Note that the entity `B` `A` 's daughter, but that doesn't influence, because the idea is to know the next entity of the expression and not your level.

## <a name="entity-previous" />Navigation for the left (former Principal)

All entity, with the exception of the first of the expression (the root entity) has knowledge of the previous entity in the expression. In the example below, we have a map of knowledge of all entities on the left of the current entity:

```
A + B + C + ( D + E + ( F + G ) ) 
    A   B     C   D     E   F
```

# <a name="entity-formatters" />Normalizing expressions

The normalizations are designed to improve the visualization of expressions.

## <a name="normalization-1" />Standardization-1 type

The **standardization of type 1** aims to wipe away expression groups that belong to the same parent entity and that are in different places in the expression.

For example:

```
A + (B + Y) + (D + (B + C))
     ^              ^
```

Note that in the expression above, the entity `B` has two groups of expression in different places. In practice, this has no problem, but will be visually better if we apply the standardization by eliminating one of the `B` principal groups, see:

```
A + (B + Y + C) + (D + B)
```

It must be said that no changes in expression must modify the your final graph. It is noticeable that in the example that did not occur, just organizations were reoganizadas.

In the next example, we will see an expression that can lead to confusion at the time of normalization:

```
A + (B + Y) + (D + (B + Y))
     ^              ^
```

In this example, it is natural to think that one of the `B` principal groups can be eliminated because they are equal, but that thinking is wrong. If we eliminate one of the groups, we will be modifying the final graph and this is not the point.

**Wrong:**

```
A + (B + Y) + (D + B)
```

**Correct:**

```
A + (B + Y + Y) + (D + B)
```

## <a name="normalization-2" />Standardisation-2 type

The **standardization of type 2** aims to organize, where possible, [end entities](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-final) at the beginning of your expression group to aid in the visualization of expression.

```
A + (B + (C + D) + E) + F + G
                   ^    ^   ^
```

After the normalization would look like this:

```
A + F + G + (B + E + (C + D))
    ^   ^        ^    
```

* Note that `F` entities and `G` were for the beginning of your group.
* The entity `E` also was reorganized for the start of your group.

## <a name="normalization-3" />Standardization-3 type

The **standardization of type 3** aims to declare as soon as possible all the [groups of expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group). This theme was also discussed in the topic [Entity declarations](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-declaration).

Example:

```
A + B + (C + G + (B + F)) + (G + F)
    ^             ^    
             ^               ^
```

Note that `B` entities and `G` are used before their groups are declared and after standardisation we have:

```
A + (B + F) + (C + (G + F) + B) + G
```

* After normalization, the groups of `B` entities and `G` were declared at the first moment they were used.
* The entity `B` , within the group `C` , and the entity `G` it's lonely at the end of the speech, turned to [End entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-final) and because of this, we can apply [Standardisation-2 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-2) to improve viewing, see:

```
A + G + (B + F) + (C + B + (G + F))
```

* Note that now the entity `G` that was at the end of the expression has been moved to the beginning. Therefore, we must apply again to [Standardization-3 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-3):

```
A + (G + F) + (B + F) + (C + B + G)
```

With this we conclude the normalization and got up an expression more readable.

# <a name="desnormalization" />Denormalizing expressions

The goal of **denormalization** is to generate a new expression where the [groups of expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group) are redeclarados every time your parent entity is used.

After the denormalization will be impossible to go back to the original expression, this is a one-way street.

Consider the following expression:

```
A + (B + D) + (E + B)
```

* Note that the entity `B` has two parents: `A` and`E`
* After the denormalization will have the following expression:

```
A + (B + D) + (E + (B + D))
                    ^
```

* After the denormalization the entity `B` had your expression group redeclared when was used again as daughter of the entity `D` .

As stated, it is impossible to go back to the original expression, because we can't distinguish which groups were expressions of the original expression. Therefore, we cannot say that an _original expression_ is equal to your _expression denormalized_.

See an example of how they are different:

```
Original:       A + (B + D) + (E + B)
Final Graph:
                A
                ---B
                ------D
                ---E
                ------B
```

If we take the expression denormalized and extract the your graph, we will have a graph differs from the original graph:

```
Original:                       A + (B + D) + (E + (B + D))
After normalization of type 1:  A + (B + D + D) + (E + B)
Final Graph:
                                A
                                ---B
                                ------D
                                ------D
                                ---E
                                ------B
```

Therefore, we cannot consider that a denormalized expression is used as an original expression, this changes the final graph. Besides, she breaks rule topic [Repetitions of expression group](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#expression-group-repeat).

# <a name="search" />Research on graph expressions

Research on expression of graphs can be divided into two types: **superficial Research** and **deep Research**.

The next few topics we will address the difference between these types of searches, but first, you must understand what a **matrix of information** that is the common theme between the two types of research.

### <a name="search-matrix-of-information" />Array of information

We can represent an expression of graphs in a vertical array with all information of an expression.

With the vision in the form of array we got a better view of the graph and understand better how does research on complex graphs using the concept of graph expression.

Let's see an example:

**Expression:**

```
Expression:     A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
Level:          1   2     2   3       2   3     3     4   5   5     4     3
Level Index:    0   0     1   0       2   0     1     0   0   1     1     2
```

**Hierarchy:**

```
A (Level Index: 0)
----B (Level Index: 0)
----C (Level Index: 1)
    ----Y (Level Index: 0)
----D (Level Index: 2)
    ----E (Level Index: 0)
    ----F (Level Index: 1)
        ----G (Level Index: 0)
            ----B (Level Index: 0)
            ----C (Level Index: 1)
        ----Y (Level Index: 1)
    ----Z (Level Index: 2)
```

**<a name="sample-matrix" />Array of information:**

```
Index   | Entity | Level | Level Index
#00     | A      | 1     | 0
#01     | B      | 2     | 0
#02     | C      | 2     | 1
#03     | Y      | 3     | 0
#04     | D      | 2     | 2
#05     | E      | 3     | 0
#06     | F      | 3     | 1
#07     | G      | 4     | 0
#08     | B      | 5     | 0
#09     | C      | 5     | 1
#10     | Y      | 4     | 1
#11     | Z      | 3     | 2
```

Note that the expression changed from _horizontal_ to _vertical orientation_ and all entities were stacked on each other and respecting the same order as they had in the expression.

Indeed, this is an important rule: _Never change the order of the rows, that completely changes the graph._

The _elements of sum_ and _parentheses_ were removed, they are not required in the array, since only with the _indexes_ and information _levels_, it is possible to identify all the _groups of expressions_.

And is based on this array of information and the fact of the entities meet their _neighbors_, namely, those that are positioned on your left or your right, regardless of the level, you can create media searches and navigations.

## <a name="search-deep" />Deep search

The **deep search** is intended to return the largest possible amount of results and that she considers all paths that an entity traverses in a graph.

In order to create a _deep research_, we need to use a **denormalized expression**. This is necessary, because only the denormalized expression contains all paths that an entity has in the graph since the original version of the expression does not repeat the expression groups (and nor should it).

Let's follow the same example used in the topic [Array of information](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-matrix-of-information), but now, the expression was denormalized:

**Expression:**

```
Original:       A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
                          ^                                   ^
Denormalized:   A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
                                                                    ^
Level:          1   2     2   3       2   3     3     4   5     5   6       4     3 
Level Index:    0   0     1   0       2   0     1     0   0     1   0       1     2
```

**Hierarchy:**

```
A (Level Index: 0)
----B (Level Index: 0)
----C (Level Index: 1) 
    ----Y (Level Index: 0)
----D (Level Index: 2)
    ----E (Level Index: 0)
    ----F (Level Index: 1)
        ----G (Level Index: 0)
            ----B (Level Index: 0)
            ----C (Level Index: 1)
                ----Y (Level Index: 0) *
        ----Y (Level Index: 1)
    ----Z (Level Index: 2)
```

* Was applied to denormalization and the entity `C` had your group of expression within the `G` entity redeclared.
* After the denormalization a new path was created for the entity `Y` :
  * Before:
    * _Occurrence 1_: Bc Y
    * _2 Occurrence_: A.D.F.G. Y
  * After:
    * _Occurrence 1_: Bc Y
    * **_2 Occurrence_: A.D.F.G.C. Y**
    * _3 Occurrence_: A.D.F.G. Y

**<a name="sample-matrix-desnormalizated" />Denormalized Array:**

See how was the denormalized expression in the form of array:

```
Index   | Entity | Level | Level Index
#00     | A      | 1     | 0 
#01     | B      | 2     | 0 
#02     | C      | 2     | 1 
#03     | Y      | 3     | 0 
#04     | D      | 2     | 2 
#05     | E      | 3     | 0 
#06     | F      | 3     | 1 
#07     | G      | 4     | 0 
#08     | B      | 5     | 0 
#09     | C      | 5     | 1 
#10     | Y *    | 6     | 0
#11     | Y      | 4     | 1 
#12     | Z      | 3     | 2 
```

* A new line was created with respect to original version: the `#10` line contains the new path.

## <a name="search-surface" />Superficial research

On **superficial Research** do not consider the paths that have already been declared (or driven), i.e. don't use the technique of **denormalization** to create these new paths. This greatly reduces the time of the survey, but in some cases will not have the same accuracy of _deep Research_.

For example, if we want to return all occurrences of the `Y` entity, we would have the following difference between the types of searches:

_Example expression:_

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + C ) + Y ) + Z )
```

**Deep search:**

First, apply the denormalization:

```
A + B + ( C + Y ) + ( D + E + ( F + ( G + B + ( C + Y ) ) + Y ) + Z )
```

* _Occurrence 1_: Bc Y
* _2 Occurrence_: A.D.F.G.C. Y-> new path
* _3 Occurrence_: A.D.F.G. Y

**Surface search:**

Uses the original expression:

* _Occurrence 1_: Bc Y
* _2 Occurrence_: A.D.F.G. Y

## <a name="search-without-references" />Unreferenced research

The **unreferenced search** search find entities or information within the array of information.

In this type of research we have no entity as a reference and the search will be made throughout the array according to the need.

As there are endless research option within a graph, we will discuss just a few examples of _unreferenced research_.

### <a name="search-find-root" />Finding the root of the expression entity

To find the **root entity** of the expression, we need to return the entity that has the **General index** equal `0` .

**Attention:** This search shows no differences between the two types of research: **Research deep** and **superficial Research**.

Based on the following expression, we can affirm that the entity `A` is the **entity root** of the expression.

```
        A + B + C
Index:  0   1   2
```

### <a name="search-find-parents" />Finding the "parent entities" of an expression

To find all **parent entities** of the graph, we must apply the following procedure:

1. Retrieve the **previous entities** of all entities whose **level index** is equal to `0` .
2. For each line found, return to your **previous entity** that will always be a **parent entity**.

**Attention:** This search can be done using the two types of research: **Research deep** and **superficial Research**. However, the _deep research_ can return duplicate entities in cases of groups of expressions that were redeclarados and it will be necessary to remove the duplication.

**Deep search**

We'll use in this example the [denormalized array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix-desnormalizated) of topic about [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep).

1. First, we find all rows with the index of the level equal to zero:
  * `#00 (A)`
  * `#01 (B)`
  * `#03 (Y)`
  * `#05 (E)`
  * `#07 (G)`
  * `#08 (B)`
  * `#10 (Y)`
2. For each line found, return to your previous entity that will be a parent entity:
  * `NULL`-> `#00 (A)` : contains no previous entity, therefore does not return anything.
  * `#00 (A)`-> `#01 (B)` : Returns the entity `A` as your previous
  * `#02 (C)`-> `#03 (Y)` : Returns the entity `C` as your previous
  * `#04 (D)`-> `#05 (E)` : Returns the entity `D` as your previous
  * `#06 (F)`-> `#07 (G)` : Returns the entity `F` as your previous
  * `#07 (G)`-> `#08 (B)` : Returns the entity `G` as your previous
  * `#09 (C)`-> `#10 (Y)` : Returns the entity `C` as your previous

With that, after we remove the repetitions (in this case, the entity `C` that appears in the lines `#2` and `#09` ), we obtain as a result end `A` entities, `D` `F` and `C` ,, `G` as the only entities with children in the expression.

**Superficial research**

The logic is the same as the **deep search**, however we will not have duplication because on _superficial research_ there are no groups of repeated expressions.

## <a name="search-with-references" />Search with reference

The **reference research** assumes that the _entity_ or one of its _instances_ have been found and on that basis we can take actions such as: _checks_, _navigations_ or research in their _ascendants_ and _descendants_.

As there are endless research option using an entity, we will discuss just a few examples of _research with references_.

### <a name="search-check-is-first-at-group-expression" />Verifying that an entity is the first expression Group (first within the parenthesis)

To find out if an entity is the first of your group (first within the parentheses), check if your **General level** is lower than the General level of the **next entity**, if it is, it is the first of your group of expression.

**Attention:** This search shows no differences between the two types of research: **Research deep** and **superficial Research**.

```
        A + B + ( C + Y ) + (D + C)
                  ^
Level:  1   2     2   3      2   3
Index:  0   1     2   3      4   5
```

In the above example, the entity `C` , `#02` index, has the same general level `2` and your next `Y` entity has the same general level `3` , therefore, she is the first inside your parentheses.

**Note:**

Do not confuse this technique as being the solution to verify that an entity contains children. We will see this in the topic [Finding all the descendants of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-descendants).

### <a name="search-check-is-last-at-group-expression" />Verifying that an entity is the last expression Group (last within the parenthesis)

To find out if an entity is the last of your group (last inside the parentheses), check whether your **General level** is greater than the General level of the **next entity**, if it is, it is the last of your group.

**Attention:** This search shows no differences between the two types of research: **Research deep** and **superficial Research**.

```
        A + B + ( C + Y ) + (D + C) + U
                      ^
Level:  1   2     2   3      2   3    2
Index:  0   1     2   3      4   5    6
```

In the above example, the entity `Y` , `#03` index, has the same general level `3` and your next `D` entity has the same general level `2` , therefore, she is the last inside your parentheses.

* The `U` `#06` content entity does not have a next entity, so she is the last of your expression group, even though he is omitted because we're in the **root expression group**.

### <a name="search-find-previous" />Finding the previous entity

To return the previous entity of a given entity, we must subtract the **General index** in your `-1` .

**Attention:** This search shows no differences between the two types of research: **Research deep** and **superficial Research**.

We'll use in this example the [denormalized array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix-desnormalizated) of topic about [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep).

1. To obtain the previous entity `Y` -entity of the line `#03` , we got your general index ( `3` ), and subtract `-1` . With the result ( `2` ), found in the array, the entity that is in that position, in this case, one goes back to `C` entity.

```
Index   | Entity | Level | Level Index
#02     | C      | 2     | 1 
#03     | Y      | 3     | 0 
```

* If the result is less than zero, is because we're in the **root entity** and there is no previous authority.

### <a name="search-find-next" />Finding the next entity

To return the next entity of a given entity, we must add the **General index** in your `+1` .

**Attention:** This search shows no differences between the two types of research: **Research deep** and **superficial Research**.

We'll use in this example the [denormalized array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix-desnormalizated) of topic about [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep).

1. To get the next `Y` line entity entity `#03` , we take your general index ( `3` ) and add `+1` . With the result ( `4` ), found in the array, the entity that is in that position, in this case, one goes back to `D` entity.

```
Index   | Entity | Level | Level Index
#03     | Y      | 3     | 0 
#04     | D      | 2     | 2 
```

* If the result is greater than the maximum quantity of items in the array is the last expression entity and there is no next entity.

### <a name="search-find-occurrences" />Finding all occurrences of an entity

To find all occurrences of an entity, we have to traverse the entire array from `0` index to last position in the array.

**Attention:** This search can be done using the two types of research: **Research deep** and **superficial Research**. However, the _deep search_ may return a larger quantity of occurrences. This is why in this kind of research groups of expressions are redeclarados.

Therefore, it is recommended to use the **deep search** if your need is to obtain the largest possible number of paths.

**Deep search**

We'll use in this example the [denormalized array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix-desnormalizated) of topic about [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep).

1. If we want to get all occurrences of the `Y` entity within the graph, we would find the lines:
  * `#03 (Y)`
  * `#10 (Y)`: This occurrence is derived from **denormalization**.
  * `#11 (Y)`

**Superficial research**

The logic is the same as the **deep search**, however we will not have any occurrences resulting from redeclarações of expression.

We'll use in this example the [original array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix) of topic about [Array of information](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-matrix-of-information).

1. If we want to get all occurrences of the `Y` entity within the graph, we would find the lines:
  * `#03 (Y)`
  * `#10 (Y)`
* Note that was found an occurrence unless on _deep research_.

### <a name="search-find-descendants" />Finding all the descendants of an entity

If we want to find the descendants of an entity, we must verify that your **General level** is lower than the General level of the **next entity**, if it is, that entity is a descendant of the current entity. This is the same technique used in the topic [Verifying that an entity is the first expression Group (first within the parenthesis)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-check-is-first-at-group-expression).

Should we continue navigating forward until the next entity has the **General level** equal or lower the **General level** of the current entity or if the expression does not have more entities.

**Attention:** This search can be done using the two types of research: **Research deep** and **superficial Research**. However, there are different approaches for each of them. In addition, we should have a special treatment for entities containing an ascendant of the entity itself, namely a **cyclic path**.

**Entity with cyclic path:**

We must take some care to find the descendants of entities with cyclic paths. This is because the groups of expressions cannot be redeclarados in these situations.

For example, how can we find the descendants of the entity `A` that are in the index `#05` ?

```
        A + B + (C + Y) + (D + A + C)
                               ^
Level:  1   2    2   3     2   3   3
Index:  0   1    2   3     4   5   6
```

* The entity `A` that is in the `#05` index has not been restated to avoid a **cyclic path**.
* Note that the entity `A` contains descendants (is the root entity), but it's impossible to figure out that if we analyze the occurrence `#05` index your.

The answer would be:

* Find all occurrences of the `A` entity.
* Among the occurrences found, we must find and use the first one that has descended and ignore the others.
  * _1 Occurrence_:
    * `#00`: The entity `A` has the General level equal to `1` .
    * `#01`: **The entity `B` is the next entity after `A` and your General level is `2` , is descended**.
    * Ready! We found the occurrence which has the Declaration of the Group of the `A` entity.
  * _2 Occurrence_:
    * `#05`: You do not need to check the second occurrence of the `A` entity, because we've found the your statement.
* Return the descendants of the entity `A` of the `#00` index:
  * `#00`: The entity `A` has the General level equal to `1` .
  * `#01`: **The entity `B` is the next entity after `A` and your General level is `2` , is descended**.
  * `#02`: **The entity `C` is the next entity after `B` and your General level is `2` , is descended**.
  * `#03`: **The entity `Y` is the next entity after `C` and your General level is `3` , is descended**.
  * `#04`: **The entity `D` is the next entity after `Y` and your General level is `2` , is descended**.
  * `#05`: **The entity `A` is the next entity after `D` and your General level is `3` , is descended**.
  * `#06`: **The entity `C` is the next entity after `A` and your General level is `3` , is descended**.
  * The term ended.
  * The following were found: `A, B, C, Y, D, A, C` .
* Remove occurrences that are duplicated:`C`
* Return the result:`A, B, C, Y, D, A`

**Deep search**

If an entity does not have a **cyclic path**, we can simply continue the search for descendants of the current occurrence, since it is guaranteed that your group of expression was redeclared.

**Superficial research**

On superficial research should have some care. Note that in the expression below in a scenario very similar to the **entities with cyclic paths**.

For example, how can we return the descendants of `C` entity `#02` index?

```
        A + B + C + (D + A + (C + Y)) + Z
                ^              
Level:  1   2   2    2   3    3   4     2
Index:  0   1   2    3   4    5   6     7
```

* The entity `C` that is in the `#02` index was not restated because we are using the superficial research.
* This expression is not **normalized**, the entity `C` should have been declared as soon as possible, but this did not occur.
* The entity `C` contains descendants. His expression is declared in the `#05` index.

In this case we have two options:

**Option 1:**

Using the same logic that was explained to **entities with cyclic paths**. With that evaluates all occurrences of the `C` entity until we find the occurrence that declares the Group of your expression.

* Would be found the occurrence `#05` index and the occurrence of the `#02` index would be discarded.
* Now that we found the correct occurrence, we should return the descendants:
  * `#05`: The entity `C` has the General level equal to `3` .
  * `#06`:**The entity `Y` is the next entity after `C` and your General level is `4` , is descended**.
  * `#07`: The entity `Z` is the next entity after `Y` and your General level is `2` , she's not a descendant.
  * The expression did not finish, but was interrupted after the negative result of the `#07` index.
  * The following were found: `Y` .
* Remove occurrences that are duplicated, in this case had no.
* Return the result:`Y`

**Option 2:**

The second option can present a better performance if the expression was born so normalized if it is ensured, we don't have to perform the first step.

* Apply [Standardization-3 type](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#normalization-3) to ensure that all entities are being declared the first use. This step is not necessary if the expression was born.

```
        A + B + (C + Y) + (D + A + C) + Z
                 ^              
Level:  1   2    2   3     2   3   3    2
Index:  0   1    2   3     4   5   6    7
```

* Find the first occurrence of the entity `C` . After normalization, we should find the occurrence you're in the index `#02` .
* Retrieve the descendants of the first occurrence of the entity `C` in the index `#02` .
  * `#02`: The entity `C` has the General level equal to `2` .
  * `#03`: **The entity `Y` is the next entity after `C` and your General level is `3` , is descended**.
  * `#04`: The entity `D` is the next entity after `Y` and your General level is `2` , she's not a descendant.
  * The expression did not finish, but was interrupted after the negative result of the `#04` index.
  * The following were found: `Y` .
* Remove occurrences that are duplicated, in this case had no.
* Return the result:`Y`

Finally, it is possible to say that we don't need to assign a special treatment for **entities with cyclic paths** if we were using a _superficial research_. We have seen that the solution is the same in both situations.

This theme was also addressed, superficially, in the topic [Entity declarations](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#entity-declaration).

### <a name="search-find-children" />Finding the children of an entity

To start this topic you need to understand the topic [Finding all the descendants of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-descendants).

The logic is exactly the same as the descendants search, the only difference is that the **overall level** will be limited to: _[General current entity-level] + 1_

We'll use in this example the [denormalized array](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#sample-matrix-desnormalizated) of topic about [Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep).

Based on this array, if we want to find all the daughters of `D` entity of the line `#04` :

* The entity `D` has the General level equal to `2` .
* **The entity `E` is the next entity after `D` and your General level is 3, is the daughter of `D` **.
* **The entity `F` is the next entity after `E` and your General level is also 3, is the daughter of `D` **.
* The next after entities `F` are: `G` , `B` , `C` , `Y` and `Y` , all levels greater than 3, then will be ignored.
* **The entity `Z` is the next entity after `Y` and your General level is also 3, is the daughter of `D` **.

Just the words and in the end we will have the result:`E, F, Z`

### <a name="search-find-ascending" />Finding all the ascendants of an entity

If we want to find the upside of an entity, we must check if the previous entity has your **General level** is lower than the **General level** of the desired entity if the entity is an ascendant.

```
                A + B
Level:          1   2
                ^   *
Parent of B:    A
```

If the entity is the same level of the entity you want to, you should ignore it and continue navigating backwards until you find the first entity with the **General level** lower than the **General level** of the desired entity.

```
                A + B + J
Level:          1   2   2
                ^       *
Parent of J:    A
```

After finding the first descent, one should continue navigating back, but the **General level** to be considered now to be the first and not ancestry more desired entity. This process should continue until you reach the root entity.

```
                A + B + (J + Y)
Level:          1   2    2   3
                ^        ^   *
Parents of Y:   J, A
```

**Attention:** This search can be done using the two types of research: **Research deep** and **superficial Research**. However, the _deep search_ may return a larger quantity of occurrences. This is why in this kind of research groups of expressions are redeclarados.

For example, if we want to catch the ascendants of the entity `C` considering all its instances:

**Occurrence: 1**

* The entity `C` of the line `#02` has the General level equal to `2` .
* `#01`: The entity `B` has the General level equal to `2` , is not.
* `#00`: **The entity `A` has the General level equal to `1` , is less, so is the first ascent, in which case the parent entity. Now the level to be considered will be the `1` level rather than the `2` level **.

Just the words and we have the following ancestors:`A`

**2 occurrence:**

* The entity `C` of the line `#09` has the General level equal to `5` .
* `#08`: The entity `B` has the General level equal to `5` , is not.
* `#07`: **The entity `G` has the General level equal to `4` , is less, so is the first ascent, in which case the parent entity. Now the level to be considered will be the `4` level rather than the `5` level **.
* `#06`: **The entity `F` has the General level equal to `3` , is less than the General level of the entity `G` , so is an ascendant. Now the level to be considered will be the `3` level rather than the `4` level **.
* `#05`: The entity `E` has the General level equal to `3` , is not a.
* `#04`: **The entity `D` has the General level equal to `2` , is an ascendant. Now the level to be considered will be the `2` level rather than the `3` level **.
* `#03`: The entity `Y` has the General level equal to `3` , is not a.
* `#02`: The entity `C` has the General level equal to `2` , is not a.
* `#01`: The entity `B` has the General level equal to `2` , is not a.
* `#00`: **The entity `A` has the General level equal to `1` , is an ascendant. Now the level to be considered will be the `1` level rather than the `2` level **.

Just the words and in the end we will have the following entities: `G` upward, `F` `D` and, `A` .

### <a name="search-find-parent" />Meeting the parents of an entity

Following the logic of the topic [Finding all the ascendants of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-ascending), to find just the father of the `Y` entity, we would need to limit the **overall level** of the ancestors: _[General current entity-level]-1_; or the first entity with the **General level** less than the desired entity.

**Attention:** This search can be done using the two types of research: **Research deep** and **superficial Research**. However, the _deep search_ may return a larger quantity of occurrences. This is why in this kind of research groups of expressions are redeclarados.

As there are 3 occurrences of the `Y` entity, we will have a _parent entity_ for occurrence:

**Occurrence: 1**

* The entity `Y` of the line `#3` has the General level equal to `3` .
* `#02`: **The entity `C` is the entity before `Y` and has the General level equal to `2` , so she is the father of the entity `Y` **.

**2 occurrence:**

* The entity `Y` of the line `#10` has the General level equal to `6` .
* `#09`: **The entity `C` is the entity before `Y` and has the General level equal to `5` , so she is the father of the entity `Y` **.

**3 occurrence:**

* The entity `Y` of the line `#11` has the General level equal to `4` .
* `#10`: The entity `Y` has the General level equal to `6` , is not a.
* `#09`: The entity `C` has the General level equal to `5` , is not a.
* `#08`: The entity `B` has the General level equal to `5` , is not a.
* `#07`: The entity `G` has the General level equal to `4` , is not a.
* `#06`: **The entity `F` is the entity before `G` and has the General level equal to `3` , so she is the father of the entity `Y` **.

# <a name="implementation" />Implementations

This topic will demonstrate in practice some examples of implementations of some of the concepts we study.

* [Creating graphs with graph expression](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-graph)
* [Converting an array of information to graph expressions](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-expression)
* [Creating an array of information from a graph](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#implementation-to-matrix)

Use the `C#` programming language due to your ability to overload mathematical operators.

## <a name="implementation-to-graph" />Creating graphs with graph expression

In this example we will demonstrate how to create a graph using Graph expression only in the most simple and objective as possible.

Will be used a **circular entity**, i.e., an entity that relates to herself.

```csharp
[DebuggerDisplay("{Name}")]
public class Entity : List<Entity>
{
    public string Name { get; private set; }
    public Entity(string identity) => this.Name = identity;

    public static Entity operator +(Entity a, Entity b)
    {
        a.Add(b);
        return a;
    }

    public static Entity operator -(Entity a, Entity b)
    {
        a.Remove(b);
        return a;
    }
}
```

* The class inherits from a generic list of the class itself, our intention is to create an instance.
* The class requires a name as input parameter, is the name of the entity
* `+`And operators `-` were overwritten, now it can be used within an expression.
  * When there's a sum ( `+` ), the entity of the right will be added in the list of the entity from the left, and the left will be returned as a result. This is the basis of the concept of graph expression.
  * When there is a minus sign ( `-` ), the entity of the right will be removed from the list of the entity from the left, and the left will be returned as a result.

To use is simple, just think the concept explained and use as if it were a mathematical expression within the `C#` :

```csharp
class Program
{
    static void Main(string[] args)
    {
        var A = new Entity("A");
        var B = new Entity("B");
        var C = new Entity("C");
        var D = new Entity("D");
        var E = new Entity("E");
        var F = new Entity("F");
        var Y = new Entity("Y");
        var H = new Entity("H");

        // expression1
        A = A + B + (C + (D + E + F)) + (Y + H);

        // expression2
        D = D - E;
    }
}
```

After the execution of the first expression have the following graph:

```
A
----B
----C
    ----D
        ----E
        ----F
----Y
    ----H
```

After the implementation of the second term, we see that the entity `D` no longer has the entity `E` as a daughter, she was subtracted/removed:

```
A
----B
----C
    ----D
        ----F
----Y
    ----H
```

Note that the expression is exactly the same as all the expressions that we saw during this study. This shows that you can enjoy circular entities of this concept without using large blocks of code.

For more complex entities, would not be possible the use of operators so that simple, there would be the need to create mechanisms for reflection and `strings` for the creation and processing of the expression. Besides, we do not recommend this effort, is not the point of this concept create serialization mechanism and deserialização of entities, that means better: `XML` and `JSON` .

## <a name="implementation-to-expression" />Converting an array of information to graph expressions

In this example we will show how to convert an array of information back to graph expression.

It is important to note that this code is simple and specific to our example. Although it can be useful for several purposes due to your ability to identify the correct times of beginning and end of an iteration of an entity.

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

* This class will be our representation of each row in the array of information, i.e., every occurrence of an entity within the expression. In it we will have all the properties that an instance of an entity may have.
* In the properties `Previous` , `Next` and `Parent` , we are implementing the techniques:
  * [Finding the previous entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-previous)
  * [Finding the next entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-next)
  * [Meeting the parents of an entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-parent)

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

The `Main` method we have to call our function, note that we are creating the array of information manually. This array must represent the following expression:

```
(A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z))
```

The function `ToExpressionAsString` will be responsible for making all the iteration and arrive at our goal is return a `string` containing our expression.

* The class `Expression` represents a graph expression as a whole. She inherits from a `EntityItem` type list to do justice to what she is within the concept: a set of entity instances with your information.
* The method `ToExpressionAsString` returns a string that will be our expression.
* The list containing all instances of entities will be covered completely. The 0 position until the end of the list. Each iteration can contain various levels of expression.
* The variable `parenthesisToClose` stores a list of all the brackets that were opened and need to be closed. The list has to be in the format: last in, first out.
* For each iteration:
  * If the entity is the entity root, does not add the `+` signal.
    * [Finding the root of the expression entity](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-find-root)
  * If the entity is the first expression group, adds the character`(`
    * [Verifying that an entity is the first expression Group (first within the parenthesis)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-check-is-first-at-group-expression)
  * If the entity is the last of your group (last inside the parentheses), then close with the `)` character. As several parentheses can have been opened in the previous iterations, so we must calculate the amount of brackets that need to be closed, and closes them. The variable `parenthesisToClose` contains the entity being closed, this can be useful for some logic.
    * [Verifying that an entity is the last expression Group (last within the parenthesis)](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-check-is-last-at-group-expression)

With these code snippets we saw how simple it is to iterate in a graph expression and understand their moments. In addition to open paths to more complete implementations such as: **research in graph expression.**

## <a name="implementation-to-matrix" />Creating an array of information from a graph

In the previous example we saw how to generate a graph expression from an array of manual and information that was represented by the class `Expression` .

In this example, let's discuss an implementation that creates this array.

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

The method `ToMatrixAsString` will be used to check the result of our example. And after processing of the graph of the entity `A` , we have the following array of information:

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

* The class gets into your constructor, the **root entity**. From this instance, let's navigate in your graph.
* The `Deep` parameter determines if scan will be deep or not and what was explained in the topic[Deep search](https://github.com/juniorgasparotto/GraphExpression/blob/master/doc/concept.md#search-deep)
* The first `if` inside the function `Build` checks to see if it is the root entity, if it is, we must create the first item. At this point, the information is fixed, since the root entity, will be the initial values.
* In the second part of the function, we start reading the children of the entity `parent` .
* Will be incremented `+1` in **General level** as deepens in the entity. This value is passed by parameter, because it transcends the entire graph.
* Will be incremented `+1` in **the index level**. This value is closed only in `foreach` scope, that is, only for the children of the entity.
* For each iteration, it is checked if the property `Deep` is `true` , if it is, we should keep the navigation even if current entity has already been covered in some point of the expression. However, the only situation that limits the continuation is if the current entity has relationships with herself in one of their ancestors. If so, stops the continuation.
* If the property `Deep` is `false` , so we should just check if the entity has already been covered at some point of the term, if it was, then continue.
* Property `LevelAtExpression` (**expression level**) is populated with the **expression level** of the parent entity in addition `+1` when the entity has children and not adding anything when I haven't.

With that, we found the three main examples of the concept and that can be the basis for more complex implementations such as **research in graph expression**.

* * *

<sub>This text was translated by a machine</sub>

https://github.com/juniorgasparotto/MarkdownGenerator