using Demo.DbRefreshManager.Common.Enums;

namespace Demo.DbRefreshManager.Common.Converters.Abstract;

/// <summary>
/// Сериализатор/десериализатор json.
/// </summary>
public interface IJsonConverter
{
    /// <summary>
    /// Сериализовать объект в json.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="textStyle">Стиль текста JSON.</param>
    /// <param name="obj">Объект.</param>
    string Serialize<T>(T obj, TextStyle textStyle = TextStyle.CamelCase);

    /// <summary>
    /// Десериализовать json в объект.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="textStyle">Стиль текста JSON.</param>
    /// <param name="json">json</param>
    T? Deserialize<T>(string json, TextStyle textStyle = TextStyle.None);
}
