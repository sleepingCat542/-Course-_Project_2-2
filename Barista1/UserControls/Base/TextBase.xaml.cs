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

namespace Barista1.UserControls.Base
{
    /// <summary>
    /// Логика взаимодействия для TextBase.xaml
    /// </summary>
    public partial class TextBase : UserControl
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        SqlCommand command;
        SqlDataAdapter adapter1;        
        DataTable dataTopic;            
        public List<Classes.Topic> listTopi = new List<Classes.Topic>();
        bool isAdmin=false;


        Classes.Admin admin;
        public TextBase(Classes.Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            isAdmin = true;
        }

        Classes.User user;
        public TextBase(Classes.User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WindowContent.Children.Clear();
            listTopi.Clear();

                    try
                    {
                        dataTopic = new DataTable();
                        command = unitOfWork.Topics.GetList();
                        adapter1 = new SqlDataAdapter(command);
                        adapter1.Fill(dataTopic);

                    SqlDataReader reader1 = command.ExecuteReader();

                    if (reader1.HasRows)
                        {
                            int j = 0;
                            while (reader1.Read())
                            {
                                listTopi.Add(new Classes.Topic());
                                listTopi[j].NameTopic = reader1.GetValue(1).ToString();
                                listTopi[j].TextTopic = reader1.GetValue(2).ToString();
                                j++;
                            }
                        }
                        reader1.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                
                       

            for (int i = 0; i < listTopi.Count; i++)
            {
                Button btn = new Button();
                btn.Style = (Style)btn.FindResource("TopicButton");
                btn.Click += btnContent;
                btn.Uid = i.ToString();
                btn.Content = listTopi[i].NameTopic.ToString();               

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
                dataTopic = new DataTable();
                command = unitOfWork.Topics.GetList();
                adapter1 = new SqlDataAdapter(command);
                adapter1.Fill(dataTopic);

                RichTextBox tb = new RichTextBox();
                tb.IsReadOnly = true;
                tb.Style = (Style)tb.FindResource("RichTextStyle");
                tb.MouseEnter += RichText_MouseEnter;
                int id=Convert.ToInt32(((Button)sender).Uid);
                tb.AppendText(Convert.ToString(dataTopic.Rows[id][2]));

                WindowContent.Children.Clear();
                WindowContent.Children.Add(tb);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter1);
            adapter1.Update(dataTopic);
        }

        private void btnDelete(object sender, RoutedEventArgs e)
        {
            MenuItem send = sender as MenuItem;
            ContextMenu cm = (ContextMenu)send.Parent;
            Button btn = cm.PlacementTarget as Button;
            string name = btn.Content as string;
                try
                {
                    try
                    {
                        unitOfWork.Topics.Delete(name);
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
            UpdateDB();
            UserControl_Loaded(new object(), new RoutedEventArgs());
        }

        private void RichText_MouseEnter(object sender, MouseEventArgs e)
        {
            ((RichTextBox)sender).BorderBrush = Brushes.Transparent;
        }
    }
}
