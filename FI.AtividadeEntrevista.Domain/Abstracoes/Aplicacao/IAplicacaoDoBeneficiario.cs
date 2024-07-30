using System.Collections.Generic;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao
{
    public interface IAplicacaoDoBeneficiario
    {
        Task<bool> Alterar(long idCliente, List<Beneficiario> models);
        Task<IEnumerable<Beneficiario>> ConsultarPorIdCliente(long idCliente);
        Task<bool> CpfBeneficiarioValido(long? idCliente, List<Beneficiario> models);
    }
}

