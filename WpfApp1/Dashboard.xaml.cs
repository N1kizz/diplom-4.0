using System.Data.SQLite;
using System;
using System.Windows;

namespace WpfApp1
{
    public partial class DashboardWindow : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public DashboardWindow(string username, string password)
        {
            InitializeComponent();
            Username = username;
            Password = password;
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Role FROM Users WHERE Username=@Username";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Username", Username);

                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Пользователь не найден или роль не установлена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    string role = result.ToString();
                    if (role == "Администратор" || role == "Сотрудник")
                    {
                        EmployeesWindow employeesWindow = new EmployeesWindow(Username);
                        employeesWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Отсутствие прав на просмотр данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при обращении к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (Username == "admin" && Password == "123")
            {
                // Успешный вход
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
            }
            else
            {
                // Ошибка входа
                MessageBox.Show("Отсутствие прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}