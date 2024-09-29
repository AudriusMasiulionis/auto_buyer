using FastEndpoints;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
var dynamoDbConfig = new AmazonDynamoDBConfig
{
    ServiceURL = builder.Configuration["DYNAMO_ENDPOINT"]
};

builder.Services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(dynamoDbConfig));
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();

