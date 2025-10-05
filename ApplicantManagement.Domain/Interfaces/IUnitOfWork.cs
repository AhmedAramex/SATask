using ApplicantManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicantManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Applicant> Applicants { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}