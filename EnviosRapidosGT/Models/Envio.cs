namespace EnviosRapidosGT.Models;

public class Envio
{
    public int Id { get; set; }

    public string CodigoRastreo { get; set; } = string.Empty;

    public decimal PesoKg { get; set; }

    public decimal Tarifa { get; set; }

    public EstadoEnvio Estado { get; set; }

    public int IntentosEntrega { get; set; }

    public Cliente Remitente { get; set; } = new();

    public Cliente Destinatario { get; set; } = new();

    public List<HistorialEstado> Historial { get; set; } = new();
}