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

            // Verify logging calls execute without errors and can trace messages
            Assert.DoesNotThrow(() =>
            {
                config.Logger.TraceEvent(TraceEventType.Information, 999, "Test trace event message from unit test");
                config.Logger.Flush();
            });

            // Verify listeners collection contains active listener
            Assert.That(config.Logger.Listeners.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Test_Config_SaveAndLoad_PersistenciaValoresReales()
        {
            Config config = Config.GetInstance();
            string originalColor = config.Color;
            string originalIdioma = config.Idioma;
            double? originalTop = config.WindowTop;
            double? originalLeft = config.WindowLeft;
            double? originalWidth = config.WindowWidth;
            double? originalHeight = config.WindowHeight;

            try
            {
                // 1. Set specific configuration
                config.Color = "#FFF8E1";
                config.Idioma = "en-US";
                config.WindowTop = 155.0;
                config.WindowLeft = 255.0;
                config.WindowWidth = 560.0;
                config.WindowHeight = 460.0;

                // 2. Save configuration to persistent storage
                bool saved = config.Save();
                Assert.That(saved, Is.True, "Config.Save() must return true on success.");

                // 3. Mutate values in memory
                config.Color = "#112233";
                config.Idioma = "es-ES";
                config.WindowTop = 10.0;
                config.WindowLeft = 20.0;
                config.WindowWidth = 300.0;
                config.WindowHeight = 300.0;

                // 4. Reload configuration from persistent storage
                bool loaded = config.Load();
                Assert.That(loaded, Is.True, "Config.Load() must return true on success.");

                // 5. Assert that values match what was persisted
                Assert.That(config.Color, Is.EqualTo("#FFF8E1"));
                Assert.That(config.Idioma, Is.EqualTo("en-US"));
                Assert.That(config.WindowTop, Is.EqualTo(155.0));
                Assert.That(config.WindowLeft, Is.EqualTo(255.0));
                Assert.That(config.WindowWidth, Is.EqualTo(560.0));
                Assert.That(config.WindowHeight, Is.EqualTo(460.0));
            }
            finally
            {
                // Restore original state
                config.Color = originalColor;
                config.Idioma = originalIdioma;
                config.WindowTop = originalTop;
                config.WindowLeft = originalLeft;
                config.WindowWidth = originalWidth;
                config.WindowHeight = originalHeight;
                config.Save();
            }
        }

        [Test]
        public void Test_Config_SaveAndLoad_NoLanzanExcepcion()
        {
            Config config = Config.GetInstance();
            Assert.That(config.Save(), Is.True);
            Assert.That(config.Load(), Is.True);
        }
    }
}
