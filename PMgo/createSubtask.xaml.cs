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
    /// Interaction logic for createSubtask.xaml
    /// </summary>
    public partial class createSubtask : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

        public createSubtask()
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

        
        public string TaskValue
        {
            get{return _theValue;}
            set
            {
                _theValue = value;
                this.taskNameBox.Text = TaskValue;
                //MessageBox.Show(this.nameBox.Text);
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
                string query = "insert into subtasks(task_id, subtask_name, subtask_description, subtask_start, subtask_end) values((select task_id from tasks where task_name = '" + this.taskNameBox.Text + "'), '"
                                                                + this.nameBox.Text
                                                                + "', '" + this.descBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text + "');";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Subtask Was Added!");

                conn.Close();
                string projectName = this.projectNameBox.Text;
                itemWindow update = new itemWindow();
                update.ProjectNameValue = projectName;
                update.ShowDialog();

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
