using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviePlatform1.DAL.Models;
using System.Security.Claims;

namespace MoviePlatform1.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }

        public DbSet<MovieCategory>MovieCategories { get; set; }
        public DbSet<MovieCategory>MovieActors { get; set; }    
         public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
            : base(options) { 
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
        }
        //بدنا نعمل اوفر رايد عليها ونضيف اشياء only that extend from AuditEntity
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //انا بدي اياك تجيب التغيرات الي صارت بالكلاسات الي الها علاقة بهاد المودلز
            var entries = ChangeTracker.Entries<AuditEntity>();
            //اليوزر موجود معنا فقط بالبزنتيشن لاير هون لا فكيف بدنا نجيبه؟ بالبرزنتيشن عنا بعض الاشياء الموجودة زي httpcontextاشي بمسك الريكوست تبعتي من ضمنها بقدر اجيب اليوزر
            var currentUserId=_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }
                if(entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdateddOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}

