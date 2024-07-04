using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Jakunen_Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void addAdminButton_Click(object sender, EventArgs e)
        {
            AdminCreationForm adminForm = new AdminCreationForm();
            adminForm.ShowDialog();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("ユーザー名とパスワードを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(DatabaseConnector.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT PasswordHash FROM Admins WHERE Username = @Username";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    string storedHash = cmd.ExecuteScalar()?.ToString();

                    if (storedHash != null && BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        MessageBox.Show("ログイン成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // ログイン成功後の処理をここに追加
                    }
                    else
                    {
                        MessageBox.Show("ユーザー名またはパスワードが間違っています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("エラーが発生しました: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
