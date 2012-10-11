using System;
using System.Collections.Generic;

namespace Melchior.Data.Common
{
    public abstract class VKDataCollection : IEnumerable<VKData>
    {
        public abstract VKData GetItem(int index);

        public abstract int GetLength();

        public IEnumerator<VKData> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}