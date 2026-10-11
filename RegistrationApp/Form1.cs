using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace RegistrationApp
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

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            string email = txtEmail.Text.Trim().ToLowerInvariant();
            // Designer uses 'textBox2' for the password field
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter both email and password.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["RegistrationDb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Connection string 'RegistrationDb' not found in configuration.", "Configuration error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            const string sql = "SELECT PasswordHash FROM dbo.Users WHERE Email = @Email;";

            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string storedHash = result.ToString();

                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

                    if (isPasswordValid)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Login successful!";
                        // TODO: proceed to next form
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Invalid email or password.";
                    }
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Invalid email or password.";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            // Open the registration form and hide this form while it's open.
            var registrationForm = new RegistrationForm();
            registrationForm.FormClosed += (s, args) => this.Show();
            registrationForm.Show();
            this.Hide();
        }
    }
}
