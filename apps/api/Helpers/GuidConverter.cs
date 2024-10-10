using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Api.Helpers;

public class GuidConverter : IPropertyConverter
{
    public object FromEntry(DynamoDBEntry entry)
    {
        if (entry is Primitive primitive)
        {
            if (primitive.Type == DynamoDBEntryType.String)
            {
                return Guid.Parse(primitive.AsString());
            }
            throw new InvalidOperationException("Cannot convert entry to Guid.");
        }
        throw new InvalidOperationException("Cannot convert entry to Guid.");
    }

    public DynamoDBEntry ToEntry(object value)
    {
        if (value is Guid guid)
        {
            return new Primitive(guid.ToString());
        }
        throw new InvalidOperationException("Cannot convert value to DynamoDBEntry.");
    }
}