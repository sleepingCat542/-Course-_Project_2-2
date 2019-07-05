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

namespace Barista1.UserControls.Coctails
{
    /// <summary>
    /// Логика взаимодействия для TextCoctails.xaml
    /// </summary>
    public partial class TextCoctails : UserControl
    {   
        SqlCommand command;
        SqlDataAdapter adapter1;        
        DataTable dataCoctails;            
        public List<Classes.Coctail> listCoctails = new List<Classes.Coctail>();
        UnitOfWork unitOfWork = new UnitOfWork();

        bool isAdmin = false;
        Classes.Admin admin;
        public TextCoctails(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }

        Classes.User user;
        public TextCoctails(Classes.User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowContent.Children.Clear();
            listCoctails.Clear();
                    try
                    {
                        command=unitOfWork.Coctails.GetList();
                        dataCoctails = new DataTable();
                        adapter1 = new SqlDataAdapter(command);
                        adapter1.Fill(dataCoctails);

                        SqlDataReader reader1 = command.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            int j = 0;
                            while (reader1.Read())
                            {
                                listCoctails.Add(new Classes.Coctail());
                                listCoctails[j].NameForRecipes = reader1.GetValue(1).ToString();
                                listCoctails[j].RightIngredients = reader1.GetValue(2).ToString();
                                listCoctails[j].TextRecipe = reader1.GetValue(3).ToString();
                                j++;
                            }
                        }
                        reader1.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                
            
            for (int i = 0; i < listCoctails.Count; i++)
            {
                Button btn = new Button();
                btn.Style = (Style)btn.FindResource("TopicButton");
                btn.Click += btnContent;
                btn.Uid = i.ToString();
                btn.Content = listCoctails[i].NameForRecipes.ToString();

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
                dataCoctails = new DataTable();
                command = unitOfWork.Coctails.GetList();
                adapter1 = new SqlDataAdapter(command);
                adapter1.Fill(dataCoctails);

                RichTextBox ingr = new RichTextBox();
                ingr.IsReadOnly = true;
                ingr.Style = (Style)ingr.FindResource("RichTextIngrStyle");
                int id = Convert.ToInt32(((Button)sender).Uid);
                ingr.AppendText(Convert.ToString(dataCoctails.Rows[id][2]));

                RichTextBox tb = new RichTextBox();
                tb.IsReadOnly = true;
                tb.Style = (Style)tb.FindResource("RichTextStyle");
                tb.AppendText(Convert.ToString(dataCoctails.Rows[id][3]));

                WindowContent.Children.Clear();
                WindowContent.Children.Add(ingr);
                WindowContent.Children.Add(tb);
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
            adapter1.Update(dataCoctails);
        }

        private void btnDelete(object sender, RoutedEventArgs e)
        {
            MenuItem send = sender as MenuItem;
            ContextMenu cm = (ContextMenu)send.Parent;
            Button btn= cm.PlacementTarget as Button;
            string name = btn.Content as string;
            try
            {
                unitOfWork.Coctails.Delete(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateDB();
            Window_Loaded(new object(), new RoutedEventArgs());
        }
    }
}

