using System.Collections.Generic;
using Xunit;

public class EmbalagemServiceTests
{
    private readonly EmbalagemService _embalagemService;

    public EmbalagemServiceTests()
    {
        _embalagemService = new EmbalagemService();
    }

    [Fact]
    public void Deve_Retornar_Caixa_Para_Pedido_Com_Produtos_Que_Cabem()
    {
        var pedido = new Pedido
        {
            pedido_id = 1,
            produtos = new List<Produto>
            {
                new Produto { produto_id = "PS5", Dimensoes = new Dimensoes { Altura = 40, Largura = 10, Comprimento = 25 } },
                new Produto { produto_id = "Volante", Dimensoes = new Dimensoes { Altura = 40, Largura = 30, Comprimento = 30 } }
            }
        };

        var resultado = _embalagemService.CalcularCaixas(pedido);

        Assert.Single(resultado);
        Assert.Equal("Caixa 2", resultado[0].CaixaId);
    }

    [Fact]
    public void Deve_Retornar_Observacao_Para_Produtos_Que_Nao_Cabem()
    {
        var pedido = new Pedido
        {
            pedido_id = 2,
            produtos = new List<Produto>
            {
                new Produto { produto_id = "Cadeira Gamer", Dimensoes = new Dimensoes { Altura = 120, Largura = 60, Comprimento = 70 } }
            }
        };

        var resultado = _embalagemService.CalcularCaixas(pedido);

        Assert.Single(resultado);
        Assert.Null(resultado[0].CaixaId);
        Assert.Equal("Produto(s) não cabem em nenhuma caixa disponível.", resultado[0].Observacao);
    }
}
