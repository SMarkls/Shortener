# Shortener - сокращатель ссылок.
Проект состоит из двух частей - [Frontend](https://github.com/SMarkls/Shortener/tree/master/app) и [Backend](https://github.com/SMarkls/Shortener/tree/master/LinkShortener).
## Руководство
Для использования сокращателя ссылок необходимо зарегистрироваться. После этого на главной форме нужно ввести ссылку, которую вы хотите сократить. Затем в таблице появится запись о сокращенной ссылке. Поле токен - и есть короткая ссылка,
которую вы можете отправить кому угодно.
## Frontend
Приложение на `Vue.js`. Состоит из двух страниц:
* [Форма авторизации](https://github.com/SMarkls/Shortener/blob/master/app/src/views/AuthForm.vue)
* [Главная страница](https://github.com/SMarkls/Shortener/blob/master/app/src/views/MainView.vue)
### Форма авторизации
Представляет собой обычную форму для авторизаци. Запрашивает ввод логина и пароля при авторизации. При регистрации появляется третье поле - подтверждение пароля. При подтверждении ввода данные с формы валидируются с помощью `Vuelidate`.
После успешной валидации данные отправляются на Backend в API.
![image](https://github.com/SMarkls/Shortener/assets/91720469/ef17ba90-b4af-45a8-9e0a-02c76c41b9f1)
### Главная страница
Состоит из двух компонентов - формы для ввода ссылки на сокращение и таблицы с сокращенными ссылками текущего пользователя.
![image](https://github.com/SMarkls/Shortener/assets/91720469/a72c7c43-039b-4f1e-bdb7-03434f537498)
## Backend
`ASP.NET Core` приложение, с архитектурным прицнипом `CQRS` и разделением приложения по слоям:
* [Application](https://github.com/SMarkls/Shortener/tree/master/LinkShortener/LinkShortener.Application)
* [Domian](https://github.com/SMarkls/Shortener/tree/master/LinkShortener/LinkShortener.Domain)
* [Infrastructure](https://github.com/SMarkls/Shortener/tree/master/LinkShortener/LinkShortener.Infrastructure)
* [Api](https://github.com/SMarkls/Shortener/tree/master/LinkShortener/LinkShortener.Api)
### Аутентификация
Аутентификация в проекте реализована посредством создания `JSON Web Token (JWT)`. Реализация аутентификации в основном приходится на [IdentityTokenService](https://github.com/SMarkls/Shortener/blob/master/LinkShortener/LinkShortener.Infrastructure/Services/IdentityTokenService.cs)
