# InvestControl.Producer — Publicador de Cotações Kafka
---

## Sobre o Projeto

O **InvestControl.Producer** é um serviço desenvolvido em **.NET 8** que integra dados de mercado da **[BRAPI.dev](https://brapi.dev/)** e os publica em tópicos **Kafka**, permitindo que o **InvestControlApp** (API principal) consuma e processe as informações em tempo real.

Ele faz parte do ecossistema **InvestControl**, composto por:
- **InvestControlApp** — API de controle e análise de investimentos;
- **InvestControl.Consumer** — Worker Service responsável por consumir mensagens Kafka e persistir cotações;
- **InvestControl.UI** — Interface web em HTML, Tailwind CSS e JS puro.

---

## Funcionalidades

-  Consome dados de cotações de ativos via API BRAPI.dev  
-  Publica mensagens JSON em tópicos Kafka 
-  Implementa arquitetura limpa e princípios de resiliência  
-  Integra-se com o **InvestControlApp** e **InvestControl.Consumer**  

---

## 🧰 Tecnologias Utilizadas

| Tecnologia | Descrição |
|-------------|------------|
| **.NET 8** | Framework principal do serviço |
| **Confluent Kafka** | Mensageria assíncrona e escalável |
| **BRAPI.dev** | Fonte de dados de mercado (cotações de ativos) |
| **Polly** | Resiliência e tolerância a falhas (retries, circuit breaker) |
| **Serilog** | Logging estruturado e monitoramento |

---

## 🧬 Arquitetura

```
┌────────────────────────┐
│     BRAPI.dev API      │
└──────────┬─────────────┘
           │ (GET JSON)
           ▼
┌────────────────────────┐
│  InvestControl.Producer │
│  (.NET 8 Service)       │
│  → lê cotações          │
│  → publica no Kafka     │
└──────────┬─────────────┘
           │ (mensageria)
           ▼
┌────────────────────────┐
│  Kafka Topic: cotacoes │
└──────────┬─────────────┘
           │ (consume)
           ▼
┌────────────────────────┐
│  InvestControl.Consumer │
│  → persiste no SQL      │
└────────────────────────┘
```

---

## Principais Conceitos

### Mensageria Assíncrona
Utiliza **Kafka** para comunicação desacoplada entre os microsserviços, garantindo performance e escalabilidade.

### Resiliência com Polly
- **Retry**: tenta reenviar requisições BRAPI até 3 vezes.  
- **Circuit Breaker**: pausa o envio após falhas consecutivas.  
- **Fallback**: ignora mensagens inválidas e continua o fluxo.

### Idempotência
Validação de duplicidade baseada em `preco_atual` e `data_hora` garante consistência nas mensagens enviadas.

---

##  Roadmap

- [x] Publicação automática de cotações no Kafka  
- [x] Resiliência com Polly  
- [x] Integração com Docker Compose  
- [ ] Logs estruturados com Serilog Sink no Elastic Stack  
- [ ] Monitoramento com Prometheus e Grafana  
- [ ] CI/CD via GitHub Actions  

---

## Autor

Desenvolvido por [**Gustavo Amorim**](https://github.com/0GustavoAmorim)  
Parte integrante do ecossistema **InvestControl** desenvolvido para teste técnico itaú, sistema completo para controle e análise de investimentos.
