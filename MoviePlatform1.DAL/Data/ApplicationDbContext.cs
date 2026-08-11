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
        public DbSet<ActorTranslation> ActorTranslations { get; set; }


        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<MovieTranslation>movieTranslations{ get; set; }

        public DbSet<MovieCategory>MovieCategories { get; set; }
        public DbSet<MovieCategory>MovieActors { get; set; }    
         public DbSet<Review> Reviews { get; set; }
        public DbSet<UserMovieAccess>userMovieAccesses { get; set; }

        public DbSet<Cart>Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }




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
            builder.Entity<Category>().HasOne(p => p.CreateBy).WithMany().
           HasForeignKey(p => p.CreatedById).OnDelete(DeleteBehavior.Restrict);

            //ممنوع نحذف اليوزر اذا كان  تابع اله كاتيجوري او برودكت
            builder.Entity<Category>().HasOne(p => p.UpdateById).WithMany().
                HasForeignKey(p => p.UpdatedById).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Movie>().HasOne(p => p.CreateBy).WithMany().
               HasForeignKey(p => p.CreatedById).OnDelete(DeleteBehavior.Restrict);
            //ممنوع نحذف اليوزر اذا كان  تابع اله كاتيجوري او برودكت
            builder.Entity<Movie>().HasOne(p => p.UpdateById).WithMany().
                HasForeignKey(p => p.UpdatedById).OnDelete(DeleteBehavior.Restrict);

         
        }
        //بدنا نعمل اوفر رايد عليها ونضيف اشياء only that extend from AuditEntity
        public override Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditEntity>();

            var currentUserId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdateddOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}

