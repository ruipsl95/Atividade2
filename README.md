# Sistema de Votação Eletrónica (Clientes gRPC)

Este projeto consiste na implementação de duas aplicações cliente para interagir com os serviços de Registo e Votação Eletrónica, utilizando gRPC em .NET.

Desenvolvido no âmbito da **Atividade II - Integração de Sistemas**.

## Pré-requisitos

* **.NET SDK 8.0** (ou superior).
* **SO:** Compatível com Windows, macOS e Linux.

## Estrutura do Projeto

* `/ClienteRegisto`: Console app para interação com a Entidade de Registo.
* `/ClienteVotacao`: Console app para interação com a Entidade de Votação.

## Instruções de Execução

Para o sistema funcionar, é necessário executar os **Clientes**. Recomenda-se o uso de **2 terminais** separados.


### Passo 1: Executar os Clientes**
Com os servidores a correr, abrir novos terminais para usar as aplicações cliente.

**Terminal 1 - Cliente de Registo (Obter Credencial):** Este cliente conecta-se ao serviço de registo para validar o Cartão de Cidadão e obter uma credencial de voto.

```bash
cd ClienteRegisto
dotnet run
```
Instruções: Inserir um número de Cartão de Cidadão (ex: 12345678) para receber uma credencial.

Nota: O servidor Mock gera credenciais válidas aleatoriamente (aprox. 70% de sucesso). Devem ser atualizadas as portas consoante as fornecidas pelo servidor


**Terminal 2 - Cliente de Votação (Votar e Resultados):** Este cliente conecta-se ao serviço de votação para consultar candidatos, votar e ver resultados. Devem ser atualizadas as portas consoante as fornecidas pelo servidor
```bash
cd ClienteVotacao
dotnet run
```
### Instruções

1. Escolher a opção **1** para ver os IDs dos candidatos.
2. Escolher a opção **2** para votar.
    * Cole a credencial obtida no passo anterior (ou use a credencial de teste `CRED-ABC-123`).
    * Insira o ID do candidato.
3. Escolhaern a opção **3** para ver a contagem de votos atualizada.

### ⚠️ Notas de Implementação

* **Versão .NET:** Os servidores mock foram ajustados para correr em **.NET 8.0** (originalmente 10.0) para garantir compatibilidade com o ambiente de desenvolvimento local.
* **Ficheiros Proto:** Os contratos `.proto` estão localizados na pasta `Protos` dentro de cada projeto cliente.    
