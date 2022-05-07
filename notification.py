import json
import sys
import requests

def send_notifications(message, title, colorcode):
    url = "<Webhook>"
    message = message
    title = title
    slack_data = {
            "username": "PMTH Notifications",
            "icon_emoji": ":satellite:",
            #"channel": "<Channel Name>",
            "attachments": [
                {
                    "color": colorcode,
                    "fields": [
                        {
                            "title": title,
                            "value": message,
                            "short": "false",
                        }
                    ]
                }
            ]
        }
    byte_length = str(sys.getsizeof(slack_data))
    headers = { 'Content-Type': "application/json", 'Content-Length': byte_length }
    response = requests.post(url, data=json.dumps(slack_data), headers=headers)
    if response.status_code != 200:
        raise Exception(response.status_code, response.text)

    else:
        print(response.text)

#send_notifications("Change in Domain Admins", (f"Domain Admins"))
