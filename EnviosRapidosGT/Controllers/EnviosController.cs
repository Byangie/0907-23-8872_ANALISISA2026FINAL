using EnviosRapidosGT.Data;
using EnviosRapidosGT.Models;
using EnviosRapidosGT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnviosController : ControllerBase
{
    private readonly EnvioService _envioService;
    private readonly AppDbContext _context;

    public EnviosController(EnvioService envioService, AppDbContext context)
    {
        _envioService = envioService;
        _context = context;
    }

    [HttpGet]
    public IActionResult Obtener()
    {
        var envios = _context.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.Historial)
            .ToList();

        return Ok(envios);
    }

    [HttpGet("{codigo}")]
    public IActionResult ObtenerPorCodigo(string codigo)
    {
        var envio = _context.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.Historial)
            .FirstOrDefault(e => e.CodigoRastreo == codigo);

        if (envio == null)
            return NotFound("Envío no encontrado");

        return Ok(envio);
    }

    [HttpPost]
    public IActionResult CrearEnvio([FromBody] Envio envio)
    {
        int correlativo = _context.Envios.Count() + 1;

        envio.CodigoRastreo = _envioService.GenerarCodigoRastreo(correlativo);
        envio.Estado = EstadoEnvio.Registrado;

        var tarifaBase = _envioService.CalcularTarifa(envio.PesoKg);

        bool remitenteTieneNit = !string.IsNullOrWhiteSpace(envio.Remitente.Nit);
        bool destinatarioTieneNit = !string.IsNullOrWhiteSpace(envio.Destinatario.Nit);

        envio.Tarifa = _envioService.AplicarDescuento(
            tarifaBase,
            remitenteTieneNit,
            destinatarioTieneNit
        );

        envio.Historial.Add(new HistorialEstado
        {
            Estado = envio.Estado.ToString(),
            Ubicacion = "Oficina Central",
            FechaHora = DateTime.Now,
            Notas = "Envío registrado"
        });

        _context.Envios.Add(envio);
        _context.SaveChanges();

        return Ok(envio);
    }

    [HttpPut("{codigo}/estado")]
    public IActionResult ActualizarEstado(
        string codigo,
        EstadoEnvio nuevoEstado,
        string ubicacion,
        string? notas = null)
    {
        var envio = _context.Envios
            .Include(e => e.Historial)
            .FirstOrDefault(e => e.CodigoRastreo == codigo);

        if (envio == null)
            return NotFound("Envío no encontrado");

        bool actualizado = _envioService.ActualizarEstado(envio, nuevoEstado, ubicacion, notas);

        if (!actualizado)
            return BadRequest("Cambio de estado inválido o ubicación vacía");

        _context.SaveChanges();

        return Ok(envio);
    }

    [HttpPost("{codigo}/intento-fallido")]
    public IActionResult RegistrarIntentoFallido(
        string codigo,
        string ubicacion,
        string? notas = null)
    {
        var envio = _context.Envios
            .Include(e => e.Historial)
            .FirstOrDefault(e => e.CodigoRastreo == codigo);

        if (envio == null)
            return NotFound("Envío no encontrado");

        _envioService.RegistrarIntentoFallido(envio, ubicacion, notas);

        _context.SaveChanges();

        return Ok(envio);
    }

    [HttpGet("{codigo}/historial")]
    public IActionResult ObtenerHistorial(string codigo)
    {
        var envio = _context.Envios
            .Include(e => e.Historial)
            .FirstOrDefault(e => e.CodigoRastreo == codigo);

        if (envio == null)
            return NotFound("Envío no encontrado");

        return Ok(envio.Historial);
    }
}