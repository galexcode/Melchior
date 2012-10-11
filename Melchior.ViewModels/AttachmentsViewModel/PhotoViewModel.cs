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
using Melchior.Models;
using System.Windows.Media.Imaging;

namespace Melchior.ViewModels.AttachmentsViewModel
{
    public class PhotoViewModel : AttachmentViewModel
    {
        public string Source { get; set; }
        public PhotoViewModel(PhotoInfo photo)
        {
            Source = photo.Source;
        }
    }
}