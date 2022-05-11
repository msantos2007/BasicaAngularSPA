using webapibasica.Entities;
using webapibasica.Models;
using AutoMapper;

namespace webapibasica.Mappings
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<Aluno, AlunoViewModel>().ReverseMap();
            CreateMap<Aluno, AlunoMediaViewModel>().ReverseMap();
        }
    }
}