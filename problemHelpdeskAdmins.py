import base64
import os
import socket
from notification import send_notifications

def ReceiveHelpdeskAdmin(s):
    conn, add = s.accept()
    data = conn.recv(1024)
    decodedString = base64.b64decode(data).decode()
    print(decodedString)

    MonitorHelpdeskAdmins(decodedString)

    conn.send(b"Received Problematic helpdesk admin\n")
    conn.close()

def MonitorHelpdeskAdmins(helpdeskAdmin):
    try:
        if "Nothing" in helpdeskAdmin:
            return
        if os.path.exists("logs/alerts/helpdeskAdmins") == True:
            if os.path.getsize("logs/alerts/helpdeskAdmins") == 0:
                f = open("logs/alerts/helpdeskAdmins","a")
                s = "This Helpdesk Administrator is also a member of Server Administrator : " + helpdeskAdmin + "\n"
                f.write(s)
                f.close()
                send_notifications("HUE HUE There is a Helpdesk Admin also a member of Server Administrators Group", (f"Helpdesk Admin problem :information_desk_person:"), "#36b80b")
        else:
            f = open("logs/alerts/helpdeskAdmins","w")
            s = "This Helpdesk Administrator is also a member of Server Administrator : " + helpdeskAdmin + "\n"
            f.write(s)
            f.close()
            send_notifications("HUE HUE There is a Helpdesk Admin also a member of Server Administrators Group", (f"Helpdesk Admin problem :information_desk_person:"), "#36b80b")
    except Exception as e:
        return
