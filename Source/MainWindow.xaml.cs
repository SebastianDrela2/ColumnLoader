using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace ReadingDataFromDataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string connection_string {get; set;}
        private void LoadData()
        {
            List<string> column_names = new List<string>();
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = "Use DevData; SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE DATA_TYPE = 'int';"
                   
                };
                connection.Open();
               SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
               {
                    while(reader.Read())
                    {
                        column_names.Add(reader[0].ToString());
                    }
               };
            }


            foreach( var line in column_names )
            {
                DataGridTextColumn column  = new DataGridTextColumn();
                column.Header = line;
                column.CellStyle = (Style)TryFindResource("DataGridCellCentered");
                column.Width = DATA_GRID.ActualWidth/column_names.Count;

                DATA_GRID.Columns.Add(column);
                              
            }

        }
        private void TestConnection()
        {
            string login = TXT_BOX_LOGIN.Text;
            string password = TXT_BOX_PASSWORD.Text;         
            string con_string = $"Data Source=localhost;User ID={login};Password={password};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection connection = new SqlConnection(con_string))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection Made.");
                    connection_string = con_string;
                }
                catch(Exception ex)
                {
                    if(ex.Data != null)
                    {
                        MessageBox.Show("Invalid Credintals.");
                        
                    }
                }
               
            }
           
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BTN_CONNECTION_Click(object sender, RoutedEventArgs e)
        {
            TestConnection();
            if (connection_string != String.Empty)
            {
                BTN_LOAD_DATA.IsEnabled = true;
            }
        }

        private void BTN_LOAD_DATA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (connection_string != String.Empty)
                {
                    LoadData();
                }

                MessageBox.Show("Loaded Column names with INT DataType");
            }
            catch(Exception ex)
            {

            }
        }
    }
}
