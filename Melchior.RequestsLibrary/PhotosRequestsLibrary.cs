using System;
using Melchior.Request.Xml;
using Melchior.Models;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Melchior.Request.Common;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Работа с фотографиями
    /// author wrmax
    /// </summary>
    public class PhotosRequestsLibrary : BaseRequestsLibrary
    {
        public PhotosRequestsLibrary(string accessToken)
            : base(accessToken)
        {
        }

        #region Внутренние методы
        internal VKRequestCommand<PhotoCollection> CreatePhotoCollectionRequestCommand(string methodName)
        {
            return new VKRequestCommand<PhotoCollection>(AccessToken, methodName, (data) => { return new PhotoCollection(data); });
        }
        #endregion

        /// <summary>
        /// Возвращает адрес сервера для загрузки﻿ фотографии в личное сообщение пользователю. 
        /// После успешной загрузки Вы можете сохранить фотографию, воспользовавшись методом photos.saveMessagesPhoto.
        /// </summary>
        public VKRequestCommand<UploadInfo> CreateGetMessagesUploadServerCommand()
        {
            const string methodName = "photos.getMessagesUploadServer";
            return CreateUploadInfoRequestCommand(methodName);
        }

        /// <summary>
        /// Сохраняет фотографию после успешной загрузки на URI, полученный методом photos.getMessagesUploadServer.
        /// </summary>
        /// <param name="server">параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <param name="photo">параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <param name="hash">параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        public VKRequestCommand<PhotoCollection> CreateSaveMessagesPhotoCommand(string server, string photo, string hash)
        {
            const string methodName = "photos.saveMessagesPhoto";
            var command = CreatePhotoCollectionRequestCommand(methodName);
            command.Parameters.Add("server", server);
            command.Parameters.Add("photo", photo);
            command.Parameters.Add("hash", hash);
            return command;
        }

        public void UploadPhoto(string uploadUrl, byte[] image, Action<PhotoCollection> callbackUploadPhoto)
        {
            string boundary = String.Format("--{0}", "--------b7X0S7Mfbmnx7HzPYokr4k");
            var request = (HttpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = "POST";
            request.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);

            string templateFile = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\";filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
            string templateEnd = "--{0}--\r\n\r\n";
            string filePath = "test.jpg";
            string fileType = "application/octet-stream";
            string name = "file1";
            byte[] contentFile = Encoding.UTF8.GetBytes(String.Format(templateFile, boundary, name, filePath, fileType));

            byte[] lineFeed = Encoding.UTF8.GetBytes("\r\n");
            byte[] contentEnd = Encoding.UTF8.GetBytes(String.Format(templateEnd, boundary));

            request.BeginGetRequestStream(
                (callbackRequest) =>
                {
                    using (var requestStream = request.EndGetRequestStream(callbackRequest))
                    {
                        requestStream.Write(contentFile, 0, contentFile.Length);
                        requestStream.Write(image, 0, image.Length);
                        requestStream.Write(lineFeed, 0, lineFeed.Length);
                        requestStream.Write(contentEnd, 0, contentEnd.Length);
                        requestStream.Close();
                    }
                    request.BeginGetResponse((callbackResponse) =>
                    {
                        var response = request.EndGetResponse(callbackResponse);
                        string result = null;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                            streamReader.Close();
                        }
                        if (result != null)
                        {
                            var document = JObject.Parse(result);
                            var data = new VKDataJSON(document);
                            if (data.GetName().Equals("error"))
                            {
                                throw new VKRequestCustomException(data);
                            }

                            var uploadPhotoInfo = new UploadPhotoInfo(data);
                            var saveMessagePhotoCommand = CreateSaveMessagesPhotoCommand(uploadPhotoInfo.Server, uploadPhotoInfo.Photo, uploadPhotoInfo.Hash);
                            saveMessagePhotoCommand.ExecuteCompleted += (xsender, xe) =>
                            {
                                if (xe.Error != null)
                                {
                                    System.Diagnostics.Debug.WriteLine(xe.Error);
                                    return;
                                }
                                if (xe.Result == null)
                                {
                                    System.Diagnostics.Debug.WriteLine("Result is empty");
                                    return;
                                }
                                callbackUploadPhoto(xe.Result);
                            };
                            saveMessagePhotoCommand.Execute();
                        }
                    }, null);

                }, null);
        }
    }
}
