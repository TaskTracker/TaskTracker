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
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public AddProject()
        {
            InitializeComponent();
            fill_pmBox();
            fill_projListBox();
        }

        public string ProjectNameValue
        {
            get;
            set;
        }


        string _theUser;

        public string UserValue
        {
            get { return _theUser; }
            set
            {
                _theUser = value;
                this.current_txt.Text = _theUser;

            }

        }

        void fill_projListBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select project_name from projects;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string projname = dr.GetString(0);
                    projListBox.Items.Add(projname);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_pmBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select user_name from users;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string username = dr.GetString(0);
                    pmBox.Items.Add(username);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into projects(proj_mgr, project_name, start_date, end_date, description) values((select users.id from users where user_name = '" + this.pmBox.SelectedItem + "'), '"
                                                                                     + this.projNameField.Text
                                                                                     + "', '" + this.startField.Text
                                                                                     + "', '" + this.endField.Text
                                                                                     + "', '" + this.descriptionField.Text + "')";
                string query2 = "insert into milestones(milestone_name, milestone_desc, milestone_start, milestone_end, project_id) values('" + this.projNameField.Text + " Start', 'Start Milestone', '" + this.startField + "','test', (select project_id from projects where project_name = '" + this.projNameField.Text + "'));";
                MessageBox.Show(query2);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                SQLiteCommand createCommand2 = new SQLiteCommand(query2, conn);                
                createCommand.ExecuteNonQuery();                
                MessageBox.Show("Project Was Added!");
                projListBox.Items.Clear();
                fill_projListBox();
                createCommand2.ExecuteNonQuery();

                string project_name = this.projNameField.Text;
                string username = this.current_txt.Text;
                MainView main = new MainView(project_name);
                main.UserValue = username;
                main.ProjectNameValue = project_name;
                main.ShowDialog();
                
                this.Close();

                
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

       
    }
}
