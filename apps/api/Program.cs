using FastEndpoints;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddFastEndpoints();

if (builder.Configuration["DYNAMO_ENDPOINT"] is null || builder.Configuration["DYNAMO_ENDPOINT"] ==  string.Empty)
{
    throw new Exception("DYNAMO_ENDPOINT environment variable is required");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dynamoDbConfig = new AmazonDynamoDBConfig
{
    ServiceURL = builder.Configuration["DYNAMO_ENDPOINT"]
};

// Use BasicAWSCredentials with fake keys
// todo make production ready
var dynamoDbClient = new AmazonDynamoDBClient(new BasicAWSCredentials("fakeAccessKey", "fakeSecretKey"), dynamoDbConfig);

// Register DynamoDB client
builder.Services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

await CreateTableIfNotExists(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFastEndpoints();

app.Run();


async Task CreateTableIfNotExists(IServiceProvider services)
{
    var dynamoDb = services.GetRequiredService<IAmazonDynamoDB>();
    
    var tableName = "Contracts"; // Name of the table
    var existingTables = await dynamoDb.ListTablesAsync();

    // Check if the table already exists
    if (!existingTables.TableNames.Contains(tableName))
    {
        var createTableRequest = new CreateTableRequest
        {
            TableName = tableName,
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("Id", KeyType.HASH) // Partition key
            },
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("Id", ScalarAttributeType.S) // String type
            },
            ProvisionedThroughput = new ProvisionedThroughput(5, 5) // Read and write capacity
        };

        // Create the table
        var response = await dynamoDb.CreateTableAsync(createTableRequest);
        Console.WriteLine($"Creating table {tableName}: {response.TableDescription.TableStatus}");
    }
}

