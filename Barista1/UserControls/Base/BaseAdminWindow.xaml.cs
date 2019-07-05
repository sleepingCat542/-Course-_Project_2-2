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

namespace Barista1.UserControls.Base
{
    /// <summary>
    /// Логика взаимодействия для BaseAdminWindow.xaml
    /// </summary>
    public partial class BaseAdminWindow : UserControl
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        Classes.Topic topicInBD = new Classes.Topic();
        List<string> listTopics = new List<string>();

        public BaseAdminWindow()
        {
            InitializeComponent();
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTitleTopic.Text.Length != 0 && txtTextTopic.Text.Length != 0)
                {
                    bool flagSave = false; //проверка, можно ли сохранить в бд
                    bool flagTopic = false; //проверка есть ли уже такая тема 

                    if (txtTitleTopic.ToString().Length < 70)
                    {
                         flagSave = true;
                    }
                    else
                    {
                        MessageBox.Show("Название темы не должно превышать 80 символов!");
                        return;
                    }
                    if (flagSave)
                    {
                            try    //выгрузить темы в список тем из бд
                            {
                            unitOfWork.Topics.GetList();
                            SqlCommand sqlCommand1 = unitOfWork.Topics.GetList();
                            SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        listTopics.Add(reader1.GetValue(1).ToString());
                                    }
                                }
                                reader1.Close();
                            }
                            
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            
                            //сравнить названия тем с новым названием

                        for (int i = 0; i < listTopics.Count; i++)
                            {
                                if (listTopics[i].ToString() == txtTitleTopic.Text)
                                {
                                    flagTopic = true;
                                    break;
                                }
                            }
                        if (flagTopic == false)
                        {
                            topicInBD.NameTopic = txtTitleTopic.Text;
                            topicInBD.TextTopic = txtTextTopic.Text;
                            txtTitleTopic.Text = "";
                            txtTextTopic.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Такая тема уже есть!");
                            flagSave = false;
                            return;
                        }
                    }
                    
                    //начинаю сохранять в бд
                    if (flagSave)
                    {
                            try
                            {
                                try {

                                    //Добавление темы.
                                    if (!flagTopic) //если такой темы ещё нет в БД
                                    {
                                    unitOfWork.Topics.Add(topicInBD.TextTopic.ToString(), topicInBD.NameTopic.ToString());
                                    }
                                    MessageBox.Show("Добавление темы прошло успешно!");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);                               
                            }
                    }
                }
                else
                {
                    MessageBox.Show("Заполните оба поля!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }     
    }  
}
