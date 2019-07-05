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

namespace Barista1.UserControls.Coctails
{
    /// <summary>
    /// Логика взаимодействия для CoctailsAdminWindow.xaml
    /// </summary>
    public partial class CoctailsAdminWindow : UserControl
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        Classes.Coctail coctailInBD = new Classes.Coctail();
        List<string> listCoctails = new List<string>();
        UnitOfWork unitOfWork = new UnitOfWork();

        public CoctailsAdminWindow()
        {
            InitializeComponent();
        }

        Classes.Coctail coctail;
        public CoctailsAdminWindow(Classes.Coctail coctail)
        {
            this.coctail = coctail;
            InitializeComponent();
            txtTitleCoctail.Text = this.coctail.NameForRecipes.ToString();
            txtTextCoctail.Text = this.coctail.TextRecipe.ToString();
            txtIngrCoctail.Text = this.coctail.RightIngredients.ToString();
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTitleCoctail.Text.Length != 0 && txtTextCoctail.Text.Length !=0 && txtIngrCoctail.Text.Length != 0)
                {
                    bool flagSave = false; //проверка, можно ли сохранить в бд
                    bool flagTopic = false; //проверка есть ли уже такой коктейль

                    if (txtTitleCoctail.ToString().Length < 50)
                    {
                        flagSave = true;
                    }
                    else
                    {
                        MessageBox.Show("Название коктейля не должно превышать 50 символов!");
                        return;
                    }
                    if (flagSave)
                    {
                            try    //выгрузить темы в список тем из бд
                            {
                            

                            SqlCommand sqlCommand1 = unitOfWork.Coctails.GetList();
                            SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        listCoctails.Add(reader1.GetValue(1).ToString());
                                    }
                                }
                                reader1.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            //сравнить названия тем с новым названием

                            for (int i = 0; i < listCoctails.Count; i++)
                            {
                                if (listCoctails[i].ToString() == txtTitleCoctail.Text)
                                {
                                    flagTopic = true;
                                    break;
                                }
                            }
                        
                        if (flagTopic == false)
                        {
                            coctailInBD.NameForRecipes = txtTitleCoctail.Text;
                            coctailInBD.RightIngredients = txtIngrCoctail.Text;
                            coctailInBD.TextRecipe= txtTextCoctail.Text;
                            txtTextCoctail.Text = "";
                            txtIngrCoctail.Text = "";
                            txtTitleCoctail.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Такой рецепт уже есть!");
                            flagSave = false;
                            return;
                        }
                    }

                    //начинаю сохранять в бд
                    if (flagSave)
                    {
                        try
                        {
                            //Добавление темы.
                            if (!flagTopic) //если такой темы ещё нет в БД
                            {
                                unitOfWork.Coctails.Add(coctailInBD.NameForRecipes.ToString(), coctailInBD.RightIngredients.ToString(), coctailInBD.TextRecipe.ToString());
                            }
                            MessageBox.Show("Добавление темы прошло успешно!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }                        
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
