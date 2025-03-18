
# Spider LED Panel Driver (C#)

Este projeto é um driver em C# para comunicação com painéis de LED da fabricante **Spider**, utilizando conexão TCP e protocolo proprietário baseado em cabeçalhos, mensagens ASCII e CRC16.

A aplicação permite enviar mensagens de texto diretamente ao painel, formatando os dados conforme o protocolo de comunicação da Spider.

## 📦 Funcionalidades

- Comunicação via TCP com painel Spider (porta padrão 2101).
- Montagem automática da estrutura de mensagem conforme protocolo Spider.
- Cálculo de CRC16 para integridade dos dados.
- Envio de mensagens em tempo real via terminal.

---

## 🚀 Como executar

### Pré-requisitos:
- .NET SDK 6.0 ou superior
- Acesso à rede do painel Spider (TCP/IP)

### Passos para rodar:

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/spider-led-driver.git
   cd spider-led-driver
   ```

2. Compile o projeto:
   ```bash
   dotnet build
   ```

3. Execute a aplicação:
   ```bash
   dotnet run
   ```

4. Digite a mensagem desejada no terminal. O texto será enviado ao painel.

---

## 🖥️ Exemplo de uso

```text
Connecting to spider device at localhost:2101
Device connected Successfully
Digite a mensagem a ser enviada
Olá, Spider Panel!
Message sent (HEX): 01 02 50 01 01 AA 01 01 82 01 01 00 14 4F 6C 61 2C 20 53 70 69 64 65 72 20 50 61 6E 65 6C 21 03 XX XX
Message 'Olá, Spider Panel!' sent to Panel
```

---

## 🔍 Estrutura da Mensagem

A mensagem enviada ao painel segue o seguinte formato (resumido):

| Campo            | Byte(s)         |
|------------------|-----------------|
| SOH (Start)      | 0x01            |
| STX (Start Text) | 0x02            |
| Classe Origem    | 0x50            |
| Grupo Origem     | 0x01            |
| ID Origem        | 0x01            |
| Classe Destino   | 0xAA            |
| Grupo Destino    | 0x01            |
| ID Destino       | 0x01            |
| Comando          | 0x82 (Quick Msg)|
| NFR              | 0x01            |
| NFRAME           | 0x01            |
| Tamanho Dados    | 2 bytes         |
| Texto (ASCII)    | variável        |
| ETX (End Text)   | 0x03            |
| CRC16 (XMODEM)   | 2 bytes         |

---

## 📁 Estrutura do Projeto

```
📂 spider-led-driver
 ┣ 📄 Program.cs              → Aplicação principal (entrada via console)
 ┗ 📄 DriverSpider.cs         → Driver TCP + montagem da mensagem com protocolo Spider
```

---

## ⚠️ Observações

- Por padrão, o host de conexão está configurado como `localhost` e porta `2101`. Caso o painel esteja em outro IP/porta, altere esses valores no método `Connect()` do `DriverSpider.cs`.
- O protocolo pode ser estendido para outros comandos além de Quick Message (`0x82`), conforme especificações da fabricante.

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE). Fique à vontade para usar, modificar e contribuir!

---

## 🤝 Contribuições

Contribuições são muito bem-vindas! Sinta-se à vontade para abrir issues ou enviar PRs com melhorias ou novas funcionalidades.

---

## 📫 Contato

Caso tenha dúvidas ou sugestões, entre em contato via [seu-email@exemplo.com](mailto:seu-email@exemplo.com)
