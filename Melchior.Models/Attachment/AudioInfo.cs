using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
	/// <summary>
	/// 
	/// <remarks>Author WrMax</remarks>
	/// </summary>
	public class AudioInfo : AttachmentInfo
	{
		/// <summary>
		/// идентификатор аудиозаписи 
		/// </summary>
        public string AudioId { get { return Data.GetFieldTextContent("aid"); } } 
		/// <summary>
		/// идентификатор владельца аудиозаписи
		/// </summary>
        public string OwnerId { get { return Data.GetFieldTextContent("owner_id"); } } 
		/// <summary>
		/// автор аудиозаписи 
		/// </summary>
        public string Performer { get { return Data.GetFieldTextContent("performer"); } } 
		/// <summary>
		/// название композиции 
		/// </summary>
        public string Title { get { return Data.GetFieldTextContent("title"); } } 
		/// <summary>
		/// длительность аудиозаписи (в секундах) 
		/// </summary>
        public string Duration { get { return Data.GetFieldTextContent("duration"); } } 
		/// <summary>
		/// адрес аудиозаписи по которому можно нчинать воспроизведение 
		/// </summary>
        public string Url { get { return Data.GetFieldTextContent("url"); } }

        public string LyricsId { get { return Data.GetFieldTextContent("lyrics_id"); } }
        public string Artist { get { return Data.GetFieldTextContent("artist"); } } 

		public AudioInfo(VKData data) : base(data)
		{
		}
	}
}