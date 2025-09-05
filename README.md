# Projeto Chat WebApp

Este documento apresenta um roteiro de desenvolvimento detalhado para a construção de um Chat WebApp moderno e escalável. O projeto será desenvolvido utilizando .NET no backend com arquitetura DDD (Domain-Driven Design) e CQRS (Command Query Responsibility Segregation), e Angular 13 no frontend para uma experiência de usuário rica e em tempo real. As funcionalidades principais incluem chat em tempo real e cadastro de usuários.

# Roteiro de Desenvolvimento Backend: .NET com DDD e CQRS

Este documento detalha o roteiro de desenvolvimento para o backend do projeto chat-webapp, utilizando a plataforma .NET com as abordagens de Domain-Driven Design (DDD) e Command Query Responsibility Segregation (CQRS).



## 1. Arquitetura Geral

A arquitetura do backend será baseada em princípios de **Domain-Driven Design (DDD)** para modelar o domínio de forma rica e expressiva, e **Command Query Responsibility Segregation (CQRS)** para separar as operações de escrita (comandos) das operações de leitura (queries), otimizando a performance e a escalabilidade. O sistema será construído sobre o framework .NET, aproveitando suas capacidades robustas para desenvolvimento de aplicações empresariais. A comunicação em tempo real será gerenciada através do SignalR, garantindo uma experiência de chat fluida e responsiva.

### 1.1. Camadas da Aplicação

O backend será estruturado em camadas lógicas para promover a separação de preocupações e facilitar a manutenção. As principais camadas incluem:

*   **Domínio (Domain Layer):** O coração da aplicação, contendo a lógica de negócio, entidades, objetos de valor, agregados, repositórios e serviços de domínio. É agnóstico a tecnologias de persistência ou UI.
*   **Aplicação (Application Layer):** Orquestra as operações de negócio, coordenando o domínio e a infraestrutura. Contém os comandos, queries e seus respectivos handlers, além de serviços de aplicação que orquestram o fluxo de trabalho.
*   **Infraestrutura (Infrastructure Layer):** Responsável por detalhes técnicos como persistência de dados (Entity Framework Core), comunicação com serviços externos, segurança e configurações. Implementa as interfaces definidas na camada de domínio e aplicação.
*   **Apresentação/API (Presentation/API Layer):** Exporá os endpoints RESTful para o frontend e gerenciará as conexões SignalR para comunicação em tempo real. Será a interface de comunicação com o mundo exterior.

Esta estrutura visa garantir um sistema coeso, testável e escalável, onde as mudanças em uma camada têm impacto mínimo nas outras.



## 2. Domain-Driven Design (DDD)

O DDD será a espinha dorsal da modelagem do domínio, garantindo que a lógica de negócio complexa seja bem compreendida e implementada. Os principais conceitos do DDD a serem aplicados incluem:

### 2.1. Entidades (Entities)

Entidades são objetos que possuem uma identidade e um ciclo de vida contínuo, mesmo que seus atributos mudem. Exemplos no contexto do chat-webapp incluem `User` (usuário), `ChatRoom` (sala de chat) e `Message` (mensagem). Cada entidade terá um identificador único.

### 2.2. Objetos de Valor (Value Objects)

Objetos de valor são objetos que descrevem características de algo e são definidos pela composição de seus atributos, sem uma identidade conceitual. Eles são imutáveis e comparados por seus valores. Exemplos podem ser `Username` (nome de usuário), `EmailAddress` (endereço de e-mail) ou `MessageContent` (conteúdo da mensagem).

### 2.3. Agregados (Aggregates)

Agregados são clusters de entidades e objetos de valor que são tratados como uma única unidade transacional. Eles possuem uma **Raiz do Agregado (Aggregate Root)**, que é a única entidade que pode ser referenciada diretamente de fora do agregado. Isso garante a consistência dos dados dentro do agregado. Por exemplo, `ChatRoom` pode ser uma Raiz do Agregado, contendo uma coleção de `Messages` e `Users` associados a ela. Todas as operações que modificam o estado de um agregado devem passar pela sua Raiz.

### 2.4. Repositórios (Repositories)

Repositórios são interfaces que fornecem métodos para persistir e recuperar agregados do armazenamento de dados. Eles abstraem os detalhes da persistência, permitindo que a camada de domínio se concentre na lógica de negócio. Cada Raiz do Agregado terá seu próprio repositório. Por exemplo, `IChatRoomRepository` e `IUserRepository`.

### 2.5. Serviços de Domínio (Domain Services)

Serviços de domínio são operações que não se encaixam naturalmente em uma entidade ou objeto de valor, mas que representam uma lógica de negócio importante que envolve múltiplos agregados ou conceitos de domínio. Por exemplo, um serviço para `UserRegistrationService` que coordena a criação de um novo usuário e a notificação de outros sistemas.

### 2.6. Eventos de Domínio (Domain Events)

Eventos de domínio representam algo significativo que aconteceu no domínio e que outros componentes da aplicação (ou até mesmo outros sistemas) podem estar interessados em reagir. Por exemplo, `UserRegisteredEvent` (usuário registrado) ou `MessageSentEvent` (mensagem enviada). A publicação e o consumo de eventos de domínio ajudam a desacoplar os componentes e a construir sistemas mais reativos e escaláveis.



## 3. Command Query Responsibility Segregation (CQRS)

CQRS é um padrão que separa as operações de leitura (queries) das operações de escrita (comandos). Isso permite otimizar cada lado independentemente, melhorando a escalabilidade, performance e complexidade de manutenção.

### 3.1. Comandos (Commands)

Comandos são objetos que representam uma intenção de mudar o estado do sistema. Eles são imperativos e devem ser processados uma única vez. Exemplos incluem `RegisterUserCommand` (registrar usuário), `SendMessageCommand` (enviar mensagem) e `CreateChatRoomCommand` (criar sala de chat). Cada comando terá um handler correspondente.

### 3.2. Queries (Queries)

Queries são objetos que representam uma solicitação para obter dados do sistema. Elas não devem alterar o estado do sistema e podem ser otimizadas para leitura. Exemplos incluem `GetUserByIdQuery` (obter usuário por ID), `GetChatRoomMessagesQuery` (obter mensagens da sala de chat) e `GetAllChatRoomsQuery` (obter todas as salas de chat). Cada query terá um handler correspondente.

### 3.3. Handlers (Handlers)

Handlers são classes responsáveis por processar comandos e queries. Um `CommandHandler` recebe um comando, valida-o, interage com o domínio (agregados e repositórios) para executar a lógica de negócio e persiste as mudanças. Um `QueryHandler` recebe uma query e recupera os dados diretamente do modelo de leitura (que pode ser otimizado para consultas, como uma view materializada ou um banco de dados NoSQL para dados de chat).

### 3.4. MediatR

Para a implementação de CQRS, utilizaremos a biblioteca **MediatR**. Ela atua como um mediador, desacopando o remetente do comando/query do seu handler. Isso simplifica a arquitetura e facilita a adição de comportamentos transversais (cross-cutting concerns) como validação, logging e caching através de *pipelines*.


## 4. Tecnologias e Bibliotecas do Backend

O backend será desenvolvido utilizando as seguintes tecnologias e bibliotecas:

*   **.NET 9:** Framework principal para o desenvolvimento da aplicação. Será utilizado o ASP.NET Core para a criação da API e o SignalR para a comunicação em tempo real.
*   **Entity Framework Core:** ORM (Object-Relational Mapper) para interação com o banco de dados. Será utilizado para persistir e recuperar os dados do domínio (usuários, salas de chat, etc.).
*   **SQL Server (ou PostgreSQL/MySQL):** Banco de dados relacional para armazenamento dos dados transacionais e de domínio. A escolha específica pode ser flexível, mas o SQL Server é uma opção robusta para ambientes .NET.
*   **MediatR:** Biblioteca para implementação do padrão Mediator, essencial para o CQRS, facilitando a comunicação entre os componentes da aplicação (comandos, queries e handlers).
*   **FluentValidation:** Biblioteca para validação de objetos (comandos, queries, entidades). Integrar-se-á com o pipeline do MediatR para garantir que as validações ocorram antes da execução da lógica de negócio.
*   **AutoMapper:** Biblioteca para mapeamento de objetos entre diferentes camadas (ex: DTOs para entidades de domínio e vice-versa). Ajuda a reduzir o código boilerplate.
*   **SignalR:** Biblioteca do ASP.NET Core para adicionar funcionalidades de web em tempo real. Será fundamental para o chat, permitindo a troca instantânea de mensagens entre os usuários.
*   **ASP.NET Core Identity:** Sistema de gerenciamento de usuários, autenticação e autorização. Será utilizado para o cadastro e login de usuários, bem como para gerenciar permissões.
*   **JWT (JSON Web Tokens):** Para autenticação baseada em tokens, permitindo que o frontend se autentique com o backend de forma segura e stateless.
*   **Swagger/OpenAPI:** Para documentação automática da API REST, facilitando o consumo pelo frontend e por outras aplicações.


## 5. Estrutura de Pastas e Organização do Código

A organização do código seguirá uma estrutura modular e limpa, refletindo as camadas da arquitetura e os princípios do DDD. Uma sugestão de estrutura de pastas para o projeto .NET seria:

```
├── src/
│   ├── ChatWebApp.Api/                 # Camada de Apresentação (ASP.NET Core Web API, SignalR Hubs)
│   │   ├── Controllers/
│   │   ├── Hubs/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── ChatWebApp.Application/         # Camada de Aplicação (Comandos, Queries, Handlers, Serviços de Aplicação)
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   ├── Services/
│   │   └── Mappings/
│   ├── ChatWebApp.Domain/              # Camada de Domínio (Entidades, Value Objects, Agregados, Repositórios, Serviços de Domínio, Eventos de Domínio)
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Aggregates/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── Events/
│   └── ChatWebApp.Infrastructure/      # Camada de Infraestrutura (Implementações de Repositórios, EF Core Context, Migrações, Integrações Externas)
│   │   ├── Persistence/
│   │   │   ├── Configurations/
│   │   │   ├── Contexts/
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   ├── Identity/
│   │   └── Services/
│   └── ChatWebApp.Web/                 # Camada de Web (Arquivos do frontend SSR do projeto Angular 13)
│       └── ClientSide/
├── tests/
│   ├── ChatWebApp.UnitTests/
│   ├── ChatWebApp.IntegrationTests/
│   └── ChatWebApp.FunctionalTests/
└── ChatWebApp.sln
```

Esta estrutura promove a separação de preocupações e facilita a navegação e manutenção do código. Cada projeto (`.csproj`) dentro da solução (`.sln`) terá uma responsabilidade clara, e as dependências seguirão o princípio de dependência invertida (Infrastructure depende de Application, Application depende de Domain, e Api depende de Application e Infrastructure).


## 6. Segurança, Autenticação e Autorização

A segurança é um aspecto crítico em qualquer aplicação, especialmente em um chat-webapp que lida com informações de usuários e comunicação em tempo real. As seguintes abordagens serão adotadas:

### 6.1. Autenticação de Usuários

Para a autenticação de usuários, será utilizado o **ASP.NET Core Identity** em conjunto com **JWT (JSON Web Tokens)**. O fluxo será o seguinte:

1.  **Registro:** Quando um novo usuário se registra, suas credenciais (nome de usuário, senha) serão enviadas para o backend. A senha será armazenada de forma segura (hash e salt) utilizando os mecanismos do ASP.NET Core Identity.
2.  **Login:** Após o login bem-sucedido, o backend gerará um JWT contendo as claims do usuário (identificador, roles, etc.). Este token será retornado ao frontend.
3.  **Autorização:** O frontend enviará este JWT em cada requisição subsequente (via cabeçalho `Authorization: Bearer <token>`) para acessar recursos protegidos da API. O backend validará o token para garantir sua autenticidade e integridade.

### 6.2. Autorização Baseada em Roles e Políticas

O ASP.NET Core oferece um sistema robusto de autorização baseado em roles (papéis) e políticas. Será possível definir diferentes papéis para os usuários (ex: `User`, `Admin`) e aplicar autorização granular a endpoints específicos da API ou a funcionalidades dentro da aplicação. Por exemplo, apenas usuários autenticados podem enviar mensagens, e apenas administradores podem gerenciar salas de chat.

### 6.3. Segurança da Comunicação (HTTPS e SignalR)

Todas as comunicações entre o frontend e o backend (API REST e SignalR) serão realizadas via **HTTPS**, garantindo que os dados transmitidos sejam criptografados e protegidos contra interceptação. O SignalR, por padrão, utiliza WebSockets sobre HTTPS, fornecendo uma comunicação segura e em tempo real.

### 6.4. Proteção contra Ataques Comuns

Serão implementadas medidas para proteger a aplicação contra ataques comuns, como:

*   **Cross-Site Scripting (XSS):** Sanitização de entradas de usuário, especialmente em mensagens de chat, para prevenir a injeção de scripts maliciosos.
*   **Cross-Site Request Forgery (CSRF):** Utilização de tokens anti-CSRF para proteger requisições que alteram o estado do sistema (embora JWTs já ofereçam alguma proteção contra CSRF em APIs stateless).
*   **SQL Injection:** O uso do Entity Framework Core com consultas parametrizadas ajuda a prevenir SQL Injection por padrão.
*   **Rate Limiting:** Implementação de limites de taxa para requisições de login e registro para mitigar ataques de força bruta.

Ao seguir estas diretrizes, o backend do chat-webapp será construído com uma base sólida de segurança, protegendo os dados dos usuários e a integridade da aplicação.


# Roteiro de Desenvolvimento Frontend: Angular 13

Este documento detalha o roteiro de desenvolvimento para o frontend do projeto chat-webapp, utilizando o framework Angular 13. O foco será na criação de uma interface de usuário responsiva e interativa, com funcionalidades de chat em tempo real e gerenciamento de usuários.

### 1.1. Estrutura Modular

A aplicação será dividida em módulos funcionais, onde cada módulo agrupará funcionalidades relacionadas. Isso ajuda a organizar o código, facilita o carregamento lazy-loading (carregamento sob demanda) e melhora a escalabilidade do projeto. Exemplos de módulos podem incluir:

*   **AuthModule:** Para funcionalidades de autenticação e registro de usuários.
*   **ChatModule:** Para a interface principal do chat, incluindo salas de chat, mensagens e lista de usuários.
*   **SharedModule:** Para componentes, diretivas e pipes que são utilizados em múltiplos módulos da aplicação.
*   **CoreModule:** Para serviços singleton que são carregados uma única vez na aplicação (ex: serviços de autenticação, interceptors HTTP).

### 1.2. Componentes e Serviços

*   **Componentes:** Serão responsáveis pela parte visual da aplicação, encapsulando a lógica de apresentação e interação com o usuário. Serão utilizados componentes de UI reutilizáveis para garantir consistência e agilidade no desenvolvimento.
*   **Serviços:** Serão responsáveis por encapsular a lógica de negócio, a comunicação com APIs externas e o gerenciamento de estado. Os serviços serão injetáveis e poderão ser utilizados por múltiplos componentes.

### 1.3. Gerenciamento de Estado

Para o gerenciamento de estado da aplicação, será considerada a utilização de uma biblioteca como o **NgRx** (Redux para Angular). O NgRx ajuda a gerenciar o estado de forma previsível e centralizada, facilitando a depuração e a manutenção de aplicações complexas. Ele se baseia nos princípios de:

*   **Store:** Um único objeto que contém o estado global da aplicação.
*   **Actions:** Objetos que descrevem eventos que ocorreram na aplicação.
*   **Reducers:** Funções puras que recebem o estado atual e uma ação, e retornam um novo estado.
*   **Effects:** Para lidar com efeitos colaterais (side effects) como requisições HTTP, interações com APIs externas e lógica assíncrona.
*   **Selectors:** Funções puras para extrair fatias específicas do estado.

Embora o NgRx adicione uma curva de aprendizado inicial, seus benefícios em termos de rastreabilidade, testabilidade e escalabilidade justificam seu uso em aplicações de médio a grande porte como um chat-webapp.




## 2. Tecnologias e Bibliotecas do Frontend

O frontend será construído com as seguintes tecnologias e bibliotecas:

*   **Angular 13:** O framework principal para o desenvolvimento da aplicação web. Será utilizado para criar a estrutura, os componentes e gerenciar o fluxo de dados.
*   **TypeScript:** Linguagem de programação utilizada pelo Angular, que adiciona tipagem estática ao JavaScript, melhorando a robustez e a manutenibilidade do código.
*   **Angular Material:** Biblioteca de componentes de UI que implementa o Material Design do Google. Oferece componentes pré-construídos e estilizados, acelerando o desenvolvimento da interface e garantindo uma experiência de usuário consistente e moderna.
*   **NgRx:** Para gerenciamento de estado reativo e centralizado da aplicação. Essencial para lidar com a complexidade do estado de um chat em tempo real (mensagens, usuários online, salas de chat, etc.).
*   **RxJS:** Biblioteca para programação reativa, amplamente utilizada no Angular para lidar com eventos assíncronos e fluxos de dados. Fundamental para a comunicação em tempo real e manipulação de dados.
*   **SignalR Client:** Biblioteca cliente para interagir com o SignalR Hub no backend. Será a base para a comunicação em tempo real do chat, permitindo o envio e recebimento instantâneo de mensagens.
*   **HttpClient:** Para realizar requisições HTTP para a API REST do backend (ex: cadastro de usuários, login, histórico de mensagens).
*   **JWT Helper:** Para auxiliar no gerenciamento e decodificação de JSON Web Tokens (JWTs) recebidos do backend para autenticação.
*   **SCSS/Sass:** Pré-processador CSS para estilização, permitindo o uso de variáveis, mixins e aninhamento, o que torna o CSS mais organizado e fácil de manter.





## 3. Implementação do Chat em Tempo Real

A funcionalidade de chat em tempo real será o coração da aplicação. A comunicação será estabelecida via SignalR, que permitirá a troca instantânea de mensagens entre os usuários conectados.

### 3.1. Conexão com SignalR Hub

O frontend estabelecerá uma conexão com o SignalR Hub no backend assim que o usuário estiver autenticado. Isso será feito utilizando o pacote `@microsoft/signalr` (SignalR Client para JavaScript/TypeScript).

```typescript
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

// ... dentro de um serviço Angular, por exemplo

private hubConnection: HubConnection;

startConnection(): Promise<void> {
  this.hubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:5001/chatHub', { accessTokenFactory: () => localStorage.getItem('jwt_token') })
    .withAutomaticReconnect()
    .build();

  return this.hubConnection.start()
    .then(() => console.log('Connection started'))
    .catch(err => console.log('Error while starting connection: ' + err));
}

// ... métodos para enviar e receber mensagens
```

### 3.2. Envio e Recebimento de Mensagens

*   **Envio:** As mensagens digitadas pelo usuário serão enviadas para o SignalR Hub através de métodos definidos no Hub (ex: `SendMessage`).
*   **Recebimento:** O frontend se inscreverá em eventos do Hub (ex: `ReceiveMessage`) para receber novas mensagens em tempo real. As mensagens recebidas serão adicionadas à interface do usuário.

### 3.3. Gerenciamento de Estado do Chat com NgRx

O estado das mensagens, usuários online e salas de chat será gerenciado pelo NgRx. Isso garantirá que a interface do usuário seja atualizada de forma reativa e consistente. Por exemplo:

*   **Actions:** `SendMessage`, `ReceiveMessage`, `LoadChatHistory`, `UserJoined`, `UserLeft`.
*   **Reducers:** Atualizarão o estado das mensagens e da lista de usuários online.
*   **Effects:** Lidarão com a comunicação com o SignalR Hub e a API REST para carregar o histórico de mensagens.

### 3.4. Histórico de Mensagens

Ao entrar em uma sala de chat, o frontend fará uma requisição à API REST do backend para carregar o histórico de mensagens daquela sala. Este histórico será então exibido ao usuário, e novas mensagens serão adicionadas em tempo real via SignalR.





## 4. Fluxo de Cadastro e Autenticação de Usuários

O frontend será responsável por fornecer uma interface intuitiva para o cadastro e autenticação de usuários, interagindo com a API REST do backend.

### 4.1. Cadastro de Usuários

*   **Formulário de Registro:** Um formulário simples será criado para coletar informações como nome de usuário, e-mail e senha.
*   **Validação:** A validação do formulário será realizada tanto no lado do cliente (Angular Forms com validações reativas) quanto no lado do servidor (API do backend).
*   **Requisição HTTP:** Ao submeter o formulário, uma requisição POST será enviada para o endpoint de registro do backend. Em caso de sucesso, o usuário será redirecionado para a tela de login ou para a tela principal do chat.

### 4.2. Autenticação de Usuários (Login)

*   **Formulário de Login:** Um formulário para coletar nome de usuário/e-mail e senha.
*   **Requisição HTTP:** Uma requisição POST será enviada para o endpoint de login do backend. Em caso de sucesso, o backend retornará um JWT (JSON Web Token).
*   **Armazenamento do JWT:** O JWT recebido será armazenado de forma segura no `localStorage` ou `sessionStorage` do navegador. Este token será utilizado para autenticar requisições subsequentes ao backend.
*   **Guards de Rota:** Serão implementados `AuthGuard`s no Angular para proteger rotas que exigem autenticação, redirecionando usuários não autenticados para a tela de login.
*   **Interceptors HTTP:** Um `HttpInterceptor` será configurado para automaticamente adicionar o JWT ao cabeçalho `Authorization: Bearer <token>` de todas as requisições HTTP enviadas ao backend, garantindo que as requisições sejam autenticadas.

### 4.3. Gerenciamento de Estado de Autenticação com NgRx

O estado de autenticação do usuário (logado/deslogado, informações do usuário, token) será gerenciado pelo NgRx. Isso permitirá que diferentes partes da aplicação reajam às mudanças no estado de autenticação de forma consistente.

*   **Actions:** `Login`, `LoginSuccess`, `LoginFailure`, `Logout`.
*   **Reducers:** Atualizarão o estado de autenticação (ex: `isAuthenticated: true/false`, `user: {}`, `token: ''`).
*   **Effects:** Lidarão com as requisições HTTP de login e registro, o armazenamento do token e o redirecionamento de rotas.





## 5. UI/UX e Responsividade

Uma interface de usuário intuitiva e responsiva é crucial para a experiência do usuário em um chat-webapp. O design será focado em usabilidade e adaptabilidade a diferentes tamanhos de tela.

### 5.1. Design de Interface com Angular Material

O **Angular Material** será a principal ferramenta para a construção da interface. Ele oferece uma vasta gama de componentes pré-construídos que seguem as diretrizes do Material Design, garantindo uma aparência moderna e consistente. Isso inclui:

*   **Layouts:** Utilização de `mat-toolbar`, `mat-sidenav`, `mat-card` para estruturar a interface.
*   **Controles de Formulário:** `mat-form-field`, `mat-input`, `mat-button` para formulários de login, registro e envio de mensagens.
*   **Listas e Navegação:** `mat-list`, `mat-menu` para exibir salas de chat, usuários online e opções de navegação.
*   **Ícones:** Utilização de `mat-icon` para ícones visuais que aprimoram a usabilidade.

### 5.2. Responsividade

A aplicação será projetada para ser totalmente responsiva, adaptando-se a diferentes dispositivos (desktops, tablets, smartphones). Isso será alcançado através de:

*   **Flexbox e CSS Grid:** Utilização de técnicas modernas de layout CSS para criar layouts flexíveis e adaptáveis.
*   **Media Queries:** Para aplicar estilos específicos com base no tamanho da tela, permitindo ajustes finos no layout e na apresentação dos componentes.
*   **Componentes Responsivos do Angular Material:** Muitos componentes do Angular Material já são responsivos por natureza ou oferecem opções para tal.
*   **Mobile-First Approach:** O desenvolvimento será iniciado com o design para dispositivos móveis, e então expandido para telas maiores. Isso garante que a experiência em dispositivos menores seja otimizada desde o início.

### 5.3. Tematização (Theming)

O Angular Material permite a criação de temas personalizados. Será possível definir uma paleta de cores primária e secundária, tipografia e outras propriedades visuais para personalizar a aparência da aplicação, alinhando-a com a identidade visual desejada.

### 5.4. Acessibilidade

Serão consideradas as diretrizes de acessibilidade (WCAG) durante o desenvolvimento. O Angular Material já oferece um bom suporte a acessibilidade, mas será importante garantir que a navegação por teclado, o contraste de cores e as descrições de elementos (ARIA attributes) sejam adequados para usuários com deficiência.

Ao focar em UI/UX e responsividade, o chat-webapp proporcionará uma experiência agradável e acessível para todos os usuários, independentemente do dispositivo que estiverem utilizando.


# Roteiro de Desenvolvimento Frontend: Angular 13

Este documento detalha o roteiro de desenvolvimento para o frontend do projeto chat-webapp, utilizando o framework Angular 13. O foco será na criação de uma interface de usuário responsiva e interativa, com funcionalidades de chat em tempo real e gerenciamento de usuários.




## 1. Arquitetura Geral

A aplicação frontend será desenvolvida com Angular 13, seguindo as melhores práticas e padrões de projeto do ecossistema Angular. A arquitetura será modular, com componentes bem definidos e serviços para gerenciar a lógica de negócio e a comunicação com o backend. O objetivo é criar uma Single Page Application (SPA) performática, escalável e de fácil manutenção.

### 1.1. Estrutura Modular

A aplicação será dividida em módulos funcionais, onde cada módulo agrupará funcionalidades relacionadas. Isso ajuda a organizar o código, facilita o carregamento lazy-loading (carregamento sob demanda) e melhora a escalabilidade do projeto. Exemplos de módulos podem incluir:

*   **AuthModule:** Para funcionalidades de autenticação e registro de usuários.
*   **ChatModule:** Para a interface principal do chat, incluindo salas de chat, mensagens e lista de usuários.
*   **SharedModule:** Para componentes, diretivas e pipes que são utilizados em múltiplos módulos da aplicação.
*   **CoreModule:** Para serviços singleton que são carregados uma única vez na aplicação (ex: serviços de autenticação, interceptors HTTP).

### 1.2. Componentes e Serviços

*   **Componentes:** Serão responsáveis pela parte visual da aplicação, encapsulando a lógica de apresentação e interação com o usuário. Serão utilizados componentes de UI reutilizáveis para garantir consistência e agilidade no desenvolvimento.
*   **Serviços:** Serão responsáveis por encapsular a lógica de negócio, a comunicação com APIs externas e o gerenciamento de estado. Os serviços serão injetáveis e poderão ser utilizados por múltiplos componentes.

### 1.3. Gerenciamento de Estado

Para o gerenciamento de estado da aplicação, será considerada a utilização de uma biblioteca como o **NgRx** (Redux para Angular). O NgRx ajuda a gerenciar o estado de forma previsível e centralizada, facilitando a depuração e a manutenção de aplicações complexas. Ele se baseia nos princípios de:

*   **Store:** Um único objeto que contém o estado global da aplicação.
*   **Actions:** Objetos que descrevem eventos que ocorreram na aplicação.
*   **Reducers:** Funções puras que recebem o estado atual e uma ação, e retornam um novo estado.
*   **Effects:** Para lidar com efeitos colaterais (side effects) como requisições HTTP, interações com APIs externas e lógica assíncrona.
*   **Selectors:** Funções puras para extrair fatias específicas do estado.

Embora o NgRx adicione uma curva de aprendizado inicial, seus benefícios em termos de rastreabilidade, testabilidade e escalabilidade justificam seu uso em aplicações de médio a grande porte como um chat-webapp.




## 2. Tecnologias e Bibliotecas do Frontend

O frontend será construído com as seguintes tecnologias e bibliotecas:

*   **Angular 13:** O framework principal para o desenvolvimento da aplicação web. Será utilizado para criar a estrutura, os componentes e gerenciar o fluxo de dados.
*   **TypeScript:** Linguagem de programação utilizada pelo Angular, que adiciona tipagem estática ao JavaScript, melhorando a robustez e a manutenibilidade do código.
*   **Angular Material:** Biblioteca de componentes de UI que implementa o Material Design do Google. Oferece componentes pré-construídos e estilizados, acelerando o desenvolvimento da interface e garantindo uma experiência de usuário consistente e moderna.
*   **NgRx:** Para gerenciamento de estado reativo e centralizado da aplicação. Essencial para lidar com a complexidade do estado de um chat em tempo real (mensagens, usuários online, salas de chat, etc.).
*   **RxJS:** Biblioteca para programação reativa, amplamente utilizada no Angular para lidar com eventos assíncronos e fluxos de dados. Fundamental para a comunicação em tempo real e manipulação de dados.
*   **SignalR Client (NPM package):** Biblioteca cliente para interagir com o SignalR Hub no backend. Será a base para a comunicação em tempo real do chat, permitindo o envio e recebimento instantâneo de mensagens.
*   **HttpClient:** Para realizar requisições HTTP para a API REST do backend (ex: cadastro de usuários, login, histórico de mensagens).
*   **JWT:** Para auxiliar no gerenciamento e decodificação de JSON Web Tokens (JWTs) recebidos do backend para autenticação.
*   **SCSS/Sass:** Pré-processador CSS para estilização, permitindo o uso de variáveis, mixins e aninhamento, o que torna o CSS mais organizado e fácil de manter.






## 3. Implementação do Chat em Tempo Real

A funcionalidade de chat em tempo real será o coração da aplicação. A comunicação será estabelecida via SignalR, que permitirá a troca instantânea de mensagens entre os usuários conectados.

### 3.1. Conexão com SignalR Hub

O frontend estabelecerá uma conexão com o SignalR Hub no backend assim que o usuário estiver autenticado. Isso será feito utilizando o pacote `@microsoft/signalr` (SignalR Client para JavaScript/TypeScript).

```typescript
import { HubConnection, HubConnectionBuilder } from \'@microsoft/signalr\';

// ... dentro de um serviço Angular, por exemplo

private hubConnection: HubConnection;

startConnection(): Promise<void> {
  this.hubConnection = new HubConnectionBuilder()
    .withUrl(\'https://localhost:5001/chatHub\', { accessTokenFactory: () => localStorage.getItem(\'jwt_token\') })
    .withAutomaticReconnect()
    .build();

  return this.hubConnection.start()
    .then(() => console.log(\'Connection started\'))
    .catch(err => console.log(\'Error while starting connection: \' + err));
}

// ... métodos para enviar e receber mensagens
```

### 3.2. Envio e Recebimento de Mensagens

*   **Envio:** As mensagens digitadas pelo usuário serão enviadas para o SignalR Hub através de métodos definidos no Hub (ex: `SendMessage`).
*   **Recebimento:** O frontend se inscreverá em eventos do Hub (ex: `ReceiveMessage`) para receber novas mensagens em tempo real. As mensagens recebidas serão adicionadas à interface do usuário.

### 3.3. Gerenciamento de Estado do Chat com NgRx

O estado das mensagens, usuários online e salas de chat será gerenciado pelo NgRx. Isso garantirá que a interface do usuário seja atualizada de forma reativa e consistente. Por exemplo:

*   **Actions:** `SendMessage`, `ReceiveMessage`, `LoadChatHistory`, `UserJoined`, `UserLeft`.
*   **Reducers:** Atualizarão o estado das mensagens e da lista de usuários online.
*   **Effects:** Lidarão com a comunicação com o SignalR Hub e a API REST para carregar o histórico de mensagens.

### 3.4. Histórico de Mensagens

Ao entrar em uma sala de chat, o frontend fará uma requisição à API REST do backend para carregar o histórico de mensagens daquela sala. Este histórico será então exibido ao usuário, e novas mensagens serão adicionadas em tempo real via SignalR.





## 4. Fluxo de Cadastro e Autenticação de Usuários

O frontend será responsável por fornecer uma interface intuitiva para o cadastro e autenticação de usuários, interagindo com a API REST do backend.

### 4.1. Cadastro de Usuários

*   **Formulário de Registro:** Um formulário simples será criado para coletar informações como nome de usuário, e-mail e senha.
*   **Validação:** A validação do formulário será realizada tanto no lado do cliente (Angular Forms com validações reativas) quanto no lado do servidor (API do backend).
*   **Requisição HTTP:** Ao submeter o formulário, uma requisição POST será enviada para o endpoint de registro do backend. Em caso de sucesso, o usuário será redirecionado para a tela de login ou para a tela principal do chat.

### 4.2. Autenticação de Usuários (Login)

*   **Formulário de Login:** Um formulário para coletar nome de usuário/e-mail e senha.
*   **Requisição HTTP:** Uma requisição POST será enviada para o endpoint de login do backend. Em caso de sucesso, o backend retornará um JWT (JSON Web Token).
*   **Armazenamento do JWT:** O JWT recebido será armazenado de forma segura no `localStorage` ou `sessionStorage` do navegador. Este token será utilizado para autenticar requisições subsequentes ao backend.
*   **Guards de Rota:** Serão implementados `AuthGuard`s no Angular para proteger rotas que exigem autenticação, redirecionando usuários não autenticados para a tela de login.
*   **Interceptors HTTP:** Um `HttpInterceptor` será configurado para automaticamente adicionar o JWT ao cabeçalho `Authorization: Bearer <token>` de todas as requisições HTTP enviadas ao backend, garantindo que as requisições sejam autenticadas.

### 4.3. Gerenciamento de Estado de Autenticação com NgRx

O estado de autenticação do usuário (logado/deslogado, informações do usuário, token) será gerenciado pelo NgRx. Isso permitirá que diferentes partes da aplicação reajam às mudanças no estado de autenticação de forma consistente.

*   **Actions:** `Login`, `LoginSuccess`, `LoginFailure`, `Logout`.
*   **Reducers:** Atualizarão o estado de autenticação (ex: `isAuthenticated: true/false`, `user: {}`, `token: \'\'`).
*   **Effects:** Lidarão com as requisições HTTP de login e registro, o armazenamento do token e o redirecionamento de rotas.






## 5. UI/UX e Responsividade

Uma interface de usuário intuitiva e responsiva é crucial para a experiência do usuário em um chat-webapp. O design será focado em usabilidade e adaptabilidade a diferentes tamanhos de tela.

### 5.1. Design de Interface com Angular Material

O **Angular Material** será a principal ferramenta para a construção da interface. Ele oferece uma vasta gama de componentes pré-construídos que seguem as diretrizes do Material Design, garantindo uma aparência moderna e consistente. Isso inclui:

*   **Layouts:** Utilização de `mat-toolbar`, `mat-sidenav`, `mat-card` para estruturar a interface.
*   **Controles de Formulário:** `mat-form-field`, `mat-input`, `mat-button` para formulários de login, registro e envio de mensagens.
*   **Listas e Navegação:** `mat-list`, `mat-menu` para exibir salas de chat, usuários online e opções de navegação.
*   **Ícones:** Utilização de `mat-icon` para ícones visuais que aprimoram a usabilidade.

### 5.2. Responsividade

A aplicação será projetada para ser totalmente responsiva, adaptando-se a diferentes dispositivos (desktops, tablets, smartphones). Isso será alcançado através de:

*   **Flexbox e CSS Grid:** Utilização de técnicas modernas de layout CSS para criar layouts flexíveis e adaptáveis.
*   **Media Queries:** Para aplicar estilos específicos com base no tamanho da tela, permitindo ajustes finos no layout e na apresentação dos componentes.
*   **Componentes Responsivos do Angular Material:** Muitos componentes do Angular Material já são responsivos por natureza ou oferecem opções para tal.
*   **Mobile-First Approach:** O desenvolvimento será iniciado com o design para dispositivos móveis, e então expandido para telas maiores. Isso garante que a experiência em dispositivos menores seja otimizada desde o início.

### 5.3. Tematização (Theming)

O Angular Material permite a criação de temas personalizados. Será possível definir uma paleta de cores primária e secundária, tipografia e outras propriedades visuais para personalizar a aparência da aplicação, alinhando-a com a identidade visual desejada.

### 5.4. Acessibilidade

Serão consideradas as diretrizes de acessibilidade (WCAG) durante o desenvolvimento. O Angular Material já oferece um bom suporte a acessibilidade, mas será importante garantir que a navegação por teclado, o contraste de cores e as descrições de elementos (ARIA attributes) sejam adequados para usuários com deficiência.

Ao focar em UI/UX e responsividade, o chat-webapp proporcionará uma experiência agradável e acessível para todos os usuários, independentemente do dispositivo que estiverem utilizando.


# Roteiro de Integração e Funcionalidades em Tempo Real

Este documento detalha a integração entre o frontend (Angular 13) e o backend (.NET com DDD e CQRS), com foco especial nas funcionalidades de chat em tempo real.




## 1. Comunicação entre Frontend e Backend

A comunicação entre o frontend Angular e o backend .NET será estabelecida através de duas principais abordagens:

### 1.1. APIs RESTful para Operações Transacionais

Para operações que envolvem o gerenciamento de dados transacionais e que não exigem comunicação em tempo real imediata, serão utilizadas APIs RESTful. Isso inclui:

*   **Autenticação e Autorização:** Endpoints para registro de novos usuários (`POST /api/auth/register`), login (`POST /api/auth/login`) e validação de tokens.
*   **Gerenciamento de Usuários:** Operações CRUD (Create, Read, Update, Delete) para perfis de usuário (ex: `GET /api/users/{id}`, `PUT /api/users/{id}`).
*   **Gerenciamento de Salas de Chat:** Criação, listagem e edição de salas de chat (ex: `POST /api/chatrooms`, `GET /api/chatrooms`).
*   **Histórico de Mensagens:** Requisições para obter o histórico de mensagens de uma sala de chat específica (`GET /api/chatrooms/{id}/messages`).

As requisições HTTP do frontend serão feitas utilizando o `HttpClient` do Angular, e o backend responderá com dados em formato JSON. A segurança será garantida pelo uso de JWTs para autenticação e HTTPS para criptografia da comunicação.

### 1.2. SignalR para Comunicação em Tempo Real

Para as funcionalidades de chat em tempo real, o **SignalR** será a tecnologia escolhida. Ele permite a comunicação bidirecional e persistente entre o servidor e os clientes, ideal para cenários onde a baixa latência e a atualização instantânea são cruciais. O SignalR abstrai a complexidade de tecnologias como WebSockets, Server-Sent Events e Long Polling, escolhendo automaticamente o melhor método de transporte disponível.

*   **Hubs:** No backend, serão criados `Hubs` do SignalR (ex: `ChatHub`) que atuarão como pontos de comunicação centralizados. Os clientes (frontend Angular) se conectarão a esses Hubs.
*   **Métodos do Hub:** O Hub exporá métodos que o cliente pode invocar (ex: `SendMessage`) e métodos que o servidor pode invocar nos clientes (ex: `ReceiveMessage`).
*   **Conexão Persistente:** Uma vez estabelecida a conexão, ela permanecerá aberta, permitindo que o servidor envie mensagens para os clientes a qualquer momento, sem a necessidade de o cliente fazer polling.

Esta combinação de APIs RESTful e SignalR garante que a aplicação possa lidar eficientemente com operações transacionais e, ao mesmo tempo, oferecer uma experiência de chat fluida e em tempo real.




## 2. Implementação do Chat em Tempo Real

A implementação do chat em tempo real envolverá a coordenação entre o frontend e o backend para garantir a troca eficiente e a persistência das mensagens, além de notificações para os usuários.

### 2.1. Fluxo de Envio de Mensagens

1.  **Frontend (Angular):** O usuário digita uma mensagem e clica em enviar. O componente Angular dispara uma ação (NgRx) `SendMessage` com o conteúdo da mensagem, ID da sala e ID do remetente.
2.  **Serviço Angular:** Um efeito NgRx ou um serviço dedicado invoca o método `SendMessage` no `ChatHub` do SignalR, passando os dados da mensagem.
3.  **Backend (SignalR Hub):** O `ChatHub` recebe a mensagem. O `SendMessageCommand` é criado e enviado ao MediatR.
4.  **Backend (CommandHandler):** O `SendMessageCommandHandler` valida a mensagem, interage com o domínio (ex: `MessageAggregate` e `IMessageRepository`) para persistir a mensagem no banco de dados. Um `MessageSentEvent` é publicado.
5.  **Backend (Domain Event Handler):** Um handler para `MessageSentEvent` no backend notifica o `ChatHub` para que ele possa transmitir a mensagem para todos os clientes conectados àquela sala de chat.
6.  **Backend (SignalR Hub):** O `ChatHub` invoca o método `ReceiveMessage` em todos os clientes conectados à sala de chat, enviando a nova mensagem.
7.  **Frontend (Angular):** O cliente Angular recebe a mensagem via `ReceiveMessage` do SignalR. Uma ação `ReceiveMessage` (NgRx) é disparada, e o reducer atualiza o estado da aplicação, adicionando a nova mensagem à lista exibida na interface do usuário.

### 2.2. Histórico de Mensagens

Quando um usuário entra em uma sala de chat, o histórico de mensagens será carregado para proporcionar contexto. Este processo utilizará a API RESTful:

1.  **Frontend (Angular):** Ao selecionar uma sala de chat, o frontend dispara uma ação `LoadChatHistory` (NgRx) com o ID da sala.
2.  **Serviço Angular:** Um efeito NgRx ou um serviço dedicado faz uma requisição HTTP `GET` para o endpoint `api/chatrooms/{id}/messages` no backend.
3.  **Backend (API Controller):** O controlador da API recebe a requisição. Uma `GetChatRoomMessagesQuery` é criada e enviada ao MediatR.
4.  **Backend (QueryHandler):** O `GetChatRoomMessagesQueryHandler` consulta o banco de dados (via Entity Framework Core) para recuperar as mensagens da sala. Pode ser uma consulta otimizada para leitura, se houver um modelo de leitura separado.
5.  **Backend (API Controller):** O controlador retorna a lista de mensagens em formato JSON para o frontend.
6.  **Frontend (Angular):** O frontend recebe a lista de mensagens. Uma ação `LoadChatHistorySuccess` (NgRx) é disparada, e o reducer atualiza o estado da aplicação com o histórico de mensagens, que é então exibido na interface.

### 2.3. Notificações em Tempo Real

Além das mensagens de chat, outras notificações em tempo real podem ser implementadas via SignalR:

*   **Usuário Online/Offline:** Quando um usuário se conecta ou desconecta, o backend pode notificar todos os clientes sobre a mudança de status, atualizando a lista de usuários online nas salas de chat.
*   **Digitação:** O frontend pode enviar um evento para o backend quando um usuário está digitando, e o backend pode retransmitir essa informação para outros usuários na mesma sala, exibindo um indicador de 

digitação. 

### 2.4. Persistência de Dados do Chat

As mensagens de chat serão persistidas no banco de dados relacional (SQL Server) utilizando o Entity Framework Core. Embora o chat seja em tempo real, a persistência é crucial para o histórico de mensagens e para garantir que as mensagens não sejam perdidas em caso de desconexão. 

*   **Modelo de Dados:** Uma entidade `Message` será criada no domínio, contendo campos como `Id`, `ChatRoomId`, `SenderId`, `Content`, `Timestamp`. 
*   **Repositório:** Um `IMessageRepository` será definido na camada de domínio e implementado na camada de infraestrutura para lidar com as operações de CRUD de mensagens. 
*   **Otimização:** Para volumes muito grandes de mensagens, pode-se considerar estratégias de otimização de leitura, como a criação de views materializadas ou até mesmo a utilização de um banco de dados NoSQL (ex: MongoDB, Cosmos DB) especificamente para o armazenamento de mensagens de chat, que são frequentemente acessadas e podem se beneficiar de um modelo de dados mais flexível e otimizado para leitura. No entanto, para a fase inicial do projeto, o banco de dados relacional com Entity Framework Core será suficiente e mais simples de gerenciar.






# Roteiro de Integração e Funcionalidades em Tempo Real

Este documento detalha a integração entre o frontend (Angular 13) e o backend (.NET com DDD e CQRS), com foco especial nas funcionalidades de chat em tempo real.




## 1. Comunicação entre Frontend e Backend

A comunicação entre o frontend Angular e o backend .NET será estabelecida através de duas principais abordagens:

### 1.1. APIs RESTful para Operações Transacionais

Para operações que envolvem o gerenciamento de dados transacionais e que não exigem comunicação em tempo real imediata, serão utilizadas APIs RESTful. Isso inclui:

*   **Autenticação e Autorização:** Endpoints para registro de novos usuários (`POST /api/auth/register`), login (`POST /api/auth/login`) e validação de tokens.
*   **Gerenciamento de Usuários:** Operações CRUD (Create, Read, Update, Delete) para perfis de usuário (ex: `GET /api/users/{id}`, `PUT /api/users/{id}`).
*   **Gerenciamento de Salas de Chat:** Criação, listagem e edição de salas de chat (ex: `POST /api/chatrooms`, `GET /api/chatrooms`).
*   **Histórico de Mensagens:** Requisições para obter o histórico de mensagens de uma sala de chat específica (`GET /api/chatrooms/{id}/messages`).

As requisições HTTP do frontend serão feitas utilizando o `HttpClient` do Angular, e o backend responderá com dados em formato JSON. A segurança será garantida pelo uso de JWTs para autenticação e HTTPS para criptografia da comunicação.

### 1.2. SignalR para Comunicação em Tempo Real

Para as funcionalidades de chat em tempo real, o **SignalR** será a tecnologia escolhida. Ele permite a comunicação bidirecional e persistente entre o servidor e os clientes, ideal para cenários onde a baixa latência e a atualização instantânea são cruciais. O SignalR abstrai a complexidade de tecnologias como WebSockets, Server-Sent Events e Long Polling, escolhendo automaticamente o melhor método de transporte disponível.

*   **Hubs:** No backend, serão criados `Hubs` do SignalR (ex: `ChatHub`) que atuarão como pontos de comunicação centralizados. Os clientes (frontend Angular) se conectarão a esses Hubs.
*   **Métodos do Hub:** O Hub exporá métodos que o cliente pode invocar (ex: `SendMessage`) e métodos que o servidor pode invocar nos clientes (ex: `ReceiveMessage`).
*   **Conexão Persistente:** Uma vez estabelecida a conexão, ela permanecerá aberta, permitindo que o servidor envie mensagens para os clientes a qualquer momento, sem a necessidade de o cliente fazer polling.

Esta combinação de APIs RESTful e SignalR garante que a aplicação possa lidar eficientemente com operações transacionais e, ao mesmo tempo, oferecer uma experiência de chat fluida e em tempo real.




## 2. Implementação do Chat em Tempo Real

A implementação do chat em tempo real envolverá a coordenação entre o frontend e o backend para garantir a troca eficiente e a persistência das mensagens, além de notificações para os usuários.

### 2.1. Fluxo de Envio de Mensagens

1.  **Frontend (Angular):** O usuário digita uma mensagem e clica em enviar. O componente Angular dispara uma ação (NgRx) `SendMessage` com o conteúdo da mensagem, ID da sala e ID do remetente.
2.  **Serviço Angular:** Um efeito NgRx ou um serviço dedicado invoca o método `SendMessage` no `ChatHub` do SignalR, passando os dados da mensagem.
3.  **Backend (SignalR Hub):** O `ChatHub` recebe a mensagem. O `SendMessageCommand` é criado e enviado ao MediatR.
4.  **Backend (CommandHandler):** O `SendMessageCommandHandler` valida a mensagem, interage com o domínio (ex: `MessageAggregate` e `IMessageRepository`) para persistir a mensagem no banco de dados. Um `MessageSentEvent` é publicado.
5.  **Backend (Domain Event Handler):** Um handler para `MessageSentEvent` no backend notifica o `ChatHub` para que ele possa transmitir a mensagem para todos os clientes conectados àquela sala de chat.
6.  **Backend (SignalR Hub):** O `ChatHub` invoca o método `ReceiveMessage` em todos os clientes conectados à sala de chat, enviando a nova mensagem.
7.  **Frontend (Angular):** O cliente Angular recebe a mensagem via `ReceiveMessage` do SignalR. Uma ação `ReceiveMessage` (NgRx) é disparada, e o reducer atualiza o estado da aplicação, adicionando a nova mensagem à lista exibida na interface do usuário.

### 2.2. Histórico de Mensagens

Quando um usuário entra em uma sala de chat, o histórico de mensagens será carregado para proporcionar contexto. Este processo utilizará a API RESTful:

1.  **Frontend (Angular):** Ao selecionar uma sala de chat, o frontend dispara uma ação `LoadChatHistory` (NgRx) com o ID da sala.
2.  **Serviço Angular:** Um efeito NgRx ou um serviço dedicado faz uma requisição HTTP `GET` para o endpoint `api/chatrooms/{id}/messages` no backend.
3.  **Backend (API Controller):** O controlador da API recebe a requisição. Uma `GetChatRoomMessagesQuery` é criada e enviada ao MediatR.
4.  **Backend (QueryHandler):** O `GetChatRoomMessagesQueryHandler` consulta o banco de dados (via Entity Framework Core) para recuperar as mensagens da sala. Pode ser uma consulta otimizada para leitura, se houver um modelo de leitura separado.
5.  **Backend (API Controller):** O controlador retorna a lista de mensagens em formato JSON para o frontend.
6.  **Frontend (Angular):** O frontend recebe a lista de mensagens. Uma ação `LoadChatHistorySuccess` (NgRx) é disparada, e o reducer atualiza o estado da aplicação com o histórico de mensagens, que é então exibido na interface.

### 2.3. Notificações em Tempo Real

Além das mensagens de chat, outras notificações em tempo real podem ser implementadas via SignalR:

*   **Usuário Online/Offline:** Quando um usuário se conecta ou desconecta, o backend pode notificar todos os clientes sobre a mudança de status, atualizando a lista de usuários online nas salas de chat.
*   **Digitação:** O frontend pode enviar um evento para o backend quando um usuário está digitando, e o backend pode retransmitir essa informação para outros usuários na mesma sala, exibindo um indicador de 

digitação. 

### 2.4. Persistência de Dados do Chat

As mensagens de chat serão persistidas no banco de dados relacional (SQL Server) utilizando o Entity Framework Core. Embora o chat seja em tempo real, a persistência é crucial para o histórico de mensagens e para garantir que as mensagens não sejam perdidas em caso de desconexão. 

*   **Modelo de Dados:** Uma entidade `Message` será criada no domínio, contendo campos como `Id`, `ChatRoomId`, `SenderId`, `Content`, `Timestamp`. 
*   **Repositório:** Um `IMessageRepository` será definido na camada de domínio e implementado na camada de infraestrutura para lidar com as operações de CRUD de mensagens. 
*   **Otimização:** Para volumes muito grandes de mensagens, pode-se considerar estratégias de otimização de leitura, como a criação de views materializadas ou até mesmo a utilização de um banco de dados NoSQL (ex: MongoDB, Cosmos DB) especificamente para o armazenamento de mensagens de chat, que são frequentemente acessadas e podem se beneficiar de um modelo de dados mais flexível e otimizado para leitura. No entanto, para a fase inicial do projeto, o banco de dados relacional com Entity Framework Core será suficiente e mais simples de gerenciar.