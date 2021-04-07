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

        /// <summary>
        /// Событие отслеживает особенныых потребителей , для которых необходимо внести специальные значения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeOfCommercialsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeOfCommercialsBox.SelectedItem != null)
            {
                if (TypeOfCommercialsBox.SelectedItem.ToString() == "Професіонально-технічні навчальні заклади з їдальнями")
                {
                    CosFiTextBox.Visibility = Visibility.Visible;
                    CosFiTextBox.Background = Brushes.Yellow;
                    CosFiText.Visibility = Visibility.Visible;
                    CosFiText.Background = Brushes.Yellow;
                    TgFiTextBox.Visibility = Visibility.Visible;
                    TgFiTextBox.Background = Brushes.Yellow;
                    TgFiText.Visibility = Visibility.Visible;
                    TgFiText.Background = Brushes.Yellow;
                    MessageBox.Show("УТОЧНІТЬ ВРУЧНУ ЗНАЧЕННЯ cosφ ТА tgφ СПОЖИВАЧА, У ПОЛЕ ЩО ПІДСВІЧЕНЕ ЖОВТИМ ЗЛІВА");
                    LoadBox.Visibility = Visibility.Collapsed;
                    LoadText.Visibility = Visibility.Collapsed;
                }
              
                else
                {
                    CosFiTextBox.Visibility = Visibility.Collapsed;
                    CosFiText.Visibility = Visibility.Collapsed;
                    TgFiTextBox.Visibility = Visibility.Collapsed;
                    TgFiText.Visibility = Visibility.Collapsed;

                    if (TypeOfCommercialsBox.SelectedItem.ToString() == "Громадські будівлі багатофункціонального призначення")
                    {

                        LoadBox.Visibility = Visibility.Visible;
                        LoadText.Visibility = Visibility.Visible;
                        LoadText.Background = Brushes.Yellow;
                        LoadBox.Background = Brushes.Yellow;
                        MessageBox.Show("УТОЧНІТЬ ВРУЧНУ ЗНАЧЕННЯ ПИТОМОГО НАВАНТАЖЕННЯ СПОЖИВАЧА У ПОЛЕ ЩО ПІДСВІЧЕНЕ ЖОВТИМ ЗЛІВА");
                    }

                    else
                    {
                        LoadBox.Visibility = Visibility.Collapsed;
                        LoadText.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }

    public class CommercialsDataBase
    {
       

        public static IEnumerable<string> CommercialTypeColl { get; } = new string[] 
        {
            "Підприємства громадського харчування повністю електрифіковані", "Підприємства громадського харчування частково електрифіковані",
            "Підприємства роздрібної торгівлі продовольчі без кондиціонування повітря", "Підприємства роздрібної торгівлі продовольчі з кондиціонуванням повітря",
            "Підприємства роздрібної торгівлі промтоварні без кондиціонування повітря", "Підприємства роздрібної торгівлі промтоварні з кондиціонуванням повітря",
            "Підприємства роздрібної торгівлі універсами без кондиціонування повітря", "Підприємства роздрібної торгівлі універсами з кондиціонуванням повітря",
            "Загальноосвітні школи з електрифікованими їдальнями та спортзалами", "Загальноосвітні школи без електрифікованих їдалень, із спортзалами",
            "Загальноосвітні школи з буфетами, без спортзалів", "Загальноосвітні школи без буфетів і спортзалів", "Професіонально-технічні навчальні заклади з їдальнями",
            "Дошкільні навчальні заклади з електрифікованими харчоблоками", "Дошкільні навчальні заклади з газовими плитами",
            "Школи-інтернати", "Будинки-інтернати для інвалідів та людей похилого віку",
            "Заклади охорони здоров'я лікарні хірургічного профілю з електрифікованими харчоблоками", "Заклади охорони здоров'я хірургічні корпуси (без харчоблоків)",
            "Заклади охорони здоров'я лікарні багатопрофільні з електрифікованими харчоблоками", "Заклади охорони здоров'я терапевтичні корпуси (без харчоблоків)",
            "Заклади охорони здоров'я радіологічні корпуси (без харчоблоків)", "Заклади охорони здоров'я лікарні дитячі з електрифікованими харчоблоками",
            "Заклади охорони здоров'я терапевтичні корпуси дитячих лікарень (без харчоблоків)", "Будинки відпочинку і пансіонати без кондиціонування повітря",
            "Дитячі табори", "Поліклініки",
            "Аптеки без приготування ліків", "Аптеки з приготуванням ліків",  "Кінотеатри та кіноконцертні зали з кондиціонуванням повітря",
            "Кінотеатри та кіноконцертні зали без кондиціонування повітря", "Театри та цирки",
            "Палаци культури, клуби", "Готелі (без ресторанів) з кондиціонуванням повітря",
            "Готелі (без ресторанів) без кондиціонування повітря", "Фабрики хімчистки та пральні самообслуговування",
            "Комплексні підприємства, служби побуту", "Перукарні"
        };



        public static IEnumerable<byte> ElectrificationLevelColl { get; } = new byte[] { 1, 3 };
        public static IEnumerable<byte> ReliabilityCathegoryColl { get; } = new byte[] { 1, 2, 3 };
       

    }

}
