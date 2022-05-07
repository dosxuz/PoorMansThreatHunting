using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using static LANALyser.Detections;

namespace LANALyser
{
    public class Client
    {
        public static void SendTest(string IP)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337);
                Console.WriteLine("Connected....");

                string data = "Hello from server\n";
                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba,0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i=0;i<k;i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
        }

        public static string SendAdmins(admindetails ad, string IP)
        {
            //Convert from admin details to string
            string number = ad.admincount.ToString();
            string names = "";
            foreach (string i in ad.sAMAccountNames)
            {
                names = names + "," + i;
            }
            string data = number + ":" + names;
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337); //Change me on all the functions
                Console.WriteLine("Connected....");

                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
            return "Admin counts sent";
        }

        public static string SendCompDetails(compDetail cd, string IP)
        {
            string data = cd.compName + ":" + cd.groupname + cd.ismember.ToString();
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337);
                Console.WriteLine("Connected....");

                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
            return "Computer Account Details Sent";
        }

        public static string SendDomainAdmins(admindetails ad, string IP)
        {
            //Convert from admin details to string
            string number = ad.admincount.ToString();
            string names = "";
            foreach (string i in ad.sAMAccountNames)
            {
                names = names + "," + i;
            }
            string data = number + ":" + names;
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337);
                Console.WriteLine("Connected....");

                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
            return "Domain Admins Sent";
        }

        public static string SendProblemHelpdeskAdmin(string data, string IP)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337);
                Console.WriteLine("Connected....");

                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
            return "Problematic helpdesk admin sent";
        }

        public static string SendBadLogins(badlogin[] b, string IP)
        {
            string data = "";
            if (b[0].sAMAccountName != "empty")
            {
                foreach (badlogin bd in b)
                {
                    data = data + bd.sAMAccountName + "," + bd.badPasswordTime.ToString() + "," + bd.badPwdCount.ToString() + ":";
                }
            }
            else
            {
                data = "empty";
            }
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");

                client.Connect(IP, 1337);
                Console.WriteLine("Connected....");

                string encodedData = Base64Encode(data);
                Stream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(encodedData);
                Console.WriteLine("Sending data...");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception....." + e.StackTrace);
            }
            return "Send Badlogins";
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
