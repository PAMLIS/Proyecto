using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class Paciente
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
    public partial class FormPacientes : Form
    {
        private BindingList<Paciente> _pacientes = new BindingList<Paciente>();
        private DataGridView dgvPacientes;
        private TableLayoutPanel layout;
        private TextBox txtNombre, txtApellidos, txtTelefono, txtDireccion;
        private DateTimePicker dtpFecha;
        private ComboBox cmbGenero;
        private Button btnGuardar, btnLimpiar, btnCerrar;

        public FormPacientes()
        {
            Text = "Registro de Pacientes";
            Size = new Size(800, 600);

            InitializeControls();
            LayoutControls();
            HookEvents();
        }

        private void InitializeControls()
        {
            
            dgvPacientes = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250,
                AutoGenerateColumns = false,
                DataSource = _pacientes
            };
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 120 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Apellidos", DataPropertyName = "Apellidos", Width = 150 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha Nac.", DataPropertyName = "FechaNacimiento", Width = 100, DefaultCellStyle = { Format = "d" } });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Género", DataPropertyName = "Genero", Width = 80 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Teléfono", DataPropertyName = "Telefono", Width = 100 });
            dgvPacientes.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Dirección", DataPropertyName = "Direccion", Width = 200 });

            
            txtNombre = new TextBox { Dock = DockStyle.Fill };
            txtApellidos = new TextBox { Dock = DockStyle.Fill };
            dtpFecha = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            cmbGenero = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGenero.Items.AddRange(new[] { "Masculino", "Femenino", "Otro" });
            cmbGenero.SelectedIndex = 0;
            txtTelefono = new TextBox { Dock = DockStyle.Fill };
            txtDireccion = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 50 };

            
            btnGuardar = new Button { Text = "Guardar", Dock = DockStyle.Fill };
            btnLimpiar = new Button { Text = "Limpiar", Dock = DockStyle.Fill };
            btnCerrar = new Button { Text = "Cerrar", Dock = DockStyle.Fill };

            
            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 4,
                Padding = new Padding(10),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 4; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        }

        private void LayoutControls()
        {
            Controls.Add(dgvPacientes);

           
            layout.Controls.Add(new Label { Text = "Nombre:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            layout.Controls.Add(txtNombre, 1, 0);
            layout.Controls.Add(new Label { Text = "Apellidos:", Anchor = AnchorStyles.Right, AutoSize = true }, 2, 0);
            layout.Controls.Add(txtApellidos, 3, 0);

            
            layout.Controls.Add(new Label { Text = "Fecha Nac.:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            layout.Controls.Add(dtpFecha, 1, 1);
            layout.Controls.Add(new Label { Text = "Género:", Anchor = AnchorStyles.Right, AutoSize = true }, 2, 1);
            layout.Controls.Add(cmbGenero, 3, 1);

           
            layout.Controls.Add(new Label { Text = "Teléfono:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            layout.Controls.Add(txtTelefono, 1, 2);
            layout.Controls.Add(new Label { Text = "Dirección:", Anchor = AnchorStyles.Right, AutoSize = true }, 2, 2);
            layout.Controls.Add(txtDireccion, 3, 2);

            
            layout.Controls.Add(btnGuardar, 0, 4);
            layout.Controls.Add(btnLimpiar, 1, 4);
            layout.Controls.Add(btnCerrar, 2, 4);
            layout.SetColumnSpan(btnCerrar, 2);

            Controls.Add(layout);
            layout.BringToFront();
        }

        private void HookEvents()
        {
            btnGuardar.Click += BtnGuardar_Click;
            btnLimpiar.Click += (s, e) => ClearForm();
            btnCerrar.Click += (s, e) => Close();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MessageBox.Show("Nombre y Apellidos son obligatorios.",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _pacientes.Add(new Paciente
            {
                Nombre = txtNombre.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                FechaNacimiento = dtpFecha.Value.Date,
                Genero = cmbGenero.Text,
                Telefono = txtTelefono.Text.Trim(),
                Direccion = txtDireccion.Text.Trim()
            });

            MessageBox.Show("Paciente guardado con éxito.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }

        private void ClearForm()
        {
            txtNombre.Clear();
            txtApellidos.Clear();
            dtpFecha.Value = DateTime.Today;
            cmbGenero.SelectedIndex = 0;
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtNombre.Focus();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

    }
}

