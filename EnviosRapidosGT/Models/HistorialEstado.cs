namespace EnviosRapidosGT.Models;

public class HistorialEstado
{
    public int Id { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string Ubicacion { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; }

    public string? Notas { get; set; }
}