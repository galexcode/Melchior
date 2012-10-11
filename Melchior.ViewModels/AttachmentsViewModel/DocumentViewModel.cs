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

namespace Melchior.ViewModels.AttachmentsViewModel
{
    public class DocumentViewModel : AttachmentViewModel
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public DocumentViewModel(DocumentInfo document)
        {
            Title = document.Title;
            Url = document.Url;
        }
    }
}