import base64
import socket
import os
from notification import send_notifications

def ReceiveComputers(s):
    conn, add = s.accept()
    data = conn.recv(1024)
    decodedString = base64.b64decode(data).decode()
    print(decodedString)

    MonitorComputers(decodedString)
    conn.send(b"Received Computer Account\n");
    conn.close()

def MonitorComputers(compDetail):
    try:
        compName = compDetail.split(":")[0]
        groupName = compDetail.split(":")[1]
        if ("empty" in compName or "empty" in groupName):
            return
        if os.path.exists("logs/alerts/compDetails") == True:
            if os.path.getsize("logs/alerts/compDetails") == 0:
                f = open("logs/alerts/compDetails","a")
                f.write(compDetail)
        else:
            f = open("logs/alerts/compDetails","w")
            f.write(compDetail)
            f.close()
            print("Computer Account : " + compName)
            print("Added To security Group : "+groupName)
            message = "Computer Account : " + compName
            send_notifications(message, (f"Computer Account Added :computer:"), "#7fc98a")
    except Exception as e:
        print(e)
