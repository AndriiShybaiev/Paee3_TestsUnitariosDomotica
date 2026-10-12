using System;
using System.Configuration;
using System.Diagnostics;

namespace DomoticaWPF
{
    /// <summary>
    /// Singleton class managing application configuration, persistence and logging.
    /// </summary>
    public class Config
    {
        private static Config? _instance;
        private static readonly object _lock = new();

        public string Color { get; set; } = "#E8F4F8";
        public string Idioma { get; set; } = "es-ES";

        // Snapshot fields for Cancel / Revert support in settings dialog
        private string _backupColor = "#E8F4F8";
        private string _backupIdioma = "es-ES";

        public TraceSource Logger { get; }

        public event Action? ConfigurationChanged;

        private Config()
        {
            Logger = new TraceSource("DomoticaWPF");
        }

        public static Config GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new Config();
                }
            }
            return _instance;
        }

        public void CreateBackup()
        {
            _backupColor = Color;
            _backupIdioma = Idioma;
        }

        public void RestoreBackup()
        {
            Color = _backupColor;
            Idioma = _backupIdioma;
            NotifyChanged();
        }

        public void NotifyChanged()
        {
            ConfigurationChanged?.Invoke();
        }

        public void Load()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["color"] != null)
                {
                    Color = config.AppSettings.Settings["color"].Value;
                }
                if (config.AppSettings.Settings["idioma"] != null)
                {
                    Idioma = config.AppSettings.Settings["idioma"].Value;
                }
                CreateBackup();
                Logger.TraceEvent(TraceEventType.Information, 100, $"Configuración cargada. Color={Color}, Idioma={Idioma}");
            }
            catch (Exception ex)
            {
                Logger.TraceEvent(TraceEventType.Error, 101, $"Error al cargar configuración: {ex.Message}");
            }
        }

        public void Save()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings["color"] == null)
                    config.AppSettings.Settings.Add("color", Color);
                else
                    config.AppSettings.Settings["color"].Value = Color;

                if (config.AppSettings.Settings["idioma"] == null)
                    config.AppSettings.Settings.Add("idioma", Idioma);
                else
                    config.AppSettings.Settings["idioma"].Value = Idioma;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                CreateBackup();
                Logger.TraceEvent(TraceEventType.Information, 102, $"Configuración guardada correctamente. Color={Color}, Idioma={Idioma}");
                NotifyChanged();
            }
            catch (Exception ex)
            {
                Logger.TraceEvent(TraceEventType.Error, 103, $"Error al guardar configuración: {ex.Message}");
            }
        }
    }
}
