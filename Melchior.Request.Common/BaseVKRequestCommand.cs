using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Melchior.Data.Common;
using System.ComponentModel;
using Melchior.Common;
using System.IO.IsolatedStorage;
using System.Windows;

namespace Melchior.Request.Common
{
    public enum VKRequestTypes : int
    {
        XML = 0,
        JSON = 1,
    }
    public delegate void VKRequestCommandExecuteCompletedEventHandler<T>(object sender, VKRequestCommandExecuteCompletedEventArgs<T> e);

    public abstract class BaseVKRequestCommand<TResult>
    {
        public VKRequestParametersCollection Parameters = new VKRequestParametersCollection();

        protected string Query { get; set; }

        public BaseVKRequestCommand()
        {
        }

        public Uri GetUri()
        {
            Uri uri;
            if (Parameters.Any())
            {
                var builder = new StringBuilder();
                builder.Append(Query);
                foreach (var parameter in Parameters)
                {
                    builder.Append(Chars.Ampersand);
                    builder.Append(parameter.Name);
                    builder.Append(Chars.Equal);
                    builder.Append(parameter.Value);
                }
                uri = new Uri(builder.ToString());
            }
            else
            {
                uri = new Uri(Query);
            }
            return uri;
        }

        public virtual void Execute(bool useStorage = false, bool refreshStorage = false)
        {
            if (useStorage && !refreshStorage)
            {
                #region Чтение данных из IsolatedStorage
                var storageFileName = GetStorageFileName();
                try
                {
                    if (IsIsolatedStorageContainResponse(storageFileName))
                    {
                        var content = LoadResponseFromIsolatedStorage(storageFileName);
                        var convertResult = ConvertResult(content);
                        RaiseExecuteCompletedEvent(new VKRequestCommandExecuteCompletedEventArgs<TResult>(convertResult, null, false, null));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //RaiseExecuteCompletedEvent(new VKRequestCommandExecuteCompletedEventArgs<TResult>(default(TResult), ex, false, null));
                    //return;
                }
                #endregion
            }

            var uri = GetUri();
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.BeginGetResponse((callbackResponse) =>
            {
                string data = null;
                TResult convertResult = default(TResult);
                try
                {
                    var response = request.EndGetResponse(callbackResponse);
                    using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        data = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                    convertResult = ConvertResult(data);
                }
                catch (Exception ex)
                {
                    RaiseExecuteCompletedEvent(new VKRequestCommandExecuteCompletedEventArgs<TResult>(default(TResult), ex, false, null));
                    return;
                }

                if (useStorage)
                {
                    #region Запись данных в IsolatedStorage
                    try
                    {
                        var storageFileName = GetStorageFileName();
                        SaveResponseToIsolatedStorage(storageFileName, data);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        //RaiseExecuteCompletedEvent(new VKRequestCommandExecuteCompletedEventArgs<TResult>(default(TResult), ex, e.Cancelled, e.UserState));
                    }
                    #endregion
                }
                
                RaiseExecuteCompletedEvent(new VKRequestCommandExecuteCompletedEventArgs<TResult>(convertResult, null, false, null));

            }, null);
        }

        public event VKRequestCommandExecuteCompletedEventHandler<TResult> ExecuteCompleted;

        protected abstract TResult ConvertResult(string result);

        protected void RaiseExecuteCompletedEvent(VKRequestCommandExecuteCompletedEventArgs<TResult> e)
        {
            var handler = ExecuteCompleted;
            if (handler != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { handler(this, e); });
            }
        }

        protected abstract string GetStorageFileName();

        private void SaveResponseToIsolatedStorage(string key, string content)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = isolatedStorage.CreateFile(key))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(content);
                    streamWriter.Close();
                }
                stream.Close();
            }
        }

        private string LoadResponseFromIsolatedStorage(string key)
        {
            String content;
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = isolatedStorage.OpenFile(key, FileMode.Open))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                stream.Close();
            }
            return content;
        }

        private bool IsIsolatedStorageContainResponse(string key)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isolatedStorage.FileExists(key);
            }
        }
    }
}
