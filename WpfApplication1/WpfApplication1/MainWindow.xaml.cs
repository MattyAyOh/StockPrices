using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
