# Desserialização <header-set anchor-name="impl-deserialization" />

A desserialização é o processo de transformação de um texto para uma entidade especificada. Dividimos a desserialização em dois tipos: **desserialização de entidades circulares** e **desserialização de entidades complexas**.

O processo de desserialização utiliza como base o compilador `Roslyn`. Isso tem prós e contras. 

A parte boa é que não precisamos reimplementar a leitura de expressões matemáticas, pois com o Roslyn é possível converter a `string`, que é uma expressão matemática, em uma `SystaxTree` e com isso fazer a compilação para uma expressão matemática. 

A classe `RoslynExpressionDeserializer<T>` é a responsável por fazer a conversão da `string` para uma expressão matemática.

O tipo inferido `T` deve obrigatoriamente conter uma sobrecarga do operador `+`.

A parte ruim dessa abordagem é que existe uma lentidão nesse processo em sua primeira execução. Por hora, não temos solução para esse problema, mas estamos acompanhando a evolução do compilador Roslyn.