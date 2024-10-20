using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly EmbalagemService _embalagemService;

    public PedidosController(EmbalagemService embalagemService)
    {
        _embalagemService = embalagemService;
    }

    public class PedidoRequest
    {
        public List<Pedido> Pedidos { get; set; }
    }

    [HttpPost]
    public IActionResult ProcessarPedidos([FromBody] PedidoRequest pedidoRequest)
    {
        if (pedidoRequest == null || pedidoRequest.Pedidos == null || !pedidoRequest.Pedidos.Any())
        {
            return BadRequest("Pedidos não podem ser nulos ou vazios.");
        }

        var resultado = new List<object>();

        foreach (var pedido in pedidoRequest.Pedidos)
        {
            var caixas = _embalagemService.CalcularCaixas(pedido);
            resultado.Add(new { PedidoId = pedido.pedido_id, Caixas = caixas });
        }

        return Ok(new { Pedidos = resultado });
    }
}
