using System.Data.SQLite;
using System;
using System.Windows;
using System.Reflection.PortableExecutable;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "SELECT Username FROM Users WHERE Username=@Username AND Password=@Password";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        DashboardWindow dashboard = new DashboardWindow(username, password);
                        dashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин или пароль.");
                    }
                }
            }
        }
    }
}
