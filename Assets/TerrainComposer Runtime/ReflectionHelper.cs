using System;
using System.Linq;
using System.Reflection;

// Made by Ilya Temnikov

public static class ReflectionHelper
{
    public static object CallMethod(this object obj, string methodName, params object[] parameters)
    {
        Type type = obj.GetType();
        var methods =
            type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(t => t.Name == methodName && t.GetParameters().Length == parameters.Length)
                .ToArray();
        if (methods.Length == 1)
        {
            return methods[0].Invoke(obj, parameters);
        }

        var parameterTypes = parameters
            .Select(t => t == null ? typeof(object) : t.GetType())
            .ToArray();

        foreach (var method in methods)
        {
            bool match = true;
            var methodParams = method.GetParameters();
            for (int i = 0; i < methodParams.Length; i++)
            {
                var parameterInfo = methodParams[i];

                if (parameterInfo.ParameterType == parameterTypes[i] ||
                    parameterTypes[i].IsSubclassOf(parameterInfo.ParameterType))
                {
                    continue;
                }
                match = false;
                break;
            }
            if (match)
            {
                return method.Invoke(obj, parameters);
            }
        }

        throw new InvalidOperationException("Can not call method: " + methodName);
    }


    public static object CallStaticMethod<T>(this object obj, string methodName, params object[] parameters)
    {
        Type type = typeof(T);
        var methods =
            type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(t => t.Name == methodName && t.GetParameters().Length == parameters.Length)
                .ToArray();
        if (methods.Length == 1)
        {
            return methods[0].Invoke(null, parameters);
        }

        var parameterTypes = parameters
            .Select(t => t == null ? typeof(object) : t.GetType())
            .ToArray();

        foreach (var method in methods)
        {
            bool match = true;
            var methodParams = method.GetParameters();
            for (int i = 0; i < methodParams.Length; i++)
            {
                var parameterInfo = methodParams[i];

                if (parameterInfo.ParameterType == parameterTypes[i] ||
                    parameterTypes[i].IsSubclassOf(parameterInfo.ParameterType))
                {
                    continue;
                }
                match = false;
                break;
            }
            if (match)
            {
                return method.Invoke(null, parameters);
            }
        }


        throw new InvalidOperationException("Can not call method!");
    }

    public static object GetFieldValue(this object obj, string fieldName)
    {
        Type type = obj.GetType();
        var field =
            type.GetField(fieldName,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field!= null)
        {
            return field.GetValue(obj);
        }

        throw new InvalidOperationException("Failed to get field!");
    }

    public static object GetStaticFieldValue<T>(this object obj, string fieldName)
    {
        Type type = typeof (T);
        var field =
            type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        if (field != null)
        {
            return field.GetValue(null);
        }

        throw new InvalidOperationException("Failed to get field!");
    }

    public static object GetProprtyValue(this object obj, string fieldName, params object[] parameters)
    {
        Type type = obj.GetType();
        var property =
            type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (property != null)
        {
            return property.GetValue(obj, parameters.Length == 0 ? null : parameters);
        }

        throw new InvalidOperationException("Failed to get property!");
    }

    public static object GetStaticProprtyValue<T>(this object obj, string fieldName, params object[] parameters)
    {
        Type type = typeof(T);

        var property =
            type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (property != null)
        {
            return property.GetValue(null, parameters.Length == 0 ? null : parameters);
        }


        throw new InvalidOperationException("Failed to get property!");
    }
}