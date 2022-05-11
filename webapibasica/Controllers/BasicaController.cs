
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using webapibasica.Entities;
using webapibasica.Mappings;
using webapibasica.MediatR;
using webapibasica.Models;

namespace webapibasica.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BasicaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasicaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("BuscarAlunos")]
        public async Task<IActionResult> BuscaAlunos() //string qualuquerparametro, string qualquerparametro2
        {
            var query = new BuscaAlunosQuery();
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [HttpGet]
        [Route("BuscaAlunoPorNomeOuId")]
        public async Task<IActionResult> BuscaAlunoPorNomeOuId(string NomeOuId) //string qualuquerparametro, string qualquerparametro2
        {
            //After MediatR
            var query = new BuscaAlunoPorNomeOuIdQuery(NomeOuId);
            var resultado = await _mediator.Send(query);
            return resultado.Id != 0 ? Ok(resultado) : NotFound("Não Encontrado");
        }

        [HttpPost]
        [Route("AdicionarNovoAluno")]
        public async Task<IActionResult> AdicionarNovoAluno(AlunoViewModel aluno_vm)
        {
            var command = new AdicionarNovoAlunoCommand(aluno_vm);
            var resultado = await _mediator.Send(command);
            return resultado.Id != 0 ? Ok(resultado) : NotFound("Erro");
        }

        [HttpPut]
        [Route("AlterarAluno")]
        public async Task<IActionResult> AlterarAluno(AlunoViewModel aluno_vm)
        {
            //Aqui está dando dois hits no banco (mudar abordagem)

            // //Isso deveria ir para o MediatR
            // var query = new BuscaAlunoPorNomeOuIdQuery(aluno_vm.Nome);
            // var resultado = await _mediator.Send(query);

            // var resposta = resultado.Id != 0;
            // if (resposta)
            // {
            //     //Isso deveria ir para o MediatR
            //     IMapper _mapper;
            //     MapperConfiguration _mapperConfiguration;
            //     _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CommonMappingProfile>());
            //     _mapper = _mapperConfiguration.CreateMapper();
            //     _mapper.Map(aluno_vm, resultado);

            //     //adicionar ajuste no mappings datetimekind
            //     resultado.DtModificacao = DateTime.Now.ToUniversalTime();
            //     resultado.DtNascimento = new DateTime(aluno_vm.DtNascimento.Ticks, DateTimeKind.Utc);

            //     var commnd = new AlterarAlunoCommand(resultado);
            //     resposta = await _mediator.Send(commnd);
            // }

            var commnd = new AlterarAlunoCommand(aluno_vm);
            var resposta = await _mediator.Send(commnd);

            return resposta.Id != 0 ? Ok("Alterado com sucesso") : NotFound("Não Encontrado");
        }

        [HttpDelete]
        [Route("RemoverAluno")]
        public async Task<IActionResult> RemoverAluno(string NomeOuId)
        {
            // //Aqui está dando dois hits no banco (mudar abordagem)
            // //Isso deveria ir para o MediatR
            // var query = new BuscaAlunoPorNomeOuIdQuery(NomeOuId);
            // var resultado = await _mediator.Send(query);

            // var resposta = resultado.Id != 0;
            // if (resposta)
            // {
            //     var commnd = new DeletarAlunoCommand(resultado);
            //     resposta = await _mediator.Send(commnd);
            // }

            var commnd = new DeletarAlunoCommand(NomeOuId);
            var resposta = await _mediator.Send(commnd);
            return resposta ? Ok("Deletado com sucesso") : NotFound("Não Encontrado");
        }
    }
}