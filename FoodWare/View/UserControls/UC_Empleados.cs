using System;
using System.Data;
using System.Windows.Forms;
using FoodWare.Controller; // Necesario para acceder al MainController

namespace FoodWare.View.UserControls
{
    // User Control encargado de la visualización de la información de Empleados.
    public partial class UC_Empleados : UserControl
    {
        // El controlador es la única dependencia que la vista debe tener
        private readonly MainController _controller;

        // Nota: Asumo que 'dataGridView1' fue generado por el diseñador y existe.

        public UC_Empleados()
        {
            InitializeComponent();
            _controller = new MainController(); // Inicializa la capa de Control

            // Cargar los datos al iniciar el User Control
            CargarEmpleados();
        }

        /// <summary>
        /// Llama al MainController para obtener los datos de la BD y los asigna al DataGridView.
        /// </summary>
        private void CargarEmpleados()
        {
            try
            {
                // ** Lógica real: Llama al controlador para obtener el DataTable **
                DataTable dtEmpleados = _controller.ObtenerEmpleadosParaVista();

                if (dtEmpleados != null && dtEmpleados.Rows.Count > 0)
                {
                    // Asigna el DataTable al DataGridView
                    dataGridView1.DataSource = dtEmpleados;

                    // Opcional: Autoajustar las columnas para que se vean mejor
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                else
                {
                    // Muestra un mensaje si no se encontraron registros (la conexión pudo ser exitosa)
                    MessageBox.Show("No se encontraron registros de EMPLEADOS en la base de datos.", "Tabla Vacía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = null; // Limpia la vista
                }
            }
            catch (Exception ex)
            {
                // Muestra un error detallado si hay problemas de conexión o en el proceso de lectura
                MessageBox.Show($"Error crítico al cargar los empleados. Verifique la conexión a la BD y las credenciales:\n{ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = null;
            }
        }

        // Aquí agregarías otros métodos de evento, como:

        /*
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Lógica para abrir formulario de adición
        }
        
        private void btnRecargar_Click(object sender, EventArgs e)
        {
            CargarEmpleados(); // Recarga los datos de la base de datos
        }
        */
    }
}
