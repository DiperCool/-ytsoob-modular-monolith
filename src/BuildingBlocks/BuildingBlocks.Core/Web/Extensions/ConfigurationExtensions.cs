using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Core.Web.Extensions;

/// <summary>
/// Static helper class for <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Attempts to bind the <typeparamref name="TOptions"/> instance to configuration section values.
    /// </summary>
    /// <typeparam name="TOptions">The given bind model.</typeparam>
    /// <param name="configuration">The configuration instance to bind.</param>
    /// <param name="section">The configuration section</param>
    /// <returns>The new instance of <typeparamref name="TOptions"/>.</returns>
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration, string section)
        where TOptions : new()
    {
        // note: with using Get<>() if there is no configuration in appsettings it just returns default value (null) for the configuration type
        // but if we use Bind() we can pass a instantiated type with its default value (for example in its property initialization) to bind method for binding configurations from appsettings
        // https://www.twilio.com/blog/provide-default-configuration-to-dotnet-applications
        var options = new TOptions();

        var optionsSection = configuration.GetSection(section);
        optionsSection.Bind(options);

        return options;
    }

    /// <summary>
    /// Attempts to bind the <typeparamref name="TOptions"/> instance to configuration section values.
    /// </summary>
    /// <typeparam name="TOptions">The given bind model.</typeparam>
    /// <param name="configuration">The configuration instance to bind.</param>
    /// <returns>The new instance of <typeparamref name="TOptions"/>.</returns>
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration)
        where TOptions : new()
    {
        return BindOptions<TOptions>(configuration, typeof(TOptions).Name);
    }

    public static IServiceCollection AddValidatedOptions<T>(this IServiceCollection services, string key)
        where T : class
    {
        return AddValidatedOptions<T>(services, key, RequiredConfigurationValidator.Validate);
    }

    public static IServiceCollection AddValidatedOptions<T>(
        this IServiceCollection services,
        string key,
        Func<T, bool> validator
    )
        where T : class
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options
        // https://thecodeblogger.com/2021/04/21/options-pattern-in-net-ioptions-ioptionssnapshot-ioptionsmonitor/
        // https://code-maze.com/aspnet-configuration-options/
        // https://code-maze.com/aspnet-configuration-options-validation/
        // https://dotnetdocs.ir/Post/42/difference-between-ioptions-ioptionssnapshot-and-ioptionsmonitor
        // https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-dotnet-6/
        services.AddOptions<T>().BindConfiguration(key).Validate(validator);

        // IOptions itself registered as singleton
        return services.AddSingleton(x => x.GetRequiredService<IOptions<T>>().Value);
    }
}

public static class RequiredConfigurationValidator
{
    public static bool Validate<T>(T arg)
        where T : class
    {
        var requiredProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => Attribute.IsDefined(x, typeof(RequiredMemberAttribute)));

        foreach (var requiredProperty in requiredProperties)
        {
            var propertyValue = requiredProperty.GetValue(arg);
            if (propertyValue is null)
            {
                throw new System.Exception($"Required property '{requiredProperty.Name}' was null");
            }
        }

        return true;
    }
}
