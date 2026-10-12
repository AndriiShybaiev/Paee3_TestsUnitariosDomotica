using System;
using System.Diagnostics;
using DomoticaWPF;
using NUnit.Framework;

namespace TestsUnitariosDomotica
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void Test_Config_EsSingleton()
        {
            Config c1 = Config.GetInstance();
            Config c2 = Config.GetInstance();

            Assert.That(c1, Is.Not.Null);
            Assert.That(c2, Is.SameAs(c1));
        }

        [Test]
        public void Test_Config_ValoresYNotificacionCambio()
        {
            Config config = Config.GetInstance();
            bool notificado = false;
            Action handler = () => notificado = true;

            config.ConfigurationChanged += handler;
            config.Color = "#E8F5E9";
            config.NotifyChanged();

            Assert.That(notificado, Is.True);
            Assert.That(config.Color, Is.EqualTo("#E8F5E9"));

            config.ConfigurationChanged -= handler;
        }

        [Test]
        public void Test_Config_BackupYRestore_CancelarRestauraValores()
        {
            Config config = Config.GetInstance();
            config.Color = "#E8F4F8";
            config.Idioma = "es-ES";
            config.WindowTop = 100.0;
            config.WindowLeft = 200.0;
            config.CreateBackup();

            // Simulate user changing values in dialog
            config.Color = "#FFF8E1";
            config.Idioma = "en-US";
            config.WindowTop = 300.0;
            config.WindowLeft = 400.0;

            // User cancels dialog -> restore
            config.RestoreBackup();

            Assert.That(config.Color, Is.EqualTo("#E8F4F8"));
            Assert.That(config.Idioma, Is.EqualTo("es-ES"));
            Assert.That(config.WindowTop, Is.EqualTo(100.0));
            Assert.That(config.WindowLeft, Is.EqualTo(200.0));
        }

        [Test]
        public void Test_Config_WindowDimensionsProperties()
        {
            Config config = Config.GetInstance();
            config.WindowWidth = 550.0;
            config.WindowHeight = 450.0;

            Assert.That(config.WindowWidth, Is.EqualTo(550.0));
            Assert.That(config.WindowHeight, Is.EqualTo(450.0));
        }

        [Test]
        public void Test_Config_LoggerTraceSourceInicializado()
        {
            Config config = Config.GetInstance();
            Assert.That(config.Logger, Is.Not.Null);
            Assert.That(config.Logger.Name, Is.EqualTo("DomoticaWPF"));

            // Verify logging calls execute without errors
            Assert.DoesNotThrow(() =>
            {
                config.Logger.TraceEvent(TraceEventType.Information, 999, "Test trace event message");
            });
        }

        [Test]
        public void Test_Config_SaveAndLoad_NoLanzanExcepcion()
        {
            Config config = Config.GetInstance();
            Assert.DoesNotThrow(() => config.Save());
            Assert.DoesNotThrow(() => config.Load());
        }
    }
}
