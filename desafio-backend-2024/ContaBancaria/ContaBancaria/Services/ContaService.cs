using ContaBancaria.Context;
using ContaBancaria.DTOs;
using ContaBancaria.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ContaBancaria.Services
{
    public class ContaService
    {
        private readonly AppDbContext _context;
        private readonly ReceitaWs _receitaWs;

        public ContaService(AppDbContext context, ReceitaWs receitaWs)
        {
            _context = context;
            _receitaWs = receitaWs;
        }

        #region Criar Conta
        public async Task<Conta> CriarContaAsync(CriarContaDTO contaDto)
        {
            // usa o service receitaws para buscar o nome da empresa
            var nomeEmpresa = await _receitaWs.ObterNomeEmpresaPorCnpj(contaDto.Cnpj);

            if (string.IsNullOrEmpty(nomeEmpresa))
                throw new Exception("CNPJ inválido ou não encontrado.");

            var caminhoArquivo = string.Empty;

            if (contaDto.ImagemDocumento != null && contaDto.ImagemDocumento.Length > 0)
            {
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
                Directory.CreateDirectory(uploadDirectory);

                var filePath = Path.Combine(uploadDirectory, contaDto.ImagemDocumento.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await contaDto.ImagemDocumento.CopyToAsync(stream);
                }

                caminhoArquivo = Path.Combine("imagens", contaDto.ImagemDocumento.FileName);
            }
            

            var novaConta = new Conta
            {
                Nome = nomeEmpresa,
                CNPJ = contaDto.Cnpj,
                NumeroConta = contaDto.NumeroConta,
                Agencia = contaDto.Agencia,
                ImagemDocumento = caminhoArquivo
            };

            _context.Contas.Add(novaConta);
            await _context.SaveChangesAsync();

            return novaConta;
        }
        #endregion

        #region Atualizar conta
        public async Task<Conta> AtualizarContaAsync(int id, CriarContaDTO contaDto)
        {
            var contaExistente = await _context.Contas.FindAsync(id);
            if (contaExistente == null)
            {
                throw new Exception("Conta não encontrada.");
            }

            // usa o service receitaws para buscar o nome da empresa
            var nomeEmpresa = await _receitaWs.ObterNomeEmpresaPorCnpj(contaDto.Cnpj);

            if (string.IsNullOrEmpty(nomeEmpresa))
            {
                throw new Exception("CNPJ inválido ou não encontrado.");
            }

            // atualizando os dados da conta
            contaExistente.Nome = nomeEmpresa;  
            contaExistente.CNPJ = contaDto.Cnpj;
            contaExistente.NumeroConta = contaDto.NumeroConta;
            contaExistente.Agencia = contaDto.Agencia;

            // verifica se a imagem foi enviada
            if (contaDto.ImagemDocumento != null)
            {
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var fileName = Path.GetFileName(contaDto.ImagemDocumento.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await contaDto.ImagemDocumento.CopyToAsync(stream);
                }

                contaExistente.ImagemDocumento = Path.Combine("imagens", fileName);
            }

            _context.Contas.Update(contaExistente);
            await _context.SaveChangesAsync();

            return contaExistente;
        }
        #endregion

        #region Get Id
        public async Task<Conta> BuscarContaPorIdAsync(int id)
        {
            return await _context.Contas.FindAsync(id);
        }
        #endregion

        #region Get para todas as contas
        public async Task<List<Conta>> BuscarTodasContasAsync()
        {
            return await _context.Contas.ToListAsync();
        }
        #endregion

        #region Delete
        public async Task<bool> ExcluirContaAsync(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
            {
                return false; 
            }

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
            return true; 
        }
        #endregion

        #region Sacar
        public async Task<bool> SacarAsync(int idConta, decimal valor) 
        {
            {
                var conta = await _context.Contas.FindAsync(idConta);

                if (conta == null)
                    throw new Exception("Conta não encontrada");

                if (conta.Saldo < valor)
                    throw new Exception("Saldo insuficiente para o saque.");

                conta.Saldo -= valor;

                
                var transacao = new Transacao
                {
                    Valor = valor,
                    Tipo = "Saque",
                    ContaId = conta.Id,
                    Data = DateTime.Now
                };

                _context.Transacoes.Add(transacao);

                await _context.SaveChangesAsync();
                return true;
            }
        }
        #endregion

        #region Depositar
        public async Task<Conta> DepositarAsync(int idConta, decimal valor) 
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser maior que zero.");

            var conta = await _context.Contas.FindAsync(idConta);

            if (conta == null)
                throw new Exception("Conta não encontrada");

            conta.Saldo += valor;

            
            var transacao = new Transacao
            {
                Valor = valor,
                Tipo = "Deposito",
                ContaId = conta.Id,
                Data = DateTime.Now
            };

            _context.Transacoes.Add(transacao);

            _context.Contas.Update(conta);
            await _context.SaveChangesAsync();

            return conta;

        }
        #endregion

        #region Transferir
        public async Task<bool> TransferirAsync(TransacaoDTO transacaoDto, int contaDestinoId)
        {
            var contaOrigem = await _context.Contas.FindAsync(transacaoDto.ContaId);
            var contaDestino = await _context.Contas.FindAsync(contaDestinoId);

            if (contaOrigem == null || contaDestino == null)
                throw new Exception("Conta de origem ou destino não encontrada");

            if (contaOrigem.Saldo < transacaoDto.Valor)
                throw new Exception("Saldo insuficiente para a transferência.");

            contaOrigem.Saldo -= transacaoDto.Valor;

            var transacaoDebito = new Transacao
            {
                Valor = transacaoDto.Valor,
                Tipo = "Debito",
                ContaId = contaOrigem.Id,
                Data = transacaoDto.Data 
            };

            _context.Transacoes.Add(transacaoDebito);

            contaDestino.Saldo += transacaoDto.Valor;

            var transacaoCredito = new Transacao
            {
                Valor = transacaoDto.Valor,
                Tipo = "Credito",
                ContaId = contaDestino.Id,
                Data = transacaoDto.Data 
            };

            _context.Transacoes.Add(transacaoCredito);

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Saldo e Extrato
        public async Task<ExtratoSaldoDTO> ObterSaldoEExtratoAsync(int contaId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var conta = await _context.Contas.FindAsync(contaId);

            if (conta == null)
            {
                throw new Exception("Conta não encontrada");
            }

            var query = _context.Transacoes.Where(t => t.ContaId == contaId);

            if (dataInicio.HasValue)
            {
                query = query.Where(t => t.Data >= dataInicio.Value);
            }

            if (dataFim.HasValue)
            {
                query = query.Where(t => t.Data <= dataFim.Value);
            }

            var extrato = await query.OrderBy(t => t.Data).ToListAsync();

            return new ExtratoSaldoDTO
            {
                ContaId = conta.Id,
                Saldo = conta.Saldo,
                Extrato = extrato.Select(t => new TransacaoDTO
                {
                    Valor = t.Valor,
                    Tipo = t.Tipo,
                    Data = t.Data,
                    ContaId = t.ContaId
                }).ToList()
            };
        }
        #endregion
    }
}
