using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    public partial class MedicalExaminationsWindow : System.Windows.Window
    {
        private string connectionString = "Data Source=users.db;Version=3;";

        public MedicalExaminationsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<EmployeeMedicalExam> employeeMedicalExams = new List<EmployeeMedicalExam>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT LastName, FirstName, MiddleName, ExaminationEndDate FROM EmployeeMedicalExams";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeMedicalExams.Add(new EmployeeMedicalExam
                            {
                                LastName = reader.GetString(0),
                                FirstName = reader.GetString(1),
                                MiddleName = reader.GetString(2),
                                ExaminationEndDate = DateTime.Parse(reader.GetString(3))
                            });
                        }
                    }
                }
            }

            MedicalExaminationsDataGrid.ItemsSource = employeeMedicalExams;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            List<EmployeeMedicalExam> employeeMedicalExams = MedicalExaminationsDataGrid.ItemsSource as List<EmployeeMedicalExam>;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var employee in employeeMedicalExams)
                {
                    string query = "INSERT OR REPLACE INTO EmployeeMedicalExams (LastName, FirstName, MiddleName, ExaminationEndDate) VALUES (@LastName, @FirstName, @MiddleName, @ExaminationEndDate)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                        command.Parameters.AddWithValue("@ExaminationEndDate", employee.ExaminationEndDate.ToString("yyyy-MM-dd"));
                        command.ExecuteNonQuery();
                    }
                }
            }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Worksheet worksheet = null;
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Employee Medical Exams";

            for (int i = 1; i < MedicalExaminationsDataGrid.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = MedicalExaminationsDataGrid.Columns[i - 1].Header;
            }

            for (int i = 0; i < employeeMedicalExams.Count; i++)
            {
                worksheet.Cells[i + 2, 1] = employeeMedicalExams[i].LastName;
                worksheet.Cells[i + 2, 2] = employeeMedicalExams[i].FirstName;
                worksheet.Cells[i + 2, 3] = employeeMedicalExams[i].MiddleName;
                worksheet.Cells[i + 2, 4] = employeeMedicalExams[i].ExaminationEndDate.ToString("yyyy-MM-dd");
            }

            workbook.SaveAs("EmployeeMedicalExams.xlsx");
            workbook.Close();
            excel.Quit();
        }
    }

    public class EmployeeMedicalExam
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime ExaminationEndDate { get; set; }
    }
}
