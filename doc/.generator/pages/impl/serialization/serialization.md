# Serialização <header-set anchor-name="impl-serialization" />

A serialização é o processo de transformação de uma entidade para expressão de grafos em forma de texto. Dividimos a serialização em dois tipos: **serialização de entidades circulares** e **serialização de entidades complexas**. 

Isso é interessante, pois entidades circulares são mais simples e precisam apenas de um nome para representa-las, ao contrário de entidades complexas que podem conter diversas propriedades.