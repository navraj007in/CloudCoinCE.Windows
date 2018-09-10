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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Founders;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using CloudCoinIE;
using Microsoft.Win32;
using CloudCoinClient.CoreClasses;
using CloudCoinCore;
using ConsoleTables;
using System.Windows.Threading;

namespace CloudCoinCE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public EventHandler RefreshCoins;
        public static RAIDA raida;
        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };
        Frack_Fixer fixer;
        public static String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        
        public static int exportOnes = 0;
        public static int exportFives = 0;
        public static int exportTens = 0;
        public static int exportQtrs = 0;
        public static int exportHundreds = 0;
        public static int exportTwoFifties = 0;
        public static int exportJpegStack = 2;
        public static string exportTag = "";

        #region Total Variables
        public static int onesCount = 0;
        public static int fivesCount = 0;
        public static int qtrCount = 0;
        public static int hundredsCount = 0;
        public static int twoFiftiesCount = 0;

        public static int onesFrackedCount = 0;
        public static int fivesFrackedCount = 0;
        public static int qtrFrackedCount = 0;
        public static int hundredsFrackedCount = 0;
        public static int twoFrackedFiftiesCount = 0;

        public static int onesTotalCount = 0;
        public static int fivesTotalCount = 0;
        public static int qtrTotalCount = 0;
        public static int hundredsTotalCount = 0;
        public static int twoFiftiesTotalCount = 0;
        #endregion
        public static int NetworkNumber = 1;
        static FileSystem FS ;
        private bool IsEchoRunning = false;
        public static SimpleLogger logger;
        public MainWindow()
        {
            
            showDisclaimer();
            rootFolder = getWorkspace();

            raida = RAIDA.GetInstance();
            raida.FS =  RAIDA.FileSystem = FS =  new FileSystem(rootFolder);
            FS.CreateDirectories();
            FS.LoadFileSystem();
            logger = new SimpleLogger(FS.LogsFolder + "logs" + DateTime.Now.ToString("yyyyMMdd").ToLower() + ".log", true);
            fixer = new Frack_Fixer(FS, timeout);
            UpdateCELog("");
            printStarLine();
            UpdateCELog("Starting CloudCoin CE at "+ DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));
            printStarLine();
            SetupRAIDA();
            RAIDA.window = this;
            InitializeComponent();
            FS.CreateDirectories();
            //loadJson();
            printWelcome();
            noteOne.NoteCount = "1";
            noteFive.NoteCount = "5";
            noteQtr.NoteCount = "10";
            noteHundred.NoteCount = "100";
            noteTwoFifty.NoteCount = "250";

            raida1.IsActive = true;
            raida2.IsActive = true;
            raida3.IsActive = true;
            raida4.IsActive = true;
            raida5.IsActive = true;
            raida6.IsActive = true;
            raida7.IsActive = true;
            raida8.IsActive = true;
            raida9.IsActive = true;
            raida10.IsActive = true;
            raida11.IsActive = true;
            raida12.IsActive = true;
            raida13.IsActive = true;
            raida14.IsActive = true;
            raida15.IsActive = true;
            raida16.IsActive = true;
            raida17.IsActive = true;
            raida18.IsActive = true;
            raida19.IsActive = true;
            raida20.IsActive = true;
            raida21.IsActive = true;
            raida22.IsActive = true;
            raida23.IsActive = true;
            raida24.IsActive = true;
            raida25.IsActive = true;

            

            
            SetLEDFlashing(true);

            Echo();

            ShowCoins();

            new Thread(delegate () {
                Task.Delay(20000).ContinueWith(t => fix());
                //fix();

            }).Start();

            resumeImport();
        }

        public void SetLEDFlashing(bool LEDStatus)
        {
            raida1.Flashing = LEDStatus;
            raida2.Flashing = LEDStatus;
            raida3.Flashing = LEDStatus;
            raida4.Flashing = LEDStatus;
            raida5.Flashing = LEDStatus;
            raida6.Flashing = LEDStatus;
            raida7.Flashing = LEDStatus;
            raida8.Flashing = LEDStatus;
            raida9.Flashing = LEDStatus;
            raida10.Flashing = LEDStatus;
            raida11.Flashing = LEDStatus;
            raida12.Flashing = LEDStatus;
            raida13.Flashing = LEDStatus;
            raida14.Flashing = LEDStatus;
            raida15.Flashing = LEDStatus;
            raida16.Flashing = LEDStatus;
            raida17.Flashing = LEDStatus;
            raida18.Flashing = LEDStatus;
            raida19.Flashing = LEDStatus;
            raida20.Flashing = LEDStatus;
            raida21.Flashing = LEDStatus;
            raida22.Flashing = LEDStatus;
            raida23.Flashing = LEDStatus;
            raida24.Flashing = LEDStatus;
            raida25.Flashing = LEDStatus;
        }
        public static void SetupRAIDA()
        {
            //RAIDA.FileSystem = new FileSystem(rootFolder);
            try
            {
                RAIDA.Instantiate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            if (RAIDA.networks.Count == 0)
            {
                UpdateCELog("No Valid Network found.Quitting!!");
                Environment.Exit(1);
            }
            else
            {
                UpdateCELog(RAIDA.networks.Count + " Networks found.");
                raida = (from x in RAIDA.networks
                         where x.NetworkNumber == NetworkNumber
                         select x).FirstOrDefault();
                raida.FS = FS;
                RAIDA.ActiveRAIDA = raida;
                if (raida == null)
                {
                    UpdateCELog("Selected Network Number not found. Quitting.");
                    Environment.Exit(0);
                }
                else
                {
                    UpdateCELog("Network Number set to " + NetworkNumber);
                }
            }
        }
        private void Refresh(object sender, EventArgs e)
        {
            ShowCoins();
        }

        public void fix()
        {

            RAIDA raida = RAIDA.GetInstance();
           EchoRaidas(false,false).Wait();

            if (raida.NotReadyCount > 8)
            {
                insufficientRAIDA();
                return;
            }

            UpdateCELog("  Fracked coins present in Fracked folder!Frack Fixing in progress! DO NOT TURN OFF CloudCoinCE application!!!!");
            UpdateCELog("  Fixing fracked coins can take many minutes.");
            UpdateCELog("  If your coins are not completely fixed, fix fracked again.");

            UpdateCELog("  Attempting to fix all fracked coins.");

            //Frack_Fixer fixer = new Frack_Fixer(FS, timeout);
            fixer.FixAll();

        }//end fix

        private void resumeImport()
        {

            int count = Directory.GetFiles(FS.SuspectFolder).Length;
            if (count > 0)
            {
                new Thread(() =>
                {

                    Thread.CurrentThread.IsBackground = true;



                    //EchoRaidas().Wait();
                    raida = RAIDA.GetInstance();
                    
                    if (raida.NotReadyCount > 8)
                    {
                       // insufficientRAIDA();
                        return;
                    }
                    else
                        import();

                    /* run your code here */
                }).Start();

            }
        }

        public void import(int resume = 0)
        {

            //Check RAIDA Status

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(FS.SuspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                updateLog("  Finishing importing coins from last time...");

                Console.ForegroundColor = ConsoleColor.White;
                
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
                updateLog("  Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(FS.ImportFolder);
            updateLog("  Loading all CloudCoins in your import folder: ");
            updateLog(FS.ImportFolder);

          
        }   // end import

    

        private void Detector_OnUpdateStatus(object sender, ProgressEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
              

            });
        }

        private void insufficientRAIDA()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                updateLog("You do not have enough RAIDA to perform an import operation.");
                updateLog("Check to make sure your internet is working.");
                updateLog("Make sure no routers at your work are blocking access to the RAIDA.");
                updateLog("Try to Echo RAIDA and see if the status has changed.");

            });

        }

        public static void UpdateCELog(string LogLine)
        {
            logger.Info(LogLine);
        }
        public void updateLog(string logLine)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                txtLogs.AppendText(logLine + Environment.NewLine);
            });

        }

        public int timeout = 10000;

        public void ShowCoins()
        {

            Console.Out.WriteLine("");
            // This is for consol apps.

            var bankCoins = FS.LoadFolderCoins(FS.BankFolder);


            onesCount = (from x in bankCoins
                         where x.denomination == 1
                         select x).Count();
            fivesCount = (from x in bankCoins
                          where x.denomination == 5
                          select x).Count();
            qtrCount = (from x in bankCoins
                        where x.denomination == 25
                        select x).Count();
            hundredsCount = (from x in bankCoins
                             where x.denomination == 100
                             select x).Count();
            twoFiftiesCount = (from x in bankCoins
                               where x.denomination == 250
                               select x).Count();

            var frackedCoins = FS.LoadFolderCoins(FS.FrackedFolder);
            bankCoins.AddRange(frackedCoins);

            onesFrackedCount = (from x in frackedCoins
                                where x.denomination == 1
                                select x).Count();
            fivesFrackedCount = (from x in frackedCoins
                                 where x.denomination == 5
                                 select x).Count();
            qtrFrackedCount = (from x in frackedCoins
                               where x.denomination == 25
                               select x).Count();
            hundredsFrackedCount = (from x in frackedCoins
                                    where x.denomination == 100
                                    select x).Count();
            twoFrackedFiftiesCount = (from x in frackedCoins
                                      where x.denomination == 250
                                      select x).Count();

            onesTotalCount = onesCount + onesFrackedCount;
            fivesTotalCount = fivesCount + fivesFrackedCount;
            qtrTotalCount = qtrCount + qtrFrackedCount;
            hundredsTotalCount = hundredsCount + hundredsFrackedCount;
            twoFiftiesTotalCount = twoFiftiesCount + twoFrackedFiftiesCount;

            int totalAmount = onesTotalCount + (fivesTotalCount * 5) + (qtrTotalCount * 25) + (hundredsTotalCount * 100) + (twoFiftiesTotalCount * 250);


            setLabelText(lblNotesTotal, Convert.ToString(totalAmount));

            UpdateNotes();
        }// end show

        private void UpdateNotes()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                noteOne.lblNoteCount.Content = Convert.ToString(onesTotalCount);
                noteFive.lblNoteCount.Content = Convert.ToString(fivesTotalCount);
                noteQtr.lblNoteCount.Content = Convert.ToString(qtrTotalCount);
                noteHundred.lblNoteCount.Content = Convert.ToString(hundredsTotalCount);
                noteTwoFifty.lblNoteCount.Content = Convert.ToString(twoFiftiesTotalCount);

				updOne.Max = (onesTotalCount);
				updFive.Max = (fivesTotalCount);
                updQtr.Max = (qtrTotalCount);
                updHundred.Max = (hundredsTotalCount);
                updTwoFifty.Max = (twoFiftiesTotalCount);


            });

        }
        private void SetLabelText(Label lbl, string text)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if (lbl != null)
                    lbl.Content = text;
            });

        }

        private void setLabelText(TextBlock lbl, string text)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if (lbl != null)
                    lbl.Text = "₡ " + text;
            });

        }

        private void Echo()
        {
            SetLEDFlashing(true);

            new Thread(async delegate () {
                if (!IsEchoRunning)
                    await EchoRaidas(true, true);
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate ()
                {
                    SetLEDFlashing(false);
                });
            }).Start();
        }


        /*
         *Shows disclaimer for a first time Run 
         */
        private void showDisclaimer()
        {
            //Properties.Settings.Default["FirstRun"] = false;

            bool firstRun = (bool)Properties.Settings.Default["FirstRun"];
            if (firstRun == true)
            //if(true)
            {
                //First application run
                //Update setting
                Properties.Settings.Default["FirstRun"] = false;
                //Save setting
                Properties.Settings.Default.Save();

                Disclaimer disclaimer = new Disclaimer();
                disclaimer.ShowDialog();

                
            }
            else
            {
                //Not first time of running application.
            }
        }

        private static void printStarLine()
        {
            logger.Info("********************************************************************************");

        }
        private void printWelcome()
        {
            updateLog("CloudCoin Consumers Edition");
            updateLog("Version WinCE-" +  DateTime.Now.ToString("dd-MMM-yyyy") +"-v1.5.0.3");
            updateLog("Used to Authenticate ,Store,Payout CloudCoins.");
            updateLog("This Software is provided as is, with all faults, defects, errors and without warranty of any kind. Free from the CloudCoin Consortium.");

            printStarLine();
            UpdateCELog("                                                                  ");
            UpdateCELog("                   CloudCoin CE Edition                           ");
            UpdateCELog(String.Format("                      Version: {0}                        ", "1.5.0.0 "+DateTime.Now.ToString("dd.MMM.yyyy")));
            UpdateCELog("          Used to Authenticate, Store and Payout CloudCoins       ");
            UpdateCELog("      This Software is provided as is with all faults, defects    ");
            UpdateCELog("          and errors, and without warranty of any kind.           ");
            UpdateCELog("                Free from the CloudCoin Consortium.               ");
            //Console.Out.WriteLine("                            Network Number " + NetworkNumber + "                      ");
            printStarLine();
        }
        private void loadJson()
        {
            string fileName = @"nodes.json";
            TextRange range;
            FileStream fStream;
            if (File.Exists(fileName))
            {
                range = new TextRange(txtLogs.Document.ContentStart, txtLogs.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Text);
                fStream.Close();
            }
        }

        /*
         * Updates LED controls based on the status of failsEcho Array
         * Green for Echo Passed 
         * Red for Echo Failed
         */
        private void updateLEDs()
        {
            raida1.ColorOn = !raida.nodes[0].FailsDetect ? Colors.Green : Colors.Red;
            raida2.ColorOn = !raida.nodes[1].FailsDetect ? Colors.Green : Colors.Red;
            raida3.ColorOn = !raida.nodes[2].FailsDetect ? Colors.Green : Colors.Red;
            raida4.ColorOn = !raida.nodes[3].FailsDetect ? Colors.Green : Colors.Red;
            raida5.ColorOn = !raida.nodes[4].FailsDetect ? Colors.Green : Colors.Red;
            raida6.ColorOn = !raida.nodes[5].FailsDetect ? Colors.Green : Colors.Red;
            raida7.ColorOn = !raida.nodes[6].FailsDetect ? Colors.Green : Colors.Red;
            raida8.ColorOn = !raida.nodes[7].FailsDetect ? Colors.Green : Colors.Red;
            raida9.ColorOn = !raida.nodes[8].FailsDetect ? Colors.Green : Colors.Red;
            raida10.ColorOn = !raida.nodes[9].FailsDetect ? Colors.Green : Colors.Red;
            raida11.ColorOn = !raida.nodes[10].FailsDetect ? Colors.Green : Colors.Red;
            raida12.ColorOn = !raida.nodes[11].FailsDetect ? Colors.Green : Colors.Red;
            raida13.ColorOn = !raida.nodes[12].FailsDetect ? Colors.Green : Colors.Red;
            raida14.ColorOn = !raida.nodes[13].FailsDetect ? Colors.Green : Colors.Red;
            raida15.ColorOn = !raida.nodes[14].FailsDetect ? Colors.Green : Colors.Red;
            raida16.ColorOn = !raida.nodes[15].FailsDetect ? Colors.Green : Colors.Red;
            raida17.ColorOn = !raida.nodes[16].FailsDetect ? Colors.Green : Colors.Red;
            raida18.ColorOn = !raida.nodes[17].FailsDetect ? Colors.Green : Colors.Red;
            raida19.ColorOn = !raida.nodes[18].FailsDetect ? Colors.Green : Colors.Red;
            raida20.ColorOn = !raida.nodes[19].FailsDetect ? Colors.Green : Colors.Red;
            raida21.ColorOn = !raida.nodes[20].FailsDetect ? Colors.Green : Colors.Red;
            raida22.ColorOn = !raida.nodes[21].FailsDetect ? Colors.Green : Colors.Red;
            raida23.ColorOn = !raida.nodes[22].FailsDetect ? Colors.Green : Colors.Red;
            raida24.ColorOn = !raida.nodes[23].FailsDetect ? Colors.Green : Colors.Red;
            raida25.ColorOn = !raida.nodes[24].FailsDetect ? Colors.Green : Colors.Red;

            SetLEDFlashing(false);


        }

        public async Task EchoRaidas(bool scanAll = false,bool silent = false)
        {
            IsEchoRunning = true;
            var networks = (from x in RAIDA.networks
                            select x).Distinct().ToList();
            if (silent)
            {
                updateLog("----------------------------------");
                updateLog(String.Format("Starting Echo to RAIDA"));
                updateLog("----------------------------------");

                UpdateCELog("----------------------------------");
                UpdateCELog(String.Format("Starting Echo to RAIDA"));
                UpdateCELog("----------------------------------");

            }
            var echos = RAIDA.GetInstance().GetEchoTasks();
            raida = RAIDA.GetInstance();

            await Task.WhenAll(echos.AsParallel().Select(async task => await task()));
            if (silent)
            {
                updateLog("Ready Count: " + raida.ReadyCount);
                updateLog("Not Ready Count: " + raida.NotReadyCount);
                updateLog("-----------------------------------");

                UpdateCELog("Ready Count: " + raida.ReadyCount);
                UpdateCELog("Not Ready Count: " + raida.NotReadyCount);
                UpdateCELog("-----------------------------------");

            }

            App.Current.Dispatcher.Invoke(delegate
            {
                updateLEDs();
            });

            

            IsEchoRunning = false;
        }

        private void cmdBackup_Click(object sender, RoutedEventArgs e)
        {
            UpdateCELog("  User Input : Backup");
            printStarLine();
            backup();
            printStarLine();
        }

        public void Backup(string TargetLocation)
        {
            if (Directory.Exists(TargetLocation))
            {
                var bankCoins = FS.LoadFolderCoins(FS.BankFolder);
                bankCoins.AddRange(FS.LoadFolderCoins(FS.FrackedFolder));
                if (bankCoins.Count() == 0)
                {
                    updateLog("No Coins available for backup.");
                    UpdateCELog("No Coins available for backup.");
                    MessageBox.Show("No Coins available for backup.");
                    return;
                }
                string FileName = TargetLocation + System.IO.Path.DirectorySeparatorChar + "CloudCoinsBackup" + DateTime.Now.ToString("yyyyMMddhhmmss").ToLower();
                FS.WriteCoinsToFile(bankCoins, FileName, ".stack");
                MessageBox.Show("Backup completed successfully.");
                UpdateCELog("CloudCoins Backed up to " + FileName +" successful");
            }
            else
            {
                UpdateCELog("The target location does not exist.");
            }
        }
        private void backup()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Backup(dialog.SelectedPath);
                    
                }
            }

        }

        private void cmdShowFolders_Click(object sender, RoutedEventArgs e)
        {
            UpdateCELog("  User Input : Show Folders");

            Process.Start(FS.RootPath);
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            ShowCoins();
            UpdateCELog("  User Input : Echo RAIDA");
            SetLEDFlashing(true);
          
            new Thread(async delegate () {
                //Echo();
                if(!IsEchoRunning)
                await EchoRaidas(true,true);

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate ()
                 {
                     SetLEDFlashing(false);
                 });
            }).Start();

          

            //worker.RunWorkerAsync();
        }

        
        public string getWorkspace()
        {
            string workspace = "";
            if (Properties.Settings.Default.WorkSpace != null && Properties.Settings.Default.WorkSpace.Length > 0)
                workspace = Properties.Settings.Default.WorkSpace;
            else
                workspace = AppDomain.CurrentDomain.BaseDirectory;
            Properties.Settings.Default.WorkSpace = workspace;
            return workspace;
        }

        private void cmdChangeWorkspace_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string sMessageBoxText = "Do you want to Change CloudCoin Folder?";
                    string sCaption = "Change Directory";

                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            /* ... */
                            // lblDirectory.Text = dialog.SelectedPath;
                            Properties.Settings.Default.WorkSpace = dialog.SelectedPath + System.IO.Path.DirectorySeparatorChar;
                            Properties.Settings.Default.Save();
                            FileSystem fileUtils = new FileSystem(Properties.Settings.Default.WorkSpace);
                            fileUtils.CreateDirectories();
                            string[] fileNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                            foreach (String fileName in fileNames)
                            {
                                if (fileName.Contains("jpeg") || fileName.Contains("jpg"))
                                {
                                    try
                                    {
                                        string outputpath = Properties.Settings.Default.WorkSpace + "Templates" + System.IO.Path.DirectorySeparatorChar + fileName.Substring(22);
                                        updateLog(outputpath);
                                        using (FileStream fileStream = File.Create(outputpath))
                                        {
                                            Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName).CopyTo(fileStream);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            break;

                        case MessageBoxResult.No:
                            /* ... */
                            break;

                        case MessageBoxResult.Cancel:
                            /* ... */
                            break;
                    }
                }
            }

        }

        private void cmdPown_Click(object sender, RoutedEventArgs e)
        {
            UpdateCELog("  User Input : Deposit");
            printStarLine();
            updateLog("Starting CloudCoin Import.");
            updateLog("  Please do not close the CloudCoin CE program until it is finished.");
            updateLog("  Otherwise it may result in loss of CloudCoins.");
            printStarLine();

            int count = FS.LoadFolderCoins(FS.ImportFolder).Count();

            if (count == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Cloudcoins (*.stack, *.jpg,*.jpeg)|*.stack;*.jpg;*.jpeg|Stack files (*.stack)|*.stack|Jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = FS.ImportFolder.Substring(0,FS.ImportFolder.Length-1).Replace("\\\\","\\");
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        try
                        {
                            if (!File.Exists(FS.ImportFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename)))
                                File.Move(filename, FS.ImportFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
                            else
                            {
                                string msg = "File " + filename + " already exists. Do you want to overwrite it?";
                                MessageBoxResult result =
                                  MessageBox.Show(
                                    msg,
                                    "CloudCoins",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning);
                                if (result == MessageBoxResult.Yes)
                                {
                                    try
                                    {
                                        File.Delete(FS.ImportFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
                                        File.Move(filename, FS.ImportFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            updateLog(ex.Message);
                        }
                    }
                }
                else
                    return;
            }

            if (raida.NotReadyCount > 8)
            {
                insufficientRAIDA();

                return;
            }

            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                fixer.continueExecution = false;
                UpdateCELog("Starting Detect..");
                printStarLine();
                await RAIDA.ProcessCoins(true);
                ShowCoins();
            }).Start();
        }

        public static void CalculateTotals()
        {
            var bankCoins = FS.LoadFolderCoins(FS.BankFolder);


            onesCount = (from x in bankCoins
                         where x.denomination == 1
                         select x).Count();
            fivesCount = (from x in bankCoins
                          where x.denomination == 5
                          select x).Count();
            qtrCount = (from x in bankCoins
                        where x.denomination == 25
                        select x).Count();
            hundredsCount = (from x in bankCoins
                             where x.denomination == 100
                             select x).Count();
            twoFiftiesCount = (from x in bankCoins
                               where x.denomination == 250
                               select x).Count();

            var frackedCoins = FS.LoadFolderCoins(FS.FrackedFolder);
            bankCoins.AddRange(frackedCoins);

            onesFrackedCount = (from x in frackedCoins
                                where x.denomination == 1
                                select x).Count();
            fivesFrackedCount = (from x in frackedCoins
                                 where x.denomination == 5
                                 select x).Count();
            qtrFrackedCount = (from x in frackedCoins
                               where x.denomination == 25
                               select x).Count();
            hundredsFrackedCount = (from x in frackedCoins
                                    where x.denomination == 100
                                    select x).Count();
            twoFrackedFiftiesCount = (from x in frackedCoins
                                      where x.denomination == 250
                                      select x).Count();

            onesTotalCount = onesCount + onesFrackedCount;
            fivesTotalCount = fivesCount + fivesFrackedCount;
            qtrTotalCount = qtrCount + qtrFrackedCount;
            hundredsTotalCount = hundredsCount + hundredsFrackedCount;
            twoFiftiesTotalCount = twoFiftiesCount + twoFrackedFiftiesCount;

        }
        public void export()
        {
            UpdateCELog("  User Input : Withdraw");
            printStarLine();
            UpdateCELog("Starting CloudCoin Export.");
            UpdateCELog("  Please do not close the CloudCoin CE program until it is finished.");
            UpdateCELog("  Otherwise it may result in loss of CloudCoins.");
            printStarLine();

            if (rdbJpeg.IsChecked == true)
                exportJpegStack = 1;
            else
                exportJpegStack = 2;


            //FS.LoadFileSystem();
            CalculateTotals();
            
            int exp_1 = Convert.ToInt16(updOne.Value);
            int exp_5 = Convert.ToInt16(updFive.Value);
            int exp_25 = Convert.ToInt16(updQtr.Value);
            int exp_100 = Convert.ToInt16(updHundred.Value);
            int exp_250 = Convert.ToInt16(updTwoFifty.Value);
            //Warn if too many coins

            if (exp_1 + exp_5 + exp_25 + exp_100 + exp_250 == 0)
            {
                MessageBox.Show("Can not export 0 Coins.","Export CloudCoins");
                UpdateCELog("Can not export 0 coins");
                return;
            }

            int file_type = 0; 
            file_type = exportJpegStack;

            String tag = txtTag.Text;// reader.readString();
            int totalSaved = exp_1 + (exp_5 * 5) + (exp_25 * 25) + (exp_100 * 100) + (exp_250 * 250);
            List<CloudCoin> totalCoins = IFileSystem.bankCoins.ToList();
            totalCoins.AddRange(IFileSystem.frackedCoins);


            var onesToExport = (from x in totalCoins
                                where x.denomination == 1
                                select x).Take(exp_1);
            var fivesToExport = (from x in totalCoins
                                 where x.denomination == 5
                                 select x).Take(exp_5);
            var qtrToExport = (from x in totalCoins
                               where x.denomination == 25
                               select x).Take(exp_25);
            var hundredsToExport = (from x in totalCoins
                                    where x.denomination == 100
                                    select x).Take(exp_100);
            var twoFiftiesToExport = (from x in totalCoins
                                      where x.denomination == 250
                                      select x).Take(exp_250);
            List<CloudCoin> exportCoins = onesToExport.ToList();
            exportCoins.AddRange(fivesToExport);
            exportCoins.AddRange(qtrToExport);
            exportCoins.AddRange(hundredsToExport);
            exportCoins.AddRange(twoFiftiesToExport);

            exportCoins.ForEach(x => x.pan = null);

            if (file_type == 1)
            {
                String filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + "");
                if (File.Exists(filename))
                {
                    // tack on a random number if a file already exists with the same tag
                    Random rnd = new Random();
                    int tagrand = rnd.Next(999);
                    filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + tagrand + "");
                }//end if file exists

                foreach (var coin in exportCoins)
                {
                    string OutputFile = FS.ExportFolder + coin.FileName + tag + ".jpg";
                    bool fileGenerated = FS.WriteCoinToJpeg(coin, FS.GetCoinTemplate(coin), OutputFile, "");
                    if (fileGenerated)
                    {
                        updateLog("CloudCoin exported as Jpeg to " + OutputFile);
                        UpdateCELog("CloudCoin exported as Jpeg to " + OutputFile);

                        Console.WriteLine("CloudCoin exported as Jpeg to " + OutputFile);
                        printStarLine();
                    }
                }

                FS.RemoveCoins(exportCoins, FS.BankFolder);
                FS.RemoveCoins(exportCoins, FS.FrackedFolder);
            }

            // Export Coins as Stack
            if (file_type == 2)
            {
                int stack_type = 1;
                if (stack_type == 1)
                {
                    String filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + "");
                    if (File.Exists(filename))
                    {
                        // tack on a random number if a file already exists with the same tag
                        Random rnd = new Random();
                        int tagrand = rnd.Next(999);
                        filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + tagrand + "");
                    }//end if file exists

                    FS.WriteCoinsToFile(exportCoins, filename, ".stack");
                    updateLog("Coins exported as stack file to " + filename);
                    UpdateCELog("Coins exported as stack file to " + filename);
                    printStarLine();
                    FS.RemoveCoins(exportCoins, FS.BankFolder);
                    FS.RemoveCoins(exportCoins, FS.FrackedFolder);
                }
                else
                {
                    foreach (var coin in exportCoins)
                    {
                        string OutputFile = FS.ExportFolder + coin.FileName + tag + ".stack";
                        FS.WriteCoinToFile(coin, OutputFile);

                        FS.RemoveCoins(exportCoins, FS.BankFolder);
                        FS.RemoveCoins(exportCoins, FS.FrackedFolder);
                        UpdateCELog("CloudCoin exported as Stack to " + OutputFile);

                        printStarLine();
                        updateLog("CloudCoin exported as Stack to " + OutputFile);
                    }

                }
            }
            // end if type jpge or stack

            //RefreshCoins?.Invoke(this, new EventArgs());
            //updateLog("Exporting CloudCoins Completed.");
            ShowCoins();
            Process.Start(FS.ExportFolder);
            cmdExport.Content = "₡0";
            txtTag.Text = "";
            updOne.Value = 0;
            updFive.Value = 0;
            updQtr.Value = 0;
            updHundred.Value = 0;
            updTwoFifty.Value = 0;

            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One


        private void Exporter_OnUpdateStatus(object sender, ProgressEventArgs e)
        {

        }

        private void txtLogs_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLogs.ScrollToEnd();
        }

        private void updOne_OnExportChanged(object sender, EventArgs e)
        {
            updateExportTotal();
        }
        int exportTotal = 0;
        public void updateExportTotal()
        {
            exportTotal = updOne.val + (updFive.val *5) + (updQtr.val*25) + (updHundred.val*100) 
                + (updTwoFifty.val*250);
			cmdExport.Content = exportTotal.ToString();

			//lblExportTotal.Text = exportTotal.ToString();
            cmdExport.Content = "₡ " + exportTotal.ToString();
            
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {

            string sMessageBoxText = "Are you sure you want to withdraw CloudCoins?";
            string sCaption = "Withdraw CloudCoins";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            FS.LoadFileSystemForExport();
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    export();
                    break;
            }
        }

        private void cmdWorkspace_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Do you want to Change CloudCoin Workspace? This will change the working directory and you will see 0 coins. Your coins will not be lost however. You will be able to see them again when you revert to old workspace.";
            string sCaption = "Change Workspace";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    
                            /* ... */
                            // lblDirectory.Text = dialog.SelectedPath;
                            UpdateCELog("  User Input : Change Workspace");

                            Properties.Settings.Default.WorkSpace = dialog.SelectedPath + System.IO.Path.DirectorySeparatorChar;
                            Properties.Settings.Default.Save();
                            printStarLine();
                            UpdateCELog("Workspace Changed to " + dialog.SelectedPath);
                            printStarLine();
                            FileSystem fileUtils = new FileSystem(Properties.Settings.Default.WorkSpace);
                            fileUtils.CreateDirectories();
                            string[] fileNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                            foreach (String fileName in fileNames)
                            {
                                if (fileName.Contains("jpeg"))
                                {
                                    try
                                    {
                                        string outputpath = Properties.Settings.Default.WorkSpace + "Templates" + System.IO.Path.DirectorySeparatorChar + fileName.Substring(22);
                                        using (FileStream fileStream = File.Create(outputpath))
                                        {
                                            Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName).CopyTo(fileStream);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            
                }
            }
                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;

                case MessageBoxResult.Cancel:
                    /* ... */
                    break;
            }

        }
    }
    public static class MyExtensions
    {
        public static string ToCustomString(this TimeSpan span)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }
    }

}
