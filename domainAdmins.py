import os
import base64
import socket

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
    if os.path.exists("logs/alerts/domainAdmins") == True:
        f = open("logs/alerts/domainAdmins","r")
        for i in f.read().splitlines():
            admincount = i.split(":")[0]
            sAMAccountNames = i.split(":")[1]
            newadmins = domainAdmins.split(":")[1]
            if admincount != ac:
                print("[*] HUE HUE New Domain Admin Added")
                print("New Admin Names : ")
                for index, i in enumerate(newadmins.split(",")):
                    if (index == len(newadmins.split(",")) - 1):
                            break
                    print(i)

            else:
                print("Current Admin Names ")
                for i in sAMAccountNames.split(","):
                    print(i)

        f.close()

    else:
        f = open("logs/domainAdmins","w")
        f.write(domainAdmins)
        f.close()
