using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для IdDataWindow.xaml
    /// </summary>
    public partial class IdDataWindow : Window
    {
        public string PassportNumber { get; set; }
        public string IdNumber { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }
        private TempIDData _tempPassportData;

        public IdDataWindow()
        {
            InitializeComponent();
            _tempPassportData = new TempIDData();
        }
        public void UpdateFields(string passportNumber, string idNumber, string issuedBy, DateTime issueDate)
        {
            idTextBox.Text = passportNumber;
            idNumberTextBox.Text = idNumber;
            issuedByTextBox.Text = issuedBy;
            issueDatePicker.SelectedDate = issueDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _tempPassportData.PassportNumber = idNumberTextBox.Text;
            _tempPassportData.IssuedBy = issuedByTextBox.Text;
            _tempPassportData.IssueDate = issueDatePicker.SelectedDate ?? DateTime.MinValue;

            // Здесь не сохраняем в базу данных, а просто закрываем окно
            DialogResult = true;
            Close();
        }

        public TempIDData GetTempPassportData()
        {
            return _tempPassportData;
        }
    }
    public class TempIDData
    {
        public string PassportNumber { get; set; }
        public string IdNumber { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
