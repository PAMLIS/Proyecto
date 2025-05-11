using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace FormularioDePagoApp
{
    public partial class Form1 : Form
    {
        private string cadenaConexion = "Server=ROBERTOREYES;Database=GestionPagos;Integrated Security=True;"; 

        private Label lblNombrePaciente;
        private TextBox txtNombrePaciente;
        private Label lblFechaPago;
        private DateTimePicker dtpFechaPago;
        private Label lblMontoOriginal;
        private TextBox txtMontoOriginal;
        private Label lblDescuento;
        private TextBox txtDescuento;
        private Label lblTipoDescuento;
        private ComboBox cmbTipoDescuento;
        private Label lblMontoFinal;
        private TextBox txtMontoFinal;
        private Label lblServiciosFacturados;
        private ComboBox cmbServiciosFacturados;
        private Label lblMetodoPago;
        private ComboBox cmbMetodoPago;
        private Label lblHistorialPagos;
        private DataGridView dgvHistorialPagos;
        private Button btnGuardarPago;
        private GroupBox grpDatosPaciente;
        private GroupBox grpDatosPago;

        public Form1()
        {
            InitializeComponent();
            CrearControlesManualmente();
        }

        private void CrearControlesManualmente()
        {
            grpDatosPaciente = new GroupBox { Text = "Información del Paciente", Location = new Point(10, 10), Size = new Size(480, 70) };
            grpDatosPago = new GroupBox { Text = "Información del Pago", Location = new Point(10, 90), Size = new Size(480, 190) };

            lblNombrePaciente = new Label { Text = "Nombre:", Location = new Point(15, 30), Size = new Size(100, 20) };
            txtNombrePaciente = new TextBox { Location = new Point(120, 27), Size = new Size(340, 25) };

            grpDatosPaciente.Controls.Add(lblNombrePaciente);
            grpDatosPaciente.Controls.Add(txtNombrePaciente);

            lblFechaPago = new Label { Text = "Fecha:", Location = new Point(15, 30), Size = new Size(100, 20) };
            dtpFechaPago = new DateTimePicker { Location = new Point(120, 27), Size = new Size(150, 25) };

            lblMontoOriginal = new Label { Text = "Monto Original:", Location = new Point(15, 60), Size = new Size(100, 20) };
            txtMontoOriginal = new TextBox { Location = new Point(120, 57), Size = new Size(100, 25) };
            txtMontoOriginal.TextChanged += txtMontoOriginal_TextChanged;

            lblDescuento = new Label { Text = "Descuento:", Location = new Point(280, 60), Size = new Size(80, 20) };
            txtDescuento = new TextBox { Location = new Point(365, 57), Size = new Size(70, 25) };
            txtDescuento.TextChanged += txtDescuento_TextChanged;

            lblTipoDescuento = new Label { Text = "Tipo:", Location = new Point(15, 90), Size = new Size(100, 20) };
            cmbTipoDescuento = new ComboBox { Location = new Point(120, 87), Size = new Size(120, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoDescuento.Items.AddRange(new string[] { "Porcentaje", "Monto" });
            cmbTipoDescuento.SelectedIndexChanged += cmbTipoDescuento_SelectedIndexChanged;

            lblMontoFinal = new Label { Text = "Monto Final:", Location = new Point(280, 90), Size = new Size(80, 20) };
            txtMontoFinal = new TextBox { Location = new Point(365, 87), Size = new Size(100, 25), ReadOnly = true };

            lblServiciosFacturados = new Label { Text = "Servicio:", Location = new Point(15, 120), Size = new Size(100, 20) };
            cmbServiciosFacturados = new ComboBox { Location = new Point(120, 117), Size = new Size(340, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbServiciosFacturados.Items.AddRange(new string[] {
                "Consulta General",
                "Consulta Especializada (Cardiología)",
                "Consulta Especializada (Dermatología)",
                "Consulta Especializada (Neurología)",
                "Análisis de Sangre Completo",
                "Análisis de Orina",
                "Radiografía Simple",
                "Ecografía",
                "Resonancia Magnética (RM)",
                "Tomografía Computarizada (TC)",
                "Fisioterapia",
                "Terapia Ocupacional",
                "Psicología Clínica",
                "Odontología General",
                "Limpieza Dental"
            });

            lblMetodoPago = new Label { Text = "Método Pago:", Location = new Point(15, 150), Size = new Size(100, 20) };
            cmbMetodoPago = new ComboBox { Location = new Point(120, 147), Size = new Size(150, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMetodoPago.Items.AddRange(new string[] { "Efectivo", "Tarjeta de Crédito", "Tarjeta de Débito", "Transferencia Bancaria", "Cheque" });

            grpDatosPago.Controls.Add(lblFechaPago);
            grpDatosPago.Controls.Add(dtpFechaPago);
            grpDatosPago.Controls.Add(lblMontoOriginal);
            grpDatosPago.Controls.Add(txtMontoOriginal);
            grpDatosPago.Controls.Add(lblDescuento);
            grpDatosPago.Controls.Add(txtDescuento);
            grpDatosPago.Controls.Add(lblTipoDescuento);
            grpDatosPago.Controls.Add(cmbTipoDescuento);
            grpDatosPago.Controls.Add(lblMontoFinal);
            grpDatosPago.Controls.Add(txtMontoFinal);
            grpDatosPago.Controls.Add(lblServiciosFacturados);
            grpDatosPago.Controls.Add(cmbServiciosFacturados);
            grpDatosPago.Controls.Add(lblMetodoPago);
            grpDatosPago.Controls.Add(cmbMetodoPago);

            lblHistorialPagos = new Label { Text = "Historial de Pagos:", Location = new Point(10, 290), Size = new Size(150, 20) };
            dgvHistorialPagos = new DataGridView { Location = new Point(10, 310), Size = new Size(480, 150), ReadOnly = true, AutoGenerateColumns = false };
            dgvHistorialPagos.Columns.Add("FechaPago", "Fecha");
            dgvHistorialPagos.Columns.Add("MontoFinal", "Monto");
            dgvHistorialPagos.Columns.Add("MetodoPago", "Método");

            btnGuardarPago = new Button { Text = "Guardar Pago", Location = new Point(380, 470), Size = new Size(110, 35) };
            btnGuardarPago.Click += btnGuardarPago_Click;

            Controls.Add(grpDatosPaciente);
            Controls.Add(grpDatosPago);
            Controls.Add(lblHistorialPagos);
            Controls.Add(dgvHistorialPagos);
            Controls.Add(btnGuardarPago);

            ClientSize = new Size(510, 520);
            Text = "Formulario de Pago";
        }

        private void txtMontoOriginal_TextChanged(object sender, EventArgs e)
        {
            CalcularMontoFinal();
        }

        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            CalcularMontoFinal();
        }

        private void cmbTipoDescuento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularMontoFinal();
        }

        private void CalcularMontoFinal()
        {
            if (decimal.TryParse(txtMontoOriginal.Text, out decimal montoOriginal))
            {
                if (decimal.TryParse(txtDescuento.Text, out decimal descuento))
                {
                    decimal montoFinal = montoOriginal;

                    if (cmbTipoDescuento.SelectedItem != null)
                    {
                        if (cmbTipoDescuento.SelectedItem.ToString() == "Porcentaje")
                        {
                            montoFinal = montoOriginal - (montoOriginal * (descuento / 100));
                        }
                        else if (cmbTipoDescuento.SelectedItem.ToString() == "Monto")
                        {
                            montoFinal = montoOriginal - descuento;
                        }
                    }

                    txtMontoFinal.Text = montoFinal.ToString("N2");
                }
                else
                {
                    txtMontoFinal.Text = montoOriginal.ToString("N2");
                }
            }
            else
            {
                txtMontoFinal.Text = "";
            }
        }

        private void btnGuardarPago_Click(object sender, EventArgs e)
        {
            string nombrePaciente = txtNombrePaciente.Text.Trim();

            if (string.IsNullOrEmpty(nombrePaciente))
            {
                MessageBox.Show("Por favor, ingrese el nombre del paciente.");
                return;
            }

            if (!DateTime.TryParse(dtpFechaPago.Text, out DateTime fechaPago))
            {
                MessageBox.Show("Por favor, ingrese una fecha de pago válida.");
                return;
            }

            if (!decimal.TryParse(txtMontoOriginal.Text, out decimal montoOriginal))
            {
                MessageBox.Show("Por favor, ingrese un monto original válido.");
                return;
            }

            decimal descuento = decimal.TryParse(txtDescuento.Text, out decimal descuentoValue) ? descuentoValue : 0;
            string tipoDescuento = cmbTipoDescuento.SelectedItem?.ToString();
            string serviciosFacturados = cmbServiciosFacturados.SelectedItem?.ToString();
            string metodoPago = cmbMetodoPago.SelectedItem?.ToString();

            if (!decimal.TryParse(txtMontoFinal.Text, out decimal montoFinal))
            {
                MessageBox.Show("El monto final no se ha calculado correctamente.");
                return;
            }

            SqlConnection conexionLocal = new SqlConnection(cadenaConexion);
            try
            {
                conexionLocal.Open();
                string consultaPaciente = "INSERT INTO Pacientes (NombreCompleto) VALUES (@NombreCompleto); SELECT SCOPE_IDENTITY();";
                SqlCommand comandoPaciente = new SqlCommand(consultaPaciente, conexionLocal);
                comandoPaciente.Parameters.AddWithValue("@NombreCompleto", nombrePaciente);
                int pacienteId = Convert.ToInt32(comandoPaciente.ExecuteScalar());

                string consultaPago = "INSERT INTO Pagos (PacienteID, FechaPago, MontoOriginal, Descuento, TipoDescuento, MontoFinal, ServiciosFacturados, MetodoPago) " +
                                      "VALUES (@PacienteID, @FechaPago, @MontoOriginal, @Descuento, @TipoDescuento, @MontoFinal, @ServiciosFacturados, @MetodoPago)";
                SqlCommand comandoPago = new SqlCommand(consultaPago, conexionLocal);
                comandoPago.Parameters.AddWithValue("@PacienteID", pacienteId);
                comandoPago.Parameters.AddWithValue("@FechaPago", fechaPago);
                comandoPago.Parameters.AddWithValue("@MontoOriginal", montoOriginal);
                comandoPago.Parameters.AddWithValue("@Descuento", descuento);
                comandoPago.Parameters.AddWithValue("@TipoDescuento", string.IsNullOrEmpty(tipoDescuento) ? (object)DBNull.Value : tipoDescuento);
                comandoPago.Parameters.AddWithValue("@MontoFinal", montoFinal);
                comandoPago.Parameters.AddWithValue("@ServiciosFacturados", serviciosFacturados ?? (object)DBNull.Value);
                comandoPago.Parameters.AddWithValue("@MetodoPago", metodoPago ?? (object)DBNull.Value);

                int filasAfectadas = comandoPago.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Pago guardado exitosamente.");
                    CargarHistorialPagos(pacienteId);
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("Error al guardar el pago.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el pago: " + ex.Message);
            }
            finally
            {
                if (conexionLocal.State == ConnectionState.Open)
                {
                    conexionLocal.Close();
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtNombrePaciente.Clear();
            dtpFechaPago.Value = DateTime.Now;
            txtMontoOriginal.Clear();
            txtDescuento.Clear();
            cmbTipoDescuento.SelectedIndex = -1;
            txtMontoFinal.Clear();
            cmbServiciosFacturados.SelectedIndex = -1;
            cmbMetodoPago.SelectedIndex = -1;
        }

        private void CargarHistorialPagos(int pacienteId)
        {
            SqlConnection conexionLocal = new SqlConnection(cadenaConexion);
            try
            {
                conexionLocal.Open();
                string consulta = "SELECT FechaPago, MontoFinal, MetodoPago FROM Pagos WHERE PacienteID = @PacienteID ORDER BY FechaPago DESC";
                SqlCommand comando = new SqlCommand(consulta, conexionLocal);
                comando.Parameters.AddWithValue("@PacienteID", pacienteId);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dtHistorial = new DataTable();
                adaptador.Fill(dtHistorial);
                dgvHistorialPagos.DataSource = dtHistorial;

                dgvHistorialPagos.Columns["FechaPago"].HeaderText = "Fecha";
                dgvHistorialPagos.Columns["MontoFinal"].HeaderText = "Monto";
                dgvHistorialPagos.Columns["MetodoPago"].HeaderText = "Método";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el historial de pagos: " + ex.Message);
            }
            finally
            {
                if (conexionLocal.State == ConnectionState.Open)
                {
                    conexionLocal.Close();
                }
            }
        }
    }
}