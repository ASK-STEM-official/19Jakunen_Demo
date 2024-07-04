using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Jakunen_Demo
{
    public partial class AdminCreationForm : Form
    {
        public AdminCreationForm()
        {
            InitializeComponent();
        }

        private void createAdminButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string fullName = fullNameTextBox.Text;
            string email = emailTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("すべてのフィールドを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            using (MySqlConnection conn = new MySqlConnection(DatabaseConnector.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Admins (Username, PasswordHash, FullName, Email) VALUES (@Username, @PasswordHash, @FullName, @Email)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"ユーザー名: {username}\nパスワードハッシュ: {passwordHash}\nフルネーム: {fullName}\nメール: {email}", "管理者追加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("エラーが発生しました: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
