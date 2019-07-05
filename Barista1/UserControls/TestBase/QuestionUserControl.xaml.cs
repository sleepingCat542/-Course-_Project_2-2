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
    /// Логика взаимодействия для QuestionUserControl.xaml
    /// </summary>
    public partial class QuestionUserControl : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Classes.Answer> listAnswers = new List<Classes.Answer>();
        //SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter1;
        //SqlDataAdapter adapter2;
        public bool isTrue=false;
        //bool isAdmin = false;
        //int j = 0;                  //для разных 
        DataTable dataAnswers;


        public QuestionUserControl()
        {
            InitializeComponent();
        }

        Classes.Question question;
        public QuestionUserControl(Classes.Question question, int j)
        {
            this.question = question;
            InitializeComponent();
            listAnswers.Clear();
            FeelAnswers();
            tbnumber.Text = question.NumberQuestion.ToString();
            tbquestion.Text = question.QuestionText.ToString();
            tb1.Text = listAnswers[0].AnswerText.ToString();
            tb2.Text = listAnswers[1].AnswerText.ToString();
            tb3.Text = listAnswers[2].AnswerText.ToString();
            tb4.Text = listAnswers[3].AnswerText.ToString();
            rb1.GroupName = $"answers{j}";
            rb2.GroupName = $"answers{j}";
            rb3.GroupName = $"answers{j}";
            rb4.GroupName = $"answers{j}";
        }


        public void FeelAnswers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    try
                    {
                        string sqlExpression = $"SELECT * FROM Answer where IdQuestion={question.IdQuestion}";
                        dataAnswers = new DataTable();
                        command = new SqlCommand(sqlExpression, connection);
                        adapter1 = new SqlDataAdapter(command);
                        adapter1.Fill(dataAnswers);

                        SqlCommand sqlCommand1 = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            int j = 0;
                            while (reader1.Read())
                            {
                                listAnswers.Add(new Classes.Answer());
                                listAnswers[j].IdAnswer = reader1.GetValue(0).ToString();
                                listAnswers[j].AnswerText = reader1.GetValue(1).ToString();
                                listAnswers[j].IsRight = reader1.GetValue(2).ToString();
                                listAnswers[j].IdQuestion = reader1.GetValue(3).ToString();
                                j++;
                            }
                        }
                        reader1.Close();
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

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            isTrue = false;
            RadioButton radiob = (RadioButton) sender;
            DockPanel parent = (DockPanel)radiob.Parent;
            TextBox textBox = (TextBox)parent.Children[1];

            for (int i=0; i<listAnswers.Count; i++)
            {
                if (textBox.Text == listAnswers[i].AnswerText.ToString() && listAnswers[i].IsRight.ToString() == "True")
                    isTrue = true;
            }

        }
    }
}
