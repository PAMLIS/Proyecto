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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(*) FROM loginapp WHERE usuario = @usuario AND password = @password";
            Conexion conexion = new Conexion();
            using (SqlConnection connection = conexion.GetConnection())
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                    cmd.Parameters.AddWithValue("@password", txtPass.Text);

                    int count = (int)cmd.ExecuteScalar();

                    if (count == 1)
                    {
                        Form2 form2 = new Form2(); // Pasar el usuario al Form2
                        form2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
                }
            }
        }

        ///
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            MessageBox.Show("Por favor contacte con nuestro soporte técnico más cercano para mayor información",
             "Alerta",
             MessageBoxButtons.OK,
             MessageBoxIcon.Warning);

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
          txtPass.PasswordChar = '*';
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
