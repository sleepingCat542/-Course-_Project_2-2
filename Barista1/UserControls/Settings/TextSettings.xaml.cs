using Barista1.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barista1.UserControls.Settings
{
    /// <summary>
    /// Логика взаимодействия для TextSettings.xaml
    /// </summary>
    public partial class TextSettings : UserControl
    {
        bool isTheme = false;
        Uri uri = null;

        public TextSettings()
        {
            InitializeComponent();      
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton source = e.OriginalSource as RadioButton;
            RadioButton sourceparent = e.Source as RadioButton;

            if  (sourceparent.Name == "Latte")
            {
                uri = new Uri("Themes/LatteT.xaml", UriKind.Relative);
                isTheme = true;                    
            }
            if (sourceparent.Name == "Strawb")
            {
                uri = new Uri("Themes/StrawberryT.xaml", UriKind.Relative);
                isTheme = true;
            }
            if (isTheme)
            {
                ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
        }        
    }
}
