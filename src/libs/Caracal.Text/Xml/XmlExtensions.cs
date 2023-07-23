﻿using System.Xml;
using System.Xml.Serialization;

namespace Caracal.Text.Xml;

public static class XmlExtensions
{
    public static string ToXml(this object obj) {
        var serializer = new XmlSerializer(obj.GetType());
        using var writer = new StringWriter();
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }
    
    public static T FromXml<T>(this string xml) {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xml);
        return (T) serializer.Deserialize(reader)! ?? throw new XmlException();
    }
}