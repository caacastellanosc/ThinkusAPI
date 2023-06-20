using Amazon.Auth.AccessControlPolicy;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class InsuranceDbContext : DbContext
{
    public DbSet<Poliza> Polizas { get; set; }

}
