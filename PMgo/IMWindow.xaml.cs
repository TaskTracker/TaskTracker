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
    /// Interaction logic for IMWindow.xaml
    /// </summary>
    public partial class IMWindow : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
        public IMWindow()
        {
            InitializeComponent();
            fill_IMusersBox();
        }

        void fill_IMusersBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);


            try
            {
                conn.Open();

                string query = "select user_name from users;";
                //MessageBox.Show(query);
                SQLiteCommand createcommand = new SQLiteCommand(query, conn);
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string userName = dr.GetString(0);
                    IMusers.Items.Add(userName);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }




}
