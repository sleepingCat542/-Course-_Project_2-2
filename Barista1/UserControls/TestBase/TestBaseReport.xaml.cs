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
    /// Логика взаимодействия для TestBaseReport.xaml
    /// </summary>
    public partial class TestBaseReport : UserControl
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter1;
        DataTable dataResults;
        bool isAdmin = false;
        string sqlExpression;

        Classes.User user;
        public TestBaseReport(Classes.User user)
        {
            InitializeComponent();
            this.user = user;
        }

        Classes.Admin admin;
        public TestBaseReport(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                if (isAdmin)
                {
                    sqlExpression = $"SELECT Login, NameTest, Date, Mark from Progress, Users WHERE Progress.IdUser = Users.IdUser";
                    userName.Visibility = Visibility.Visible;
                }
                else
                    sqlExpression = $"select NameTest, Date, Mark from Progress where IdUser={user.idUser}";
                dataResults = new DataTable();
                command = new SqlCommand(sqlExpression, connection);
                adapter1 = new SqlDataAdapter(command);
                adapter1.Fill(dataResults);
                grid.ItemsSource = dataResults.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter1);
            adapter1.Update(dataResults);
        }
    }
}

