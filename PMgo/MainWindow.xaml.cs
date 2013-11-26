using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace PMgo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

        public MainWindow()
        {
            InitializeComponent();
            //fill_projectField();
        }
        string _theValue;
        public string UserValue
        {
            get { return _theValue; } 
            set 
            {
                _theValue = value;
                this.current_txt.Text = _theValue;
                fill_projectField();

            }
        }

        void fill_projectField()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select project_name from projects where proj_mgr = (select id from users where user_name = '" + this.current_txt.Text + "');";
                MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string proj_name = dr.GetString(0);
                    projectField.Items.Add(proj_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            addTask tasks = new addTask();
            tasks.ShowDialog();
            this.Close();

        }

        private void projectField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String project_name = this.projectField.SelectedItem.ToString();
            String user = this.current_txt.Text;

            MainView mainView = new MainView(project_name);
            mainView.UserValue = user;
            mainView.ShowDialog();
         
         }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddProject project = new AddProject();
            project.ShowDialog();
            this.Close();
        }
    }
}
