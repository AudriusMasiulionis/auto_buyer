using Amazon.DynamoDBv2.DataModel;
using FastEndpoints;
using Api.Helpers;
using System.Text.Json;

namespace Api.Jobs;

[DynamoDBTable("Jobs")]
public class JobRecord : IJobStorageRecord, IJobResultStorage
{
    [DynamoDBProperty(Converter = typeof(GuidConverter))]
    [DynamoDBHashKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string QueueID { get; set; } = default!;
    [DynamoDBProperty(Converter = typeof(GuidConverter))]
    public Guid TrackingID { get; set; } =  Guid.NewGuid();
    [DynamoDBIgnore]
    public object Command { get; set; } = default!;
    public string CommandJson { get; set; } = default!;
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }
    public bool IsCancelled { get; set; }
    public object? Result { get; set; }
    public string ResultJson { get; set; } = default!;

    TCommand IJobStorageRecord.GetCommand<TCommand>()
        => JsonSerializer.Deserialize<TCommand>(CommandJson)!;

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command)
        => CommandJson = JsonSerializer.Serialize(command);

    TResult? IJobResultStorage.GetResult<TResult>() where TResult : default
        => ResultJson is not null
               ? JsonSerializer.Deserialize<TResult>(ResultJson)
               : default;

    void IJobResultStorage.SetResult<TResult>(TResult result)
        => ResultJson = JsonSerializer.Serialize(result);

    
}