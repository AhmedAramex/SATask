using ApplicantManagement.Application.DTOs;
using ApplicantManagement.Application.Services;
using ApplicantManagement.Domain.Entities;
using ApplicantManagement.Domain.Interfaces;
using ApplicantManagement.Infrastructure.Data.Repositories;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace ApplicantManagement.Tests.Services;

public class ApplicantServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<Domain.Interfaces.IRepository<Applicant>> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ApplicantService _service;

    public ApplicantServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IRepository<Applicant>>();

        // Setup UnitOfWork to return mocked repository
        _unitOfWorkMock.Setup(u => u.Applicants).Returns(_repositoryMock.Object);

        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Applicant, ApplicantDto>();
            cfg.CreateMap<CreateApplicantDto, Applicant>();
            cfg.CreateMap<UpdateApplicantDto, Applicant>();
        });
        _mapper = config.CreateMapper();

        _service = new ApplicantService(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllApplicantsAsync_ShouldReturnAllApplicants()
    {
        
        var applicants = new List<Applicant>
        {
            new() { ID = 1, Name = "Ahmed", FamilyName = "Alaa", Address = "Abbas",
                    CountryOfOrigin = "USA", EmailAddress = "Ahmed@test.com", Age = 25, Hired = false },
            new() { ID = 2, Name = "Alaa", FamilyName = "Alallah", Address = "Abbas",
                    CountryOfOrigin = "Egypt", EmailAddress = "Abbas@test.com", Age = 30, Hired = true }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(applicants);

        
        var result = await _service.GetAllApplicantsAsync();

        
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Message.Should().Be("Applicants retrieved successfully");
    }

    [Fact]
    public async Task GetApplicantByIdAsync_WithValidId_ShouldReturnApplicant()
    {
        
        var applicant = new Applicant
        {
            ID = 1,
            Name = "Ahmed",
            FamilyName = "Alaa",
            Address = "Asbbas el Akkad St",
            CountryOfOrigin = "EGypt",
            EmailAddress = "Ahmed@gmail.com",
            Age = 25,
            Hired = false
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(applicant);

        
        var result = await _service.GetApplicantByIdAsync(1);

        
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.ID.Should().Be(1);
        result.Data.Name.Should().Be("John");
    }

    [Fact]
    public async Task CreateApplicantAsync_WithValidData_ShouldCreateApplicant()
    {
        
        var createDto = new CreateApplicantDto
        {
            Name = "Ahmed",
            FamilyName = "Alaa",
            Address = "Asbbas el Akkad St",
            CountryOfOrigin = "EGypt",
            EmailAddress = "Ahmed@gmail.com",
            Age = 25
        };

        var createdApplicant = new Applicant
        {
            ID = 1,
            Name = createDto.Name,
            FamilyName = createDto.FamilyName,
            Address = createDto.Address,
            CountryOfOrigin = createDto.CountryOfOrigin,
            EmailAddress = createDto.EmailAddress,
            Age = createDto.Age,
            Hired = false
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Applicant>())).ReturnsAsync(createdApplicant);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        
        var result = await _service.CreateApplicantAsync(createDto);

        
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("John");
        result.Message.Should().Be("Applicant created successfully");

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Applicant>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateApplicantAsync_WithoutHiredValue_ShouldDefaultToFalse()
    {
        
        var createDto = new CreateApplicantDto
        {
            Name = "Ahmed",
            FamilyName = "Alaa",
            Address = "Asbbas el Akkad St",
            CountryOfOrigin = "EGypt",
            EmailAddress = "Ahmed@gmail.com",
            Age = 25,
            Hired = null 
        };

        var createdApplicant = new Applicant
        {
            ID = 1,
            Name = createDto.Name,
            FamilyName = createDto.FamilyName,
            Address = createDto.Address,
            CountryOfOrigin = createDto.CountryOfOrigin,
            EmailAddress = createDto.EmailAddress,
            Age = createDto.Age,
            Hired = false
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Applicant>())).ReturnsAsync(createdApplicant);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        
        var result = await _service.CreateApplicantAsync(createDto);

        
        result.Data!.Hired.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateApplicantAsync_WithValidData_ShouldUpdateApplicant()
    {
        
        var existingApplicant = new Applicant
        {
            ID = 1,
            Name = "Ahmed",
            FamilyName = "Alaa",
            Address = "Asbbas el Akkad St",
            CountryOfOrigin = "EGypt",
            EmailAddress = "Ahmed@gmail.com",
            Age = 25,
            Hired = false
        };

        var updateDto = new ApplicantDto
        {
            Name = "Ahmed Updated",
            FamilyName = "Alaa Updated",
            Address = "Asbbas el Akkad St",
            CountryOfOrigin = "EGypt",
            EmailAddress = "Ahmed@gmail.com",
            Age = 26,
            Hired = true
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingApplicant);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Applicant>())).ReturnsAsync(existingApplicant);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        
        var result = await _service.UpdateApplicantAsync(1, updateDto);

        
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Message.Should().Be("Applicant updated successfully");

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Applicant>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteApplicantAsync_WithValidId_ShouldDeleteApplicant()
    {
        _repositoryMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _service.DeleteApplicantAsync(1);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.Message.Should().Be("Applicant deleted successfully");

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

}