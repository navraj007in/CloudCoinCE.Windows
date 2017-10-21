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

namespace CloudCoinCE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public EventHandler RefreshCoins;

        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };

        public static String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static String importFolder = rootFolder + "Import" + System.IO.Path.DirectorySeparatorChar;
        public static String importedFolder = rootFolder + "Imported" + System.IO.Path.DirectorySeparatorChar;
        public static String trashFolder = rootFolder + "Trash" + System.IO.Path.DirectorySeparatorChar;
        public static String suspectFolder = rootFolder + "Suspect" + System.IO.Path.DirectorySeparatorChar;
        public static String frackedFolder = rootFolder + "Fracked" + System.IO.Path.DirectorySeparatorChar;
        public static String bankFolder = rootFolder + "Bank" + System.IO.Path.DirectorySeparatorChar;
        public static String templateFolder = rootFolder + "Templates" + System.IO.Path.DirectorySeparatorChar;
        public static String counterfeitFolder = rootFolder + "Counterfeit" + System.IO.Path.DirectorySeparatorChar;
        public static String directoryFolder = rootFolder + "Directory" + System.IO.Path.DirectorySeparatorChar;
        public static String exportFolder = rootFolder + "Export" + System.IO.Path.DirectorySeparatorChar;
        public static String languageFolder = rootFolder + "Language" + System.IO.Path.DirectorySeparatorChar;
        public static String partialFolder = rootFolder + "Partial" + System.IO.Path.DirectorySeparatorChar;
        public static String detectedFolder = rootFolder + "Detected" + System.IO.Path.DirectorySeparatorChar;
        public static String logsFolder = rootFolder + "Logs" + System.IO.Path.DirectorySeparatorChar;

        public static int exportOnes = 0;
        public static int exportFives = 0;
        public static int exportTens = 0;
        public static int exportQtrs = 0;
        public static int exportHundreds = 0;
        public static int exportTwoFifties = 0;
        public static int exportJpegStack = 2;
        public static string exportTag = "";

        FileUtils fileUtils = FileUtils.GetInstance(rootFolder);

        public MainWindow()
        {
            showDisclaimer();
            setupFolders();

            InitializeComponent();
            fileUtils.CreateDirectoryStructure();
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

            raida1.Flashing = true;
            raida2.Flashing = true;
            raida3.Flashing = true;
            raida4.Flashing = true;
            raida5.Flashing = true;
            raida6.Flashing = true;
            raida7.Flashing = true;
            raida8.Flashing = true;
            raida9.Flashing = true;
            raida10.Flashing = true;
            raida11.Flashing = true;
            raida12.Flashing = true;
            raida13.Flashing = true;
            raida14.Flashing = true;
            raida15.Flashing = true;
            raida16.Flashing = true;
            raida17.Flashing = true;
            raida18.Flashing = true;
            raida19.Flashing = true;
            raida20.Flashing = true;
            raida21.Flashing = true;
            raida22.Flashing = true;
            raida23.Flashing = true;
            raida24.Flashing = true;
            raida25.Flashing = true;

            //new Thread(delegate () {
            //    echoRaida();
            //}).Start();

            worker.DoWork += Worker_DoWork; ;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted; ;
            worker.RunWorkerAsync();

            showCoins();
            resumeImport();
        }

        private void Refresh(object sender, EventArgs e)
        {
            showCoins();
        }

        private void resumeImport()
        {

            int count = Directory.GetFiles(MainWindow.suspectFolder).Length;
            if (count > 0)
            {
                new Thread(() =>
                {

                    Thread.CurrentThread.IsBackground = true;

                    echoRaida();

                    int totalRAIDABad = 0;
                    for (int i = 0; i < 25; i++)
                    {
                        if (RAIDA_Status.failsEcho[i])
                        {
                            totalRAIDABad += 1;
                        }
                    }
                    if (totalRAIDABad > 8)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You do not have enough RAIDA to perform an import operation.");
                        Console.Out.WriteLine("Check to make sure your internet is working.");
                        Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                        Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                        Console.ForegroundColor = ConsoleColor.White;

                        insufficientRAIDA();
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
            String[] suspectFileNames = new DirectoryInfo(fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                updateLog("  Finishing importing coins from last time...");

                Console.ForegroundColor = ConsoleColor.White;
                multi_detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
                updateLog("  Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(fileUtils.importFolder);
            updateLog("  Loading all CloudCoins in your import folder: ");
            updateLog(fileUtils.importFolder);

            Console.ForegroundColor = ConsoleColor.White;
            Importer importer = new Importer(fileUtils);
            if (!importer.importAll() && resume == 0)//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                updateLog("No coins in import Folder");

                Console.ForegroundColor = ConsoleColor.White;
                App.Current.Dispatcher.Invoke(delegate
                {

                    //cmdRestore.IsEnabled = true;
                    //cmdImport.IsEnabled = true;
                });

            }
            else
            {
                DateTime before = DateTime.Now;
                DateTime after;
                TimeSpan ts = new TimeSpan();
                //Console.Out.WriteLine("  IMPORT DONE> NOW DETECTING MULTI. Do you want to start detecting?");// "No coins in import folder.");
                // Console.In.ReadLine();
                multi_detect();
                // Console.Out.WriteLine("  DETCATION DONE> NOW GRADING. Do you want to start Grading?");// "No coins in import folder.");
                // Console.In.ReadLine();
                after = DateTime.Now;
                ts = after.Subtract(before);//end the timer

                grade();
                // Console.Out.WriteLine("  GRADING DONE NOW SHOWING. Do you wnat to show");// "No coins in import folder.");
                // Console.In.ReadLine();
                Console.Out.WriteLine("Time in ms to multi detect pown " + ts.TotalMilliseconds);
                RAIDA_Status.showMultiMs();
                showCoins();
                // multi_detect();
                //detect(1);
            }//end if coins to import
        }   // end import

        public void detect(int resume = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            updateLog("  Detecting Authentication of Suspect Coins");

            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);

            detector.OnUpdateStatus += Detector_OnUpdateStatus; ;
            detector.txtLogs = txtLogs;
            int[] detectionResults = detector.detectAll();
            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            //Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
            updateLog("  Total imported to bank: " + detectionResults[0]);
            //updateLog("  Total imported to fracked: " + detectionResults[2]);
            // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            updateLog("  Total Counterfeit: " + detectionResults[1]);
            updateLog("  Total Kept in suspect folder: " + detectionResults[3]);
            updateLog("  Total Notes imported to Bank: " + detector.totalImported);

            //            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
            updateLog("Time to import " + detectionResults[0] + " Coins: " + stopwatch.Elapsed.ToCustomString() + "");

            string messageBoxText = "Finished Importing Coins.";
            string caption = "Coins";
            RefreshCoins?.Invoke(this, new EventArgs());

            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            if (resume > 0)
                MessageBox.Show(messageBoxText, caption, button, icon);
            App.Current.Dispatcher.Invoke(delegate
            {
                //cmdRestore.IsEnabled = true;
                //cmdImport.IsEnabled = true;
                //progressBar.Value = 100;
                showCoins();
            });
        }//end detect

        private void Detector_OnUpdateStatus(object sender, ProgressEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                //progressBar.Value = e.percentage;
                //if (e.percentage > 0)
                //   lblStatus.Content = String.Format("{0} % of Coins Scanned.", Convert.ToString(e.percentage));

            });
        }

        private void insufficientRAIDA()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                txtLogs.AppendText("You do not have enough RAIDA to perform an import operation.");
                txtLogs.AppendText("Check to make sure your internet is working.");
                txtLogs.AppendText("Make sure no routers at your work are blocking access to the RAIDA.");
                txtLogs.AppendText("Try to Echo RAIDA and see if the status has changed.");


                //cmdImport.IsEnabled = true;
                //cmdRestore.IsEnabled = true;
            });

        }
        private void updateLog(string logLine)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                txtLogs.AppendText(logLine + Environment.NewLine);
            });

        }


        int[] bankTotals;
        int[] frackedTotals;
        int[] partialTotals;
        public int timeout = 5000;

        public void showCoins()
        {

            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(fileUtils);
            bankTotals = bank.countCoins(fileUtils.bankFolder);
            frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            partialTotals = bank.countCoins(fileUtils.partialFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //setLabelText(lblOnesCount, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            //setLabelText(lblFivesCount, Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]));
            //setLabelText(lblQtrCount, Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]));
            //setLabelText(lblHundredCount, Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]));
            //setLabelText(lblTwoFiftiesCount, Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]));

            //setLabelText(lblOnesValue, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            //setLabelText(lblFivesValue, Convert.ToString((bankTotals[2] + frackedTotals[2] + partialTotals[2]) * 5));
            //setLabelText(lblQtrValue, Convert.ToString((bankTotals[3] + frackedTotals[3] + partialTotals[3]) * 25));
            //setLabelText(lblHundredValue, Convert.ToString((bankTotals[4] + frackedTotals[4] + partialTotals[4]) * 100));
            //setLabelText(lblTwoFiftiesValue, Convert.ToString((bankTotals[5] + frackedTotals[5] + partialTotals[5]) * 250));
            //setLabelText(lblTotalCoins, "Total Coins in Bank : " + Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]));

            setLabelText(lblNotesTotal, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]
                + bankTotals[2] + frackedTotals[2] + partialTotals[2]
                + bankTotals[3] + frackedTotals[3] + partialTotals[3]
                + bankTotals[4] + frackedTotals[4] + partialTotals[4]
                + bankTotals[5] + frackedTotals[5] + partialTotals[5]));

            setLabelText(lblNotesTotal, Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]));
            updateNotes();
        }// end show

        private void updateNotes()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                noteOne.lblNoteCount.Content = Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]);
                noteFive.lblNoteCount.Content = Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]);
                noteQtr.lblNoteCount.Content = Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]);
                noteHundred.lblNoteCount.Content = Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]);
                noteTwoFifty.lblNoteCount.Content = Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]);

				updOne.Max = (bankTotals[1] + frackedTotals[1] + partialTotals[1]);
				updFive.Max = (bankTotals[2] + frackedTotals[2] + partialTotals[2]);
                updQtr.Max = (bankTotals[3] + frackedTotals[3] + partialTotals[3]);
                updHundred.Max = (bankTotals[4] + frackedTotals[4] + partialTotals[4]);
                updTwoFifty.Max = (bankTotals[5] + frackedTotals[5] + partialTotals[5]);


            });

        }
        private void setLabelText(Label lbl, string text)
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


        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            raida1.Flashing = false;
            raida2.Flashing = false;
            raida3.Flashing = false;
            raida4.Flashing = false;
            raida5.Flashing = false;
            raida6.Flashing = false;
            raida7.Flashing = false;
            raida8.Flashing = false;
            raida9.Flashing = false;
            raida10.Flashing = false;
            raida11.Flashing = false;
            raida12.Flashing = false;
            raida13.Flashing = false;
            raida14.Flashing = false;
            raida15.Flashing = false;
            raida16.Flashing = false;
            raida17.Flashing = false;
            raida18.Flashing = false;
            raida19.Flashing = false;
            raida20.Flashing = false;
            raida21.Flashing = false;
            raida22.Flashing = false;
            raida23.Flashing = false;
            raida24.Flashing = false;
            raida25.Flashing = false;

        }

        /*
         *Shows disclaimer for a first time Run 
         */
        private void showDisclaimer()
        {
            //Properties.Settings.Default["FirstRun"] = false;

            bool firstRun = (bool)Properties.Settings.Default["FirstRun"];
            if (firstRun == false)
            {
                //First application run
                //Update setting
                Properties.Settings.Default["FirstRun"] = true;
                //Save setting
                Properties.Settings.Default.Save();

                Disclaimer disclaimer = new Disclaimer();
                disclaimer.ShowDialog();

                //Create new instance of Dialog you want to show
                //FirstDialogForm fdf = new FirstDialogForm();
                //Show the dialog
                //fdf.ShowDialog();
            }
            else
            {
                //Not first time of running application.
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            echoRaida();
        }

        private void printWelcome()
        {
            updateLog("CloudCoin Consumers Edition");
            updateLog("Version " + DateTime.Now.ToShortDateString());
            updateLog("Used to Authenticate ,Store,Payout CloudCoins");
            updateLog("This Software is provided as is with all faults, defects and errors, and without warranty of any kind.Free from the CloudCoin Consortium.");
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
        private void updateLEDs(bool[] failsEcho)
        {
            raida1.ColorOn = !failsEcho[0] ? Colors.Green : Colors.Red;
            raida2.ColorOn = !failsEcho[1] ? Colors.Green : Colors.Red;
            raida3.ColorOn = !failsEcho[2] ? Colors.Green : Colors.Red;
            raida4.ColorOn = !failsEcho[3] ? Colors.Green : Colors.Red;
            raida5.ColorOn = !failsEcho[4] ? Colors.Green : Colors.Red;
            raida6.ColorOn = !failsEcho[5] ? Colors.Green : Colors.Red;
            raida7.ColorOn = !failsEcho[6] ? Colors.Green : Colors.Red;
            raida8.ColorOn = !failsEcho[7] ? Colors.Green : Colors.Red;
            raida9.ColorOn = !failsEcho[8] ? Colors.Green : Colors.Red;
            raida10.ColorOn = !failsEcho[9] ? Colors.Green : Colors.Red;
            raida11.ColorOn = !failsEcho[10] ? Colors.Green : Colors.Red;
            raida12.ColorOn = !failsEcho[11] ? Colors.Green : Colors.Red;
            raida13.ColorOn = !failsEcho[12] ? Colors.Green : Colors.Red;
            raida14.ColorOn = !failsEcho[13] ? Colors.Green : Colors.Red;
            raida15.ColorOn = !failsEcho[14] ? Colors.Green : Colors.Red;
            raida16.ColorOn = !failsEcho[15] ? Colors.Green : Colors.Red;
            raida17.ColorOn = !failsEcho[16] ? Colors.Green : Colors.Red;
            raida18.ColorOn = !failsEcho[17] ? Colors.Green : Colors.Red;
            raida19.ColorOn = !failsEcho[18] ? Colors.Green : Colors.Red;
            raida20.ColorOn = !failsEcho[19] ? Colors.Green : Colors.Red;
            raida21.ColorOn = !failsEcho[20] ? Colors.Green : Colors.Red;
            raida22.ColorOn = !failsEcho[21] ? Colors.Green : Colors.Red;
            raida23.ColorOn = !failsEcho[22] ? Colors.Green : Colors.Red;
            raida24.ColorOn = !failsEcho[23] ? Colors.Green : Colors.Red;
            raida25.ColorOn = !failsEcho[24] ? Colors.Green : Colors.Red;


        }
        public bool echoRaida()
        {


            RAIDA_Status.resetEcho();
            RAIDA raida1 = new RAIDA();
            Response[] results = raida1.echoAll(5000);
            int totalReady = 0;
            Console.Out.WriteLine("");
            //For every RAIDA check its results
            int longestCountryName = 15;

            Console.Out.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                int padding = longestCountryName - countries[i].Length;
                string strPad = "";
                for (int j = 0; j < padding; j++)
                {
                    strPad += " ";
                }//end for padding
                if (RAIDA_Status.failsEcho[i])
                {
                    Console.Out.Write(strPad + countries[i] + "N");
                }
                else
                {
                    Console.Out.Write(strPad + countries[i] + "Y");
                }
                //if (RAIDA_Status.failsEcho[i])
                //    raidas[i].Background = Brushes.Red;
                //else
                //    raidas[i].Background = Brushes.Green;

                if (i == 4 || i == 9 || i == 14 || i == 19) { Console.WriteLine(); }
            }//end for

            totalReady = RAIDA_Status.failsEcho.Where(c => !c).Count();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.Write("---  RAIDA Health: " + totalReady + " / 25: ----");//"RAIDA Health: " + totalReady );
            this.Dispatcher.Invoke(() => {
                updateLEDs(RAIDA_Status.failsEcho);
            });
            if (totalReady < 16)//
            {
                return false;
            }
            else
            {
                return true;
            }
            // Running on the UI thread
            //                cmdRefresh.IsEnabled = true;

            //Check if enough are good 
        }//End echo


        public void export(string backupDir)
        {


            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);

            //updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = bankTotals[1] + frackedTotals[1] + partialTotals[1];
            int exp_5 = bankTotals[2] + frackedTotals[2] + partialTotals[2];
            int exp_25 = bankTotals[3] + frackedTotals[3] + partialTotals[3];
            int exp_100 = bankTotals[4] + frackedTotals[4] + partialTotals[4];
            int exp_250 = bankTotals[5] + frackedTotals[5] + partialTotals[5];
            //Warn if too many coins

            if (exp_1 + exp_5 + exp_25 + exp_100 + exp_250 == 0)
            {
                Console.WriteLine("Can not export 0 coins");
                return;
            }

            //updateLog(Convert.ToString(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5] + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]));

            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5]) + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]) > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                Console.Out.WriteLine("Do not export stack files with more than 1000 notes. .");
                //updateLog("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                //updateLog("Do not export stack files with more than 1000 notes. .");

                Console.ForegroundColor = ConsoleColor.White;
            }//end if they have more than 1000 coins

            Console.Out.WriteLine("  Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");
            Exporter exporter = new Exporter(fileUtils);

            String tag = "backup";// reader.readString();
                                  //Console.Out.WriteLine(("Exporting to:" + exportFolder));

            exporter.writeJSONFile(exp_1, exp_5, exp_25, exp_100, exp_250, tag, 1, backupDir);


            // end if type jpge or stack




            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One


        private void cmdBackup_Click(object sender, RoutedEventArgs e)
        {
            backup();
        }
        private void backup()
        {
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);


            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    export(dialog.SelectedPath);
                    //copyFolders(dialog.SelectedPath);
                    MessageBox.Show("Backup completed successfully.");
                }
            }

        }

        private void cmdShowFolders_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(MainWindow.rootFolder);
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                raida1.Flashing = true;
                raida2.Flashing = true;
                raida3.Flashing = true;
                raida4.Flashing = true;
                raida5.Flashing = true;
                raida6.Flashing = true;
                raida7.Flashing = true;
                raida8.Flashing = true;
                raida9.Flashing = true;
                raida10.Flashing = true;
                raida11.Flashing = true;
                raida12.Flashing = true;
                raida13.Flashing = true;
                raida14.Flashing = true;
                raida15.Flashing = true;
                raida16.Flashing = true;
                raida17.Flashing = true;
                raida18.Flashing = true;
                raida19.Flashing = true;
                raida20.Flashing = true;
                raida21.Flashing = true;
                raida22.Flashing = true;
                raida23.Flashing = true;
                raida24.Flashing = true;
                raida25.Flashing = true;

                worker.DoWork += Worker_DoWork; ;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted; ;
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setupFolders()
        {
            rootFolder = getWorkspace();

            importFolder = rootFolder + "Import" + System.IO.Path.DirectorySeparatorChar;
            importedFolder = rootFolder + "Imported" + System.IO.Path.DirectorySeparatorChar;
            trashFolder = rootFolder + "Trash" + System.IO.Path.DirectorySeparatorChar;
            suspectFolder = rootFolder + "Suspect" + System.IO.Path.DirectorySeparatorChar;
            frackedFolder = rootFolder + "Fracked" + System.IO.Path.DirectorySeparatorChar;
            bankFolder = rootFolder + "Bank" + System.IO.Path.DirectorySeparatorChar;
            templateFolder = rootFolder + "Templates" + System.IO.Path.DirectorySeparatorChar;
            counterfeitFolder = rootFolder + "Counterfeit" + System.IO.Path.DirectorySeparatorChar;
            directoryFolder = rootFolder + "Directory" + System.IO.Path.DirectorySeparatorChar;
            exportFolder = rootFolder + "Export" + System.IO.Path.DirectorySeparatorChar;
            languageFolder = rootFolder + "Language" + System.IO.Path.DirectorySeparatorChar;
            partialFolder = rootFolder + "Partial" + System.IO.Path.DirectorySeparatorChar;
            detectedFolder = rootFolder + "Detected" + System.IO.Path.DirectorySeparatorChar;

            fileUtils = FileUtils.GetInstance(MainWindow.rootFolder);


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
                            FileUtils fileUtils = FileUtils.GetInstance(Properties.Settings.Default.WorkSpace);
                            fileUtils.CreateDirectoryStructure();
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
            int count = Directory.GetFiles(MainWindow.importFolder).Length;
            if (count == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Cloudcoins (*.stack, *.jpg,*.jpeg)|*.stack;*.jpg;*.jpeg|Stack files (*.stack)|*.stack|Jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = fileUtils.importFolder;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        try
                        {
                            if (!File.Exists(fileUtils.importFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename)))
                                File.Move(filename, fileUtils.importFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
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
                                        File.Delete(fileUtils.importFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
                                        File.Move(filename, fileUtils.importFolder + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(filename));
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

            int totalRAIDABad = 0;
            for (int i = 0; i < 25; i++)
            {
                if (RAIDA_Status.failsEcho[i])
                {
                    totalRAIDABad += 1;
                }
            }
            if (totalRAIDABad > 8)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("You do not have enough RAIDA to perform an import operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                Console.ForegroundColor = ConsoleColor.White;

                insufficientRAIDA();

                return;
            }
            //cmdImport.IsEnabled = false;
            //cmdRestore.IsEnabled = false;
            //progressBar.Visibility = Visibility.Visible;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                import();

                /* run your code here */
            }).Start();
        }

        public void export()
        {
            if (rdbJpeg.IsChecked == true)
                exportJpegStack = 1;
            else
                exportJpegStack = 2;

            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);

            //updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = Convert.ToInt16(updOne.Value);
            int exp_5 = Convert.ToInt16(updFive.Value);
            int exp_25 = Convert.ToInt16(updQtr.Value);
            int exp_100 = Convert.ToInt16(updHundred.Value);
            int exp_250 = Convert.ToInt16(updTwoFifty.Value);
            //Warn if too many coins

            if (exp_1 + exp_5 + exp_25 + exp_100 + exp_250 == 0)
            {
                Console.WriteLine("Can not export 0 coins");
                return;
            }

            //updateLog(Convert.ToString(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5] + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]));

            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5]) + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]) > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                Console.Out.WriteLine("Do not export stack files with more than 1000 notes. .");
                //updateLog("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                //updateLog("Do not export stack files with more than 1000 notes. .");

                Console.ForegroundColor = ConsoleColor.White;
            }//end if they have more than 1000 coins

            Console.Out.WriteLine("  Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");
            int file_type = 0; //reader.readInt(1, 2);





            Exporter exporter = new Exporter(fileUtils);
            exporter.OnUpdateStatus += Exporter_OnUpdateStatus; ;
            file_type = exportJpegStack;

            String tag = txtTag.Text;// reader.readString();
            //Console.Out.WriteLine(("Exporting to:" + exportFolder));

            if (file_type == 1)
            {
                exporter.writeJPEGFiles(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
                // stringToFile( json, "test.txt");
            }
            else
            {
                exporter.writeJSONFile(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
            }


            // end if type jpge or stack

            RefreshCoins?.Invoke(this, new EventArgs());
            //updateLog("Exporting CloudCoins Completed.");
            showCoins();
            Process.Start(fileUtils.exportFolder);
            cmdExport.Content = "₡0";
            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One

        public void multi_detect()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            MultiDetect multi_detector = new MultiDetect(fileUtils);
            multi_detector.txtLogs = txtLogs;

            //Calculate timeout
            int detectTime = 20000;
            if (RAIDA_Status.getLowest21() > detectTime)
            {
                detectTime = RAIDA_Status.getLowest21() + 200;
            }//Slow connection

            multi_detector.detectMulti(detectTime);
            // grade();
            // showCoins();

        }//end multi detect

        public void grade()
        {
            Console.Out.WriteLine("");
            updateLog("  Grading Authenticated Coins");
            Console.Out.WriteLine("  Grading Authenticated Coins");// "Detecting Authentication of Suspect Coins");
            Grader grader = new Grader(fileUtils);
            int[] detectionResults = grader.gradeAll(5000, 2000);
            updateLog("  Total imported to bank: " + detectionResults[0]);
            updateLog("  Total imported to fracked: " + detectionResults[1]);
            updateLog("  Total Counterfeit: " + detectionResults[2]);
            updateLog("  Total moved to Lost folder: " + detectionResults[4]);

            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[1]);//"Total imported to fracked: "                                                                       // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[2]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            Console.Out.WriteLine("  Total moved to Lost folder: " + detectionResults[4]);//"Total Kept in suspect folder: " 

        }//end detect


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
            export();
        }

        private void cmdWorkspace_Click(object sender, RoutedEventArgs e)
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
                            FileUtils fileUtils = FileUtils.GetInstance(Properties.Settings.Default.WorkSpace);
                            fileUtils.CreateDirectoryStructure();
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
    }
    public static class MyExtensions
    {
        public static string ToCustomString(this TimeSpan span)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }
    }

}
