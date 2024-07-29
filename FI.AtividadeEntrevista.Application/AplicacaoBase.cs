using FI.AtividadeEntrevista.Dominio;
using FI.AtividadeEntrevista.Dominio.Abstracoes.Aplicacao.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Application
{
    public abstract class AplicacaoBase<TEntity> where TEntity : EntityBase
    {
        private readonly IRepositorio<TEntity> _repository;
        //private readonly IValidatorService<TEntity> _validatorService;

        public AplicacaoBase(IRepositorio<TEntity> repository
                              //IValidatorService<TEntity> validatorService
                              )
        {
            _repository = repository;
            //_validatorService = validatorService;
        }

        public virtual async Task<TEntity> Inserir(TEntity model)
        {
            //if (!await _validatorService.IsValidAsync(ValidatorType.Create, model))
            //    return Activator.CreateInstance<TEntity>();

            model.Id = Convert.ToInt32(await _repository.Incluir(model));

            return model;
        }

        public virtual async Task<bool> Alterar(long id, TEntity model)
        {
            //if (!await _validatorService.IsValidAsync(ValidatorType.Update, model) &&
            //    !await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0)
            //   )
            //    return false;

            var entity = await _repository.ObterPorId(id);
            if (entity.Id == 0)
                return false;

            entity = model;
            model.Id = entity.Id;

            return await _repository.Atualizar(model);
        }

        public virtual async Task<bool> Excluir(long id)
        {
            //if (!await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0))
            //    return false;

            var entity = await _repository.ObterPorId(id);
            if (entity.Id == 0)
                return false;

            return await _repository.Excluir(entity);
        }

        public virtual async Task<TEntity> ObterPorId(long id)
        {
            //if (!await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0))
            //    return Activator.CreateInstance<TEntity>();

            return await _repository.Obter("FI_SP_ConsCliente", new { id });
        }

        public virtual async Task<IEnumerable<TEntity>> Todos()
        {
            return await _repository.Todos();
        }
    }
}
