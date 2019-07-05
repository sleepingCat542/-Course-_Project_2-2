using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Barista1
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public SignIn()
        {
            InitializeComponent();
        }

        private void btnClick_Submit(object sender, RoutedEventArgs e)
        {
            Classes.Admin admin = new Classes.Admin();
            if (txbLogin.Text == admin.login && txbPassword.Password == admin.password)
            {
                admin.img = new BitmapImage(new Uri("pack://application:,,,/img/total/boss.jpg")); 
                MainWindow mainWindow = new MainWindow(admin);
                mainWindow.Show();
                Close();
                return;
            }

            try
            {
                    if (txbLogin.Text != String.Empty && txbPassword.Password!=String.Empty)
                    {
                        SqlCommand command=unitOfWork.Users.GetList();
                        SqlDataReader reader = command.ExecuteReader();

                        Classes.User tempUser = new Classes.User();
                        if (reader.HasRows)
                        {
                            bool flagPerson = false;
                            while (reader.Read())
                            {
                                if (txbLogin.Text == (string)reader.GetValue(1) && txbPassword.Password == (string)reader.GetValue(2))
                                {
                                    flagPerson = true;
                                    tempUser.idUser = reader.GetValue(0);
                                    tempUser.login = reader.GetValue(1);
                                    tempUser.password = reader.GetValue(2);
                                    tempUser.eMail = reader.GetValue(3);
                                    tempUser.ImageData = reader.GetValue(4);
                                    break;
                                }
                            }
                            reader.Close();
                        

                            if (flagPerson)
                            {
                                MainWindow mainWindow = new MainWindow(tempUser);
                                mainWindow.Show();
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль!");
                                txbLogin.Text = "";
                                txbPassword.Password = "";
                            }
                        }
                        else
                           MessageBox.Show("База данных пуста!");
                    }
                    else
                    {
                        MessageBox.Show("Заполните оба поля!");
                        txbLogin.Text = "";
                        txbPassword.Password = "";
                    }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClick_Registration(object sender, RoutedEventArgs e)
        {
            Regestration registraition = new Regestration();
            registraition.Show();
            Close();
        }

        private void btnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
