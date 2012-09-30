using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;

namespace Melchior
{
    public partial class AttachmentsPage : PhoneApplicationPage
    {
        public class PairImage
        {
            public AttachImage LeftImage { get; set; }
            public AttachImage RightImage { get; set; }
        }

        public AttachmentsPage()
        {
            InitializeComponent();
        }

        public void BuildAttachmentsList()
        {
            var list = new List<PairImage>();
            int i = 0;
            foreach (var attachImage in AttachmentsContext.Instance.AttachImages)
            {
                if (i % 2 == 0)
                {
                    var pair = new PairImage();
                    pair.LeftImage = attachImage;
                    list.Add(pair);
                }
                else
                {
                    list[list.Count - 1].RightImage = attachImage;
                }
                i++;
            }
            AttachmentsList.ItemsSource = list;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);
            AttachmentsTitle.Text = String.Format("{0} вложений", AttachmentsContext.Instance.AttachImages.Count);

            BuildAttachmentsList();
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            if (AttachmentsContext.Instance.AttachImages.Count < 10)
            {
                var task = new PhotoChooserTask();
                task.Completed += (xsender, xe) =>
                {
                    byte[] bytes = new byte[xe.ChosenPhoto.Length];
                    xe.ChosenPhoto.Read(bytes, 0, bytes.Length);

                    AttachmentsContext.Instance.AddAttachImage(bytes);

                    BuildAttachmentsList();
                };
                task.Show();
            }
        }
        private void CameraButton_Click(object sender, EventArgs e)
        {

        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageGuid = (int)((sender as Button).CommandParameter);            
            AttachmentsContext.Instance.RemoveByGuid(imageGuid);

            BuildAttachmentsList();
        }
    }
}