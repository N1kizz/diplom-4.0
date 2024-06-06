using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections.Generic;
using OpenXmlPowerTools;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using static WpfApp1.MillitaryRegWindow;

namespace WpfApp1
{
    public partial class AddEmployeeWindow : Window
    {
        public Employee NewEmployee { get; private set; }
        private ObservableCollection<Employee> _employees;
        private TempIDData _tempPassportData;
        private TempMillitaryData _tempMillitaryData;

        public AddEmployeeWindow(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employees = employees;
            _tempPassportData = new TempIDData();
            _tempMillitaryData = new TempMillitaryData();
    }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<string> missingFields = new List<string>();

                // Проверка на заполнение всех обязательных полей
                if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                    missingFields.Add("Фамилия");

                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
                    missingFields.Add("Имя");

                if (string.IsNullOrWhiteSpace(MiddleNameTextBox.Text))
                    missingFields.Add("Отчество");

                if (string.IsNullOrWhiteSpace(PositionTextBox.Text))
                    missingFields.Add("Должность");

                if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                    missingFields.Add("Телефон");

                if (missingFields.Count > 0)
                {
                    string message = "Пожалуйста, заполните следующие поля:\n" + string.Join("\n", missingFields);
                    MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int employeeId = GetNextEmployeeId();


                NewEmployee = new Employee
                {
                    LastName = LastNameTextBox.Text,
                    FirstName = FirstNameTextBox.Text,
                    MiddleName = MiddleNameTextBox.Text,
                    Position = PositionTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Male = MaleComboBox.Text,
                    DateBrt = BirthdayDatePicker.Text,
                    Nation = NationalityTextBox.Text,
                    Family = FamilySatusTextBox.Text,
                    Place = BirthdayPlaceTextBox.Text,
                    Profession = ProfessonTextBox.Text,
                    SecProfession = SecondProfessionTextBox.Text,
                    NumberTabel = PersonnelNumberTextBox.Text
                };

                _employees.Add(NewEmployee);

                using (var connection = new SQLiteConnection("Data Source=users.db"))
                {
                    connection.Open();
                    string query = "INSERT INTO MilitaryRegistration1 (EmployeeId, GroupRegistration, CategotyRegistration, Composition, MilitaryRank, MilitarySpecialty, FitnessForService, MilitaryCommisariat, SpecialRegistration) " +
                                   "VALUES (@EmployeeId, @GroupRegistration, @CategoryRegistration, @Composition, @MilitaryRank, @MilitarySpecialty, @FitnessForService, @MilitaryCommisariat, @SpecialRegistration)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        //cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.Parameters.AddWithValue("@GroupRegistration", _tempMillitaryData.TempGroupRegistration);
                        cmd.Parameters.AddWithValue("@CategoryRegistration", _tempMillitaryData.TempCategoryRegistration);
                        cmd.Parameters.AddWithValue("@Composition", _tempMillitaryData.TempComposition);
                        cmd.Parameters.AddWithValue("@MilitaryRank", _tempMillitaryData.TempMilitaryRank);
                        cmd.Parameters.AddWithValue("@MilitarySpecialty", _tempMillitaryData.TempMilitarySpecialty);
                        cmd.Parameters.AddWithValue("@FitnessForService", _tempMillitaryData.TempFitnessForService);
                        cmd.Parameters.AddWithValue("@MilitaryCommisariat", _tempMillitaryData.TempMilitaryCommisariat);
                        cmd.Parameters.AddWithValue("@SpecialRegistration", _tempMillitaryData.TempSpecialRegistration);

                        cmd.ExecuteNonQuery();
                    }
                }

                using (var connection = new SQLiteConnection("Data Source=users.db"))
                {
                    connection.Open();
                    string query = "INSERT INTO PassportData (EmployeeId, PassportNumber, IdNumber, IssuedBy, IssueDate) VALUES (@EmployeeId, @PassportNumber, @IdNumber, @IssuedBy, @IssueDate)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId); // Здесь должно быть значение ID сотрудника
                        cmd.Parameters.AddWithValue("@PassportNumber", _tempPassportData.PassportNumber);
                        cmd.Parameters.AddWithValue("@IdNumber", _tempPassportData.IdNumber);
                        cmd.Parameters.AddWithValue("@IssuedBy", _tempPassportData.IssuedBy);
                        cmd.Parameters.AddWithValue("@IssueDate", _tempPassportData.IssueDate.ToString("yyyy-MM-dd")); // Предполагается, что IssueDate типа DateTime
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var connection = new SQLiteConnection("Data Source=users.db"))
                {
                    connection.Open();
                    string query = "INSERT INTO Employees (LastName, FirstName, MiddleName, Position, Phone, Male, DateBrt, Nation, Family, Place, Profession, SecProfession, NumberTabel) " +
                        "VALUES (@LastName, @FirstName, @MiddleName, @Position, @Phone, @Male, @DateBrt, @Nation, @Family, @Place, @Profession, @SecProfession, @NumberTabel)";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@LastName", NewEmployee.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", NewEmployee.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", NewEmployee.MiddleName);
                    cmd.Parameters.AddWithValue("@Position", NewEmployee.Position);
                    cmd.Parameters.AddWithValue("@Phone", NewEmployee.Phone);
                    cmd.Parameters.AddWithValue("@Male", NewEmployee.Male);
                    cmd.Parameters.AddWithValue("@DateBrt", NewEmployee.DateBrt);
                    cmd.Parameters.AddWithValue("@Nation", NewEmployee.Nation);
                    cmd.Parameters.AddWithValue("@Family", NewEmployee.Family);
                    cmd.Parameters.AddWithValue("@Place", NewEmployee.Place);
                    cmd.Parameters.AddWithValue("@Profession", NewEmployee.Profession);
                    cmd.Parameters.AddWithValue("@SecProfession", NewEmployee.SecProfession);
                    cmd.Parameters.AddWithValue("@NumberTabel", NewEmployee.NumberTabel);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении сотрудника: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            bool employeeAddedSuccessfully = true; // Пример

            if (employeeAddedSuccessfully)
            {
                // Сохранение данных о военной регистрации
               

                MessageBox.Show("Данные о военной регистрации успешно сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очистка временных переменных после успешного сохранения
            }
            else
            {
                MessageBox.Show("Не удалось добавить сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IdButton_Click(object sender, RoutedEventArgs e)
        {
            IdDataWindow passportDataWindow = new IdDataWindow();
            if (passportDataWindow.ShowDialog() == true)
            {
                _tempPassportData = passportDataWindow.GetTempPassportData();

            }
        }

        private void MillitaryRegButton_Click(object sender, RoutedEventArgs e)
        {
            MillitaryRegWindow millitaryRegWindow = new MillitaryRegWindow();
            if (millitaryRegWindow.ShowDialog() == true)
            {
                _tempMillitaryData = millitaryRegWindow.GetTempMillitaryData();
            }
        }
        private int GetNextEmployeeId()
        {
            int nextId = 0;
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "SELECT MAX(Id) FROM Employees";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextId = Convert.ToInt32(result) + 1;
                    }
                }
            }
            return nextId;
        }
    }
}
