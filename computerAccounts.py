import base64
import socket
import os

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
        f = open("logs/alerts/compDetails","w")
        f.write(compDetail)
        f.close()
        print("Computer Account : " + compName)
        print("Added To security Group : "+groupName)
    except Exception as e:
        print(e)
