using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Data.SQLite;
using System.Data;

namespace PMgo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class createDoc : Window
    {
        public createDoc()
        {
            InitializeComponent();
        }

        string _theType;
        public string TypeValue
        {
            get { return _theType; }
            set
            {
                _theType = value;
                this.typeBox.Text = _theType;
                
            }

        }

        string _theParent;

        public string ParentValue
        {
            get { return _theParent; }
            set
            {
                _theParent = value;
                this.parentBox.Text = _theParent;
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Stream mystream;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((mystream = ofd.OpenFile()) != null)
                {
                    String filename = ofd.FileName;
                    this.locationBox.Text = filename;
                    String text = File.ReadAllText(filename);
                    this.contentBox.Text = text;
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                string query;
                conn.Open();
                if (this.typeBox.Text == "milestone")
                {
                    query = "insert into documents(doc_name, doc_description, doc_location, milestone_id) values('" + this.docName.Text + "','" +
                                                                                                                      this.descBox.Text + "','" +
                                                                                                                      this.locationBox.Text +
                                                                                                                      "',(select milestone_id from milestones where milestone_name = '" + this.parentBox.Text + "'));";
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    System.Windows.MessageBox.Show("document Was Added!");
                }

                else if (this.typeBox.Text == "task")
                {
                    query = "insert into documents(doc_name, doc_description, doc_location, task_id) values('" + this.docName.Text + "','" + 
                                                                                                                 this.descBox.Text + "','" +
                                                                                                                 this.locationBox.Text +
                                                                                                                 "',(select task_id from tasks where task_name = '" + this.parentBox.Text + "'));";
                    System.Windows.Forms.MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    System.Windows.MessageBox.Show("document Was Added!");
                }

                else if (this.typeBox.Text == "subtask")
                {
                    query = "insert into documents(doc_name, doc_description, doc_location, subtask_id) values('" + this.docName.Text + "','" +
                                                                                                                    this.descBox.Text + "','" +
                                                                                                                    this.locationBox.Text +
                                                                                                                    "',(select subtask_id from subtasks where subtask_name = '" + this.parentBox.Text + "'));";
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    System.Windows.MessageBox.Show("document Was Added!");
                }             

                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void scrollBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            contentBox.ScrollToEnd();
        }
    }
}
