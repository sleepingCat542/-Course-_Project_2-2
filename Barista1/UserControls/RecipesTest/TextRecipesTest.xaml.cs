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

namespace Barista1.UserControls.RecipesTest
{
    /// <summary>
    /// Логика взаимодействия для TextRecipesTest.xaml
    /// </summary>
    public partial class TextRecipesTest : UserControl
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter1;      
        DataTable dataResults;
        bool isAdmin = false;
        string sqlExpression;
        UnitOfWork unitOfWork = new UnitOfWork();

        Classes.User user;
        public TextRecipesTest(Classes.User user)
        {
            InitializeComponent();
            this.user = user;
        }

        Classes.Admin admin;
        public TextRecipesTest(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (isAdmin)
                {
                    sqlExpression = $"SELECT Login, NameTest, Date, Mark from ProgressRecipes, Users WHERE ProgressRecipes.IdUser = Users.IdUser";
                    userName.Visibility = Visibility.Visible;
                }
                else
                    sqlExpression = $"select NameTest, Date, Mark from ProgressRecipes where IdUser={user.idUser}";
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
                unitOfWork.Close();
            }
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter1);  
            adapter1.Update(dataResults);
        }
    }
}
