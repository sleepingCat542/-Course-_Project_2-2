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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barista1.UserControls.RecipesTest
{

    /// <summary>
    /// Логика взаимодействия для TestRecipesNames.xaml
    /// </summary>
    public partial class TestRecipesNames : UserControl
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter1;        
        SqlDataAdapter adapter2;
        DataTable dataNamesRecipesTests;
        DataTable dataRecipesTestsAnswers;
        public List<Classes.RecipesTest> listRTests = new List<Classes.RecipesTest>();
        public List<Classes.IngrAnswer> listIngrAnswers = new List<Classes.IngrAnswer>();
        UnitOfWork unitOfWork = new UnitOfWork();

        public int n;
        public int point;
        public string nameTest;
        public int rightAnswer = 0;
        public int wrongAnswer = 0;
        bool isAdmin = false;

        Classes.Admin admin;
        public TestRecipesNames(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }

        Classes.User user;
        public TestRecipesNames(Classes.User user)
        {
            this.user = user;
            InitializeComponent();           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowContent.Children.Clear();
            listRTests.Clear();
                    try
                    {
                        dataNamesRecipesTests = new DataTable();
                        command = unitOfWork.RecipeTests.GetList();
                        adapter1 = new SqlDataAdapter(command);
                        adapter1.Fill(dataNamesRecipesTests);

                        SqlCommand sqlCommand1 = unitOfWork.RecipeTests.GetList();
                        SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            int j = 0;
                            while (reader1.Read())
                            {
                                listRTests.Add(new Classes.RecipesTest());
                                listRTests[j].TestName = reader1.GetValue(0).ToString();
                                listRTests[j].TestNumber = reader1.GetValue(1).ToString();
                                j++;
                            }
                        }
                        reader1.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                

            
            for (int i = 0; i < listRTests.Count; i++)
            {

                Button btn = new Button();
                btn.Style = (Style)btn.FindResource("TopicButton");
                btn.Click += btnContent;
                btn.Uid = i.ToString();
                btn.Content = listRTests[i].TestName.ToString();

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
                Button button = sender as Button;
                nameTest = button.Content.ToString();

                dataNamesRecipesTests = new DataTable();
                command = unitOfWork.IngrAnswers.GetListName(nameTest);
                adapter1 = new SqlDataAdapter(command);
                adapter1.Fill(dataNamesRecipesTests);
                n = Convert.ToInt32(dataNamesRecipesTests.Rows[0][1]);

                unitOfWork.IngrAnswers.GetListNumber(n);
                string sqlExpression2 = $"SELECT * FROM RecipeAnswer where TestNumber={n}";
                dataRecipesTestsAnswers = new DataTable();
                command = unitOfWork.IngrAnswers.GetListNumber(n);
                adapter2 = new SqlDataAdapter(command);
                adapter2.Fill(dataRecipesTestsAnswers);

                SqlDataReader reader1 = command.ExecuteReader();

                if (reader1.HasRows)
                {
                    int j = 0;
                    while (reader1.Read())
                    {
                        listIngrAnswers.Add(new Classes.IngrAnswer());
                        listIngrAnswers[j].Answer = reader1.GetValue(1).ToString();
                        listIngrAnswers[j].IsRight = "nothing";
                        listIngrAnswers[j].TestNumber = reader1.GetValue(3).ToString();
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
            for (int i = 0; i < listIngrAnswers.Count; i++)
            {
                DockPanel dockPanel = new DockPanel();
                dockPanel.Margin = new Thickness(5);

                CheckBox checkBox = new CheckBox();
                checkBox.Background = Brushes.Green;
                checkBox.Foreground = Brushes.White;
                checkBox.Style = (Style)checkBox.FindResource("MaterialDesignActionLightCheckBox");                
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.Checked += IngrChecked;
                checkBox.Unchecked += IngrUnchecked;
                checkBox.Uid = Convert.ToString(dataRecipesTestsAnswers.Rows[i][1]);

                TextBlock textBlock = new TextBlock();
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                textBlock.Style = (Style)textBlock.FindResource("FontM");
                textBlock.Text = Convert.ToString(dataRecipesTestsAnswers.Rows[i][1]);

                dockPanel.Children.Add(checkBox);
                dockPanel.Children.Add(textBlock);

                WindowContent.Children.Add(dockPanel);
            }
            unitOfWork.Close();

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
            adapter1.Update(dataNamesRecipesTests);
        }

        private void btnDelete(object sender, RoutedEventArgs e)
        {
            MenuItem send = sender as MenuItem;
            ContextMenu cm = (ContextMenu)send.Parent;
            Button btn = cm.PlacementTarget as Button;
            string name = btn.Content as string;
            if (connection.State == ConnectionState.Closed)
            connection.Open();
                    try
                    {
                        string sqlExpression = $"Select TestNumber from TestRecipes where TestName=N'{name}'";
                        SqlCommand sqlCommand1 = new SqlCommand(sqlExpression, connection);
                        int testNumber = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                        string sqlExpression2 = $"Delete from RecipeAnswer where TestNumber=N'{testNumber}'";
                        sqlCommand1 = new SqlCommand(sqlExpression2, connection);
                        sqlCommand1.ExecuteNonQuery();
                        string sqlExpression1 = $"Delete from TestRecipes where TestName=N'{name}'";
                        sqlCommand1 = new SqlCommand(sqlExpression1, connection);
                        sqlCommand1.ExecuteNonQuery();                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                
            
            UpdateDB();
            Window_Loaded(new object(), new RoutedEventArgs());

        }

        private void IngrChecked(object sender, RoutedEventArgs e)
        {
            CheckBox send = sender as CheckBox;
            for (int j = 0; j < listIngrAnswers.Count; j++)
            {
                if (listIngrAnswers[j].Answer.ToString() == send.Uid)
                    listIngrAnswers[j].IsRight = true;
            }
        }

        private void IngrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox send = sender as CheckBox;
            for (int j = 0; j < listIngrAnswers.Count; j++)
            {
                if (listIngrAnswers[j].Answer.ToString() == send.Uid)
                    listIngrAnswers[j].IsRight = false;
            }
        }

        private void btnСonfirm(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                try
                {
                    SqlCommand sqlCommand1 = unitOfWork.IngrAnswers.GetListNumber(n);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        int j = 0;
                        while (reader1.Read())
                        {
                            if (listIngrAnswers[j].IsRight.ToString() != "nothing")
                            {
                                if (listIngrAnswers[j].IsRight.ToString() == reader1.GetValue(2).ToString() && listIngrAnswers[j].IsRight.ToString() == "True")
                                {
                                    rightAnswer++;
                                }
                                else
                                    wrongAnswer++;
                            }
                            j++;
                        }
                    }
                    reader1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                unitOfWork.Close();
                StoredProcedure();
            }
            else
                MessageBox.Show("Вы не можете пройти тест поскольку являетесь администратором!");
        }

        void StoredProcedure()
        {
            string sqlExpression1 = "sp_PRecTest";
            try
            {
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

                SqlParameter rightAnswerParam = new SqlParameter
                {
                    ParameterName = "@RIGHT",
                    Value = rightAnswer
                };
                command.Parameters.Add(rightAnswerParam);

                SqlParameter wrongAnswerParam = new SqlParameter
                {
                    ParameterName = "@FALSE",
                    Value = wrongAnswer
                };
                command.Parameters.Add(wrongAnswerParam);

                SqlParameter pointParam = new SqlParameter
                {
                    ParameterName = "@POINT",
                    SqlDbType = SqlDbType.Int
                };
                pointParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(pointParam);

                command.ExecuteNonQuery();
                point = Convert.ToInt32(command.Parameters["@POINT"].Value);
                MessageBox.Show($"Ваша оценка: {point}!");
                rightAnswer = 0;
                wrongAnswer = 0;
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

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string sqlExpression = $"INSERT INTO ProgressRecipes (IdUser, NameTest, Date, Mark, TestNumber) VALUES ({Convert.ToInt32(this.user.idUser)}, N'{nameTest}', '{date1.ToString("s")}', {point}, {n})";
                try
                {
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            listIngrAnswers.Clear();
            Window_Loaded(new object(), new RoutedEventArgs());
        }
    }
}


