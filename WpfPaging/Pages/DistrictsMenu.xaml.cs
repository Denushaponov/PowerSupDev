using DevExpress.Mvvm;
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

namespace WpfPaging.Pages
{
    /// <summary>
    /// Логика взаимодействия для DisrictMenu.xaml
    /// </summary>
    public partial class DisrictsMenu : Page 
    {
        public DisrictsMenu()
        {
            
            InitializeComponent();
        }

       private void Button_Click(object sender, RoutedEventArgs e)
       {
           if (MenuPanel.Visibility == Visibility.Hidden&&TextBoxTitleEdit.Text!=""&&TextBoxTitleEdit.Text!="Введіть назву")
           {
               MenuPanel.Visibility = Visibility.Visible;
           }
           else
           MenuPanel.Visibility = Visibility.Hidden;
      
       }

        private void DistrictsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DistrictsList.Items.Count > 0&& DistrictsList.SelectedItem!=null)
            {
                MenuPanel.Visibility = Visibility.Visible;
            }
            else if (DistrictsList.SelectedItem==null || DistrictsList.Items.Count<1)
                MenuPanel.Visibility = Visibility.Hidden;
        }

        private void DetectChangesInList(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Hidden;
           
            if (DistrictsList.SelectedItem == null || DistrictsList.Items.Count < 1)
                MenuPanel.Visibility = Visibility.Hidden;
        }
    }
}
