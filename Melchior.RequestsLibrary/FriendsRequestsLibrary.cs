using System;
using System.Collections.Generic;
using Melchior.Models;
using Melchior.Data.Common;
using Melchior.Common;
using Melchior.Request.Xml;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Методы для работы со списком друзей
    /// author wrmax
    /// </summary>
    public class FriendsRequestsLibrary : BaseRequestsLibrary
    {
        public FriendsRequestsLibrary(string accessToken)
            : base(accessToken)
        {
        }

        #region Внутренние методы
        internal VKRequestCommand<RequestCollection> CreateRequestCollectionRequestCommand(string methodName)
        {
            return new VKRequestCommand<RequestCollection>(AccessToken, methodName, (data) => { return new RequestCollection(data); });
        }
        #endregion

        /// <summary>
        /// Возвращает список друзей пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, для которого необходимо получить список друзей. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: uid, first_name, last_name, nickname, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, domain, has_mobile, rate, contacts, education.</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom.</param>
        /// <param name="count">Количество друзей, которое нужно вернуть. (по умолчанию – все друзья)</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества друзей.</param>
        /// <param name="listId">Идентификатор списка друзей, полученный методом friends.getLists, друзей из которого необходимо получить. Данный параметр учитывается, только когда параметр uid равен идентификатору текущего пользователя. Данный параметр доступен только для Desktop-приложений.</param>
        /// <param name="order">Порядок в котором нужно вернуть список друзей. Допустимые значения: name - сортировать по имени (работает только при переданном параметре fields). hints - сортировать по рейтингу, аналогично тому, как друзья сортируются в разделе Моя друзья (данный параметр доступен только для Desktop-приложений).</param>
        public VKRequestCommand<UserCollection> CreateGetFriendsCommand(string userId, string[] fields, string nameCase, int count, int offset, string listId, string order)
        {
            const string methodName = "friends.get";
            var command = CreateUserCollectionRequestCommand(methodName);
            if (userId != null && !String.IsNullOrEmpty(userId)) command.Parameters.Add("uid", userId);
            if (fields != null && fields.Length > 0) command.Parameters.Add("fields", StringUtils.Join(fields, Chars.Comma));
            if (nameCase != null && !String.IsNullOrEmpty(nameCase)) command.Parameters.Add("name_case", nameCase);
            if (count > 0) command.Parameters.Add("count", count);
            if (offset > 0) command.Parameters.Add("offset", offset);
            if (listId != null && !String.IsNullOrEmpty(listId)) command.Parameters.Add("lid", listId);
            if (order != null && !String.IsNullOrEmpty(order)) command.Parameters.Add("order", order);

            return command;
        }
        public VKRequestCommand<UserCollection> CreateGetFriendsCommand(string userId, string[] fields)
        {
            return CreateGetFriendsCommand(userId, fields, null, -1, -1, null, "hints");
        }
        /// <summary>
        /// Отправляет заявку в друзья или одобряет входящую заявку на добавление.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя которому необходимо отправить заявку, либо заявку от которого необходимо одобрить.</param>
        /// <param name="text">Текст сопроводительного сообщения для заявки на добавление в друзья. Максимальная длина сообщения - 500 символов.</param>
        public VKRequestCommand<bool> CreateAddFriendCommand(string userId, string text)
        {
            if (userId == null || String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("uid");
            }
            const string methodName = "friends.add";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("uid", userId);
            if (text != null && !String.IsNullOrEmpty(text)) command.Parameters.Add("text", text);
            return command;
        }
        public VKRequestCommand<bool> CreateAddFriendCommand(string userId)
        {
            return CreateAddFriendCommand(userId, null);
        }
        /// <summary>
        /// Удаляет пользователя из друзей или отклоняет заявку на добавление.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которого необходимо удалить из списка друзей, либо заявку от которого необходимо отклонить.</param>
        public VKRequestCommand<bool> CreateDeleteFriendCommand(string userId)
        {
            if (userId == null || String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }
            const string methodName = "friends.delete";
            var command = CreateBooleanRequestCommand(methodName);
            command.Parameters.Add("uid", userId);
            return command;
        }
        /// <summary>
        /// Возвращает список заявок в друзья у текущего пользователя.
        /// </summary>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества заявок на добавление в друзья.</param>
        /// <param name="count">Максимальное количество заявок на добавление в друзья, которые необходимо получить (не более 1000). Если параметр не задан, то считается, что он равен 100.</param>
        /// <param name="needMessages">Определяет требуется ли возвращать в ответе сообщения от пользователей, подавших заявку на добавление в друзья.</param>
        /// <param name="needMutual">Определяет требуется ли возвращать в ответе список общих друзей, если они есть. Обратите внимание, что при использовании need_mutual будет возвращено не более 20 заявок.</param>
        /// <param name="outSign">0 - возвращать полученные заявки в друзья (по умолчанию), 1 - возвращать отправленные пользователем заявки.</param>
        public VKRequestCommand<RequestCollection> CreateGetRequestsCommand(int offset, int count, bool needMessages, bool needMutual, bool outSign)
        {
            const string methodName = "friends.getRequests";
            var command = CreateRequestCollectionRequestCommand(methodName);
            if (offset > 0) command.Parameters.Add("offset", offset);
            if (count > 0) command.Parameters.Add("count", count);
            command.Parameters.Add("need_messages", needMessages ? 1 : 0);
            command.Parameters.Add("need_mutual", needMutual ? 1 : 0);
            command.Parameters.Add("out", outSign ? 1 : 0);
            return command;
        }
        public VKRequestCommand<RequestCollection> CreateGetRequestsCommand()
        {
            return CreateGetRequestsCommand(-1, -1, false, false, false);
        }
        /// <summary>
        /// Отклоняет все заявки на добавление в друзья.
        /// </summary>
        public VKRequestCommand<bool> CreateDeleteAllRequestsCommand()
        {
            const string methodName = "friends.deleteAllRequests";
            var command = CreateBooleanRequestCommand(methodName);
            return command;
        }
        /// <summary>
        /// Возвращает список рекомендаций людей на добавление в друзья текущему пользователю.
        /// </summary>
        /// <param name="filter">Типы предрагаемых друзей которые нужно вернуть, перечисленные через запятую.
        /// Параметр может принимать следующие значения:
        /// mutual - пользователи, с которыми много общих друзей,
        /// contacts - пользователи найденные благодаря методу account.importContacts.
        /// mutual_contacts - пользователи, которые импортировали те же контакты что и текущий пользователь, используя метод account.importContacts.
        /// </param>
        /// <returns>По умолчанию будут возвращены все возможные друзья.</returns>
        public VKRequestCommand<UserCollection> CreateGetSuggestionsCommand(string filter)
        {
            const string methodName = "friends.getSuggestions";
            var command = CreateUserCollectionRequestCommand(methodName);
            if (filter != null && !String.IsNullOrEmpty(filter)) command.Parameters.Add("filter", filter);
            return command;
        }

        public VKRequestCommand<UserCollection> CreateGetSuggestionsCommand()
        {
            return CreateGetSuggestionsCommand(null);
        }
        /// <summary>
        /// Возвращает друзей пользователя по их номерам телефонов.
        /// </summary>
        /// <param name="phones">Список телефонных номеров в формате MSISDN разделеннных запятыми. Максимальное количество номеров в списке - 1000.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Список доступных полей указан на странице Описание полей параметра fields.</param>
        public VKRequestCommand<UserCollection> CreateGetFriendsByPhonesCommand(List<string> phones, string[] fields)
        {
            if (phones == null || phones.Count == 0)
            {
                throw new ArgumentNullException("phones");
            }
            const string methodName = "friends.getByPhones";
            var command = CreateUserCollectionRequestCommand(methodName);
            command.Parameters.Add("phones", StringUtils.Join(phones, Chars.Comma));
            if (fields != null && fields.Length > 0) command.Parameters.Add("fields", StringUtils.Join(fields, Chars.Comma));
            return command;
        }
    }
}