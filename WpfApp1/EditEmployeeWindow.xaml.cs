using System.Windows;

namespace WpfApp1
{
    public partial class EditEmployeeWindow : Window
    {
        public Employee CurrentEmployee { get; private set; }

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            CurrentEmployee = employee;
            LastNameTextBox.Text = employee.LastName;
            FirstNameTextBox.Text = employee.FirstName;
            MiddleNameTextBox.Text = employee.MiddleName;
            PositionTextBox.Text = employee.Position;
            PhoneTextBox.Text = employee.Phone;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentEmployee.LastName = LastNameTextBox.Text;
            CurrentEmployee.FirstName = FirstNameTextBox.Text;
            CurrentEmployee.MiddleName = MiddleNameTextBox.Text;
            CurrentEmployee.Position = PositionTextBox.Text;
            CurrentEmployee.Phone = PhoneTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void IdButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MillitaryRegButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
