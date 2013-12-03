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
    /// Interaction logic for account.xaml
    /// </summary>
    public partial class account : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

        public account()
        {
            InitializeComponent();
        }

           
        string _theUser;

        public string UserValue
        {
            get { return _theUser; }
            set
            {
                _theUser = value;
                this.current_txt.Text = _theUser;
                fill_userData();
                
            }

        }

        void fill_userData()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                
                conn.Open();
                string query = "select * from users where user_name = '" + this.UserValue + "';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string title = dr.GetString(2);
                    string email = dr.GetString(3);
                    string phone = dr.GetString(4);
                    string password = dr.GetString(5);
                    string office = dr.GetString(6);
                    this.titleField.Text = title;
                    this.emailField.Text = email;
                    this.phoneField.Text = phone;
                    this.passwordField.Text = password;
                    this.officeField.Text = office;

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
                string query = "update users set password = '" + this.passwordField.Text + "' where user_name = '" + this.current_txt.Text + "';";

                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                
                MessageBox.Show("Password Was Successfully Changed!");
                conn.Close();
                


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
    }
}
