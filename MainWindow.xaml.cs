using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Info> infos = new List<Info>();

        public MainWindow()
        {
            InitializeComponent();

            prepare_comboBox();

            test_init();
        }

        private void test_init()
        {
            HtmlParseAction(htmlRequest("B00100000000O68N3"));
            HtmlParseAction(htmlRequest("B00100000000MYDCN"));
            HtmlParseAction(htmlRequest("B00100000000O54NX"));
            HtmlParseAction(htmlRequest("B00100000000O5VWY"));
            HtmlParseAction(htmlRequest("B00100000000O5VSU"));
            HtmlParseAction(htmlRequest("B00100000000MZYIG"));
            HtmlParseAction(htmlRequest("B0010000000129E8B"));
            HtmlParseAction(htmlRequest("B001000000012VGOG"));
            HtmlParseAction(htmlRequest("B001000000012ZD81"));
            HtmlParseAction(htmlRequest("B001000000012U6Q7"));
            HtmlParseAction(htmlRequest("B001000000012U4YD"));
            HtmlParseAction(htmlRequest("B00100000000OJK27"));
            HtmlParseAction(htmlRequest("B00100000000RCX0E"));
        }
        private void prepare_comboBox()
        {
            comboBox1.Items.Add("용도");
            comboBox1.Items.Add("층수");
            comboBox1.Items.Add("지하");
            comboBox1.Items.Add("건물면적");
            comboBox1.Items.Add("건물높이");
            comboBox1.Items.Add("용적률");
            comboBox1.Items.Add("건폐율");
            comboBox1.Items.Add("연면적");
            comboBox1.Items.Add("대지면적");
            comboBox1.Items.Add("구조");
            comboBox1.Items.Add("준공일자");
        }

        private Uri CreateUrl(string key)
        {
            return new Uri(String.Format("http://map.vworld.kr/v4map_po_buildMetaInfov15.do?geoidn={0}", key));
        }

        private string htmlRequest(string key)
        {
            Uri url = CreateUrl(key);
            // 스트링 빌더 클래스 인스턴스
            StringBuilder sb = new StringBuilder();
            int TimeOut = 10000;
            try
            {
                // 웹사이트에 대한 정보 얻기
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.ConnectionLimit = 50;
                request.Timeout = TimeOut;
                request.ReadWriteTimeout = TimeOut * 5;
                request.KeepAlive = false;
                // 웹사이트에 응답하는 객체 만들기           
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // 스트림 객체 가져오기
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        // 스트림에서 데이터 읽기(한글 처리)
                        //using (StreamReader read = new StreamReader(responseStream, Encoding.Default, true))
                        using (StreamReader read = new StreamReader(responseStream))
                        {
                            // 스트림에서 데이터 읽기
                            sb.Append(read.ReadToEnd());
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message.ToString());
            }
            return sb.ToString();

        }
 
        private void HtmlParseAction(String html)
        {
            try
            {
                // 건물동명칭 
                HtmlAgilityPack.HtmlDocument mydoc = new HtmlAgilityPack.HtmlDocument();
                mydoc.LoadHtml(html);

                HtmlAgilityPack.HtmlNodeCollection nodes = mydoc.DocumentNode.SelectNodes("//tbody/tr/td");

                Info info = new Info();

                int count = 1;
                foreach (HtmlAgilityPack.HtmlNode node in nodes)
                {
                   Console.WriteLine(count++ + node.InnerText.Trim());
                }
               
                info.building1 = nodes[0].InnerText.Trim().Equals("-") ? "blank" : nodes[0].InnerText.Trim();
                info.building2 = nodes[1].InnerText.Trim().Equals("-") ? "blank" : nodes[1].InnerText.Trim();
                info.building3 = nodes[2].InnerText.Trim().Equals("-") ? "blank" : nodes[2].InnerText.Trim();
                info.buildingUsing = nodes[3].InnerText.Trim().Equals("-") ? "blank" : nodes[3].InnerText.Trim();
                info.buildingState = nodes[4].InnerText.Trim().Equals("-") ? "blank" : nodes[4].InnerText.Trim();
                info.buildingFloor = nodes[5].InnerText.Trim().Equals("-") ? "blank" : nodes[5].InnerText.Trim();
                info.buildingUnderFloor = nodes[6].InnerText.Trim().Equals("-") ? "blank" : nodes[6].InnerText.Trim();
                info.buildingArea = nodes[7].InnerText.Trim().Equals("-") ? "blank" : nodes[7].InnerText.Trim().Split(' ')[0];
                info.buildingHeight = nodes[8].InnerText.Trim().Equals("-") ? "blank" : nodes[8].InnerText.Trim().Split(' ')[0];
                info.areaRatio = nodes[9].InnerText.Trim().Equals("-") ? "blank" : nodes[9].InnerText.Trim().Split(' ')[0];
                info.coverrageRatiod = nodes[9].InnerText.Trim().Equals("-") ? "blank" : nodes[10].InnerText.Trim().Split(' ')[0];
                info.grossArea = nodes[10].InnerText.Trim().Equals("-") ? "blank" : nodes[11].InnerText.Trim();
                info.landArea = nodes[11].InnerText.Trim().Equals("-") ? "blank" : nodes[12].InnerText.Trim().Split(' ')[0];
                info.structure = nodes[12].InnerText.Trim().Equals("-") ? "blank" : nodes[13].InnerText.Trim();
                info.endingDate = nodes[13].InnerText.Trim().Equals("-") ? "blank" : nodes[14].InnerText.Trim();

                infos.Add(info);
                
                InfoListView.ItemsSource = infos;
                InfoListView.Items.Refresh();

            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message.ToString());
            }
            
            // XDocument buildingInformationXml = XDocument.Parse(sb.ToString());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            //count++;
            //string key = "B0010000000B616LM";
            string key = this.urlText.Text;
  
            string resp = htmlRequest(key);
            HtmlParseAction(resp);
        }
        private void Button_Search(object sender, RoutedEventArgs e)
        {
            try
            {
                string txt = comboBox1.SelectedItem as String;
                string searchValue = this.searchValue.Text;
                Console.WriteLine(txt);
                List<Info> searchInfos = new List<Info>();

                if (txt.Equals("용도"))
                {
                    foreach (Info i in infos)
                    {
                        if (i.buildingUsing.Contains(searchValue)) {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("구조")) {
                    foreach (Info i in infos)
                    {
                        if (i.structure.Contains(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("층수"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.buildingFloor.Equals("-")) && Int32.Parse(i.buildingFloor) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("지하"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.buildingUnderFloor.Equals("-")) && Int32.Parse(i.buildingUnderFloor) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("건물면적"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.buildingArea.Equals("-")) && Int32.Parse(i.buildingArea) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("건물높이"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.buildingHeight.Equals("-")) && Int32.Parse(i.buildingHeight) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("용적률"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.areaRatio.Equals("-")) && Int32.Parse(i.areaRatio) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("건폐율"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.coverrageRatiod.Equals("-")) && Int32.Parse(i.coverrageRatiod) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("연면적"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.grossArea.Equals("-")) && Int32.Parse(i.grossArea) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("대지면적"))
                {
                    foreach (Info i in infos)
                    {
                        if (!(i.landArea.Equals("-")) && Int32.Parse(i.landArea) < Int32.Parse(searchValue))
                        {
                            searchInfos.Add(i);
                        }
                    }
                }else if (txt.Equals("준공일자"))
                {
                    string year = searchValue.Split('-')[0];
                    string month = searchValue.Split('-')[1];
                    string day = searchValue.Split('-')[2];
                }
                //List 업데이트
                InfoListView.ItemsSource = searchInfos;
            }catch(Exception e3)
            {
                Console.WriteLine(e3.Message.ToString());
            }
        }
        private void Button_Restore(object sender, RoutedEventArgs e)
        {
            InfoListView.ItemsSource = infos;
            InfoListView.Items.Refresh();
        }


        private void InfoListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }
        
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Console.Write("URL 확인");
            Regex regex = new Regex(@"/^ (file | gopher | news | nntp | telnet | https ?| ftps ?| sftp):\/\/ ([a - z0 - 9 -] +\.)+[a - z0 - 9]{ 2,4}.*$/");
            Boolean ismatch = regex.IsMatch(urlText.Text);
            if (!ismatch)
            {
                MessageBox.Show("URL을 입력하세요");

            }
        }
    }
}
