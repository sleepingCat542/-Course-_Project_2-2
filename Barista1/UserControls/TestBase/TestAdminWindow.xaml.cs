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

namespace Barista1.UserControls.TestBase
{
    /// <summary>
    /// Логика взаимодействия для TestAdminWindow.xaml
    /// </summary>
    public partial class TestAdminWindow : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<QuestionUserControl> questionsControls = new List<QuestionUserControl>();
        List<string> listNames = new List<string>();
        public int i = 0;

        public TestAdminWindow()
        {
            InitializeComponent();
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            questionsControls.Add(new QuestionUserControl());
            questionsControls[i] = new QuestionUserControl();
            questionsControls[i].tbpoint.Visibility = Visibility.Visible;
            questionsControls[i].tb1.IsReadOnly = false;
            questionsControls[i].tb2.IsReadOnly = false;
            questionsControls[i].tb3.IsReadOnly = false;
            questionsControls[i].tb4.IsReadOnly = false;
            questionsControls[i].tbnumber.IsReadOnly = false;
            questionsControls[i].tbpoint.IsReadOnly = false;
            questionsControls[i].tbquestion.IsReadOnly = false;
            questionsControls[i].rb1.GroupName = $"answers{i}";
            questionsControls[i].rb2.GroupName = $"answers{i}";
            questionsControls[i].rb3.GroupName = $"answers{i}";
            questionsControls[i].rb4.GroupName = $"answers{i}";
            AnswerStack.Children.Add(questionsControls[i]);
            i++;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try    //выгрузить из бд
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sqlExpression1 = "SELECT * FROM Tests";
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand sqlCommand1 = new SqlCommand(sqlExpression1, connection);
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
            if (txbTestName.Text != String.Empty && txbTestName.Text.Length < 80 && AnswerStack.Children.Count != 0 && flagTopic==false)
            {
                string sqlExpression = $"INSERT INTO Tests (Name) VALUES (N'{txbTestName.Text}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                        string sqlExpression1 = $"Select TestNumber from Tests where Name = N'{txbTestName.Text}'";
                        command = new SqlCommand(sqlExpression1, connection);
                        int testNumber = Convert.ToInt32(command.ExecuteScalar());
                        int countQuestions = 0;
                        int countAnswers = 0;
                        for (int i = 0; i < AnswerStack.Children.Count; i++)
                        {                            
                            QuestionUserControl question = (QuestionUserControl)AnswerStack.Children[i];
                            for (int j = 0; j < AnswerStack.Children.Count; j++)
                            {
                                QuestionUserControl question2 = (QuestionUserControl)AnswerStack.Children[j];
                                if (question2.tbnumber.Text == question.tbnumber.Text && question!= question2)
                                    throw new Exception("Не допускаются одинаковые номера вопросов!");
                            }
                            int qNumber = Convert.ToInt32(question.tbnumber.Text);
                            string qText = question.tbquestion.Text;
                            int qPoints = Convert.ToInt32(question.tbpoint.Text);
                            if (qNumber != 0 && qText != String.Empty && qPoints!=0)
                            {
                                string sqlExpression2 = $"INSERT INTO Questions (Question, NumberQuestion, Points, TestNumber) VALUES (N'{qText}', {qNumber}, {qPoints}, {testNumber})";
                                command = new SqlCommand(sqlExpression2, connection);
                                command.ExecuteNonQuery();
                                countQuestions++;

                                string sqlExpression4 = $"Select IdQuestion from Questions where NumberQuestion = {qNumber} and TestNumber={testNumber}";
                                command = new SqlCommand(sqlExpression4, connection);
                                int questionId = Convert.ToInt32(command.ExecuteScalar());
                                

                                if (question.tb1.Text !=String.Empty && question.tb2.Text != String.Empty && question.tb3.Text != String.Empty && question.tb4.Text != String.Empty
                                    && ((bool)question.rb1.IsChecked || (bool)question.rb2.IsChecked || (bool)question.rb3.IsChecked || (bool)question.rb4.IsChecked))   
                                        
                                {
                                    SqlTransaction tx = connection.BeginTransaction();  //начали транзакцию
                                    command.Transaction = tx;//Чтобы все операции с объектом SqlCommand выполнялись как одна транзакция, надо присвоить объект транзакции его свойству Transaction
                                    try
                                    {
                                        command.CommandText = $"INSERT INTO Answer (Answer, IsRight, IdQuestion) VALUES (N'{question.tb1.Text}', '{question.rb1.IsChecked.ToString()}', {questionId})";
                                        command.ExecuteNonQuery();
                                        command.CommandText = $"INSERT INTO Answer (Answer, IsRight, IdQuestion) VALUES (N'{question.tb2.Text}', '{question.rb2.IsChecked}', {questionId})";
                                        command.ExecuteNonQuery();
                                        command.CommandText = $"INSERT INTO Answer (Answer, IsRight, IdQuestion) VALUES (N'{question.tb3.Text}', '{question.rb3.IsChecked}', {questionId})";
                                        command.ExecuteNonQuery();
                                        command.CommandText = $"INSERT INTO Answer (Answer, IsRight, IdQuestion) VALUES (N'{question.tb4.Text}', '{question.rb4.IsChecked}', {questionId})";
                                        command.ExecuteNonQuery();

                                        tx.Commit();                                        
                                        countAnswers = 4;
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("транзакция не выполнена" + ex.Message);
                                        countAnswers = 0;
                                    }

                                    }
                                
                                if (countAnswers == 0 || countQuestions == 0)
                                {
                                    string sqlExpression5 = $"Delete from Questions  where TestNumber = {testNumber}";
                                    command = new SqlCommand(sqlExpression5, connection);
                                    command.ExecuteNonQuery();
                                    string sqlExpression3 = $"Delete from Tests  where Name = N'{txbTestName.Text}'";
                                    command=new SqlCommand(sqlExpression3, connection);
                                    command.ExecuteNonQuery();                             
                                    MessageBox.Show("Тест без вопросов или ответов не может быть добавлен!");
                                }
                            }
                        }                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message+ "\nКорректно заполните все поля");
                    }
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Укажите корректное название теста!");
            }
            txbTestName.Text = "";
            AnswerStack.Children.Clear();
        }
    }
}
