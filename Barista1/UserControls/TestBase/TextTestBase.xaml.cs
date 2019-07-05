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
    /// Логика взаимодействия для TextTestBase.xaml
    /// </summary>
    public partial class TextTestBase : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter1;
        SqlDataAdapter adapter2;
        bool isAdmin = false;
        DataTable dataNamesTests;
        DataTable dataQuestions;
        public int n;
        public int point;
        public int usPoint;
        public string nameTest;
        public List<Classes.Test> listTests = new List<Classes.Test>();
        public List<Classes.Question> listQuestions = new List<Classes.Question>();
        public List<QuestionUserControl> questionsControls = new List<QuestionUserControl>();

        Classes.Admin admin;
        public TextTestBase(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }

        Classes.User user;
        public TextTestBase(Classes.User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WindowContent.Children.Clear();
            listTests.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    try
                    {
                        string sqlExpression = "SELECT * FROM Tests";
                        dataNamesTests = new DataTable();
                        command = new SqlCommand(sqlExpression, connection);
                        adapter1 = new SqlDataAdapter(command);
                        adapter1.Fill(dataNamesTests);

                        string sqlExpression1 = "SELECT * FROM Tests";
                        SqlCommand sqlCommand1 = new SqlCommand(sqlExpression1, connection);
                        SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            int j = 0;
                            while (reader1.Read())
                            {
                                listTests.Add(new Classes.Test());
                                listTests[j].TestName = reader1.GetValue(0).ToString();
                                listTests[j].TestNumber = reader1.GetValue(1).ToString();
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
            for (int i = 0; i < listTests.Count; i++)
            {

                Button btn = new Button();
                btn.Style = (Style)btn.FindResource("TopicButton");
                btn.Click += btnContent;
                btn.Uid = i.ToString();
                btn.Content = listTests[i].TestName.ToString();

                if (isAdmin)
                {
                    ContextMenu context = new ContextMenu();
                    MenuItem mdelete = new MenuItem();
                    mdelete.Header = "Удалить";
                    mdelete.Click += btnDelete;
                    context.Items.Add(mdelete);
                    btn.ContextMenu = context;
                }
                WindowContent.Children.Add(btn);
            }
        }

        private void btnContent(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                Button button = sender as Button;
                nameTest = button.Content.ToString();

                string sqlExpression = $"SELECT * FROM Tests where Name=N'{nameTest}'";
                dataNamesTests = new DataTable();
                command = new SqlCommand(sqlExpression, connection);
                adapter1 = new SqlDataAdapter(command);
                adapter1.Fill(dataNamesTests);
                n = Convert.ToInt32(dataNamesTests.Rows[0][1]);

                string sqlExpression2 = $"SELECT * FROM Questions where TestNumber={n}";
                dataQuestions = new DataTable();
                command = new SqlCommand(sqlExpression2, connection);
                adapter2 = new SqlDataAdapter(command);
                adapter2.Fill(dataQuestions);

                SqlDataReader reader1 = command.ExecuteReader();

                if (reader1.HasRows)
                {
                    int j = 0;
                    while (reader1.Read())
                    {
                        listQuestions.Add(new Classes.Question());
                        listQuestions[j].IdQuestion = reader1.GetValue(0).ToString();
                        listQuestions[j].TestNumber = reader1.GetValue(1).ToString();
                        listQuestions[j].NumberQuestion = reader1.GetValue(2).ToString();
                        listQuestions[j].QuestionText = reader1.GetValue(3).ToString();
                        listQuestions[j].Points= reader1.GetValue(4).ToString();
                        j++;
                    }
                }
                reader1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            WindowContent.Children.Clear();
            for (int i = 0; i < listQuestions.Count; i++)
            {
                questionsControls.Add(new QuestionUserControl());
                questionsControls[i] = new QuestionUserControl(listQuestions[i], i);
                WindowContent.Children.Add(questionsControls[i]);
            }
            connection.Close();

            Button btn = new Button();
            btn.Style = (Style)btn.FindResource("ProfButton");
            btn.Margin = new Thickness(15);
            btn.Width = 300;
            btn.Height = 40;
            btn.Click += btnСonfirm;
            btn.Content = "Закончить";
            WindowContent.Children.Add(btn);
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter1);
            adapter1.Update(dataNamesTests);
        }

        private void btnDelete(object sender, RoutedEventArgs e)
        {
            MenuItem send = sender as MenuItem;
            ContextMenu cm = (ContextMenu)send.Parent;
            Button btn = cm.PlacementTarget as Button;
            string name = btn.Content as string;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string sqlExpression = $"Select TestNumber from Tests where Name=N'{name}'";
                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression, connection);
                    int testNumber = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                    string sqlExpression5 = $"Select count(*) from Questions where TestNumber=N'{testNumber}'";
                    sqlCommand1 = new SqlCommand(sqlExpression5, connection);
                    int questionCount = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                    try
                    {
                        for(int j=0; j< questionCount; j++)
                        {                            
                            string sqlExpression1 = $"Select IdQuestion from Questions where TestNumber={testNumber}";
                            sqlCommand1 = new SqlCommand(sqlExpression1, connection);
                            int idQuestion = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                            string sqlExpression2 = $"Delete from Answer where IdQuestion={idQuestion}";
                            sqlCommand1 = new SqlCommand(sqlExpression2, connection);
                            sqlCommand1.ExecuteNonQuery();
                      
                            string sqlExpression3 = $"Delete from Questions where IdQuestion={idQuestion}";
                            sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                            sqlCommand1.ExecuteNonQuery();
                        }
                        SqlCommand command = connection.CreateCommand();
                        string sqlExpression4 = $"Delete from Tests where Name=N'{name}'";
                        command = new SqlCommand(sqlExpression4, connection);
                        command.ExecuteNonQuery();
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
            UpdateDB();
            UserControl_Loaded(new object(), new RoutedEventArgs());

        }

        private void btnСonfirm(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                try
                {
                    for (int i = 0; i < questionsControls.Count; i++)
                    {
                        if (questionsControls[i].isTrue == true)
                        {
                            point += Convert.ToInt32(listQuestions[i].Points);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                StoredProcedure();
            }
            else
                MessageBox.Show("Вы не можете пройти тест поскольку являетесь администратором");
        }

        void StoredProcedure()
        {
            string sqlExpression1 = "sp_PTest";
            try
            {
                connection = new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression1, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter testNumberParam = new SqlParameter
                {
                    ParameterName = "@TestNumber",
                    Value = n
                };
                command.Parameters.Add(testNumberParam);

                SqlParameter userPoints = new SqlParameter
                {
                    ParameterName = "@REALPOINTS",
                    Value = point
                };
                command.Parameters.Add(userPoints);

                SqlParameter pointParam = new SqlParameter
                {
                    ParameterName = "@POINT",
                    SqlDbType = SqlDbType.Int
                };
                pointParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(pointParam);

                command.ExecuteNonQuery();
                usPoint = Convert.ToInt32(command.Parameters["@POINT"].Value);
                MessageBox.Show($"Ваша оценка: {usPoint}!");
                point = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            InsertResult();
        }

        void InsertResult()
        {
            DateTime date1 = DateTime.Now;

            string sqlExpression2 = $"INSERT INTO Progress (IdUser, NameTest, Date, Mark, TestNumber) VALUES ({Convert.ToInt32(this.user.idUser)}, N'{nameTest}', '{date1.ToString("s")}', {usPoint}, {n})";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression2, connection);
                    int number = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            listQuestions.Clear();
            UserControl_Loaded(new object(), new RoutedEventArgs());
        }
    }
}
