using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANALyser
{
    public class Filters
    {
        public static string GetFilter(string FilterType)
        {
            IDictionary<string, string> FilterTypes = new Dictionary<string, string>();
            FilterTypes.Add("Members of a group", "(memberOf=CN=Domain Admins,CN=Users,DC=dosxuz,DC=local)");
            FilterTypes.Add("Server Administrators", "(memberOf:1.2.840.113556.1.4.1941:=CN=Server Administrators,DC=dosxuz,DC=local)");
            FilterTypes.Add("Domain Admins", "(memberOf=CN=Domain Admins,CN=Users,DC=dosxuz,DC=local)");
            FilterTypes.Add("Exchange Admins", "(memberOf:1.2.840.113556.1.4.1941:=CN=Exchange Admins,DC=dosxuz,DC=local)");
            FilterTypes.Add("Helpdesk Admins", "(memberOf:1.2.840.113556.1.4.1941:=CN=Helpdesk Admins,DC=dosxuz,DC=local)");
            FilterTypes.Add("Server Maintenance", "(memberOf:1.2.840.113556.1.4.1941:=CN=Server Maintenance,DC=dosxuz,DC=local)");
            FilterTypes.Add("Get Computer Accounts", "(&(objectClass=computer))");
            FilterTypes.Add("All Users", "(&(objectClass=person)(objectCategory=user))");
            return FilterTypes[FilterType];
        }
    }
}
