using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

using static LANALyser.Detections;
using static LANALyser.Filters;
using static LANALyser.Client;

namespace LANALyser
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            admindetails ad = MonitorAdmins();
            compDetail cd = MonitorComputerAccounts();
            admindetails ad1 = MonitorDomainAdmins();
            string problemHelpdeskAdmin = MonitorHelpdeskAdmins();
            badlogin[] b = MonitorPasswordSprays();
            */
            while (true)
            {
                admindetails ad = MonitorAdmins();
                compDetail cd = MonitorComputerAccounts();
                admindetails ad1 = MonitorDomainAdmins();
                string problemHelpdeskAdmin = MonitorHelpdeskAdmins();
                badlogin[] b = MonitorPasswordSprays();
                System.Threading.Thread.Sleep(300);

                SendAdmins(ad);
                System.Threading.Thread.Sleep(700);

                if (cd.ismember == true)
                {
                    SendCompDetails(cd);
                }
                else
                {
                    cd.ismember = false;
                    cd.groupname = "empty";
                    cd.compName = "empty";
                    SendCompDetails(cd);
                }
                System.Threading.Thread.Sleep(700);

                SendDomainAdmins(ad1);
                System.Threading.Thread.Sleep(700);
                SendProblemHelpdeskAdmin(problemHelpdeskAdmin);
                System.Threading.Thread.Sleep(700);

                if (b.Count() == 0)
                {
                    badlogin[] bb = new badlogin[1];
                    bb[0].sAMAccountName = "empty";
                    bb[0].badPasswordTime = 0;
                    bb[0].badPwdCount = 0;
                    SendBadLogins(bb);
                }
                else
                {
                    SendBadLogins(b);
                }
            }
        }
    }
}