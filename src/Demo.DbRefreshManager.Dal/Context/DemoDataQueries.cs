namespace Demo.DbRefreshManager.Dal.Context;

/// <summary>
/// Запросы заполнения демонстрационными данными.
/// </summary>
internal static class DemoDataQueries
{
    /// <summary>
    /// Группы БД.
    /// </summary>
    public const string DbGroups = @"
        INSERT INTO db_groups
        (id, sort_order, css_color, description)
        VALUES
        (1, 1000, 'ghostwhite', 'Не назначено'),
        (2, 10, 'beige', 'TST1 - Тестирование (NTLM)'),
        (3, 20, 'beige', 'TST2 - Тестирование (БТ)'),
        (4, 30, 'beige', 'TST3 - Тестирование (МП)'),
        (5, 40, 'beige', 'TST4-12 - Тестирование'),
        (6, 50, 'floralwhite', 'TST21-23 - Тимсити'),
        (7, 60, 'bisque', 'TST20 - Разработка (3я линия)'),
        (8, 70, 'lemonchiffon', 'TST24-39 - Разработка'),
        (9, 80, 'lavender', 'TST16-18,46 - Поддержка'),
        (10, 90, 'powderblue', 'TST19 - Внедрение'),
        (11, 100, 'lightskyblue', 'TST15,40,41,42 - Аналитики'),
        (12, 110, 'lemonchiffon', 'TST43,47 - Резерв'),
        (13, 120, '#FAC0F6', 'TST13,14,49,50 - FLORA'),
        (14, 130, 'azure', 'TST51-54 - ЭДО'),
        (15, 105, 'mistyrose', 'TST44,45 - WMS.EME');
    ";

    /// <summary>
    /// Задачи на перезаливку БД.
    /// </summary>
    public const string DbRefreshJobs = @"
        INSERT INTO db_refresh_jobs
        (id, db_name, db_group_id, schedule_refresh_time, ssh_script)
        VALUES
        (1, 'TST1', 2, '05:00', 'sleep 10'),
        (2, 'TST2', 3, '05:10', 'sleep 10'),
        (3, 'TST3', 4, '05:15', 'sleep 10'),
        (4, 'TST4', 5, '05:20', 'sleep 10'),
        (5, 'TST5', 5, '05:25', 'sleep 10'),
        (6, 'TST6', 5, '05:30', 'sleep 10'),
        (7, 'TST7', 5, '05:35', 'sleep 10'),
        (8, 'TST8', 5, '05:40', 'sleep 10'),
        (9, 'TST9', 5, '05:45', 'sleep 10'),
        (10, 'TST10', 5, '05:50', 'sleep 10'),
        (11, 'TST11', 5, '05:55', 'sleep 10'),
        (12, 'TST12', 5, '06:00', 'sleep 10'),
        (13, 'TST13', 13, '06:05', 'sleep 10'),
        (14, 'TST14', 13, '06:10', 'sleep 10'),
        (15, 'TST15', 11, '06:20', 'sleep 10'),
        (16, 'TST16', 9, '06:25', 'sleep 10'),
        (17, 'TST17', 9, '06:30', 'sleep 10'),
        (18, 'TST18', 9, '06:35', 'sleep 10'),
        (19, 'TST19', 10, '06:40', 'sleep 10'),
        (20, 'TST20', 7, '06:50', 'sleep 10'),
        (21, 'TST21', 6, '05:05', 'sleep 10'),
        (22, 'TST22', 6, '05:10', 'sleep 10'),
        (23, 'TST23', 6, '05:15', 'sleep 10'),
        (24, 'TST24', 8, '05:20', 'sleep 10'),
        (25, 'TST25', 8, '05:25', 'sleep 10'),
        (26, 'TST26', 8, '05:30', 'sleep 10'),
        (27, 'TST27', 8, '05:35', 'sleep 10'),
        (28, 'TST28', 8, '05:40', 'sleep 10'),
        (29, 'TST29', 8, '05:45', 'sleep 10'),
        (30, 'TST30', 8, '05:50', 'sleep 10'),
        (31, 'TST31', 8, '05:55', 'sleep 10'),
        (32, 'TST32', 8, '06:00', 'sleep 10'),
        (33, 'TST33', 8, '07:00', 'sleep 10'),
        (34, 'TST34', 8, '07:00', 'sleep 10'),
        (35, 'TST35', 8, '07:10', 'sleep 10'),
        (36, 'TST36', 8, '07:20', 'sleep 10'),
        (37, 'TST37', 8, '07:30', 'sleep 10'),
        (38, 'TST38', 8, '07:40', 'sleep 10'),
        (39, 'TST39', 8, '07:50', 'sleep 10'),
        (40, 'TST40', 11, '09:10', 'sleep 10'),
        (41, 'TST41', 11, '08:00', 'sleep 10'),
        (42, 'TST42', 11, '08:10', 'sleep 10'),
        (43, 'TST43', 12, '08:20', 'sleep 10'),
        (44, 'TST44', 15, '08:30', 'sleep 10'),
        (45, 'TST45', 15, '08:40', 'sleep 10'),
        (46, 'TST46', 9, '08:50', 'sleep 10'),
        (47, 'TST47', 12, '09:00', 'sleep 10'),
        (48, 'TST48', 1, '09:20', 'sleep 10'),
        (49, 'TST49', 13, '09:30', 'sleep 10'),
        (50, 'TST50', 13, '08:00', 'sleep 10'),
        (51, 'TST51', 14, '10:00', 'sleep 10'),
        (52, 'TST52', 14, '10:10', 'sleep 10'),
        (53, 'TST53', 14, '10:20', 'sleep 10'),
        (54, 'TST54', 14, '10:30', 'sleep 10');
    ";

    /// <summary>
    /// Роли пользователей.
    /// </summary>
    public const string UserRoles = @"
        INSERT INTO roles
        (id, ""name"", ldap_group, description)
        VALUES
        (1, 'MASTER', 'RefreshManagerMaster', 'Мастер-роль для всех доступов'),
        (2, 'DEVELOPER', 'RefreshManagerDev', 'Разработчик'),
        (3, 'QA', 'RefreshManagerQA', 'Тестировщик'),
        (4, 'SUPPORT', 'RefreshManagerSupport', 'Тех. поддержка'),
        (5, 'ANALYST', 'RefreshManagerAnalyst', 'Аналитик'),
        (6, 'INTEGRATE', 'RefreshManagerIntegrate', 'Группа внедрения'),
        (7, 'FLORA', 'RefreshManagerFlora', 'Доступ к БД Flora'),
        (8, 'EDO', 'RefreshManagerEDO', 'Доступ к БД ЭДО');
    ";

    /// <summary>
    /// Связка групп с ролями.
    /// </summary>
    public const string GroupsRoles = @"
        INSERT INTO db_groups_roles 
        (role_id, db_group_id)
        VALUES
        -- MASTER
        (1, 1),
        (1, 2),
        (1, 3),
        (1, 4),
        (1, 5),
        (1, 6),
        (1, 7),
        (1, 8),
        (1, 9),
        (1, 10),
        (1, 11),
        (1, 12),
        (1, 13),
        (1, 14),
        (1, 15),
        -- DEVELOPER
        (2, 6),
        (2, 7),
        (2, 8),
        (2, 12),
        (2, 13),
        (2, 15),
        -- QA
        (3, 2),
        (3, 3),
        (3, 4),
        (3, 5),
        (3, 6),
        (3, 10),
        (3, 11),
        (3, 12),
        (3, 13),
        (3, 15),
        -- SUPPORT
        (4, 9),
        (4, 13),
        -- ANALYST
        (5, 11),
        -- INTEGRATE
        (6, 10),
        -- FLORA
        (7, 13),
        -- EDO
        (8, 14);
    ";
}
