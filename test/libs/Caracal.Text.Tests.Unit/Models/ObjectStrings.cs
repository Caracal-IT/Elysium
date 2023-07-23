namespace Caracal.Text.Tests.Unit.Models;

public static class ObjectStrings
{
    public const string SimpleXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
        </User>
        """;
    
    public const string ComplexXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
          <Address>
            <Street>Main</Street>
            <Number>123</Number>
          </Address>
        </User>
        """;
    
    public const string SimpleJson = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":42,\"Address\":null}";
    public const string ComplexJson = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":42,\"Address\":{\"Street\":\"Main\",\"Number\":123}}";
    public const string JsonFromXml = "{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"},\"User\":{\"@xmlns:xsi\":\"http://www.w3.org/2001/XMLSchema-instance\",\"@xmlns:xsd\":\"http://www.w3.org/2001/XMLSchema\",\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":\"42\",\"Address\":{\"Street\":\"Main\",\"Number\":\"123\"}}}";
}