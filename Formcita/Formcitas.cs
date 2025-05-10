using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace Formcita
{
    public partial class Formcitas : Form
    {
        private readonly string connectionString = "Server=KEVINARGUMEDO\\SQLEXPRESS;Database=loginapp;User Id=sa;Password=12345678;";

        private class DoctorInfo
        {
            public int IdDoctor { get; set; }
            public string NombreCompleto { get; set; }
            public int IdEspecialidad { get; set; }
            public override string ToString() => NombreCompleto;
        }

        private class EspecialidadInfo
        {
            public int IdEspecialidad { get; set; }
            public string NombreEspecialidad { get; set; }
            public override string ToString() => NombreEspecialidad;
        }

        private class PacienteInfo
        {
            public int IdPaciente { get; set; }
            public string NombreCompleto { get; set; }
            public override string ToString() => NombreCompleto;
        }

        public Formcitas()
        {
            InitializeComponent();
            CargarEspecialidades();
            CargarDoctores();
            CargarPacientes();
        }

        private void CargarEspecialidades()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    const string query = "SELECT IdEspecialidad, Nombre FROM Especialidades;";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        listBox2.Items.Clear();
                        listBox2.Items.Add(new EspecialidadInfo { IdEspecialidad = -1, NombreEspecialidad = "Todas" });
                        while (reader.Read())
                        {
                            listBox2.Items.Add(new EspecialidadInfo
                            {
                                IdEspecialidad = (int)reader["IdEspecialidad"],
                                NombreEspecialidad = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las especialidades: {ex.Message}");
            }
        }

        private void CargarDoctores()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IdDoctor, Nombre, Apellido, IdEspecialidad FROM Doctor";

                    if (listBox2.SelectedItem is EspecialidadInfo selectedEspecialidad && selectedEspecialidad.IdEspecialidad != -1)
                    {
                        query += " WHERE IdEspecialidad = @idEspecialidad";
                    }

                    using (var command = new SqlCommand(query, connection))
                    {
                        if (listBox2.SelectedItem is EspecialidadInfo especialidad && especialidad.IdEspecialidad != -1)
                        {
                            command.Parameters.AddWithValue("@idEspecialidad", especialidad.IdEspecialidad);
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            listBox1.Items.Clear();
                            while (reader.Read())
                            {
                                listBox1.Items.Add(new DoctorInfo
                                {
                                    IdDoctor = (int)reader["IdDoctor"],
                                    NombreCompleto = $"{reader["Nombre"]} {reader["Apellido"]}",
                                    IdEspecialidad = (int)reader["IdEspecialidad"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los doctores: {ex.Message}");
            }
        }

        private void CargarPacientes()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    const string query = "SELECT id_paciente, nombres, apellidos FROM dbo.paciente;";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        listBoxPacientes.Items.Clear();
                        while (reader.Read())
                        {
                            listBoxPacientes.Items.Add(new PacienteInfo
                            {
                                IdPaciente = (int)reader["id_paciente"],
                                NombreCompleto = $"{reader["nombres"]} {reader["apellidos"]}"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los pacientes: {ex.Message}");
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDoctores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is DoctorInfo selectedDoctor && listBoxPacientes.SelectedItem is PacienteInfo selectedPaciente)
            {
                try
                {
                    DateTime fechaCita = dateTimePicker1.Value.Date;
                    TimeSpan horaCita = dateTimePicker2.Value.TimeOfDay;
                    DateTime fechaHoraCita = fechaCita.Add(horaCita);

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        const string query = "INSERT INTO Cita (Fecha, IdDoctor, id_paciente) VALUES (@fecha, @idDoctor, @idPaciente)";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@fecha", fechaHoraCita);
                            command.Parameters.AddWithValue("@idDoctor", selectedDoctor.IdDoctor);
                            command.Parameters.AddWithValue("@idPaciente", selectedPaciente.IdPaciente);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Cita agendada en la base de datos.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agendar la cita: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un doctor y un paciente.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        
    }
}
