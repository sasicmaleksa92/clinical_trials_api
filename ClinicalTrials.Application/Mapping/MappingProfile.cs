using AutoMapper;
using ClinicalTrials.Application.Dtos;
using ClinicalTrials.Domain.Common.Extensions;
using ClinicalTrials.Domain.Entities;

namespace ClinicalTrials.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClinicalTrialDto, ClinicalTrial>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => MapEndDate(src)));

            CreateMap<ClinicalTrial, ClinicalTrialResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetDescription()));

        }

        private DateTime? MapEndDate(ClinicalTrialDto clinicalTrialDto)
        {
            if (clinicalTrialDto.EndDate == null && clinicalTrialDto.Status == Domain.Enums.ClinicalTrialStatusEnum.Ongoing)
                return clinicalTrialDto.StartDate.AddMonths(1);

            return clinicalTrialDto.EndDate;
        }
    }
}