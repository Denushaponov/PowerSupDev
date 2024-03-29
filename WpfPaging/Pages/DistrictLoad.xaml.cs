﻿using DistrictSupplySolution.MessageWindows;
using DistrictSupplySolution.ViewModels;
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

     

     

        private void HiddenTextBlockWithSideNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HiddenTextBlockWithSideNote.Text == "Особливий")
            {
              ParticipanceInMaxText1.Visibility = Visibility.Visible;
                ParticipanceInMaxText2.Visibility = Visibility.Visible;
                //InputFieldCoefficientOfParticipanceInMax.Visibility = Visibility.Visible;
                CoeficientOfParticipanceInputPanel.Background = Brushes.Coral;
                
                GoToChangeCoefficientsOfMaxForSpecialConsumer.Visibility = Visibility.Visible;
                MessageBox.Show("ОСОБЛИВИЙ СПОЖИВАЧ: необхідно уточнити усі його можливі коефіцієнти участі");
            }    
             
            else if (HiddenTextBlockWithSideNote.Text == "Особливий визначений")
            {
                GoToChangeCoefficientsOfMaxForSpecialConsumer.Visibility = Visibility.Visible;
                MessageBox.Show("ОСОБЛИВИЙ СПОЖИВАЧ: натисніть, щоб редагувати значення");
            }    

            else
            {
                ParticipanceInMaxText1.Visibility = Visibility.Collapsed;
                ParticipanceInMaxText2.Visibility = Visibility.Collapsed;
                
                CoeficientOfParticipanceInputPanel.Background = Brushes.Transparent;
                GoToChangeCoefficientsOfMaxForSpecialConsumer.Visibility = Visibility.Hidden;
            }

        }

        private void GoToNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ParticipanceInMaximumTab.IsSelected = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FinalDistrictLoadTab.IsSelected = true;
        }

       
    }

   
    
}
