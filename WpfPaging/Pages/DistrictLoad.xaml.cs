using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DistrictSupplySolution.Pages
{
    /// <summary>
    /// Логика взаимодействия для DistrictLoad.xaml
    /// </summary>
    public partial class DistrictLoad : Page
    {
        public DistrictLoad()
        {
           
        InitializeComponent();
        }

     

        private void CalculateLightningButton_Click(object sender, RoutedEventArgs e)
        {
            GoToNextStepButton.IsEnabled = true;
        }
    }

   
    
}
