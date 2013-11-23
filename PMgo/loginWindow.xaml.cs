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
    /// Interaction logic for startup.xaml
    /// </summary>
    public partial class startup : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public startup()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                conn.Open();
                string query = "select * from users where email = '" + this.userEmailField.Text + "' and password = '" + this.passwordField.Password + "';";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {
                    count = count + 1;
                }
                if (count == 1)
                {                    
                    MainWindow start = new MainWindow();
                    start.ShowDialog();
                    this.Close();
                }
                else if (count < 1)
                {
                    MessageBox.Show("You have not entered the correct email or password!  Please Try Again :)");
                }
                else if (count > 1)
                {
                    MessageBox.Show("There are duplicate records with your information.  Please contact the Security Admin.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
