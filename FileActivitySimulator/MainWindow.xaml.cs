using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;
using System.Windows.Controls.Primitives;
using System.Data;
using System.Windows.Forms;
using System.Net;

namespace FileActivitySimulator
{
      

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string parentActivityFolderPath;
        public bool simulationActive = false;
        public string configFilePath;

        private SimulationManager SimManager;
        
       
        //Lists
        public List<FileSimulationObject> fileSimulationObjects = new List<FileSimulationObject>();
        public List<FileSimulationObject> fileSimulationFiles = new List<FileSimulationObject>();
        public List<FileSimulationObject> fileSimulationDirectories = new List<FileSimulationObject>();

        //Cred
        public List<NetworkCredential> NetworkCredentialList = new List<NetworkCredential>();
        private CredentialManager CredManager;

        //Console Stuff
        ConsoleContent dc = new ConsoleContent();

        // Sim Timers
        public double simulationActivityDelay = 10000;
        private static System.Timers.Timer SimulationTimer;

        private int fileSimulationFilesCount;


        private bool configFileLoaded = false; 


        public MainWindow()
        {
            InitializeComponent();
            DataContext = dc;
            SetupTimer();
            SetupDataGrids();

            

        }

        private void SetupDataGrids()
        {
         


        }

        private void SetupTimer()
        {

            SimManager = new SimulationManager();
            // Tell the timer what to do when it elapses
            SimulationTimer = new System.Timers.Timer(simulationActivityDelay);

            // Set it to go off every interval defined by slider
            simulationActivityDelay = 10000;
            SimulationTimer.Interval = simulationActivityDelay;
            SimulationTimer.Enabled = false;

            SimulationTimer.Elapsed += new ElapsedEventHandler(initateNewSimulationEvent);
    
        }


        private void UpdateTimerInterval()
        {
            SimulationTimer.Interval = simulationActivityDelay;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create a "Save As" dialog for selecting a directory (HACK)
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = txtboxParentActivityFolder.Text; // Use current value for initial dir
            dialog.Title = "Select a Directory"; // instead of default "Save As"
            dialog.Filter = "Directory|*.this.directory"; // Prevents displaying files
            dialog.FileName = "select"; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");

                parentActivityFolderPath = path;
                txtboxParentActivityFolder.Text = path;
                insertToConsole("Selected Parent Folder: " + path);
                

            }
        }
        private void btnConfigFileLoad_Click(object sender, RoutedEventArgs e)
        {
            insertToConsole("Starting to Load Configuration File!");
            
            try
            {

                // Get Current Config file
                string pathtoConfig = configFilePath;
                            
                // read JSON directly from a file
                using (StreamReader file = File.OpenText(pathtoConfig))
                {
                    string json = file.ReadToEnd();
                    fileSimulationObjects = JsonConvert.DeserializeObject<List<FileSimulationObject>>(json);
                    foreach (FileSimulationObject fileSimObject in fileSimulationObjects)
                    {
                        if (fileSimObject.fileType == "Directory")
                        {
                            fileSimulationDirectories.Add(fileSimObject);
                        }

                        if (fileSimObject.fileType == "File")
                        {
                            fileSimulationFiles.Add(fileSimObject);
                        }
                    }


                    fileSimulationFilesCount = fileSimulationFiles.Count;

                }
                configFileLoaded = true;
                insertToConsole("Configuration File Loaded!");

            }
            catch (Exception err)
            {
                insertToConsole("Exception reading Configuration file");
                insertToConsole(err.TargetSite + " " + err.Message);

            }
                      

        }

     
        private void btnConfigFileBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Create a "Save As" dialog for selecting a directory (HACK)
            var configFileDialog = new Microsoft.Win32.OpenFileDialog();
            configFileDialog.InitialDirectory = txtboxConfigurationFilePath.Text; // Use current value for initial dir
            configFileDialog.Title = "Select a config file"; // instead of default "Save As"
            configFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            configFileDialog.FileName = "";
            if (configFileDialog.ShowDialog() == true)
            {
                string path = configFileDialog.FileName;
            
                // Our final value is in path
                txtboxConfigurationFilePath.Text = path;
                string directoryPath = (System.IO.Path.GetDirectoryName(path)) + "\\" + (System.IO.Path.GetFileName(path));
                configFilePath = directoryPath;
                insertToConsole("Selected Config File: " + configFilePath);


                


            }
        }

        private void sliderActivityLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblActivityslider.Foreground = Brushes.Black;
            if (sliderActivityLevel.Value <= 3)
            {
                lblActivityslider.Content = "Minimal";
                simulationActivityDelay = 2000;


            }
            else if (sliderActivityLevel.Value <6)
            {
                lblActivityslider.Content = "Moderate";
                simulationActivityDelay = 7500;


            }
            else if (sliderActivityLevel.Value < 9)
            {
                lblActivityslider.Content = "High";
                simulationActivityDelay = 250;


            }
            else if (sliderActivityLevel.Value > 9)
            {

                lblActivityslider.Content = "Ludicrous";
                simulationActivityDelay = 100;
                lblActivityslider.Foreground = Brushes.Red;
            }

            UpdateTimerInterval();
        }


        private void insertToConsole(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                Scroller.ScrollToBottom();
                dc.WriteToConsole(DateTime.Now.ToString() + " : " + text);
            });

    
        }

        private void initateNewSimulationEvent(object source, ElapsedEventArgs e)
        {

       

            
            // Load Up a File System Operations
            FileSystemOperation FileSimulationOp = new FileSystemOperation();

            // Check Sim Manager to get Operation Type
            FileSimulationOp.OperationType = FileSimulationOp.GetRandomOperationType(SimManager);
            string RandomOperationType = FileSimulationOp.OperationType;


            // Setup Credential for Operations
            int networkCredentialsCount = NetworkCredentialList.Count;

            if (networkCredentialsCount > 0)
            {
                Random rnd = new Random();
                int r = rnd.Next(NetworkCredentialList.Count);

                FileSimulationOp.OperationCredential = NetworkCredentialList[r];
            }
            else
            {
                FileSimulationOp.OperationCredential = null;
            }

            //Check Random Op

            if (RandomOperationType == "NONE")
            {
                insertToConsole("No Event Types Selected");
            }
            else
            {
                // Setup New Instance of File Simulation Object

                // Right now a File Simulation is really just a FILE instance
                // Files / Objects should be seperated from the OPERATION LOGIC
                
                FileSimulationObject FileSimulationObject = new FileSimulationObject();

                // Pick File
                var random = new Random();
                int randomFileNumber = random.Next(fileSimulationFilesCount);
                FileSimulationObject = (fileSimulationFiles[randomFileNumber]);

                // Setup Operation Type
                FileSimulationObject.FileSystemOperation = FileSimulationOp;

                string FilePath = (parentActivityFolderPath + "\\" + FileSimulationObject.path);

                if (FileSimulationObject.FileSystemOperation.OperationType == "Read")
                {

                    insertToConsole(RandomOperationType + ": " + FileSimulationObject.name);
                    FileSimulationObject.FileSystemOperation.ReadFile(FilePath);


                }
                else if (FileSimulationObject.FileSystemOperation.OperationType == "Write")
                {
                    insertToConsole(RandomOperationType + ": " + FileSimulationObject.name);
                    FileSimulationObject.FileSystemOperation.UpdateFile(FilePath);
                }

                else if (FileSimulationObject.FileSystemOperation.OperationType == "Rename")
                {
                    insertToConsole(RandomOperationType + ": " + FileSimulationObject.name);
                    FileSimulationObject.FileSystemOperation.RenameFile(FilePath);
                }

                else if (FileSimulationObject.FileSystemOperation.OperationType == "Create")
                {
                    insertToConsole(RandomOperationType + ": " + FileSimulationObject.name);
                    FileSimulationObject.FileSystemOperation.createFile(FilePath);
                }

                else if (FileSimulationObject.FileSystemOperation.OperationType == "Delete")
                {
                    insertToConsole(RandomOperationType + ": " + FileSimulationObject.name);
                    FileSimulationObject.FileSystemOperation.DeleteFile(FilePath);
                }

                else
                {
                    insertToConsole(FileSimulationObject.FileSystemOperation.OperationType + " Not Implemented!");
                }

                //insertToConsole("Event Instance Sim Done");
            }
            
        }

        private void btnStartSimulation_Click(object sender, RoutedEventArgs e)
        {

            if (simulationActive == false)
            {

                if(configFileLoaded == true)
                {
                    insertToConsole("Starting Simulation");
                    // And start it        
                    SimulationTimer.Enabled = true;
                    SimulationTimer.Start();
                    simulationActive = true;
                    btnStartSimulation.Content = "STOP";
                }
                else
                {
                    insertToConsole("Failed to Start - Load Configuration File first!");
                }

            }

            else
            {
                insertToConsole("Stopping Simulation");
                // STOP THE SIMULATION
                SimulationTimer.Stop();
                SimulationTimer.Enabled = false;
                simulationActive = false;
                btnStartSimulation.Content = "START";



            }
            



        }
        
        private class domainObject
        {
            public string userDomain { get; set; }

        }
    

        private void chkboxRead_Click(object sender, RoutedEventArgs e)
        {
            if (chkboxRead.IsChecked.Value)
            {
                SimManager.DoFileReads = true;
            }
            else
            {
                SimManager.DoFileReads = false;
            }
            
        }

        private void chkboxWrites_Click(object sender, RoutedEventArgs e)
        {
            if (chkboxRead.IsChecked.Value)
            {
                SimManager.DoFileWrites = true;
            }
            else
            {
                SimManager.DoFileWrites = false;
            }

        }

        private void chkboxRenames_Click(object sender, RoutedEventArgs e)
        {
            if (chkboxRenames.IsChecked.Value)
            {
                SimManager.DoFileRenames = true;
            }
            else
            {
                SimManager.DoFileRenames = false;
            }

        }


        private void chkboxCreates_Click(object sender, RoutedEventArgs e)
        {
            if (chkboxCreates.IsChecked.Value)
            {
                SimManager.DoFileCreates = true;
            }
            else
            {
                SimManager.DoFileCreates = false;
            }
        }



        private void AddUser_Click(object sender, RoutedEventArgs e)
        {

            //pass to child like this: ChildWindow child= new ChildWindow("abc","somevalue");
            UserWindow subWindow = new UserWindow("","","","new");
            subWindow.Owner = this;

            subWindow.passUserDetailsToParent += returnedCredManager => AddToDgUser(returnedCredManager);
            subWindow.Show();

                       

        }

 
        public void AddToDgUser(CredentialManager UserCreds)
        {
            // Add To Datagrid
            var data = new CredentialManager { userName = UserCreds.userName, userDomain = UserCreds.userDomain };
            dgUsers.Items.Add(data);
            
            // Add to Network Cred list
            NetworkCredentialList.Add(UserCreds.SetupUserCreds(UserCreds.userName, UserCreds.userPassword, UserCreds.userDomain));

            insertToConsole("Added User: " + UserCreds.userDomain + "\\" + UserCreds.userName);
        }


        public void AddToDgDomain(string UserDomain)
        {
            // Add To Datagrid
            var data = new domainObject { userDomain = UserDomain };
           
            dgDomains.Items.Add(data);

            insertToConsole("Added Domain: " + UserDomain);

        }



        public void EditDgUser(CredentialManager UserCreds)
        {

            var currentItem = dgUsers.SelectedItem as CredentialManager;

            //Update Datagrid
            currentItem.userName = UserCreds.userName;
            currentItem.userDomain = UserCreds.userDomain;
            dgUsers.SelectedItem = currentItem;
            this.dgUsers.Items.Refresh();

            // Update NetworkCred List
            var credSearch = NetworkCredentialList.FirstOrDefault(x => x.UserName == currentItem.userName);
            if (credSearch != null)
            {
                credSearch.UserName = UserCreds.userName;
                credSearch.Domain = UserCreds.userDomain;
                credSearch.Password = UserCreds.userPassword;
            }
            
            // Console
            insertToConsole("Modified User: " + UserCreds.userDomain + "\\" + UserCreds.userName);

        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            // Deleted from Grid
            var selectedItem = dgUsers.SelectedItem;
            if (selectedItem != null)
            {
                var currentItem = dgUsers.SelectedItem as CredentialManager;

                // Remove Item From NetworkCred List
                var credSearch = NetworkCredentialList.FirstOrDefault(x => x.UserName == currentItem.userName);
                if (credSearch != null)
                {
                    NetworkCredentialList.Remove(credSearch);
                }

                // Remove from Data Grid
                dgUsers.Items.Remove(selectedItem);
                this.dgUsers.Items.Refresh();
                insertToConsole("Deleted User");
            }

        }

        private void btnModifyUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgUsers.SelectedItem;
            if (selectedItem != null)
            {
                var currentItem = dgUsers.SelectedItem as CredentialManager;
            

                UserWindow subWindow = new UserWindow(currentItem.userName, currentItem.userDomain, "", "edit");
                subWindow.Owner = this;

                subWindow.passUserDetailsToParent += returnedCredManager => EditDgUser(returnedCredManager);
                subWindow.Show();
            }

           
        }

        private void AddDomain_Click(object sender, RoutedEventArgs e)
        {
            //pass to child like this: ChildWindow child= new ChildWindow("abc","somevalue");
            DomainWindow subWindow = new DomainWindow("", "new");
            subWindow.Owner = this;

            subWindow.passDomainDetailsToParent += returnedDomain => AddToDgDomain(returnedDomain);
            subWindow.Show();
        }


        private void btnModifyDomain_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgDomains.SelectedItem;
            if (selectedItem != null)
            {
                var currentItem = dgDomains.SelectedItem as domainObject;

                DomainWindow subWindow = new DomainWindow(currentItem.userDomain, "edit");
                subWindow.Owner = this;

                subWindow.passDomainDetailsToParent += returneddomain => EditDgDomain(returneddomain);
                subWindow.Show();
            }
        }

       
        public void EditDgDomain(string domain)
        {

            var currentItem = dgDomains.SelectedItem as domainObject;

            //Update Datagrid
            currentItem.userDomain = domain;
            dgDomains.SelectedItem = currentItem;
            this.dgDomains.Items.Refresh();

       
            // Console
            insertToConsole("Modified Domain: " + domain);

        }

        private void btnDeleteDomain_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgDomains.SelectedItem;
            if (selectedItem != null)
            {
                var currentItem = dgDomains.SelectedItem as domainObject;

                // Remove from Data Grid
                dgDomains.Items.Remove(selectedItem);
                this.dgDomains.Items.Refresh();
                insertToConsole("Deleted Domain");
            }

        }

        
    }



















}
