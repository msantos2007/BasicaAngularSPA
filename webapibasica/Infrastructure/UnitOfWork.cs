using webapibasica.Data;
using webapibasica.Entities;
using webapibasica.Repository;

namespace webapibasica.Infrastructure
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BasicaContext _context;

        public UnitOfWork(BasicaContext context)
        {
            _context = context;
        }

        public async Task<bool> Complete() => await _context.SaveChangesAsync() > 0;
        public void Dispose()
        {
            _context.Dispose();
        }

        private IRepository<Aluno> _alunoRepository => new Repository<Aluno>(_context);
        public IRepository<Aluno> AlunoRepository => _alunoRepository ?? new Repository<Aluno>(_context);

        private IRepository<AlunoNota> _alunoNotaRepository => new Repository<AlunoNota>(_context);
        public IRepository<AlunoNota> AlunoNotaRepository => _alunoNotaRepository ?? new Repository<AlunoNota>(_context);

        //Custom Repositoty
        private IAlunoCustomReposistory _alunoCustomRepository => new AlunoCustomRepository(_context);
        public IAlunoCustomReposistory AlunoCustomReposistory => _alunoCustomRepository ?? new AlunoCustomRepository(_context);
    }

}