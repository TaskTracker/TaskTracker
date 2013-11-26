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
    /// Interaction logic for createMilestone.xaml
    /// </summary>
    public partial class createMilestone : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public createMilestone()
        {
            InitializeComponent();
        }

        string _theValue;
        public string ProjectValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.projectNameBox.Text = _theValue;
                //fillMilestoneBox();
                //MessageBox.Show(this.projectNameBox.Text);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into milestones(milestone_name, milestone_desc, milestone_start, milestone_end, project_id) values('" + this.nameBox.Text 
                                                                + "', '" + this.descBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text + "', (select project_id from projects where project_name = '" + this.projectNameBox.Text + "'));";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Milestone Was Added!");

                conn.Close();
//no need to create a new itemWindow since we didn't close the original
                this.Close();

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
    }
}
