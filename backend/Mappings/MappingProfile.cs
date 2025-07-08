using System;
using AutoMapper;
using Congratz.backend.Models;
using Congratz.backend.Dtos;

namespace Congratz.backend.Mappings
{
    public class BirthdayMappingProfile : Profile
    {
        public BirthdayMappingProfile()
        {
            CreateMap<BirthdayPerson, BirthdayPersonDto>()
                .ForMember(dest => dest.PhotoBase64,
                    opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null));

            CreateMap<BirthdayPerson, BirthdayPersonShortDto>();

            CreateMap<BirthdayPersonCreateDto, BirthdayPerson>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.PhotoMimeType, opt => opt.Ignore());

            CreateMap<BirthdayPersonUpdateDto, BirthdayPerson>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.PhotoMimeType, opt => opt.Ignore());
        }
    }
}