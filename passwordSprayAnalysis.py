import base64
import os
import socket
from notification import send_notifications

def ReceiveBadPassDetails(s):
    conn, add = s.accept()
    data = conn.recv(1024)
    decodedString = base64.b64decode(data).decode()
    print(decodedString)

    if ("empty" in decodedString):
        print("No password spray")
        return

    MonitorPassSprays(decodedString)

    conn.send(b"Received Bad Logins\n")
    conn.close()

def MonitorPassSprays(badlogins):
    try:
        badloginTimes = []
        badloginCounts = []
        sAMAccountNames = ""

        for index, badlogin in enumerate(badlogins.split(":")):
            try:
                if index == len(badlogins.split(":")) - 1:
                    break
                badloginTimes.append(badlogin.split(",")[1])
                badloginCounts.append(badlogin.split(",")[2])
                sAMAccountNames = sAMAccountNames + " , " + badlogin.split(",")[0]
            except Exception as e1:
                print("Exception in Bad Pass analysis")
                print(e1)
                pass

        remainders = []
        for badloginTime in badloginTimes:
            remainders.append(int(int(badloginTime)/10**7))

        print("Remainders : ")
        c = 0
        head = 0
        for remainder in remainders:
            head = remainder
            if (head == remainder):
                c += 1

        if (c > 3):
            if os.path.exists("logs/alerts/passwordSprayDetected"):
                if os.path.getsize("logs/alerts/passwordSprayDetected") == 0:
                    f = open("logs/alerts/passwordSprayDetected","a")
                    c = "Number of attempted password sprays on accounts : " + str(c - 1)

                    f.write(c)
                    f.write(sAMAccountNames)
                    f.close()
                    send_notifications("There might be a password spray incident", (f"Password Spray Detected :key:"), "#de1010")
            else:
                f = open("logs/alerts/passwordSprayDetected","w")
                c = "Number of attempted password sprays on accounts : " + str(c - 1)

                f.write(c)
                f.write(sAMAccountNames)
                send_notifications("There might be a password spray incident", (f"Password Spray Detected :key:"), "#de1010")
                f.close()


    except Exception as e:
        print("Exception occured : ")
        print(e)
        return
