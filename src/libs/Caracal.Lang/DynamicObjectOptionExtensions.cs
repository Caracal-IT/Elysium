using System.Reflection;

namespace Caracal.Lang;

public static class DynamicObjectOptionExtensions
{
    public static T? CreateObjectFromOption<T>(this IDynamicObjectOption processorObjectOptions)
    {
        var type = Assembly.Load(processorObjectOptions.Assembly).GetType(processorObjectOptions.Type);
        if (type == null) return default;

        var instance = Activator.CreateInstance(type, processorObjectOptions);

        return instance is not T item ? default : item;
    }
}