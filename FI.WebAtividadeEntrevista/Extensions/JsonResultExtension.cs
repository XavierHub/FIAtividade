using FI.AtividadeEntrevista.Dominio.Abstracoes.Servicos;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Extensions
{
    public static class JsonResultExtensions
    {
        public static JsonResult ComNotificacoes(this JsonResult jsonResult, IServicoNotificacao servicoNotificacao, HttpResponseBase response)
        {
            if (servicoNotificacao.TemNotificacao())
            {
                response.StatusCode = 400;
                var erros = string.Join("<br>", servicoNotificacao.Notificacoes().Select(n => n.Mensagem));
                jsonResult = new JsonResult { Data = erros, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return jsonResult;
        }

        public static JsonResult ComModelStateErros(this JsonResult jsonResult, ModelStateDictionary modelState, HttpResponseBase response)
        {
            if (!modelState.IsValid)
            {
                List<string> erros = (from item in modelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                response.StatusCode = 400;
                jsonResult = new JsonResult { Data = string.Join("<br>", erros), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return jsonResult;
        }
    }
}