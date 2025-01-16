[[_TOC_]]

## Назначение

Демонстрационный проект пользовательского интерфейса планировщика перезаливок (разворачивания из бэкапа) тестовых БД.
Проект состоит из .NET REST + GraphQL API и веб интерфейса пользователя на Typescript + React.

## Основные технологии

### Backend

- **.NET 9.0** - основная платформа разработки;
- **SQLite** - тип БД;
- **HotChocolate** - набор библиотек для бэкенда GraphQL ([документация](https://chillicream.com/docs/hotchocolate));
- **EntityFrameworkCore** - ORM для работы с БД;
- **Quartz.net** - библиотека для планировки запуска задач по расписанию ([документация](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html));
- **Swagger** - автодокументация REST API;
- **Serilog** - библиотека логирования ([документация](https://serilog.net/)).

### Frontend

- **Typescript** - язык программирования;
- **NodeJS (v20+)** - среда выполнения js/ts ([установка](https://nodejs.org/ru));
- **Yarn** - менеджер библиотек js ([документация](https://yarnpkg.com/getting-started/install));
- **Craco** - Надстройка над create-react-app для упрощения конфигурации сборки, доп. плагинов ([документация](https://craco.js.org/), [плагины](https://craco.js.org/plugins/));
- **React** - библиотека разработки SPA приложения ([материал по теме](https://metanit.com/web/react/));
- **Jotai** - библиотека для работы с хранилищем глобального состояния ([документация](https://jotai.org/docs/introduction));
- **Mui** - библиотека компонентов интерфейса ([документация](https://mui.com/material-ui/react-button/));
- **Mui DataGrid** - библиотека компонента грида ([документация](https://mui.com/x/react-data-grid/));

Полный список frontend библиотек в файле [package.json](/src/Demo.DbRefreshManager.WebUI/package.json) проекта [WebUI](/src/Demo.DbRefreshManager.WebUI).

## Запуск преокта

Проект состоит из 2 основных частей - WebApi (backend api) и WebUI (приложение frontend), возможен запуск бэкенда для дебага без frontend.

### Требования для запуска backend

- Установить [Microsoft Visual Studio](https://visualstudio.microsoft.com/ru/downloads/) 2022+ версии, указав зависимости связанные с разработкой веб приложений ASP.NET, Nodejs.

### Требования для запуска frontend

- Установить [NodeJS 20+ версии](https://nodejs.org/ru), проверка установки - команда node --version в консоли;
- Установить yarn стабильной версии - в командной строке выполнить команды:
	- corepack enable
	- yarn set version stable
- В командной строке выполнить команду yarn в папке проекта [WebUI](/src/Demo.DbRefreshManager.WebUI) для загрузки библиотек.

### Запуск backend без frontend

- Открыть в visual studio файл [Demo.DbRefreshManager.sln](/src/Demo.DbRefreshManager.sln);
- Установить параметр launchBrowser: true в [launchSettings.json](/src/Demo.DbRefreshManager.WebApi/Properties/launchSettings.json);
- В параметрах запуска указать single startup project - WebAPI и запустить проект (F5).

![configure_startup](/docs/img/configure_startup.png)
![web_api_startup](/docs/img/web_api_startup.png)

### Запуск backend + frontend

- Открыть в visual studio файл [Demo.DbRefreshManager.sln](/src/Demo.DbRefreshManager.sln);
- (Опционально) Установить параметр launchBrowser: false в [launchSettings.json](/src/Demo.DbRefreshManager.WebApi/Properties/launchSettings.json);
- В параметрах запуска указать multiple startup project, выставить Action Start для WebAPI, WebUI и запустить проект (F5).

![configure_startup](/docs/img/configure_startup.png)
![web_api_startup](/docs/img/web_ui_startup.png)