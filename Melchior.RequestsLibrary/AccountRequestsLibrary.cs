using System;
using Melchior.Data.Common;
using Melchior.Common;
using Melchior.Request.Xml;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Дополнительные методы
    /// author wrmax
    /// </summary>
    public class AccountRequestsLibrary : BaseRequestsLibrary
    {
        public AccountRequestsLibrary(string accessToken)
            : base(accessToken)
        {

        }
        /// <summary>
        /// Принимает список контактов пользователя для поиска зарегистрированных ВКонтакте пользователей методом Friends.GetSuggestions.		
        /// </summary>
        /// <param name="contacts">список телефонов или email адресов друзей пользователя, указанных через запятую.</param>
        /// <returns>В случае успешного завершения будет возвращено true, в противном случае false</returns>
        public VKRequestCommand<bool> CreateImportContactsCommand(string contacts)
        {
            const string methodName = "account.importContacts";
            var command = CreateBooleanRequestCommand(methodName);
            if (contacts != null && !String.IsNullOrEmpty(contacts)) command.Parameters.Add("contacts", contacts);
            return command;
        }
        public VKRequestCommand<bool> CreateImportContactsCommand()
        {
            return CreateImportContactsCommand(null);
        }
        /// <summary>
        /// Подписывает устройство на Push уведомления.
        /// Для выполнения данного метода необходимо включить Push уведомления в настройках приложения.    
        /// </summary>
        /// <param name="token">Идентификатор устройства, используемый для отправки уведомлений.</param>
        /// <param name="deviceModel">Строковое название модели устройства.</param>
        /// <param name="systemVersion">Строковая версия операционной системы устройства.</param>
        /// <param name="noText">1 - Не передавать текст сообщения в push уведомлении. 0 - (по умолчанию) текст сообщения передаётся.</param>
        /// <returns>В случае успешного завершения будет возвращено true, в противном случае false</returns>
        public VKRequestCommand<bool> CreateRegisterDeviceCommand(string token, string deviceModel, string systemVersion, bool noText)
        {
            if (token == null || String.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }
            const string methodName = "account.registerDevice";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("token", token);
            if (deviceModel != null && !String.IsNullOrEmpty(deviceModel)) command.Parameters.Add("device_model", deviceModel);
            if (systemVersion != null && !String.IsNullOrEmpty(systemVersion)) command.Parameters.Add("system_version", systemVersion);
            command.Parameters.Add("no_text", noText ? 1 : 0);
            return command;
        }
        public VKRequestCommand<bool> CreateRegisterDeviceCommand(string token)
        {
            return CreateRegisterDeviceCommand(token, null, null, false);
        }
        /// <summary>
        /// Отписывает устройство от Push уведомлений
        /// </summary>
        /// <param name="token">Идентификатор устройства, используемый для отправки уведомлений.</param>
        /// <returns>В случае успешного завершения будет возвращено true, в противном случае false</returns>
        public VKRequestCommand<bool> CreateUnRegisterDeviceCommand(string token)
        {
            if (token == null || String.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }
            const string methodName = "account.unregisterDevice";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("token", token);
            return command;
        }
        /// <summary>
        /// Отключает звук в параметрах, отправляемых push уведомлений на заданный промежуток времени.
        /// </summary>
        /// <param name="token">Идентификатор устройства, использованный в методе registerDevice.</param>
        /// <param name="time">Количество секунд, в течение которых уведомления будут приходить без звука.</param>
        /// <returns>В случае успешного завершения будет возвращено true, в противном случае false</returns>
        public VKRequestCommand<bool> CreateSetSilenceModeCommand(string token, int time)
        {
            const string methodName = "account.setSilenceMode";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("token", token);
            command.Parameters.Add("time", time);
            return command;
        }
    }
}