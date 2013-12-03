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
    /// Interaction logic for addUser.xaml
    /// </summary>
    public partial class addUser : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public addUser()
        {
            InitializeComponent();
            HideAddUserButton();    
           
        }

        string _theValue;

        public string ProjectNameValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.projectNameBox.Text = _theValue;                
                fill_assignedUsersBox();
                fill_userNameBox();
                fill_documentManagerBox();
                fill_documentReviewerBox();
            }

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

        private void HideButtons()
        {
            deleteUserControl.Visibility = Visibility.Hidden;
            modifyUserControl.Visibility = Visibility.Hidden;

        }

        private void ShowButtons()
        {
            deleteUserControl.Visibility = Visibility.Visible;
            modifyUserControl.Visibility = Visibility.Visible;

        }

        private void HideAddUserButton()
        {
            addUserControl.Visibility = Visibility.Hidden;            
           
        }

        private void ShowAddUserButton()
        {
            addUserControl.Visibility = Visibility.Visible;
        }

        void clearUserItems()
        {
            userIdField.Text = String.Empty;
            userNameField.Text = String.Empty;
            titleField.Text = String.Empty;
            emailField.Text = String.Empty;
            phoneField.Text = String.Empty;
            passwordField.Text = String.Empty;
            officeField.Text = String.Empty;
            
        }
        void fill_userNameBox()
        {
            
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                userNameBox.Items.Clear();
                conn.Open();
                string query = "select user_name from users;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string username = dr.GetString(0);
                    userNameBox.Items.Add(username);
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                conn.Close();
            }
            
        }

        void fill_documentManagerBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select user_name from users join projects on (projects.doc_mgr = users.id) where project_name = '" + this.projectNameBox.Text + "' and projects.doc_mgr = users.id;";
                MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string assigned = dr.GetString(0);
                    documentManagerBox.Text = assigned;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void fill_documentReviewerBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select user_name from users join projects on (projects.doc_reviewer = users.id) where project_name = '" + this.projectNameBox.Text + "' and projects.doc_reviewer = users.id;";
                MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string assigned = dr.GetString(0);
                    documentReviewerBox.Text = assigned;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void fill_assignedUsersBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                assignedUsersBox.Items.Clear();
                conn.Open();
                string query = "select user_name from users join projects_users on (projects_users.user_id = users.id) where users.id = projects_users.user_id and projects_users.proj_id = (select project_id from projects where project_name = '" + ProjectNameValue + "');";
                MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string assignedUser = dr.GetString(0);
                    assignedUsersBox.Items.Add(assignedUser);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ShowButtons();
            HideAddUserButton();
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select * from users where user_name = '" + userNameBox.SelectedItem.ToString() + "' ;";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr.GetInt32(0).ToString();
                    string username = dr.GetString(1);
                    string title = dr.GetString(2);
                    string email = dr.GetString(3);
                    string phone = dr.GetString(4);
                    string password = dr.GetString(5);
                    string office = dr.GetString(6);

                    userIdField.Text = id;
                    userNameField.Text = username;
                    titleField.Text = title;
                    emailField.Text = email;
                    phoneField.Text = phone;
                    passwordField.Text = password;
                    officeField.Text = office;


                }
                
                //userNameBox.Items.Clear();
                //fill_userBox();

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
                string query = "insert into users(user_name, title, email, phone1, password, office_location) values('"
                                                                                     + this.userNameField.Text
                                                                                     + "', '" + this.titleField.Text
                                                                                     + "', '" + this.emailField.Text
                                                                                     + "', '" + this.phoneField.Text
                                                                                     + "', '" + this.passwordField.Text
                                                                                     + "', '" + this.officeField.Text + "')";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();                
                MessageBox.Show("User Was Added!");                
                fill_userNameBox();
                HideAddUserButton();
                ShowButtons();
                //clearUserItems();
                conn.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clearUserItems();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                //userNameBox.Items.Clear();
                conn.Open();
                string query = "delete from users where id = '" + this.userIdField.Text + "';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                //userNameBox.UnselectAll();
                //MessageBox.Show("User Was Deleted!");
                //userNameBox.Items.Refresh();
                fill_userNameBox();
                clearUserItems();
                conn.Close();                

            }
            catch (Exception)
            {
                MessageBox.Show("User was successfully deleted!");
            }
            //userNameBox.Items.Clear();
            
            
            
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "update users set user_name = '" + this.userNameField.Text
                            + "', title = '" + this.titleField.Text
                            + "', email = '" + this.emailField.Text
                            + "', phone1 = '" + this.phoneField.Text
                            + "', password = '" + this.passwordField.Text
                            + "', office_location = '" + this.officeField.Text + "' where id = '" + this.userIdField.Text + "'";

                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                userNameBox.Items.Refresh();
                MessageBox.Show("User Was Modified!");
                conn.Close();
                fill_userNameBox();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //userIdField.IsEnabled = false;
            clearUserItems();
            HideButtons();
            ShowAddUserButton();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "delete from projects_users where (user_id in (select id from users where user_name ='"
                                + this.assignedUsersBox.SelectedItem + "') and proj_id in (select project_id from projects where project_name = '"
                                + this.ProjectNameValue + "'));";
                //MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("User was Removed!");                
                fill_assignedUsersBox();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into projects_users (proj_id, user_id) select project_id, id from projects, users where projects.project_name = '"
                                + ProjectNameValue + "'and users.user_name = '" + userNameBox.SelectedItem + "';";
                //MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("User was Assigned!");
                //userNameBox.Items.Clear();
                fill_assignedUsersBox();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("User is already assigned!");
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            if (this.documentManagerBox != null)
            {
                try
                {
                    conn.Open();
                    string query = "select id from users where user_name = '" + this.userNameBox.SelectedItem + "';";
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    SQLiteDataReader dr = createCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        int userID = dr.GetInt32(0);


                        string query2 = "update projects set doc_mgr =" + userID + " where project_name = '" + this.projectNameBox.Text + "';";
                        MessageBox.Show(query2);
                        SQLiteCommand createCommand2 = new SQLiteCommand(query2, conn);
                        createCommand2.ExecuteNonQuery();
                        MessageBox.Show("User was assigned as Document Manager!");
                    }
                    //userNameBox.Items.Clear();
                    fill_documentManagerBox();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User is already assigned!");
                }
            }
           
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            if (this.documentReviewerBox != null)
            {
                try
                {
                    conn.Open();
                    string query = "select id from users where user_name = '" + this.userNameBox.SelectedItem + "';";
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    SQLiteDataReader dr = createCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        int userID = dr.GetInt32(0);


                        string query2 = "update projects set doc_reviewer=" + userID + " where project_name = '" + this.projectNameBox.Text + "';";
                        MessageBox.Show(query2);
                        SQLiteCommand createCommand2 = new SQLiteCommand(query2, conn);
                        createCommand2.ExecuteNonQuery();
                        MessageBox.Show("User was assigned as Document Reviewer!");
                    }
                    //userNameBox.Items.Clear();
                    fill_documentReviewerBox();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User is already assigned!");
                }
            }
        }
    }
}
