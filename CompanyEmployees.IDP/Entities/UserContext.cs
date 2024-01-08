using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.IDP.Entities;

public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<User>(options);
