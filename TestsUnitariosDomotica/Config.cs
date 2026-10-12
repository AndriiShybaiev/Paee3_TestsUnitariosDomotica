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
        public double? WindowTop { get; set; }
        public double? WindowLeft { get; set; }
        public double? WindowWidth { get; set; }
        public double? WindowHeight { get; set; }

        // Snapshot fields for Cancel / Revert support in settings dialog
        private string _backupColor = "#E8F4F8";
        private string _backupIdioma = "es-ES";
        private double? _backupWindowTop;
        private double? _backupWindowLeft;
        private double? _backupWindowWidth;
        private double? _backupWindowHeight;

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
            _backupWindowTop = WindowTop;
            _backupWindowLeft = WindowLeft;
            _backupWindowWidth = WindowWidth;
            _backupWindowHeight = WindowHeight;
        }

        public void RestoreBackup()
        {
            Color = _backupColor;
            Idioma = _backupIdioma;
            WindowTop = _backupWindowTop;
            WindowLeft = _backupWindowLeft;
            WindowWidth = _backupWindowWidth;
            WindowHeight = _backupWindowHeight;
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
                if (config.AppSettings.Settings["windowTop"] != null &&
                    double.TryParse(config.AppSettings.Settings["windowTop"].Value, System.Globalization.CultureInfo.InvariantCulture, out double top))
                {
                    WindowTop = top;
                }
                if (config.AppSettings.Settings["windowLeft"] != null &&
                    double.TryParse(config.AppSettings.Settings["windowLeft"].Value, System.Globalization.CultureInfo.InvariantCulture, out double left))
                {
                    WindowLeft = left;
                }
                if (config.AppSettings.Settings["windowWidth"] != null &&
                    double.TryParse(config.AppSettings.Settings["windowWidth"].Value, System.Globalization.CultureInfo.InvariantCulture, out double width))
                {
                    WindowWidth = width;
                }
                if (config.AppSettings.Settings["windowHeight"] != null &&
                    double.TryParse(config.AppSettings.Settings["windowHeight"].Value, System.Globalization.CultureInfo.InvariantCulture, out double height))
                {
                    WindowHeight = height;
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

                SetAppSetting(config, "color", Color);
                SetAppSetting(config, "idioma", Idioma);
                if (WindowTop.HasValue) SetAppSetting(config, "windowTop", WindowTop.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (WindowLeft.HasValue) SetAppSetting(config, "windowLeft", WindowLeft.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (WindowWidth.HasValue) SetAppSetting(config, "windowWidth", WindowWidth.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (WindowHeight.HasValue) SetAppSetting(config, "windowHeight", WindowHeight.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));

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

        private static void SetAppSetting(System.Configuration.Configuration config, string key, string value)
        {
            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
        }
    }
}
