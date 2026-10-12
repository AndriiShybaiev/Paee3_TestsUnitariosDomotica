using System;
using Domotica.io;
using Domotica.stubs;
using NUnit.Framework;

namespace TestsUnitariosDomotica
{
    [TestFixture]
    public class SistemaEntradaSalidaTests
    {
        private IConectorSistemaDomotica conector;

        [SetUp]
        public void SetUp()
        {
            conector = new ConectorSistemaDomoticaStub();
        }

        [Test]
        public void Test_EstadoInicial_PersianaCerrada()
        {
            // Initial position is 0.0 (closed). Lowering it must maintain 0.0.
            double posicion = conector.BajarPersiana();
            Assert.That(posicion, Is.EqualTo(0.0).Within(0.001));
        }

        [Test]
        public void Test_SubirPersiana_UnPaso()
        {
            // Moving up one step from 0.0 should result in 0.1
            double posicion = conector.SubirPersiana();
            Assert.That(posicion, Is.EqualTo(0.1).Within(0.001));
        }

        [Test]
        public void Test_BajarPersiana_UnPaso()
        {
            // Moving up to 0.1 then down one step should return to 0.0
            conector.SubirPersiana();
            double posicion = conector.BajarPersiana();
            Assert.That(posicion, Is.EqualTo(0.0).Within(0.001));
        }

        [Test]
        public void Test_LimiteSuperior_NoSuperaUno()
        {
            // Invoking SubirPersiana repeatedly (e.g. 15 times) must never exceed 1.0
            double posicion = 0.0;
            for (int i = 0; i < 15; i++)
            {
                posicion = conector.SubirPersiana();
            }

            Assert.That(posicion, Is.EqualTo(1.0).Within(0.001));
        }

        [Test]
        public void Test_LimiteInferior_NoBajaDeCero()
        {
            // Invoking BajarPersiana repeatedly on closed blinds must never go below 0.0
            double posicion = 0.0;
            for (int i = 0; i < 15; i++)
            {
                posicion = conector.BajarPersiana();
            }

            Assert.That(posicion, Is.EqualTo(0.0).Within(0.001));
        }

        [Test]
        public void Test_Actividad1_AbrirYCerrarPersianaPasosFinitos()
        {
            // Actividad 1: Verify the blind reaches 1.0 (fully open) and closes back to 0.0 in finite deterministic steps (10 steps each).
            const int maxIntentos = 20;
            int pasosSubida = 0;
            double posicionActual = 0.0;

            while (posicionActual < 1.0 && pasosSubida < maxIntentos)
            {
                posicionActual = conector.SubirPersiana();
                pasosSubida++;
            }

            Assert.That(posicionActual, Is.EqualTo(1.0).Within(0.001), "The blind must reach full aperture (1.0).");
            Assert.That(pasosSubida, Is.EqualTo(10), "Full aperture must be reached in exactly 10 steps of 0.1.");

            int pasosBajada = 0;
            while (posicionActual > 0.0 && pasosBajada < maxIntentos)
            {
                posicionActual = conector.BajarPersiana();
                pasosBajada++;
            }

            Assert.That(posicionActual, Is.EqualTo(0.0).Within(0.001), "The blind must reach full closure (0.0).");
            Assert.That(pasosBajada, Is.EqualTo(10), "Full closure must be reached in exactly 10 steps of 0.1.");
        }
    }
}
