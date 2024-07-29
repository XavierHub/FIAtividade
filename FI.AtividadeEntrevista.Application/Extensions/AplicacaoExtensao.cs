using EmpXpo.Accounting.Application.Services;
using EmpXpo.Accounting.Application.Services.Validators;
using FI.AtividadeEntrevista.Application;
using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using Unity;
using Unity.Lifetime;

namespace EmpXpo.Accounting.Application
{
    public static class AplicacaoExtensao
    {
        public static void AddAplicacao(this IUnityContainer container)
        {
            container.RegisterType<IAplicacaoDoCliente, AplicacaoDoCliente>(new HierarchicalLifetimeManager());
            container.RegisterType<IAplicacaoDoBeneficiario, AplicacaoDoBeneficiario>(new HierarchicalLifetimeManager());
            container.RegisterType<IServicoNotificacao, ServicoNotificacao>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IServicoValidacao<Cliente>), typeof(ServicoValidacaoCliente), new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IServicoValidacao<Beneficiario>), typeof(ServicoValidacaoBeneficiario), new HierarchicalLifetimeManager());
        }
    }
}
