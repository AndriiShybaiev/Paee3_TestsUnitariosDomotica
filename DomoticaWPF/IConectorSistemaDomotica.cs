namespace Domotica.io
{
    /// <summary>
    /// Interface for domotic blinds control.
    /// Default position: 0.0 (closed / 0% aperture), max position: 1.0 (fully open / 100% aperture).
    /// </summary>
    public interface IConectorSistemaDomotica
    {
        double SubirPersiana();
        double BajarPersiana();
    }
}
