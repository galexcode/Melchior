using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Melchior.RequestsLibrary;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Melchior.Models;
using Melchior.Common;
using Melchior.ViewModels.PageViewModels;
using System.Linq;
using System.Collections.ObjectModel;
using Melchior.ViewModels;

namespace Melchior
{
    public enum MessageFlags : int
    {
        /// <summary>сообщение не прочитано</summary>
        Unread = 1,
        /// <summary>исходящее сообщение</summary>
        Outbox = 2,
        /// <summary>на сообщение был создан ответ</summary>
        Replied = 4,
        /// <summary>помеченное сообщение</summary>
        Important = 8,
        /// <summary>сообщение отправлено через чат</summary>
        Chat = 16,
        /// <summary>сообщение отправлено другом</summary>
        Friends = 32,
        /// <summary>сообщение помечено как "Спам"</summary>
        Spam = 64,
        /// <summary>сообщение удалено (в корзине)</summary>
        Deleted = 128,
        /// <summary>сообщение проверено пользователем на спам</summary>
        Fixed = 256,
        /// <summary>сообщение содержит медиаконтент</summary>
        Media = 512
    }
    public enum UserStatuses
    {
        Online = 0,
        Offline = 1,
        Timeout = 2
    }
    public sealed class MelchiorContext
    {
        private static volatile MelchiorContext instance;
 
        private static readonly Object lockObject = new Object();

        public static MelchiorContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MelchiorContext();
                        }
                    }
                }
                return instance;
            }
        }

        public string AccessToken { get; private set; }
        public string OwnerUserId { get; private set; }

        public MessagesRequestsLibrary MessagesRequestsLibrary { get; private set; }
        public FriendsRequestsLibrary FriendsRequestsLibrary { get; private set; }
        public UsersRequestsLibrary UsersRequestsLibrary { get; private set; }
        public PhotosRequestsLibrary PhotosRequestsLibrary { get; private set; }

        private List<LongPollEventDescription> changeMessages = new List<LongPollEventDescription>();
        public List<LongPollEventDescription> ChangeMessages { get { return changeMessages; } }

        private Dictionary<string, UserStatuses> usersStatus = new Dictionary<string, UserStatuses>();
        public Dictionary<string, UserStatuses> UsersStatus { get { return usersStatus; } }

        private List<LongPollEventDescription> changeUsers = new List<LongPollEventDescription>();
        public List<LongPollEventDescription> ChangeUsers { get { return changeUsers; } }

        private List<LongPollEventDescription> changeChat = new List<LongPollEventDescription>();
        public List<LongPollEventDescription> ChangeChat { get { return changeChat; } }

        public bool SwitchVibrate { get; set; }
        public bool SwitchSound { get; set; }
        public bool SwitchHint { get; set; }
        public DateTime? DisableNotificationDate { get; set; }

        public ConversationViewModel ConversationViewModel { get; set; }
        private MelchiorContext()
        {
            SwitchVibrate = true;
            SwitchSound = true;
            SwitchHint = true;
            DisableNotificationDate = null;
        }
        public void Authorize(string accessToken, string ownerUserId)
        {
            lock (lockObject)
            {
                AccessToken = accessToken;
                OwnerUserId = ownerUserId;
                MessagesRequestsLibrary = new MessagesRequestsLibrary(accessToken);
                FriendsRequestsLibrary = new FriendsRequestsLibrary(accessToken);
                UsersRequestsLibrary = new UsersRequestsLibrary(accessToken);
                PhotosRequestsLibrary = new PhotosRequestsLibrary(accessToken);

                GetLongPollServer();
            }
        }

        public void LogOff()
        {
            lock (lockObject)
            {
                MessagesRequestsLibrary = null;
                FriendsRequestsLibrary = null;
                UsersRequestsLibrary = null;
                PhotosRequestsLibrary = null;
                AccessToken = null;
                OwnerUserId = null;
            }
        }

        public void GetLongPollServer()
        {
            var getLongPollServerCommand = MessagesRequestsLibrary.CreateGetLongPollServerCommand();
            getLongPollServerCommand.ExecuteCompleted += (xsender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    getLongPollServerCommand.Execute();
                    return;
                }
                if (xe.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                SubscribeToLongPoll(xe.Result.Key, xe.Result.Server, xe.Result.TS);
            };
            getLongPollServerCommand.Execute();
        }
        private void SubscribeToLongPoll(string key, string server, string ts)
        {
            var command = new LongPollRequestCommand(server, key);
            command.Parameters.Add(new Request.Common.VKRequestParameter("ts", ts));
            command.Parameters.Add(new Request.Common.VKRequestParameter("wait", 25));
            //mode - параметр, определяющий наличие поля прикреплений в получаемых данных. Значения: 2 - получать прикрепления, 0 - не получать.
            command.Parameters.Add(new Request.Common.VKRequestParameter("mode", 0));
            command.ExecuteCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(e.Error);
                    GetLongPollServer();
                    return;
                }
                if (e.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                var updates = e.Result.Updates;
                if (updates != null)
                {
                    foreach (var update in e.Result.Updates)
                    {
                        var code = Int32.Parse(update[0]);
                        switch (code)
                        {
                            case 0: //changeMessages.Add(update);
                                if (ConversationViewModel != null && ConversationViewModel.Messages != null)
                                {
                                    var messageId = update[1];
                                    var removeMessage = ConversationViewModel.Messages.FirstOrDefault(x => x.MessageId == messageId);
                                    if (removeMessage != null) ConversationViewModel.Messages.Remove(removeMessage);
                                }
                                break; // 0,$message_id,0 -- удаление сообщения с указанным local_id
                            case 1: break; // 1,$message_id,$flags -- замена флагов сообщения (FLAGS:=$flags)
                            case 2: break; // 2,$message_id,$mask[,$user_id] -- установка флагов сообщения (FLAGS|=$mask)
                            case 3: break; // 3,$message_id,$mask[,$user_id] -- сброс флагов сообщения (FLAGS&=~$mask)
                            case 4:
                                // 4,$message_id,$flags,$from_id,$timestamp,$subject,$text,$attachments -- добавление нового сообщения
                                if (ConversationViewModel != null && ConversationViewModel.Messages != null)
                                {
                                    var messageId = update[1];
                                    var getMessagecommand = MessagesRequestsLibrary.CreateGetMessagesByIdCommand(messageId);

                                    getMessagecommand.ExecuteCompleted += (xsender, xe) =>
                                    {
                                        if (xe.Error != null)
                                        {
                                            System.Diagnostics.Debug.WriteLine(xe.Error);
                                            return;
                                        }
                                        if (xe.Result == null || xe.Result.Count < 1)
                                        {
                                            System.Diagnostics.Debug.WriteLine("Result is empty");
                                            return;
                                        }
                                        var message = xe.Result[0];
                                        if (ConversationViewModel != null) ConversationViewModel.Messages.Add(new ViewModels.MessageViewModel(AccessToken, OwnerUserId, ConversationViewModel.ConversationChatId, ConversationViewModel.ConversationUserId, message));
                                    };
                                    getMessagecommand.Execute(true);

                                    var getHistoryCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetHistoryCommand(ConversationViewModel.ConversationUserId, ConversationViewModel.ConversationChatId);
                                    getHistoryCommand.ExecuteCompleted += (xsender, xe) =>
                                    {
                                        if (xe.Error != null)
                                        {
                                            System.Diagnostics.Debug.WriteLine(e.Error);
                                            return;
                                        }
                                        if (xe.Result == null)
                                        {
                                            System.Diagnostics.Debug.WriteLine("Result is empty");
                                            return;
                                        }
                                        MelchiorContext.Instance.ConversationViewModel.Messages = new ObservableCollection<MessageViewModel>(xe.Result.Select(x => new MessageViewModel(MelchiorContext.Instance.AccessToken, MelchiorContext.Instance.OwnerUserId, ConversationViewModel.ConversationChatId, ConversationViewModel.ConversationUserId, x)));
                                    };
                                    getHistoryCommand.Execute(true);
                                }
                                break;

                            case 8:
                                // 8,-$user_id,0 -- друг $user_id стал онлайн
                                {
                                    var userId = update[1];
                                    if (!usersStatus.ContainsKey(userId))
                                    {
                                        usersStatus.Add(userId, UserStatuses.Online);
                                    }
                                    else
                                    {
                                        usersStatus[userId] = UserStatuses.Online;
                                    }
                                }
                                break;
                            case 9:
                                //9,-$user_id,$flags -- друг $user_id стал оффлайн ($flags равен 0, если пользователь покинул сайт (например, нажал выход) и 1, если оффлайн по таймауту (например, статус away))
                                {
                                    var userId = update[1];
                                    var userStatus = update[2].Equals(Chars.Zero) ? UserStatuses.Offline : UserStatuses.Timeout;
                                    if (!usersStatus.ContainsKey(userId))
                                       {
                                        usersStatus.Add(userId, UserStatuses.Online);
                                    }
                                    else
                                    {
                                        usersStatus[userId] = UserStatuses.Online;
                                    }
                                }
                                break;

                            case 51:
                                // 51,$chat_id,$self -- один из параметров (состав, тема) беседы $chat_id были изменены. $self - были ли изменения вызываны самим пользователем
                                break;
                            case 61:
                                // 61,$user_id,$flags -- пользователь $user_id начал набирать текст в диалоге. событие должно приходить раз в ~5 секунд при постоянном наборе текста. $flags = 1
                                {
                                    var userId = update[1];
                                    SendEventForConversationUserKeyPressed(userId);
                                }
                                break;
                            case 62:
                                // 62,$user_id,$chat_id -- пользователь $user_id начал набирать текст в беседе $chat_id.
                                {
                                    var userId = update[1];
                                    SendEventForConversationUserKeyPressed(userId);
                                }
                                break;
                            case 70:
                                // 70,$user_id,$call_id -- пользователь $user_id совершил звонок имеющий идентификатор $call_id, дополнительную информацию о звонке можно получить используя метод voip.getCallInfo.
                                {
                                    var userId = update[1];
                                    SendEventForConversationUserKeyPressed(userId);
                                }
                                break;
                        }
                        System.Diagnostics.Debug.WriteLine(update);
                    }
                }
                command.Parameters[0].Value = e.Result.TS;
                command.Execute();
            };
            command.Execute();
        }

        private void SendEventForConversationUserKeyPressed(string userId)
        {
            if (ConversationViewModel == null) return;
            var getUserCommand = UsersRequestsLibrary.CreateGetUsersCommand(userId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName });
            getUserCommand.ExecuteCompleted += (xsender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (xe.Result == null || xe.Result.Count < 1)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                var user = xe.Result[0];
                if (ConversationViewModel != null) ConversationViewModel.EventNote = String.Format("Пользовать {0} {1} начал набирать текст...", user.LastName, user.FirstName);
            };
            getUserCommand.Execute(true);
        }
    }
}
