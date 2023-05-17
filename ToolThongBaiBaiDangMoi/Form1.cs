using HtmlAgilityPack;
using NAudio.Wave;
using Newtonsoft.Json;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolThongBaoBaiDangMoi
{
    public partial class Form1 : Form
    {
        ChromeDriver chromeDriver;
        private bool isPlayingAlarm;
        private int iTimeSleep;
        private bool bIsRunning;

        private List<LastDataModel> listLastDataChoTotModel;

        private List<List<ChoTotExcelModel>> listChoTotExcelModels;


        private Dictionary<string, string> dataRegionBonBanh;

        private List<LastDataModel> listLastDataBonBanhModel;
        public Form1()
        {
            InitializeComponent();
        }

        private void CloseAllChromeDriver()
        {
            try
            {
                Process[] arrayProcesses = Process.GetProcessesByName("chromedriver");
                if (arrayProcesses != null && arrayProcesses.Length > 0)
                {
                    foreach (var process in arrayProcesses)
                    {
                        process.Kill();
                    }
                }
            }
            catch
            {

            }
        }

        private void CloseAllChrome()
        {
            try
            {
                Process[] arrayProcesses = Process.GetProcessesByName("chrome");
                if (arrayProcesses != null && arrayProcesses.Length > 0)
                {
                    foreach (var process in arrayProcesses)
                    {
                        process.Kill();
                    }
                }
            }
            catch
            {

            }
        }










        List<string> saveFileChoTot;

        private void GetChoTot()
        {
            saveFileChoTot = new List<string>();
            foreach (ChoTotRegion regionData in Enum.GetValues(typeof(ChoTotRegion)))
            {
                string d = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                if (Directory.Exists(d) == false)
                {
                    Directory.CreateDirectory(d);
                }
                string file = Path.Combine(d, regionData.ToString() + "_" + d + ".xlsx");
                saveFileChoTot.Add(file);
            }

            new Thread(async () =>
        {


            while (bIsRunning)
            {
                Thread.CurrentThread.IsBackground = true;

                Check.CheckTime();
                int indexRegion = 0;



                foreach (ChoTotRegion regionData in Enum.GetValues(typeof(ChoTotRegion)))
                {
                    try
                    {
                        var options = new RestClientOptions()
                        {
                            MaxTimeout = -1,
                            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                        };
                        var client = new RestClient(options);
                        var request = new RestRequest($"https://gateway.chotot.com/v1/public/ad-listing?region_v2={((int)regionData)}&cg=2010&f=p&page=1&st=s,k&limit=20&w=1&key_param_included=true", Method.Get);
                        request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"110\", \"Not A(Brand\";v=\"24\", \"Google Chrome\";v=\"110\"");
                        request.AddHeader("Accept", "application/json, text/plain, */*");
                        //request.AddHeader("ct-fingerprint", "113b8d37-5d7c-40c9-b167-e52b26f4131d");
                        request.AddHeader("sec-ch-ua-mobile", "?0");
                        request.AddHeader("ct-platform", "web");
                        request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                        request.AddHeader("Sec-Fetch-Site", "same-site");
                        request.AddHeader("Sec-Fetch-Mode", "cors");
                        request.AddHeader("Sec-Fetch-Dest", "empty");
                        //request.AddHeader("Cookie", "__cf_bm=xih1RY8ijyHYR8Nl5mpGw7Myrm3By8xoIVxRJdgGb9w-1678160065-0-AY6HuNkNKkz1t3XOZTcJ4Lu6DqdCEK9QQN5yTRKzjI2a56c8oWMI0M1fnfIEar7qRwfdee8StqJf2eyW9n0Sk7U=");
                        RestResponse response = await client.ExecuteAsync(request);
                        Console.WriteLine(response.Content);

                        ChoTotModel data = JsonConvert.DeserializeObject<ChoTotModel>(response.Content);

                        string firstSubject = data.ads.First().subject;
                        string firstLink = data.ads.First().list_id;

                        if (string.IsNullOrEmpty(listLastDataChoTotModel[indexRegion].Subject))
                        {
                            listLastDataChoTotModel[indexRegion].Subject = firstSubject;
                            listLastDataChoTotModel[indexRegion].ID = firstLink;


                            chromeDriver.Navigate().GoToUrl($"https://xe.chotot.com/{listLastDataChoTotModel[indexRegion].ID}.htm");

                            IJavaScriptExecutor javaScriptExecutor = chromeDriver;

                            Thread.Sleep(1000);

                            IWebElement hidePhone = chromeDriver.FindElement(By.XPath("//div[@class='InlineShowPhoneButton_wrapper__NtHmX']"));
                            hidePhone.Click();
                            Thread.Sleep(1000);

                            //javaScriptExecutor.ExecuteScript("arguments[0].click()", hidePhone);
                            string phone = chromeDriver.FindElement(By.XPath("//div[@class='InlineShowPhoneButton_wrapper__NtHmX']")).Text.Split(':')[1].Trim();

                            string namSanXuat = string.Empty;
                            string hopSo = string.Empty;
                            string soKmDaDi = string.Empty;

                            foreach (var p in data.ads.First().@params)
                            {
                                if (p.id == "mfdate")
                                {
                                    namSanXuat = p.value;
                                }
                                else if (p.id == "gearbox")
                                {
                                    hopSo = p.value;
                                }
                                else if (p.id == "mileage_v2")
                                {
                                    soKmDaDi = p.value;
                                }
                            }

                            string pageSource = chromeDriver.PageSource;
                            string hang = string.Empty;
                            string dong = string.Empty;
                            string kieuDang = string.Empty;
                            string xuatXu = string.Empty;
                            string nhienLieu = string.Empty;
                            string soCho = string.Empty;

                            try
                            {
                                hang = pageSource.Split(new[] { "\"id\":\"carbrand\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            try
                            {
                                dong = pageSource.Split(new[] { "\"id\":\"carmodel\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            try
                            {
                                kieuDang = pageSource.Split(new[] { "\"id\":\"cartype\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            try
                            {
                                xuatXu = pageSource.Split(new[] { "\"id\":\"carorigin\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            try
                            {
                                nhienLieu = pageSource.Split(new[] { "\"id\":\"fuel\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            try
                            {
                                soCho = pageSource.Split(new[] { "\"id\":\"carseats\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                            }
                            catch { }

                            ChoTotExcelModel e = new ChoTotExcelModel()
                            {
                                TieuDe = firstSubject,
                                SoDienThoai = phone,
                                GiaBan = data.ads.First().price,
                                SoKmDaDi = soKmDaDi,
                                HopSo = hopSo,
                                NamSX = namSanXuat,
                                Hang = hang,
                                DongXe = dong,
                                XuatXu = xuatXu,
                                KieuDang = kieuDang,
                                NhienLieu = nhienLieu,
                                SoCho = soCho
                            };

                            //string line = $"{e.TieuDe},{e.GiaBan},{e.SoDienThoai},{e.Hang},{e.DongXe},{e.NamSX},{e.HopSo},{e.SoKmDaDi},{e.XuatXu},{e.NhienLieu},{e.KieuDang},{e.SoCho}";

                            listChoTotExcelModels[indexRegion].Add(e);

                            using (ExcelPackage excelPackage = new ExcelPackage("Template.xlsx"))
                            {
                                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.SingleOrDefault(x => x.Name == "Template");

                                for (int i = 0; i < listChoTotExcelModels[indexRegion].Count; i++)
                                {
                                    excelWorksheet.Cells[i + 2, 1].Value = listChoTotExcelModels[indexRegion][i].TieuDe;
                                    excelWorksheet.Cells[i + 2, 2].Value = listChoTotExcelModels[indexRegion][i].GiaBan;
                                    excelWorksheet.Cells[i + 2, 3].Value = listChoTotExcelModels[indexRegion][i].SoDienThoai;
                                    excelWorksheet.Cells[i + 2, 4].Value = listChoTotExcelModels[indexRegion][i].Hang;
                                    excelWorksheet.Cells[i + 2, 5].Value = listChoTotExcelModels[indexRegion][i].DongXe;
                                    excelWorksheet.Cells[i + 2, 6].Value = listChoTotExcelModels[indexRegion][i].NamSX;
                                    excelWorksheet.Cells[i + 2, 7].Value = listChoTotExcelModels[indexRegion][i].HopSo;
                                    excelWorksheet.Cells[i + 2, 8].Value = listChoTotExcelModels[indexRegion][i].SoKmDaDi;
                                    excelWorksheet.Cells[i + 2, 9].Value = listChoTotExcelModels[indexRegion][i].XuatXu;
                                    excelWorksheet.Cells[i + 2, 10].Value = listChoTotExcelModels[indexRegion][i].NhienLieu;
                                    excelWorksheet.Cells[i + 2, 11].Value = listChoTotExcelModels[indexRegion][i].KieuDang;
                                    excelWorksheet.Cells[i + 2, 12].Value = listChoTotExcelModels[indexRegion][i].SoCho;
                                }
                                excelPackage.SaveAs(saveFileChoTot[indexRegion]);
                            }
                        }
                        else
                        {
                            if (listLastDataChoTotModel[indexRegion].Subject != firstSubject)
                            {
                                listLastDataChoTotModel[indexRegion].Subject = firstSubject;
                                listLastDataChoTotModel[indexRegion].ID = firstLink;
                                AddLogChoTot($"{listLastDataChoTotModel[indexRegion].Region}. Có bài viết mới: {listLastDataChoTotModel[indexRegion].Subject}. Link: https://xe.chotot.com/{listLastDataChoTotModel[indexRegion].ID}.htm => Đang xử lý");

                                chromeDriver.Navigate().GoToUrl($"https://xe.chotot.com/{listLastDataChoTotModel[indexRegion].ID}.htm");

                                IJavaScriptExecutor javaScriptExecutor = chromeDriver;

                                Thread.Sleep(1000);

                                IWebElement hidePhone = chromeDriver.FindElement(By.XPath("//div[@class='InlineShowPhoneButton_wrapper__NtHmX']"));
                                hidePhone.Click();
                                Thread.Sleep(1000);

                                //javaScriptExecutor.ExecuteScript("arguments[0].click()", hidePhone);
                                string phone = chromeDriver.FindElement(By.XPath("//div[@class='InlineShowPhoneButton_wrapper__NtHmX']")).Text.Split(':')[1].Trim();

                                string namSanXuat = string.Empty;
                                string hopSo = string.Empty;
                                string soKmDaDi = string.Empty;

                                foreach (var p in data.ads.First().@params)
                                {
                                    if (p.id == "mfdate")
                                    {
                                        namSanXuat = p.value;
                                    }
                                    else if (p.id == "gearbox")
                                    {
                                        hopSo = p.value;
                                    }
                                    else if (p.id == "mileage_v2")
                                    {
                                        soKmDaDi = p.value;
                                    }
                                }

                                string pageSource = chromeDriver.PageSource;
                                string hang = string.Empty;
                                string dong = string.Empty;
                                string kieuDang = string.Empty;
                                string xuatXu = string.Empty;
                                string nhienLieu = string.Empty;
                                string soCho = string.Empty;

                                try
                                {
                                    hang = pageSource.Split(new[] { "\"id\":\"carbrand\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                try
                                {
                                    dong = pageSource.Split(new[] { "\"id\":\"carmodel\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                try
                                {
                                    kieuDang = pageSource.Split(new[] { "\"id\":\"cartype\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                try
                                {
                                    xuatXu = pageSource.Split(new[] { "\"id\":\"carorigin\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                try
                                {
                                    nhienLieu = pageSource.Split(new[] { "\"id\":\"fuel\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                try
                                {
                                    soCho = pageSource.Split(new[] { "\"id\":\"carseats\",\"value\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"," }, StringSplitOptions.None)[0];
                                }
                                catch { }

                                ChoTotExcelModel e = new ChoTotExcelModel()
                                {
                                    TieuDe = firstSubject,
                                    SoDienThoai = phone,
                                    GiaBan = data.ads.First().price,
                                    SoKmDaDi = soKmDaDi,
                                    HopSo = hopSo,
                                    NamSX = namSanXuat,
                                    Hang = hang,
                                    DongXe = dong,
                                    XuatXu = xuatXu,
                                    KieuDang = kieuDang,
                                    NhienLieu = nhienLieu,
                                    SoCho = soCho
                                };

                                //string line = $"{e.TieuDe},{e.GiaBan},{e.SoDienThoai},{e.Hang},{e.DongXe},{e.NamSX},{e.HopSo},{e.SoKmDaDi},{e.XuatXu},{e.NhienLieu},{e.KieuDang},{e.SoCho}";

                                listChoTotExcelModels[indexRegion].Add(e);

                                using (ExcelPackage excelPackage = new ExcelPackage("Template.xlsx"))
                                {
                                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets.SingleOrDefault(x => x.Name == "Template");

                                    for (int i = 0; i < listChoTotExcelModels[indexRegion].Count; i++)
                                    {
                                        excelWorksheet.Cells[i + 2, 1].Value = listChoTotExcelModels[indexRegion][i].TieuDe;
                                        excelWorksheet.Cells[i + 2, 2].Value = listChoTotExcelModels[indexRegion][i].GiaBan;
                                        excelWorksheet.Cells[i + 2, 3].Value = listChoTotExcelModels[indexRegion][i].SoDienThoai;
                                        excelWorksheet.Cells[i + 2, 4].Value = listChoTotExcelModels[indexRegion][i].Hang;
                                        excelWorksheet.Cells[i + 2, 5].Value = listChoTotExcelModels[indexRegion][i].DongXe;
                                        excelWorksheet.Cells[i + 2, 6].Value = listChoTotExcelModels[indexRegion][i].NamSX;
                                        excelWorksheet.Cells[i + 2, 7].Value = listChoTotExcelModels[indexRegion][i].HopSo;
                                        excelWorksheet.Cells[i + 2, 8].Value = listChoTotExcelModels[indexRegion][i].SoKmDaDi;
                                        excelWorksheet.Cells[i + 2, 9].Value = listChoTotExcelModels[indexRegion][i].XuatXu;
                                        excelWorksheet.Cells[i + 2, 10].Value = listChoTotExcelModels[indexRegion][i].NhienLieu;
                                        excelWorksheet.Cells[i + 2, 11].Value = listChoTotExcelModels[indexRegion][i].KieuDang;
                                        excelWorksheet.Cells[i + 2, 12].Value = listChoTotExcelModels[indexRegion][i].SoCho;
                                    }
                                    excelPackage.SaveAs(saveFileChoTot[indexRegion]);
                                }


                            }
                        }
                        indexRegion++;
                    }
                    catch (Exception ex)
                    {
                        if (bIsRunning == false)
                        {
                            return;
                        }

                        AddLogChoTot($"Chạy {listLastDataChoTotModel[indexRegion].Region} bị lỗi => chạy lại");
                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(iTimeSleep);
            }
        }).Start();
        }

        private void PlayAudio()
        {
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("alarm.wav");
            //player.Play();
            //isPlayingAlarm = false;
            //    //  new Thread(() =>
            //    //    {
            //    //      try
            //    {
            //    using (IWavePlayer waveOutDevice = new WaveOut())
            //    {
            //        AudioFileReader audioFileReader = new AudioFileReader(@"alarm.mp3");

            //        waveOutDevice.Init(audioFileReader);
            //        waveOutDevice.Play();
            //    }
            //}
            ////catch
            ////{
            ////}
            ////finally
            //{
            //    isPlayingAlarm = false;
            //}
            //   }).Start();
        }

        private void AddLogChoTot(string log)
        {
            richTextBoxLogChoTot.Invoke(new Action(() =>
            {
                if (richTextBoxLogChoTot.Lines.Length > 1000)
                {
                    richTextBoxLogChoTot.Clear();
                }
                richTextBoxLogChoTot.Text += log + Environment.NewLine;
            }));
        }
        private void AddLogBonBanh(string log)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            listLastDataChoTotModel = new List<LastDataModel>();
            foreach (ChoTotRegion regionChoTot in Enum.GetValues(typeof(ChoTotRegion)))
            {
                LastDataModel lastDataChoTotModel = new LastDataModel()
                {
                    ID = string.Empty,
                    Subject = string.Empty,
                    Region = regionChoTot.ToString()
                };
                listLastDataChoTotModel.Add(lastDataChoTotModel);
            }

            bIsRunning = false;
            iTimeSleep = 0;
            isPlayingAlarm = false;


            dataRegionBonBanh = new Dictionary<string, string>();
            dataRegionBonBanh.Add("Ho_Chi_Minh", "https://bonbanh.com/tp-hcm/oto");
            dataRegionBonBanh.Add("An_Giang", "https://bonbanh.com/an-giang/oto");
            dataRegionBonBanh.Add("Ba_Ria_Vung_Tau", "https://bonbanh.com/ba-ria-vung-tau/oto");
            dataRegionBonBanh.Add("Bac_Lieu", "https://bonbanh.com/bac-lieu/oto");
            dataRegionBonBanh.Add("Ben_Tre", "https://bonbanh.com/ben-tre/oto");
            dataRegionBonBanh.Add("Binh_Duong", "https://bonbanh.com/binh-duong/oto");
            dataRegionBonBanh.Add("Binh_Phuoc", "https://bonbanh.com/binh-phuoc/oto");
            dataRegionBonBanh.Add("Ca_Mau", "https://bonbanh.com/ca-mau/oto");
            dataRegionBonBanh.Add("Can_Tho", "https://bonbanh.com/can-tho/oto");
            dataRegionBonBanh.Add("Dong_Nai", "https://bonbanh.com/dong-nai/oto");
            dataRegionBonBanh.Add("Dong_Thap", "https://bonbanh.com/dong-thap/oto");
            dataRegionBonBanh.Add("Hau_Giang", "https://bonbanh.com/hau-giang/oto");
            dataRegionBonBanh.Add("Kien_Giang", "https://bonbanh.com/kien-giang/oto");
            dataRegionBonBanh.Add("Long_An", "https://bonbanh.com/long-an/oto");
            dataRegionBonBanh.Add("Soc_Trang", "https://bonbanh.com/soc-trang/oto");
            dataRegionBonBanh.Add("Tay_Ning", "https://bonbanh.com/tay-ninh/oto");
            dataRegionBonBanh.Add("Tien_Giang", "https://bonbanh.com/tien-giang/oto");
            dataRegionBonBanh.Add("Tra_Vinh", "https://bonbanh.com/tra-vinh/oto");
            dataRegionBonBanh.Add("Vinh_Long", "https://bonbanh.com/vinh-long/oto");


            listLastDataBonBanhModel = new List<LastDataModel>();

            foreach (var dic in dataRegionBonBanh)
            {
                LastDataModel lastDataModel = new LastDataModel()
                {
                    ID = string.Empty,
                    Subject = string.Empty,
                    Region = dic.Key,
                    LinkGet = dic.Value
                };
                listLastDataBonBanhModel.Add(lastDataModel);
            }


        }

        private void GetBonBanh()
        {
            new Thread(async () =>
            {
                while (bIsRunning)
                {
                    Check.CheckTime();
                    int indexRegion = 0;
                    foreach (var regionData in listLastDataBonBanhModel)
                    {
                        try
                        {
                            var options = new RestClientOptions()
                            {
                                MaxTimeout = -1,
                                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36",
                            };
                            var client = new RestClient(options);
                            var request = new RestRequest(regionData.LinkGet, Method.Get);
                            request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"110\", \"Not A(Brand\";v=\"24\", \"Google Chrome\";v=\"110\"");
                            request.AddHeader("sec-ch-ua-mobile", "?0");
                            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                            request.AddHeader("Upgrade-Insecure-Requests", "1");
                            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                            request.AddHeader("Cookie", "PHPSESSID=4bm35jbj16eme4b3ch9ip9ob52; __ck__=12345; bbredirecturl=aHR0cHM6Ly9ib25iYW5oLmNvbS9vdG8%3D; uadpv=1");
                            RestResponse response = await client.ExecuteAsync(request);
                            Console.WriteLine(response.Content);

                            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                            htmlDoc.LoadHtml(response.Content);

                            var firstSubject = htmlDoc.DocumentNode.SelectNodes("//div[@class='cb2_02']").First().InnerText;

                            var node = htmlDoc.DocumentNode.SelectNodes("//li[@class='car-item row1']").First();
                            string firstLink = node.SelectSingleNode("a").Attributes["href"].Value;

                            //string firstLink = string.Empty;
                            //foreach (HtmlNode node in nodes)
                            //{
                            //    firstLink = node.SelectSingleNode("a").Attributes["href"].Value;
                            //}


                            if (string.IsNullOrEmpty(listLastDataBonBanhModel[indexRegion].Subject))
                            {
                                listLastDataBonBanhModel[indexRegion].Subject = firstSubject;
                                listLastDataBonBanhModel[indexRegion].ID = firstLink;
                                AddLogBonBanh($"{listLastDataBonBanhModel[indexRegion].Region}. Có bài viết mới: {listLastDataBonBanhModel[indexRegion].Subject}. Link: https://bonbanh.com/{listLastDataBonBanhModel[indexRegion].ID}");





                            }
                            else
                            {
                                if (listLastDataBonBanhModel[indexRegion].Subject != firstSubject)
                                {
                                    listLastDataBonBanhModel[indexRegion].Subject = firstSubject;
                                    listLastDataBonBanhModel[indexRegion].ID = firstLink;
                                    AddLogBonBanh($"{listLastDataBonBanhModel[indexRegion].Region}. Có bài viết mới: {listLastDataBonBanhModel[indexRegion].Subject}. Link: https://bonbanh.com/{listLastDataBonBanhModel[indexRegion].ID}");
                                    if (isPlayingAlarm == false)
                                    {
                                        PlayAudio();
                                    }
                                }
                            }
                            Thread.Sleep(1000);
                            indexRegion++;
                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(iTimeSleep);
                }
            }).Start();
        }

        private void txtTimeSleep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (bIsRunning == false)
            {
                listChoTotExcelModels = new List<List<ChoTotExcelModel>>();

                btnStop.Enabled = true;
                btnStart.Enabled = false;
                txtTimeSleep.Enabled = false;
                richTextBoxLogChoTot.Clear();

                listLastDataChoTotModel = new List<LastDataModel>();
                foreach (ChoTotRegion regionChoTot in Enum.GetValues(typeof(ChoTotRegion)))
                {
                    LastDataModel lastDataChoTotModel = new LastDataModel()
                    {
                        ID = string.Empty,
                        Subject = string.Empty,
                        Region = regionChoTot.ToString()
                    };
                    listLastDataChoTotModel.Add(lastDataChoTotModel);


                    List<ChoTotExcelModel> choTotExcelModels = new List<ChoTotExcelModel>();
                    listChoTotExcelModels.Add(choTotExcelModels);
                }


                listLastDataBonBanhModel = new List<LastDataModel>();

                foreach (var dic in dataRegionBonBanh)
                {
                    LastDataModel lastDataModel = new LastDataModel()
                    {
                        ID = string.Empty,
                        Subject = string.Empty,
                        Region = dic.Key,
                        LinkGet = dic.Value
                    };
                    listLastDataBonBanhModel.Add(lastDataModel);
                }

                iTimeSleep = int.Parse(txtTimeSleep.Text) * 1000;
                bIsRunning = true;
                AddLogChoTot($"Chạy vòng đầu tiên sau khi mở tool nên không thông báo");
                //      AddLogBonBanh($"Chạy lần đầu mở tool nên thông báo tất cả nhưng không thông báo âm thanh");



                CloseAllChromeDriver();
                CloseAllChrome();
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("user-data-dir=" + Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data"));
                chromeOptions.AddArgument("--disable-notifications");
                chromeOptions.AddArguments("--disable-infobars");
                chromeOptions.AddExcludedArgument("enable-automation");
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                chromeOptions.AddArguments("--blink-settings=imagesEnabled=false");
                chromeOptions.BinaryLocation = Environment.CurrentDirectory + "\\GoogleChromePortable\\App\\Chrome-bin\\chrome.exe";

                chromeOptions.AddArgument("headless");

                chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);
                chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(30);






                GetChoTot();
                //  GetBonBanh();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bIsRunning = false;


            try
            {
                chromeDriver.Close();
                chromeDriver.Quit();
            }
            catch
            {
                try
                {
                    chromeDriver.Quit();
                    chromeDriver.Close();
                }
                catch
                {
                    try
                    {
                        chromeDriver.Quit();
                    }
                    catch
                    {
                    }
                }
            }



            txtTimeSleep.Enabled = true;
            btnStop.Enabled = false;
            btnStart.Enabled = true;


        }

        private void richTextBoxLogChoTot_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void richTextBoxLogBonBanh_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void btnDeleteLogChoTot_Click(object sender, EventArgs e)
        {
            richTextBoxLogChoTot.Clear();
        }

        private void btnDeleteLogBonBanh_Click(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                chromeDriver.Close();
                chromeDriver.Quit();
            }
            catch
            {
                try
                {
                    chromeDriver.Quit();
                    chromeDriver.Close();
                }
                catch
                {
                    try
                    {
                        chromeDriver.Quit();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            CloseAllChromeDriver();
            CloseAllChrome();
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("user-data-dir=" + Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data"));
            chromeOptions.AddArgument("--disable-notifications");
            chromeOptions.AddArguments("--disable-infobars");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            chromeOptions.AddArguments("--blink-settings=imagesEnabled=false");
            chromeOptions.BinaryLocation = Environment.CurrentDirectory + "\\GoogleChromePortable\\App\\Chrome-bin\\chrome.exe";
            chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);
            chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(30);

        }
    }
    public enum ChoTotRegion
    {
        Ho_Chi_Minh = 13000,
        Can_Tho = 5027,
        Binh_Duong = 2011,
        An_Giang = 5024,
        Ba_Ria_Vung_Tau = 2010,
        Bac_Lieu = 5025,
        Ben_Tre = 5026,
        Binh_Phuoc = 2012,
        Ca_Mau = 5028,
        Dong_Thap = 5029,
        Dong_Nai = 2013,
        Hau_Giang = 5030,
        Lam_Dong = 9057,
        Long_An = 5032,
        Soc_Trang = 5033,
        Tay_Ninh = 2014,
        Tien_Giang = 5034
    }
}