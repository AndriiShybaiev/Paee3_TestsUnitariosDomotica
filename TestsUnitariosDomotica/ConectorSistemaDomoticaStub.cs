using System;
using Domotica.io;

namespace Domotica.stubs
{
    /// <summary>
    /// Stub simulating the behavior of a domotics blind connector.
    /// Operates in increments/decrements of 0.1 between 0.0 (fully closed) and 1.0 (fully open).
    /// </summary>
    public class ConectorSistemaDomoticaStub : IConectorSistemaDomotica
    {
        private double posicionPersiana = 0.0;

        public double SubirPersiana()
        {
            if (posicionPersiana <= 0.9)
            {
                posicionPersiana = Math.Round(posicionPersiana + 0.1, 2);
            }
            else
            {
                posicionPersiana = 1.0;
            }

            return posicionPersiana;
        }

        public double BajarPersiana()
        {
            if (posicionPersiana >= 0.1)
            {
                posicionPersiana = Math.Round(posicionPersiana - 0.1, 2);
            }
            else
            {
                posicionPersiana = 0.0;
            }

            return posicionPersiana;
        }
    }
}
