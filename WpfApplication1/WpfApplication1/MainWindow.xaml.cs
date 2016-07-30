using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace WpfApplication1
{
    public class StockItem
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _strStockName = "";
        private string _strStockPrice = "";

        public string strStockName
        {
            get { return _strStockName; }
            set
            {
                if ( value != _strStockName)
                {
                    _strStockName = value;
                    OnPropertyChanged( "strStockName" );
                }
            }
        }

        public string strStockPrice
        {
            get { return _strStockPrice; }
            set
            {
                if ( value != _strStockPrice )
                {
                    _strStockPrice = value;
                    OnPropertyChanged( "strStockPrice" );
                }
            }
        }

        protected void OnPropertyChanged( string name )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( name ) );
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BindInXaml();
        }

        private void BindInXaml()
        {
            base.DataContext = new StockItem { strStockName = "Msft", strStockPrice = "56.21" };
        }

        public string GetStockPrice( string strTickerSymbol )
        {
            string result = "";

            using( var client = new HttpClient( new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                // sample yahoo finance request syntax: http://finance.yahoo.com/d/quotes.csv?s=msft&f=price
                // sample response content: 56.21,26.99,N/A,"+0.47 - +0.84%",2.10
                client.BaseAddress = new Uri( "http://finance.yahoo.com/d/" );
                HttpResponseMessage response = client.GetAsync( "quotes.csv?s=" + strTickerSymbol + "&f=price" ).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                string[] temp = result.Split( ';' );
                result = temp[0];
            }
            return result;
        }
    }
}
