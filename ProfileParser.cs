#define LOGINRUN
#define M_PROFILES
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
//using System.Net;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;


namespace Jeevansathi
{
    partial class Program
    {
        static void Main(string[] args)
        {

            // Come till here for contact details
            //<div class="lf"><img src="http://ser4.jeevansathi.com/img_revamp/cont_top_bg.gif"></div> 

            //<ul class="con_list"> 
            // then pickup each field
            //<li>Mobile no.</li> 
            //<li>Landline no.</li> 
            //<li>Suitable time to call</li> 
            //<li>Address</li> 
            //<li>Parent's address</li> 
            //<li>Messenger ID</li> 
            //</ul>

            //<li>Mobile no. of  Parent (Sibling)<br>
            //<b>9938592881</b></li>
            //<li>Suitable time to call<br>
            //<b>1 AM to 1 AM</b></li>
            //<li>Parent's address<br>
            //<b>plot no 82 , nageshwar tangi , HB colony bhubaneswar orissa</b></li>
            //<li>email ID<br>
            //<b>farha.patnaik@gmail.com</b>
            //</li>
            //<li>Messenger ID<br>
            //<b>farha_s_patnaik@Yahoo</b></li>



            // Specify the directory you want to manipulate.
            string profilesDir = @"D:\JS\";

            string searchPattern = "*.htm";

            DirectoryInfo di = new DirectoryInfo(profilesDir);
            DirectoryInfo[] directories =
                di.GetDirectories("*", SearchOption.TopDirectoryOnly);

            FileInfo[] files =
                di.GetFiles(searchPattern, SearchOption.AllDirectories);

            Console.WriteLine(
                "All sub-directories in {0}", profilesDir);
            foreach (DirectoryInfo dir in directories)
            {
                Console.WriteLine(
                    "{0,-25} {1,25}", dir.FullName, dir.LastWriteTime);
            }

            Console.WriteLine();
            Console.WriteLine("Files with pattern {0} in {0}", searchPattern, profilesDir);

            //string strPreContacts = "<div class=\"lf\"><img src=\"http://ser4.jeevansathi.com/img_revamp/cont_top_bg.gif\"></div>"; 

            string cdLocked = "Contact Details are Locked";

            //string strPreContacts = "<ul class=\"con_list\""; 
            string strContactsStart = "<ul class=\"con_list\""; 
            //string strContactsStart = "<li>";
            string strContactsEnd = "</ul>";
            
            string fldStart = "<li>";
            string fldEnd = "</li>";

            string fldDataSep = "<br>";

            string dataStart = "<b>";
            string dataEnd = "</b>";

            string noiseStr1 = "<br />";
            string noiseStr2 = "<br/>";


            foreach (FileInfo file in files)
            {
                Console.WriteLine(
                    "{0,-25} {1,25}", file.Name, file.LastWriteTime);

                string profileId = file.Name.Remove(file.Name.IndexOf(".htm", 4));

                // Read the htm file

                string fileData = File.ReadAllText(file.FullName);

                if (fileData.Contains(cdLocked))
                {
                    Console.WriteLine("Details locked");
                    continue;
                }

                int idxContactsStart = 0;
                
                //idxContactsStart = fileData.IndexOf(strPreContacts);

                //idxContactsStart = fileData.IndexOf(">", idxContactsStart);

                int tempIndex;

                string contactsBlob = StringParser.GetStringBetween(fileData, idxContactsStart, strContactsStart, strContactsEnd, null, out tempIndex);

                int nextDetailIdx = 0;
                string token = StringParser.GetStringBetween(contactsBlob, 0, fldStart, fldEnd, null, out nextDetailIdx);

                while (!String.IsNullOrEmpty(token))
                {
                    // Field Name
                    string fldName = StringParser.GetStringBetween(token, 0, "", fldDataSep, null, out tempIndex);
                    Console.Write(fldName + ": ");

                    // Field's Data value
                    string dataValue = StringParser.GetStringBetween(token, 0, dataStart, dataEnd, null, out tempIndex);
                    
                    // Noise removal
                    int noiseStrIdx = dataValue.IndexOf(noiseStr1);
                    if (noiseStrIdx > 0)
                    {
                        dataValue = dataValue.Remove(noiseStrIdx, noiseStr1.Length);
                    }
                    noiseStrIdx = dataValue.IndexOf(noiseStr2);
                    if (noiseStrIdx > 0)
                    {
                        dataValue = dataValue.Remove(noiseStrIdx, noiseStr2.Length);
                    }
                    Console.WriteLine(dataValue);

                    // Next token
                    token = StringParser.GetStringBetween(contactsBlob, nextDetailIdx, fldStart, fldEnd, null, out nextDetailIdx);

                }


                //    <br>
                //        <li>Mobile no.</li> 
                //            <b>farha.patnaik@gmail.com</b>



                //Console.WriteLine(contactsBlob);
            }

            Console.ReadLine();
        }
    }

}