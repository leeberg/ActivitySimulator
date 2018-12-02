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
using SimpleImpersonation;

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
            formUserCredential.userPassword = strpassword;

            this.Close();

        }

        private void userButtonTest_Click(object sender, RoutedEventArgs e)
        {
            
            string domain = txtBoxDomain.Text;
            string username = txtBoxUserName.Text;
            string password = txtBoxPassword.Password;

            var credentials = new UserCredentials(domain, username, password);

            try
            {
                Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                {
                    // do whatever you want as this user.
                });

                MessageBox.Show("Authentication OK!");
            }
            catch
            {
                MessageBox.Show("Authentication FAIL!");
            }
            


        }


        //delegate void ClosingEventHandler(object sender, CredentialManager data);



    }
}
