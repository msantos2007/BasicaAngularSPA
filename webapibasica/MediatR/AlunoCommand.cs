using MediatR;
using webapibasica.Entities;
using webapibasica.Models;

namespace webapibasica.MediatR
{
    public class AdicionarNovoAlunoCommand : IRequest<Aluno>
    {
        public AlunoViewModel _alunoViewModel { get; }

        public AdicionarNovoAlunoCommand(AlunoViewModel alunoViewModel)
        {
            _alunoViewModel = alunoViewModel;
        }
    }

    public class AlterarAlunoCommand : IRequest<Aluno>
    {
        public AlunoViewModel _alunoViewModel { get; }
        public AlterarAlunoCommand(AlunoViewModel alunoViewModel)
        {
            _alunoViewModel = alunoViewModel;
        }
    }

    public class DeletarAlunoCommand : IRequest<bool>
    {
        public String _nomeOuId { get; }
        public DeletarAlunoCommand(String nomeOuId)
        {
            _nomeOuId = nomeOuId;
        }
    }

}