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
using Melchior.RequestsLibrary;

namespace Melchior
{
    public sealed class AuthorizationContext
    {
        private static volatile AuthorizationContext instance;
 
        private static readonly Object lockObject = new Object();

        public static AuthorizationContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AuthorizationContext();
                        }
                    }
                }
                return instance;
            }
        }

        public readonly string ClientId;
        public readonly string ClientSecret;
        public AuthRequestsLibrary AuthRequestsLibrary;

        private AuthorizationContext()
        {
            ClientId = "2979806";
            ClientSecret = "LdW65MFyWqq8C4nLFkjK";
            AuthRequestsLibrary = new AuthRequestsLibrary(ClientId, ClientSecret);
        }
    }
}
