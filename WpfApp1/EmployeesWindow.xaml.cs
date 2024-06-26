﻿using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
// 124124124142
namespace WpfApp1
{
    public partial class EmployeesWindow : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Functions { get; private set; }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        private UserPermissions _userPermissions;

        public EmployeesWindow(string username)
        {
            InitializeComponent();
            Username = username;
            LoadEmployees();
            EmployeesDataGrid.ItemsSource = Employees;
            CheckPermissions();
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "SELECT Id, LastName, FirstName, MiddleName, Position, Phone, NumberTabel FROM Employees";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            Position = reader["Position"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            NumberTabel = reader["NumberTabel"].ToString(), // Добавляем новое поле
                        });
                    }
                }
            }
        }

        private void CheckPermissions()
        {
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "SELECT Functions FROM Users WHERE Username=@Username";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", Username);
                int functions = Convert.ToInt32(cmd.ExecuteScalar());

                if (functions == 0)
                {
                    FunButtons.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FunButtons.Visibility = Visibility.Visible;
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(Employees);
            if (addEmployeeWindow.ShowDialog() == true)
            {
                LoadEmployees(); // Перезагрузить сотрудников из базы данных
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
            {
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(selectedEmployee.Id);
                if (editEmployeeWindow.ShowDialog() == true)
                {
                    Employee editedEmployee = editEmployeeWindow.CurrentEmployee;

                    using (var connection = new SQLiteConnection("Data Source=users.db"))
                    {
                        connection.Open();
                        string query = "UPDATE Employees SET LastName=@LastName, FirstName=@FirstName, MiddleName=@MiddleName, Position=@Position, Phone=@Phone, NumberTabel=@NumberTabel, " +
                            "Male=@Male, DateBrt=@DateBrt, Nation=@Nation, Family=@Family, Place=@Place, Profession=@Profession, SecProfession=@SecProfession WHERE Id=@Id";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@LastName", editedEmployee.LastName);
                            cmd.Parameters.AddWithValue("@FirstName", editedEmployee.FirstName);
                            cmd.Parameters.AddWithValue("@MiddleName", editedEmployee.MiddleName);
                            cmd.Parameters.AddWithValue("@Position", editedEmployee.Position);
                            cmd.Parameters.AddWithValue("@Phone", editedEmployee.Phone);
                            cmd.Parameters.AddWithValue("@Id", editedEmployee.Id);
                            cmd.Parameters.AddWithValue("@NumberTabel", editedEmployee.NumberTabel);
                            cmd.Parameters.AddWithValue("@Male", editedEmployee.Male);
                            cmd.Parameters.AddWithValue("@DateBrt", editedEmployee.DateBrt);
                            cmd.Parameters.AddWithValue("@Nation", editedEmployee.Nation);
                            cmd.Parameters.AddWithValue("@Family", editedEmployee.Family);
                            cmd.Parameters.AddWithValue("@Place", editedEmployee.Place);
                            cmd.Parameters.AddWithValue("@Profession", editedEmployee.Profession);
                            cmd.Parameters.AddWithValue("@SecProfession", editedEmployee.SecProfession);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadEmployees(); // Перезагрузка данных после редактирования
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
            {
                using (var connection = new SQLiteConnection("Data Source=users.db"))
                {
                    connection.Open();
                    string query = "DELETE FROM Employees WHERE Id=@Id";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", selectedEmployee.Id);
                    cmd.ExecuteNonQuery();
                }

                LoadEmployees();
            }
        }

        private void SearchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredEmployees = Employees.Where(emp =>
                emp.LastName.ToLower().Contains(searchText) ||
                emp.FirstName.ToLower().Contains(searchText) ||
                emp.MiddleName.ToLower().Contains(searchText) ||
                emp.Position.ToLower().Contains(searchText) ||
                emp.Phone.ToLower().Contains(searchText) ||
                emp.NumberTabel.ToLower().Contains(searchText));

            EmployeesDataGrid.ItemsSource = filteredEmployees;
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Male { get; set; }
        public string DateBrt { get; set; }
        public string Nation { get; set; }
        public string Family { get; set; }
        public string Place { get; set; }
        public string Profession { get; set; }
        public string SecProfession { get; set; }
        public string NumberTabel { get; set; }
    }

    public class UserPermissions
    {
        public string Role { get; set; }
        public bool Functions { get; set; }
    }
}
