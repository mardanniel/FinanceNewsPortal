﻿using FinanceNewsPortal.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceNewsPortal.Web.Data
{
    public class FinanceNewsPortalDbContext : IdentityDbContext<ApplicationUser>
    {
        private ILogger _logger { get; }
        private IConfiguration _appConfig { get; }

        public FinanceNewsPortalDbContext(
            ILogger<FinanceNewsPortalDbContext> logger,
            IConfiguration appConfig)
        {
            this._logger = logger;
            this._appConfig = appConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var developmentConnection = this._appConfig.GetConnectionString("DefaultConnection");
            var server = this._appConfig.GetConnectionString("Server");
            var db = this._appConfig.GetConnectionString("DB");
            var username = this._appConfig.GetConnectionString("UserName");
            var password = this._appConfig.GetConnectionString("Password");

            string connectionString = developmentConnection != null
                                    ? developmentConnection 
                                    : $"Server={server};Database={db};User Id={username};Password={password};MultipleActiveResultSets=true";

            optionsBuilder
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.NewsArticles)
                .WithOne(n => n.Author)
                .HasForeignKey(n => n.ApplicationUserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Company>()
                .HasMany(c => c.Staffs)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.InvokeIdentityRoleSeed();
            builder.InvokeCompanySeed();
            builder.InvokeUsersSeed();
            builder.InvokeNewsArticleTags();
            builder.InvokeIdentityUserRoleSeed();
            builder.InvokeNewsArticleSeed();

            base.OnModelCreating(builder);
        }

        public DbSet<NewsArticle> NewsArticle { get; set; }
        public DbSet<NewsArticleType> NewsArticleTypes { get; set; }
        public DbSet<NewsArticleTag> NewsArticleTags { get; set; }
        public DbSet<Company> Company { get; set; }
    }
}
