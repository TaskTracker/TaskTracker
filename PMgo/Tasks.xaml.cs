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
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public Tasks()
        {
            InitializeComponent();
            fill_taskBox();
            fill_projBox();
            fill_otherUsersBox();
            fill_progressBox();
        }

        void fill_projBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                //string query = "select task_name from tasks;"
                string query = "select project_name from projects join tasks on (tasks.proj_id = projects.project_id) join users_tasks on (users_tasks.task_id = tasks.task_id) join users on (users_tasks.user_id = users.id) where users.user_name = '"
                                                            + this.userNameField.Text + "' group by projects.project_name;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string task_name = dr.GetString(0);
                    projNameBox.Items.Add(task_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_taskBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                //string query = "select task_name from tasks;"
                string query = "select task_name from tasks join users_tasks on (users_tasks.task_id = tasks.task_id) join users on (users_tasks.user_id = users.id) where users.user_name = '" 
                                                            + this.userNameField.Text + "' and tasks.status = 1;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string task_name = dr.GetString(0);
                    taskListBox.Items.Add(task_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void fill_progressBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                //string query = "select task_name from tasks;"
                string query = "select * from progress where task_id = '" + this.taskIdField.Text + "';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string progressDate= dr.GetString(2);
                    string progressComments = dr.GetString(3);
                    string progressUserName = dr.GetString(5);
                    progressListBox.Items.Add(progressDate + "  --->  " + progressUserName + ":  " + progressComments);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void clearTaskItems()
        {
            taskIdField.Text = String.Empty;
            //projectIdField.Text = String.Empty;
            taskNameField.Text = String.Empty;
            taskDescField.Text = String.Empty;
            startDateField.Text = String.Empty;
            dueDateField.Text = String.Empty;

        }

        void fill_otherUsersBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                usersListBox.Items.Clear();
                conn.Open();
                string query = "select user_name from users join users_tasks on (users_tasks.user_id = users.id) join tasks on (tasks.task_id = users_tasks.task_id) join projects on (projects.project_id = tasks.proj_id) where task_name = '"
                                                                                    + taskListBox.SelectedItem + "' and projects.project_name = '" + projNameBox.SelectedItem + "'and users.user_name <> '" + this.userNameField.Text + "';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                usersListBox.Items.Clear();
                while (dr.Read())
                {
                    string user = dr.GetString(0);
                    usersListBox.Items.Add(user);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void taskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select * from tasks where task_name = '" + taskListBox.SelectedItem.ToString() + "' ;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr.GetInt32(0).ToString();
                    //string proj_id = dr.GetInt32(1).ToString();
                    string taskName = dr.GetString(2);
                    string taskDesc = dr.GetString(3);
                    string start = dr.GetString(4);
                    string due = dr.GetString(5);


                    taskIdField.Text = id;
                    //projectIdField.Text = proj_id;
                    taskNameField.Text = taskName;
                    taskDescField.Text = taskDesc;
                    startDateField.Text = start;
                    dueDateField.Text = due;


                }
                usersListBox.Items.Clear();
                fill_otherUsersBox();
                progressListBox.Items.Clear();
                fill_progressBox();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void projNameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            clearTaskItems();
            try
            {
                conn.Open();
                string query = "select task_name from tasks join projects on (projects.project_id = tasks.proj_id) join users_tasks on (users_tasks.task_id = tasks.task_id) join users on (users.id = users_tasks.user_id) where project_name = '"
                                + projNameBox.SelectedItem + "' and user_name = '" + this.userNameField.Text + "' and tasks.status = 1;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                
                taskListBox.Items.Clear();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string task_name = dr.GetString(0);
                    taskListBox.Items.Add(task_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            addTask task = new addTask();
            task.ShowDialog();
            
            this.Close();

        }

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string value_id = this.taskIdField.Text;
            string value_name = this.userNameField.Text;
            Progress progress = new Progress();
            progress.TaskIDValue = value_id;
            progress.UserNameValue = value_name;
            progress.ShowDialog();
        }

        private void commentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
        }
    }
}
