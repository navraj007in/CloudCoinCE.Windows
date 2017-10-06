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

namespace CloudCoinCE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };

        public MainWindow()
        {
            InitializeComponent();
            loadJson();
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

            new Thread(delegate () {
                echoRaida();
            }).Start();

            worker.DoWork += Worker_DoWork; ;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted; ;
            worker.RunWorkerAsync();

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

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            echoRaida();
        }

        private void loadJson()
        {
            string fileName = @"nodes.json";
            TextRange range;
            FileStream fStream;
            if (File.Exists(fileName))
            {
                range = new TextRange(txtOutput.Document.ContentStart, txtOutput.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Text);
                fStream.Close();
            }
        }

        private void updateLEDs()
        {
            raida1.ColorOn = RAIDA_Status.failsEcho[0] ? Colors.Green : Colors.Red;
            raida2.ColorOn = RAIDA_Status.failsEcho[1] ? Colors.Green : Colors.Red;
            raida3.ColorOn = RAIDA_Status.failsEcho[2] ? Colors.Green : Colors.Red;
            raida4.ColorOn = RAIDA_Status.failsEcho[3] ? Colors.Green : Colors.Red;
            raida5.ColorOn = RAIDA_Status.failsEcho[4] ? Colors.Green : Colors.Red;
            raida6.ColorOn = RAIDA_Status.failsEcho[5] ? Colors.Green : Colors.Red;
            raida7.ColorOn = RAIDA_Status.failsEcho[6] ? Colors.Green : Colors.Red;
            raida8.ColorOn = RAIDA_Status.failsEcho[7] ? Colors.Green : Colors.Red;
            raida9.ColorOn = RAIDA_Status.failsEcho[8] ? Colors.Green : Colors.Red;
            raida10.ColorOn = RAIDA_Status.failsEcho[9] ? Colors.Green : Colors.Red;
            raida11.ColorOn = RAIDA_Status.failsEcho[10] ? Colors.Green : Colors.Red;
            raida12.ColorOn = RAIDA_Status.failsEcho[11] ? Colors.Green : Colors.Red;
            raida13.ColorOn = RAIDA_Status.failsEcho[12] ? Colors.Green : Colors.Red;
            raida14.ColorOn = RAIDA_Status.failsEcho[13] ? Colors.Green : Colors.Red;
            raida15.ColorOn = RAIDA_Status.failsEcho[14] ? Colors.Green : Colors.Red;
            raida16.ColorOn = RAIDA_Status.failsEcho[15] ? Colors.Green : Colors.Red;
            raida17.ColorOn = RAIDA_Status.failsEcho[16] ? Colors.Green : Colors.Red;
            raida18.ColorOn = RAIDA_Status.failsEcho[17] ? Colors.Green : Colors.Red;
            raida19.ColorOn = RAIDA_Status.failsEcho[18] ? Colors.Green : Colors.Red;
            raida20.ColorOn = RAIDA_Status.failsEcho[19] ? Colors.Green : Colors.Red;
            raida21.ColorOn = RAIDA_Status.failsEcho[20] ? Colors.Green : Colors.Red;
            raida22.ColorOn = RAIDA_Status.failsEcho[21] ? Colors.Green : Colors.Red;
            raida23.ColorOn = RAIDA_Status.failsEcho[22] ? Colors.Green : Colors.Red;
            raida24.ColorOn = RAIDA_Status.failsEcho[23] ? Colors.Green : Colors.Red;
            raida25.ColorOn = RAIDA_Status.failsEcho[24] ? Colors.Green : Colors.Red;


        }
        public bool echoRaida()
        {


            RAIDA_Status.resetEcho();
            RAIDA raida1 = new RAIDA(15000);
            Response[] results = raida1.echoAll(15000);
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
                 // Console.Out.Write(RAIDA_Status.failsEcho[i]);
                if (RAIDA_Status.failsEcho[i])
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Out.Write(strPad + countries[i]);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Out.Write(strPad + countries[i]);
                    totalReady++;
                }
                //if (RAIDA_Status.failsEcho[i])
                //    raidas[i].Background = Brushes.Red;
                //else
                //    raidas[i].Background = Brushes.Green;

                if (i == 4 || i == 9 || i == 14 || i == 19) { Console.WriteLine(); }
            }//end for
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.Write("  RAIDA Health: " + totalReady + " / 25: ");//"RAIDA Health: " + totalReady );
            this.Dispatcher.Invoke(() => {
                updateLEDs();
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


    }
}
