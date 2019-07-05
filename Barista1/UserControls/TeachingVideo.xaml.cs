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

namespace Barista1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TeachingVideo.xaml
    /// </summary>
    public partial class TeachingVideo : UserControl
    {
        DataTable videosTable = new DataTable();
        SqlDataAdapter adapter;
        List<Classes.Video> listVideos = new List<Classes.Video>();
        UnitOfWork unitOfWork = new UnitOfWork();

        public TeachingVideo()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            listVideos.Clear();
            try
            {
                SqlCommand command = unitOfWork.Videos.GetList();
                adapter = new SqlDataAdapter(command);
                adapter.Fill(videosTable);

                SqlDataReader reader1 = command.ExecuteReader();

                if (reader1.HasRows)
                {
                    int j = 0;
                    while (reader1.Read())
                    {
                        listVideos.Add(new Classes.Video());
                        listVideos[j].idVideo = reader1.GetValue(0);
                        listVideos[j].nameVideo = reader1.GetValue(1).ToString();
                        listVideos[j].sourceVideo = reader1.GetValue(2).ToString();
                        j++;
                    }
                }
                reader1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            addElements();
        }

        void addElements()
        {
            WindowContent.Children.Clear();
            for (int i = 0; i < listVideos.Count; i++)
            {
                StackPanel stackPanel = new StackPanel();

                Border videoBorder = new Border();
                videoBorder.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/total/coffee20.ico")));

                MediaElement media = new MediaElement();
                media.Width = 350;
                media.Height = 300;
                media.Name = $"VideoControl{i}";
                media.Source = new Uri($"{listVideos[i].sourceVideo.ToString()}", UriKind.Relative);
                media.LoadedBehavior = MediaState.Manual;
                media.UnloadedBehavior = MediaState.Stop;

                TextBox textBox = new TextBox();
                textBox.Margin = new Thickness(0, 10, 0, 0);
                textBox.BorderThickness = new Thickness(0);
                textBox.Height = 30;
                textBox.IsReadOnly = true;
                textBox.Style = (Style)textBox.FindResource("TopicText");
                textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox.Text = Convert.ToString(listVideos[i].nameVideo);

                DockPanel dockPanel = new DockPanel();
                dockPanel.Background = (Brush)dockPanel.FindResource("Dark");

                Button btn1 = new Button();
                btn1.Style = (Style)btn1.FindResource("TopicButton");
                btn1.BorderThickness = new Thickness(0);
                btn1.Width = 200;
                btn1.Click += PlayClick;
                btn1.Content = "Воспроизвести";

                Button btn2 = new Button();
                btn2.Style = (Style)btn1.FindResource("TopicButton");
                btn2.BorderThickness = new Thickness(0);
                btn2.Width = 200;
                btn2.Click += StopClick;
                btn2.Content = "Стоп";

                videoBorder.Child = media;
                dockPanel.Children.Add(btn1);
                dockPanel.Children.Add(btn2);
                stackPanel.Children.Add(videoBorder);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(dockPanel);
                WindowContent.Children.Add(stackPanel);
            }
        }

        void PlayClick(object sender, EventArgs e)
        {
            Button btn=(Button) sender;
            DockPanel dp =(DockPanel) btn.Parent;
            StackPanel sp = (StackPanel)dp.Parent;
            Border border = (Border)sp.Children[0];
            MediaElement me = (MediaElement)border.Child;
            me.Play();
        }

        void StopClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            DockPanel dp = (DockPanel)btn.Parent;
            StackPanel sp = (StackPanel)dp.Parent;
            Border border = (Border)sp.Children[0];
            MediaElement me = (MediaElement)border.Child;
            me.Stop();
        }

    }
}
