using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace LoginAplication
{
    public partial class Form3 : Form
    {

        private Conexion conexion;
       
        public Form3()
        {

            InitializeComponent();
            conexion = new Conexion();
            MessageBox.Show("Favor ingresar el número original del DUI del paciente para realizar la búsqueda.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
         


        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Esta información solo puede ser actualizada por su médico de cabecera.",
           "Alerta",
           MessageBoxButtons.OK,
           MessageBoxIcon.Warning);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Cierra el formulario actual
            this.Close();

            // Abre el formulario del menú principal y pasa el argumento 'usuario'
            Form2 menu = new Form2();
            menu.Show();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            // Cierra el formulario actual
            this.Close();

            // Abre el formulario del menú principal y pasa el argumento 'usuario'
            Form2 menu = new Form2();
            menu.Show();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener los datos actualizados de los campos del formulario
                string nombres = txtNombre.Text.Trim();
                string apellidos = txtApellidos.Text.Trim();
                DateTime fechaNacimiento = DateTime.Parse(txtNacimiento.Text);
                int edad = int.Parse(txtEdad.Text);
                string genero = txtGenero.Text.Trim();
                string telefono = txtTelefono.Text.Trim();
                string direccion = txtDireccion.Text.Trim();
                string correo = txtCorreo.Text.Trim();
                string dui = txtDui.Text.Trim(); // Ahora obtenemos el valor del DUI
                string seguro = txtSeguro.Text.Trim();
                string sanguineo = txtSanguineo.Text.Trim();
                string alergias = txtAlergias.Text.Trim();
                string medicamentos = txtMedicamentos.Text.Trim();
                DateTime ultimaConsulta = DateTime.Parse(txtUltimaConsulta.Text);

                using (SqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE paciente
                           SET nombres = @nombres,
                               apellidos = @apellidos,
                               fechadenacimiento = @fechaNacimiento,
                               edad = @edad,
                               genero = @genero,
                               telefono = @telefono,
                               direccion = @direccion,
                               correo = @correo,
                               seguro = @seguro,
                               sanguineo = @sanguineo,
                               alergias = @alergias,
                               medicamentos = @medicamentos,
                               ultimaconsulta = @ultimaConsulta
                           WHERE dui = @dui_original"; // Usamos el DUI original para identificar el paciente

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Asignar los valores a los parámetros
                        cmd.Parameters.AddWithValue("@nombres", nombres);
                        cmd.Parameters.AddWithValue("@apellidos", apellidos);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        cmd.Parameters.AddWithValue("@edad", edad);
                        cmd.Parameters.AddWithValue("@genero", genero);
                        cmd.Parameters.AddWithValue("@telefono", telefono);
                        cmd.Parameters.AddWithValue("@direccion", direccion);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@seguro", seguro);
                        cmd.Parameters.AddWithValue("@sanguineo", sanguineo);
                        cmd.Parameters.AddWithValue("@alergias", alergias);
                        cmd.Parameters.AddWithValue("@medicamentos", medicamentos);
                        cmd.Parameters.AddWithValue("@ultimaConsulta", ultimaConsulta);

                        // *** IMPORTANTE: ***
                        // Necesitas tener el DUI original del paciente que estás actualizando
                        // para usarlo en la cláusula WHERE. Esto generalmente se obtiene
                        // cuando realizas la búsqueda inicial del paciente.
                        // Asumo que tienes una variable o el valor en el txtDui desde la búsqueda.
                        cmd.Parameters.AddWithValue("@dui_original", txtDui.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Información del paciente actualizada correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar la información del paciente. Verifique el DUI.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la información: " + ex.Message);
            }
        }
        //
        private void txtDui_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, retroceso y el guion
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 45)
            {
                e.Handled = true; // Bloquear cualquier tecla que no sea un número, retroceso o guion
            }
        }



        private void txtDui_TextChanged(object sender, EventArgs e)
        {
            // Eliminar cualquier espacio o guion previo
            string dui = txtDui.Text.Replace("-", "").Trim();

            // Limitar a 9 caracteres para los 8 números más el dígito de verificación
            if (dui.Length > 9)
            {
                dui = dui.Substring(0, 9);
            }

            // Agregar el guion después de los primeros 8 caracteres
            if (dui.Length == 8)
            {
                dui = dui.Substring(0, 8) + "-" + dui.Substring(8);
            }

            // Actualizar el campo de texto
            txtDui.Text = dui;
            txtDui.SelectionStart = txtDui.Text.Length; // Mantener el cursor al final
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string dui = txtBusqueda.Text.Trim();

                using (SqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM paciente WHERE dui = @dui";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dui", dui);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtNombre.Text = reader["nombres"].ToString();

                                txtApellidos.Text = reader["apellidos"].ToString();
                                txtNacimiento.Text = Convert.ToDateTime(reader["fechadenacimiento"]).ToShortDateString();
                                txtEdad.Text = reader["edad"].ToString();
                                txtGenero.Text = reader["genero"].ToString();
                                txtTelefono.Text = reader["telefono"].ToString();
                                txtDireccion.Text = reader["direccion"].ToString();
                                txtCorreo.Text = reader["correo"].ToString();
                                txtDui.Text = reader["dui"].ToString();
                                txtSeguro.Text = reader["seguro"].ToString();
                                txtSanguineo.Text = reader["sanguineo"].ToString();
                                txtAlergias.Text = reader["alergias"].ToString();
                                txtMedicamentos.Text = reader["medicamentos"].ToString();
                                txtUltimaConsulta.Text = Convert.ToDateTime(reader["ultimaconsulta"]).ToShortDateString();
                            }
                            else
                            {
                                MessageBox.Show("Paciente no encontrado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información: " + ex.Message);
            }
        }




        private void button6_Click(object sender, EventArgs e)
        {
            
            txtNombre.Text = "";
            txtBusqueda.Text = "";
            txtApellidos.Text = "";
            txtNacimiento.Text = "";
            txtTelefono.Text = "";
            txtEdad.Text = "";
            txtGenero.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            txtCorreo.Text = "";
            txtDui.Text = "";
            txtSeguro.Text = "";
            txtSanguineo.Text = "";
            txtAlergias.Text = "";
            txtMedicamentos.Text = "";
            txtUltimaConsulta.Text = "";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


    



