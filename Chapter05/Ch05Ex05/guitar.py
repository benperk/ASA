def get_fender_item():
    fender_item = {
        'id': '1',
        'Description': [
            {
                'Brand': 'Fender',
                'Model': 'Stratocaster',
                'Year': 1956,
                'Color': 'Sunburst',
                'Condition': 'Mint',
                'Accessories': 'Case,Strings,Picks'
            }            
        ],
        'OriginalOwner': 'csharpguitar'
    }
    return fender_item

# ---- Add this JSON document manually in the portal, remove the pound/sharp sign ----
# --------------- See Figure 5-35 as an example of how it should look ----------------
#{
#    "id": "1",
#    "Description": [
#    {
#        "Brand": "Fender",
#        "Model": "Stratocaster",
#        "Year": 1956,
#        "Color": "Sunburst",
#        "Condition": "Mint",
#        "Accessories": "Case,Strings,Picks"
#    }            
#    ],
#    "OriginalOwner": "csharpguitar"
#}
