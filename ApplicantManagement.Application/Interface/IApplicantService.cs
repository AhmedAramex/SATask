using ApplicantManagement.Application.DTOs;

namespace ApplicantManagement.Application.Interfaces;

public interface IApplicantService
{
    Task<ApiResponse<IEnumerable<ApplicantDto>>> GetAllApplicantsAsync();
    Task<ApiResponse<ApplicantDto>> GetApplicantByIdAsync(int id);
    Task<ApiResponse<ApplicantDto>> CreateApplicantAsync(CreateApplicantDto createDto);
    Task<ApiResponse<ApplicantDto>> UpdateApplicantAsync(int id, ApplicantDto updateDto);
    Task<ApiResponse<bool>> DeleteApplicantAsync(int id);
}