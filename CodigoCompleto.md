## Archivo: .\FoodWare\Controller\Program.cs

`csharp
using FoodWare.View.Forms;
namespace FoodWare.Controller
{
    internal static class Program
    {
        /// <summary>
        ///  Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. Inicializa la configuración de la app (fuentes, DPI, etc.)
            ApplicationConfiguration.Initialize();

            // 2. Creamos y mostramos el LoginForm primero como un Diálogo (ShowDialog).
            // La ejecución del código se detiene aquí hasta que el LoginForm se cierre.
            LoginForm loginForm = new();
            loginForm.ShowDialog();

            // 3. Revisamos el resultado DESPUÉS de que se cerró el login.
            // (El login nos devuelve "DialogResult.OK" si el usuario y pass fueron correctos).
            if (loginForm.DialogResult == DialogResult.OK)
            {
                // 4. Si el login fue exitoso, lanzamos la aplicación principal (FormMain).
                Application.Run(new FormMain());
            }

            // Si el resultado NO es OK (el usuario cerró la ventana [X]), 
            // la función Main() termina y la aplicación se cierra limpiamente.
        }
    }
}
``n
## Archivo: .\FoodWare\Controller\Logic\InventarioController.cs

`csharp
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess; // Necesario para crear el Mock

namespace FoodWare.Controller.Logic
{
    public class InventarioController
    {
        // El controlador SÓLO conoce la interfaz
        private readonly ProductoMockRepository _repositorio;

        public InventarioController()
        {
            // Le decimos que use el Repositorio FALSO.
            _repositorio = new ProductoMockRepository();

            // Cuando tengamos la BD, solo tendremos que camiar esta linea:
            // _repositorio = new ProductoSqlRepository(); 
        }

        // El controlador expone los métodos que la Vista necesita
        public List<Producto> CargarProductos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void GuardarNuevoProducto(string nombre, string categoria, int stock, decimal precio)
        {
            // Aquí podríamos agregar validaciones
            if (string.IsNullOrEmpty(nombre) || stock < 0 || precio < 0)
            {
                throw new System.Exception("Datos del producto inválidos.");
            }

            Producto nuevo = new Producto
            {
                Nombre = nombre,
                Categoria = categoria,
                StockActual = stock,
                PrecioCosto = precio
            };
            _repositorio.Agregar(nuevo);
        }

        public void EliminarProducto(int id)
        {
            _repositorio.Eliminar(id);
        }

        // Aquí iran métodos para Actualizar...
    }
}
``n
## Archivo: .\FoodWare\Controller\Logic\LoginController.cs

`csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// Contiene la lógica de negocio para operaciones de autenticación.
    /// </summary>
    public class LoginController
    {
        /// <summary>
        /// Valida las credenciales del usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario ingresado.</param>
        /// <param name="password">La contraseña ingresada.</param>
        /// <returns>True si el login es válido, false si no lo es.</returns>
        public bool ValidarLogin(string username, string password)
        {
            //
            // ¡AQUÍ ESTÁ LA LÓGICA QUE MOVIMOS!
            //
            // TODO: Reemplazar esta simulación con la lógica real de validación contra la BD.
            bool loginValido = (username == "admin" && password == "123");

            // El controlador solo devuelve el resultado.
            return loginValido;
        }
    }
}
``n
## Archivo: .\FoodWare\Model\DataAccess\ProductoMockRepository.cs

`csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;

namespace FoodWare.Model.DataAccess
{
    // Enlace con IProductoRepository
    public class ProductoMockRepository : IProductoRepository
    {
        // Nuestra "base de datos" falsa
        private static List<Producto> _productos = new()
        {
            new() { IdProducto = 1, Nombre = "Tomate", Categoria = "Verduras", StockActual = 50, PrecioCosto = 20.5m },
            new() { IdProducto = 2, Nombre = "Pechuga de Pollo", Categoria = "Carnes", StockActual = 30, PrecioCosto = 80m },
            new() { IdProducto = 3, Nombre = "Pan de Hamburguesa", Categoria = "Panadería", StockActual = 100, PrecioCosto = 5m }
        };
        private static int _nextId = 4; // Para simular el auto-incremento

        public List<Producto> ObtenerTodos()
        {
            // Devuelve una copia para simular que vienen de la BD
            return _productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            // Si no se encuentra, lanza una excepción para cumplir con la interfaz
            var producto = _productos.FirstOrDefault(p => p.IdProducto == id);
            return producto ?? throw new InvalidOperationException($"No se encontró el producto con Id {id}.");
        }

        public void Agregar(Producto producto)
        {
            producto.IdProducto = _nextId++;
            _productos.Add(producto);
        }

        public void Actualizar(Producto producto)
        {
            var existente = _productos.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
            if (existente != null)
            {
                existente.Nombre = producto.Nombre;
                existente.Categoria = producto.Categoria;
                existente.StockActual = producto.StockActual;
                existente.PrecioCosto = producto.PrecioCosto;
            }
        }

        public void Eliminar(int id)
        {
            var producto = _productos.FirstOrDefault(p => p.IdProducto == id);
            if (producto != null)
            {
                _productos.Remove(producto);
            }
        }
    }
}
``n
## Archivo: .\FoodWare\Model\Entities\Producto.cs

`csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public required string Nombre { get; set; }
        public required string Categoria { get; set; }
        public int StockActual { get; set; }
        public decimal PrecioCosto { get; set; }
    }
}
``n
## Archivo: .\FoodWare\Model\Entities\Usuario.cs

`csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Entities
{
    /// <summary>
    /// Representa un usuario del sistema (Empleado o Administrador).
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; } // En el futuro será un hash
        public string Rol { get; set; } // Ej. "Administrador", "Mesero"
    }
}

``n
## Archivo: .\FoodWare\Model\Interfaces\IProductoRepository.cs

`csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;

namespace FoodWare.Model.Interfaces
{
    /// <summary>
    /// Define las operaciones CRUD para la entidad Producto, 
    /// proporcionando los métodos necesarios para gestionar productos en el sistema.
    /// </summary>
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();        // R - Read (Leer todos)
        Producto ObtenerPorId(int id);      // R - Read (Leer uno)
        void Agregar(Producto producto);      // C - Create (Crear)
        void Actualizar(Producto producto);   // U - Update (Actualizar)
        void Eliminar(int id);                // D - Delete (Eliminar)
    }
}

``n
## Archivo: .\FoodWare\obj\Debug\net9.0-windows\.NETCoreApp,Version=v9.0.AssemblyAttributes.cs

`csharp
// <autogenerated />
using System;
using System.Reflection;
[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]

``n
## Archivo: .\FoodWare\obj\Debug\net9.0-windows\FoodWare.AssemblyInfo.cs

`csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Este cÃ³digo fue generado por una herramienta.
//     VersiÃ³n de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrÃ­an causar un comportamiento incorrecto y se perderÃ¡n si
//     se vuelve a generar el cÃ³digo.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("FoodWare")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+65d8828096e6e5f4360a71fdbde7b9f81224ba68")]
[assembly: System.Reflection.AssemblyProductAttribute("FoodWare")]
[assembly: System.Reflection.AssemblyTitleAttribute("FoodWare")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Runtime.Versioning.TargetPlatformAttribute("Windows7.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("Windows7.0")]

// Generado por la clase WriteCodeFragment de MSBuild.


``n
## Archivo: .\FoodWare\obj\Debug\net9.0-windows\FoodWare.GlobalUsings.g.cs

`csharp
// <auto-generated/>
global using global::System;
global using global::System.Collections.Generic;
global using global::System.Drawing;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Net.Http;
global using global::System.Threading;
global using global::System.Threading.Tasks;
global using global::System.Windows.Forms;

``n
## Archivo: .\FoodWare\Properties\Resources.Designer.cs

`csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodWare.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FoodWare.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca un recurso adaptado de tipo System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap InicioBienvenida {
            get {
                object obj = ResourceManager.GetObject("InicioBienvenida", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}

``n
## Archivo: .\FoodWare\View\Forms\FormMain.cs

`csharp
using System;
using System.Windows.Forms;
using FoodWare.View.UserControls;
using FoodWare.View.Helpers;

namespace FoodWare.View.Forms
{
    /// <summary>
    /// Formulario principal y contenedor (Shell) de la aplicación FoodWare.
    /// Gestiona la navegación del menú lateral y la carga dinámica de módulos (UserControls) 
    /// en el panel de contenido principal.
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Inicializa el formulario principal.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            PersonalizarDiseno(); // Configura el estado visual inicial de los controles.
            AplicarEstilos();     // Aplica la paleta de colores centralizada desde EstilosApp.
            CargarInicio();       // Carga el UserControl por defecto (Dashboard/Inicio).
        }

        /// <summary>
        /// Establece el estado visual inicial de la UI al arrancar la aplicación.
        /// (Oculta submenús, define placeholders, etc.)
        /// </summary>
        private void PersonalizarDiseno()
        {
            // Ocultar submenús al inicio
            panelGPSubmenu.Visible = false;
            panelAdminSubmenu.Visible = false;
            panelAnalisisSubmenu.Visible = false;

            // Placeholder de búsqueda
            try { txtBusqueda.PlaceholderText = "Buscar en FoodWare"; } catch { }
        }

        /// <summary>
        /// Aplica los estilos visuales centralizados desde la clase EstilosApp
        /// a los componentes de este formulario (paneles y botones).
        /// </summary>
        private void AplicarEstilos()
        {
            // Paneles
            EstilosApp.EstiloPanel(panelMenu, EstilosApp.ColorMenu);
            EstilosApp.EstiloPanel(panelBarra, EstilosApp.ColorBarra);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);

            // Botones principales (Nivel 1)
            EstilosApp.EstiloBotonMenu(btnGP);
            EstilosApp.EstiloBotonMenu(btnAdmin);
            EstilosApp.EstiloBotonMenu(btnAnalisis);
            EstilosApp.EstiloBotonMenu(btnConfig);

            // Botones secundarios (Nivel 2 - Submenús)
            EstilosApp.EstiloBotonSubmenu(btnInicio);
            EstilosApp.EstiloBotonSubmenu(btnInventario);
            EstilosApp.EstiloBotonSubmenu(btnMenu);
            EstilosApp.EstiloBotonSubmenu(btnVentas);
            EstilosApp.EstiloBotonSubmenu(btnEmpleados);
            EstilosApp.EstiloBotonSubmenu(btnFinanzas);
            EstilosApp.EstiloBotonSubmenu(btnReportes);
        }

        /// <summary>
        /// Carga el módulo (UserControl) de Inicio por defecto en el panel de contenido.
        /// </summary>
        private void CargarInicio()
        {
            AbrirModulo(new UC_Inicio());
        }

        // --- LÓGICA DE NAVEGACIÓN DEL MENÚ ---

        /// <summary>
        /// Método de ayuda para cerrar todos los submenús abiertos.
        /// </summary>
        private void OcultarSubmenu()
        {
            if (panelGPSubmenu.Visible) panelGPSubmenu.Visible = false;
            if (panelAdminSubmenu.Visible) panelAdminSubmenu.Visible = false;
            if (panelAnalisisSubmenu.Visible) panelAnalisisSubmenu.Visible = false;
        }

        /// <summary>
        /// Gestiona la lógica de acordeón para mostrar/ocultar un submenú específico.
        /// </summary>
        /// <param name="submenu">El Panel (que actúa como submenú) que se debe mostrar u ocultar.</param>
        private void MostrarSubmenu(Panel submenu)
        {
            if (!submenu.Visible)
            {
                // Si está cerrado: cerramos cualquier otro abierto y abrimos este.
                OcultarSubmenu();
                submenu.Visible = true;
            }
            else
            {
                // Si ya estaba abierto: lo cerramos.
                submenu.Visible = false;
            }
        }

        /// <summary>
        /// Método principal para cargar dinámicamente un módulo (UserControl) en el área de contenido.
        /// </summary>
        /// <param name="modulo">La instancia del UserControl que se va a cargar.</param>
        private void AbrirModulo(UserControl modulo)
        {
            panelContenido.Controls.Clear(); // Limpia el módulo anterior
            modulo.Dock = DockStyle.Fill; // Asegura que el control llene el panel
            panelContenido.Controls.Add(modulo); // Añade el nuevo control
            modulo.BringToFront(); // Lo trae al frente
        }


        // --- EVENT HANDLERS (EVENTOS DE CLICK) ---

        // Botones padres (Categorías)
        private void btnGP_Click(object sender, EventArgs e) => MostrarSubmenu(panelGPSubmenu);
        private void btnAdmin_Click(object sender, EventArgs e) => MostrarSubmenu(panelAdminSubmenu);
        private void btnAnalisis_Click(object sender, EventArgs e) => MostrarSubmenu(panelAnalisisSubmenu);

        // Botones hijos (Módulos)
        private void btnInicio_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inicio()); OcultarSubmenu(); }
        private void btnInventario_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inventario()); OcultarSubmenu(); }
        private void btnMenu_Click(object sender, EventArgs e) { AbrirModulo(new UC_Menu()); OcultarSubmenu(); }
        private void btnVentas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Ventas()); OcultarSubmenu(); }
        private void btnEmpleados_Click(object sender, EventArgs e) { AbrirModulo(new UC_Empleados()); OcultarSubmenu(); }
        private void btnFinanzas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Finanzas()); OcultarSubmenu(); }
        private void btnReportes_Click(object sender, EventArgs e) { AbrirModulo(new UC_Reportes()); OcultarSubmenu(); }
        private void btnConfig_Click(object sender, EventArgs e) { AbrirModulo(new UC_Configuracion()); OcultarSubmenu(); }


        // --- EVENTOS DE BÚSQUEDA ---

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta la tecla "Enter" en el cuadro de búsqueda
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido "ding" de Windows

                // TODO: Implementar la lógica de búsqueda global aquí.
                MessageBox.Show("Buscando: " + txtBusqueda.Text); // Acción placeholder
            }
        }
    }
}
``n
## Archivo: .\FoodWare\View\Forms\FormMain.Designer.cs

`csharp
namespace FoodWare.View.Forms
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelBarra;
        private System.Windows.Forms.Panel panelContenido;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.Panel panelBusqueda; // contenedor para el buscador

        private System.Windows.Forms.Button btnGP;
        private System.Windows.Forms.Panel panelGPSubmenu;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnVentas;

        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Panel panelAdminSubmenu;
        private System.Windows.Forms.Button btnEmpleados;
        private System.Windows.Forms.Button btnFinanzas;

        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Panel panelAnalisisSubmenu;
        private System.Windows.Forms.Button btnReportes;

        private System.Windows.Forms.Button btnConfig;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnConfig = new Button();
            panelAnalisisSubmenu = new Panel();
            btnReportes = new Button();
            btnAnalisis = new Button();
            panelAdminSubmenu = new Panel();
            btnFinanzas = new Button();
            btnEmpleados = new Button();
            btnAdmin = new Button();
            panelGPSubmenu = new Panel();
            btnVentas = new Button();
            btnMenu = new Button();
            btnInventario = new Button();
            btnInicio = new Button();
            btnGP = new Button();
            panelBarra = new Panel();
            lblTitulo = new Label();
            panelBusqueda = new Panel();
            txtBusqueda = new TextBox();
            panelContenido = new Panel();
            panelMenu.SuspendLayout();
            panelAnalisisSubmenu.SuspendLayout();
            panelAdminSubmenu.SuspendLayout();
            panelGPSubmenu.SuspendLayout();
            panelBarra.SuspendLayout();
            panelBusqueda.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.AutoScroll = true;
            panelMenu.Controls.Add(btnConfig);
            panelMenu.Controls.Add(panelAnalisisSubmenu);
            panelMenu.Controls.Add(btnAnalisis);
            panelMenu.Controls.Add(panelAdminSubmenu);
            panelMenu.Controls.Add(btnAdmin);
            panelMenu.Controls.Add(panelGPSubmenu);
            panelMenu.Controls.Add(btnGP);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 56);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(240, 664);
            panelMenu.TabIndex = 2;
            // 
            // btnConfig
            // 
            btnConfig.Dock = DockStyle.Bottom;
            btnConfig.Location = new Point(0, 641);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(240, 23);
            btnConfig.TabIndex = 0;
            btnConfig.Text = "Configuracion";
            btnConfig.Click += btnConfig_Click;
            // 
            // panelAnalisisSubmenu
            // 
            panelAnalisisSubmenu.Controls.Add(btnReportes);
            panelAnalisisSubmenu.Dock = DockStyle.Top;
            panelAnalisisSubmenu.Location = new Point(0, 269);
            panelAnalisisSubmenu.Name = "panelAnalisisSubmenu";
            panelAnalisisSubmenu.Size = new Size(240, 100);
            panelAnalisisSubmenu.TabIndex = 1;
            // 
            // btnReportes
            // 
            btnReportes.Dock = DockStyle.Top;
            btnReportes.Location = new Point(0, 0);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(240, 23);
            btnReportes.TabIndex = 0;
            btnReportes.Text = "Reportes";
            btnReportes.Click += btnReportes_Click;
            // 
            // btnAnalisis
            // 
            btnAnalisis.Dock = DockStyle.Top;
            btnAnalisis.Location = new Point(0, 246);
            btnAnalisis.Name = "btnAnalisis";
            btnAnalisis.Size = new Size(240, 23);
            btnAnalisis.TabIndex = 2;
            btnAnalisis.Text = "Analisis";
            btnAnalisis.Click += btnAnalisis_Click;
            // 
            // panelAdminSubmenu
            // 
            panelAdminSubmenu.Controls.Add(btnFinanzas);
            panelAdminSubmenu.Controls.Add(btnEmpleados);
            panelAdminSubmenu.Dock = DockStyle.Top;
            panelAdminSubmenu.Location = new Point(0, 146);
            panelAdminSubmenu.Name = "panelAdminSubmenu";
            panelAdminSubmenu.Size = new Size(240, 100);
            panelAdminSubmenu.TabIndex = 3;
            // 
            // btnFinanzas
            // 
            btnFinanzas.Dock = DockStyle.Top;
            btnFinanzas.Location = new Point(0, 23);
            btnFinanzas.Name = "btnFinanzas";
            btnFinanzas.Size = new Size(240, 23);
            btnFinanzas.TabIndex = 0;
            btnFinanzas.Text = "Finanzas";
            btnFinanzas.Click += btnFinanzas_Click;
            // 
            // btnEmpleados
            // 
            btnEmpleados.Dock = DockStyle.Top;
            btnEmpleados.Location = new Point(0, 0);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(240, 23);
            btnEmpleados.TabIndex = 1;
            btnEmpleados.Text = "Empleados";
            btnEmpleados.Click += btnEmpleados_Click;
            // 
            // btnAdmin
            // 
            btnAdmin.Dock = DockStyle.Top;
            btnAdmin.Location = new Point(0, 123);
            btnAdmin.Name = "btnAdmin";
            btnAdmin.Size = new Size(240, 23);
            btnAdmin.TabIndex = 4;
            btnAdmin.Text = "Administracion";
            btnAdmin.Click += btnAdmin_Click;
            // 
            // panelGPSubmenu
            // 
            panelGPSubmenu.Controls.Add(btnVentas);
            panelGPSubmenu.Controls.Add(btnMenu);
            panelGPSubmenu.Controls.Add(btnInventario);
            panelGPSubmenu.Controls.Add(btnInicio);
            panelGPSubmenu.Dock = DockStyle.Top;
            panelGPSubmenu.Location = new Point(0, 23);
            panelGPSubmenu.Name = "panelGPSubmenu";
            panelGPSubmenu.Size = new Size(240, 100);
            panelGPSubmenu.TabIndex = 5;
            // 
            // btnVentas
            // 
            btnVentas.Dock = DockStyle.Top;
            btnVentas.Location = new Point(0, 69);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(240, 23);
            btnVentas.TabIndex = 0;
            btnVentas.Text = "Ventas";
            btnVentas.Click += btnVentas_Click;
            // 
            // btnMenu
            // 
            btnMenu.Dock = DockStyle.Top;
            btnMenu.Location = new Point(0, 46);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(240, 23);
            btnMenu.TabIndex = 1;
            btnMenu.Text = "Menu";
            btnMenu.Click += btnMenu_Click;
            // 
            // btnInventario
            // 
            btnInventario.Dock = DockStyle.Top;
            btnInventario.Location = new Point(0, 23);
            btnInventario.Name = "btnInventario";
            btnInventario.Size = new Size(240, 23);
            btnInventario.TabIndex = 2;
            btnInventario.Text = "Inventario";
            btnInventario.Click += btnInventario_Click;
            // 
            // btnInicio
            // 
            btnInicio.Dock = DockStyle.Top;
            btnInicio.Location = new Point(0, 0);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(240, 23);
            btnInicio.TabIndex = 3;
            btnInicio.Text = "Inicio";
            btnInicio.Click += btnInicio_Click;
            // 
            // btnGP
            // 
            btnGP.Dock = DockStyle.Top;
            btnGP.Location = new Point(0, 0);
            btnGP.Name = "btnGP";
            btnGP.Size = new Size(240, 23);
            btnGP.TabIndex = 6;
            btnGP.Text = "Gestion Principal";
            btnGP.Click += btnGP_Click;
            // 
            // panelBarra
            // 
            panelBarra.Controls.Add(lblTitulo);
            panelBarra.Controls.Add(panelBusqueda);
            panelBarra.Dock = DockStyle.Top;
            panelBarra.Location = new Point(0, 0);
            panelBarra.Name = "panelBarra";
            panelBarra.Size = new Size(1200, 56);
            panelBarra.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Bauhaus 93", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(150, 56);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "FoodWare";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelBusqueda
            // 
            panelBusqueda.Controls.Add(txtBusqueda);
            panelBusqueda.Dock = DockStyle.Right;
            panelBusqueda.Location = new Point(940, 0);
            panelBusqueda.Name = "panelBusqueda";
            panelBusqueda.Padding = new Padding(10, 14, 10, 14);
            panelBusqueda.Size = new Size(260, 56);
            panelBusqueda.TabIndex = 0;
            // 
            // txtBusqueda
            // 
            txtBusqueda.Dock = DockStyle.Fill;
            txtBusqueda.Location = new Point(10, 14);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.Size = new Size(240, 27);
            txtBusqueda.TabIndex = 101;
            txtBusqueda.KeyDown += txtBusqueda_KeyDown;
            // 
            // panelContenido
            // 
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(240, 56);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(960, 664);
            panelContenido.TabIndex = 0;
            // 
            // FormMain
            // 
            ClientSize = new Size(1200, 720);
            Controls.Add(panelContenido);
            Controls.Add(panelMenu);
            Controls.Add(panelBarra);
            MinimumSize = new Size(900, 600);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FoodWare";
            panelMenu.ResumeLayout(false);
            panelAnalisisSubmenu.ResumeLayout(false);
            panelAdminSubmenu.ResumeLayout(false);
            panelGPSubmenu.ResumeLayout(false);
            panelBarra.ResumeLayout(false);
            panelBusqueda.ResumeLayout(false);
            panelBusqueda.PerformLayout();
            ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\Forms\LoginForm.cs

`csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoodWare.View.Helpers;
using FoodWare.Controller.Logic;

namespace FoodWare.View.Forms
{
    /// <summary>
    /// Formulario de Inicio de Sesión. Valida al usuario y, si tiene éxito,
    /// devuelve DialogResult.OK para que Program.cs inicie el FormMain.
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Inicializa el formulario de Login.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            AplicarEstilosDeLogin();
        }

        /// <summary>
        /// Método de ayuda para aplicar los estilos de EstilosApp a este formulario.
        /// </summary>
        private void AplicarEstilosDeLogin()
        {
            // 1. Estilo de la Ventana
            this.BackColor = EstilosApp.ColorMenu;

            // 2. Campos de Entrada (TextBox)
            EstilosApp.EstiloTextBoxLogin(this.txtUsuario);
            EstilosApp.EstiloTextBoxLogin(this.txtPassword);

            // 3. Botón Principal (INGRESAR)
            EstilosApp.EstiloBotonAccionPrincipal(this.btnIngresar);

            // 4. Label de Mensajes de Error
            EstilosApp.EstiloLabelError(this.lblMensajeError);
        }

        /// <summary>
        /// Método de ayuda para mostrar mensajes de error usando la etiqueta estilizada.
        /// </summary>
        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = "     * " + mensaje; // Muestra el mensaje de error (con prefijo)
            lblMensajeError.Visible = true;
        }

        // --- LÓGICA DE VALIDACIÓN Y EVENTOS ---

        /// <summary>
        /// Evento principal del botón Ingresar. Valida las credenciales.
        /// </summary>
        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            // --- Modificacion de MVC ---

            // 1. La Vista crea una instancia del Controlador
            LoginController loginCtrl = new LoginController();

            // 2. La Vista recoge los datos y se los pasa al Controlador
            bool loginValido = loginCtrl.ValidarLogin(this.txtUsuario.Text, this.txtPassword.Text);

            // 3. La Vista reacciona a la respuesta del Controlador
            if (loginValido)
            {
                // ÉXITO: Informa al Program.cs que el resultado es OK y cierra.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // FRACASO:
                txtPassword.Text = "";
                txtPassword.Focus();
                MostrarError("Usuario o contraseña incorrectos.");
            }
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario 
        /// intenta corregir el nombre de usuario.
        /// </summary>
        private void TxtUsuario_TextChanged(object sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario
        /// intenta corregir la contraseña.
        /// </summary>
        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }
    }
}
``n
## Archivo: .\FoodWare\View\Forms\LoginForm.Designer.cs

`csharp
namespace FoodWare.View.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            txtUsuario = new TextBox();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            lblMensajeError = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(341, 61);
            label2.Name = "label2";
            label2.Size = new Size(116, 28);
            label2.TabIndex = 1;
            label2.Text = "Bienvenid@";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(315, 119);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Ingrese su usuario";
            txtUsuario.Size = new Size(165, 27);
            txtUsuario.TabIndex = 2;
            txtUsuario.TextChanged += TxtUsuario_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(315, 173);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Ingrese su contraseña";
            txtPassword.Size = new Size(165, 27);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += TxtPassword_TextChanged;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(351, 243);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(89, 39);
            btnIngresar.TabIndex = 4;
            btnIngresar.Text = "INGRESAR";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += BtnIngresar_Click;
            // 
            // lblMensajeError
            // 
            lblMensajeError.Location = new Point(173, 335);
            lblMensajeError.Name = "lblMensajeError";
            lblMensajeError.Size = new Size(439, 20);
            lblMensajeError.TabIndex = 5;
            lblMensajeError.Text = "labelError";
            lblMensajeError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Bauhaus 93", 13.8F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(341, 9);
            label1.Name = "label1";
            label1.Size = new Size(115, 26);
            label1.TabIndex = 0;
            label1.Text = "FoodWare";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMensajeError);
            Controls.Add(btnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(txtUsuario);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "LoginForm";
            Text = "Login FoodWare";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Label lblMensajeError;
        private Label label1;
    }
}
``n
## Archivo: .\FoodWare\View\Helpers\EstilosApp.cs

`csharp
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.Helpers
{
    /// <summary>
    /// Clase estática de utilidad para centralizar la paleta de colores y los estilos 
    /// de los controles de Windows Forms para toda la aplicación FoodWare.
    /// </summary>
    public static class EstilosApp
    {
        // --- PALETA DE COLORES PRINCIPAL ---
        public static Color ColorMenu = ColorTranslator.FromHtml("#2C3E50");
        public static Color ColorBarra = ColorTranslator.FromHtml("#34495E");
        public static Color ColorFondo = ColorTranslator.FromHtml("#ECF0F1");
        public static Color ColorTextoOscuro = ColorTranslator.FromHtml("#2C3E50");
        public static Color ColorActivo = ColorTranslator.FromHtml("#1ABC9C");
        public static Color ColorAccion = ColorTranslator.FromHtml("#27AE60");
        public static Color ColorAlerta = ColorTranslator.FromHtml("#E74C3C");
        public static Color ColorSubmenuBG = Color.FromArgb(0, 100, 100);

        // --- MÉTODOS DE ESTILO (MENÚ PRINCIPAL) ---

        /// <summary>
        /// Aplica un color de fondo estándar a un control Panel.
        /// </summary>
        public static void EstiloPanel(Panel panel, Color color)
        {
            panel.BackColor = color;
        }

        /// <summary>
        /// Aplica un estilo visual estandarizado a un botón de Menú Principal (Nivel 1).
        /// </summary>

        public static void EstiloBotonMenu(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // Requerido para FlatStyle para quitar el borde
            btn.ForeColor = Color.White;
            btn.BackColor = ColorMenu;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.Height = 40;
            btn.Dock = DockStyle.Top;
        }

        /// <summary>
        /// Aplica un estilo visual estandarizado a un botón de Submenú (Nivel 2).
        /// </summary>

        public static void EstiloBotonSubmenu(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = ColorSubmenuBG;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0); // Mayor indentación para Nivel 2
            btn.Height = 35;
            btn.Dock = DockStyle.Top;
        }

        // --- NUEVOS MÉTODOS DE ESTILO (LOGIN FORM) ---

        /// <summary>
        /// Aplica el estilo de acción principal (color Activo) a un botón, 
        /// usado para el botón "INGRESAR" del Login. (Spec: #1ABC9C)
        /// </summary>
        public static void EstiloBotonAccionPrincipal(Button btn)
        {
            btn.BackColor = ColorActivo;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }

        /// <summary>
        /// Aplica el estilo estándar para campos de texto de Login (Usuario/Contraseña).
        /// </summary>
        public static void EstiloTextBoxLogin(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTextoOscuro; // Spec: #2C3E50
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Aplica el estilo estándar para las etiquetas de mensaje de error.
        /// Las oculta por defecto y asigna el color de Alerta.
        /// </summary>
        public static void EstiloLabelError(Label lbl)
        {
            lbl.ForeColor = ColorAlerta; // Spec: #E74C3C
            lbl.BackColor = Color.Transparent;
            lbl.Visible = false; // Oculto por defecto
        }

        // --- MÉTODOS DE ESTILO (CONTROLES DE MÓDULO) ---

        /// <summary>
        /// Aplica un estilo visual estándar a un DataGridView.
        /// </summary>
        public static void EstiloDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = ColorFondo;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;

            // Estilo del Header
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorBarra;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 30;

            // Estilo de las Filas
            dgv.DefaultCellStyle.BackColor = ColorFondo;
            dgv.DefaultCellStyle.ForeColor = ColorTextoOscuro;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.SelectionBackColor = ColorActivo;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 25;

            // Comportamiento
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Positiva" (Guardar, Crear).
        /// </summary>
        public static void EstiloBotonModulo(Button btn)
        {
            btn.BackColor = ColorAccion; // Verde
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Peligro" (Eliminar).
        /// </summary>
        public static void EstiloBotonModuloAlerta(Button btn)
        {
            btn.BackColor = ColorAlerta; // Rojo
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Secundaria" (Limpiar, Cancelar).
        /// </summary>
        public static void EstiloBotonModuloSecundario(Button btn)
        {
            btn.BackColor = Color.White;
            btn.ForeColor = ColorTextoOscuro; // Color gris/azul
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorTextoOscuro;
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a una etiqueta de formulario.
        /// </summary>
        public static void EstiloLabelModulo(Label lbl)
        {
            lbl.ForeColor = ColorTextoOscuro;
            lbl.BackColor = Color.Transparent;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a una caja de texto de formulario.
        /// </summary>
        public static void EstiloTextBoxModulo(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTextoOscuro;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 9);
        }
    }
}
``n
## Archivo: .\FoodWare\View\UserControls\UC_Configuracion.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Configuracion : UserControl
    {
        public UC_Configuracion()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Configuracion",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Configuracion.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Configuracion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Reportes
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Configuracion";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Empleados.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Empleados : UserControl
    {
        public UC_Empleados()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Empleados",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Empleados.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Empleados
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Empleados
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Empleados";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Finanzas.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Finanzas : UserControl
    {
        public UC_Finanzas()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Finanzas",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Finanzas.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Finanzas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Finanzas
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Finanzas";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Inicio.cs

`csharp
using System;
using System.Drawing; // Aunque ya no se usa aquí, es estándar dejarlo por si acaso.
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    /// <summary>
    /// Módulo (UserControl) que actúa como la pantalla principal o Dashboard.
    /// Actualmente muestra una imagen de bienvenida (pictureBoxBienvenida) definida en el Diseñador.
    /// </summary>
    public partial class UC_Inicio : UserControl
    {
        /// <summary>
        /// Inicializa el módulo de Inicio.
        /// </summary>
        public UC_Inicio()
        {
            // Este método carga TODOS los componentes definidos en el archivo .Designer.cs
            // (En este caso, carga el 'pictureBoxBienvenida').
            InitializeComponent();
        }

        // TODO: (Guía de Proyecto): Este UserControl (con la imagen de bienvenida)
        // sigue siendo un marcador de posición. En la fase 2 del proyecto,
        // este control debe ser reemplazado por el Dashboard real de KPIs
        // (Ventas del día, Mesas abiertas, Alertas de inventario, etc.).
    }
}
``n
## Archivo: .\FoodWare\View\UserControls\UC_Inicio.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Inicio
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBoxBienvenida = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBienvenida).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxBienvenida
            // 
            pictureBoxBienvenida.Dock = DockStyle.Fill;
            pictureBoxBienvenida.Image = Properties.Resources.InicioBienvenida;
            pictureBoxBienvenida.Location = new Point(0, 0);
            pictureBoxBienvenida.Name = "pictureBoxBienvenida";
            pictureBoxBienvenida.Size = new Size(800, 450);
            pictureBoxBienvenida.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBienvenida.TabIndex = 0;
            pictureBoxBienvenida.TabStop = false;
            // 
            // UC_Inicio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBoxBienvenida);
            Name = "UC_Inicio";
            Size = new Size(800, 450);
            ((System.ComponentModel.ISupportInitialize)pictureBoxBienvenida).EndInit();
            ResumeLayout(false);
        }
        private PictureBox pictureBoxBienvenida;
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Inventario.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.View.Helpers;
using FoodWare.Controller.Logic;
using FoodWare.Model.Entities;


namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private InventarioController _controller;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos(); // ¡Llamamos a nuestro método de estilos!
            _controller = new InventarioController();
        }

        /// <summary>
        /// Aplica los estilos de EstilosApp a este UserControl.
        /// </summary>
        private void AplicarEstilos()
        {
            // Fondo del UserControl y Panel
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelInputs, EstilosApp.ColorFondo);

            // Etiquetas
            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblCategoria);
            EstilosApp.EstiloLabelModulo(lblStock);
            EstilosApp.EstiloLabelModulo(lblPrecio);

            // Cajas de Texto
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtCategoria);
            EstilosApp.EstiloTextBoxModulo(txtStock);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvInventario);
        }

        private void UC_Inventario_Load(object sender, EventArgs e)
        {
            if (!DesignMode) // Esto evita que el código se ejecute en el diseñador
            {
                CargarGrid(); // Le decimos que llene la tabla
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista RECOGE datos
                string nombre = txtNombre.Text;
                string cat = txtCategoria.Text;
                if (!int.TryParse(txtStock.Text, out int stock))
                {
                    MessageBox.Show("Stock debe ser un número entero válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("Precio debe ser un número válido (puede llevar decimales).", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // 2. La Vista ENVÍA datos al controlador
                _controller.GuardarNuevoProducto(nombre, cat, stock, precio);

                // 3. La Vista se ACTUALIZA
                MessageBox.Show("¡Producto guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrid(); // Refresca la tabla
                LimpiarCampos(); // Limpia las cajas de texto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica si hay una fila seleccionada en el grid
            if (this.dgvInventario.CurrentRow != null && this.dgvInventario.CurrentRow.DataBoundItem is Producto producto)
            {
                int id = producto.IdProducto;

                // 2. Pide confirmación
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{producto.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    // 3. Envía la orden al controlador
                    _controller.EliminarProducto(id);

                    // 4. Actualiza la vista
                    CargarGrid();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un producto para eliminar.");
            }
        }

        // --- MÉTODOS DE AYUDA DE LA VISTA ---

        /// <summary>
        /// Pide los productos al controlador y actualiza el DataGridView.
        /// </summary>
        private void CargarGrid()
        {
            var productos = _controller.CargarProductos();
            this.dgvInventario.DataSource = null;
            this.dgvInventario.DataSource = productos;

            // Opcional: ajustar columnas
            var columnas = this.dgvInventario.Columns;
            if (columnas != null && columnas["IdProducto"] is not null)
            {
                columnas["IdProducto"]!.HeaderText = "ID";
                columnas["IdProducto"]!.Width = 50;
            }
            // Verifica que la columna "StockActual" existe antes de acceder a ella
            if (columnas != null && columnas["StockActual"] is not null)
            {
                columnas["StockActual"]!.HeaderText = "Stock Actual";
            }

            if (columnas != null && columnas["PrecioCosto"] is not null)
            {
                columnas["PrecioCosto"]!.HeaderText = "Precio Costo";
                columnas["PrecioCosto"]!.Width = 150;
            }
        }

        /// <summary>
        /// Limpia las cajas de texto del formulario.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCategoria.Text = "";
            txtStock.Text = "";
            txtPrecio.Text = "";
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
``n
## Archivo: .\FoodWare\View\UserControls\UC_Inventario.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Inventario
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelInputs = new Panel();
            btnGuardar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            lblStock = new Label();
            lblCategoria = new Label();
            lblNombre = new Label();
            txtStock = new TextBox();
            txtCategoria = new TextBox();
            txtNombre = new TextBox();
            dgvInventario = new DataGridView();
            panelInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            SuspendLayout();
            // 
            // panelInputs
            // 
            panelInputs.Controls.Add(btnGuardar);
            panelInputs.Controls.Add(btnEliminar);
            panelInputs.Controls.Add(btnLimpiar);
            panelInputs.Controls.Add(txtPrecio);
            panelInputs.Controls.Add(lblPrecio);
            panelInputs.Controls.Add(lblStock);
            panelInputs.Controls.Add(lblCategoria);
            panelInputs.Controls.Add(lblNombre);
            panelInputs.Controls.Add(txtStock);
            panelInputs.Controls.Add(txtCategoria);
            panelInputs.Controls.Add(txtNombre);
            panelInputs.Dock = DockStyle.Top;
            panelInputs.Location = new Point(0, 0);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(960, 179);
            panelInputs.TabIndex = 0;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.Location = new Point(802, 16);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(104, 49);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEliminar.Location = new Point(802, 67);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(104, 49);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += BtnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiar.Location = new Point(802, 119);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(104, 49);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += BtnLimpiar_Click;
            // 
            // txtPrecio
            // 
            txtPrecio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPrecio.Location = new Point(164, 137);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(590, 27);
            txtPrecio.TabIndex = 7;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(44, 137);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(50, 20);
            lblPrecio.TabIndex = 6;
            lblPrecio.Text = "Precio";
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(44, 96);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(45, 20);
            lblStock.TabIndex = 5;
            lblStock.Text = "Stock";
            lblStock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(44, 56);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(74, 20);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoria";
            lblCategoria.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.ImageAlign = ContentAlignment.BottomCenter;
            lblNombre.Location = new Point(44, 16);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStock
            // 
            txtStock.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtStock.Location = new Point(164, 96);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(590, 27);
            txtStock.TabIndex = 2;
            // 
            // txtCategoria
            // 
            txtCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCategoria.Location = new Point(164, 56);
            txtCategoria.Name = "txtCategoria";
            txtCategoria.Size = new Size(590, 27);
            txtCategoria.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.Location = new Point(164, 17);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(590, 27);
            txtNombre.TabIndex = 0;
            // 
            // dgvInventario
            // 
            dgvInventario.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventario.Location = new Point(0, 179);
            dgvInventario.Name = "dgvInventario";
            dgvInventario.RowHeadersWidth = 51;
            dgvInventario.Size = new Size(960, 485);
            dgvInventario.TabIndex = 1;
            // 
            // UC_Inventario
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(panelInputs);
            Controls.Add(dgvInventario);
            Name = "UC_Inventario";
            Size = new Size(960, 664);
            Load += UC_Inventario_Load;
            panelInputs.ResumeLayout(false);
            panelInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ResumeLayout(false);
        }
        private Panel panelInputs;
        private TextBox txtStock;
        private TextBox txtCategoria;
        private TextBox txtNombre;
        private Label lblStock;
        private Label lblCategoria;
        private Label lblNombre;
        private TextBox txtPrecio;
        private Label lblPrecio;
        private Button btnGuardar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvInventario;
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Reportes.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Reportes : UserControl
    {
        public UC_Reportes()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Reportes",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Reportes.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Reportes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Reportes
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Reportes";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Ventas.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Ventas : UserControl
    {
        public UC_Ventas()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Ventas",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\UC_Ventas.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Ventas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Ventas
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Ventas";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\US_Menu.cs

`csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Menu : UserControl
    {
        public UC_Menu()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Menú",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}

``n
## Archivo: .\FoodWare\View\UserControls\US_Menu.Designer.cs

`csharp
namespace FoodWare.View.UserControls
{
    partial class UC_Menu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UC_Configuracion
            // 
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "UC_Menu";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
        }
    }
}

``n

