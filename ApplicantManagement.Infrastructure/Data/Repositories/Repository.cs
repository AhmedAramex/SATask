using ApplicantManagement.Domain.Entities;
using ApplicantManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApplicantManagement.Infrastructure.Data.Repositories;

public class Repository : IRepository<Applicant>
{


    protected readonly ApplicantDbContext _context;

    public Repository(ApplicantDbContext context)
    {
        _context = context;
    }

    public Task<Applicant> AddAsync(Applicant entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Applicant>> FindAsync(Expression<Func<Applicant, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Applicant>> GetAllAsync()
    {
        return _context.Applicants.ToListAsync().ContinueWith(t => (IEnumerable<Applicant>)t.Result);
    }

    public Task<Applicant?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Applicant> UpdateAsync(Applicant entity)
    {
        throw new NotImplementedException();
    }
}
