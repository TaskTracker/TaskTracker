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
    /// Interaction logic for viewDoc.xaml
    /// </summary>
    public partial class viewDoc : Window
    {
        public viewDoc()
        {
            InitializeComponent();
            //populateDoc();
        }

        string _theName;
        public string NameValue
        {
            get { return _theName; }
            set
            {
                _theName = value;
                this.docName.Text = _theName;
            }

        }

        string _theDesc;

        public string DescValue
        {
            get { return _theDesc; }
            set
            {
                _theDesc = value;
                this.docDesc.Text = _theDesc;
            }

        }

        string _theLocation;
        public string LocationValue
        {
            get { return _theLocation; }
            set
            {
                _theLocation = value;
                this.locationBox.Text = value;
                populateDoc();
            }

        }

        void populateDoc()
        {
            string filename = this.locationBox.Text;
                   
            String text = File.ReadAllText(filename);
            this.docText.Text = text;
                     
            
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

        private void docTextScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            docText.ScrollToHome();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Document was saved!");
        }

       
       

        
    }
}
