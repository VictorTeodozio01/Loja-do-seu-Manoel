using System.Collections.Generic;
using System.Linq;

public class EmbalagemService
{
    private List<Caixa> caixasDisponiveis = new List<Caixa>
    {
        new Caixa { CaixaId = "Caixa 1", Dimensoes = new Dimensoes { Altura = 30, Largura = 40, Comprimento = 80 } },
        new Caixa { CaixaId = "Caixa 2", Dimensoes = new Dimensoes { Altura = 80, Largura = 50, Comprimento = 40 } },
        new Caixa { CaixaId = "Caixa 3", Dimensoes = new Dimensoes { Altura = 50, Largura = 80, Comprimento = 60 } }
    };

    public List<CaixaResultado> CalcularCaixas(Pedido pedido)
    {
        var resultado = new List<CaixaResultado>();
        var produtosRestantes = pedido.produtos.ToList();

        foreach (var caixa in caixasDisponiveis)
        {
            var produtosQueCabem = new List<Produto>();

            foreach (var produto in produtosRestantes)
            {
                if (ProdutoCabeNaCaixa(produto, caixa))
                {
                    produtosQueCabem.Add(produto);
                }
            }

            if (produtosQueCabem.Any())
            {
                resultado.Add(new CaixaResultado
                {
                    CaixaId = caixa.CaixaId,
                    produtos = produtosQueCabem.Select(p => p.produto_id).ToList()
                });

                produtosRestantes = produtosRestantes.Except(produtosQueCabem).ToList();
            }

            if (!produtosRestantes.Any())
            {
                break;
            }
        }

        // Verifica se há produtos que não cabem em nenhuma caixa
        if (produtosRestantes.Any())
        {
            resultado.Add(new CaixaResultado
            {
                CaixaId = null,
                produtos = produtosRestantes.Select(p => p.produto_id).ToList(),
                Observacao = "Produto(s) não cabem em nenhuma caixa disponível."
            });
        }

        return resultado;
    }

    private bool ProdutoCabeNaCaixa(Produto produto, Caixa caixa)
    {
        return produto.Dimensoes.Altura <= caixa.Dimensoes.Altura &&
               produto.Dimensoes.Largura <= caixa.Dimensoes.Largura &&
               produto.Dimensoes.Comprimento <= caixa.Dimensoes.Comprimento;
    }
}

public class CaixaResultado
{
    public string CaixaId { get; set; }
    public List<string> produtos { get; set; }
    public string Observacao { get; set; }
}
