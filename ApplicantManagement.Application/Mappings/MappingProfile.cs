using ApplicantManagement.Application.DTOs;
using ApplicantManagement.Domain.Entities;
using AutoMapper;

namespace ApplicantManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Applicant mappings
        CreateMap<Applicant, ApplicantDto>();
        CreateMap<CreateApplicantDto, Applicant>();
        CreateMap<ApplicantDto, Applicant>();
    }
}