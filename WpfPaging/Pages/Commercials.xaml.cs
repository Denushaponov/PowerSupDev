using System.Windows;
using System.Windows.Controls;

namespace WpfPaging.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogPage.xaml
    /// </summary>
    public partial class Commercials : Page
    {
        public Commercials()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommercialLoadsTab.IsSelected = true;
        }


    }
}
