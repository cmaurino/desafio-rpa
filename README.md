# Projeto de Automação de cursos da Alura 

## Visão Geral

Este projeto consiste em uma aplicação de automação web desenvolvida em C# utilizando Selenium WebDriver. A aplicação acessa o site da Alura, realiza uma busca por cursos relacionados ao tema "Games" (pode ser alterado para qualquer valor) e coleta informações sobre esses cursos, como título, descrição, duração e professor.

## Uso

Execute a aplicação, aguarde até o termino da automação e após isso confira em seu banco de dados os dados gravados utilizando o seu gerenciador preferido.

## FAQ

**P:Qual a forma de arquitetura usada?**
R: O projeto segue uma arquitetura baseada em DDD (Domain-Driven Design), com as seguintes camadas:

Camada de Apresentação: Contém a classe Program responsável pela inicialização e execução da aplicação.
Camada de Serviço: Contém a interface ICourseService e a implementação CourseService, responsável por salvar os cursos no banco de dados.
Camada de Infraestrutura: Contém a classe AppDbContext, responsável pela configuração e acesso ao banco de dados SQLite.

**P:Existem tratamentos de erros neste projeto?**

R: Sim, caso não seja localizado os elementos de link, titulo, descrição, etc. O mesmo vai criar uma mensagem de log indicando onde ocorreu o erro. E para o caso de não encontrar um "Professor" atrelado ao curso, atribuirá o valor "Alura" indicando que o curso é oficial da plataforma e não tem a indicação de um professor.

**P:Qual forma de persistencia foi utilizada nesse projeto?**

R: Os dados serão armazenados em um banco de dados SQLite, visto que por hora é um ganho de performance trabalhar com algo não tão robusto já que é uma automatização simples.

## Autor

Caio Maurino Rosa
