# Churras Manager API - Trinca

API para gerenciamento de churrascos da Trinca.

## Tecnologias

- _Asp .Net Core 3.1_
- _Entity Framework Core_
- _Swagger_

## Arquitetura

Esta API foi estruturada com base no **Repository Pattern**, de forma que como o objeto é permutado no banco (_ORM_) e o tipo de banco de dados (_Sql, Postgresql, etc_) não faz diferença na camada dos controllers pois isto está implementado no respectivo Repository do model.

> Por exemplo, a relação do model **Churrasco.cs** com o banco de dados está implementada no **ChurrascoRepository.cs** que este é chamado no controller.

Além disso, foi abstraído o método que realiza o **Commit** no banco de dados para a classe **UnitOfWork.cs** de forma que quando necessário realizar uma transação em mais de uma tabela dentro do banco todas as alterações sejam realizadas de uma única vez, evitando que uma transação de uma tabela seja permutada e a outra não.

## Entity Framework Core - ORM

Para esta aplicação foi utilizado o **Entity Framework Core** para armazenamento dos dados. Mas para que não seja necessário a criação de uma banco de dados, foi utilizado a versão **InMemory** da _ORM_ assim a aplicação irá criar um banco de dados na memória da aplicação sendo desfeito sempre que a aplicação for encerrada.
Nesta aplicação também foi utilizado os **DataAnnotations** para controle de propriedades obrigatórias do model na hora de implementá-lo.

## Api aberta e Jwt

Existem duas branchs neste repositório, na master se encontra o projeto de Api aberta, sem autenticação de acesso aos endpoints e uma branch que contem o mesmo projeto mas com autenticação utilizando Jwt Token.

## Swagger

Para maior controle e uso da Api, está implementado neste projeto o Swagger já na página inicial da Api. Na branch de Jwt o swagger está configurado para realizar autenticação, basta realizar o login no controller de autenticação e inserir o token dentro da opção Authorize localizado antes das controllers. **"Bearer _Seutoken_"**
