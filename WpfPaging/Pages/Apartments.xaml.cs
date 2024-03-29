﻿using DevExpress.Mvvm;
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
using WpfPaging.ViewModels;

namespace WpfPaging.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Apartments : Page
    {
        public Apartments()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApartmentBuildingsLoadsTab.IsSelected = true;
        }

        private void GoToApartmentsAsOne_Click(object sender, RoutedEventArgs e)
        {
            UnitedApartmentBuildingsTab.IsSelected = true;
        }
      
    }

    public class ApartmentDataBase
    {
        public static IEnumerable<double> LevelsColl { get; } = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 17, 24 };
        public static IEnumerable<double> EntrancesColl { get; } = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public static IEnumerable<double> ApartmentsOnSiteColl { get; } = new double[] { 2, 3, 4, 5, 6, 7, 8 };
        public static IEnumerable<double> ElectrificationLevelColl { get; } = new double[] { 1, 3 };
        public static IEnumerable<double> ReliabilityCathegoryColl { get; } = new double[] { 1, 2, 3 };
        public static IEnumerable<double> ElevatorsPowerColl { get; } = new double[] {0, 6, 7, 8, 9, 10, 11, 12 };
        public static IEnumerable<double> PompsPowerColl { get; } = new double[] {0, 8, 9, 10, 11, 12, 13, 14, 15 };

       
    }

 


}
