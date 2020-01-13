def get_csharp_item():
    csharp_item = {
        'id': '2',
        'Description': [
            {
                'Name': 'C#',
                'Pronunciation': 'see sharp',
                'Year': 2000,
                'TargetOS': 'Windows',
                'Crossplatform': True,
                'CurrentVersion': 8.0
            }            
        ],
        'DevelopedBy': 'Microsoft'
    }
    return csharp_item


# ---- Add this JSON document manually in the portal, remove the pound/sharp sign ----
# --------------- See Figure 5-35 as an example of how it should look ----------------
#{
#    "id": "2",
#    "Description": [
#        {
#            "Name": "C#",
#            "Pronunciation": "see sharp",
#            "Year": 2000,
#            "TargetOS": "Windows",
#            "Crossplatform": true,
#            "CurrentVersion": 8.0
#        }            
#    ],
#    "DevelopedBy": "Microsoft"
#}
