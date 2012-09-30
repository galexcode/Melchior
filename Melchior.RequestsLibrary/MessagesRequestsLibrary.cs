using System;
using Melchior.Models;
using Melchior.Data.Common;
using Melchior.Common;
using Melchior.Request.Xml;

namespace Melchior.RequestsLibrary
{
	/// <summary>
	/// Получение сообщений.
	/// Отправка/удаление сообщений.
	/// Работа с беседами.
	/// Активность пользователя.
	/// Работа с LongPoll-сервером
	/// author wrmax
	/// </summary>
	public class MessagesRequestsLibrary : BaseRequestsLibrary
	{
		public MessagesRequestsLibrary(string accessToken) : base(accessToken)
		{
		}

        #region Внутренние методы
        internal VKRequestCommand<MessageCollection> CreateMessageCollectionRequestCommand(string methodName)
        {
            return new VKRequestCommand<MessageCollection>(AccessToken, methodName, (data) => { return new MessageCollection(data); });
        }

        internal VKRequestCommand<ActivityInfo> CreateActivityInfoRequestCommand(string methodName)
        {
            return new VKRequestCommand<ActivityInfo>(AccessToken, methodName, (data) => { return new ActivityInfo(data); });
        }

        internal VKRequestCommand<DialogsDataInfo> CreateDialogsDataInfoRequestCommand(string methodName)
        {
            return new VKRequestCommand<DialogsDataInfo>(AccessToken, methodName, (data) => { return new DialogsDataInfo(data); });
        }

        internal VKRequestCommand<LongPollInfo> CreateLongPollInfoVKRequestCommand(string methodName)
        {
            return new VKRequestCommand<LongPollInfo>(AccessToken, methodName, (data) => { return new LongPollInfo(data); });
        }
        #endregion

        #region Получение сообщений
        /// <summary>
		/// Возвращает список диалогов текущего пользователя	
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, последнее сообщение в переписке с которым необходимо вернуть.</param>
        /// <param name="chatId">Идентификатор беседы, последнее сообщение в которой необходимо вернуть.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества диалогов.</param>
        /// <param name="count">Количество диалогов, которое необходимо получить (но не более 100).</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        public VKRequestCommand<MessageCollection> CreateGetDialogsCommand(string userId, string chatId, int offset, int count, int previewLength)
		{
			const string methodName = "messages.getDialogs";
            var command = CreateMessageCollectionRequestCommand( methodName);
			if (userId != null && !String.IsNullOrEmpty(userId)) command.Parameters.Add("uid", userId);
			if (chatId != null && !String.IsNullOrEmpty(chatId)) command.Parameters.Add("chat_id", chatId);
			if (offset > 0) command.Parameters.Add("offset", offset);
			if (count > 0) command.Parameters.Add("count", count);
			command.Parameters.Add("preview_length", previewLength);

			return command;
		}
        public VKRequestCommand<MessageCollection> CreateGetDialogsCommand()
		{
			return CreateGetDialogsCommand(null, null, -1, -1, 0);
		}
		/// <summary>
		/// Возвращает историю сообщений для указанного пользователя или групповой бесед	
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, историю переписки с которым необходимо вернуть. Является необязательным параметром в случае с истории сообщений в беседе.</param>
        /// <param name="chatId">Идентификатор беседы, историю переписки в которой необходимо вернуть.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        public VKRequestCommand<MessageCollection> CreateGetHistoryCommand(string userId, string chatId, int offset, int count)
		{
			if ((userId == null || String.IsNullOrEmpty(userId)) && (chatId == null || String.IsNullOrEmpty(chatId)))
			{
				throw new ArgumentNullException("Должен быть задан userId или chatId");
			}
			const string methodName ="messages.getHistory";
            var command = CreateMessageCollectionRequestCommand( methodName);
			if (userId != null && !String.IsNullOrEmpty(userId)) command.Parameters.Add("uid", userId);
			if (chatId != null && !String.IsNullOrEmpty(chatId)) command.Parameters.Add("chat_id", chatId);
			if (offset > 0) command.Parameters.Add("offset", offset);
			if (count > 0) command.Parameters.Add("count", count);
			return command;
		}
        public VKRequestCommand<MessageCollection> CreateGetHistoryCommand(string userId, string chatId)
        {
            return CreateGetHistoryCommand(userId, chatId, -1, -1);
        }
        public VKRequestCommand<MessageCollection> CreateGetHistoryCommand(string userId)
		{
			return CreateGetHistoryCommand(userId, null, -1, -1);
		}
		/// <summary>
		/// Возвращает сообщения по их ID
        /// </summary>
        /// <param name="messageId">ID сообщения, если необходимо получить одно сообщение. Если указан параметр messageIds, этот параметр игнорируется.</param>
        /// <param name="messageIds">ID сообщений, которые необходимо вернуть, разделенные запятыми (не более 100).</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются)</param>
        public VKRequestCommand<MessageCollection> CreateGetMessagesByIdCommand(string messageId, string messageIds, int previewLength)
		{
			if ((messageId == null || String.IsNullOrEmpty(messageId)) && (messageIds == null || String.IsNullOrEmpty(messageIds)))
			{
				throw new ArgumentNullException("Должен быть задан messageId или messageIds");
			}
			const string methodName ="messages.getById";
            var command = CreateMessageCollectionRequestCommand( methodName);
			if (messageId != null && !String.IsNullOrEmpty(messageId)) command.Parameters.Add("mid", messageId);
			if (messageIds != null && !String.IsNullOrEmpty(messageIds)) command.Parameters.Add("mids", messageIds);
			command.Parameters.Add("preview_length", previewLength);
			return command;
		}
        public VKRequestCommand<MessageCollection> CreateGetMessagesByIdCommand(string messageId)
		{
			return CreateGetMessagesByIdCommand(messageId, null, 0);
		}
        /// <summary>
        /// Возвращает сообщения по их ID
        /// </summary>
        /// <param name="messageIds">ID сообщений, которые необходимо вернуть, разделенные запятыми (не более 100).</param>      
        public VKRequestCommand<MessageCollection> CreateGetListMessagesByIdCommand(string messageIds)
        {
            return CreateGetMessagesByIdCommand(null, messageIds, 0);
        }
		/// <summary>
		/// Возвращает список входящих или исходящих сообщений текущего пользователя
        /// </summary>
        /// <param name="outSign">Если этот параметр равен 1, сервер вернет исходящие сообщения.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        /// <param name="filters">Фильтр возвращаемых сообщений: 1 - только непрочитанные; 2 - не из чата; 4 - сообщения от друзей. Если установлен флаг "4", то флаги "1" и "2" не учитываются.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются). Обратите внимание что сообщения обрезаются по словам.</param>
        /// <param name="timeOffset">Максимальное время, прошедшее с момента отправки сообщения до текущего момента в секундах. 0, если Вы хотите получить сообщения любой давности.</param>
        public VKRequestCommand<MessageCollection> CreateGetMessagesCommand(bool outSign, int offset, int count, int filters, int previewLength, int timeOffset)
		{
			const string methodName ="messages.get";
            var command = CreateMessageCollectionRequestCommand( methodName);
			command.Parameters.Add("out", outSign ? 1 : 0);
			if (offset > 0) command.Parameters.Add("offset", offset);
			if (count > 0) command.Parameters.Add("count", count);
			if (filters > 0) command.Parameters.Add("filters", filters);
			command.Parameters.Add("preview_length", previewLength);
			command.Parameters.Add("time_offset", timeOffset);

			return command;
		}
        public VKRequestCommand<MessageCollection> CreateGetMessagesCommand(bool outSign)
		{
			return CreateGetMessagesCommand(outSign, -1, -1, 0, 0, 0);
		}
        public VKRequestCommand<MessageCollection> CreateGetMessagesCommand()
		{
			return CreateGetMessagesCommand(false, -1, -1, 0, 0, 0);
		}
		/// <summary>
		/// Возвращает список диалогов и бесед пользователя по поисковому запросу
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="fields">Поля профилей собеседников, которые необходимо вернуть.</param>
        public VKRequestCommand<DialogsDataInfo> CreateSearchDialogsCommand(string query, string fields)
		{
			if (query == null || String.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query");
			}
			const string methodName ="messages.searchDialogs";
            var command = CreateDialogsDataInfoRequestCommand(methodName);
			command.Parameters.Add("q", query);
			if (fields != null && !String.IsNullOrEmpty(fields)) command.Parameters.Add("fields", fields);

			return command;
		}
        public VKRequestCommand<DialogsDataInfo> CreateSearchDialogsCommand(string query)
		{
			return CreateSearchDialogsCommand(query, null);
		}
		/// <summary>
		/// Возвращает найденные сообщения текущего пользователя по введенной строке поиска
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений из списка найденных.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        public VKRequestCommand<MessageCollection> CreateSearchCommand(string query, int offset, int count)
		{
			if (query == null || String.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query");
			}
			const string methodName ="messages.search";
            var command = CreateMessageCollectionRequestCommand( methodName);
			command.Parameters.Add("q", query);
			if (offset > 0) command.Parameters.Add("offset", offset);
			if (count > 0) command.Parameters.Add("count", count);
			return command;
		}
        public VKRequestCommand<MessageCollection> CreateSearchCommand(string query)
		{
			return CreateSearchCommand(query, -1, -1);
		}
        #endregion

        #region Отправка/удаление сообщений
		/// <summary>
		/// Посылает сообщение
        /// </summary>
        /// <param name="userId">ID пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="chatId">ID беседы, к которой будет относиться сообщение</param>
        /// <param name="message">Текст личного cообщения (является обязательным, если не задан параметр attachment)</param>
        /// <param name="attachment">
        /// медиа-приложения к личному сообщению, перечисленные через запятую. Каждое прикрепление представлено в формате:
		/// {type}{owner_id}_{media_id}
		/// {type} - тип медиа-приложения, photo - фотография, video - видеозапись, audio - аудиозапись, doc - документ, wall - запись на стене
		/// {owner_id} - идентификатор владельца медиа-приложения
		/// {media_id} - идентификатор медиа-приложения.
		/// Например: photo100172_166443618
		/// Параметр является обязательным, если не задан параметр message.
		/// 
        /// <param name="forwardMessages">идентификаторы пересылаемых сообщений, перечисленные через запятую. Перечисленные сообщения отправителя будут отображаться в теле письма у получателя.
		/// Например: 123,431,544
        /// </param>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="type">0 - обычное сообщение, 1 - сообщение из чата. (по умолчанию 0)</param>
        /// <param name="latitude">latitude, широта при добавлении метоположения.</param>
        /// <param name="longitude">longitude, долгота при добавлении метоположения.</param>
        /// <param name="guid">Уникальный строковой идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения.</param>
        public VKRequestCommand<string> CreateSendMessageCommand(string userId, string chatId, string message, string[] attachment, string forwardMessages, string title, int type, int latitude, int longitude, string guid)
		{
			if ((userId == null || String.IsNullOrEmpty(userId)) && (chatId == null || String.IsNullOrEmpty(chatId)))
			{
				throw new ArgumentNullException("Должен быть задан userId или chatId");
			}
            if ((message == null || String.IsNullOrEmpty(message)) && (attachment == null || attachment.Length == 0))
			{
				throw new ArgumentNullException("Должен быть задан message или attachment");
			}
			const string methodName ="messages.send";
            var command = CreateStringRequestCommand(methodName);
			command.Parameters.Add("uid", userId);
			command.Parameters.Add("chat_id", chatId);
			if (message != null && !String.IsNullOrEmpty(message)) command.Parameters.Add("message", message);
			if (attachment != null && attachment.Length > 0) command.Parameters.Add("attachment", String.Join(Chars.Comma, attachment));
			if (forwardMessages != null && !String.IsNullOrEmpty(forwardMessages)) command.Parameters.Add("forward_messages", forwardMessages);
			if (title != null && !String.IsNullOrEmpty(title)) command.Parameters.Add("title", title);
			command.Parameters.Add("type", type);
			if (latitude > 0) command.Parameters.Add("lat", latitude);
			if (longitude > 0) command.Parameters.Add("long", longitude);
			if (guid != null && !String.IsNullOrEmpty(guid)) command.Parameters.Add("guid", guid);
			return command;
		}
        public VKRequestCommand<string> CreateSendMessageCommand(string userId, string chatId, string message, string[] attachment)
        {
            return CreateSendMessageCommand(userId, chatId, message, attachment, null, null, 0, -1, -1, null);
        }
        public VKRequestCommand<string> CreateSendMessageCommand(string userId, string chatId, string message)
		{
			return CreateSendMessageCommand(userId, chatId, message, null, null, null, 0, -1, -1, null);
		}
		/// <summary>
		/// Удаляет сообщение
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения.</param>
        public VKRequestCommand<bool> CreateDeleteMessageCommand(string messageId)
		{
			if (messageId == null || String.IsNullOrEmpty(messageId))
			{
				throw new ArgumentNullException("messageId");
			}
			const string methodName ="messages.delete";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("mid", messageId);
			return command;
		}
		/// <summary>
		/// Удаляет все сообщения в диалоге
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="chatId">ID беседы, к которой будет относиться сообщение</param>
        /// <param name="offset">Начиная с какого сообщения нужно удалить переписку. (По умолчанию удаляются все сообщения начиная с первого).</param>
        /// <param name="limit">Как много сообщений нужно удалить. Обратите внимание что на метод наложено ограничение, за один вызов нельзя удалить больше 10000 сообщений, поэтому если сообщений в переписке больше - метод нужно вызывать несколько раз</param>
        public VKRequestCommand<bool> CreateDeleteDialogCommand(string userId, string chatId, int offset, int limit)
		{
			if (userId == null || String.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			const string methodName ="messages.deleteDialog";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("uid", userId);
			command.Parameters.Add("chat_id", chatId);
			if (offset > 0) command.Parameters.Add("offset", offset);
			if (limit > 0) command.Parameters.Add("limit", limit);
			return command;
		}
        public VKRequestCommand<bool> CreateDeleteDialogCommand(string userId, string chatId)
		{
			return CreateDeleteDialogCommand(userId, chatId, -1, -1);
		}
		/// <summary>
		/// Восстанавливает только что удаленное сообщение.
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения.</param>
        public VKRequestCommand<bool> CreateRestoreCommand(string messageId)
		{
			if (messageId == null || String.IsNullOrEmpty(messageId))
			{
				throw new ArgumentNullException("messageId");
			}
			const string methodName ="messages.restore";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("mid", messageId);
			return command;
		}
		/// <summary>
		/// Помечает сообщения как непрочитанные
        /// </summary>
        /// <param name="messageIds">Список идентификаторов сообщений, разделенных запятой.</param>
        public VKRequestCommand<bool> CreateMarkAsNewCommand(string messageIds)
		{
			if (messageIds == null || String.IsNullOrEmpty(messageIds))
			{
				throw new ArgumentNullException("messageIds");
			}
			const string methodName ="messages.markAsNew";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("mids", messageIds);
			return command;
		}
		/// <summary>
		/// Помечает сообщения как прочитанные
        /// </summary>
        /// <param name="messageIds">Список идентификаторов сообщений, разделенных запятой.</param>
        public VKRequestCommand<bool> CreateMarkAsReadCommand(string messageIds)
		{
			if (messageIds == null || String.IsNullOrEmpty(messageIds))
			{
				throw new ArgumentNullException("messageIds");
			}
			const string methodName ="messages.markAsRead";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("mids", messageIds);
			return command;
		}
        #endregion

        #region Работа с беседами
		/// <summary>
		/// Создаёт беседу с несколькими участниками.
        /// </summary>
        /// <param name="userIds">Список идентификаторов друзей текущего пользователя с которыми необходимо создать беседу.</param>
        /// <param name="title">Название мультидиалога.</param>
        public VKRequestCommand<string> CreateNewChatCommand(string userIds, string title)
		{
			if (userIds == null || String.IsNullOrEmpty(userIds))
			{
				throw new ArgumentNullException("userIds");
			}
			const string methodName ="messages.createChat";
            var command = CreateStringRequestCommand(methodName);
			command.Parameters.Add("uids", userIds);
			if (title != null && !String.IsNullOrEmpty(title)) command.Parameters.Add("title", title);            
			return command;
		}
		/// <summary>
		/// Изменяет название беседы
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="title">Название беседы.</param>
        public VKRequestCommand<bool> CreateEditChatCommand(string chatId, string title)
		{
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			if (title == null || String.IsNullOrEmpty(title))
			{
				throw new ArgumentNullException("title");
			}
			const string methodName ="messages.editChat";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("chat_id", chatId);
			command.Parameters.Add("title", title);
			return command;
		}
		/// <summary>
		/// Получает список участников беседы
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="fields">Перечисленные через запятую поля объектов пользователей, которые необходимо вернуть. Поле invited_by (id пригласившего пользователя) передаётся всегда, если даннный параметр задан. Если параметр fields не задан метод вернёт список, содержащий только id участников.</param>
        public VKRequestCommand<UserCollection> CreateGetChatUsersCommand(string chatId, string[] fields)
		{
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			const string methodName ="messages.getChatUsers";
            var command = CreateUserCollectionRequestCommand(methodName);
			command.Parameters.Add("chat_id", chatId);
            if (fields != null && fields.Length > 0) command.Parameters.Add("fields", StringUtils.Join(fields, Chars.Comma));
			return command;
		}
		/// <summary>
		/// Добавляет в беседу нового участника
        /// </summary>
        /// <param name="chatId">ID беседы, в которую необходимо добавить пользователя</param>
        /// <param name="userId">ID пользователя.</param>
        public VKRequestCommand<bool> CreateAddChatUserCommand(string chatId, string userId)
		{
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			if (userId == null || String.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			const string methodName ="messages.addChatUser";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("chat_id", chatId);
			command.Parameters.Add("uid", userId);
			return command;
		}
		/// <summary>
		/// Исключает участника из беседы
        /// </summary>
        /// <param name="chatId">ID беседы, из которой необходимо удалить пользователя.</param>
        /// <param name="userId">ID пользователя.</param>
        public VKRequestCommand<bool> CreateRemoveChatUserCommand(string chatId, string userId)
		{
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			if (userId == null || String.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			const string methodName ="messages.removeChatUser";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("chat_id", chatId);
			command.Parameters.Add("uid", userId);
			return command;
		}
        #endregion

        #region Активность пользователя
		/// <summary>
		/// Изменяет статус набора текста пользователем в диалоге.
        /// </summary>
        /// <param name="userId">ID пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="chatId">ID беседы, к которой будет относиться сообщение</param>
        /// <param name="type">Typing - пользователь начал набирать текст</param>
        public VKRequestCommand<bool> CreateSetActivityCommand(string userId, string chatId, string type)
		{
			if (userId == null || String.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			if (chatId == null || String.IsNullOrEmpty(chatId))
			{
				throw new ArgumentNullException("chatId");
			}
			if (type == null || String.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type");
			}
			const string methodName = "messages.setActivity";
            var command = CreateBooleanRequestCommand(methodName);
			command.Parameters.Add("uid", userId);
			command.Parameters.Add("chat_id", chatId);
			command.Parameters.Add("type", type);
			return command;
		}
		/// <summary>
		/// Возвращает текущий статус и время последней активности пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя, для которого нужно получить время активности.</param>
        public VKRequestCommand<ActivityInfo> CreateGetLastActivityCommand(string userId)
		{
			const string methodName ="messages.getLastActivity";
            var command = CreateActivityInfoRequestCommand(methodName);
			command.Parameters.Add("uid", userId);
			return command;
		}
        #endregion

        #region Работа с LongPoll-сервером
		/// <summary>
		/// Возвращает данные, необходимые для подключения к LongPoll серверу
		/// </summary>
        public VKRequestCommand<LongPollInfo> CreateGetLongPollServerCommand()
		{
			const string methodName ="messages.getLongPollServer";
            return CreateLongPollInfoVKRequestCommand(methodName);
		}
		/// <summary>
		/// Возвращает последовательность обновлений в личных сообщениях пользователя начиная с указанного времени.
        /// </summary>
        /// <param name="ts">Последнее значение параметра ts, полученное от Long Poll сервера или с помощью метода messages.getLongPollServer</param>
        /// <param name="maxMessageId">Максимальный идентификатор сообщения среди уже имеющихся в локальной копии. Необходимо учитывать как сообщения, полученные через методы API (например messages.getDialogs, messages.getHistory), так и данные, полученные из Long Poll сервера (события с кодом 4).</param>
        public VKRequestCommand<string> CreateGetLongPollHistoryCommand(string ts, string maxMessageId)
		{
            throw new NotImplementedException();
			const string methodName ="messages.getLongPollHistory";
            var command = CreateStringRequestCommand(methodName);
			command.Parameters.Add("ts", ts);
			command.Parameters.Add("max_msg_id", maxMessageId);
			return command;
        }
        #endregion
    }
}