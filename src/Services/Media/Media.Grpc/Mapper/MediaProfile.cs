using AutoMapper;
using Media.Grpc.Protos;

namespace Media.Grpc.Mapper
{
    public class MediaProfile:Profile
    {
        public MediaProfile()
        {
            CreateMap<Entities.Media, MediaModel>().ReverseMap();
        }
    }
}