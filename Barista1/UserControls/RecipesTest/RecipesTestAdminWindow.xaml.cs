using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Barista1.UserControls.RecipesTest
{
    /// <summary>
    /// Логика взаимодействия для RecipesTestAdminWindow.xaml
    /// </summary>
    public partial class RecipesTestAdminWindow : UserControl
    {
        List<string> listNames = new List<string>();
        UnitOfWork unitOfWork = new UnitOfWork();

        public RecipesTestAdminWindow()
        {
            InitializeComponent();
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            DockPanel dockPanel = new DockPanel();
            dockPanel.Margin = new Thickness(5);

            CheckBox checkBox = new CheckBox();
            checkBox.Background = Brushes.Green;
            checkBox.Foreground = Brushes.White;
            checkBox.Style = (Style)checkBox.FindResource("MaterialDesignActionLightCheckBox");
            checkBox.Margin = new Thickness(10, 0, 10, 0);

            TextBox textBox = new TextBox();
            textBox.Height = 25;
            textBox.Style = (Style)textBox.FindResource("FontMB");
            textBox.BorderThickness = new Thickness(1);
            textBox.VerticalContentAlignment =VerticalAlignment.Center;

            dockPanel.Children.Add(checkBox);
            dockPanel.Children.Add(textBox);

            AnswerStack.Children.Add(dockPanel);          
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try    //выгрузить из бд
            {
                SqlCommand sqlCommand1 = unitOfWork.RecipeTests.GetList();
                SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        listNames.Add(reader1.GetValue(0).ToString());
                    }
                }
                reader1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            bool flagTopic = false;
            for (int i = 0; i < listNames.Count; i++)
            {
                if (listNames[i].ToString() == txbTestName.Text)
                {
                    flagTopic = true;
                    break;
                }
            }

            if (txbTestName.Text != String.Empty && txbTestName.Text.Length < 80 && AnswerStack.Children.Count != 0 && flagTopic == false)
            {
                try
                {
                    unitOfWork.RecipeTests.Add(txbTestName.Text);
                    int testNumber = unitOfWork.RecipeTests.GetNumber(txbTestName.Text);
                    int countAnswers = 0;
                    for (int i = 0; i < AnswerStack.Children.Count; i++)
                    {
                        DockPanel dp = (DockPanel)AnswerStack.Children[i];
                        TextBox tb = (TextBox)dp.Children[1];
                        CheckBox chb = (CheckBox)dp.Children[0];
                        if (tb.Text != String.Empty && tb.Text.Length < 60)
                        {
                            unitOfWork.IngrAnswers.Add(tb.Text, chb.IsChecked,testNumber);
                            countAnswers++;
                        }
                    }
                    if (countAnswers == 0)
                    {

                        unitOfWork.RecipeTests.Delete(txbTestName.Text);
                        MessageBox.Show("Тест без ответов не может быть добавлен!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                unitOfWork.Close();
            }
            else
            {
                MessageBox.Show("Укажите корректное название теста!");
            }           
            txbTestName.Text="";
            AnswerStack.Children.Clear();
        }
    }
}
