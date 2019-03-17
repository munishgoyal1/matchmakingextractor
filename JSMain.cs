#define LOGINRUN
#define F_PROFILES
using System;
using System.Collections.Generic;
using System.Text;
using HtmlParsingLibrary;
using System.IO;
using System.ComponentModel;


//using System.Xml;
using System.Threading;
//using System.Data.SqlClient;
using System.Security.Cryptography;
using HttpLibrary;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;


namespace Jeevansathi
{
    partial class Program
    {
        public static bool bTouchProfiles = true;

        public string mChecksum = "checksum=d0c089efa50843ec7c47e9ca52f86d09|i|aee434154bbe5fa0a2b8ae997f648490i4142500&profilechecksum=aee434154bbe5fa0a2b8ae997f648490i4142500";

        public static string mFailedProfilesFile = @"I:\JS_new\failedProfiles.txt";

        public static object LoggingLock = new object();
 
        public static void LogFailedProfileDownload(string profileId)
        {
            if (bTouchProfiles)
                return;

            lock (LoggingLock)
            {

                using (FileStream fs = new FileStream(mFailedProfilesFile, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(profileId);
                    }
                }

            }
        }

        public static DirectoryInfo CreateDir(string path)
        {
            DirectoryInfo di = null;
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("The path {0} exists already.", path);
                    return di;
                }

                // Try to create the directory.
                di = Directory.CreateDirectory(path);
                //int workerThreads = 0;
                //int ioThreads = 0;
                //ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
                //Console.WriteLine("NumThreads: {0},{1} The directory {2} was created successfully at {3}.", workerThreads, ioThreads,path, Directory.GetCreationTime(path));
                Console.WriteLine("The directory {0} was created successfully at {1}.", path, Directory.GetCreationTime(path));

            }
            catch (Exception e)
            {
                Console.WriteLine("CreateDir failed: {0}", e.ToString());
                LogFailedProfileDownload(path.Substring(path.LastIndexOf("\\")));
            }
            finally { }

            return di;
        }


        public static void CreateAndWriteFile(string path, string data)
        {

            try
            {
                // Determine whether the file exists.
                if (File.Exists(path))
                {
                    Console.WriteLine("The file {0} exists already.", path);
                }
                else
                {
                    // Try to create the file.
                    FileStream fs = File.Create(path);
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(data);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateFile or Write to file {0} failed: {1}", path, e.ToString());
            }
            finally { }
        }

        //public static void DownloadFileCallback2(
        //    object sender,
        //    AsyncCompletedEventArgs e)
        //{
        //    Guid taskId = (Guid)e.UserState;
        //    //FileInfo fi = new FileInfo(photoPath);
        //    //if (fi.Length < 5000)
        //    //{
        //    //    fi.Delete();
        //    //}
        //}

        public static void DownloadPhotos(CookieContainer mCookieContainer, string profilePath, string profilePhotoId)
        {
            string profilePhotoURLStart = "http://ser4.jeevansathi.com/profile/photo_serve.php?profileid=";
            string profilePhotoURLEnd = "&photo=";
            string mainPhoto = "MAINPHOTO";
            string photo1 = "ALBUMPHOTO1";
            string photo2 = "ALBUMPHOTO2";

            string photoURL = null;
            //string photoData = null;
            string photoFileName = null;
            string photoPath = null;

            WebClient webClient = null;
            Uri uri = null;
            FileInfo fi = null;
            try
            {
                // Main photo


                photoURL = profilePhotoURLStart + profilePhotoId + profilePhotoURLEnd + mainPhoto;

                photoFileName = mainPhoto + ".jpg";
                photoPath = profilePath + "\\" + photoFileName;

                fi = new FileInfo(photoPath);
                if (!fi.Exists || fi.Length == 0)
                {
                    webClient = new WebClient();
                    uri = new Uri(photoURL);
                    // Specify that the DownloadFileCallback method gets called
                    // when the download completes.
                    ///webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback2);

                    webClient.DownloadFileAsync(uri, photoPath);
                }
                // Album photo1

                photoURL = profilePhotoURLStart + profilePhotoId + profilePhotoURLEnd + photo1;
                photoFileName = photo1 + ".jpg";
                photoPath = profilePath + "\\" + photoFileName;
                fi = new FileInfo(photoPath);
                if (!fi.Exists || fi.Length == 0)
                {
                    webClient = new WebClient();
                    uri = new Uri(photoURL);
                    // Specify that the DownloadFileCallback method gets called
                    // when the download completes.
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback2);
                    webClient.DownloadFileAsync(uri, photoPath);
                }
                //fi = new FileInfo(photoPath);
                //if (fi.Length < 5000)
                //{
                //    fi.Delete();
                //}

                // Album photo2

                photoURL = profilePhotoURLStart + profilePhotoId + profilePhotoURLEnd + photo2;

                photoFileName = photo2 + ".jpg";
                photoPath = profilePath + "\\" + photoFileName;
                fi = new FileInfo(photoPath);
                if (!fi.Exists || fi.Length == 0)
                {
                    webClient = new WebClient();
                    uri = new Uri(photoURL);
                    // Specify that the DownloadFileCallback method gets called
                    // when the download completes.
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback2);
                    webClient.DownloadFileAsync(uri, photoPath);
                }
                    //fi = new FileInfo(photoPath);
                //if (fi.Length < 5000)
                //{
                //    fi.Delete();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("DownloadPhotos {0} failed: {1}", profilePath, e.ToString());
                LogFailedProfileDownload(profilePath.Substring(profilePath.LastIndexOf("\\")));
            }
            finally { }
        }

        public static void LogIn(int start, int end, string username, bool bKeepDoing = true)
        {
            CookieContainer mCookieContainer = new CookieContainer();
#if LOGINRUN
#if F_PROFILES
            if(string.IsNullOrEmpty(username))
            username = "munishgoyal1@gmail.com";
            string password = "amit1978";

#else
            string username = "sonalsinha28@yahoo.in";
            string password = "O1lyhead";
#endif
            string pageData = "hii";
            //pageData = HttpHelper.GetWebPageResponse("http://www.jeevansathi.com/profile/login.php",
            //    "submit=Go&from_homepage=1&login_from=H&" + "username=" + username + "&password=" + password,
            //    "http://www.jeevansathi.com/profile/login.php",
            //    mCookieContainer);
            pageData = HttpHelper.GetWebPageResponse("http://www.jeevansathi.com/profile/login.php",
                 "submit=Login" + "&username=" + username + "&password=" + password,
                 "http://www.jeevansathi.com/profile/login.php",
                 mCookieContainer);

            // parse page data to redirect to the login page
            //string homePage = "http://www.jeevansathi.com/";
            //string loginRedirectUrl = "profile/login_redirect.php?get1=a%3A0%3A%7B%7D&post1=a%3A3%3A%7Bs%3A6%3A%22submit%22%3Bs%3A2%3A%22Go%22%3Bs%3A13%3A%22from_homepage%22%3Bs%3A1%3A%221%22%3Bs%3A10%3A%22login_from%22%3Bs%3A1%3A%22H%22%3B%7D";

            //pageData = HttpHelper.GetWebPageResponse(homePage + loginRedirectUrl,
            //    null,
            //    null,
            //    mCookieContainer);


           if (String.IsNullOrEmpty(pageData))
            {
                Console.WriteLine("** No response: server busy, couldnt login");
                return;
            }
            if (pageData.IndexOf("loadpopunder") >= 0)
            {
                Console.WriteLine("Successful Login");
            }      
#else
#endif
            string searchPage = null;

#if M_PROFILES1_REDUNDANT_DELETEIT
            string queryString = "?&Gender=M&Religion=&religionVal=&caste=&casteVal=&STYPE=Q&searchonline=&E_CLASS=&checksum=&TOP_BAND_SEARCH=Y&lage=21&hage=70&community=&mtongueVal=&hp_mstatus=&Search=Search";
            searchPage = HttpHelper.GetWebPageResponse("http://www.jeevansathi.com/profile/search.php" + queryString,
                null,
                null,
                mCookieContainer);
            if (String.IsNullOrEmpty(searchPage))
            {
                Console.WriteLine("Null Initial Search page data!..");
                return;
            }
            if(searchPage.Contains("Scheduled Maintenance"))
            {
                Console.WriteLine("Site is down");
                return;
            }
#endif
            // Specify the directory you want to manipulate.
            string jsProfilesDir = @"D:\JS_new";

            int searchPageNum = 0;
            // "<div class="rf"><a href="http://www.jeevansathi.com/profile/matrimonial-464233R4.htm" class="blink" "
            string profilePartURL = "http://www.jeevansathi.com/profile/matrimonial-";
            //string profilePartURLStart = "<div class=\"rf\"><a href=\"http://www.jeevansathi.com/profile/matrimonial-";
            //string profilePartURLEnd = ".htm\" class=\"blink\"";
            string profilePartURLStart = "position:absolute;margin-top:-25px;margin-left:300px\"><a href=\"http://www.jeevansathi.com/profile/matrimonial-";
            string profilePartURLEnd =  ".htm\" class=\"blink\"";//".htm\" style=\"color:#E40410\"";

#if M_PROFILES
            string searchId = "64770197";
#else
            string searchId = "63825787";
#endif

            string strBeforeProfilePhotoId = "photo_serve.php?profileid=";
            string strAfterProfilePhotoId = "&photo=PROFILEPHOTO";
    
            string searchPageURLStart = "http://www.jeevansathi.com/profile/search.php?searchid=";
            string searchID = "";
            string searchPageURLMid1 = "&j=";
            string searchPageURLMid2 = "&contact=&SIM_USERNAME=&NAVIGATOR=";
            string navigator = "";
            string searchPageURLEnd = "&nextViewSim=1&overwrite=1&google_kwd=";
            
            //string searchPageURLStart = "http://www.jeevansathi.com/profile/search.php?searchid=63825787&j=";
            //string searchPageURLEnd = "&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63825787%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd=";
                 
            //string searchPageURLStart = "http://www.jeevansathi.com/profile/search.php?searchid=" + searchId + "&j=";
            //string searchPageURLEnd = "&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__" + searchId + "%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd=";
      // jsb9Track:1272563597548|http://www.jeevansathi.com/profile/search.php?searchid=153121496&j=6&contact=&SIM_USERNAME=&NAVIGATOR=5a4d1c9e7ba33b9c72f6d7995362b88f&nextViewSim=1&overwrite=1&google_kwd=

            //string completeSampleSearchUrl = "http://www.jeevansathi.com/profile/search.php?searchid=153121496&j=1&contact=&SIM_USERNAME=&NAVIGATOR=5a4d1c9e7ba33b9c72f6d7995362b88f&nextViewSim=1&overwrite=1&google_kwd=";
            
  
            int successivefailCount = 0;
            bool bRelogin = false;
            double sleepMinutes = 0;
           // CookieContainer mCookieContainer1 = new CookieContainer();
            //string cc = mCookieContainer.Add(.ToString();

            //Cookie c = new Cookie("jsb9Track", "1272563597548|" + completeSampleSearchUrl);
            //CookieCollection cc = new CookieCollection();
            //cc.Add(c);
            //mCookieContainer.Add(cc);

            // Setting up page search intially one-time

            string firstSearchPostdata = "Gender=F&lage=22&hage=28&Religion=Select+a+Religion&hp_mstatus=N&community=&city_country_resArr%5B%5D=&caste=&Search=&Search=&casteVal=&religionVal=&checksum=d0c089efa50843ec7c47e9ca52f86d09%7Ci%7Caee434154bbe5fa0a2b8ae997f648490i4142500&CLICKTIME=1272550914496&STYPE=Q&E_CLASS=&TOP_BAND_SEARCH=Y&mtongueVal=&doesnt_mater_arr_0=&doesnt_mater_arr_1=&doesnt_mater_arr_3=&selectedRel=1%27%2C%279&list_all_caste_labels=%2C20%2C25%2C75%2C76%2C82%2C98%2C103%2C109%2C112%2C116%2C134%2C141%2C";
            string referer = "http://www.jeevansathi.com/profile/mainmenu.php?checksum=d0c089efa50843ec7c47e9ca52f86d09|i|aee434154bbe5fa0a2b8ae997f648490i4142500";
            string searchPageUrl = "http://www.jeevansathi.com/profile/search.php";
            searchPage = HttpHelper.GetWebPageResponse(searchPageUrl,
                 firstSearchPostdata,
                 referer,
                 mCookieContainer);

            string searchUrlRef = searchPageUrl;
 

            // Get the search ID and Navigator ID : one-time

            int tmpIndex = 0;
            searchID = StringParser.GetStringBetween(searchPage, 0, "var searchidL=\'", "\';", null, out tmpIndex);

            do
            {
                for (searchPageNum = start; searchPageNum <= end; searchPageNum++)
                {
                    int profileNum = 0;
                    int index = 0;
                    int idFoundIndex = 0;
                    string profileId = null;

                    navigator = StringParser.GetStringBetween(searchPage, 0, "NAVIGATOR=", "&searchid", null, out tmpIndex);

                    //string searchUrl = searchPageURLStart + searchPageNum.ToString() + searchPageURLEnd;
                    string searchUrl = searchPageURLStart + searchID + searchPageURLMid1 + searchPageNum.ToString() + searchPageURLMid2 + navigator + searchPageURLEnd;
                    //string searchUrl = "http://www.jeevansathi.com/profile/search.php?searchid=153164321&j=2&contact=&SIM_USERNAME=&NAVIGATOR=35912df3b3fca1e2b92f6dff85b39300&nextViewSim=1&overwrite=1&google_kwd=";

                    if (bRelogin)
                    {
                        Console.WriteLine("After 5 minutes of failed-tries: Doing re-login");
                        // try to re-login and pick up from same search page
                        pageData = HttpHelper.GetWebPageResponse("http://www.jeevansathi.com/profile/login.php",
                             "submit=Login" + "&username=" + username + "&password=" + password,
                             "http://www.jeevansathi.com/profile/login.php",
                             mCookieContainer);
                        if (String.IsNullOrEmpty(pageData))
                        {
                            Console.WriteLine("** No response: server busy, couldnt login: Sleeping for 2 minutes");
                            Thread.Sleep(2 * 60 * 1000);
                            sleepMinutes += 2;
                            continue;
                        }
                        else
                        {
                            searchPage = HttpHelper.GetWebPageResponse(searchPageUrl,
                                  firstSearchPostdata,
                                  referer,
                                  mCookieContainer);
                            searchID = StringParser.GetStringBetween(searchPage, 0, "var searchidL=\'", "\';", null, out tmpIndex);
                        }
                    }

                    // Get next page
                    searchPage = HttpHelper.GetWebPageResponse(searchUrl,
                        "Gender=F",
                        null,
                        mCookieContainer);

                    searchUrlRef = searchUrl;

                    if (String.IsNullOrEmpty(searchPage))
                    {
                        Console.WriteLine("Null Initial Search page data. Skipped page {0}", searchPageNum);
                        LogFailedProfileDownload("PageNum=" + searchPageNum.ToString());
                        successivefailCount++;

                        if (successivefailCount > 10)
                        {
                            Console.WriteLine("No Response: Failed 10 times in succession..sleeping for 2 minutes");
                            sleepMinutes += 2;
                            bRelogin = true;
                            Thread.Sleep(2 * 60 * 1000); // Sleep for 10 minutes
                        }
                        else
                        {
                            continue;
                        }
                        searchPageNum--;
                        continue;
                    }
                    if (searchPage.Contains("Scehduled Maintenance") || searchPage.Contains("technical reasons"))
                    {
                        Console.WriteLine("Site is down, sleep for 10 minutes");
                        Thread.Sleep(10 * 60 * 1000); // Sleep for 10 minutes
                        sleepMinutes += 10;
                        bRelogin = true;
                        searchPageNum--;
                        continue;
                    }
                    if (searchPage.Contains("We have encountered a temporary error"))
                    {
                        Console.WriteLine("Temporary Error, retry after 30 seconds");
                        Thread.Sleep(30000);
                        sleepMinutes += 0.5;

                        successivefailCount++;
                        if (successivefailCount > 10)
                        {
                            Console.WriteLine("TempError: Failed 10 times in succession..sleeping for 2 minutes");
                            Thread.Sleep(2 * 60 * 1000); // Sleep for 2 minutes
                            sleepMinutes += 2;

                            bRelogin = true;
                        }
                        else
                        {
                            continue;
                        }
                        searchPageNum--;
                        continue;
                    }
                    sleepMinutes = 0;
                    successivefailCount = 0;
                    bRelogin = false;
                    //Debugger.Break();
                    for (profileNum = 0; profileNum < 10; profileNum++)
                    {
                        profileId = StringParser.GetStringBetween(searchPage, index, profilePartURLStart, profilePartURLEnd, null, out idFoundIndex);
                        string profilePath = jsProfilesDir + "\\" + profileId;

                        // If by any chance we hit the end of page without success, break this loop
                        if (idFoundIndex < 0)
                            break;

                        index = idFoundIndex;

                        string strProfileNumber = (((searchPageNum - 1) * 10) + profileNum).ToString();
                        Console.Write("Profile {0}: ", strProfileNumber);

                        // If profile folder exists , we assume it has already been downloaded hence
                        // not trying to download again, saving a web call
                        if (Directory.Exists(profilePath) && !bTouchProfiles)
                        {
                            Console.WriteLine("The path {0} exists already.", profilePath);
                            continue;
                        }

                        string profileURL = profilePartURL + profileId + ".htm";


                        // "http://www.jeevansathi.com/profile/viewprofile.php?profilechecksum=435a4098ce676d9b5862566c44d2154di5600274&cid=&inf_checksum=&stype=Q&searchid=153170159&j=100&total_rec=73916&Sort=S&offset=1&NAVIGATOR=cfeac3cb484f52c46a33803169289379";

                        string profileData = null;
                        if (bTouchProfiles)
                        {
                            IAsyncResult async = null;
                            //profileData = HttpHelper.GetWebPageResponseAsync(profileURL,
                            //     "Gender=F",
                            //     null,
                            //     mCookieContainer,
                            //     out async);

                            profileData = HttpHelper.GetWebPageResponse(profileURL,
                                "Gender=F",
                                null,
                                mCookieContainer,
                                true);
                        }
                        else
                        {
                            profileData = HttpHelper.GetWebPageResponse(profileURL,
                                "Gender=F",
                                null,
                                mCookieContainer,
                                true);
                        }

                        if (String.IsNullOrEmpty(profileData))
                        {
                            Console.WriteLine("** No response to profile page request: server busy");
                            LogFailedProfileDownload(profileId);
                            continue;
                        }
                        if (profileData.IndexOf("Sorry, you cannot view this profile") > 0)
                        {
                            Console.WriteLine("Profile Locked!");
                            continue;
                        }
                        if (bTouchProfiles)
                        {
                            Console.WriteLine("Profile viewed successfully");
                            continue;
                        }

                        if (profileData.IndexOf("This profile require login before it get viewed") < 0)
                        {
                            // Create folder with profileId
                            DirectoryInfo di = CreateDir(profilePath);

                            // Create biodata file
                            string biodataFileName = profileId + ".htm";
                            string biodataPath = profilePath + "\\" + biodataFileName;
                            CreateAndWriteFile(biodataPath, profileData);

                            // Create photo files
                            int tempIndex = 0;
                            string profilePhotoId = StringParser.GetStringBetween(profileData, 0, strBeforeProfilePhotoId, strAfterProfilePhotoId, null, out tempIndex);

                            if (!String.IsNullOrEmpty(profilePhotoId))
                            {
                                DownloadPhotos(mCookieContainer, profilePath, profilePhotoId);
                            }
                        }
                        else
                        {
                            Console.WriteLine("**** Login needed to download profile");
                        }
                    }
                }
            } while (bKeepDoing);

            // Initial search page A: http://www.jeevansathi.com/profile/search.php
            // Each result page will have 10 profiles
            // Pick "http://www.jeevansathi.com/profile/matrimonial-" + profileID.htm strings (occur 3 times for each profile)
            // and get the pages. Save each of these pages as htm page.
            // Between each member profile may search for "eValue Member" and name the contacts file accordingly.

            // Get SearchID as
            // "checksum=d0c089efa50843ec7c47e9ca52f86d09|i|aee434154bbe5fa0a2b8ae997f648490i4142500&searchid=63825217"
   
            // Further pages: How to navigate (samples, choose last)
            // "http://www.jeevansathi.com/profile/search.php?searchid=63822404&j=3&contact=&SIM_USERNAME=&NAVIGATOR=SR:searchid__63822404@j__2@contact__@SIM_USERNAME__/Back+to+Search+Results;;&overwrite=1&google_kwd="
            // "http://www.jeevansathi.com/profile/search.php?searchid=63822125&j=3&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63822125%40j__2%40contact__%40SIM_USERNAME__%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd=
            // "http://www.jeevansathi.com/profile/search.php?searchid=63824911&j=2&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63824911%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd="
            // "http://www.jeevansathi.com/profile/search.php?searchid=63825787&j=2&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63825787%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd="
            
            // Referer page: It is just the previous url :)
            // "http://www.jeevansathi.com/profile/search.php?searchid=63825787&j=2&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63825787%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd="
            
            // Here search id is the only variant and seems can be random number
            
            // j=CurrentPageNum [just keep changing this], j__PrevPageNum (referrer page, not important, just keep it CurrentPageNum-1)
            // go till page 14490
            // Pick all 1.4 lakh profiles one by one, check how to keep navigating till end 


            // On each profile page B: http://www.jeevansathi.com/profile/matrimonial-221812s4.htm
            // Create a folder 221812s4, inside that save biodata, photos, contact details

            // Save:
            // 1. PROFILE data as html page


            // How to save photos?
            // 2. PHOTOS
            // On profile's page search for "<a class=thickbox href=" ("Click here for Album" or "View photo")
            // just next is the photo link

            // Photo links:
            
            // Page B has this link, from where need to parse out profileID and then download mainphoto, albumphoto1, albumphoto2.
            // Savable photos:
            // http://ser7.jeevansathi.com/profile/photo_serve.php?profileid=a50f9004d4e5697c729cdea84ca7c2ffi126380&photo=PROFILEPHOTO
            
            
            // 
            // http://ser4.jeevansathi.com/profile/photo_serve.php?profileid=a50f9004d4e5697c729cdea84ca7c2ffi126380&photo=MAINPHOTO
            // http://ser4.jeevansathi.com/profile/photo_serve.php?profileid=a50f9004d4e5697c729cdea84ca7c2ffi126380&photo=ALBUMPHOTO1
            // http://ser4.jeevansathi.com/profile/photo_serve.php?profileid=a50f9004d4e5697c729cdea84ca7c2ffi126380&photo=ALBUMPHOTO2

            // ** Change names of photos while saving each, can do that in offline processing as well 
            // 


            // 3. CONTACT DETAILS : 
            // Contact file name will be : X.txt where X=evalue,erishta,free
            // These come with the profile's htm. Can be processed later offline.


            // Offline processing: later part
            // Will late do offline re-build of all database by replacing JS links with local redirections
            // will once fetch all images etc. from JS and store locally
            // Will in one pass replace JS links with local links in all profile HTML pages.

            //pageData = HttpHelper.GetWebPageResponse("http://www.jeevansathi.com/profile/search.php?searchid=63824911&j=2&contact=&SIM_USERNAME=&NAVIGATOR=SR%3Asearchid__63824911%2FBack+to+Search+Results%3B%3B&overwrite=1&google_kwd=",
            //    null,
            //    null,

        }

        class SearchOptions
        {
            public int startPage;
            public int endPage;
            public string username;
        }

        static void ThreadProcSync(Object stateInfo)
        {
            SearchOptions so = (SearchOptions)stateInfo;

            //Thread.Sleep(so.startPage * 10);

            LogIn(so.startPage, so.endPage, so.username);

        }

        static void Main(string[] args)
        {
            int workerThreads = 0;
            int completionPortThreads = 0;
  
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

            Console.WriteLine("Max --- wt = {0}, cpt = {1}", workerThreads, completionPortThreads);

            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);

            Console.WriteLine("Min --- wt = {0}, cpt = {1}", workerThreads, completionPortThreads);

            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);

            Console.WriteLine("Available --- wt = {0}, cpt = {1}", workerThreads, completionPortThreads);

            int instanceNum = int.Parse(args[2]);
            Thread.Sleep(1000 * instanceNum);
            // For async, optimal values are workerThreads / 20, completionPortThreads / 100
            // 250, 1000 = 61 sec, rate = 80
            // in async , if threadpool is large then lot of context switching

            //for 250/2 , 1000/4 and /4, /8 etc. threads get stuck (reason unknown , switching or otherwise) 
            //rate is 50 at stuck, then it slows down after logjam gets cleared after sometime.
            //mostly reason is too many threads and hence switching.


            //for 250/16, 1000/32 . rate slowly improves from 20 to 45 linearly. 45 at end

            //for 250/20, 1000/50. rate quickly picks up to 80 and then declines to 74

            //for 250/20, 1000/100. rate increases linearly to 80 and then ends at 75


            // For sync, 5000 requests
            // 250, 1000 = 61 sec, rate = 80
            // 250/2 , 1000/4 = 60 sec , rate = 82
            // 250/2.5 , 1000/5 = 69 sec , rate = 73
            // 250/4 , 1000/8 = 101 sec , rate = 50
            // 1, 1 = 260 sec, rate = slowly increased from 4 to 20 linearly

            //optimal values are workerThreads / 2, completionPortThreads / 4

            //if (!ThreadPool.SetMinThreads(10, 20))
            {
                Console.WriteLine("Failed setting MinThreads");
            }

            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);

            Console.WriteLine("Modified Min --- wt = {0}, cpt = {1}", workerThreads, completionPortThreads);

            int ThreadCount = 1;

            int startProfile = int.Parse(args[0]);
            int totalProfiles = int.Parse(args[1]);

            int perThreadChunk = totalProfiles / ThreadCount;

            //var handles = new ManualResetEvent[ThreadCount];

            //Action<HttpReqParms> fetchUrl = ThreadProc;

            for (int i = 1; i <= ThreadCount; i++)
            {
                SearchOptions so = new SearchOptions();
                so.startPage = (i - 1) * perThreadChunk + 1 + startProfile;
                so.endPage = i * perThreadChunk + startProfile;

                so.startPage /= 10;
                so.endPage /= 10;

                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProcSync), so);
            }
            


            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadLine();
        }
    }
}
