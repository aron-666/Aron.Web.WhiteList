using System.Net;

namespace Aron.Web.WhiteList
{
    public interface IWhiteListService
    {
        bool Check(string path, IPAddress address);
        bool Check(string path, string address);
        bool Enable();
        bool Disable(int waitMs = 10000);
        bool IsEnable();

    }
}