using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

using static LANALyser.Conversions;
using static LANALyser.Filters;

public struct admindetails
{
    public int admincount;
    public string[] sAMAccountNames;
}

public struct compDetail
{
    public string groupname;
    public string compName;
    public bool ismember;
}

public struct badlogin
{
    public Int64 badPasswordTime;
    public int badPwdCount;
    public string sAMAccountName;
}

namespace LANALyser
{
    public class Detections
    {
        public static admindetails MonitorAdmins()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string rootldap = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();

            DirectoryEntry d = new DirectoryEntry(rootldap);
            DirectorySearcher ds = new DirectorySearcher(d);

            int admincount = 0;
            admindetails ad = new admindetails();
            string[] sAMAccountNames = new string[10000];
            ad = GetNumberOfUsers("Server Administrators", ds);
            Array.Copy(ad.sAMAccountNames, 0, sAMAccountNames, 0, ad.admincount);
            admincount = admincount + ad.admincount;

            ad =  GetNumberOfUsers("Domain Admins", ds);
            Array.Copy(ad.sAMAccountNames, 0, sAMAccountNames, admincount, ad.admincount);
            admincount = admincount + ad.admincount;

            ad = GetNumberOfUsers("Exchange Admins", ds);
            Array.Copy(ad.sAMAccountNames, 0, sAMAccountNames, admincount, ad.admincount);
            admincount = admincount + ad.admincount;

            ad = GetNumberOfUsers("Helpdesk Admins", ds);
            Array.Copy(ad.sAMAccountNames, 0, sAMAccountNames, admincount, ad.admincount);
            admincount = admincount + ad.admincount;

            string[] uniqueNames = RemoveDuplicates(sAMAccountNames);

            for (int i=0; i<admincount;i++)
            {
                if (uniqueNames[i] == null)
                {
                    admincount = i;
                    break;
                }
                Console.WriteLine(uniqueNames[i]);
            }
            Console.WriteLine("Admin Count : " + admincount);
            ad.admincount = admincount;
            ad.sAMAccountNames = uniqueNames;
            return ad;
        }
        public static admindetails GetNumberOfUsers(string FilterType, DirectorySearcher ds)
        {
            int count = 0;
            ds.Filter = GetFilter(FilterType);
            string[] properties = { "sAMAccountName"};
            List<string> sAMAccountNames = new List<string>();
            admindetails ad = new admindetails();
            try
            {
                SearchResultCollection results = ds.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult result in results)
                    {
                        int c = 0;
                        foreach (string i in properties)
                        {
                            if (result.Properties[i.ToLower()].Count > 0)
                            {
                                var prop = result.Properties[i.ToLower()][0];
                                Type tp = prop.GetType();
                                if (tp.Equals(typeof(string)))
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    sAMAccountNames.Add(prop.ToString());
                                }
                                else
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                }
                            }
                            else
                                Console.WriteLine(i + " : " + "Not Found");
                        }
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine();
                    }
                    count = results.Count;
                    ad.admincount = count;
                    ad.sAMAccountNames = sAMAccountNames.ToArray();
                }
                else
                {
                    Console.WriteLine("No result found");
                    return ad;
                }
            }
            catch (ArgumentException e)
            {
                if (e.ToString().Contains("search filter is invalid"))
                {
                    Console.WriteLine("Invalid LDAP Query");
                    return ad;
                }
            }
            return ad;
        }

        public static compDetail MonitorComputerAccounts()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string rootldap = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();

            DirectoryEntry d = new DirectoryEntry(rootldap);
            DirectorySearcher ds = new DirectorySearcher(d);

            compDetail cd = new compDetail();
            compDetail[] compDetails = new compDetail[100];
            compDetails = GetComputerAccounts("Get Computer Accounts",ds);
            Console.WriteLine("Sanity Check");
            foreach (compDetail i in compDetails)
            {
                if (i.ismember == true)
                {
                    Console.WriteLine("Member of Group : " + i.ismember);
                    Console.WriteLine("Group Name : " + i.groupname);
                    Console.WriteLine("Computer Name : " + i.compName);
                    cd = i;
                }
            }
            return cd;
        }

        public static admindetails MonitorDomainAdmins()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string rootldap = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();

            DirectoryEntry d = new DirectoryEntry(rootldap);
            DirectorySearcher ds = new DirectorySearcher(d);

            admindetails ad = GetNumberOfUsers("Domain Admins", ds);
            Console.WriteLine("Users in Domain admins : ");

            foreach(string i in ad.sAMAccountNames)
            {
                if (i != null)
                {
                    Console.WriteLine("Name of the user account : " + i);
                }
            }
            return ad;
        }

        public static string MonitorHelpdeskAdmins()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string rootldap = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();

            DirectoryEntry d = new DirectoryEntry(rootldap);
            DirectorySearcher ds = new DirectorySearcher(d);

            admindetails ad = GetNumberOfUsers("Helpdesk Admins", ds);

            admindetails ad1 = GetNumberOfUsers("Server Administrators", ds);
            string hue = null;

            foreach(string i in ad.sAMAccountNames)
            {
                if (Array.Exists(ad1.sAMAccountNames, element => element == i))
                {
                    Console.WriteLine("This Helpdesk Admin is also a member of Server Adminstrators : " + i);
                    hue = i;
                }
            }
            if (hue == null)
            {
                return "Nothing";
            }
            else
            {
                return hue;
            }
        }

        public static badlogin[] MonitorPasswordSprays()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string rootldap = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();

            DirectoryEntry d = new DirectoryEntry(rootldap);
            DirectorySearcher ds = new DirectorySearcher(d);

            badlogin[] bl = new badlogin[500];
            bl = GetBadLogins("All Users", ds);

            List<badlogin> b2 = new List<badlogin>();
            foreach(badlogin i in bl)
            {
                if (i.badPwdCount != 0)
                {
                    b2.Add(i);
                }
            }

            return b2.ToArray();
        }

        public static badlogin[] GetBadLogins(string FilterType, DirectorySearcher ds)
        {
            ds.Filter = GetFilter(FilterType);
            string[] properties = { "badPasswordTime", "badPwdCount", "sAMAccountName" };
            List<badlogin> logins = new List<badlogin>();
            try
            {
                SearchResultCollection results = ds.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult result in results)
                    {
                        badlogin b = new badlogin();
                        foreach (string i in properties)
                        {
                            if (result.Properties[i.ToLower()].Count > 0)
                            {
                                var prop = result.Properties[i.ToLower()][0];
                                Type tp = prop.GetType();
                                if (tp.Equals(typeof(string)) && i.ToLower().Contains("samaccountname"))
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    b.sAMAccountName = prop.ToString();
                                }

                                else if (tp.Equals(typeof(Int64)) && i.ToLower().Contains("badpasswordtime"))
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    b.badPasswordTime = Int64.Parse(prop.ToString());
                                }

                                else
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    b.badPwdCount = Int32.Parse(prop.ToString());
                                }
                            }
                            else
                                Console.WriteLine(i + " : " + "Not Found");
                        }
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine();
                        logins.Add(b);
                    }
                }
                else
                {
                    Console.WriteLine("No result found");
                }
            }
            catch (ArgumentException e)
            {
                if (e.ToString().Contains("search filter is invalid"))
                {
                    Console.WriteLine("Invalid LDAP Query");
                }
            }
            return logins.ToArray();
        }

        public static string[] GetAllUsers(string FilterType, DirectorySearcher ds)
        {
            ds.Filter = GetFilter(FilterType);
            string[] properties = { "sAMAccountName" };
            List<string> samaccountnames = new List< string > ();
            try
            {
                SearchResultCollection results = ds.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult result in results)
                    {
                        foreach (string i in properties)
                        {
                            if (result.Properties[i.ToLower()].Count > 0)
                            {
                                var prop = result.Properties[i.ToLower()][0];
                                Type tp = prop.GetType();
                                if (tp.Equals(typeof(string)))
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    samaccountnames.Add(prop.ToString());
                                }
                                else
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                }
                            }
                            else
                                Console.WriteLine(i + " : " + "Not Found");
                        }
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No result found");
                }
            }
            catch (ArgumentException e)
            {
                if (e.ToString().Contains("search filter is invalid"))
                {
                    Console.WriteLine("Invalid LDAP Query");
                }
            }
            string[] sAMAccountNames = samaccountnames.ToArray();
            return sAMAccountNames;
        }
        public static compDetail[] GetComputerAccounts(string FilterType, DirectorySearcher ds)
        {
            ds.Filter = GetFilter(FilterType);
            string[] properties = { "name" , "memberOf"};
            int count = 0;
            compDetail[] compDetails = new compDetail[100];
            try
            {
                SearchResultCollection results = ds.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult result in results)
                    {
                        foreach (string i in properties)
                        {
                            if (result.Properties[i.ToLower()].Count > 0)
                            {
                                var prop = result.Properties[i.ToLower()][0];
                                Type tp = prop.GetType();
                                if (tp.Equals(typeof(string)) && i.ToLower().Contains("name"))
                                {
                                    Console.WriteLine(i + " : " + prop.ToString());
                                    compDetails[count].compName = prop.ToString();
                                }

                                else
                                {
                                    //Check if it is a member of any security group
                                    if (prop.ToString() != null)
                                    {
                                        compDetails[count].ismember = true;
                                        compDetails[count].groupname = prop.ToString();
                                    }
                                }

                            }
                            else
                                Console.WriteLine(i + " : " + "Not Found");
                        }
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine();
                        count++;
                    }
                }
                else
                {
                    Console.WriteLine("No result found");
                }
            }
            catch (ArgumentException e)
            {
                if (e.ToString().Contains("search filter is invalid"))
                {
                    Console.WriteLine("Invalid LDAP Query");
                }
            }
            return compDetails;
        }

        public static string[] RemoveDuplicates(string[] s)
        {
            HashSet<string> set = new HashSet<string>(s);
            string[] result = new string[set.Count];
            set.CopyTo(result);
            return result;
        }
    }
}
