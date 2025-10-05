using ApplicantManagement.Application.DTOs;
using ApplicantManagement.Application.Interfaces;
using ApplicantManagement.Domain.Entities;
using ApplicantManagement.Domain.Interfaces;
using AutoMapper;

namespace ApplicantManagement.Application.Services;

public class ApplicantService : IApplicantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApplicantService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ApplicantDto>>> GetAllApplicantsAsync()
    {
        try
        {
            var applicants = await _unitOfWork.Applicants.GetAllAsync();
            var applicantDtos = _mapper.Map<IEnumerable<ApplicantDto>>(applicants);

            return ApiResponse<IEnumerable<ApplicantDto>>.SuccessResponse(
                applicantDtos,
                "Applicants retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ApplicantDto>>.FailureResponse(
                "Error retrieving applicants",
                new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<ApplicantDto>> GetApplicantByIdAsync(int id)
    {
        try
        {
            var applicant = await _unitOfWork.Applicants.GetByIdAsync(id);

            if (applicant == null)
            {
                return ApiResponse<ApplicantDto>.FailureResponse(
                    $"Applicant with ID {id} not found");
            }

            var applicantDto = _mapper.Map<ApplicantDto>(applicant);

            return ApiResponse<ApplicantDto>.SuccessResponse(
                applicantDto,
                "Applicant retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ApplicantDto>.FailureResponse(
                "Error retrieving applicant",
                new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<ApplicantDto>> CreateApplicantAsync(CreateApplicantDto createDto)
    {
        try
        {
            var applicant = _mapper.Map<Applicant>(createDto);

            // Set default value for Hired if not provided
            if (!createDto.Hired.HasValue)
            {
                applicant.Hired = false;
            }

            var createdApplicant = await _unitOfWork.Applicants.AddAsync(applicant);
            await _unitOfWork.SaveChangesAsync();

            var applicantDto = _mapper.Map<ApplicantDto>(createdApplicant);

            return ApiResponse<ApplicantDto>.SuccessResponse(
                applicantDto,
                "Applicant created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ApplicantDto>.FailureResponse(
                "Error creating applicant",
                new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<ApplicantDto>> UpdateApplicantAsync(int id, ApplicantDto updateDto)
    {
        try
        {
            var existingApplicant = await _unitOfWork.Applicants.GetByIdAsync(id);

            if (existingApplicant == null)
            {
                return ApiResponse<ApplicantDto>.FailureResponse(
                    $"Applicant with ID {id} not found");
            }

            // Map updated values
            _mapper.Map(updateDto, existingApplicant);

            await _unitOfWork.Applicants.UpdateAsync(existingApplicant);
            await _unitOfWork.SaveChangesAsync();

            var applicantDto = _mapper.Map<ApplicantDto>(existingApplicant);

            return ApiResponse<ApplicantDto>.SuccessResponse(
                applicantDto,
                "Applicant updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ApplicantDto>.FailureResponse(
                "Error updating applicant",
                new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<bool>> DeleteApplicantAsync(int id)
    {
        try
        {
            var exists = await _unitOfWork.Applicants.ExistsAsync(id);

            if (!exists)
            {
                return ApiResponse<bool>.FailureResponse(
                    $"Applicant with ID {id} not found");
            }

            var deleted = await _unitOfWork.Applicants.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(
                deleted,
                "Applicant deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse(
                "Error deleting applicant",
                new List<string> { ex.Message });
        }
    }
}