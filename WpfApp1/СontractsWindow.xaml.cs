using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    public partial class СontractsWindow : System.Windows.Window
    {
        private string connectionString = "Data Source=users.db;Version=3;";
        public СontractsWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            List<EmployeeContract> employeeContracts = new List<EmployeeContract>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Загружаем данные из существующей таблицы сотрудников
                string employeeQuery = "SELECT LastName, FirstName, MiddleName FROM Employees";
                using (SQLiteCommand employeeCommand = new SQLiteCommand(employeeQuery, connection))
                {
                    using (SQLiteDataReader employeeReader = employeeCommand.ExecuteReader())
                    {
                        while (employeeReader.Read())
                        {
                            employeeContracts.Add(new EmployeeContract
                            {
                                LastName = employeeReader.GetString(0),
                                FirstName = employeeReader.GetString(1),
                                MiddleName = employeeReader.GetString(2),
                                ContractEndDate = DateTime.MinValue
                            });
                        }
                    }
                }

                // Обновляем данными из таблицы EmployeeContracts, если они существуют
                string contractQuery = "SELECT LastName, FirstName, MiddleName, ContractEndDate FROM EmployeeContracts";
                using (SQLiteCommand contractCommand = new SQLiteCommand(contractQuery, connection))
                {
                    using (SQLiteDataReader contractReader = contractCommand.ExecuteReader())
                    {
                        while (contractReader.Read())
                        {
                            var employee = employeeContracts.Find(e =>
                                e.LastName == contractReader.GetString(0) &&
                                e.FirstName == contractReader.GetString(1) &&
                                e.MiddleName == contractReader.GetString(2));

                            if (employee != null)
                            {
                                employee.ContractEndDate = DateTime.Parse(contractReader.GetString(3));
                            }
                        }
                    }
                }
            }

            EmployeeDataGrid.ItemsSource = employeeContracts;
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            List<EmployeeContract> employeeContracts = EmployeeDataGrid.ItemsSource as List<EmployeeContract>;

            // Сохранение данных в SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var employee in employeeContracts)
                {
                    string query = "INSERT OR REPLACE INTO EmployeeContracts (LastName, FirstName, MiddleName, ContractEndDate) VALUES (@LastName, @FirstName, @MiddleName, @ContractEndDate)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                        command.Parameters.AddWithValue("@ContractEndDate", employee.ContractEndDate.ToString("yyyy-MM-dd"));
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Экспорт в Excel
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Worksheet worksheet = (Worksheet)workbook.Sheets["Sheet1"];
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Employee Data";

            for (int i = 1; i < EmployeeDataGrid.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = EmployeeDataGrid.Columns[i - 1].Header;
            }

            for (int i = 0; i < employeeContracts.Count; i++)
            {
                worksheet.Cells[i + 2, 1] = employeeContracts[i].LastName;
                worksheet.Cells[i + 2, 2] = employeeContracts[i].FirstName;
                worksheet.Cells[i + 2, 3] = employeeContracts[i].MiddleName;
                worksheet.Cells[i + 2, 4] = employeeContracts[i].ContractEndDate.ToString("yyyy-MM-dd");
            }

            workbook.SaveAs("EmployeeData.xlsx");
            workbook.Close();
            excel.Quit();
        }
    }
    public class EmployeeContract
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime ContractEndDate { get; set; }
    }
}