using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
// using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

using System.Globalization;

using Microsoft.Win32;
using System.Runtime.InteropServices;

using System.Management;
using HtmlAgilityPack;

namespace EasyRustServer
{
    class Program
    {
        public static bool umodDownloaded { get; set; }//currentversion downloaded
        public static bool umodInstalled { get; set; }
        public static bool SteamDownloaded { get; set; }
        public static bool SteamInstalled { get; set; }
        public static bool serverInstalled { get; set; }
        public static bool NoConnection { get; set; }
        public static bool ServerRunning { get; set; }
        public static bool SteamRunning { get; set; }
        enum serverStatus
        {
            FreshInstall,
            SteamMissing,
            ServerMising,
            UmodMissing,
            OutOFDate,
            Allgood,
            NoConnection
        }


        static serverStatus CurrentStatus;
        public static string NewVersion
        {
            get;
            set;
        }
        public static string MyVersion
        {
            get;
            set;
        }
        public static string extractPath
        {
            get;
            set;
        }
        public static string Umod
        {
            get;
            set;
        }
        public static string App_Path = AppDomain.CurrentDomain.BaseDirectory;
        public static string Steam
        {
            get;
            set;
        }
        public static string SteamCmd
        {
            get;
            set;
        }
        public static string ForceDir
        {
            get;
            set;
        }
        public static string ServerDir
        {
            get;
            set;
        }
        public static string UpdateUrl;
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        public static bool settingsReturn, refreshReturn;
        // public string path = Application.StartupPath;
        public static string home = "gameserver.vip";
        public static string game = "Rust";
        public static string SteamApp;
        public static string SteamAppNumber;
        public static string UmodPatchDir;
        public static string ServerRunFile;
        public static string ServerProccess;
        public static string RustVersion;
        public static long ServerProccessid;
        public static string ServerStartupConfig;
        public static string UmodDownload;
        public static string KorisnikPublic;
        public static bool LogSnimen;
        public static string MyIP = "";
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public static RegistryKey regCheck1;
        string tempPath = System.IO.Path.GetTempPath();
        static string Id = "";
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();





        static void Main(string[] args)
        {



            // UnloadProxy();
            MyIP = GetIP();
            // if (!Main.CheckForInternetConnection())
            //        locked();
            //      this.Close();
            // MessageBox.Show(Environment.UserName, "Locked!");
            string uniqueId = getUniqueID("C");
            string tableSource = GetTableSource(uniqueId);
            Id = uniqueId;
            KorisnikPublic = uniqueId;//check if registered

            if (tableSource == "true")
            {
                //  label1.Text = "Days Left " + this.GetUserDate(uniqueId);
                // this.label5.Text = "1";
                if (!GetProgramVersion().Contains("v0.5.1"))
                {
                    //  Console.WriteLine("You are using an old version!\nProgram will now update!", "Update"");
                    //  Console.ReadKey();
                    //obsolete code ..probaly shouldnt touch
                    //  int num = (int)MessageBox.Show("You are using an old version!\nProgram will now update!", "Update");
                    //   string str = Directory.GetCurrentDirectory() + "/updater.exe";
                    //   if (System.IO.File.Exists(str))
                    //      Process.Start(str);
                    //   Environment.Exit(0);
                    // this.richTextBox1.Text = this.GetProgramMessage();

                }

            }
            else
            {

               // if (tableSource == "404" )
                
                    Id = uniqueId;
                    locked();//log locked
                    ShowMyDialogBox();//actual lock screen
                    Environment.Exit(0);
                
            }











            #region first run


            string d1 = App_Path + @"umod\";
            string d2 = App_Path + @"Steam\";
            System.IO.Directory.CreateDirectory(d1);
            System.IO.Directory.CreateDirectory(d2);
            extractPath = App_Path + @"umod\";
            Umod = App_Path + @"umod\";
            Steam = App_Path + @"Steam\";
            SteamCmd = App_Path + @"Steam\";

            ServerDir = App_Path + @"server\";
            UpdateUrl = "http://" + home + "/UmodUpdateCheck.php";
            UmodDownload = "http://" + home + "/UmodDownload.php";

            //  SteamApp = @"steamcmd.exe +login anonymous +force_install_dir " + App_Path + @"server\" + " +app_update 258550 +quit";
            SteamAppNumber = "258550";
            UmodPatchDir = "RustDedicated_Data";
            ServerRunFile = "start.bat";//should be a bat file must recheck this later
            ServerProccess = "RustDedicated";
            ServerStartupConfig = "RustEditStartupHere.bat";

            if (File.Exists(ServerDir + @"oxide\oxide.config.json"))
            {
                Console.WriteLine("Starting First Instalation");
                //               CheckInstallSteam();

                //               RunUpdateSteam();
                
                CheckUmodVersion();//UnzipPlaceUmod()
                Console.WriteLine("fisrt run RunServer()");
                try
                {
                    Console.WriteLine("fisrt run RunServer().Copy() " + App_Path + @"config\" + ServerStartupConfig);
                    Console.WriteLine("fisrt run RunServer().To() " + ServerDir + @"\" + ServerRunFile);
                    File.Copy(App_Path + @"config\" + ServerStartupConfig, ServerDir + @"\" + ServerRunFile, true);
                }
                catch { }
                Console.WriteLine("Running Server!");
                foreach (var process in Process.GetProcessesByName(ServerProccess))
                {
                    process.Kill();
                }
                System.Threading.Thread.Sleep(5 * 1000);//5 second wait not workign somehow some times?
                RunServer();
                Console.WriteLine("going idle then recheck updates......");
                System.Threading.Thread.Sleep(10 * 60 * 1000);
               

            }
            
            
                foreva:
                if (!CheckSteam())
                {
                    try
                    {
                        KillDir(Steam);
                        Console.WriteLine("Files Missing Updating Steam");
                        using (var webClient = new System.Net.WebClient())
                        {
                            webClient.Headers.Add("User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            //
                            string dl = "http://" + home + "/SteamDownload.php?&key=" + Id;
                            string fl = Steam + "steamcmd.zip";
                            webClient.DownloadFile(dl, fl);
                            try
                            {
                                ZipFile.ExtractToDirectory(Steam + "steamcmd.zip", Steam);
                            }
                            catch
                            {
                                KillDir(Steam);

                                Console.WriteLine("No connections? no internet?");

                                goto foreva;


                            }
                        }
                    }
                    catch { }


                }//steam installed
                 //run steal instal and upadate
               
                umod:




                if (!CheckUmodVersion())
                {
                    if (DownloadUmod())
                    {
                        RunUpdateSteam(); //difrent update version hack for steam not closing right
                        UnzipPlaceUmod();

                    }
                    else
                    {
                        Console.WriteLine("No connections? no internet? retry in 20 seconds!");
                        System.Threading.Thread.Sleep(10 * 1000);//5 second wait not workign somehow some times?) 
                        goto umod;
                    }


                 }

                RunServer();

                //time to run the server first lets do some diagnotics
                #endregion


                //  Console.WriteLine("Start up Diganostics");
                //  b:
                //  Console.WriteLine("Checking...");
                //    Startdiagnose();
                //  System.Threading.Thread.Sleep(1 * 60 * 1000);
                //   if (CurrentStatus != serverStatus.Allgood) { goto b; }
                
                Console.WriteLine("Rechecking updates");
                //diagnose();
                Console.WriteLine("going idle then recheck updates......");
                System.Threading.Thread.Sleep(15 * 60 * 1000);

                goto umod;
                //CheckSteam();
             




           
        }

        public static void UpdateRust()
        {
            RustVersion = ChecRustVersion();
             
            // checked if file exists
            if (File.Exists(App_Path + @"version\" + RustVersion))
            {
                Console.WriteLine("Up To Date " + App_Path + @"version\" + RustVersion);
                
                    //dont do nuthin
               // umodDownloaded = true; //current version uoto date
               // return true;

            }
            else
            {

                CurrentStatus = serverStatus.OutOFDate;
                Console.WriteLine("serverStatus.OutOFDate");
                if (!CheckUmodVersion())
                {
                    //do update
                    //write to file rust is up to date
                    
                    if (!File.Exists(App_Path + @"version\" + RustVersion))
                    {
                        using (var tw = new StreamWriter(App_Path + @"version\" + RustVersion))
                        {
                            tw.WriteLine(RustVersion);
                        }
                    }
                    //run steam app update rust


                }
                else
                {
                    //loop until umod update is ready
                }


            } ///al
            //if not
            //if so
        }
        public static bool UmodUptoDate()
        {
            return CheckUmodVersion();
        }
        public static bool RustUptoDate()
        {
            if (File.Exists(App_Path + @"version\" + ChecRustVersion()))
            {
                Console.WriteLine("Up To Date " + App_Path + @"version\" + RustVersion);

                //dont do nuthin
                // umodDownloaded = true; //current version uoto date
                // return true;
                return true ;
            }
            return false;

        }
        public static string ChecRustVersion()
        {
            string str1 = new WebClient().DownloadString("http://gameserver.vip/steamrust.php");
            //  xPath = "/html/body/div[1]/div/div[4]/table/tbody/tr[1]/td[1]/a";
            string Url = "http://gameserver.vip/steamrust.php";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("/html/body/div[1]/div/div[3]/table"))
            {
              //  Console.WriteLine("Found: " + table.Id);
                foreach (HtmlNode row in table.SelectNodes("//tr"))
                {
                    //Console.WriteLine("row");
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        if (IsInteger(cell.InnerText))
                        {
                            //Console.WriteLine();
                            return cell.InnerText;
                        }
                    }
                }
            }
            return "null";
        }
        static void KillDir(string idir) {

                System.IO.DirectoryInfo di = new DirectoryInfo(idir);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }

        }

        public static bool IsInteger(string s)
        {
            if (String.IsNullOrEmpty(s))
                return false;

            int i;
            return Int32.TryParse(s, out i);
        }


        static void RunServer()
            {
                Console.WriteLine("RunServer()");
              //  Process[] pname = Process.GetProcessesByName(ServerProccess);
               // if (pname.Length == 0)
              //      ServerRunning = false;
              //  else
               //     ServerRunning = true;

               // if (CurrentStatus == serverStatus.Allgood && ServerRunning) { return; }

                try
                {


                    Console.WriteLine("config check " + App_Path + @"config\" + ServerStartupConfig);
                    Console.WriteLine("set config " + ServerDir + ServerRunFile);
                    File.Copy(App_Path + @"config\" + ServerStartupConfig, ServerDir + ServerRunFile, true);
                }
                catch { }
                Console.WriteLine("Running Rust Server!");
            //               foreach (var process in Process.GetProcessesByName(ServerProccess))
            //               {
            //                   process.Kill();
            //               }
            //                System.Threading.Thread.Sleep(5 * 1000);//5 second wait not workign somehow some times?
            //if (!ServerRunning) { ExecuteCommand(ServerDir + "start.bat"); }
            //  Console.WriteLine("ExecuteCommand(" + ServerDir + ServerRunFile + ")";
            Process[] pname = Process.GetProcessesByName(ServerProccess);
            if (pname.Length == 0)
            {
                Console.WriteLine("No Server Procces Found");
                ExecuteCommand(ServerDir + ServerRunFile);
            }
            else
            {
                Console.WriteLine("Server Procces Found");
            }

        }
            static void ExecuteCommand(string command)
            {
                Console.WriteLine("ExecuteCommand(" + command + ")");





                //int exitCode;
                ProcessStartInfo processInfo;
                Process process;

                processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                //  processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = true;
                // *** Redirect the output ***
                //  processInfo.RedirectStandardError = true;
                //  processInfo.RedirectStandardOutput = true;
                processInfo.WorkingDirectory = ServerDir;
                process = Process.Start(processInfo);
                //process.WaitForExit();

                // *** Read the streams ***
                // Warning: This approach can lead to deadlocks, see Edit #2
                // string output = process.StandardOutput.ReadToEnd();
                // string error = process.StandardError.ReadToEnd();

                // exitCode = process.ExitCode;

                // Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
                //Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
                //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
                // process.Close();
            }

            static bool CheckUmodVersion()
            {
                Console.WriteLine("CheckUmodVersion()");
                try
                {
                    using (var webClient = new System.Net.WebClient())
                    {
                        webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                      "Windows NT 5.2; .NET CLR 1.0.3705;)");
                        // webClient.Headers.Add("User-Agent: Other");
                        //var json = webClient.DownloadString("http://" + home + "/UmodUpdateCheck.php?&key=" + Id);
                        //https://umod.org/games/rust/versions.json

                      //  Console.WriteLine(UpdateUrl + "?&key=" + Id);
                        var json = webClient.DownloadString(UpdateUrl + "?&key=" + Id);

                        //Console.WriteLine(value);
                        string v = JObject.Parse(json)["version"].ToString();
                        Console.WriteLine("Umod Version" + v);
                        NewVersion = v;
                        //  MyVersion = v;
                        // label1.Text = v;

                        if (File.Exists(App_Path + @"umod\" + NewVersion + ".zip"))
                        {
                            Console.WriteLine("Up To Date " + App_Path + @"umod\" + NewVersion + ".zip");
                            MyVersion = NewVersion;
                            //textBox1.AppendText(Environment.NewLine);
                            // textBox1.AppendText("All Up To Date");
                            //  Console.WriteLine("Umod All Up To Date");
                            umodDownloaded = true; //current version uoto date
                            return true;

                        }
                        else
                        {
                            CurrentStatus = serverStatus.OutOFDate;
                            Console.WriteLine("serverStatus.OutOFDate");

                           // CheckInstallSteam();

                           // Console.WriteLine("DownloadUmod()");
                           // DownloadUmod();
                           // Console.WriteLine("UnzipPlaceUmod();");
                           // UnzipPlaceUmod();
                            return false;

                        } ///all upto date good to go

                    }

                }

                catch (ArgumentNullException ex)
                {
                    //error no data downloaded 
                    NoConnection = true;
                    return false;
                    //code specifically for a ArgumentNullException
                }
                catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    NoConnection = true;
                    return false;
                    //error protocal error ex.Status.ToString();
                    //code specifically for a WebException ProtocolError
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    NoConnection = true;
                    //file not found fail
                    return false;
                    //code specifically for a WebException NotFound
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.InternalServerError)
                {
                    NoConnection = true;
                    return false;

                    //code specifically for a WebException InternalServerError
                }
                finally
                {


                }


            }
            static bool DownloadUmod()
            {
                Console.WriteLine("DownloadUmod()");
                try
                {
                    using (var webClient = new System.Net.WebClient())
                    {

                        webClient.Headers.Add("User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

             //           Console.WriteLine(UmodDownload + "?&key=" + Id);
                        Console.WriteLine(App_Path + @"umod\" + NewVersion + ".zip");
                        webClient.DownloadFile(UmodDownload + "?&key=" + Id, App_Path + @"umod\" + NewVersion + ".zip");

                        //System.IO.File.WriteAllText (@"D:\path.txt", contents);

                        //unzip the file


                    }



                    // textBox2.Text = json.
                }

                catch (ArgumentNullException ex)
                {
                    // textBox1.AppendText(Environment.NewLine);
                    // textBox1.AppendText("error umod download " + ex.ToString());
                    Console.WriteLine("error umod download ");
                    //error no data downloaded 
                    return false;
                    //code specifically for a ArgumentNullException

                }
                catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("error umod download ");
                    return false;
                    //error protocal error ex.Status.ToString();
                    //code specifically for a WebException ProtocolError
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("error umod download ");
                    //file not found fail
                    return false;
                    //code specifically for a WebException NotFound
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.InternalServerError)
                {
                    Console.WriteLine("error umod download ");
                    return false;

                    //code specifically for a WebException InternalServerError
                }
                finally
                {

                    // Console.WriteLine("umod download and extracted ");

                }




                return true;
            }
            static void UnzipPlaceUmod()
            {
                Console.WriteLine("UnzipPlaceUmod()");
                Console.WriteLine("rem dir " + App_Path + @"umod\" + UmodPatchDir + @"\");
                System.IO.DirectoryInfo di = new DirectoryInfo(App_Path + @"umod\" + UmodPatchDir + @"\");
                try
                {

                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch
                {
                    //dont stop keep going
                    //Catch the exception  
                }
                finally
                {
                    //nada
                }

            try
            {

                foreach (var process in Process.GetProcessesByName(ServerProccess))
                {
                    process.Kill();
                }


                //checked for steam running if so kill
            //    foreach (var process in Process.GetProcessesByName(ServerProccess))
            //    {
            //        process.Kill();
            //    }
            }
            catch { }
                Console.WriteLine("Path" + App_Path + @"umod\" + NewVersion + ".zip");
                Console.WriteLine("Unzip to " + App_Path + @"umod\");
                ZipFile.ExtractToDirectory(App_Path + @"umod\" + NewVersion + ".zip", App_Path + @"umod\");
                // File.Copy(App_Path + @"/umod/RustDedicated_Data/", App_Path + @"/server/RustDedicated_Data/", true);
                System.Threading.Thread.Sleep(10000);//10 seconds
                                                     //Now Create all of the 
                Console.WriteLine("Copy from " + App_Path + @"umod\" + UmodPatchDir + @"\");
                Console.WriteLine("Copy to" + ServerDir + UmodPatchDir + @"\");
                string SourcePath = App_Path + @"umod\" + UmodPatchDir + @"\";
                string DestinationPath = ServerDir + UmodPatchDir + @"\";
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                System.Threading.Thread.Sleep(10000);//10 seconds


                try
                {


                }
                catch { }



                //System.IO.Compression.GZipStream.ExtractToDirectory(App_Path + @"/umod/" + NewVersion + ".zip", extractPath);
                Console.WriteLine("umod download ready to run the server");
            }
            static void OpenBrowser(string url)
            {
                //https://stackoverflow.com/questions/14982746/open-a-browser-with-a-specific-url-by-console-application
                try
                {
                    Process.Start(url);
                }
                catch
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        url = url.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start("xdg-open", url);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Process.Start("open", url);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        
        static bool CheckSteam()
        {
            Console.WriteLine("CheckSteam()");
            if (File.Exists(Steam + "steamcmd.zip") && File.Exists(Steam + "steamcmd.exe"))
            {
                Console.WriteLine("Steam file exists " + Steam + "steamcmd.zip");
                Console.WriteLine("Steam file exists " + Steam + "steamcmd.exe");
                Console.WriteLine("Steam Verified Present");
                return true;
            }
            else
            {
                return false;
            }




 }
        static void RunUpdateSteam()
        {

            Console.WriteLine("RunUpdateSteam()");

            // ProcessStartInfo start_info = new ProcessStartInfo(SteamCmd + "steam.bat", "");
            //new ProcessStartInfo(SteamCmd + "steamcmd.exe", @" +login anonymous +force_install_dir " + (char)34 + App_Path + @"server\" + (char)34 + " +app_update 258550 +quit");
            // start_info.WindowStyle = ProcessWindowStyle.Maximized;
            //start_info.UseShellExecute = false;
            // Open wordpad.
            // Process proc = new Process();
            //proc.StartInfo = start_info;
            //proc.Start();

            // if server running 
            Process[] pname = Process.GetProcessesByName(ServerProccess);
            if (pname.Length == 0)
            {
                Console.WriteLine("No Server Procces Found");
                string sexe = SteamCmd + "steamcmd.exe";
                Console.WriteLine(sexe);
                string scmd = @" +login anonymous +force_install_dir " + (char)34 + ServerDir + (char)34 + " +app_update " + SteamAppNumber + " +quit";
                Console.WriteLine(scmd);
                Process.Start(sexe, scmd).WaitForExit();
            }
            else
            {
                Console.WriteLine("Server Procces Found");
                foreach (var process in Process.GetProcessesByName(ServerProccess))
                {
                    process.Kill();
                }
                string sexe = SteamCmd + "steamcmd.exe";
                Console.WriteLine(sexe);
                string scmd = @" +login anonymous +force_install_dir " + (char)34 + ServerDir + (char)34 + " +app_update " + SteamAppNumber + " +quit";
                Console.WriteLine(scmd);
                Process.Start(sexe, scmd).WaitForExit();
               



 
            }

            
            // steamcmd.exe +login USER PASS +force_install_dir "C:\Some\Path\Where\You\Want\Game\Server\Files\To\Go" +app_update 344760 validate +quit

            // Process p = new Process();
            //  p.StartInfo.Arguments = @" +login anonymous +force_install_dir " + (char)34 + App_Path + @"server\" + (char)34 + " +app_update 258550 +quit";
            //p.StartInfo.FileName = SteamCmd + "steam.bat";
            //  p.Start();
            // p.WaitForExit();
            Console.WriteLine("Steam run and installed");
            //textBox1.AppendText(Environment.NewLine);
            // textBox1.AppendText("Steam run and installed");
            // Process p = new Process();
            //    p.StartInfo.Arguments = @" +login anonymous +force_install_dir " + (char)34 + App_Path + @"server\" + (char)34 + " +app_update 258550 +quit";
            //  p.StartInfo.FileName = SteamCmd + "steamcmd.exe";


        }
        /*
        static void UpdateSteam()
        {

            Console.WriteLine("UpdateSteam()");



            string sexe = SteamCmd + "CMD";
            Console.WriteLine(sexe);
            string scmd = @"start /b steamcmd.exe +login anonymous +force_install_dir " + (char)34 + ServerDir + (char)34 + " +app_update " + SteamAppNumber + " +quit";
            Console.WriteLine(scmd);
            Process.Start(sexe, scmd);

            Console.WriteLine("Steam Update run and installed");
            Console.WriteLine("going idle then checking progress......");
            System.Threading.Thread.Sleep(10 * 60 * 1000);
            foreach (var process in Process.GetProcessesByName("steamcmd"))
            {
                process.Kill();
            }


            //checked for steam running if so kill
            foreach (var process in Process.GetProcessesByName("steamcmd"))
            {
                process.Kill();
            }

        }
        */
        string ID()
        { return Id; }

        static void ShowMyDialogBox()
        {

            OpenBrowser("http://gameserver.vip/index.php?qa=easy-rust-server-subscribe");
            Console.WriteLine("Please subscribe! Not Registered!");
            Console.ReadKey();
            // LockedForm testDialog = new LockedForm();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            //   if (testDialog.ShowDialog(this) == DialogResult.OK)
            //   {
            // Read the contents of testDialog's TextBox.

            //   }
            //   else
            //   {

            //   }
            //   testDialog.Dispose();
        }
        static void Notlocked()
        {
            string localIP = string.Empty;
            string a1 = string.Empty;


            //   a1 = "http://" + home  + "/qzpl/savelog.php?user=" + this.KorisnikPublic + "&baza=login-" + System.Environment.MachineName + "&fb=1" + "&cname=";
            using (WebClient webClient = new WebClient())
                webClient.DownloadString("http://" + home + "/savelog.php?user=" + KorisnikPublic + "&baza=App-Startup-" + MyIP + "-" + Environment.UserName + "-" + System.Environment.MachineName + "&fb=1");
        }

        static void locked()
        {
            IPHostEntry host;

            host = Dns.GetHostEntry(Dns.GetHostName());

            using (WebClient webClient = new WebClient())
                webClient.DownloadString("http://" + home + "/savelog.php?user=" + KorisnikPublic + "&baza=locked-" + "-" + MyIP + "-" + Environment.UserName + "-" + System.Environment.MachineName + "&fb=1");
        }
        static string getCPUID()
        {
            string str = string.Empty;
            foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor").Get())
            {
                if (str == string.Empty)
                {
                    try
                    {
                        str = (string)managementObject["ProcessorId"];
                    }
                    catch (NullReferenceException ex)
                    {
                        Console.Write(ex);
                        return str;
                    }
                }
            }
            return str;
        }

        static string getSpace()
        {
            string str = string.Empty;
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.Name.Contains("C:") && driveInfo.IsReady)
                {
                    if (str == string.Empty)
                    {
                        try
                        {
                            str = Math.Round((double)driveInfo.TotalSize / 1024.0 / 1024.0 / 1024.0).ToString();
                        }
                        catch (NullReferenceException ex)
                        {
                            Console.Write(ex);
                            return str;
                        }
                    }
                }
            }
            return str;
        }

        static string getVolumeSerial(string drive)
        {
            ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
            managementObject.Get();
            string str = managementObject["VolumeSerialNumber"].ToString();
            managementObject.Dispose();
            return str;
        }

        bool CheckForInternetConnection()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (webClient.OpenRead("http://www.google.com"))
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }
        static string GetUserDate(string userid)
        {
            return new WebClient().DownloadString("http://" + home + "/date.php?userid=" + userid);
        }

        static string GetIP()
        {
            IP:
            try
            {
                return new WebClient().DownloadString("http://" + home + "/IP.php");
            }
            catch
            {

                Console.WriteLine("No connections? no internet?");
                System.Threading.Thread.Sleep(1 * 60 * 1000);
                goto IP;
            }
        }
        static int SnimiLog2(string link)
        {
            if (!LogSnimen)
            {

                LogSnimen = true;
                string str1 = link;
                int startIndex = str1.IndexOf("&u=") + "&u=".Length;
                int num = str1.LastIndexOf("&k=");
                string str2 = str1.Substring(startIndex, num - startIndex);


                using (WebClient webClient = new WebClient())
                    webClient.DownloadString("http://" + home + "/savelog.php?user=" + KorisnikPublic + "&baza=" + str2 + "-" + MyIP + "-" + Environment.UserName + "-" + System.Environment.MachineName + "&fb=1");

            }
            return 1;
        }
        static string GetProgramVersion()
        {
            return new WebClient().DownloadString("http://" + home + "/version.php");
        }
        static string getUniqueID(string drive)
        {
            if (drive == string.Empty)
            {
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    if (driveInfo.IsReady)
                    {
                        drive = driveInfo.RootDirectory.ToString();
                        break;
                    }
                }
            }
            if (drive.EndsWith(":\\"))
                drive = drive.Substring(0, drive.Length - 2);
            string volumeSerial = getVolumeSerial(drive);
            //https://stackoverflow.com/questions/2004666/get-unique-machine-id

            //string deviceId = new DeviceIdBuilder()
            //.AddMachineName()
            //.AddMacAddress()
            //.AddProcessorId()
            //.AddMotherboardSerialNumber()
            //.ToString();
            string cpuid = getCPUID();
            string space = getSpace();
            return "7" + cpuid + volumeSerial + space + "E";
        }
        static string GetTableSource(string key)
        {
            string str1 = new WebClient().DownloadString("http://" + home + "/reg.php?&key=" + key);
            // string str2 = str1.Remove(0, str1.IndexOf("<table"));
            // return str2.Remove(str2.IndexOf("</table>") + 7);
            return str1;
        }









    }

}
