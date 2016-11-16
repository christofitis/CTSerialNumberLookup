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
using System.Data.OleDb;
using System.Data;
using System.Windows.Threading;

namespace CTInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string versionInfo = "Serial Number Lookup\nVersion: 3.0\nCreated by: Chris Bryant\n©2016";

        DataTable serialDataTable = new DataTable();
        DataTable serialDataTableLow = new DataTable();
        product product = new product();
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = product;
            searchingLabel.Visibility = Visibility.Hidden;
            
        }

        private DataTable getDataFromDatabase() //returns a dataset with all the information in a specified sheet
        {
            //string dataPath = @"p:\criticalos\serialLookup\CriticalProdDB.xlsx";
            string dataPath = @"c:\temp\CriticalProdDB.xlsx";

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + dataPath + ";Extended Properties='Excel 12.0;'";
            DataTable myDataTable = null;
            



            using (OleDbConnection oleDbConection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand oleDbCommand = new OleDbCommand("SELECT * FROM " + product.sheet , oleDbConection);
                    OleDbDataAdapter oleDataAdapter = new OleDbDataAdapter();
                    oleDbConection.Open();
                    oleDataAdapter = new OleDbDataAdapter(oleDbCommand);
                    myDataTable = new DataTable();
                    oleDataAdapter.Fill(myDataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "getDataFromDatabase method error");
                }
                oleDbConection.Dispose();
            }
            return myDataTable;
        }


        private bool isUserSerialNumberValid() //checks user input for errors and then defines product.serial number for further use
        {
            bool isUserSerialNumberValid = false;
            int n = 0;
            int.TryParse(userInputTextBox.Text.Remove(2).ToUpper(), out n);

            if (n == 0) //checks for users entering only numbers
            {

            
            if (userInputTextBox.Text.Count() >= 3 && userInputTextBox.Text.Remove(1).ToUpper() != "C" && userInputTextBox.Text.Count() <= 7) //if cx1, cx2, cx2r, atom
            {
                    if (userInputTextBox.Text.Remove(2).ToUpper() == "1X" ||
                        userInputTextBox.Text.Remove(2).ToUpper() == "1B" ||
                        userInputTextBox.Text.Remove(2).ToUpper() == "2B" ||
                        userInputTextBox.Text.Remove(2).ToUpper() == "2R"
                        ||
                        userInputTextBox.Text.Remove(2).ToUpper() == "AT"
                        ) {
                        switch (userInputTextBox.Text.Remove(2).ToUpper())
                        {

                            case "1X":
                                product.sheet = "[cx1$]";
                                product.name = "CX-1";
                                product.prefix = "1X";
                                isUserSerialNumberValid = true;
                                break;
                            case "1B":
                                product.sheet = "[cx1$]";
                                product.name = "CX-1";
                                product.prefix = "1B";
                                isUserSerialNumberValid = true;
                                break;
                            case "2B":
                                product.sheet = "[cx2$]";
                                product.name = "CX-2";
                                product.prefix = "2B";
                                isUserSerialNumberValid = true;
                                break;
                            case "2R":
                                product.name = "CX-2R";
                                product.sheet = "[cx2r$]";
                                product.prefix = "2R";
                                isUserSerialNumberValid = true;
                                break;
                            case "AT":
                                product.sheet = "[atom$]";
                                product.name = "ATOM";
                                product.prefix = "AT";
                                isUserSerialNumberValid = true;
                                break;
                            default:
                                isUserSerialNumberValid = false;
                                break;
                        }
                        if (seporateNumberfromPrefix() == 0)
                        {
                            isUserSerialNumberValid = false;
                        }
                        else
                        {
                            product.number = seporateNumberfromPrefix();
                            isUserSerialNumberValid = true;
                        }
                    }
            }
            else if (userInputTextBox.Text.Count() >= 4 && userInputTextBox.Text.Count() <= 8) //if cxp
            {
                if (userInputTextBox.Text.Remove(3).ToUpper() == "CXP")
                {
                    //MessageBox.Show("is cxp");
                    product.sheet = "[cxp$]";
                    product.name = "CXP";
                    product.prefix = "CXP";
                    isUserSerialNumberValid = true;
                }
                if (seporateNumberfromPrefix() == 0)
                {
                    isUserSerialNumberValid = false;
                }
                else
                {
                    product.number = seporateNumberfromPrefix();
                    isUserSerialNumberValid = true;
                }
            }

            }
            return isUserSerialNumberValid;
       
        }

        private int seporateNumberfromPrefix()
        {
            int i = 0;
            string s = "";
            string sReversed = "";
            char[] charArray = userInputTextBox.Text.ToCharArray(); //convert s to char array
            Array.Reverse(charArray); //reverese entire char array
            sReversed = new string(charArray);
            if (product.prefix == "CXP")
            {
                s = sReversed.Remove(charArray.Length - 3);
            }
            else
            {
                s = sReversed.Remove(charArray.Length - 2);
            }
            charArray = s.ToCharArray();
            Array.Reverse(charArray);
            s = new string(charArray);
            int.TryParse(s, out i);
            return i;
        }

        private void resetInterface()
        {

            rightNotesBoxScrollViewer.Margin = new Thickness(265, 263, 0, 0);
            productPurchaseHighDateLabel.Margin = new Thickness(298, 207, 0, 0);
            productRevisionHighLabel.Margin = new Thickness(298, 167, 0, 0);
            productSerialHighLabel.Margin = new Thickness(298, 127, 0, 0);
            leftNotesBoxScrollViewer.Margin = new Thickness(7, 263, 0, 0);
            productPurchaseLowDateLabel.Margin = new Thickness(99, 207, 0, 0);
            productRevisionLowLabel.Margin = new Thickness(99, 167, 0, 0);
            productSerialLowLabel.Margin = new Thickness(99, 127, 0, 0);

            searchingLabel.Visibility = Visibility.Hidden;

            leftNotesBoxScrollViewer.Visibility = Visibility.Visible;
            productPurchaseLowDateLabel.Visibility = Visibility.Visible;
            productRevisionLowLabel.Visibility = Visibility.Visible;
            productSerialLowLabel.Visibility = Visibility.Visible;

            rightNotesBoxScrollViewer.Visibility = Visibility.Visible;
            productPurchaseHighDateLabel.Visibility = Visibility.Visible;
            productRevisionHighLabel.Visibility = Visibility.Visible;
            productSerialHighLabel.Visibility = Visibility.Visible;

            productNameLabel.Content = "";
            productSerialHighLabel.Content = "";
            productSerialLowLabel.Content = "";
            productRevisionHighLabel.Content = "";
            productRevisionLowLabel.Content = "";
            productPurchaseHighDateLabel.Content = "";
            productPurchaseLowDateLabel.Content = "";
            productNotesTextBlockHigh.Text = "";
            productNotesTextBlockLow.Text = "";
            

        }

        private void resetProductValues()
        {
            product.name = "";
            product.prefix = "";
            product.number = 0;

            product.userInputSerialNumber = "";
                       
            product.sheet = "";

            product.serialNumberLow = "";
            product.serialNumberHigh = "";

            product.notesLow = "";
            product.notesHigh = "";

            product.purchaseDateHigh = "";
            product.purchaseDateLow = "";

            product.revisionHigh = "";
            product.revisionLow = "";



        }

        private void assignLabels()
        {

            product.dateTimeHigh = Convert.ToDateTime(product.purchaseDateHigh);
            product.dateTimeLow = Convert.ToDateTime(product.purchaseDateLow);

            productNameLabel.Content = product.name;
            productSerialHighLabel.Content = product.serialNumberHigh;
            productSerialLowLabel.Content = product.serialNumberLow;
            productRevisionHighLabel.Content = product.revisionHigh;
            productRevisionLowLabel.Content = product.revisionLow;
            productPurchaseHighDateLabel.Content = product.dateTimeHigh.ToString("d");
            productPurchaseLowDateLabel.Content = product.dateTimeLow.ToString("d");
            productNotesTextBlockHigh.Text = product.notesHigh;
            productNotesTextBlockLow.Text = product.notesLow;

            productNameLabel.Content = product.name; //TODO: delete these
            searchingLabel.Visibility = Visibility.Hidden;


        }





        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            resetInterface();
            resetProductValues();
            searchingLabel.Visibility = Visibility.Visible;

            bool ifExactMatch = true; //used to determin if exact match was found between user and DB

            if (isUserSerialNumberValid())
            {
                int searchingInt = 0;

                int serialIncrimentLoopBreak = 0;
                product.numberLow = product.number;
                serialDataTable = getDataFromDatabase();
                serialDataTableLow = getDataFromDatabase();
                
                product.userInputSerialNumber = product.prefix + product.number.ToString("00000");
                serialDataTable.DefaultView.RowFilter = string.Format("Serial = '{0}'", product.userInputSerialNumber);
                product.serialNumberLow = product.prefix + product.numberLow.ToString("00000");
                serialDataTableLow.DefaultView.RowFilter = string.Format("Serial = '{0}'", product.serialNumberLow);
                while (serialDataTable.DefaultView.Count == 0 ) //find HIGH serial number
                {
                    // MessageBox.Show(product.serialNumber, product.number.ToString());
                    ifExactMatch = false;
                    product.number++;
                    product.serialNumberHigh = product.prefix + product.number.ToString("00000");
                    serialDataTable.DefaultView.RowFilter = string.Format("Serial = '{0}'", product.serialNumberHigh);
                    
                    serialIncrimentLoopBreak++;

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Action)(() =>
                    {
                        searchingInt++;
                        searchingLabel.RenderTransform = new RotateTransform(searchingInt);
                    }));

                }
                while (serialDataTableLow.DefaultView.Count == 0) //find Low Serial Number
                {
                   
                    product.numberLow--;
                    product.serialNumberLow = product.prefix + product.numberLow.ToString("00000");
                    serialDataTableLow.DefaultView.RowFilter = string.Format("Serial = '{0}'", product.serialNumberLow);

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Action)(() =>
                    {
                        searchingInt++;
                        searchingLabel.RenderTransform = new RotateTransform(searchingInt*-1);
                    }));

                }

                if (ifExactMatch) //is exact match is found in database from user input do this
                {
                    messageLabel.Content = "Exact match found!";
                    leftNotesBoxScrollViewer.Visibility = Visibility.Hidden;
                    productPurchaseLowDateLabel.Visibility = Visibility.Hidden;
                    productRevisionLowLabel.Visibility = Visibility.Hidden;
                    productSerialLowLabel.Visibility = Visibility.Hidden;

                    rightNotesBoxScrollViewer.Visibility = Visibility.Visible;
                    productPurchaseHighDateLabel.Visibility = Visibility.Visible;
                    productRevisionHighLabel.Visibility = Visibility.Visible;
                    productSerialHighLabel.Visibility = Visibility.Visible;

                    rightNotesBoxScrollViewer.Margin = new Thickness(136, 263, 0, 0);
                    productPurchaseHighDateLabel.Margin = new Thickness(210, 207, 0, 0);//
                    productRevisionHighLabel.Margin = new Thickness(210, 167, 0, 0);//
                    productSerialHighLabel.Margin = new Thickness(210, 127, 0, 0);//
                    leftNotesBoxScrollViewer.Margin = new Thickness(7, 263, 0, 0);
                    productPurchaseLowDateLabel.Margin = new Thickness(99, 207, 0, 0);
                    productRevisionLowLabel.Margin = new Thickness(99, 167, 0, 0);
                    productSerialLowLabel.Margin = new Thickness(99, 127, 0, 0);

                    product.serialNumberHigh = product.userInputSerialNumber;
                    product.notesHigh = serialDataTable.DefaultView[0]["Notes"].ToString();
                    product.purchaseDateHigh = serialDataTable.DefaultView[0]["Date"].ToString();          
                    product.revisionHigh = serialDataTable.DefaultView[0]["Rev"].ToString();
                    product.purchaseDateLow = serialDataTable.DefaultView[0]["Date"].ToString();



                }
               else //if exact match from user input is not found, do this
                {
                    messageLabel.Content = "Could not find exact match. That serial is between these:";
                    leftNotesBoxScrollViewer.Visibility = Visibility.Visible;
                    productPurchaseLowDateLabel.Visibility = Visibility.Visible;
                    productRevisionLowLabel.Visibility = Visibility.Visible;
                    productSerialLowLabel.Visibility = Visibility.Visible;

                    rightNotesBoxScrollViewer.Visibility = Visibility.Visible;
                    productPurchaseHighDateLabel.Visibility = Visibility.Visible;
                    productRevisionHighLabel.Visibility = Visibility.Visible;
                    productSerialHighLabel.Visibility = Visibility.Visible;

                    rightNotesBoxScrollViewer.Margin = new Thickness(265, 263, 0, 0); 
                    productPurchaseHighDateLabel.Margin = new Thickness(298, 207, 0, 0);
                    productRevisionHighLabel.Margin = new Thickness(298, 167, 0, 0);
                    productSerialHighLabel.Margin = new Thickness(298, 127, 0, 0);
                    leftNotesBoxScrollViewer.Margin = new Thickness(7, 263, 0, 0);
                    productPurchaseLowDateLabel.Margin = new Thickness(99, 207, 0, 0);
                    productRevisionLowLabel.Margin = new Thickness(99, 167, 0, 0);
                    productSerialLowLabel.Margin = new Thickness(99, 127, 0, 0);

                    product.serialNumberHigh = product.serialNumberHigh;
                    product.notesHigh = serialDataTable.DefaultView[0]["Notes"].ToString();
                    product.purchaseDateHigh = serialDataTable.DefaultView[0]["Date"].ToString();
                    product.revisionHigh = serialDataTable.DefaultView[0]["Rev"].ToString();
                    product.serialNumberLow = product.serialNumberLow;
                    product.notesLow = serialDataTableLow.DefaultView[0]["Notes"].ToString();
                    product.purchaseDateLow = serialDataTableLow.DefaultView[0]["Date"].ToString();
                    product.revisionLow = serialDataTableLow.DefaultView[0]["Rev"].ToString();
                }
                
                assignLabels();
                               
            }
            
            else
            {
                MessageBox.Show("Unrecognized Serial Number.\nPlease try another.", "Serial Number Error");
                searchingLabel.Visibility = Visibility.Hidden;
            }
            
        }




        private void enterKeyCatch(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                searchButton_Click(sender, e);

            }

        }

        private void appStartFocusSet(object sender, RoutedEventArgs e)
        {
            userInputTextBox.Focus();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(versionInfo, "About");
        }

        private void minumizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dragWindowsEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
