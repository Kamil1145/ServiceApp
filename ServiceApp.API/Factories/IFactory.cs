
using ServiceApp.Models;

namespace ServiceApp.API.Factories
{
    public interface IFactory
    {
        public IProduct Create();
    }
}