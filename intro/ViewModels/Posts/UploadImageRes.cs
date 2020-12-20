using System;
using System.Collections.Generic;

namespace intro.ViewModels.Posts
{
    public class UploadImageRes
    {
        public bool uploaded{get;set;}
        public IEnumerable<string> url{get;set;}
    }
}