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
using System.Windows.Shapes;

namespace DistrictSupplySolution.MessageWindows
{
    /// <summary>
    /// Логика взаимодействия для SaveSpecialCoefficientsOfParticipance.xaml
    /// </summary>
    public partial class SaveSpecialCoefficientsOfParticipance : Window
    {
        public SaveSpecialCoefficientsOfParticipance()
        {
            InitializeComponent();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            MessageBox.Show("Мікрорайон збережено");
            DialogResultText.Text = "Так";
        }

        private void CancelChangesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResultText.Text = "Ні";
            MessageBox.Show("Зміни відхилено");
        }

        public string TextResult
        {
            get { return DialogResultText.Text.ToString(); }
        }

       
    }
}
