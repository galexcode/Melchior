using System;
using Melchior.Models;
using Melchior.RequestsLibrary;

namespace Melchior.ViewModels
{
    public class RequestViewModel : ContactViewModel
    {
        public string Message { get; private set; }

        public RequestViewModel(string accessToken, RequestInfo request) : base(accessToken, request.UserId)
        {
            Message = request.Message;
        }
    }
}
