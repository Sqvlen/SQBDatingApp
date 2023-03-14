using AutoMapper;
using WebAPI.Entities;
using WebAPI.Extensions;
using WebAPI.DataTransferObjects;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>().ForMember(destinationMember => destinationMember.PhotoUrl,
                option => option.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(destinationMember => destinationMember.Age, 
                    option => option.MapFrom(source => source.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Message, MessageDTO>()
                .ForMember(message => message.SenderPhotoUrl,
                    options => options.MapFrom(sender => sender.Sender.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(message => message.RecipientPhotoUrl, 
                    options => options.MapFrom(recipient => recipient.Recipient.Photos.FirstOrDefault(photo => photo.IsMain).Url));
            CreateMap<DateTime, DateTime>()
                .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>()
                .ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
