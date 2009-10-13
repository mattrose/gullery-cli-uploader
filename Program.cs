using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Net;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CookieContainer CC;
            HttpWebResponse loginresponse;
            GulleryLogin("mattrose","password",out CC, out loginresponse);

            //AddPicture(CC);
            
            //string header = "--" + boundary + "Content-Disposition: form-data; name=\"asset[project_id]\"\r\n\r\n1\r\n--" + boundary + "Content-Disposition: form-data; name="asset[file_field]"; filename="Toco Toucan.jpg" 
        }

        private static void AddPicture(CookieContainer CC)
        {

            //This is where we upload the file
            HttpWebRequest addrequest = (HttpWebRequest)WebRequest.Create("http://www.folkwolf.net:3000/assets/create");
            addrequest.Method = "POST";
            addrequest.CookieContainer = CC;

            string path = @"C:\Users\Public\Pictures\Sample Pictures\Creek.jpg";
            string boundary = "------" + DateTime.Now.Ticks.ToString("x");
            addrequest.ContentType = "multipart/form-data; boundary=" + boundary + "\r\n";

            string sb_proj = ("Content-Disposition: form-data; name=\"asset[project_id]\"\r\n\r\n1\r\n");
            string sb_file = ("Content-Disposition: form-data; name=\"asset[file_field]\"; filename=\"" + path + "\"\r\nContent-Type: image/jpeg\r\n\r\n");
            Console.WriteLine("--" + boundary + "\r\n" + sb_proj + boundary + "\r\n" + sb_file);
            //byte[] mdbyteArray = Encoding.ASCII.GetBytes(sb_proj + sb_file);
            byte[] projbyteArray = Encoding.ASCII.GetBytes("--" + boundary + "\r\n" + sb_proj);
            byte[] fileheadbyteArray = Encoding.ASCII.GetBytes("--" + boundary + "\r\n" + sb_file);
            byte[] filebyteArray = File.ReadAllBytes(path);
            byte[] footbyteArray = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n" + "Content-Disposition: form-data; name=\"asset[caption]\"\r\n\r\n\r\n");
            byte[] footbyteArray2 = Encoding.ASCII.GetBytes("--" + boundary + "\r\n" + "Content-Disposition: form-data; name=\"commit\"\r\n\r\nSave\r\n");
            byte[] footerArray = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            long length = projbyteArray.Length + fileheadbyteArray.Length + filebyteArray.Length + footbyteArray.Length + footbyteArray2.Length + footerArray.Length;
            addrequest.ContentLength = length;
            addrequest.Timeout = 600000;
            Stream requestStream = addrequest.GetRequestStream();
            //requestStream.Write(mdbyteArray, 0, mdbyteArray.Length);
            requestStream.Write(projbyteArray, 0, projbyteArray.Length);
            requestStream.Write(fileheadbyteArray, 0, fileheadbyteArray.Length);
            requestStream.Write(filebyteArray, 0, filebyteArray.Length);
            requestStream.Write(footbyteArray, 0, footbyteArray.Length);
            requestStream.Write(footbyteArray2, 0, footbyteArray2.Length);
            requestStream.Write(footerArray, 0, footerArray.Length);


            HttpWebResponse addresponse = (HttpWebResponse)addrequest.GetResponse();
            Console.WriteLine(((HttpWebResponse)addresponse).StatusDescription);
            Console.WriteLine("\nThe HttpHeaders are \n\n\tName\t\tValue\n{0}", addrequest.Headers);
            foreach (Cookie cook in addresponse.Cookies)
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                Console.WriteLine("Domain: {0}", cook.Domain);
                Console.WriteLine("Path: {0}", cook.Path);
                Console.WriteLine("Port: {0}", cook.Port);
                Console.WriteLine("Secure: {0}", cook.Secure);

                Console.WriteLine("When issued: {0}", cook.TimeStamp);
                Console.WriteLine("Expires: {0} (expired? {1})",
                    cook.Expires, cook.Expired);
                Console.WriteLine("Don't save: {0}", cook.Discard);
                Console.WriteLine("Comment: {0}", cook.Comment);
                Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");

                // Show the string representation of the cookie.
                Console.WriteLine("String: {0}", cook.ToString());
            }
            //HttpWebResponse addresponse = (HttpWebResponse)addrequest.GetResponse();
        }

        private static void GulleryLogin(string username,string password,out CookieContainer CC, out HttpWebResponse loginresponse)
        {
            // This is where we log in

            HttpWebRequest loginrequest = (HttpWebRequest)WebRequest.Create("http://www.folkwolf.net:3000/login");
            loginrequest.Method = "POST";
            loginrequest.ContentType = "application/x-www-form-urlencoded";
            loginrequest.AllowAutoRedirect = false;

            CC = new CookieContainer();
            loginrequest.CookieContainer = CC;

            string postHeader = "login=" + username +"&password=" + password + "&remember_me=0&commit=\"Log in\"";
            byte[] byteArray = Encoding.ASCII.GetBytes(postHeader);
            Stream dataStream = loginrequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            loginresponse = (HttpWebResponse)loginrequest.GetResponse();
            Console.WriteLine(((HttpWebResponse)loginresponse).StatusDescription);
            for (int i = 0; i < loginresponse.Headers.Count; ++i)
                Console.WriteLine("\nHeader Name:{0}, Header value :{1}", loginresponse.Headers.Keys[i], loginresponse.Headers[i]);
            Console.WriteLine("Cookie: {0}", loginresponse.Headers[5]);
            // Print the properties of each cookie.
            foreach (Cookie cook in loginresponse.Cookies)
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                Console.WriteLine("Domain: {0}", cook.Domain);
                Console.WriteLine("Path: {0}", cook.Path);
                Console.WriteLine("Port: {0}", cook.Port);
                Console.WriteLine("Secure: {0}", cook.Secure);

                Console.WriteLine("When issued: {0}", cook.TimeStamp);
                Console.WriteLine("Expires: {0} (expired? {1})",
                    cook.Expires, cook.Expired);
                Console.WriteLine("Don't save: {0}", cook.Discard);
                Console.WriteLine("Comment: {0}", cook.Comment);
                Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");

                // Show the string representation of the cookie.
                Console.WriteLine("String: {0}", cook.ToString());
             
            }
            Console.WriteLine(((HttpWebResponse)loginresponse).StatusCode);
        }
    }
}
