namespace intro.ViewModels.Posts
{
    public class ModifyPost
    {
        public ulong p_id{get;set;}
        public Content content{get;set;}

        public class Content
        {
            public string name{get;set;}
            public ulong[] tags{get;set;}
            public string content {get;set;}
            public string photo{get;set;}

        }
    }

    
}