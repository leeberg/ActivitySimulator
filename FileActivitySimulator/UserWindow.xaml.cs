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

namespace FileActivitySimulator
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public event Action<CredentialManager> passUserDetailsToParent;

        private CredentialManager formUserCredential;



        public UserWindow(string username, string domain, string password, string mode)
        {
            InitializeComponent();
            Closed += ImportForm_Closed;
            //CredentialManager formUserCredential = new CredentialManager formUserCredential();
            formUserCredential = new CredentialManager();

       
            txtBoxUserName.Text = username;
            txtBoxDomain.Text = domain;
            //txtBoxPassword = password;
           

        }

        private void ImportForm_Opened(string username, string domain, string password)
        {
            passUserDetailsToParent(formUserCredential);
        }


        private void ImportForm_Closed(object sender, EventArgs e)
        {
            
            passUserDetailsToParent(formUserCredential);
        }


        private void userButtonSave_Click(object sender, RoutedEventArgs e )
        {

            string strusername = txtBoxUserName.Text;
            string strdomain = txtBoxDomain.Text;
            string strpassword = txtBoxPassword.Password;

        
            formUserCredential.userName = strusername;
            formUserCredential.userDomain = strdomain;            

            this.Close();

        }


        //delegate void ClosingEventHandler(object sender, CredentialManager data);



    }
}
