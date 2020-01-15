from azure.cosmos import exceptions, CosmosClient, PartitionKey
import csharp
import guitar

print('-----------------Start------------------')
endpoint = 'https://<REPLACE>.documents.azure.com:443/'
key = '<REPLACE WITH YOUR KEY>'

client = CosmosClient(endpoint, key)
database_name = 'csharpguitar-cosmos-db'
database = client.create_database_if_not_exists(id=database_name)
container_name = 'csharpguitar-container'
container = database.create_container_if_not_exists(
    id=container_name, 
    partition_key=PartitionKey(path="/Year"),
    offer_throughput=400
)
csharpguitar_items_to_create = [csharp.get_csharp_item(), guitar.get_fender_item()]
for csharpguitar_item in csharpguitar_items_to_create:
    container.create_item(body=csharpguitar_item)

query = "SELECT * FROM c WHERE c.id IN ('1', '2')"

items = list(container.query_items(
    query=query,
    enable_cross_partition_query=True
))
request_charge = container.client_connection.last_response_headers['x-ms-request-charge']

print('Query returned {0} items. Operation consumed {1} request units'.format(len(items), request_charge))

print('-----------------End------------------')
