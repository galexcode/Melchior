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
using System.Reflection;
using Melchior.ViewModels;
using Melchior.ViewModels.AttachmentsViewModel;

namespace Melchior
{
    public class AttachmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate
        {
            get;
            set;
        }

        public DataTemplate PhotoTemplate
        {
            get;
            set;
        }
        public DataTemplate AudioTemplate
        {
            get;
            set;
        }

        public DataTemplate DocumentTemplate
        {
            get;
            set;
        }
        public DataTemplate VideoTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var attachment = item as AttachmentViewModel;
            if (attachment != null)
            {
                var type = attachment.GetType();
                if (type == typeof(PhotoViewModel))
                {
                    return PhotoTemplate;
                }
                if (type == typeof(AudioViewModel))
                {
                    return AudioTemplate;
                }
                if (type == typeof(DocumentViewModel))
                {
                    return DocumentTemplate;
                }
                if (type == typeof(VideoViewModel))
                {
                    return VideoTemplate;
                }
            }
            return DefaultTemplate;
        }
    }
}
