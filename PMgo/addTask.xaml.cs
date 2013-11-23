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
    /// Interaction logic for addTask.xaml
    /// </summary>
    public partial class addTask : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public addTask()
        {
            InitializeComponent();            
            fill_projectField();
            fill_addUserBox();
            //fill_userBox();
        }

        void fill_projectField()
        {
           SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select * from projects;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string proj_name = dr.GetString(1);
                    projectField.Items.Add(proj_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_projectIdField()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try{
                conn.Open();
                    string query = "select project_id from projects where project_name = '" + projectField.SelectedItem + "';";
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    //createCommand.ExecuteNonQuery();
                    SQLiteDataReader dr = createCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        string proj_id = dr.GetInt32(0).ToString();
                        projectIdField.Text = proj_id;
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
                string query = "select task_name from tasks join projects on (projects.project_id = tasks.proj_id) where project_name = '" + projectField.SelectedItem + "' and status = 1;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string task_name = dr.GetString(0);                    
                    taskBox.Items.Add(task_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_userBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select user_name from users join users_tasks on (users_tasks.user_id = users.id) join tasks on (users_tasks.task_id = tasks.task_id) join projects on (projects.project_id = tasks.proj_id) where task_name = '"
                                                                                    + taskBox.SelectedItem + "' and projects.project_name = '" + projectField.SelectedItem +"';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                //userBox.Items.Add("PROJECT: '" + projectField.SelectedItem + "'");
                //userBox.Items.Add("TASK: '" + taskBox.SelectedItem + "'");
                //userBox.Items.Add("USERS:  ");
                while (dr.Read())
                {
                    string user = dr.GetString(0);                    
                    userBox.Items.Add(user);
                    
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fill_addUserBox()
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
                    string user = dr.GetString(0);
                    addUserBox.Items.Add(user);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            
            try
            {
                conn.Open();
                string query = "insert into tasks(proj_id, task_name, task_description, task_start, task_end) values('"
                                                                                     + this.projectIdField.Text
                                                                                     + "', '" + this.taskNameField.Text
                                                                                     + "', '" + this.taskDescField.Text
                                                                                     + "', '" + this.startDateField.Text
                                                                                     + "', '" + this.dueDateField.Text + "')";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                taskBox.Items.Refresh();
                MessageBox.Show("Task Was Added!");
                taskBox.Items.Clear();
                fill_taskBox();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void clearTaskItems()
        {
            taskIdField.Text = String.Empty;
            projectIdField.Text = String.Empty;
            taskNameField.Text = String.Empty;
            taskDescField.Text = String.Empty;
            startDateField.Text = String.Empty;
            dueDateField.Text = String.Empty;

        }

        private void taskBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select * from tasks where task_name = '" + taskBox.SelectedItem.ToString() + "' ;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr.GetInt32(0).ToString();
                    string proj_id = dr.GetInt32(1).ToString();
                    string taskName = dr.GetString(2);
                    string taskDesc = dr.GetString(3);
                    string start= dr.GetString(4);
                    string due = dr.GetString(5);
                   

                    taskIdField.Text = id;
                    projectIdField.Text = proj_id;
                    taskNameField.Text = taskName;
                    taskDescField.Text = taskDesc;
                    startDateField.Text = start;
                    dueDateField.Text = due;

                    
                }
                userBox.Items.Clear();
                fill_userBox();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void projectField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userBox.Items.Clear();
            addUserBox.Items.Clear();
            taskBox.Items.Clear();
            clearTaskItems();
            fill_taskBox();
            fill_projectIdField();
            fill_addUserBox();
            
        }

        private void delTask_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "update tasks set status = 0 where task_id = '" + this.taskIdField.Text + "' ";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                //MessageBox.Show("Task was deleted!");
                userBox.Items.Clear();
                projectField.Items.Clear();
                taskBox.Items.Clear();
                fill_projectField();
                fill_taskBox();
                conn.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            clearTaskItems();
        }

        private void userBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void findUsersButton_Click(object sender, RoutedEventArgs e)
        {
            userBox.Items.Clear();
            fill_userBox();
        }

        private void addUserBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into users_tasks (user_id, task_id) select id, task_id from users,tasks where users.user_name = '"
                                + this.addUserBox.SelectedItem + "'and tasks.task_name = '" + this.taskBox.SelectedItem + "';";
                //MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                //MessageBox.Show("User was Added!");
                userBox.Items.Clear();
                fill_userBox();

                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("User is already assigned!");
            }
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into users_tasks (user_id, task_id) select id, task_id from users,tasks where users.user_name = '" 
                                + this.addUserBox.SelectedItem + "'and tasks.task_name = '"+ this.taskBox.SelectedItem + "';";
                //MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                //MessageBox.Show("User was Added!");
                userBox.Items.Clear();
                fill_userBox();
                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void removeUser_Click_3(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            { 
                conn.Open();
                string query = "delete from users_tasks where (user_id in (select id from users where user_name ='" 
                                + this.userBox.SelectedItem + "') and task_id in (select task_id from tasks where task_name = '"
                                + this.taskBox.SelectedItem + "'));";
                //MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("User was Removed!");
                userBox.Items.Clear();
                fill_userBox();
                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Tasks taskWindow = new Tasks();
            taskWindow.ShowDialog();
            this.Close();
        }

        
        

        
     
        
    }
}
