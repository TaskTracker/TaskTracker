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
            userNameBox.Items.Refresh();
            //userNameBox.Items.Clear();
            //clearUserItems();
            fill_userBox();
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
        void fill_userBox()
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
                fill_userBox();
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
                fill_userBox();
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
                fill_userBox();


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
    }
}
