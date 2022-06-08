using webapibasica.Entities;
using webapibasica.Repository;

namespace webapibasica.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Aluno> AlunoRepository { get; }
        IRepository<AlunoNota> AlunoNotaRepository { get; }
        IRepository<AlunoImagem> AlunoImagemRepository { get; }

        IAlunoCustomReposistory AlunoCustomReposistory { get; }

        Task<bool> Complete();
    }
}