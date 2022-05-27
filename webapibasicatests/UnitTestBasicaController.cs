using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;
using MediatR;
using AutoMapper;
using webapibasica.Mappings;
using webapibasica.Data;
using webapibasica.Entities;
using webapibasica.Infrastructure;
using webapibasica.MediatR;
using webapibasica.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using webapibasica.Models;

namespace webapibasicatests;

public class BasicaController_UnitTest
{
    private readonly IMapper _IMapper;
    private readonly MapperConfiguration _MapperConfiguration;

    public BasicaController_UnitTest()
    {
        _MapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CommonMappingProfile>());
        _IMapper = _MapperConfiguration.CreateMapper();
    }

    public async Task<BasicaContext> DbContextInMemory()
    {
        var options = new DbContextOptionsBuilder<BasicaContext>()
                          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                          .Options;

        var _databasecontext = new BasicaContext(options);
        _databasecontext.Database.EnsureCreated();

        if (await _databasecontext.AlunoDbSet.CountAsync() == 0)
        {
            var _Alunos = new List<Aluno>();

            _Alunos.Add(new Aluno
            {   //Media = 10,
                Id = 1,
                DtNascimento = DateTime.Now.ToUniversalTime(),
                Nome = "Marcelo",

                AlunoNotas = new List<AlunoNota>
                {
                    new AlunoNota
                    {
                        Id = 1,
                        AlunoId = 1,
                        Nota = 10
                    },
                    new AlunoNota
                    {
                        Id = 2,
                        AlunoId = 1,
                        Nota = 7
                    },
                    new AlunoNota
                    {
                        Id = 3,
                        AlunoId = 1,
                        Nota = 7
                    }
                }
            });
            _Alunos.Add(new Aluno
            {
                Id = 2,
                DtNascimento = DateTime.Now.ToUniversalTime(),
                Nome = "Ferreira",
                //, Media = 5
                AlunoNotas = new List<AlunoNota>
                {
                    new AlunoNota
                    {
                        Id = 4,
                        AlunoId = 2,
                        Nota = 4
                    }
                }
            });
            _Alunos.Add(new Aluno
            {
                Id = 3,
                DtNascimento = DateTime.Now.ToUniversalTime(),
                Nome = "Santos"
            });
            _Alunos.Add(new Aluno
            {   //Media = 10,
                Id = 4,
                DtNascimento = DateTime.Now.ToUniversalTime(),
                Nome = "Marcelo",

                AlunoNotas = new List<AlunoNota>
                {
                    new AlunoNota
                    {
                        Id = 5,
                        AlunoId = 4,
                        Nota = 10
                    },
                    new AlunoNota
                    {
                        Id = 6,
                        AlunoId = 4,
                        Nota = 7
                    },
                    new AlunoNota
                    {
                        Id = 7,
                        AlunoId = 4,
                        Nota = 7
                    }
                }
            });
            _Alunos.Add(new Aluno
            {   //Media = 10,
                Id = 5,
                DtNascimento = DateTime.Now.ToUniversalTime(),
                Nome = "Marcelo",

                AlunoNotas = new List<AlunoNota>
                {
                    new AlunoNota
                    {
                        Id = 8,
                        AlunoId = 5,
                        Nota = 10
                    },
                    new AlunoNota
                    {
                        Id = 9,
                        AlunoId = 5,
                        Nota = 7
                    }
                }
            });

            await _databasecontext.AlunoDbSet.AddRangeAsync(_Alunos);
            await _databasecontext.SaveChangesAsync();
        }

        return _databasecontext;
    }

    // Add_credit_updates_customer_balance
    // Purchase_without_funds_is_not_possible
    // Add_affiliate_discount
    // UnitOfWork_StateUnderTest_ExpectedBehavior
    // Should_Increase_Balance_When_Deposit_Is_Made

    [Fact]
    public void BuscaAlunos_CriarQuery_MesmoTipo()
    {
        //Arrange

        //Act
        var query = new webapibasica.MediatR.BuscaAlunosQuery();

        //Assert 
        Assert.Equal(typeof(BuscaAlunosQuery), query.GetType());
    }

    [Fact]
    public async Task BuscaAlunos_Retorna_NotNull()
    {
        //Arrange
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);
        var _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<BuscaAlunosQuery>(), It.IsAny<CancellationToken>()))
                     .Returns(uow.AlunoCustomReposistory.BuscaAlunos());

        //Act
        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.BuscaAlunos();

        //Assert 
        Assert.NotNull(resultado);
        uow.Dispose();
        dbContext.Dispose();
    }

    [Fact]
    public async Task BuscaAlunos_Retorna_OkResult()
    {
        //Arrange
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);
        var _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<BuscaAlunosQuery>(), It.IsAny<CancellationToken>()))
                     .Returns(uow.AlunoCustomReposistory.BuscaAlunos());

        //Act
        dbContext.AlunoDbSet.RemoveRange(await dbContext.AlunoDbSet.ToListAsync());
        dbContext.SaveChanges();

        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.BuscaAlunos();
        var resultado_status = resultado as IStatusCodeActionResult;

        //Assert
        Assert.Equal(resultado_status?.StatusCode, StatusCodes.Status200OK);
        uow.Dispose();
        dbContext.Dispose();
    }


    [Theory]
    [InlineData("Marcelo", StatusCodes.Status200OK)]
    [InlineData("Santos", StatusCodes.Status200OK)]
    [InlineData("4", StatusCodes.Status200OK)]
    [InlineData("ferreira", StatusCodes.Status404NotFound)]
    [InlineData("0", StatusCodes.Status404NotFound)]
    [InlineData("18", StatusCodes.Status404NotFound)]
    [InlineData("2", StatusCodes.Status200OK)]
    public async Task BuscaAlunoPorNomeOuId_CheckEachStatus_MatchingTheory(string NomeOuId, int StatusCodeEsperado)
    {
        //Testa apenas depois do retorno do mediatr.send (não avalia query, command ou handler)

        //Rever: Entender melhor relação entre Stub, Fake, Mock
        //Rever: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
        
        //Arrange
        var _mediatorMock = new Mock<IMediator>();
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);
        var data = uow.AlunoCustomReposistory.BuscaAlunoPorNomeOuId(NomeOuId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<BuscaAlunoPorNomeOuIdQuery>(), It.IsAny<CancellationToken>()))
                     .Returns(data);

        //Act
        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.BuscaAlunoPorNomeOuId(NomeOuId);
        var resultado_status = resultado as IStatusCodeActionResult; //as ObjectResult;

        //Assert
        Assert.Equal(resultado_status?.StatusCode, StatusCodeEsperado);

        uow.Dispose();
        dbContext.Dispose();
    }

    [Fact]
    public async Task AdicionarNovoAluno_ReturnNotEmptyValue()
    {
        //Testa query, command ou handler (mas sem Mock)
        //Testa controller com o resultado da query, command ou handler (com Mock)

        //Arrange        
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);

        var _mediatorMock = new Mock<IMediator>();

        var novo_aluno_viewModel = new AlunoViewModel();

        novo_aluno_viewModel.Nome = "Pedro";
        novo_aluno_viewModel.DtNascimento = new DateTime(1980, 10, 14);

        //Act
        var command = new AdicionarNovoAlunoCommand(novo_aluno_viewModel);
        var handler = new AdicionarNovoAlunoHandler(uow);

        var retorno = handler.Handle(command, new CancellationToken());

        _mediatorMock.Setup(x => x.Send(It.IsAny<AdicionarNovoAlunoCommand>(), It.IsAny<CancellationToken>()))
                     .Returns(retorno);

        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.AdicionarNovoAluno(novo_aluno_viewModel);

        //Assert
        Assert.Equal(StatusCodes.Status200OK, (resultado as IStatusCodeActionResult)?.StatusCode);

        uow.Dispose();
        dbContext.Dispose();
    }

    [Fact]
    public async void AlterarAluno_ResultadoOK()
    {
        //Arrange
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);

        var _mediatorMock = new Mock<IMediator>();

        // Id = 2,
        // DtNascimento = DateTime.Now.ToUniversalTime(),
        // Nome = "Ferreira",
        var alterar_aluno_viewModel = new AlunoViewModel();
        alterar_aluno_viewModel.Nome = "Ferreira";
        alterar_aluno_viewModel.DtNascimento = new DateTime(1979, 10, 14);

        //Act       
        var command = new AlterarAlunoCommand(alterar_aluno_viewModel);
        var handler = new AlterarAlunoHandler(uow, _IMapper);

        var retorno = handler.Handle(command, new CancellationToken());

        _mediatorMock.Setup(x => x.Send(It.IsAny<AlterarAlunoCommand>(), It.IsAny<CancellationToken>()))
                     .Returns(retorno);

        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.AlterarAluno(alterar_aluno_viewModel);

        var httpResponse = resultado as ObjectResult;

        //Assert
        Assert.True(httpResponse?.StatusCode == StatusCodes.Status200OK);
        //Assert.True(resultado.)

        uow.Dispose();
        dbContext.Dispose();
    }

    [Fact]
    public async void RemoverAluno_Retorna200OK()
    {
        //Arrange
        var dbContext = await DbContextInMemory();
        var uow = new UnitOfWork(dbContext);

        var _mediatorMock = new Mock<IMediator>(); // Nome = "Ferreira",

        var nomeOuId_deletar = "Ferreira";

        //Act        
        var command = new DeletarAlunoCommand(nomeOuId_deletar);
        var handler = new DeletarAlunoHandler(uow);

        var retorno = handler.Handle(command, new CancellationToken());

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeletarAlunoCommand>(), It.IsAny<CancellationToken>()))
                     .Returns(retorno);

        var _controller = new BasicaController(_mediatorMock.Object);
        var resultado = await _controller.RemoverAluno(nomeOuId_deletar);
        var httpResponse = resultado as ObjectResult;

        //Rever: deveria ter duas assertivas?
        //Assert
        Assert.Equal(StatusCodes.Status200OK, httpResponse?.StatusCode);
        Assert.Equal("Deletado com sucesso", httpResponse?.Value);

        uow.Dispose();
        dbContext.Dispose();
    }
}