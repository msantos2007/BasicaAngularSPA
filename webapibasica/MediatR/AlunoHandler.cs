using webapibasica.Entities;
using webapibasica.Repository;
using MediatR;
using webapibasica.Infrastructure;
using AutoMapper;
using webapibasica.Mappings;

namespace webapibasica.MediatR
{
    public class BuscaAlunosHandler : IRequestHandler<BuscaAlunosQuery, IEnumerable<Aluno>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuscaAlunosHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Aluno>> Handle(BuscaAlunosQuery request, CancellationToken cancellationToken)
        {
            var alunos = await _unitOfWork.AlunoCustomReposistory.BuscaAlunos();
            return alunos;
        }
    }

    public class BuscaAlunoPorIdHandler : IRequestHandler<BuscaAlunoPorIdQuery, Aluno>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuscaAlunoPorIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Aluno> Handle(BuscaAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            var aluno = await _unitOfWork.AlunoCustomReposistory.BuscaAlunoPorId(request._id);
            if (aluno == null) aluno = new Aluno();
            return aluno;
        }
    }

    public class BuscaAlunoPorNomeOuiIdHandler : IRequestHandler<BuscaAlunoPorNomeOuIdQuery, Aluno>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuscaAlunoPorNomeOuiIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Aluno> Handle(BuscaAlunoPorNomeOuIdQuery request, CancellationToken cancellationToken)
        {

            var alunos = _unitOfWork.AlunoRepository.AllIncluding(x => x.AlunoNotas).ToList();


            var aluno = await _unitOfWork.AlunoCustomReposistory.BuscaAlunoPorNomeOuId(request._NomeOuId);
            if (aluno == null) aluno = new Aluno();
            return aluno;
        }
    }

    public class AdicionarNovoAlunoHandler : IRequestHandler<AdicionarNovoAlunoCommand, Aluno>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdicionarNovoAlunoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Aluno> Handle(AdicionarNovoAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = new Aluno();

            aluno.DtNascimento = new DateTime(request._alunoViewModel.DtNascimento.Ticks, DateTimeKind.Utc);
            aluno.Nome = request._alunoViewModel.Nome;
            aluno.AlunoNotas = new List<AlunoNota>();
            aluno.DtInclusao = DateTime.Now.ToUniversalTime();

            _unitOfWork.AlunoRepository.Add(aluno);
            var response = await _unitOfWork.Complete();
            return response == true ? aluno : new Aluno();
        }
    }

    public class AlterarAlunoHandler : IRequestHandler<AlterarAlunoCommand, Aluno>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlterarAlunoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Aluno> Handle(AlterarAlunoCommand request, CancellationToken cancellationToken)
        {
            var query = new BuscaAlunoPorNomeOuIdQuery(request._alunoViewModel.Nome);
            var handler = new BuscaAlunoPorNomeOuiIdHandler(_unitOfWork);

            var aluno_alterar = await handler.Handle(query, cancellationToken);

            var resposta = aluno_alterar.Id != 0;
            if (resposta)
            {
                _mapper.Map(request._alunoViewModel, aluno_alterar);

                //adicionar ajuste no mappings datetimekind
                aluno_alterar.DtModificacao = DateTime.Now.ToUniversalTime();
                aluno_alterar.DtNascimento = new DateTime(request._alunoViewModel.DtNascimento.Ticks, DateTimeKind.Utc);

                _unitOfWork.AlunoRepository.Update(aluno_alterar);
            }
            var retorno = await _unitOfWork.Complete();

            return retorno ? aluno_alterar : new Aluno();
        }
    }

    public class DeletarAlunoHandler : IRequestHandler<DeletarAlunoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletarAlunoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletarAlunoCommand request, CancellationToken cancellationToken)
        {
            //Aqui está dando dois hits no banco (mudar abordagem)
            var query = new BuscaAlunoPorNomeOuIdQuery(request._nomeOuId);
            var handler = new BuscaAlunoPorNomeOuiIdHandler(_unitOfWork);

            var aluno_remover = await handler.Handle(query, cancellationToken);

            var resposta = aluno_remover.Id != 0;
            if (resposta)
            {
                _unitOfWork.AlunoRepository.Delete(aluno_remover);
            }

            var response = await _unitOfWork.Complete();

            return response;
        }
    }
}