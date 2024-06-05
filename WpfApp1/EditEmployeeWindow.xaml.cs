using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace WpfApp1
{
    public partial class EditEmployeeWindow : Window
    {
        public Employee CurrentEmployee { get; private set; }
        private int _employeeId;
        private WpfApp1.TempIDData _tempPassportData;

        public EditEmployeeWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId; // присваиваем значение переменной
            LoadEmployeeFromDatabase(_employeeId);
            LoadPassportDataFromDatabase(_employeeId);
            DataContext = CurrentEmployee;

            // Заполняем поля значениями из CurrentEmployee
            LastNameTextBox.Text = CurrentEmployee.LastName;
            FirstNameTextBox.Text = CurrentEmployee.FirstName;
            MiddleNameTextBox.Text = CurrentEmployee.MiddleName;
            PositionTextBox.Text = CurrentEmployee.Position;
            PhoneTextBox.Text = CurrentEmployee.Phone;
            PersonnelNumberTextBox.Text = CurrentEmployee.NumberTabel;
            MaleComboBox.Text = CurrentEmployee.Male;
            BirthdayDatePicker.Text = CurrentEmployee.DateBrt;
            NationalityTextBox.Text = CurrentEmployee.Nation;
            FamilyStatusTextBox.Text = CurrentEmployee.Family;
            BirthdayPlaceTextBox.Text = CurrentEmployee.Place;
            ProfessionTextBox.Text = CurrentEmployee.Profession;
            SecondProfessionTextBox.Text = CurrentEmployee.SecProfession;
        }

        private void LoadEmployeeFromDatabase(int employeeId)
        {
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Employees WHERE Id = @Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", employeeId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CurrentEmployee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? string.Empty : reader.GetString(reader.GetOrdinal("LastName")),
                                    FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? string.Empty : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? string.Empty : reader.GetString(reader.GetOrdinal("Position")),
                                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone")),
                                    NumberTabel = reader.IsDBNull(reader.GetOrdinal("NumberTabel")) ? string.Empty : reader.GetString(reader.GetOrdinal("NumberTabel")),
                                    Male = reader.IsDBNull(reader.GetOrdinal("Male")) ? string.Empty : reader.GetString(reader.GetOrdinal("Male")),
                                    DateBrt = reader.IsDBNull(reader.GetOrdinal("DateBrt")) ? string.Empty : reader.GetString(reader.GetOrdinal("DateBrt")),
                                    Nation = reader.IsDBNull(reader.GetOrdinal("Nation")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nation")),
                                    Family = reader.IsDBNull(reader.GetOrdinal("Family")) ? string.Empty : reader.GetString(reader.GetOrdinal("Family")),
                                    Place = reader.IsDBNull(reader.GetOrdinal("Place")) ? string.Empty : reader.GetString(reader.GetOrdinal("Place")),
                                    Profession = reader.IsDBNull(reader.GetOrdinal("Profession")) ? string.Empty : reader.GetString(reader.GetOrdinal("Profession")),
                                    SecProfession = reader.IsDBNull(reader.GetOrdinal("SecProfession")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecProfession"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных из базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void LoadPassportDataFromDatabase(int employeeId)
        {
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM PassportData WHERE EmployeeId = @EmployeeId";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _tempPassportData = new TempIDData
                                {
                                    PassportNumber = reader.IsDBNull(reader.GetOrdinal("PassportNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("PassportNumber")),
                                    IssuedBy = reader.IsDBNull(reader.GetOrdinal("IssuedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("IssuedBy")),
                                    IssueDate = reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("IssueDate"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке паспортных данных из базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployee.LastName = LastNameTextBox.Text;
            CurrentEmployee.FirstName = FirstNameTextBox.Text;
            CurrentEmployee.MiddleName = MiddleNameTextBox.Text;
            CurrentEmployee.Position = PositionTextBox.Text;
            CurrentEmployee.Phone = PhoneTextBox.Text;
            CurrentEmployee.NumberTabel = PersonnelNumberTextBox.Text;
            CurrentEmployee.Male = MaleComboBox.Text;
            CurrentEmployee.DateBrt = BirthdayDatePicker.Text;
            CurrentEmployee.Nation = NationalityTextBox.Text;
            CurrentEmployee.Family = FamilyStatusTextBox.Text;
            CurrentEmployee.Place = BirthdayPlaceTextBox.Text;
            CurrentEmployee.Profession = ProfessionTextBox.Text;
            CurrentEmployee.SecProfession = SecondProfessionTextBox.Text;

            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "UPDATE Employees SET LastName=@LastName, FirstName=@FirstName, MiddleName=@MiddleName, Position=@Position, Phone=@Phone, NumberTabel=@NumberTabel, " +
                               "Male=@Male, DateBrt=@DateBrt, Nation=@Nation, Family=@Family, Place=@Place, Profession=@Profession, SecProfession=@SecProfession WHERE Id=@Id";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@LastName", CurrentEmployee.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", CurrentEmployee.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", CurrentEmployee.MiddleName);
                    cmd.Parameters.AddWithValue("@Position", CurrentEmployee.Position);
                    cmd.Parameters.AddWithValue("@Phone", CurrentEmployee.Phone);
                    cmd.Parameters.AddWithValue("@NumberTabel", CurrentEmployee.NumberTabel);
                    cmd.Parameters.AddWithValue("@Male", CurrentEmployee.Male);
                    cmd.Parameters.AddWithValue("@DateBrt", CurrentEmployee.DateBrt);
                    cmd.Parameters.AddWithValue("@Nation", CurrentEmployee.Nation);
                    cmd.Parameters.AddWithValue("@Family", CurrentEmployee.Family);
                    cmd.Parameters.AddWithValue("@Place", CurrentEmployee.Place);
                    cmd.Parameters.AddWithValue("@Profession", CurrentEmployee.Profession);
                    cmd.Parameters.AddWithValue("@SecProfession", CurrentEmployee.SecProfession);
                    cmd.Parameters.AddWithValue("@Id", CurrentEmployee.Id);
                    cmd.ExecuteNonQuery();
                }
            }

            DialogResult = true;
            Close();
        }
        private void IdButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tempPassportData != null)
            {
                EditIdDataWindow passportDataWindow = new EditIdDataWindow(_tempPassportData, _employeeId);
                if (passportDataWindow.ShowDialog() == true)
                {
                    _tempPassportData = passportDataWindow.GetTempPassportData();
                }
            }
        }

        private void MillitaryRegButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработка нажатия кнопки MillitaryReg
        }
        public class EditTempIDData
        {
            public string PassportNumber { get; set; }
            public string IdNumber { get; set; }
            public string IssuedBy { get; set; }
            public DateTime IssueDate { get; set; }
        }
    }
}