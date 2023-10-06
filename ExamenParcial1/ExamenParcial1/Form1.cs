using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ExamenParcial1
{
    public partial class Form1 : Form
    {
        private RegistroPacientes registroPacientes;

        public Form1()
        {
            InitializeComponent();
            registroPacientes = new RegistroPacientes();
            dgvPacientes.AutoGenerateColumns = true; // Configurar las columnas automáticamente
            dgvPacientes.ReadOnly = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Obtén los datos de los TextBox y crea un nuevo paciente
                Paciente nuevoPaciente = new Paciente
                {
                    Nombre = txtNombre.Text,
                    Edad = int.Parse(txtEdad.Text),
                    Genero = txtGenero.Text,
                    NumeroTelefono = txtTelefono.Text,
                    DiagnosticoMedico = txtDiagnostico.Text,

                };

                // Agrega el paciente al registro
                registroPacientes.AgregarPaciente(nuevoPaciente);

                // Actualiza el DataGridView
                ActualizarDataGridView();

                // Limpia los TextBox
                LimpiarTextBox();
                MostrarEstadisticas();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreABuscar = txtNombre.Text;
            List<Paciente> pacientesEncontrados = registroPacientes.BuscarPacientePorNombre(nombreABuscar);

            // Actualiza el DataGridView con los pacientes encontrados
            dgvPacientes.DataSource = pacientesEncontrados;

            // Actualiza las estadísticas
            MostrarEstadisticas();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Obtén el paciente seleccionado en el DataGridView
            if (dgvPacientes.SelectedRows.Count > 0)
            {
                Paciente pacienteSeleccionado = (Paciente)dgvPacientes.SelectedRows[0].DataBoundItem;

                // Elimina el paciente del registro
                registroPacientes.EliminarPaciente(pacienteSeleccionado);

                // Actualiza el DataGridView
                ActualizarDataGridView();

                // Actualiza las estadísticas
                MostrarEstadisticas();
            }
        }

        private void ActualizarDataGridView()
        {
            dgvPacientes.DataSource = null; // Limpia la fuente de datos
            dgvPacientes.DataSource = registroPacientes.ObtenerListaPacientes();
        }

        private void MostrarEstadisticas()
        {
            int totalPacientes = registroPacientes.ObtenerTotalPacientes();
            double edadPromedio = registroPacientes.ObtenerEdadPromedio();

            lblTotalPacientes.Text = $"Total de Pacientes: {totalPacientes}";
            lblEdadPromedio.Text = $"Edad Promedio: {edadPromedio:F2}";
        }

        private void LimpiarTextBox()
        {
            txtNombre.Clear();
            txtEdad.Clear();
            txtGenero.Clear();
            txtTelefono.Clear();
            txtDiagnostico.Clear();
        }
    }
}
public class Paciente
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Genero { get; set; }
    public string NumeroTelefono { get; set; }
    public string DiagnosticoMedico { get; set; }
}

public class RegistroPacientes
{
    private List<Paciente> pacientes;

    public RegistroPacientes()
    {
        pacientes = new List<Paciente>();
    }

    public void AgregarPaciente(Paciente paciente)
    {
        pacientes.Add(paciente);
    }

    public List<Paciente> BuscarPacientePorNombre(string nombre)
    {
        return pacientes.Where(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public void EliminarPaciente(Paciente paciente)
    {
        pacientes.Remove(paciente);
    }

    public List<Paciente> ObtenerListaPacientes()
    {
        return pacientes;
    }

    public int ObtenerTotalPacientes()
    {
        return pacientes.Count;
    }

    public double ObtenerEdadPromedio()
    {
        if (pacientes.Count == 0)
        {
            return 0;
        }
        double sumaEdades = pacientes.Sum(p => p.Edad);
        return sumaEdades / pacientes.Count;
    }
}
