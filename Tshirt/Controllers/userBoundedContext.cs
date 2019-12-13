using System;

namespace Tshirt.Controllers
{
    internal class userBoundedContext : IDisposable
    {
        public object Database { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}