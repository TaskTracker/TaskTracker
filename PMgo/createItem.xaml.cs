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

        public createItem()
        {
            
            InitializeComponent();
            projectNameBox.Text = this.ProjectNameValue;
            fillMilestoneBox();
            
        }


        string _theValue;
        public string ProjectNameValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.projectNameBox.Text = _theValue;
            }    

        }

        

        void fillMilestoneBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                this.ProjectNameValue = this.projectNameBox.Text;
                conn.Open();
                string query = "select milestone_name from milestones where project_id = (select project_id from projects where project_name = '" + ProjectNameValue + "');";
                MessageBox.Show(query);
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

        }
    }
}
