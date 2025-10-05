namespace ApplicantManagement.Application.DTOs;

public class ApplicantDto
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CountryOfOrigin { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool Hired { get; set; }
}