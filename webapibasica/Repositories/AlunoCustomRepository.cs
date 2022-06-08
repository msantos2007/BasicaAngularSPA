using Microsoft.EntityFrameworkCore;
using webapibasica.Data;
using webapibasica.Entities;
using webapibasica.Infrastructure;

namespace webapibasica.Repository
{
    public interface IAlunoCustomReposistory
    {
        Task<IEnumerable<Aluno>> BuscaAlunos();
        Task<Aluno> BuscaAlunoPorNomeOuId(string NomeOuId);
        Task<Aluno> BuscaAlunoPorId(int Id);
    }

    public class AlunoCustomRepository : Repository<Aluno>, IAlunoCustomReposistory
    {
        public AlunoCustomRepository(BasicaContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Aluno>> BuscaAlunos()
        {
            var response = await _context.AlunoDbSet.Include(b => b.AlunoNotas).Include(c => c.AlunoImagens.Where(w => w.Ativo == true)).ToListAsync();
            response = response ?? new List<Aluno>();
            List<Aluno> retorno = response ?? new List<Aluno>();
            return retorno;
        }

        public async Task<Aluno> BuscaAlunoPorNomeOuId(string NomeOuId)
        {
            int _id = 0;
            if (Int32.TryParse(NomeOuId, out int numValue))
            {
                _id = numValue;
            }

            var response = await _context.AlunoDbSet.Include(b => b.AlunoNotas).Where(r => r.Id == _id || r.Nome == NomeOuId).FirstOrDefaultAsync();
            Aluno retorno = response ?? new Aluno();
            return retorno;
        }

        public async Task<Aluno> BuscaAlunoPorId(int Id)
        {
            var response = await _context.AlunoDbSet.Include(b => b.AlunoNotas).Where(r => r.Id == Id).FirstOrDefaultAsync();
            Aluno retorno = response ?? new Aluno();
            return retorno;
        }

    }
}