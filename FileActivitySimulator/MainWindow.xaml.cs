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
using Quartz;
using Quartz.Impl;
using System.Timers;
using System.Threading.Tasks;


namespace FileActivitySimulator
{
      

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool simulationActive = false;
        public string configFilePath;
        public List<FileSimObject> fileSimulationObjects = new List<FileSimObject>();
        ConsoleContent dc = new ConsoleContent();
        public double simulationActivityDelay = 10000;
        private static System.Timers.Timer SimulationTimer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = dc;
            SetupTimer();

            

        }

        private void SetupTimer()
        {
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

                txtboxParentActivityFolder.Text = path;
                insertToConsole("Selected Parent Folder...");

            }
        }
        private void btnConfigFileLoad_Click(object sender, RoutedEventArgs e)
        {
            // Get Current Config file
            string pathtoConfig = configFilePath;
 
            // read JSON directly from a file
            using (StreamReader file = File.OpenText(pathtoConfig))
            {
                string json = file.ReadToEnd();
                fileSimulationObjects = JsonConvert.DeserializeObject<List<FileSimObject>>(json);
                
            }

            insertToConsole("Loaded Config File...");

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
                insertToConsole("Selected Config File...");
                
            }
        }

        private void sliderActivityLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblActivityslider.Foreground = Brushes.Black;
            if (sliderActivityLevel.Value <= 3)
            {
                lblActivityslider.Content = "Minimal";
                simulationActivityDelay = 10000;


            }
            else if (sliderActivityLevel.Value <6)
            {
                lblActivityslider.Content = "Moderate";
                simulationActivityDelay = 5000;


            }
            else if (sliderActivityLevel.Value < 9)
            {
                lblActivityslider.Content = "High";
                simulationActivityDelay = 1000;


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
            // Load Up File System Operations
            FileSystemOperations FSOps = new FileSystemOperations();
            FSOps.OperationType = FSOps.GetRandomOperationType();
            string RandomOperationType = FSOps.OperationType;

            // Get something from our file system collection
            FileSimObject FSObject = new FileSimObject();
            var random = new Random();
            int index = random.Next(fileSimulationObjects.Count);
            FSObject.name = (fileSimulationObjects[index].name);
            string RandomFileName = FSObject.name;


            insertToConsole(RandomOperationType + " Event for file: " + RandomFileName + " !");
        }

        private void btnStartSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (simulationActive == false)
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
                insertToConsole("Stopping Simulation");
                // STOP THE SIMULATION
                SimulationTimer.Stop();
                SimulationTimer.Enabled = false;
                simulationActive = false;
                btnStartSimulation.Content = "START";



            }
            



        }
    }



















}
