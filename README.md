# iTasks - Gestão de Tarefas Mobile

> **Projeto Académico - Laboratório de Desenvolvimento II (ISTEC)**

O **iTasks** é uma solução completa de gestão de tarefas baseada na metodologia **Kanban**, desenvolvida para otimizar o fluxo de trabalho entre Gestores e Programadores. O sistema é composto por uma aplicação móvel (**.NET MAUI**) e uma API robusta (**ASP.NET Core**).

---

## 📋 Índice
- [Funcionalidades](#-funcionalidades)
- [Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [Algoritmo Preditivo](#-algoritmo-preditivo-requisito-28)
- [Pré-requisitos](#-pré-requisitos)
- [Como Clonar e Executar](#-como-clonar-e-executar)
- [Autor](#-autor)

---

## 🚀 Funcionalidades

### 🔐 Autenticação & Perfil
* **Login Diferenciado:** Acesso distinto para Gestores e Programadores.
* **Gestão de Perfil:** Atualização de dados pessoais (Nome, Username, Senha) e profissionais (Departamento/Nível) com validação de unicidade de username.

### 📋 Quadro Kanban (Core)
* **Visualização:** Colunas *To Do*, *Doing* e *Done*.
* **Drag & Drop:** Movimentação intuitiva de tarefas entre estados.
* **Regras de Negócio Rigorosas:**
    * Respeito pela "Ordem de Execução" definida pelo gestor.
    * Bloqueio automático se o programador já tiver 2 tarefas em "Doing".
    * Registo automático de datas reais (*DataRealInicio* e *DataRealFim*) ao mover cartões.

### 👔 Módulo do Gestor
* **CRUD de Tarefas:** Criação e edição completa de tarefas.
* **Dashboard Inteligente:** Visualização da previsão de tempo para limpar o backlog (*To Do*), baseada no histórico da equipa.
* **Relatórios:** Exportação de tarefas concluídas para ficheiro **CSV** (guardado no dispositivo).
* **Monitorização:** Listagem com comparativo de prazos (*Previsto vs Real*) e identificação do executor.

### 💻 Módulo do Programador
* **Meus Trabalhos:** Filtro automático para mostrar apenas as tarefas atribuídas ao utilizador logado.
* **Histórico:** Acesso à lista pessoal de tarefas concluídas.
* **Detalhes:** Visualização das tarefas em modo de leitura (Read-Only).

---

## 🛠 Tecnologias Utilizadas

### Mobile (Frontend)
* **Framework:** .NET MAUI (.NET 8).
* **Padrão Arquitetural:** MVVM (Model-View-ViewModel).
* **Bibliotecas:**
    * `CommunityToolkit.Mvvm`: Para comandos e propriedades observáveis.
    * `CommunityToolkit.Maui`: Para exportação de ficheiros (FileSaver).

### Backend (API)
* **Framework:** ASP.NET Core Web API (.NET 8).
* **Base de Dados:** PostgreSQL.
* **ORM:** Entity Framework Core.
* **Padrões:** Repository Pattern, DTOs (Data Transfer Objects), Injeção de Dependência.

---

## 🧠 Algoritmo Preditivo (Requisito 28)

Um dos diferenciais deste projeto é o algoritmo de estimativa temporal presente no Dashboard do Gestor.

**Como funciona:**
1.  O sistema analisa todas as tarefas já concluídas ("Done").
2.  Calcula a média de horas gastas por *Story Points* (Dificuldade).
3.  Aplica essa média às tarefas pendentes no "To Do".
4.  **Lógica "Nearest Neighbor":** Se não existir histórico para uma pontuação exata (ex: 13 pontos), o algoritmo utiliza a média da pontuação mais próxima disponível (ex: 8 ou 20 pontos).

---

## 📦 Pré-requisitos

Para rodar este projeto localmente, precisas de ter instalado:

1.  **Visual Studio 2022** (com a workload *.NET Multi-platform App UI development*).
2.  **.NET 8 SDK**.
3.  **PostgreSQL** (e uma ferramenta de gestão como pgAdmin ou DBeaver).
4.  **Git**.

---

## ⚡ Como Clonar e Executar

Siga este guia passo-a-passo para colocar o projeto a funcionar na sua máquina.

### 1. Clonar o Repositório
Abra o terminal ou CMD na pasta onde deseja guardar o projeto e execute:

```bash
git clone [https://github.com/seu-usuario/iTasks.git](https://github.com/seu-usuario/iTasks.git)
cd iTasks
```
### 2. Configurar a Base de Dados (PostgreSQL)
Abra o pgAdmin (ou similar) e crie uma nova base de dados chamada iTasksDB.

Abra a solução iTasks.sln no Visual Studio.

Navegue até ao projeto iTaskAPI e abra o ficheiro appsettings.json.

Atualize a ConnectionStrings com as suas credenciais do Postgres:
```JSON
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=iTasksDB;Username=postgres;Password=sua_senha_aqui"
}
```

No Visual Studio, vá a Ferramentas > Gerenciador de Pacotes NuGet > Consola do Gerenciador de Pacotes.

Selecione o projeto padrão como iTaskAPI e execute:

```PowerShell
Update-Database
```

Isto irá criar todas as tabelas automaticamente.

3. Configurar e Executar a API
No projeto iTaskAPI, abra Properties/launchSettings.json.

Certifique-se que o perfil http ou o perfil com o nome do projeto está configurado para escutar em todos os IPs (0.0.0.0) para que o Emulador Android consiga conectar. Exemplo:

```JSON
"applicationUrl": "[http://0.0.0.0:5055](http://0.0.0.0:5055)"
```
Defina o projeto iTaskAPI como Projeto de Arranque (StartUp Project).

Inicie a API (F5 ou Ctrl+F5).

Nota: A API deve abrir um browser com o Swagger. Mantenha a API a rodar.

4. Configurar e Executar a App Mobile
No Visual Studio, clique com o botão direito na Solução e escolha Configurar Projetos de Arranque. Escolha iniciar ambos (API e Mobile) ou mantenha a API a rodar numa instância separada.

Vá ao projeto iTask-App-Mobile.

Abra o ficheiro Services/UserService.cs (e TarefaService.cs).

Verifique se o URL base está correto para o seu ambiente:

```C#
private string BaseUrl =>
    DeviceInfo.Platform == DevicePlatform.Android
    ? "[http://10.0.2.2:5055](http://10.0.2.2:5055)"  // Endereço especial do Emulador Android para o localhost do PC
    : "http://localhost:5055"; // Para Windows Machine
```

Selecione o emulador Android ou Windows Machine na barra de ferramentas.

Execute a aplicação Mobile.

## 👤 Autor
Leonel Mendes Francisco

Nº de Estudante: 2024178

Curso: CTeSP em Desenvolvimento para Dispositivos Móveis

Instituição: ISTEC

## 📄 Licença
Este projeto foi desenvolvido para fins estritamente académicos no âmbito da unidade curricular de Laboratório de Desenvolvimento II.
