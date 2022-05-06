import socket
import os
import base64

def ReceiveAdmins(s):
    conn, add = s.accept()
    data = conn.recv(1024)
    decodedString = base64.b64decode(data).decode()
    print(decodedString)

    MonitorAdmins(decodedString)

    conn.send(b"Received Admin Count and their names\n")
    conn.close()

def MonitorAdmins(adminDetails):
    ac = adminDetails.split(":")[0]
    if os.path.exists("logs/adminDetails") == True:
        f = open("logs/adminDetails","r")
        for i in f.read().splitlines():
            admincount = i.split(":")[0]
            sAMAccountNames = i.split(":")[1]
            if admincount != ac:
                print("[*] HUE HUE Admin Changed!!!!!!")
                print("New admin names ")
                if os.path.exists("logs/alerts/adminAlerts") == True:
                    if os.path.getsize("logs/alerts/adminAlerts") == 0:
                        alert = open("logs/alerts/adminAlerts","a")
                else:
                    alert = open("logs/alerts/adminAlerts","w")
                for index, i in enumerate(sAMAccountNames.split(",")):
                    if (index == len(sAMAccountNames.split(",")) - 1):
                        break
                    alert.write("New Admins Added : ")
                    alert.write(i+",")
                    print(i)
                alert.write("\n")
                alert.close()

            else:
                print("Current Admin Names")
                for i in sAMAccountNames.split(","):
                    print(i)


        f.close()
    else:
        f = open("logs/adminDetails","w")
        f.write(adminDetails)
        f.close()
