namespace Demo.DbRefreshManager.Services.Infrastructure.Constants;

/// <summary>
/// Названия атрибутов LDAP.
/// </summary>
public static class LdapAttributes
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public const string Login = "sAMAccountName";

    /// <summary>
    /// Адрес Email.
    /// </summary>
    public const string Email = "mail";

    /// <summary>
    /// Имя.
    /// </summary>
    public const string FirstName = "givenName";

    /// <summary>
    /// Фамилия.
    /// </summary>
    public const string LastName = "sn";

    /// <summary>
    /// ФИО/название.
    /// </summary>
    public const string FullName = "name";

    /// <summary>
    /// Должность.
    /// </summary>
    public const string Position = "title";

    /// <summary>
    /// Отдел.
    /// </summary>
    public const string Department = "department";

    /// <summary>
    /// Название компании.
    /// </summary>
    public const string Company = "company";

    /// <summary>
    /// Дата последнего изменения.
    /// </summary>
    public const string WhenChanged = "whenChanged";

    /// <summary>
    /// Список групп.
    /// </summary>
    public const string GroupsList = "memberOf";
}
