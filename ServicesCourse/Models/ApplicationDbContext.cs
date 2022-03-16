using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServicesCourse.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() : base()
        {

        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Sex> Sex { get; set; }

        public DbSet<Service> Service { get; set; }
        public DbSet<Subsection> Subsection { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<History> History { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SexConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new SubsectionConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());
        }

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(p => p.Login);
                builder.Property(p => p.Password).IsRequired().HasMaxLength(20);
                builder.Property(p => p.UserTypeId).IsRequired();
                builder.Property(p => p.ActivityStatus).IsRequired();

                builder.HasOne(u => u.UserProfile)
                    .WithOne(p => p.User)
                    .HasForeignKey<UserProfile>(p => p.Login);

                builder.HasData(new User[]
                {
                    new User { Login = "admin", Password = "admin", UserTypeId = 1, ActivityStatus = true},
                    new User { Login = "user", Password = "user", UserTypeId = 2, ActivityStatus = true}
                });
            }
        }

        public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
        {
            public void Configure(EntityTypeBuilder<UserType> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.UserTypeName).IsRequired();
                builder.Property(p => p.UserTypeName).HasConversion<string>();

                builder.HasData(new UserType[]
                {
                    new UserType { Id = 1, UserTypeName = UserTypes.Администратор },
                    new UserType { Id = 2, UserTypeName = UserTypes.Пользователь }
                });
            }
        }

        public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
        {
            public void Configure(EntityTypeBuilder<UserProfile> builder)
            {
                builder.HasKey(p => p.Login);
                builder.Property(p => p.BirthDate).HasColumnType("date");
                
                builder.HasData(new UserProfile[]
                {
                    new UserProfile { Login = "admin" },
                    new UserProfile { Login = "user" }
                });
            }
        }

        public class SexConfiguration : IEntityTypeConfiguration<Sex>
        {
            public void Configure(EntityTypeBuilder<Sex> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.SexName).IsRequired().HasConversion<string>();

                builder.HasData(new Sex[]
                {
                    new Sex {  Id = 1, SexName = SexNames.Мужской },
                    new Sex {  Id = 2, SexName = SexNames.Женский }
                });
            }
        }

        public class SectionConfiguration : IEntityTypeConfiguration<Section>
        {
            public void Configure(EntityTypeBuilder<Section> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.SectionName).IsRequired();


                builder.HasData(new Section[]
                {
                    new Section { Id = 1, SectionName = "Общее" }
                });
            }
        }

        public class SubsectionConfiguration : IEntityTypeConfiguration<Subsection>
        {
            public void Configure(EntityTypeBuilder<Subsection> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.SubscetionName).IsRequired();
                builder.Property(p => p.SectionId).IsRequired();

                builder.HasData(new Subsection[]
                {
                    new Subsection { Id = 1, SectionId = 1, SubscetionName = "Общее" }
                });

            }
        }

        public class ServiceConfiguration : IEntityTypeConfiguration<Service>
        {
            public void Configure(EntityTypeBuilder<Service> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.ServiceName).IsRequired();
                builder.Property(p => p.ActivityStatus).IsRequired();
                builder.Property(p => p.SubsectionId).IsRequired();

                builder.HasMany(u => u.Users)
                    .WithMany(p => p.Services)
                    .UsingEntity<History>(
                        j => j
                        .HasOne(pt => pt.User)
                        .WithMany(p => p.HistoryRecords)
                        .HasForeignKey(pt => pt.Login),
                        j => j
                        .HasOne(pt => pt.Service)
                        .WithMany(p => p.HistoryRecords)
                        .HasForeignKey(pt => pt.ServiceId),
                        j =>
                        {
                            j.Property(pt => pt.AccessTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        });
            }
        }

        public class HistoryConfiguration : IEntityTypeConfiguration<History>
        {
            public void Configure(EntityTypeBuilder<History> builder)
            {
                builder.HasKey(p => new { p.Login, p.AccessTime });
            }
        }
    }
}
