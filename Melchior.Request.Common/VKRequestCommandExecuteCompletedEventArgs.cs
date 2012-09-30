using System;
using System.Net;
using System.ComponentModel;

namespace Melchior.Request.Common
{
    public class VKRequestCommandExecuteCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        public T Result { get; private set; }

        public VKRequestCommandExecuteCompletedEventArgs()
        {
        }
        public VKRequestCommandExecuteCompletedEventArgs(T result, Exception exception, bool canceled, object userState)
            : base(exception, canceled, userState)
        {
            Result = result;
        }
        public VKRequestCommandExecuteCompletedEventArgs(T result)
            : base()
        {
            Result = result;
        }
    }
    
}
