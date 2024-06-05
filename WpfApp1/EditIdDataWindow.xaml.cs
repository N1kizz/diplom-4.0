using System;
using System.Data.SQLite;
using System.Windows;

namespace WpfApp1
{
    public partial class EditIdDataWindow : Window
    {
        private TempIDData _tempPassportData;
        private int employeeId;

        public EditIdDataWindow(TempIDData tempPassportData, int employeeId)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            _tempPassportData = tempPassportData;
            UpdateFields(_tempPassportData.PassportNumber, _tempPassportData.IdNumber, _tempPassportData.IssuedBy, _tempPassportData.IssueDate);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновление временных паспортных данных из полей формы
            _tempPassportData.PassportNumber = idTextBox.Text;
            _tempPassportData.IdNumber = idNumberTextBox.Text;
            _tempPassportData.IssuedBy = issuedByTextBox.Text;
            _tempPassportData.IssueDate = issueDatePicker.SelectedDate ?? DateTime.MinValue;

            // Сохранение в базу данных
            using (var connection = new SQLiteConnection("Data Source=users.db"))
            {
                connection.Open();
                string query = "UPDATE PassportData SET PassportNumber=@PassportNumber, IdNumber=@IdNumber, IssuedBy=@IssuedBy, IssueDate=@IssueDate WHERE EmployeeId=@EmployeeId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PassportNumber", _tempPassportData.PassportNumber);
                    cmd.Parameters.AddWithValue("@IdNumber", _tempPassportData.IdNumber);
                    cmd.Parameters.AddWithValue("@IssuedBy", _tempPassportData.IssuedBy);
                    cmd.Parameters.AddWithValue("@IssueDate", _tempPassportData.IssueDate);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Закрытие окна
            DialogResult = true;
            Close();
        }
        private void UpdateFields(string passportNumber, string idNumber, string issuedBy, DateTime issueDate)
        {
            idTextBox.Text = passportNumber;
            idNumberTextBox.Text = idNumber;
            issuedByTextBox.Text = issuedBy;
            issueDatePicker.SelectedDate = issueDate;
        }

        public TempIDData GetTempPassportData()
        {
            return _tempPassportData;
        }
    }
}