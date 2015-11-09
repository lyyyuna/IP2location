using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

using Windows.Storage;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click1(object sender, RoutedEventArgs e)
        {
            var ipaddressString = nameInput.Text;
            if (ipaddressString == "")
            {
                ipaddressString = null;
            }
            
            try
            {
                IPAddress Address = IPAddress.Parse(ipaddressString);
                var normalizeAdress = Address.ToString();
                Uri baiduuri = new Uri("http://api.map.baidu.com/location/ip?ak=ojGKrXy7e3v4rCTs1tIHMM18&ip="+normalizeAdress);
                var httpclient = new HttpClient();

                // send to baidu api
                // and parse json
                var result = await httpclient.GetStringAsync(baiduuri);
                JObject jsonobj = JObject.Parse(result);

                var locationaddress = jsonobj["content"]["address"];

                Uri baidumapuri = new Uri("http://api.map.baidu.com/staticimage?width=800&height=400&center=" + locationaddress);
                ShowBaiduImage.Navigate(baidumapuri);

                greetingOutput.Text = "您所输入的 IPv4 地址为 " + normalizeAdress + "\n" + "所在地址为" + locationaddress.ToString();

            }
            catch (ArgumentNullException ee)
            {
                greetingOutput.Text = "请输入 IPv4 地址再查询";
                ShowBaiduImage.Navigate(new Uri("about:blank"));
            }
            catch (FormatException ee)
            {
                greetingOutput.Text = "您输入的 IPv4 格式不正确";
                ShowBaiduImage.Navigate(new Uri("about:blank"));
            }
            catch
            {
                greetingOutput.Text = "抱歉，查不到你所输入的 IPv4 地址" 
                        + "\n请检查网络连接并确保是国内的 IPv4 地址";
                ShowBaiduImage.Navigate(new Uri("about:blank"));
            }



        }
    }
}
