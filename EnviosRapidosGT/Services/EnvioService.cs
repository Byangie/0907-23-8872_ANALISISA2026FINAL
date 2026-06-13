using EnviosRapidosGT.Models;

namespace EnviosRapidosGT.Services;

public class EnvioService
{
    public decimal CalcularTarifa(decimal pesoKg)
    {
        if (pesoKg <= 1) return 25;
        if (pesoKg <= 5) return 45;
        if (pesoKg <= 10) return 75;
        return 100;
    }

    public decimal AplicarDescuento(decimal tarifa, bool remitenteTieneNit, bool destinatarioTieneNit)
    {
        if (remitenteTieneNit || destinatarioTieneNit)
            return tarifa * 0.95m;

        return tarifa;
    }

    public string GenerarCodigoRastreo(int correlativo)
    {
        return $"ENV-{DateTime.Now:yyyyMMdd}-{correlativo.ToString("D4")}";
    }

    public void RegistrarIntentoFallido(Envio envio, string ubicacion, string? notas = null)
    {
        envio.IntentosEntrega++;

        if (envio.IntentosEntrega >= 3)
        {
            ActualizarEstado(envio, EstadoEnvio.EnDevolucion, ubicacion, notas);
        }
    }

    public bool ActualizarEstado(Envio envio, EstadoEnvio nuevoEstado, string ubicacion, string? notas = null)
    {
        if (string.IsNullOrWhiteSpace(ubicacion))
            return false;

        if (!EsCambioValido(envio.Estado, nuevoEstado))
            return false;

        envio.Estado = nuevoEstado;

        envio.Historial.Add(new HistorialEstado
        {
            Estado = nuevoEstado.ToString(),
            Ubicacion = ubicacion,
            FechaHora = DateTime.Now,
            Notas = notas
        });

        return true;
    }

    public bool EsCambioValido(EstadoEnvio estadoActual, EstadoEnvio nuevoEstado)
    {
        return estadoActual switch
        {
            EstadoEnvio.Registrado => nuevoEstado == EstadoEnvio.EnTransito,

            EstadoEnvio.EnTransito => nuevoEstado == EstadoEnvio.EnReparto,

            EstadoEnvio.EnReparto =>
                nuevoEstado == EstadoEnvio.Entregado ||
                nuevoEstado == EstadoEnvio.EnDevolucion ||
                nuevoEstado == EstadoEnvio.Devuelto,

            EstadoEnvio.EnDevolucion => nuevoEstado == EstadoEnvio.Devuelto,

            _ => false
        };
    }
}