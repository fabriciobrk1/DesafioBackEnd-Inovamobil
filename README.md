# Documentação WEB API Conta Bancária

Esta API foi desenvolvida com ASP.NET Core 8, que permite o gerenciamento de contas
bancárias cadastradas apenas por CNPJ. Os endpoints incluem saque, depósito,
transferência e extrato.

# GET /api/Conta/{id}

Endpoint para retornar uma conta pelo id

**Parâmetros:**

# Id: 3

**Resposta:**

{
"id": 3,
"nome": "INOVACAO COMPUTACAO MOVEL LTDA",
"cnpj": "04225153000198",
"numeroConta": "24321342131",
"agencia": "001",
"saldo": 1709.5,
"imagemDocumento": "imagens\\inovamobil_logo.jpg"
}

# POST /depositar

Endpoint para realizar um depósito em uma conta criada.

**Parâmetros:**

{
"idConta": 3,
"valor": 10.
}

**Resposta:**

{
"mensagem": "Depósito realizado com sucesso.",
"saldoAtual": 2470
}


# POST /transferir

Endpoint para realizar transferência de uma conta para outra

**Parâmetros:**

ContaDestinoId: 1

Request body

{

"valor": 20,

"tipo": "Debito",

"contaId": 3,

"data": "2024- 12 - 27T16:58:39.712Z"

}

**Resposta:**

Transferência realizada com sucesso.

# POST /sacar

Endpoint para realizar saque de uma conta

**Parâmetros:**

{

"idConta": 3,

"valor": 20.

}

**Resposta:**

Saque realizado com sucesso.


# GET /{id}/saldoextrato

Endpoint para exibir o extrato por período de uma conta

**Parâmetros:**

Id: 3

DataInicio : (Pode deixar null)

DataFim : (Pode deixar null)

**Resposta:**

{
"contaId": 3,
"saldo": 1709.5,
"extrato": [
{
"valor": 50,
"tipo": "Saque",
"contaId": 3,
"data": "2024- 12 - 26T12:11:06.979683"
},
{
"valor": 10,
"tipo": "Deposito",
"contaId": 3,
"data": "2024- 12 - 26T12:11:52.583839"
}



