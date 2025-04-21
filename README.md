# aeon‑bkd – Back‑end do Projeto Aeon

> Repositório oficial da API responsável pela geração e distribuição dos arquivos **`.keymap`** que configuram o teclado split **Aeon**.
> Toda a lógica de negócio aqui descrita segue a arquitetura de microsserviços proposta no [System Design](https://raw.githubusercontent.com/aeon-keyboard/.github/main/assets/system_design.png).

---

## Índice

- [aeon‑bkd – Back‑end do Projeto Aeon](#aeonbkd--backend-do-projeto-aeon)
  - [Índice](#índice)
  - [Contexto do Projeto](#contexto-do-projeto)
  - [Arquitetura e Tecnologias](#arquitetura-e-tecnologias)
  - [Como Executar Localmente](#como-executar-localmente)
  - [Estrutura do Repositório](#estrutura-do-repositório)
  - [Fluxo de Deploy *(Futuro)*](#fluxo-de-deploy-futuro)
  - [Padrões de Desenvolvimento](#padrões-de-desenvolvimento)
  - [Referências Úteis](#referências-úteis)
  - [Licença](#licença)

---

## Contexto do Projeto

O **Projeto Aeon** tem como objetivo criar um teclado ergonômico que se adapta ao corpo humano, acompanhado de uma plataforma de configuração simplificada.
Este back‑end concentra‑se em dois domínios principais:

1. **Conversão de Layout:** Recebe um *payload* JSON representando o layout desejado e devolve um arquivo binário `.keymap` compatível com o firmware **ZMK**.
2. **Integração com Hardware:** Expõe um *endpoint* que abre um canal seguro (Protocol Buffers sobre porta COM) para enviar o `.keymap` diretamente ao microcontrolador **nRF52840** sem necessidade de
   *flash* manual.

Para detalhes mais amplos de requisitos e roadmap, consulte a seção [Parte 2 – Software](https://github.com/aeon-keyboard/.github/tree/main/profile#parte-2---software) da documentação principal.

---

## Arquitetura e Tecnologias

| Camada               | Tecnologia / Padrão         | Responsabilidade Principal                          |
|----------------------|-----------------------------|-----------------------------------------------------|
| **API HTTP**         | **.NET 9 Web API**          | Receber solicitações REST, validação e orquestração |
| **Conversor Keymap** | **C (bkt-converter)**       | Transformar JSON ➡️ binário `.keymap`               |
| **Mensageria**       | **RabbitMQ**                | Desacoplamento entre serviços e filas de geração    |
| **Data Store**       | **PostgreSQL**              | Histórico de layouts e auditoria de downloads       |
| **Entrega Contínua** | **Docker & Docker Compose** | Empacotar, isolar e versionar serviços              |
| **Proxy / TLS**      | **Nginx**                   | Terminação HTTPS e roteamento interno               |

A figura abaixo ilustra a comunicação entre os serviços:

![Arquitetura em Microsserviços](https://raw.githubusercontent.com/aeon-keyboard/.github/main/assets/system_design.png)

---

## Como Executar Localmente

> Requisitos: **Docker 24+**, **Docker Compose v2**, e `make`.

```bash
# 1. Clone o monorepo (front + back)
$ git clone https://github.com/aeon-keyboard/aeon-bkd.git
$ cd aeon-bkd

# 2. Gere as imagens e suba o stack
$ make up

# 3. Acesse a documentação interativa
$ open http://localhost:8080/swagger
```

---

## Estrutura do Repositório

``` doc
📂 aeon-bkd/
├─ src/
│  ├─ Aeon.API/          # Camada de apresentação (REST)
│  ├─ Aeon.Application/  # Casos de uso, CQRS, validações
│  ├─ Aeon.Domain/       # Núcleo de domínio (entidades, VOs, serviços)
│  └─ Aeon.Infra/        # Persistência e integrações externas
├─ docker/               # Dockerfiles e compose.yaml
└─ docs/                 # Diagramas, ADRs e instruções adicionais
```

---

## Fluxo de Deploy *(Futuro)*

1. **Push → GitHub Actions**
   Build, testes e *scan* de segurança.
2. **Registry ghcr.io**
   Imagens versionadas com `semantic-version`.
3. **AWS ECS / Fargate**
   *Blue‑green* deploy automático via GitHub OIDC.

Diagrama completo disponível em [docs/deploy-flow.drawio](docs/deploy-flow.drawio).

---

## Padrões de Desenvolvimento

- **Commits:** *Conventional Commits v2* (ex.: `feat(service): suporte a macros`).
- **Branches:** `main` **protegido**; `dev` **Implementações**.
- **Code Style:** `dotnet format` + `clang-format`.

---

## Referências Úteis

- **Front‑end** ‑ Interface de configuração: <https://github.com/aeon-keyboard/aeon-fnd>
- **PCB & Case** ‑ Hardware: <https://github.com/aeon-keyboard/pcb>, <https://github.com/aeon-keyboard/case>
- **Design Visual**: <https://github.com/aeon-keyboard/branding>
- **Documentação Geral do Sistema**: <https://github.com/aeon-keyboard/.github/tree/main/profile>

---

## Licença

Distribuído sob a **MIT License**. Consulte o arquivo [`LICENSE`](LICENSE) para mais detalhes.
