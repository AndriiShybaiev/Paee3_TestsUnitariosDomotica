using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomoticaWPF
{
    public partial class WindowConfig : Window
    {
        private readonly Config _config;
        private string _selectedColor;
        private string _selectedIdioma;
        private bool _saved = false;
        private bool _isUpdatingText = false;

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
            UpdateHexAndPreview(_selectedColor, updateText: true);

            // Set Color radio buttons
            if (rbColorVerde.Tag?.ToString() == _selectedColor)
                rbColorVerde.IsChecked = true;
            else if (rbColorGris.Tag?.ToString() == _selectedColor)
                rbColorGris.IsChecked = true;
            else if (rbColorCalido.Tag?.ToString() == _selectedColor)
                rbColorCalido.IsChecked = true;
            else if (rbColorAzul.Tag?.ToString() == _selectedColor)
                rbColorAzul.IsChecked = true;

            // Set Language radio buttons
            if (_selectedIdioma.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                rbIdiomaEn.IsChecked = true;
            else
                rbIdiomaEs.IsChecked = true;
        }

        private void UpdateHexAndPreview(string hexColor, bool updateText)
        {
            try
            {
                var brush = (Brush)new BrushConverter().ConvertFromString(hexColor)!;
                colorPreviewBox.Background = brush;
                if (updateText)
                {
                    _isUpdatingText = true;
                    txtCustomHex.Text = hexColor;
                    _isUpdatingText = false;
                }
            }
            catch
            {
                // Invalid color format
            }
        }

        private void OnColorOptionChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton { Tag: string colorTag })
            {
                _selectedColor = colorTag;
                UpdateHexAndPreview(_selectedColor, updateText: true);
                _config.Color = _selectedColor;
                _config.NotifyChanged();
            }
        }

        private void TxtCustomHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;

            string hex = txtCustomHex.Text.Trim();
            if (!hex.StartsWith("#"))
                hex = "#" + hex;

            try
            {
                var brush = (Brush)new BrushConverter().ConvertFromString(hex)!;
                colorPreviewBox.Background = brush;
                _selectedColor = hex;
                _config.Color = _selectedColor;
                _config.NotifyChanged();
            }
            catch
            {
                // Not a valid color yet while typing
            }
        }

        private void BtnAplicarHex_Click(object sender, RoutedEventArgs e)
        {
            string hex = txtCustomHex.Text.Trim();
            if (!hex.StartsWith("#"))
                hex = "#" + hex;

            try
            {
                var brush = (Brush)new BrushConverter().ConvertFromString(hex)!;
                colorPreviewBox.Background = brush;
                _selectedColor = hex;
                _config.Color = _selectedColor;
                _config.NotifyChanged();
            }
            catch
            {
                MessageBox.Show("Formato de color hexadecimal no válido (ejemplo: #E8F4F8 o #FFFFFF).", "Color Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            bool ok = _config.Save();

            if (ok)
            {
                _saved = true;
                _config.Logger.TraceEvent(TraceEventType.Information, 201, "Cambios de configuración aceptados y guardados.");
                DialogResult = true;
                Close();
            }
            else
            {
                _config.Logger.TraceEvent(TraceEventType.Error, 204, "Error al guardar la configuración.");
                MessageBox.Show("No se pudo guardar la configuración en el archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _saved = false;
            DialogResult = false;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            _saved = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!_saved)
            {
                _config.RestoreBackup();
                _config.Logger.TraceEvent(TraceEventType.Information, 202, "Cambios de configuración cancelados / ventana cerrada. Valores restaurados.");
            }

            base.OnClosed(e);
        }
    }
}
