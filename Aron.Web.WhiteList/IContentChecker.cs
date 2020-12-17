using Aron.Web.WhiteList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aron.Web.WhiteList
{
    public interface IContentChecker
    {

        bool CheckForCidr(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack);
        bool CheckForSingleAddr(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack);
        bool CheckForAny(IEnumerable<WlContent> wlContents, IPAddress address, out bool isBlack);

    }
}
