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

namespace WpfApp1
{
    public partial class AddEmployeeWindow : Window
    {
        public Employee NewEmployee { get; private set; }
        private ObservableCollection<Employee> _employees;

        public AddEmployeeWindow(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employees = employees;
        }

        SaveFileDialog fd = new SaveFileDialog();

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

                string Surname = LastNameTextBox.Text;
                string Name = FirstNameTextBox.Text;
                string MiddleName = MiddleNameTextBox.Text;
                string Birthday = BirthdayDatePicker.Text;
                string BP = BirthdayPlaceTextBox.Text;
                string BP1 = BP;
                string BP2 = NationalityTextBox.Text;
                string BP3 = "BP.Substring(50, BP.Length - 50)";
                fd.Filter = "Text files(*.docx)|*.docx|All files(*.*)|*.*";
                fd.ShowDialog();
                var templateDoc = File.ReadAllBytes("T2.docx");
                var generatedDoc = SearchAndReplace(templateDoc, new Dictionary<string, string>(){
                    {"<Surname>", Surname},
                    {"<Name>", Name},
                    {"<Middlename>", MiddleName},
                    {"<Birthday>", Birthday},
                    {"<BP>", BP},
                    {"<MillitaryName>", BP},
                    {"<BP3>", BP2},
                });
                File.WriteAllBytes(fd.FileName, generatedDoc);

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
        }

        private void IdButton_Click(object sender, RoutedEventArgs e)
        {
            IdDataWindow window = new IdDataWindow();
            window.ShowDialog();
        }

        private void MillitaryRegButton_Click(object sender, RoutedEventArgs e)
        {
            MillitaryRegWindow window = new MillitaryRegWindow();
            window.ShowDialog();
        }

        protected byte[] SearchAndReplace(byte[] file, IDictionary<string, string> translations)
        {
            WmlDocument doc = new WmlDocument(file.Length.ToString(), file);

            foreach (var translation in translations)
                doc = doc.SearchAndReplace(translation.Key, translation.Value, true);

            return doc.DocumentByteArray;
        }
    }
}
