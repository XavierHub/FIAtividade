using System.Collections.Generic;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao
{
    public interface IAplicacaoDoCliente
    {
        Task<Cliente> Inserir(Cliente cliente);
        Task<bool> Alterar(long id, Cliente model);
        Task<Cliente> ObterPorId(long id);
        Task<(IEnumerable<Cliente>, int)> Pesquisar(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente);
    }
}

