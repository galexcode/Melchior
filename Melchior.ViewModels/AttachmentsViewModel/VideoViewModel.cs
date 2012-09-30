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
    public class VideoViewModel : AttachmentViewModel
    {
        public string Image { get; set; }
        public VideoViewModel(VideoInfo video)
        {
            Image = video.Image;
        }
    }
}
