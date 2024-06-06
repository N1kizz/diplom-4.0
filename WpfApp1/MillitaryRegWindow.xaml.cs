using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MillitaryRegWindow.xaml
    /// </summary>
    public partial class MillitaryRegWindow : Window
    {
        private TempMillitaryData _tempMillitaryData;

        public MillitaryRegWindow()
        {
            InitializeComponent();
            _tempMillitaryData = new TempMillitaryData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _tempMillitaryData.TempGroupRegistration = GroupRegistrationTextBox.Text;
            _tempMillitaryData.TempCategoryRegistration = CategoryRegistrationTextBox.Text;
            _tempMillitaryData.TempComposition = CompositionTextBox.Text;
            _tempMillitaryData.TempMilitaryRank = MilitaryRankTextBox.Text;
            _tempMillitaryData.TempMilitarySpecialty = MilitarySpecialtyTextBox.Text;
            _tempMillitaryData.TempFitnessForService = FitnessForServiceTextBox.Text;
            _tempMillitaryData.TempMilitaryCommisariat = MilitaryCommisariatTextBox.Text;
            _tempMillitaryData.TempSpecialRegistration = SpecialRegistrationTextBox.Text;


            DialogResult = true;
            Close();

            MessageBox.Show("Данные временно сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class TempMillitaryData
        {
            public string TempGroupRegistration {  get; set; }
            public string TempCategoryRegistration { get; set; }
            public string TempComposition { get; set; }
            public string TempMilitaryRank { get; set; }
            public string TempMilitarySpecialty { get; set; }
            public string TempFitnessForService { get; set; }
            public string TempMilitaryCommisariat { get; set; }
            public string TempSpecialRegistration { get; set; }
        }

        public TempMillitaryData GetTempMillitaryData()
        {
            return _tempMillitaryData;
        }
    }
}
