using System;
using System.Collections.Generic;
using Melchior.Common;
using Melchior.Data.Common;

namespace Melchior.Models
{
	/// <summary>
	/// Описание пользователя
	/// <remarks>Author WrMax</remarks>
	///
	/// </summary>
	public class UserInfo : DataInfo
	{
		public const string FieldUserId = "uid";
		public const string FieldFirstName = "first_name";
		public const string FieldLastName = "last_name";
		public const string FieldSex = "sex";
		public const string FieldBirthDate = "bdate";
		public const string FieldCityId = "city";
		public const string FieldCountryId = "country";
		public const string FieldPhotoLink = "photo";
		public const string FieldPhotoMediumLink = "photo_medium";
		public const string FieldRectanglePhotoMediumLink = "photo_medium_rec";
		public const string FieldPhotoBigLink = "photo_big";
		public const string FieldRectanglePhotoLink = "photo_rec";
		public const string FieldIsOnline = "online";
		public const string FieldLists = "lists";
		public const string FieldScreenName = "screen_name";
		public const string FieldHasMobile = "has_mobile";
		public const string FieldRate = "rate";
		public const string FieldContacts = "contacts";
		public const string FieldEducation = "education";
		public const string FieldCanPost = "can_post";
		public const string FieldCanSeeAllPosts = "can_see_all_posts";
		public const string FieldCanWritePrivateMessage = "can_write_private_message";
		public const string FieldActivity = "activity";
		public const string FieldLastSeen = "last_seen";
		public const string FieldRelation = "relation";
		public const string FieldCounters = "counters";
		public const string FieldNickName = "nickname";
		public const string FieldExports = "exports";
		public const string FieldCanWallComments = "wall_comments";
		public const string FieldRelatives = "relatives";
		
		/// <summary>
		/// ID пользователя
		/// </summary>
		public string UserId { get { return  Data.GetFieldTextContent(FieldUserId); } }
		/// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get { return  Data.GetFieldTextContent(FieldFirstName); } }
		/// <summary>
		/// Фамилия
		/// </summary>
		public string LastName { get { return  Data.GetFieldTextContent(FieldLastName); } }
		/// <summary>
		/// Пол
		/// </summary>
		public int Sex
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldSex);
                if (String.IsNullOrEmpty(content)) return 0; // без указания пола.
                if (content.Equals(Chars.One)) return 1; // женский
                if (content.Equals(Chars.Two)) return 2; // мужской
                return 0; // без указания пола.
            }
		}
		/// <summary>
		/// Дата рождения, выдаётся в формате: "23.11.1981" или "21.9" (если год скрыт). 
		/// </summary>
		public string BirthDate { get { return  Data.GetFieldTextContent(FieldBirthDate); } }
		/// <summary>
		/// Выдаётся id города, указанного у пользователя в разделе "Контакты". Название города по его id можно узнать при помощи метода getCities.
		/// </summary>
		public string CityId { get { return  Data.GetFieldTextContent(FieldCityId); } }
		/// <summary>
		/// Выдаётся id страны, указанной у пользователя в разделе "Контакты". Название страны по её id можно узнать при помощи метода getCountries. 
		/// </summary>
		public string CountryId { get { return  Data.GetFieldTextContent(FieldCountryId); } }
		/// <summary>
		/// Выдаётся url фотографии пользователя, имеющей ширину 50 пикселей.  
		/// </summary>
		public string PhotoLink { get { return  Data.GetFieldTextContent(FieldPhotoLink); } }
		/// <summary>
		/// Выдаётся url фотографии пользователя, имеющей ширину 100 пикселей. 
		/// </summary>
		public string PhotoMediumLink { get { return  Data.GetFieldTextContent(FieldPhotoMediumLink); } }
		/// <summary>
		/// Выдаётся url квадратной фотографии пользователя, имеющей ширину 100 пикселей. 
		/// </summary>
		public string RectanglePhotoMediumLink { get { return  Data.GetFieldTextContent(FieldRectanglePhotoMediumLink); } }
		/// <summary>
		/// Выдаётся url фотографии пользователя, имеющей ширину 200 пикселей.  
		/// </summary>
		public string PhotoBigLink { get { return  Data.GetFieldTextContent(FieldPhotoBigLink); } }
		/// <summary>
		/// Выдаётся url квадратной фотографии пользователя, имеющей ширину 50 пикселей. 
		/// </summary>
		public string RectanglePhotoLink { get { return  Data.GetFieldTextContent(FieldRectanglePhotoLink); } }
		/// <summary>
		/// Показывает, находится ли этот пользователь сейчас на сайте.
		/// </summary>
		public bool IsOnline
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldIsOnline);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Список, содержащий id списков друзей, в которых состоит текущий друг пользователя.
		/// Поле доступно только для метода friends.get.
		/// </summary>
		public string Lists { get { return  Data.GetFieldTextContent(FieldLists); } }
		/// <summary>
		/// Возвращает короткий адрес страницы пользователя
		/// </summary>
		public string ScreenName { get { return  Data.GetFieldTextContent(FieldScreenName); } }
		/// <summary>
		/// Показывает, известен ли номер мобильного телефона пользователя.
		/// Рекомендуется перед вызовом метода secure.sendSMSNotification.
		/// </summary>
		public bool HasMobile
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldHasMobile);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Возвращает рейтинг пользователя. 
		/// </summary>
		public string Rate { get { return  Data.GetFieldTextContent(FieldRate); } }
		/// <summary>
		/// Возвращает в поле mobile_phone мобильный телефон пользователя, 
		/// а в поле home_phone домашний телефон пользователя,
		/// если эти данные указаны и не скрыты соотвествующими настройками приватности. 
		/// TODO дописать
		/// </summary>
		public string Contacts { get { return  Data.GetFieldTextContent(FieldContacts); } }
		/// <summary>
		/// Возвращает код и название университета пользователя, а также факультет и год оконочания. 
		/// </summary>
		public string Education { get { return  Data.GetFieldTextContent(FieldEducation); } }
		/// <summary>
		/// Разрешено ли оставлять записи на стене у данного пользователя.
		/// </summary>
		public bool CanPost
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldCanPost);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Разрешено ли текущему пользователю видеть записи других пользователей на стене данного пользователя. 
		/// </summary>
		public bool CanSeeAllPosts
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldCanSeeAllPosts);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Разрешено ли написание личных сообщений данному пользователю. 
		/// </summary>
		public bool CanWritePrivateMessage
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldCanWritePrivateMessage);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Возвращает статус, расположенный в профиле, под именем пользователя 
		/// </summary>
		public string Activity { get { return  Data.GetFieldTextContent(FieldActivity); } }
		/// <summary>
		/// Возвращает объект, содержащий поле time, в котором содержится время последнего захода пользователя.
		/// TODO дописать 
		/// </summary>
		public string LastSeen { get { return  Data.GetFieldTextContent(FieldLastSeen); } }
		/// <summary>
		/// Возвращает семейное положение пользователя: 
		/// 1 - не женат/не замужем 
		/// 2 - есть друг/есть подруга 
		/// 3 - помолвлен/помолвлена 
		/// 4 - женат/замужем 
		/// 5 - всё сложно 
		/// 6 - в активном поиске 
		/// 7 - влюблён/влюблена
		/// </summary>
		public int GetRelation
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldRelation);
                if (String.IsNullOrEmpty(content)) return 0;
                return Int32.Parse(content);
            }
		}
		/// <summary>
		/// Возвращает количество различных объектов у пользователя. Поле возвращается только в методе getProfiles при запросе информации об одном пользователе.
		/// Данное поле является объектом, который содержит следующие поля
		/// </summary>
		public string Counters { get { return  Data.GetFieldTextContent(FieldCounters); } }
		/// <summary>
		/// Данное поле возвращается только в том случае, если получается не больше одного профиля. 
		/// </summary>
		public string NickName { get { return  Data.GetFieldTextContent(FieldNickName); } }
		/// <summary>
		/// Данное поле доступно только Standalone приложениям.
		/// В случае, если запрашивается текущий пользователь — 
		/// возвращает объект exports содержаций настроенные пользователем сервисы для экспорта, 
		/// например twitter и facebook. 
		/// </summary>
		public string Exports { get { return  Data.GetFieldTextContent(FieldExports); } }
		/// <summary>
		/// Разрешено ли комментирование стены.
		/// </summary>
		public bool CanWallComments
		{
            get
            {
                string content = Data.GetFieldTextContent(FieldCanWallComments);
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// Возвращает список родственников текущего пользователя, в виде объектов, содержащих поля uid и type. type может принимать одно из следующих значений: grandchild, grandparent, child, sibling, parent.
		/// TODO дописать
		/// </summary>
        public List<RelativeInfo> GetRelatives
        {
            get
            {
                var relativesField = Data.GetField(FieldRelatives);
                if (relativesField == null) return null;
                var relatives = new List<RelativeInfo>();
                VKDataCollection items = relativesField.GetChildren();
                for (int i = 0, length = items.GetLength(); i < length; i++)
                {
                    relatives.Add(new RelativeInfo(items.GetItem(i)));
                }

                return relatives;
            }
        }
		
		public UserInfo(VKData data) : base(data)
		{
		}
	}
}
