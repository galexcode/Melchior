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
using Melchior.RequestsLibrary;
using Melchior.ViewModels;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Melchior.Request.Xml;
using Melchior.Models;
using System.Text;
using System.IO;
using Melchior.Common;
using Microsoft.Phone.Shell;
using Melchior.ViewModels.PageViewModels;
using System.Collections.ObjectModel;

namespace Melchior
{
    public partial class ConversationPage : PhoneApplicationPage
    {
        private VKRequestCommand<MessageCollection> GetHistoryCommand { get; set; }

        public ConversationPage()
        {
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);
                        
            var conversationUserId = NavigationContext.QueryString.ContainsKey("ConversationUserId") ? NavigationContext.QueryString["ConversationUserId"] : null;
            var conversationChatId = NavigationContext.QueryString.ContainsKey("ConversationChatId") ? NavigationContext.QueryString["ConversationChatId"] : null;

            MelchiorContext.Instance.ConversationViewModel = new ConversationViewModel(conversationChatId, conversationUserId);
            DataContext = MelchiorContext.Instance.ConversationViewModel;

            var uri = new Uri(String.Format("/Images/Appbar_Icons/appbar.attachments-{0}.rest.png", AttachmentsContext.Instance.AttachImages.Count), UriKind.Relative);
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IconUri = uri;

            GetHistoryCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetHistoryCommand(conversationUserId, conversationChatId);
            GetHistoryCommand.ExecuteCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(e.Error);
                    return;
                }
                if (e.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
               MelchiorContext.Instance.ConversationViewModel.Messages = new ObservableCollection<MessageViewModel>(e.Result.Select(x => new MessageViewModel(MelchiorContext.Instance.AccessToken, MelchiorContext.Instance.OwnerUserId, conversationChatId, conversationUserId, x)));
            };
            GetHistoryCommand.Execute(true);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }


        private void AudioStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
            {
                BackgroundAudioPlayer.Instance.Pause();
            }
        }
        private void AudioPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
            {
                BackgroundAudioPlayer.Instance.Pause();
            }
            if (PlayState.Playing != BackgroundAudioPlayer.Instance.PlayerState)
            {
                var url = (string)((sender as Button).CommandParameter);
                
                BackgroundAudioPlayer.Instance.Track = new AudioTrack(new Uri(url), "test title", "test artist", "test album", null);
                BackgroundAudioPlayer.Instance.Play();
            }
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    break;

                case PlayState.Paused:
                case PlayState.Stopped:
                    break;
            }
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            var viewModel = (DataContext as ConversationViewModel);
            var message = MessageTextBox.Text;
            if (AttachmentsContext.Instance.AttachImages.Count > 0)
            {
                var photo2 = AttachmentsContext.Instance.AttachImages.First();
                SendImages(photo2, (photoCollection) =>
                {
                    var attachment = photoCollection.Select(x => String.Concat("photo", x.OwnerId, Chars.Underline, x.PictureId)).ToArray();

                    var sendMessageCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateSendMessageCommand(viewModel.ConversationUserId, viewModel.ConversationChatId, message, attachment);
                    sendMessageCommand.ExecuteCompleted += (xsender, xe) =>
                    {
                        if (xe.Error != null)
                        {
                            System.Diagnostics.Debug.WriteLine(xe.Error);
                            return;
                        }
                        AttachmentsContext.Instance.AttachImages.Clear();
                        if (xe.Result == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Result is empty");
                            return;
                        }
                        GetHistoryCommand.Execute(true, true);
                    };
                    sendMessageCommand.Execute();
                });
                return;
            }
            if (!String.IsNullOrEmpty(message))
            {
                var sendMessageCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateSendMessageCommand(viewModel.ConversationUserId, viewModel.ConversationChatId, message);
                sendMessageCommand.ExecuteCompleted += (xsender, xe) =>
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
                    GetHistoryCommand.Execute(true, true);
                };
                sendMessageCommand.Execute();
            }
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

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var uri = new Uri(String.Format("/Images/Appbar_Icons/appbar.attachments-{0}.rest.png", AttachmentsContext.Instance.AttachImages.Count), UriKind.Relative);
                        (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IconUri = uri;
                    });
                };
                task.Show();
            }
        }

        public void SendImages(AttachImage photo, Action<PhotoCollection> callbackSavePhotos)
        {
            var getMessagesUploadServer = MelchiorContext.Instance.PhotosRequestsLibrary.CreateGetMessagesUploadServerCommand();
            getMessagesUploadServer.ExecuteCompleted += (xsender, xe) =>
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
                var uploadUrl = xe.Result.UploadUrl;
                if (!String.IsNullOrEmpty(uploadUrl))
                {

                    MelchiorContext.Instance.PhotosRequestsLibrary.UploadPhoto(uploadUrl, photo.Bytes, callbackSavePhotos);
                }
            };
            getMessagesUploadServer.Execute();
        }
        

        private void AttachmentsButton_Click(object sender, EventArgs e)
        {
            var uri = new Uri("/AttachmentsPage.xaml?", UriKind.Relative);
            NavigationService.Navigate(uri);  
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            var viewModel = (DataContext as ConversationViewModel);

            if (!String.IsNullOrEmpty(viewModel.ConversationChatId))
            {
                var uri = new Uri(String.Concat("/ConversationSettingsPage.xaml?", "&ConversationChatId=", viewModel.ConversationChatId), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }

        private void RemoveDialogButton_Click(object sender, EventArgs e)
        {
            var viewModel = (DataContext as ConversationViewModel);

            // удалять можно только 10000 сообщений
            var deleteDialogCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateDeleteDialogCommand(viewModel.ConversationUserId, viewModel.ConversationChatId);
            deleteDialogCommand.ExecuteCompleted += (xsender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (!xe.Result)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                GetHistoryCommand.Execute(true, true);
            };
            deleteDialogCommand.Execute(true);
        }

        private void ProfileTitle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CopyMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}