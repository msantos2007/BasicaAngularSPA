using MediatR;
using webapibasica.Entities;

namespace webapibasica.MediatR
{
    public class BuscaAlunosQuery : IRequest<IEnumerable<Aluno>>
    {

    }

    public class BuscaAlunoPorIdQuery : IRequest<Aluno>
    {
        public int _id { get; }

        public BuscaAlunoPorIdQuery(int id)
        {
            _id = id;
        }
    }

    public class BuscaAlunoPorNomeOuIdQuery : IRequest<Aluno>
    {
        public string _NomeOuId { get; }

        public BuscaAlunoPorNomeOuIdQuery(string NomeOuId)
        {
            _NomeOuId = NomeOuId;
        }
    }
}