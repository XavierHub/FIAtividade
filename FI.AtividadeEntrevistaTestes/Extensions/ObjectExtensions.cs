using System;

public static class ObjectExtensions
{
    public static T Get<T>(this object obj, string propertyName)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var property = obj.GetType().GetProperty(propertyName);
        if (property == null)
            throw new ArgumentException($"Property '{propertyName}' not found on object of type '{obj.GetType().FullName}'");

        return (T)property.GetValue(obj);
    }
}
