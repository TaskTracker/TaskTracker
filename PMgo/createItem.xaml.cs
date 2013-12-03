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
    /// Interaction logic for createItem.xaml
    /// </summary>
    public partial class createItem : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";


        string _theValue;
        public string ProjectValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.projectNameBox.Text = _theValue;
                fillMilestoneBox();
            }

        }

        public createItem()
        {

            InitializeComponent();
            this.projectNameBox.Text = ProjectValue;
            //this.projectNameBox.Text = this.ProjectValue;
            //fillMilestoneBox();

        }
        

        void fillMilestoneBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                //this.ProjectValue = this.projectNameBox.Text;
                conn.Open();
                string query = "select milestone_name from milestones where project_id = (select project_id from projects where project_name = '" + this.projectNameBox.Text + "');";
               // MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string milestone = dr.GetString(0);
                    milestoneBox.Items.Add(milestone);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into tasks(proj_id, task_name, task_description, task_start, task_end, milestone_id) values((select project_id from projects where project_name = '" + this.projectNameBox.Text + "'), '"
                                                                + this.nameBox.Text
                                                                + "', '" + this.descBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text
                                                                + "', (select milestone_id from milestones where milestone_name = '" + this.milestoneBox.SelectedItem + "')) ;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Task Was Added!");             
                
                conn.Close();


				//not necessary if we don't close the original one
				//so we just need to update it
				
                //string projectName = this.projectNameBox.Text;
                //itemWindow update = new itemWindow();
                //update.ProjectNameValue = projectName;
                //update.ShowDialog();

				
                
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
