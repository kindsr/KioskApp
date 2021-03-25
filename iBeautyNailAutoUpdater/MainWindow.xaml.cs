using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Net.NetworkInformation;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models;
using System.Xml;

namespace iBeautyNailAutoUpdater
{
    public class MyData
    {
        public string zzFileName { get; set; }
        public DateTime zzLastWriteTime { get; set; }
        public long zzLength { get; set; }
        public string zzType { get; set; }

        private static List<MyData> instance;

        public static List<MyData> GetInstance()
        {
            if (instance == null)
                instance = new List<MyData>();

            return instance;
        }
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SERVER_URL = "kindsr82.iptime.org:2121/nailpod_Init/";
        private string LOGIN_ID = "gituser";
        private string LOGIN_PW = "irobo!234";
        private string UPDATEFILEINFO = "update.json";
        private string DELETEFILEINFO = "delete.json";
        private string BASEPATH = @"C:\";
        private string START_PROGRAM = "iBeautyNailApp.exe";
        private string BASEPATH_FOR_UPDATE = @"C:\nailpod_update_contents";
        private bool IS_DEV = true;
        private bool IS_TOPMOST = false;
        private int WAITING_DEVICES = 90;

        private List<FileInfoModel> downloadFileList = new List<FileInfoModel>();          // 서버로부터 다운로드 해야할 파일
        private List<FileInfoModel> deleteFileList;          // 삭제 해야할 파일
        private WebClient updateServerConnector;
        private WebClient deleteServerConnector;
        private Uri serverUrl;                          // 서버측 주소
        private Uri updateFileInfoUrl;                  // 업데이트 파일 정보의 주소
        private Uri deleteFileInfoUrl;                  // 삭제 파일 정보의 주소
        private Stream updateInfoStream;                // 업데이트 정보 스트림
        private Stream deleteInfoStream;                // 삭제 정보 스트림
        private int index;                              // 파일 인덱스
        private int lastIndex;
        private DirectoryInfo currentDir;               // 현재 디렉토리
        private Thread progressBarThread;               // 
        private bool m_bLoop;
        IniFile iniFile = new IniFile();
        private string currentProcPath;
        private bool isLoaded;

        public IntPtr HWND { get; set; }

        private List<FileInfoModel> forList = new List<FileInfoModel>();
        private List<FileInfoModel> checkedList = new List<FileInfoModel>();
        private List<FileInfoModel> serverFileInfoList = new List<FileInfoModel>();

        // for New Update
        private bool isNewUpdateAvailable;
        private string serverAppVersion;
        private string currentAppVersion;
        private string currentMachineID;

        private List<UpdateProcessObj> deleteContentsList = new List<UpdateProcessObj>();
        private List<UpdateProcessObj> updateContentsList = new List<UpdateProcessObj>();

        public MainWindow()
        {
            InitializeComponent();

            isLoaded = false;
            listChkbox.Visibility = Visibility.Hidden;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Closed += new EventHandler(MainWindow_Closed);
            this.ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            if (isLoaded) return;

            // ini file read
            if (File.Exists("iBeautyNailAutoUpdater.ini"))
            {
                iniFile.Load("iBeautyNailAutoUpdater.ini");
                SERVER_URL = iniFile["iBeautyNailAutoUpdater"]["ServerUrl"].ToString();
                LOGIN_ID = iniFile["iBeautyNailAutoUpdater"]["LoginId"].ToString();
                LOGIN_PW = iniFile["iBeautyNailAutoUpdater"]["LoginPw"].ToString();
                UPDATEFILEINFO = iniFile["iBeautyNailAutoUpdater"]["UpdateFileInfo"].ToString();
                DELETEFILEINFO = iniFile["iBeautyNailAutoUpdater"]["DeleteFileInfo"].ToString();
                BASEPATH = iniFile["iBeautyNailAutoUpdater"]["Basepath"].ToString();
                BASEPATH_FOR_UPDATE = iniFile["iBeautyNailAutoUpdater"]["BasepathForUpdateContents"].ToString();
                START_PROGRAM = iniFile["iBeautyNailAutoUpdater"]["StartProgram"].ToString();
                IS_DEV = iniFile["iBeautyNailAutoUpdater"]["IsDev"].ToBool();
                WAITING_DEVICES = iniFile["iBeautyNailAutoUpdater"]["WaitingDevices"].ToInt();
            }

            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.Contains("/Auto"))
                {
                    IS_DEV = false;
                }

                if (arg.Contains("/Topmost"))
                {
                    IS_TOPMOST = true;
                }
            }

            if (IS_DEV)
            {
                btnUpdate.Visibility = Visibility.Visible;
                btnRun.Visibility = Visibility.Visible;
                btnCreateUpdateJson.Visibility = Visibility.Visible;
                btnCreateDeleteJson.Visibility = Visibility.Visible;
            }
            else
            {
                btnUpdate.Visibility = Visibility.Collapsed;
                btnRun.Visibility = Visibility.Collapsed;
                btnCreateUpdateJson.Visibility = Visibility.Collapsed;
                btnCreateDeleteJson.Visibility = Visibility.Collapsed;
            }

            // 업데이터 바로 실행 후 네트워크 연결될때까지 대기
            txtPartial.Text = "Waiting for devices.";
            Task.Factory.StartNew(() => Thread.Sleep(WAITING_DEVICES * 1000))
                .ContinueWith(t =>
                {
                    txtPartial.Text = "Check Updates.";
                }, TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(t => Thread.Sleep(3000))
                .ContinueWith(t =>
                {
                    //btnUpdate.Visibility = Visibility.Visible;
                    //btnRun.Visibility = Visibility.Visible;

                    // 네트워크 체크
                    if (!IsEstablishedNetwork())
                    {
                        txtPartial.Text = "Network is not available.";
                        Console.WriteLine("Network is not available.");
                        //return;
                        Thread.Sleep(100);

                        // 네트웍 안되도 그냥 실행
                        AppStart();
                        return;
                    }

                    HWND = new WindowInteropHelper(this).Handle;

                    m_bLoop = true;
                    lastIndex = index = 0;
                    currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
                    progressBarThread = new Thread(new ThreadStart(Step));
                    progressBarThread.Start();
                    Thread.Sleep(100);

                    var filePath = @"C:\nailpod\Configs\application.config";
                    if (File.Exists(filePath))
                    {
                        isNewUpdateAvailable = true;

                        currentProcPath = BASEPATH;
                        serverUrl = new Uri("ftp://" + SERVER_URL);

                        try
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(filePath);
                            XmlElement root = xmldoc.DocumentElement;

                            XmlNodeList nodes = root.ChildNodes;

                            XmlElement appNode = (XmlElement)root.SelectSingleNode("machine");
                            currentMachineID = appNode.GetAttribute("id");
                            currentAppVersion = appNode.GetAttribute("appversion");

                            // API call - SelectAppVersionAsync
                            var res = Task.Run(() => SelectAppVersion()).Result;
                            serverAppVersion = res.Version;

                            // 장비별 시작시간 조금씩 변경
                            Thread.Sleep(Int32.Parse(currentMachineID) * 3);

                            // 파일 삭제
                            deleteContentsList = Task.Run(() => SelectDeleteContents(currentAppVersion)).Result;
                            DeleteContents(deleteContentsList);

                            if (serverAppVersion == currentAppVersion)
                            {
                                AppStart();
                            }
                            else
                            {
                                try
                                {
                                    // 다운로드 리스트
                                    updateContentsList = Task.Run(async () => await SelectUpdateContents(Int32.Parse(currentMachineID), currentAppVersion)).Result;

                                    Thread.Sleep(1000);

                                    UpdateContents(updateContentsList);
                                }
                                catch (Exception ex)
                                {
                                    AppStart();
                                }
                            }
                        }
                        catch (IOException ex)
                        {
                            AppStart();
                        }
                    }
                    // 기존 로직
                    else
                    {
                        isNewUpdateAvailable = false;

                        btnCreateUpdateJson.Visibility = Visibility.Visible;
                        btnCreateDeleteJson.Visibility = Visibility.Visible;

                        if (IS_DEV == true)
                        {
                            this.Height = 400;
                            listChkbox.Visibility = Visibility.Hidden;
                        }

                        else
                        {
                            this.Height = 174;
                            listChkbox.Visibility = Visibility.Hidden;
                            currentProcPath = BASEPATH;
                            ConnectDeleteServer(SERVER_URL, DELETEFILEINFO);
                        }
                    }

                    isLoaded = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            // Content Rendered 로 이동
            btnUpdate.Visibility = Visibility.Collapsed;
            btnRun.Visibility = Visibility.Collapsed;
            btnCreateUpdateJson.Visibility = Visibility.Collapsed;
            btnCreateDeleteJson.Visibility = Visibility.Collapsed;
        }

        #region Old Update
        private void ConnectUpdateServer(string url, string fileInfo)
        {
            updateServerConnector = new WebClient();
            updateServerConnector.Credentials = new NetworkCredential(LOGIN_ID, LOGIN_PW);
            updateServerConnector.OpenReadCompleted += new OpenReadCompletedEventHandler(updateServerConnector_OpenReadCompleted);
            serverUrl = new Uri("ftp://" + url);
            updateFileInfoUrl = new Uri(serverUrl.ToString() + fileInfo);

            try
            {
                updateServerConnector.OpenReadAsync(updateFileInfoUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Close();
                if (IS_DEV != true)
                    AppStart();
            }
        }

        private void ConnectDeleteServer(string url, string fileInfo)
        {
            deleteServerConnector = new WebClient();
            deleteServerConnector.Credentials = new NetworkCredential(LOGIN_ID, LOGIN_PW);
            deleteServerConnector.OpenReadCompleted += new OpenReadCompletedEventHandler(deleteServerConnector_OpenReadCompleted);
            serverUrl = new Uri("ftp://" + url);
            deleteFileInfoUrl = new Uri(serverUrl.ToString() + fileInfo);

            try
            {
                deleteServerConnector.OpenReadAsync(deleteFileInfoUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Update 실행
                ConnectUpdateServer(url, fileInfo);
            }
        }

        /// <summary>
        /// updateServerConnector의 OpenReadAsync가 완료되면 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateServerConnector_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            try
            {
                updateInfoStream = e.Result;
                if (updateInfoStream != null)
                {
                    CheckUpdate(updateInfoStream);

                    if (SERVER_URL == "kindsr82.iptime.org:2121/nailpod_Update/")
                    {
                        listChkbox.SelectAll();
                    }


                    if (listChkbox.SelectedItems.Count > 0)
                    {
                        forList.Clear();
                        foreach (MyData md in listChkbox.SelectedItems)
                        {
                            forList.Add(new FileInfoModel() { FileName = md.zzFileName, LastWriteTime = md.zzLastWriteTime, Length = md.zzLength, Type = md.zzType });
                        }

                        List<FileInfoModel> final = new List<FileInfoModel>();
                        foreach (FileInfoModel i1 in serverFileInfoList)
                        {
                            foreach (FileInfoModel i2 in forList)
                            {
                                if (i1.FileName.Contains(i2.FileName + "\\"))
                                {
                                    final.Add(i1);
                                }
                            }
                        }

                        // 로컬 파일
                        List<FileInfoModel> localFileInfoList = new List<FileInfoModel>();

                        foreach (FileInfoModel fi in forList)
                        {
                            string p = Path.Combine(BASEPATH, fi.FileName);
                            DirectorySecurity directorySecurity = new DirectorySecurity();
                            directorySecurity.SetAccessRuleProtection(true, false);

                            if (!Directory.Exists(p))
                            {
                                string user = Environment.UserDomainName + "\\" + Environment.UserName;
                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                Directory.CreateDirectory(p);
                            }
                            string[] allFiles = Directory.GetFileSystemEntries(p, "*", SearchOption.AllDirectories);

                            foreach (var file in allFiles)
                            {
                                FileInfo info = new FileInfo(file);

                                if (info.Attributes.HasFlag(FileAttributes.Directory))
                                {
                                    localFileInfoList.Add(new FileInfoModel()
                                    {
                                        FileName = info.FullName,
                                        // FileName = file.ToString(),
                                        LastWriteTime = info.LastWriteTime,
                                        Length = 0,
                                        Type = "Dir"
                                    });
                                }
                                else
                                {
                                    localFileInfoList.Add(new FileInfoModel()
                                    {
                                        FileName = info.FullName,
                                        //FileName = file.ToString(),
                                        LastWriteTime = System.IO.File.GetCreationTime(file), //info.LastWriteTime,
                                        Length = info.Length,
                                        Type = "File"
                                    });
                                }
                            }

                        }

                        // 서버와 로컬 파일 비교
                        // 파일이름이 같고 생성일이 다른것
                        var downlist1 = (from x in serverFileInfoList
                                         where localFileInfoList.Any(y => (BASEPATH + x.FileName).Equals(y.FileName) && x.LastWriteTime != y.LastWriteTime) == true
                                         select x).ToList();

                        //serverFileInfoList에만 있는 파일
                        var downlist2 = new List<FileInfoModel>();

                        foreach (FileInfoModel f1 in forList)
                        {
                            // 서버에 있는 "폴더명\\"으로 시작하는 리스트
                            List<FileInfoModel> t1 = (from x in serverFileInfoList
                                                      where x.FileName.StartsWith(f1.FileName + @"\")
                                                      select x).ToList();


                            // 서버에 있는 "폴더명\\"으로 시작하는 리스트 중 로컬에는 존재하지 않는 파일
                            List<FileInfoModel> t2 = (from x in t1
                                                      where localFileInfoList.Any(y => (BASEPATH + x.FileName).Equals(y.FileName)) == false
                                                      select x).ToList();

                            downlist2.AddRange(t2);
                        }

                        downloadFileList = downlist1.Concat(downlist2).ToList();

                        // iBeautyNailApp Kill
                        string progName = Path.GetFileNameWithoutExtension("iBeautyNailApp.exe");
                        Process[] processList = Process.GetProcessesByName(progName);

                        if (processList.Length >= 1)
                        {
                            for (int i = 0; i < processList.Length; i++)
                                processList[i].Kill();
                        }

                        // NailManager, PartnerDRV Kill
                        string progName2 = Path.GetFileNameWithoutExtension("PartnerDRV.exe");
                        Process[] processList2 = Process.GetProcessesByName(progName2);

                        if (processList2.Length >= 1)
                        {
                            for (int i = 0; i < processList2.Length; i++)
                                processList2[i].Kill();
                        }

                        string progName3 = Path.GetFileNameWithoutExtension("NailPrinterManager.exe");
                        Process[] processList3 = Process.GetProcessesByName(progName3);

                        if (processList3.Length >= 1)
                        {
                            for (int i = 0; i < processList3.Length; i++)
                                processList3[i].Kill();
                        }

                        StartDownload();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot connect to the server");
                //Close();
                if (IS_DEV != true)
                    AppStart();
            }
        }

        /// <summary>
        /// 서버로부터 업데이트 파일 리스트를 받아옴
        /// </summary>
        private void CheckUpdate(Stream stream)
        {
            try
            {
                // 서버의 파일
                StreamReader sr = new StreamReader(stream);
                serverFileInfoList = JsonConvert.DeserializeObject<List<FileInfoModel>>(sr.ReadToEnd());

                if (listChkbox.SelectedItems.Count == 0)
                {
                    foreach (FileInfoModel fim in serverFileInfoList)   //최상위 폴더,파일만 보여줌
                    {
                        if (!fim.FileName.Contains(@"\"))
                            forList.Add(fim);
                    }


                    foreach (FileInfoModel i in forList)
                    {
                        MyData.GetInstance().Add(new MyData() { zzFileName = i.FileName, zzLastWriteTime = i.LastWriteTime, zzLength = i.Length, zzType = i.Type });
                    }
                    listChkbox.ItemsSource = MyData.GetInstance();
                    listChkbox.FontSize = 15;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot check the update information. :: " + ex.ToString());
                //Close();
            }
        }


        /// <summary>
        /// deleteServerConnector의 OpenReadAsync가 완료되면 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteServerConnector_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                deleteInfoStream = e.Result;
                if (deleteInfoStream != null)
                {
                    deleteFileList = new List<FileInfoModel>();
                    deleteFileList = CheckDelete(currentProcPath, deleteInfoStream);

                    // iBeautyNailApp Kill
                    string progName = Path.GetFileNameWithoutExtension("iBeautyNailApp.exe");
                    Process[] processList = Process.GetProcessesByName(progName);

                    if (processList.Length >= 1)
                    {
                        for (int i = 0; i < processList.Length; i++)
                            processList[i].Kill();
                    }

                    // NailManager, PartnerDRV Kill
                    string progName2 = Path.GetFileNameWithoutExtension("PartnerDRV.exe");
                    Process[] processList2 = Process.GetProcessesByName(progName2);

                    if (processList2.Length >= 1)
                    {
                        for (int i = 0; i < processList2.Length; i++)
                            processList2[i].Kill();
                    }

                    string progName3 = Path.GetFileNameWithoutExtension("NailPrinterManager.exe");
                    Process[] processList3 = Process.GetProcessesByName(progName3);

                    if (processList3.Length >= 1)
                    {
                        for (int i = 0; i < processList3.Length; i++)
                            processList3[i].Kill();
                    }

                    // Delete files
                    foreach (var file in deleteFileList)
                    {
                        try
                        {
                            if (File.Exists(currentProcPath + "\\" + file.FileName) && file.Type != "Dir")
                                File.Delete(currentProcPath + "\\" + file.FileName);
                            else if (File.Exists(currentProcPath + "\\" + file.FileName) && file.Type == "Dir")
                                Directory.Delete(currentProcPath + "\\" + file.FileName, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Delete file error!" + ex.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot connect to the server." + ex.ToString());
            }

            // 완료 후 Update 실행
            ConnectUpdateServer(SERVER_URL, UPDATEFILEINFO);
        }

        /// <summary>
        /// 서버로부터 업데이트 파일 리스트를 받아옴
        /// </summary>
        private List<FileInfoModel> CheckDelete(string path, Stream stream)
        {
            if (stream == null) return null;

            try
            {
                // 서버의 파일
                StreamReader sr = new StreamReader(stream);
                List<FileInfoModel> serverFileInfoList = JsonConvert.DeserializeObject<List<FileInfoModel>>(sr.ReadToEnd());

                return serverFileInfoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot check the delete information. :: " + ex.ToString());
                //Close();
            }

            return null;
        }

        /// <summary>
        /// 서버로부터 파일들을 다운로드
        /// </summary>
        private void StartDownload()
        {
            try
            {
                index = 0;
                TotalDownloadProgressBar.Value = 0;

                downloadFile(downloadFileList.AsEnumerable());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot download. ::" + ex.ToString());
                //Close();
            }
        }

        private void Step()
        {
            while (m_bLoop)
            {
                if (lastIndex < index)
                {
                    lastIndex++;
                }
                Thread.Sleep(1000);
            }
        }

        private Queue<string> _downloadUrls = new Queue<string>();


        private void downloadFile(IEnumerable<FileInfoModel> urls)
        {
            try
            {
                foreach (var url in urls)
                {
                    if (url.Type == "Dir")
                    {
                        if (!Directory.Exists(Path.Combine(currentProcPath, url.FileName)))
                            Directory.CreateDirectory(Path.Combine(currentProcPath, url.FileName));

                        DeleteUpdateProcess(Int32.Parse(currentMachineID??"0"), url.FileName);
                    }
                    else
                    {
                        _downloadUrls.Enqueue(url.FileName);
                    }

                }
                // Starts the download
                DownloadFile();
            }

            catch (Exception e1)
            {

            }

        }

        private void DownloadFile()
        {
            try
            {
                if (_downloadUrls.Any())
                {
                    listChkbox.IsEnabled = false;

                    WebClient client = new WebClient();
                    client.Credentials = new NetworkCredential(LOGIN_ID, LOGIN_PW);
                    client.DownloadProgressChanged += client_DownloadProgressChanged;
                    client.DownloadFileCompleted += client_DownloadFileCompleted;

                    //var url = _downloadUrls.Dequeue();
                    string fileName = _downloadUrls.Dequeue();

                    txtPartial.Text = fileName;
                    var url = new Uri(serverUrl.ToString() + fileName);

                    client.DownloadFileAsync(url, Path.Combine(currentProcPath, fileName));
                    return;
                }

                // End of the download
                listChkbox.IsEnabled = true;
                txtTotal.Text = "Download Complete";
                TotalDownloadProgressBar.Value = TotalDownloadProgressBar.Maximum;
                LogFileWrite();

                if (isNewUpdateAvailable)
                {
                    // master table update
                    Task.Run(() => UpdateUpdateYN(Int32.Parse(currentMachineID), "Y"));

                    // AppVersion update
                    var filePath = @"C:\nailpod\Configs\application.config";
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(filePath);
                            XmlElement root = xmldoc.DocumentElement;

                            XmlNodeList nodes = root.ChildNodes;

                            XmlElement appNode = (XmlElement)root.SelectSingleNode("machine");

                            appNode.SetAttribute("appversion", serverAppVersion);

                            root.ReplaceChild(appNode, appNode);
                            xmldoc.Save(filePath);
                        }
                        catch (IOException ex)
                        {
                        }
                    }
                }

                if (IS_DEV != true)
                    AppStart();
            }

            catch (Exception e) //조심
            {

            }

        }

        /// <summary>
        /// 파일 다운로드 상황 반영
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                Console.WriteLine(string.Format("download file completed error! {0} :: {1}", txtPartial.Text, e.Error.ToString()));
                //DownloadFile();
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(LOGIN_ID, LOGIN_PW);
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                // 오류시 다음파일
                string fileName = _downloadUrls.Dequeue();
                txtPartial.Text = fileName;
                var url = new Uri(serverUrl.ToString() + txtPartial.Text);
                client.DownloadFileAsync(url, Path.Combine(currentProcPath, txtPartial.Text));
                return;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
                Console.WriteLine(string.Format("download file completed cancelled! {0} :: {1}", txtPartial.Text, e.Cancelled.ToString()));
            }
            index++;
            TotalDownloadProgressBar.Value = (Convert.ToDouble(index) / Convert.ToDouble(downloadFileList.Count)) * 100;
            txtTotal.Text = string.Format("{0:0.0} %", (Convert.ToDouble(index) / Convert.ToDouble(downloadFileList.Count)) * 100);

            if (isNewUpdateAvailable)
            {
                // DeleteUpdateProcess
                DeleteUpdateProcess(Int32.Parse(currentMachineID??"0"), txtPartial.Text);
            }

            DownloadFile();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write(".");
            PartialDownloadProgressBar.Value = e.ProgressPercentage;
        }
        #endregion

        private void LogFileWrite()
        {
            try
            {
                FileStream fs = new FileStream(currentDir.FullName + "\\" + "log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("<업그레이드 정보>");
                sw.WriteLine("<업데이트날짜>");
                sw.WriteLine("{0}", DateTime.Today.ToLongDateString());
                sw.WriteLine("<업데이트 파일>");
                for (int i = 0; i < downloadFileList.Count; i++)
                    sw.WriteLine("{0}", downloadFileList[i].FileName);
                sw.Close();
                //fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot write log file. :: " + ex.ToString());
            }
        }

        private void AppStart()
        {
            this.Topmost = false;

            if (File.Exists(Path.Combine(@"C:\nailpod", START_PROGRAM)))
            {
                string start_program = Path.Combine(@"C:\nailpod", START_PROGRAM);
                Process proc = new Process();
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo(start_program, IS_TOPMOST ? "/Topmost" : "");
                proc.Start();
            }

            Close();
        }

        // 네트워크 체크 함수
        private bool IsEstablishedNetwork()
        {
            bool networkUp = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;
            if (networkUp)
            {
                string addr = "www.google.com";
                if (string.IsNullOrEmpty(addr))
                {
                    pingResult = true;
                }
                else
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(addr, 300);
                    pingResult = reply.Status == IPStatus.Success;
                }
            }
            //return networkUp & pingResult;
            return networkUp;
        }

        #region New Update
        private void MakeQuery(List<FileInfoModel> fileInfoList)
        {
            string res = string.Empty;
            string fileName = "insert_query.txt";
            int i = 1;

            foreach (var f in fileInfoList)
            {
                res += string.Format("INSERT INTO update_contents VALUES ({6}, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\");\n", f.FileName.Replace("\\", "\\\\\\\\"), f.Type, f.LastWriteTime.ToString("yyyy-MM-dd hh:mm:ss"), "1.0", "N", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), i++);
            }

            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, res);
        }

        private void DeleteContents(List<UpdateProcessObj> deleteContentsList)
        {
            // iBeautyNailApp Kill
            string progName = Path.GetFileNameWithoutExtension("iBeautyNailApp.exe");
            Process[] processList = Process.GetProcessesByName(progName);

            if (processList.Length >= 1)
            {
                for (int i = 0; i < processList.Length; i++)
                    processList[i].Kill();
            }

            // NailManager, PartnerDRV Kill
            string progName2 = Path.GetFileNameWithoutExtension("PartnerDRV.exe");
            Process[] processList2 = Process.GetProcessesByName(progName2);

            if (processList2.Length >= 1)
            {
                for (int i = 0; i < processList2.Length; i++)
                    processList2[i].Kill();
            }

            string progName3 = Path.GetFileNameWithoutExtension("NailPrinterManager.exe");
            Process[] processList3 = Process.GetProcessesByName(progName3);

            if (processList3.Length >= 1)
            {
                for (int i = 0; i < processList3.Length; i++)
                    processList3[i].Kill();
            }

            // Delete files
            foreach (var file in deleteContentsList)
            {
                try
                {
                    if (File.Exists(currentProcPath + "\\" + file.FileName.Replace(@"\\\\", @"\")) && file.FileType != "Dir")
                        File.Delete(currentProcPath + "\\" + file.FileName.Replace(@"\\\\", @"\"));
                    else if (File.Exists(currentProcPath + "\\" + file.FileName.Replace(@"\\\\", @"\")) && file.FileType == "Dir")
                        Directory.Delete(currentProcPath + "\\" + file.FileName.Replace(@"\\\\", @"\"), false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Delete file error!" + ex.ToString());
                }

            }
        }

        private void UpdateContents(List<UpdateProcessObj> updateContentsList)
        {

            //downloadFileList 
            foreach (var f in updateContentsList)
            {
                if (string.IsNullOrEmpty(f.FileName)) continue;
                downloadFileList.Add(new FileInfoModel() { FileName = f.FileName.Replace(@"\\\\", @"\").Replace(@"\\", @"\"), Type = f.FileType });
            }

            // iBeautyNailApp Kill
            string progName = Path.GetFileNameWithoutExtension("iBeautyNailApp.exe");
            Process[] processList = Process.GetProcessesByName(progName);

            if (processList.Length >= 1)
            {
                for (int i = 0; i < processList.Length; i++)
                    processList[i].Kill();
            }

            // NailManager, PartnerDRV Kill
            string progName2 = Path.GetFileNameWithoutExtension("PartnerDRV.exe");
            Process[] processList2 = Process.GetProcessesByName(progName2);

            if (processList2.Length >= 1)
            {
                for (int i = 0; i < processList2.Length; i++)
                    processList2[i].Kill();
            }

            string progName3 = Path.GetFileNameWithoutExtension("NailPrinterManager.exe");
            Process[] processList3 = Process.GetProcessesByName(progName3);

            if (processList3.Length >= 1)
            {
                for (int i = 0; i < processList3.Length; i++)
                    processList3[i].Kill();
            }

            StartDownload();
        }
        #endregion

        #region ClickEvent
        private void btnCreateJson_Click(object sender, RoutedEventArgs e)
        {
            var path = BASEPATH_FOR_UPDATE;
            if (Directory.Exists(path) == false) return;

            var fileName = String.Empty;

            fileName = UPDATEFILEINFO;

            List<FileInfoModel> fileInfoList = new List<FileInfoModel>();

            // read base directory (BASEPATH = D:\Projects_C#\iBeautyNail_Renewal)
            string[] allFiles = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.FullName.Contains(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location))) continue;
                if (info.FullName.ToLower().Contains("logs")) continue;
                if (info.FullName.ToLower().Contains("newtonsoft")) continue;
                if (info.FullName.ToLower().Contains("delete")) continue;
                if (info.FullName.ToLower().Contains(UPDATEFILEINFO) || info.FullName.ToLower().Contains(DELETEFILEINFO))
                    continue;

                if (info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    fileInfoList.Add(new FileInfoModel()
                    {
                        FileName = info.FullName.Replace(path + "\\", ""),
                        LastWriteTime = info.LastWriteTime,
                        Length = 0,
                        Type = "Dir"
                    });
                }
                else
                {
                    fileInfoList.Add(new FileInfoModel()
                    {
                        FileName = info.FullName.Replace(path + "\\", ""),
                        LastWriteTime = info.LastWriteTime,
                        Length = info.Length,
                        Type = "File"
                    });
                }
            }

            // make update.json
            var json = JsonConvert.SerializeObject(fileInfoList);

            // for New Update
            MakeQuery(fileInfoList);
            try
            {
                if (string.IsNullOrEmpty(serverAppVersion))
                    serverAppVersion = Task.Run(() => SelectAppVersion()).Result.Version;
                var newServerAppVersion = serverAppVersion.Split('.')[0] + '.' + (int.Parse(serverAppVersion.Split('.')[1]) + 1).ToString();
                Task.Run(() => UpsertUpdateContents(json, newServerAppVersion));
                Task.Run(() => UpdateAppVersion(newServerAppVersion));
                Task.Run(() => UpdateUpdateYN(-1, "N"));
            }
            catch (Exception ex)
            {

            }

            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }

        private void btnDeleteJson_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(BASEPATH_FOR_UPDATE, "delete");
            if (Directory.Exists(path) == false) return;

            var fileName = String.Empty;

            fileName = DELETEFILEINFO;


            List<FileInfoModel> fileInfoList = new List<FileInfoModel>();
            List<FileInfoModel> folderInfoList = new List<FileInfoModel>();

            // read base directory (BASEPATH = D:\Projects_C#\iBeautyNail_Renewal)
            string[] allFiles = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.FullName.Contains(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location))) continue;
                if (info.FullName.ToLower().Contains("logs")) continue;
                if (info.FullName.ToLower().Contains("newtonsoft")) continue;

                if (info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    folderInfoList.Add(new FileInfoModel()
                    {
                        FileName = info.FullName.Replace(path + "\\", ""),
                        LastWriteTime = info.LastWriteTime,
                        Length = 0,
                        Type = "Dir"
                    });
                }
                else
                {
                    fileInfoList.Add(new FileInfoModel()
                    {
                        FileName = info.FullName.Replace(path + "\\", ""),
                        LastWriteTime = info.LastWriteTime,
                        Length = info.Length,
                        Type = "File"
                    });
                }
            }

            // make update.json
            folderInfoList.Reverse();
            var json = JsonConvert.SerializeObject(fileInfoList.Concat(folderInfoList).ToList());

            // for New Update
            try
            {
                var jsonForNew = JsonConvert.SerializeObject(fileInfoList);
                Task.Run(() => UpdateDelYN(jsonForNew));
            }
            catch (Exception ex)
            {

            }

            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, json);
        }

        private void btnCheckUpdateJson_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(UPDATEFILEINFO))
            {
                string jsonTxt = File.ReadAllText(UPDATEFILEINFO);
                var json = JsonConvert.DeserializeObject<IEnumerable<FileInfoModel>>(jsonTxt);

                List<FileInfoModel> fileInfoList = JsonConvert.DeserializeObject<List<FileInfoModel>>(jsonTxt);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            currentProcPath = BASEPATH;
            ConnectDeleteServer(SERVER_URL, DELETEFILEINFO);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            AppStart();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region WebAPI
        // SelectAppVersionAsync
        private async Task<AppVersionObj> SelectAppVersion()
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");

            var res = await Api.UpdateInfo.SelectAppVersionAsync();
            return res;
        }

        // SeleteDeleteContents
        private async Task<List<UpdateProcessObj>> SelectDeleteContents(string version)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");

            var res = await Api.UpdateInfo.SelectDeleteContentsAsync(version);
            return res;
        }

        // SeleteUpdateContents
        private async Task<List<UpdateProcessObj>> SelectUpdateContents(int machine_id, string version)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");

            var res = await Api.UpdateInfo.SelectUpdateProcessAsync(new UpdateInfoRequestObj() { MachineID = machine_id, VersionInfo = version });
            return res;
        }

        // DeleteUpdateProcess
        private async Task<UpdateInfoResponseObj> DeleteUpdateProcess(int machine_id, string fileName)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var res = await Api.UpdateInfo.DeleteUpdateProcessAsync(new UpdateInfoRequestObj() { MachineID = machine_id, FileName = fileName.Replace(@"\\", @"\") });
            return res;
        }

        // UpdateUpdateYN
        private async Task UpdateUpdateYN(int machine_id, string updateYn)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            await Api.UpdateInfo.UpdateUpdateYNAsync(machine_id, updateYn);
        }

        // UpsertUpdateContents
        private async Task<UpdateInfoResponseObj> UpsertUpdateContents(string fileInfoList, string versionInfo)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var res = await Api.UpdateInfo.UpsertUpdateContentsAsync(new UpdateInfoRequestObj() { FileInfoList = fileInfoList, VersionInfo = versionInfo });
            return res;
        }

        // UpdateDelYN
        private async Task<UpdateInfoResponseObj> UpdateDelYN(string fileInfoList)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var res = await Api.UpdateInfo.UpdateDelYNAsync(new UpdateInfoRequestObj() { FileInfoList = fileInfoList });
            return res;
        }

        // UpdateAppVersion
        private async Task UpdateAppVersion(string version)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            await Api.UpdateInfo.UpdateAppVersionAsync(version);
        }
        #endregion
    }
}
