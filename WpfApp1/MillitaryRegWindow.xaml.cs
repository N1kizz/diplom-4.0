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
        private string _tempGroupRegistration;
        private string _tempCategoryRegistration;
        private string _tempComposition;
        private string _tempMilitaryRank;
        private string _tempMilitarySpecialty;
        private string _tempFitnessForService;
        private string _tempMilitaryCommisariat;
        private string _tempSpecialRegistration;

        public MillitaryRegWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _tempGroupRegistration = GroupRegistrationTextBox.Text;
            _tempCategoryRegistration = CategoryRegistrationTextBox.Text;
            _tempComposition = CompositionTextBox.Text;
            _tempMilitaryRank = MilitaryRankTextBox.Text;
            _tempMilitarySpecialty = MilitarySpecialtyTextBox.Text;
            _tempFitnessForService = FitnessForServiceTextBox.Text;
            _tempMilitaryCommisariat = MilitaryCommisariatTextBox.Text;
            _tempSpecialRegistration = SpecialRegistrationTextBox.Text;

            MessageBox.Show("Данные временно сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
