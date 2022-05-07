import os
import base64
import socket
from notification import send_notifications

def ReceiveDomainAdmins(s):
    conn, add = s.accept()
    data = conn.recv(1024)
    decodedString = base64.b64decode(data).decode()
    print(decodedString)

    MonitorDomainAdmins(decodedString)

    conn.send(b"Received Domain Admin Count and their names\n")
    conn.close()

def MonitorDomainAdmins(domainAdmins):
    ac = domainAdmins.split(":")[0]
    if os.path.exists("logs/domainAdmins") == True:
        f = open("logs/domainAdmins","r")
        for i in f.read().splitlines():
            admincount = i.split(":")[0]
            sAMAccountNames = i.split(":")[1]
            newadmins = domainAdmins.split(":")[1]
            if admincount != ac:
                print("[*] HUE HUE New Domain Admin Added")
                print("New Admin Names : ")
                if os.path.exists("logs/alerts/domainAdmins") == True:
                    if os.path.getsize("logs/alerts/domainAdmins") == 0:

                        alert = open("logs/alerts/domainAdmins","a")
                        alert.write("New Domain Admins : ")

                        for index, i in enumerate(newadmins.split(",")):
                            if (index == len(newadmins.split(",")) - 1):
                                break
                            print(i)
                            alert.write(i+" , ")

                        send_notifications("[*] HUE HUE New Domain Admin Added", (f"Domain Admin Added :technologist:") , "#7e1fd1")
                        alert.close()
                else:
                    alert = open("logs/alerts/domainAdmins","w")
                    alert.write("New Domain Admins : ")

                    for index, i in enumerate(newadmins.split(",")):
                        if (index == len(newadmins.split(",")) - 1):
                            break
                        print(i)
                        alert.write(i+" , ")

                    alert.close()
                    send_notifications("[*] HUE HUE New Domain Admin Added", (f"Domain Admin Added :technologist:") , "#7e1fd1")

            else:
                print("Current Admin Names ")
                for i in sAMAccountNames.split(","):
                    print(i)

        f.close()

    else:
        f = open("logs/domainAdmins","w")
        f.write(domainAdmins)
        f.close()
