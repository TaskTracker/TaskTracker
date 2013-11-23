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
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        private string _theValue;
        private string _theValue2;
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public string TaskIDValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.taskIdField.Text = _theValue;
            }
        }

        public string UserNameValue
        {
            get { return _theValue2; }
            set
            {
                _theValue2 = value;
                this.userNameField.Text = _theValue2;
            }
        }

        public Progress()
        {
            InitializeComponent();
           
        }

        
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "insert into progress(task_id, user_name, date, comments) values('"
                                                                                     + this.taskIdField.Text + "','" 
                                                                                     + this.userNameField.Text
                                                                                     + "', '" + this.dateField.Text
                                                                                     + "', '" + this.commentsField.Text + "');";
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Progress Was Added!");
               
                this.Close();             
               


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
