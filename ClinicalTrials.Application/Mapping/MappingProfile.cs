using AutoMapper;
using ClinicalTrials.Application.Dtos;
using ClinicalTrials.Domain.Entities;

namespace ClinicalTrials.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClinicalTrialDto, ClinicalTrial>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => MapEndDate(src)));

            CreateMap<ClinicalTrial, ClinicalTrialResponseDto>();

        }

        private DateTime? MapEndDate(ClinicalTrialDto clinicalTrialDto)
        {
            if (clinicalTrialDto.EndDate == null && clinicalTrialDto.Status == Domain.Enums.ClinicalTrialStatusEnum.Ongoing)
                return clinicalTrialDto.StartDate.AddMonths(1);

            return clinicalTrialDto.EndDate;
        }
    }
}