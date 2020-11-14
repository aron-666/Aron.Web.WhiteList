using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intro.Models
{
    public class MyMapper : Profile
    {
        private IMapper mapper2;
        public MyMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new Models.MyMapper2()));
            mapper2 = config.CreateMapper();
            CreateMap<Models.WhiteLists.Whitelists, Aron.Web.WhiteList.Models.WhiteLists>()
                .ForMember(att => att.WlContent, opt => opt.MapFrom(src => src.Source.Select(x => mapper2.Map<Aron.Web.WhiteList.Models.WlContent>(x))))
                .AfterMap((x, y) => { foreach (var i in y.WlContent) { i.Source = y; } });

        }

        
    }

    public class MyMapper2 : Profile
    {
        public MyMapper2()
        {
            CreateMap<Models.WhiteLists.WlContent, Aron.Web.WhiteList.Models.WlContent>()
                .ForMember(att => att.policy, opt => opt.Ignore());
                ;
        }
    }
}
