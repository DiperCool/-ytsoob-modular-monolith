using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BuildingBlocks.Core.Utils;

public static class ReflectionUtilities
{
    public static dynamic CreateGenericType(Type genericType, Type[] typeArguments, params object?[] constructorArgs)
    {
        var type = genericType.MakeGenericType(typeArguments);
        return Activator.CreateInstance(type, constructorArgs);
    }

    public static dynamic CreateGenericType<TGenericType>(Type[] typeArguments, params object?[] constructorArgs)
    {
        return CreateGenericType(typeof(TGenericType), typeArguments, constructorArgs);
    }

    public static IEnumerable<Type> GetAllTypesImplementingInterface<TInterface>(params Assembly[] assemblies)
    {
        var inputAssemblies = assemblies.Any() ? assemblies : AppDomain.CurrentDomain.GetAssemblies();
        return inputAssemblies.SelectMany(GetAllTypesImplementingInterface<TInterface>);
    }

    private static IEnumerable<Type> GetAllTypesImplementingInterface<TInterface>(Assembly? assembly = null)
    {
        var inputAssembly = assembly ?? Assembly.GetExecutingAssembly();
        return inputAssembly
            .GetTypes()
            .Where(type =>
                typeof(TInterface).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && type.IsClass
            );
    }

    public static IEnumerable<string?> GetPropertyNames<T>(params Expression<Func<T, object>>[] propertyExpressions)
    {
        var retVal = new List<string?>();
        foreach (var propertyExpression in propertyExpressions)
        {
            retVal.Add(GetPropertyName(propertyExpression));
        }

        return retVal;
    }

    public static string? GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
    {
        string? retVal = null;
        if (propertyExpression != null)
        {
            var lambda = (LambdaExpression)propertyExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            retVal = memberExpression.Member.Name;
        }

        return retVal;
    }

    /// <summary>
    /// Get All referenced assemblies of root assembly(EntryAssembly if not provide) and loading them explicitly because assemblies will load lazily based on their dependent assembly code use in dependency graph (it is possible to get ReflectionTypeLoadException, because some dependent type assembly are lazy and not loaded yet).
    /// Ref: https://stackoverflow.com/a/10253634/581476, https://www.davidguida.net/how-to-find-all-application-assemblies, https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
    /// </summary>
    /// <param name="rootAssembly"></param>
    /// <returns></returns>
    public static IReadOnlyList<Assembly> GetReferencedAssemblies(Assembly? rootAssembly)
    {
        var visited = new HashSet<string>();
        var queue = new Queue<Assembly?>();
        var listResult = new List<Assembly>();

        var root = rootAssembly ?? Assembly.GetEntryAssembly();
        queue.Enqueue(root);

        while (queue.Any())
        {
            var asm = queue.Dequeue();

            if (asm == null)
                break;

            listResult.Add(asm);

            foreach (var reference in asm.GetReferencedAssemblies())
            {
                if (!visited.Contains(reference.FullName))
                {
                    // `Load` will add assembly into the `application domain` of the caller. loading assemblies explicitly to AppDomain, because assemblies are loaded lazily
                    // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.load
                    queue.Enqueue(Assembly.Load(reference));
                    visited.Add(reference.FullName);
                }
            }
        }

        return listResult.Distinct().ToList().AsReadOnly();
    }

    public static Type? GetTypeFromAnyReferencingAssembly(string typeName)
    {
        var referencedAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Select(a => a.FullName);

        if (referencedAssemblies == null)
            return null;

        return AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => referencedAssemblies.Contains(a.FullName))
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))
            .FirstOrDefault();
    }

    public static Type? GetFirstMatchingTypeFromCurrentDomainAssemblies(string typeName)
    {
        return AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))
            .FirstOrDefault();
    }

    public static Type? GetFirstMatchingTypeFromAssembly(string typeName, Assembly assembly)
    {
        return assembly.GetTypes().FirstOrDefault(x => x.FullName == typeName || x.Name == typeName);
    }

    public static IReadOnlyList<Assembly> GetApplicationPartAssemblies(Assembly rootAssembly)
    {
        var rootNamespace = rootAssembly.GetName().Name!.Split('.').First();
        var list = rootAssembly!
            .GetCustomAttributes<ApplicationPartAttribute>()
            .Where(x => x.AssemblyName.StartsWith(rootNamespace, StringComparison.InvariantCulture))
            .Select(name => Assembly.Load(name.AssemblyName))
            .Distinct();

        return list.ToList().AsReadOnly();
    }
}
