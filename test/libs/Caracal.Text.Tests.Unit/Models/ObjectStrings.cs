namespace Caracal.Text.Tests.Unit.Models;

public static class ObjectStrings
{
    public const string SimpleXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User>
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
        </User>
        """;
    
    public const string ComplexXml = """
        <?xml version="1.0" encoding="utf-16"?>
        <User>
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
          <Address>
            <Street>Main</Street>
            <Number>123</Number>
          </Address>
        </User>
        """;
    
    public const string ComplexXmlWithAttributesAndLists = """
        <?xml version="1.0" encoding="utf-16"?>
        <User userId="MockId1">
          <FirstName>John</FirstName>
          <LastName>Doe</LastName>
          <Age>42</Age>
          <Address>
            <Street>Main</Street>
            <Number>123</Number>
          </Address>
          <Roles>
            <Role id="MockId2">Admin</Role>
            <Role id="MockId3">User</Role>
         </Roles>
        </User>
        """;
    
    public const string SimpleJson = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":42,\"Address\":null}";
    public const string ComplexJson = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":42,\"Address\":{\"Street\":\"Main\",\"Number\":123}}";
    public const string JsonFromXml = "{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"utf-16\"},\"User\":{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Age\":\"42\",\"Address\":{\"Street\":\"Main\",\"Number\":\"123\"}}}";
    
    public const string SimpleXslt = """
        <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
            <xsl:template match="User">
                <Employee>
                    <FirstName>
                        <xsl:value-of select="FirstName"/>
                    </FirstName>
                    <Surname>
                        <xsl:value-of select="LastName"/>
                    </Surname>
                </Employee>
            </xsl:template>
        </xsl:stylesheet>
        """; 
    
    public const string ComplexXslt = """
        <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
            <xsl:template match="User">
                <Employee>
                    <xsl:attribute name="Id">
                        <xsl:value-of select="@userId"/>
                    </xsl:attribute>
                    <FirstName>
                        <xsl:value-of select="FirstName"/>
                    </FirstName>
                    <Surname>
                        <xsl:value-of select="LastName"/>
                    </Surname>
                    <Roles>
                        <Role>Admin</Role>
                        <Role>Employee</Role>
                    </Roles>
                </Employee>
            </xsl:template>
        </xsl:stylesheet>
        """;
}