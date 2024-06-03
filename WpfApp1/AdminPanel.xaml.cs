using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public List<UserPermission> UserPermissions { get; set; }

        public AdminPanel()
        {
            InitializeComponent();
            LoadUserPermissions();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = NewUsernameTextBox.Text;
            string password = NewPasswordBox.Password;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem)?.Content.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Логин, пароль или роль не могут быть пустыми.");
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "INSERT INTO users (Username, Password, Role, Functions) VALUES (@Username, @Password, @Role, @Functions)";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Functions", 1); // Default to 1 (no restrictions)
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Пользователь успешно зарегистрирован.");
            LoadUserPermissions();
        }

        private void LoadUserPermissions()
        {
            UserPermissions = new List<UserPermission>();

            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "SELECT Username, Password, Role, Functions FROM users";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserPermissions.Add(new UserPermission
                    {
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        Functions = Convert.ToInt32(reader["Functions"])
                    });
                }
            }

            UserPermissionsDataGrid.ItemsSource = UserPermissions;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();

                foreach (var userPermission in UserPermissions)
                {
                    string query = "UPDATE users SET Password = @Password, Role = @Role, Functions = @Functions WHERE Username = @Username";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Password", userPermission.Password);
                    cmd.Parameters.AddWithValue("@Role", userPermission.Role);
                    cmd.Parameters.AddWithValue("@Functions", userPermission.Functions);
                    cmd.Parameters.AddWithValue("@Username", userPermission.Username);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Права доступа обновлены.");
        }
    }

    public class UserPermission
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int Functions { get; set; }
    }
}
