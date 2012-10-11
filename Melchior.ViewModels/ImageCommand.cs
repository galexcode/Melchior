using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace Melchior.ViewModels
{
    public class LoadImageCommandExecuteCompletedEventArgs : AsyncCompletedEventArgs
    {
        public BitmapImage Result { get; private set; }

        public LoadImageCommandExecuteCompletedEventArgs()
        {
        }
        public LoadImageCommandExecuteCompletedEventArgs(BitmapImage result, Exception exception, bool canceled, object userState)
            : base(exception, canceled, userState)
        {
            Result = result;
        }
        public LoadImageCommandExecuteCompletedEventArgs(BitmapImage result)
            : base()
        {
            Result = result;
        }
    }
    public delegate void LoadImageCommandExecuteCompletedEventHandler(object sender, LoadImageCommandExecuteCompletedEventArgs e);

    public class LoadImageCommand
    {
        public readonly Uri Uri;
        public LoadImageCommand(Uri uri)
        {
            Uri = uri;
        }
        public void Execute(bool cacheQuery = false)
        {
            if (Path.GetExtension(Uri.AbsolutePath) != ".jpg")
            {
                if (Uri.AbsolutePath != "/images/deactivated_c.gif" && Uri.AbsolutePath != "/images/camera_c.gif")
                {
                    var ex = new Exception(String.Format("Photo {0} cannot be loaded. Only jpg format supported.", Uri.AbsolutePath));
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, ex, false, null));
                    return;
                }
                else
                {
                    var ex = new Exception(String.Format("Photo {0} cannot be loaded. Only jpg format supported.", Uri.AbsolutePath));
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, ex, false, null));
                    return;
                }
            }
            if (cacheQuery)
            {
                var storageFileName = GetStorageFileName();
                try
                {
                    if (IsIsolatedStorageContainResponse(storageFileName))
                    {
                        var bitmap = LoadResponseFromIsolatedStorage(storageFileName);
                        RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(bitmap, null, false, null));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, ex, false, null));
                    return;
                }
            }

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.OpenReadCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, e.Error, e.Cancelled, e.UserState));
                    return;
                }
                if (cacheQuery)
                {
                    try
                    {
                        var storageFileName = GetStorageFileName();
                        SaveResponseToIsolatedStorage(storageFileName, e.Result);
                    }
                    catch (Exception ex)
                    {
                        RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, ex, e.Cancelled, e.UserState));
                        return;
                    }
                }
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(e.Result);
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(bitmap, null, e.Cancelled, e.UserState));
                    return;
                }
                catch (Exception ex)
                {
                    RaiseExecuteCompletedEvent(new LoadImageCommandExecuteCompletedEventArgs(null, ex, e.Cancelled, e.UserState));
                    return;
                }
            };
            client.OpenReadAsync(Uri);
        }

        public event LoadImageCommandExecuteCompletedEventHandler ExecuteCompleted;

        protected void RaiseExecuteCompletedEvent(LoadImageCommandExecuteCompletedEventArgs e)
        {
            var handler = ExecuteCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private string GetStorageFileName()
        {
            return Uri.AbsolutePath.Replace("/", "");
        }
        private void SaveResponseToIsolatedStorage(string key, Stream inputStream)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = isolatedStorage.CreateFile(key))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var streamReader = new StreamReader(inputStream))
                    {
                        streamWriter.Write(streamReader.ReadToEnd());
                        streamReader.Close();
                    }
                    streamWriter.Close();
                }
                stream.Close();
            }
            inputStream.Seek(0, SeekOrigin.Begin);
        }

        private BitmapImage LoadResponseFromIsolatedStorage(string key)
        {
            var bitmap = new BitmapImage();
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = isolatedStorage.OpenFile(key, FileMode.Open))
            {
                bitmap.SetSource(stream);
                stream.Close();
            }
            return bitmap;
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