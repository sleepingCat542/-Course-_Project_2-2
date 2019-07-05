using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace Barista1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        UnitOfWork unitOfWork = new UnitOfWork();

        bool isAdmin = false;
        
        public Classes.User user;
        public MainWindow(Classes.User user)
        {
            this.user = user;
            InitializeComponent();
            try
            {
                if (user.ImageData.ToString() != String.Empty)
                {
                    image.ImageSource = new BitmapImage(new Uri(user.ImageData.ToString()));
                }
            }
            catch
            {
                MessageBox.Show($"Путь {user.ImageData.ToString()} не найден");
            }

            txbLogin.Text = user.login.ToString();
        }

        Classes.Admin admin;
        public MainWindow(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();

            image.ImageSource = new BitmapImage(new Uri(admin.img.ToString()));
            isAdmin = true;
            txbLogin.Text = admin.login;
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Наставник бариста";

            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.Main.MainImg());

            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.Main.MainSettings());

            WindowAdmin.Children.Clear();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Настройки";

            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.Settings.SettingsImg());

            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.Settings.TextSettings());

            WindowAdmin.Children.Clear();
        }

        private void Base_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Основы профессии";

            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.Base.BaseImg());

            WindowText.Children.Clear();

            if (isAdmin)
            {
                WindowText.Children.Add(new UserControls.Base.TextBase(admin));

                WindowAdmin.Children.Clear();
                Button buttonChange = new Button();

                buttonChange.Style = (Style)buttonChange.FindResource("TopicButton");
                buttonChange.Content = "Добавить тему";
                buttonChange.Click += AddTopic;
                WindowAdmin.Children.Add(buttonChange);
            }
            else
                WindowText.Children.Add(new UserControls.Base.TextBase(user));
        }

        private void Сocktail_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Рецепты коктейлей";

            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.Coctails.CoctailsImg());

            WindowText.Children.Clear();

            if (isAdmin)
            {
                WindowText.Children.Add(new UserControls.Coctails.TextCoctails(admin));

                WindowAdmin.Children.Clear();
                Button buttonChange = new Button();

                buttonChange.Style = (Style)buttonChange.FindResource("TopicButton");
                buttonChange.Content = "Добавить коктейль";
                buttonChange.Click += AddCoctails;
                WindowAdmin.Children.Add(buttonChange);
            }
            else
                WindowText.Children.Add(new UserControls.Coctails.TextCoctails(user));

        }

        string path;
        private void btnUpdateImage_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                try
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Image (*.jpg, *.png)|*.jpg;*.png";

                    if (openFile.ShowDialog() == true)
                    {
                        path = openFile.FileName;

                        image.ImageSource = new BitmapImage(new Uri(openFile.FileName));
                        admin.img = new BitmapImage(new Uri(openFile.FileName));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Image (*.jpg, *.png)|*.jpg;*.png";

                    if (openFile.ShowDialog() == true)
                    {
                        path = openFile.FileName;

                        image.ImageSource = new BitmapImage(new Uri(openFile.FileName));
                            try
                            {
                            unitOfWork.Users.UpdateImage(openFile.FileName, user.idUser);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            SignIn sign = new SignIn();
            sign.Show();
            Close();
        }

        private void TestRecipes_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Тесты на знание рецептов";

            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.RecipesTest.RecipesTestImg());

            WindowText.Children.Clear();

            if (isAdmin)
            {
                WindowText.Children.Clear();
                WindowText.Children.Add(new UserControls.RecipesTest.RecipesTestAdminWindow());

                WindowAdmin.Children.Clear();
                Button buttonChange = new Button();
                buttonChange.Style = (Style)buttonChange.FindResource("TopicButton");
                buttonChange.Content = "Просмотреть тесты";
                buttonChange.Click +=ChangeRecipeTest;
                WindowAdmin.Children.Add(buttonChange);
            }
            else
                WindowText.Children.Add(new UserControls.RecipesTest.TestRecipesNames(user));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Тематические тесты";


            WindowImg.Children.Clear();
            WindowImg.Children.Add(new UserControls.TestBase.TestBaseImg());

            WindowText.Children.Clear();

            if (isAdmin)
            {
                WindowText.Children.Clear();
                WindowText.Children.Add(new UserControls.TestBase.TestAdminWindow());

                WindowAdmin.Children.Clear();
                Button buttonChange = new Button();
                buttonChange.Style = (Style)buttonChange.FindResource("TopicButton");
                buttonChange.Content = "Просмотреть тесты";
                buttonChange.Click += ChangeTest;
                WindowAdmin.Children.Add(buttonChange);
            }
            else
                WindowText.Children.Add(new UserControls.TestBase.TextTestBase(user));

        }

        public void AddTopic(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.Base.BaseAdminWindow());
        }

        public void AddCoctails(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.Coctails.CoctailsAdminWindow());
        }

        private void BtnGoToRecResult_Click(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            if (isAdmin)
                WindowText.Children.Add(new UserControls.RecipesTest.TextRecipesTest(admin));
            else
                WindowText.Children.Add(new UserControls.RecipesTest.TextRecipesTest(user));
        }

        private void Video_Click(object sender, RoutedEventArgs e)
        {
            TitleWin.Text = "Видео-уроки";
            WindowImg.Children.Clear();

            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.TeachingVideo());

            WindowAdmin.Children.Clear();
        }

        private void BtnGoToResult_Click(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            if (isAdmin)
                WindowText.Children.Add(new UserControls.TestBase.TestBaseReport(admin));
            else
                WindowText.Children.Add(new UserControls.TestBase.TestBaseReport(user));
        }

        public void ChangeTest(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.TestBase.TextTestBase(admin));
        }

        public void ChangeRecipeTest(object sender, RoutedEventArgs e)
        {
            WindowText.Children.Clear();
            WindowText.Children.Add(new UserControls.RecipesTest.TestRecipesNames(admin));
        }
    }
    }

