﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Server.Api.Models;

namespace Restaurant.Server.Api.Data
{
	[ExcludeFromCodeCoverage]
	public class RestaurantDbContext : DbContext
	{
		public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Food> Foods { get; set; }

		public virtual DbSet<Order> Orders { get; set; }

		public virtual DbSet<DailyEating> DailyEatings { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Food>(b =>
			{
				b.HasMany<Favorite>().WithOne().HasForeignKey(x => x.FoodId).IsRequired();
				b.HasMany<OrderItem>().WithOne().HasForeignKey(x => x.FoodId).IsRequired();
			});

			builder.Entity<OrderItem>(b =>
			{
				b.HasKey(oi => new { oi.FoodId, oi.OderId });
				b.ToTable("OrderItems");
			});

			builder.Entity<Favorite>(b =>
			{
				b.HasKey(f => new { f.FoodId, f.UserId });
				b.ToTable("FavoriteFoods");
			});

			builder.Entity<Category>(b =>
			{
				b.HasMany(x => x.Foods).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
				b.ToTable("Categories");
			});
		}
	}
}