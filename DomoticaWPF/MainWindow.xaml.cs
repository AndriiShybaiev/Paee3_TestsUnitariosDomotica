using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Domotica.io;
using Domotica.stubs;
using Res = DomoticaWPF.Properties.Resources;

namespace DomoticaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConectorSistemaDomotica _conector;
        private double _posicionActual = 0.0;
        private const double MaxBlindHeight = 220.0;

        public MainWindow()
        {
            // Load configuration and set UI culture before InitializeComponent
            Config.GetInstance().Load();
            CambiarIdioma(Config.GetInstance().Idioma);

            InitializeComponent();

            _conector = new ConectorSistemaDomoticaStub();
            Config.GetInstance().ConfigurationChanged += AplicaConfiguracion;

            AplicaConfiguracion();
            ActualizarEstadoVisual(0.0);

            Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 10, "MainWindow inicializada con éxito.");
        }

        public static void CambiarIdioma(string cultureCode)
        {
            try
            {
                var culture = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Res.Culture = culture;
            }
            catch (Exception ex)
            {
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Warning, 11, $"Error al aplicar cultura '{cultureCode}': {ex.Message}");
            }
        }

        private void AplicaConfiguracion()
        {
            try
            {
                var colorStr = Config.GetInstance().Color;
                if (!string.IsNullOrEmpty(colorStr))
                {
                    var converter = new BrushConverter();
                    if (converter.ConvertFromString(colorStr) is Brush brush)
                    {
                        Background = brush;
                    }
                }
            }
            catch (Exception ex)
            {
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Warning, 12, $"Error al aplicar color de fondo: {ex.Message}");
            }
        }

        private void ActualizarTextosLocalizados()
        {
            Title = Res.AppTitle;
            txtTitle.Text = Res.AppTitle;
            btnConfig.Content = Res.BtnConfig;
            btnSubir.Content = Res.BtnSubir;
            btnBajar.Content = Res.BtnBajar;
            txtNivelAperturaLabel.Text = Res.LblApertura;
            ActualizarEstadoVisual(_posicionActual);
        }

        private void ActualizarEstadoVisual(double posicion)
        {
            _posicionActual = posicion;
            int porcentaje = (int)Math.Round(_posicionActual * 100);

            // Blind height covers remaining closed portion: (1.0 - aperture) * MaxBlindHeight
            blindContainer.Height = Math.Max(0, MaxBlindHeight * (1.0 - _posicionActual));

            pbApertura.Value = porcentaje;
            txtPorcentaje.Text = $"{porcentaje}%";

            if (porcentaje == 0)
            {
                txtEstadoDescripcion.Text = Res.LblEstadoCerrado;
            }
            else if (porcentaje == 100)
            {
                txtEstadoDescripcion.Text = Res.LblEstadoAbierto;
            }
            else
            {
                txtEstadoDescripcion.Text = string.Format(Res.LblEstadoParcial, porcentaje);
            }
        }

        private void BtnSubir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double nuevaPos = _conector.SubirPersiana();
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 20, $"Acción SubirPersiana ejecutada. Nueva posición={nuevaPos:F2}");
                ActualizarEstadoVisual(nuevaPos);
            }
            catch (Exception ex)
            {
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Error, 21, $"Error al subir persiana: {ex.Message}");
            }
        }

        private void BtnBajar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double nuevaPos = _conector.BajarPersiana();
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 22, $"Acción BajarPersiana ejecutada. Nueva posición={nuevaPos:F2}");
                ActualizarEstadoVisual(nuevaPos);
            }
            catch (Exception ex)
            {
                Config.GetInstance().Logger.TraceEvent(TraceEventType.Error, 23, $"Error al bajar persiana: {ex.Message}");
            }
        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            var configDlg = new WindowConfig { Owner = this };
            bool? resultado = configDlg.ShowDialog();

            if (resultado == true)
            {
                CambiarIdioma(Config.GetInstance().Idioma);
                ActualizarTextosLocalizados();
                AplicaConfiguracion();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 30, "MainWindow cerrada.");
            base.OnClosed(e);
        }
    }
}
