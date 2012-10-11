using System;
using Melchior.Data.Common;
using Melchior.Common;
using Melchior.Request.Xml;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Регистрация пользователей
    /// author wrmax
    /// </summary>
    public class AuthRequestsLibrary : BaseAuthRequestsLibrary
    {
        public AuthRequestsLibrary(string clientId, string clientSecret)
            : base(clientId, clientSecret)
        {
        }
        /// <summary>
        /// Регистрирует нового пользователя по номеру телефона
        /// В случае успешного выполнения метода на номер телефона, указанный пользователем, будет отправлено SMS со специальным кодом,
        /// который может быть использован для завершения регистрации методом confirm. 
        /// 
        /// Обратите внимание, что в процессе выполнения запроса к данному методу, несмотря на правильность ввода всех данных,
        /// может быть возвращена ошибка с кодом 1112. 
        /// Данная ошибка означает, что введённый номер анализируется, 
        /// поэтому следует повторить вызов данного метода с теми же параметрами через некоторое время (приблизительно 5 сек).
        /// </summary>
        /// <param name="phone">Номер телефона регистрируемого пользователя. Номер телефона может быть проверен заранее методом auth.checkPhone.</param>
        /// <param name="firstName">Имя пользователя.</param>
        /// <param name="lastName">Фамилия пользователя.</param>
        /// <param name="sex">Пол пользователя: 1 - Женский, 2 - Мужской.</param>
        /// <param name="password">Пароль пользователя, который будет использоваться при входе. Не меньше 6 символов. Также пароль может быть указан позже, при вызове метода auth.confirm.</param>
        /// <param name="voice">1 - в случае если вместо SMS необходимо позвонить на указанный номер и продиктовать код голосом. 0 - (по умолчанию) необходимо отправить SMS.</param>
        /// <param name="sid">Идентификатор сессии, необходимый при повторном вызове метода, в случае если SMS сообщение доставлено не было.</param>
        /// <param name="testMode">1 - тестовый режим, при котором не будет зарегистрирован новый пользователь, но при этом номер не будет проверяться на использованность. 0 - (по умолчанию) рабочий.</param>
        /// <returns>В качестве ответа будет возвращено поле sid, необходимое для повторного вызова метода, в случае если SMS-сообщение не дошло.</returns>
        public VKRequestCommand<string> CreateSignUpCommand(string phone, string firstName, string lastName, int sex, string password, bool voice, string sid, bool testMode)
        {
            if (phone == null || String.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("phone");
            }
            if (firstName == null || String.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("firstName");
            }
            if (lastName == null || String.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("lastName");
            }
            if (sex != 1 && sex != 2)
            {
                throw new ArgumentNullException("Sex. Пол пользователя: 1 - Женский, 2 - Мужской.");
            }
            if (password != null && !String.IsNullOrEmpty(password) && password.Length < 6)
            {
                throw new ArgumentNullException("Пароль должен иметь длину не менее 6 символов");
            }
            const string methodName = "auth.signup";
            var command = CreateStringRequestCommand(methodName);
            command.Parameters.Add("phone", phone);
            command.Parameters.Add("first_name", firstName);
            command.Parameters.Add("last_name", lastName);
            command.Parameters.Add("sex", sex);
            if (password != null && !String.IsNullOrEmpty(password)) command.Parameters.Add("password", password);
            command.Parameters.Add("voice", voice ? 1 : 0);
            if (sid != null && !String.IsNullOrEmpty(sid)) command.Parameters.Add("sid", sid);
            command.Parameters.Add("test_mode", testMode ? 1 : 0);
            return command;
        }
        public VKRequestCommand<string> CreateSignUpCommand(string phone, string firstName, string lastName)
        {
            return CreateSignUpCommand(phone, firstName, lastName, 1, null, false, null, false);
        }
        /// <summary>
        /// Завершает регистрацию нового пользователя по коду, полученному через SMS
        /// </summary>
        /// <param name="phone">Номер телефона регистрируемого пользователя. Номер телефона может быть проверен заранее методом checkPhone</param>
        /// <param name="code">Код, полученный через SMS в результате выполнения метода signup.</param>
        /// <param name="password">Пароль пользователя, который будет использоваться при входе. Не меньше 6 символов. Также пароль может быть указан позже, при вызове метода signup.</param>
        /// <param name="testMode">1 - тестовый режим, при котором не будет зарегистрирован новый пользователь, но при этом номер не будет проверяться на использованность. 0 - (по умолчанию) рабочий.</param>
        /// <returns>Возвращает идентификатор зарегистрированного пользователя</returns>
        public VKRequestCommand<string> CreateConfirmCommand(string phone, string code, string password, bool testMode)
        {
            if (phone == null || String.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("phone");
            }
            if (code == null || String.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            if (password != null && !String.IsNullOrEmpty(password) && password.Length < 6)
            {
                throw new ArgumentNullException("Пароль должен иметь длину не менее 6 символов");
            }
            const string methodName = "sigconfirmnup";
            var command = CreateStringRequestCommand(methodName);
            command.Parameters.Add("phone", phone);
            command.Parameters.Add("code", code);
            if (password != null && !String.IsNullOrEmpty(password)) command.Parameters.Add("password", password);
            command.Parameters.Add("test_mode", testMode ? 1 : 0);
            return command;
        }
        public VKRequestCommand<string> CreateConfirmCommand(string phone, string code)
        {
            return CreateConfirmCommand(phone, code, null, false);
        }
        /// <summary>
        /// Проверяет корректность введённого номера
        /// 
        /// Обратите внимание, что в процессе выполнения запроса к данному методу, 
        /// несмотря на правильность ввода всех данных может быть возвращена ошибка с кодом 1112. 
        /// Данная ошибка означает, что введённый номер анализируется, 
        /// поэтому следует повторить вызов данного метода с теми же параметрами через некоторое время (приблизительно 5 сек), 
        /// либо через это время вызвать метод signup.
        /// </summary>
        /// <param name="phone">Номер телефона пользователя.</param>
        /// <param name="code">Код, полученный через SMS в результате выполнения метода signup.</param>
        /// <returns>В случае успешного завершения будет возвращено true, в противном случае false</returns>
        public VKRequestCommand<bool> CreateCheckPhoneCommand(string phone)
        {
            if (phone == null || String.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("phone");
            }
            const string methodName = "вcheckPhone";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("phone", phone);
            return command;
        }
    }
}