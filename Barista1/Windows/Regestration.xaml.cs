using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Regestration.xaml
    /// </summary>
    public partial class Regestration : Window
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public Regestration()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            SignIn signin = new SignIn();
            signin.Show();
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
                try
                {
                    if (txbLogin.Text!= String.Empty && txbPassword1.Password != String.Empty)
                    {
                        Regex regex = new Regex(@"^[a-zA-Zа-яА-Я0-9]{3,15}$");
                        Match match = regex.Match(txbPassword1.Password.ToString());
                        if (!match.Success)
                        {
                            MessageBox.Show("Пароль не соответствует условиям ввода");
                            return;
                        }
                        if (txbPassword1.Password != txbPassword2.Password)
                        {
                            MessageBox.Show("Пароли не совпадают!");
                            return;
                        }
                        if (txbEmail.Text != String.Empty)
                        {
                            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                 @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                            Regex regexEmail;
                            regexEmail = new Regex(pattern, RegexOptions.IgnoreCase);
                            Match match2 = regexEmail.Match(txbEmail.Text);
                            if (!match2.Success && txbEmail.Text.Length>50)
                            {
                                MessageBox.Show("Введите корректный E-mail");
                                return;
                            }
                        }
                        if (txbLogin.Text.Length > 40)
                        {
                            MessageBox.Show("Длина логина не должна быть более 40 символов!");
                            return;
                        }

                        SqlCommand command=unitOfWork.Users.GetList();
                        SqlDataReader reader = command.ExecuteReader();

                        Classes.User tempUser = new Classes.User();
                        bool flagPerson = false;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (txbLogin.Text == (string)reader.GetValue(1))
                                {
                                    flagPerson = true;
                                    tempUser.idUser = reader.GetValue(0);
                                    tempUser.login = reader.GetValue(1);
                                    tempUser.password = reader.GetValue(2);
                                    break;
                                }
                            }
                        }
                        reader.Close();
                   


                        if (!flagPerson)
                        {
                            unitOfWork.Users.Add(txbLogin.Text, txbPassword1.Password, txbEmail.Text);
                            MessageBox.Show("Регистрация прошла успешно!");

                            SignIn signIn = new SignIn();
                            signIn.Show();
                            Close();

                            txbLogin.Text = "";
                            txbPassword1.Password = "";
                            txbPassword2.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("Такой пользователь уже существует!");

                            txbLogin.Text = "";
                            txbPassword1.Password = "";
                            txbPassword2.Password = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!");
                        txbLogin.Text = "";
                        txbPassword1.Password = "";
                        txbPassword2.Password = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}
