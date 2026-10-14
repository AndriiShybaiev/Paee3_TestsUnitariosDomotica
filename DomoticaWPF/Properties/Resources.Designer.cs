namespace DomoticaWPF.Properties {
    using System;
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DomoticaWPF.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string AppTitle => ResourceManager.GetString("AppTitle", resourceCulture) ?? "Control Domótico de Persianas";
        public static string BtnSubir => ResourceManager.GetString("BtnSubir", resourceCulture) ?? "▲ Subir Persiana";
        public static string BtnBajar => ResourceManager.GetString("BtnBajar", resourceCulture) ?? "▼ Bajar Persiana";
        public static string BtnConfig => ResourceManager.GetString("BtnConfig", resourceCulture) ?? "⚙ Personalización";
        public static string LblApertura => ResourceManager.GetString("LblApertura", resourceCulture) ?? "Nivel de apertura:";
        public static string LblEstadoCerrado => ResourceManager.GetString("LblEstadoCerrado", resourceCulture) ?? "Persiana cerrada (0%)";
        public static string LblEstadoAbierto => ResourceManager.GetString("LblEstadoAbierto", resourceCulture) ?? "Persiana completamente abierta (100%)";
        public static string LblEstadoParcial => ResourceManager.GetString("LblEstadoParcial", resourceCulture) ?? "Persiana parcialmente abierta ({0}%)";
        public static string ConfigTitle => ResourceManager.GetString("ConfigTitle", resourceCulture) ?? "Personalización del Sistema";
        public static string ConfigColorGroup => ResourceManager.GetString("ConfigColorGroup", resourceCulture) ?? "Color de Fondo de la Ventana";
        public static string ConfigIdiomaGroup => ResourceManager.GetString("ConfigIdiomaGroup", resourceCulture) ?? "Idioma de la Interfaz";
        public static string IdiomaEspanol => ResourceManager.GetString("IdiomaEspanol", resourceCulture) ?? "Español";
        public static string IdiomaIngles => ResourceManager.GetString("IdiomaIngles", resourceCulture) ?? "English (Inglés)";
        public static string BtnAceptar => ResourceManager.GetString("BtnAceptar", resourceCulture) ?? "Aceptar";
        public static string BtnCancelar => ResourceManager.GetString("BtnCancelar", resourceCulture) ?? "Cancelar";
        public static string BtnCerrar => ResourceManager.GetString("BtnCerrar", resourceCulture) ?? "Cerrar";
        public static string ColorAzul => ResourceManager.GetString("ColorAzul", resourceCulture) ?? "Azul Claro";
        public static string ColorVerde => ResourceManager.GetString("ColorVerde", resourceCulture) ?? "Verde Menta";
        public static string ColorGris => ResourceManager.GetString("ColorGris", resourceCulture) ?? "Gris Suave";
        public static string ColorCalido => ResourceManager.GetString("ColorCalido", resourceCulture) ?? "Arena Cálido";
        public static string LblStubInfo => ResourceManager.GetString("LblStubInfo", resourceCulture) ?? "Stub: ConectorSistemaDomoticaStub (Paso = 10%)";
        public static string ConfigColorCustom => ResourceManager.GetString("ConfigColorCustom", resourceCulture) ?? "Color personalizado (HEX):";
        public static string BtnAplicarColor => ResourceManager.GetString("BtnAplicarColor", resourceCulture) ?? "Aplicar";
    }
}
