using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using FI.WebAtividadeEntrevista.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IAplicacaoDoCliente _aplicacaoDoCliente;
        private readonly IAplicacaoDoBeneficiario _aplicacaoDoBeneficiario;
        private readonly IServicoNotificacao _servicoNotificacao;

        public ClienteController(IAplicacaoDoCliente aplicacaoDoCliente,
                                 IAplicacaoDoBeneficiario aplicacaoDoBeneficiario,
                                 IServicoNotificacao servicoNotificacao
                                )
        {
            _aplicacaoDoCliente = aplicacaoDoCliente;
            _servicoNotificacao = servicoNotificacao;
            _aplicacaoDoBeneficiario = aplicacaoDoBeneficiario;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Incluir(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new JsonResult().ComModelStateErros(this.ModelState, Response);
            }

            var entity = await _aplicacaoDoCliente.Inserir(FillCliente(model));
            await _aplicacaoDoBeneficiario.Alterar(entity.Id, FillBeneficiarios(model.Beneficiarios?.ToList(), entity.Id));

            return Json("Cadastro efetuado com sucesso").ComNotificacoes(_servicoNotificacao, Response);
        }

        [HttpPost]
        public async Task<JsonResult> Alterar(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new JsonResult().ComModelStateErros(this.ModelState, Response);
            }

            await _aplicacaoDoCliente.Alterar(model.Id.Value, FillCliente(model));
            await _aplicacaoDoBeneficiario.Alterar(model.Id.Value, FillBeneficiarios(model.Beneficiarios?.ToList(), model.Id.Value));

            return Json("Cadastro alterado com sucesso").ComNotificacoes(_servicoNotificacao, Response);
        }

        [HttpGet]
        public async Task<ActionResult> Alterar(long id)
        {
            Cliente cliente = await _aplicacaoDoCliente.ObterPorId(id);
            return View(FillClienteModel(cliente));
        }

        [HttpPost]
        public async Task<JsonResult> ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                var resultado = await _aplicacaoDoCliente.Pesquisar(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase));

                //Return result to jTable
                return Json(new { Result = "OK", Records = resultado.Item1, TotalRecordCount = resultado.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<ActionResult> ListarBeneficiarios(long? idCliente = null)
        {
            if (idCliente == null)
                return Json(new List<BeneficiarioModel>());

            var result = await _aplicacaoDoBeneficiario.ConsultarPorIdCliente(idCliente.Value);

            return Json(FillBeneficiariosModel(result));
        }

        #region private method

        private List<Beneficiario> FillBeneficiarios(List<BeneficiarioModel> models, long idCliente)
        {
            if (models == null || models.Count() == 0) return new List<Beneficiario>();

            var clientes = models.Select(model => new Beneficiario()
            {
                Id = (long)(model.Id == null ? 0 : model.Id),
                IdCliente = idCliente,
                Nome = model.Nome,
                CPF = model.CPF
            }).ToList();

            return clientes;
        }

        private List<BeneficiarioModel> FillBeneficiariosModel(IEnumerable<Beneficiario> entidades)
        {
            if (entidades == null) return new List<BeneficiarioModel>();

            var beneficiarioModels = new List<BeneficiarioModel>();

            foreach (var entidade in entidades)
            {
                var beneficiarioModel = new BeneficiarioModel()
                {
                    Id = entidade.Id,
                    IdCliente = entidade.IdCliente,
                    Nome = entidade.Nome,
                    CPF = entidade.CPF,
                };

                beneficiarioModels.Add(beneficiarioModel);
            }
            return beneficiarioModels;
        }

        private Cliente FillCliente(ClienteModel model)
        {
            if (model == null) return new Cliente();

            var cliente = new Cliente()
            {
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                CPF = model.CPF,
                Telefone = model.Telefone,
            };
            return cliente;
        }

        private static ClienteModel FillClienteModel(Cliente cliente)
        {
            if (cliente == null) new ClienteModel();

            return new ClienteModel()
            {
                Id = cliente.Id,
                CEP = cliente.CEP,
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone
            };
        }

        #endregion
    }
}