using System;
using System.Collections.Generic;

namespace Melchior.Request.Common
{
    /// <summary>
    /// Коллекция параметров для команды VKRequestCommand
    /// @author wrmax
    ///
    /// </summary>

    public class VKRequestParametersCollection : List<VKRequestParameter>
    {
        public VKRequestParametersCollection()
        {
        }
        public void Add(string name, object value)
        {
            Add(new VKRequestParameter(name, value));
        }
    }
}