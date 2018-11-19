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
    /// Interaction logic for UserDomain.xaml
    /// </summary>
    public partial class DomainWindow : Window
    {
        public event Action<string> passDomainDetailsToParent;

        private string formUserDomain;


        public DomainWindow(string domain, string mode)
        {
            InitializeComponent();
            Closed += ImportForm_Closed;
            //CredentialManager formUserCredential = new CredentialManager formUserCredential();

            formUserDomain = domain;
            txtBoxDomain.Text = formUserDomain;
   

        }

        private void ImportForm_Closed(object sender, EventArgs e)
        {

            passDomainDetailsToParent(formUserDomain);

        }

        private void domainButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string userDomain = txtBoxDomain.Text;

            formUserDomain = userDomain;

            this.Close();

        }
    }
}
