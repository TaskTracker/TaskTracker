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
using System.Data;

namespace PMgo
{
    /// <summary>
    /// Interaction logic for addMenu.xaml
    /// </summary>
    public partial class addMenu : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

        public addMenu()
        {
            InitializeComponent();
            fill_availableUsersBox();
        }

        string _theValue;
        public string ProjectNameValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.project_txt.Text = value;
            }

        }
        void fill_for_subtask()
        {

            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionstring);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
              string Query = "select task_name, project_name from tasks, projects where projects.project_id = tasks.proj_id and projects.project_name = '" + this.project_txt.Text + "'";
           
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                //MessageBox.Show(Query);
                // createcommand.ExecuteNonQuery();
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    parent_combo.Items.Add(name);
                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_availableUsersBox()
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionstring);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
                string Query = "select user_name from users;";
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                //MessageBox.Show(Query);
                // createcommand.ExecuteNonQuery();
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    availableUsersBox.Items.Add(name);
                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_for_task()
        {

            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionstring);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
               string Query = "select milestone_name, project_name from milestones, projects where projects.project_id = milestones.project_id and projects.project_name = '" + this.project_txt.Text + "'";
               
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                //MessageBox.Show(Query);
                // createcommand.ExecuteNonQuery();
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    parent_combo.Items.Add(name);
                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       /* void fill_for_milestone()
        {
            parent_combo.IsEnabled = false;
        }
        */

        
        void fill_for_document()
        {

            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionstring);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
                string Query = "select milestones.milestone_name, tasks.task_name, subtasks.subtask_name from milestones INNER JOIN tasks ON (milestones.milestone_id = tasks.milestone_id) INNER JOIN subtasks ON (tasks.task_id = subtasks.task_id) INNER JOIN projects ON (milestones.project_id = projects.project_id) WHERE projects.project_name = '" + this.project_txt.Text + "'";

                
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                //MessageBox.Show(Query);
                // createcommand.ExecuteNonQuery();
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    string name1 = dr.GetString(1);
                    string name2 = dr.GetString(2);
                    parent_combo.Items.Add(name);
                    parent_combo.Items.Add(name1);
                    parent_combo.Items.Add(name2);

                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void clearInputItems()
        {
            nameBox.Text = String.Empty;
            descriptionBox.Text = String.Empty;
            startBox.Text = String.Empty;
            endBox.Text = String.Empty;
            parent_combo.Items.Clear();
            
        }

        private void type_combo_Loaded(object sender, RoutedEventArgs e)
       { 
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Milestone");
            data.Add("Task");
            data.Add("Subtask");
            data.Add("Document");
            // ... Get the ComboBox reference.
            var combo = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            combo.ItemsSource = data;

            // ... Make the first item selected.
            combo.SelectedIndex = 0;
        }

        private void type_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var combo = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = combo.SelectedItem as string;
            this.Title = "Selected: " + value;
            
            string selected = combo.SelectedItem as string;
           
            if (selected == "Task")
            {
                parent_combo.IsEnabled = true;
                parent_combo.Items.Clear();
                //MessageBox.Show(selected);
                fill_for_task();
            }

            if (selected == "Subtask")
            {
                parent_combo.IsEnabled = true;
                parent_combo.Items.Clear();
                //MessageBox.Show(selected);
                fill_for_subtask();
            }

            if (selected == "Milestone")
            {
                //MessageBox.Show(selected);
                parent_combo.Items.Clear();
                parent_combo.IsEnabled = false;
            }

            if (selected == "Document")
            {
                parent_combo.IsEnabled = true;
                parent_combo.Items.Clear();
                //MessageBox.Show(selected);
                fill_for_document();
            }

        }
        string dbConnectionstring = @"Data Source=PMgo.sqlite;Version=3;";

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();  
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void submit_btn_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();

                if (type_combo.SelectedItem == "Task")
                {
                    string query = "insert into tasks(proj_id, task_name, task_description, task_start, task_end) values((select project_id from projects where project_name = '" + this.project_txt.Text + "'), '"
                                                                + this.nameBox.Text
                                                                + "', '" + this.descriptionBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text
                                                                + "');";
                    MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Task Was Added!");
                    conn.Close();
                    clearInputItems();
                }

                else if (type_combo.SelectedItem == "Milestone")
                {
                    string query = "insert into milestones(milestone_name, milestone_desc, milestone_start, milestone_end, project_id) values('"
                                                                + this.nameBox.Text
                                                                + "', '" + this.descriptionBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text
                                                                + "',(select project_id from projects where project_name = '" + this.project_txt.Text + "'));";
                    MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Milestone Was Added!");
                    conn.Close();
                    clearInputItems();
                }

                else if (type_combo.SelectedItem == "Subtask")
                {
                    string query = "insert into subtasks(task_id, subtask_name, subtask_description, subtask_start, subtask_end) values((select task_id from tasks where task_name = '" + this.parent_combo.Text + "'), '"
                                                                + this.nameBox.Text
                                                                + "', '" + this.descriptionBox.Text
                                                                + "', '" + this.startBox.Text
                                                                + "', '" + this.endBox.Text
                                                                + "');";
                    MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Subtask Was Added!");
                    conn.Close();
                    clearInputItems(); 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
     
    }
}
