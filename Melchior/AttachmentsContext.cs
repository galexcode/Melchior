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
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;

namespace Melchior
{
    public class AttachImage
    {
        public int ImageGuid { get; private set; }

        public readonly byte[] Bytes;

        public BitmapImage bitmapImage = null;
        public BitmapImage BitmapImage
        {
            get
            {
                if (bitmapImage == null)
                {
                    bitmapImage = new BitmapImage();
                    using (var stream = new MemoryStream(Bytes))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        bitmapImage.SetSource(stream);
                    }
                }
                return bitmapImage;
            }
        }

        public AttachImage(int imageGuid, byte[] bytes)
        {
            ImageGuid = imageGuid;
            Bytes = bytes;
        }
    }
    public class AttachmentsContext
    {
        private static volatile AttachmentsContext instance;

        private static readonly Object lockObject = new Object();

        public static AttachmentsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AttachmentsContext();
                        }
                    }
                }
                return instance;
            }
        }

        private static int AttachmentCounter = 0;
        public List<AttachImage> AttachImages = new List<AttachImage>();
        public void AddAttachImage(byte[] bytes)
        {
            AttachmentCounter = AttachmentCounter++;
            AttachImages.Add(new AttachImage(AttachmentCounter, bytes)); 
        }

        public void RemoveByGuid(int imageGuid)
        {
            var removeAttach = AttachImages.FirstOrDefault(x => x.ImageGuid == imageGuid);
            if (removeAttach != null)
            {
                AttachImages.Remove(removeAttach);
            }
        }
    }
}
