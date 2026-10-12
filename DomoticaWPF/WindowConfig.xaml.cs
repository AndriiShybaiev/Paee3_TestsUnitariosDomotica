using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DomoticaWPF
{
    public partial class WindowConfig : Window
    {
        private readonly Config _config;
        private string _selectedColor;
        private string _selectedIdioma;

        public WindowConfig()
        {
            InitializeComponent();
            _config = Config.GetInstance();
            _config.CreateBackup();

            _selectedColor = _config.Color;
            _selectedIdioma = _config.Idioma;

            InitializeFormValues();
            _config.Logger.TraceEvent(TraceEventType.Information, 200, "Diálogo de configuración abierto.");
        }

        private void InitializeFormValues()
        {
            // Set Color radio buttons
            if (rbColorVerde.Tag?.ToString() == _selectedColor)
                rbColorVerde.IsChecked = true;
            else if (rbColorGris.Tag?.ToString() == _selectedColor)
                rbColorGris.IsChecked = true;
            else if (rbColorCalido.Tag?.ToString() == _selectedColor)
                rbColorCalido.IsChecked = true;
            else
                rbColorAzul.IsChecked = true;

            // Set Language radio buttons
            if (_selectedIdioma.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                rbIdiomaEn.IsChecked = true;
            else
                rbIdiomaEs.IsChecked = true;
        }

        private void OnColorOptionChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton { Tag: string colorTag })
            {
                _selectedColor = colorTag;
                // Live preview if desired
                _config.Color = _selectedColor;
                _config.NotifyChanged();
            }
        }

        private void OnIdiomaOptionChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton { Tag: string idiomaTag })
            {
                _selectedIdioma = idiomaTag;
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            _config.Color = _selectedColor;
            _config.Idioma = _selectedIdioma;
            _config.Save();
            _config.Logger.TraceEvent(TraceEventType.Information, 201, "Cambios de configuración aceptados y guardados.");
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _config.RestoreBackup();
            _config.Logger.TraceEvent(TraceEventType.Information, 202, "Cambios de configuración cancelados. Valores restaurados.");
            DialogResult = false;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            _config.RestoreBackup();
            _config.Logger.TraceEvent(TraceEventType.Information, 203, "Diálogo de configuración cerrado sin guardar.");
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
