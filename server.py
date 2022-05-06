import os
import socket
import time
import base64
import admincount
import computerAccounts
import domainAdmins
import problemHelpdeskAdmins
import passwordSprayAnalysis

host = "0.0.0.0"
port = 1337

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)


s.bind((host, port))
s.listen()

if os.path.exists("logs/") == False:
    os.mkdir("logs/")

if os.path.exists("logs/alerts") == False:
    os.mkdir("logs/alerts")

print("[*] Server started")
while(True):
    try:
        admincount.ReceiveAdmins(s)
        print("Received Admins")
        computerAccounts.ReceiveComputers(s)
        print("Received Computers")
        domainAdmins.ReceiveDomainAdmins(s)
        print("Received Domain Admins")
        problemHelpdeskAdmins.ReceiveHelpdeskAdmin(s)
        print("Received Helpdesk Admins")
        passwordSprayAnalysis.ReceiveBadPassDetails(s)
        print("Received Bad Pass")
        #time.sleep(300)
    except KeyboardInterrupt:
        s.close()
        break
    except Exception as e:
        print("Exception occured")
        print(e)
        break

s.close()
