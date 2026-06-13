using EnviosRapidosGT.Models;
using EnviosRapidosGT.Services;

namespace EnviosRapidosGT.Tests;

public class EnvioServiceTests
{
    [Fact]
    public void CalcularTarifa_PesoMenorIgual1_Retorna25()
    {
        var service = new EnvioService();

        var resultado = service.CalcularTarifa(1);

        Assert.Equal(25, resultado);
    }

    [Fact]
    public void CalcularTarifa_PesoEntre1Y5_Retorna45()
    {
        var service = new EnvioService();

        var resultado = service.CalcularTarifa(3);

        Assert.Equal(45, resultado);
    }

    [Fact]
    public void CalcularTarifa_PesoEntre5Y10_Retorna75()
    {
        var service = new EnvioService();

        var resultado = service.CalcularTarifa(8);

        Assert.Equal(75, resultado);
    }

    [Fact]
    public void CalcularTarifa_PesoMayor10_Retorna100()
    {
        var service = new EnvioService();

        var resultado = service.CalcularTarifa(15);

        Assert.Equal(100, resultado);
    }

    [Fact]
    public void AplicarDescuento_ConNit_Retorna95PorCiento()
    {
        var service = new EnvioService();

        var resultado = service.AplicarDescuento(100, true, false);

        Assert.Equal(95, resultado);
    }

    [Fact]
    public void CambioValido_RegistradoAEnTransito_RetornaTrue()
    {
        var service = new EnvioService();

        var resultado = service.EsCambioValido(EstadoEnvio.Registrado, EstadoEnvio.EnTransito);

        Assert.True(resultado);
    }

    [Fact]
    public void CambioInvalido_EntregadoARegistrado_RetornaFalse()
    {
        var service = new EnvioService();

        var resultado = service.EsCambioValido(EstadoEnvio.Entregado, EstadoEnvio.Registrado);

        Assert.False(resultado);
    }

    [Fact]
    public void RegistrarIntentoFallido_TercerIntento_CambiaAEnDevolucion()
    {
        var service = new EnvioService();
        var envio = new Envio
        {
            Estado = EstadoEnvio.EnReparto
        };

        service.RegistrarIntentoFallido(envio, "Jalapa");
        service.RegistrarIntentoFallido(envio, "Jalapa");
        service.RegistrarIntentoFallido(envio, "Jalapa");

        Assert.Equal(EstadoEnvio.EnDevolucion, envio.Estado);
    }

    [Fact]
    public void ActualizarEstado_ConUbicacion_GuardaHistorial()
    {
        var service = new EnvioService();
        var envio = new Envio
        {
            Estado = EstadoEnvio.Registrado
        };

        service.ActualizarEstado(envio, EstadoEnvio.EnTransito, "Guatemala");

        Assert.Single(envio.Historial);
    }

    [Fact]
    public void GenerarCodigoRastreo_FormatoCorrecto()
    {
        var service = new EnvioService();

        var codigo = service.GenerarCodigoRastreo(1);

        Assert.StartsWith("ENV-", codigo);
        Assert.EndsWith("0001", codigo);
    }

    [Fact]
    public void AplicarDescuento_SinNit_NoAplicaDescuento()
    {
        var service = new EnvioService();

        var resultado = service.AplicarDescuento(100, false, false);

        Assert.Equal(100, resultado);
    }

    [Fact]
    public void ActualizarEstado_SinUbicacion_RetornaFalse()
    {
        var service = new EnvioService();

        var envio = new Envio
        {
            Estado = EstadoEnvio.Registrado
        };

        var resultado = service.ActualizarEstado(
            envio,
            EstadoEnvio.EnTransito,
            ""
        );

        Assert.False(resultado);
    }


}