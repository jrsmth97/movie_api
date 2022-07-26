using System;
using Microsoft.EntityFrameworkCore;
using movie_api.Models;

namespace movie_api.Contexts
{
    public class DBContext : DbContext
    {
        public virtual DbSet<Users> users { get; set; }
        public virtual DbSet<Movies> movies { get; set; }
        public virtual DbSet<MovieSchedules> movieSchedules { get; set; }
        public virtual DbSet<MovieTags> movieTags { get; set; }
        public virtual DbSet<Tags> tags { get; set; }
        public virtual DbSet<Studios> studios { get; set; }
        public virtual DbSet<Orders> orders { get; set; }
        public virtual DbSet<OrderItems> orderItems { get; set; }

        public DBContext(DbContextOptions options) : base(options)
        {
        }

        private static void BuildElementsTable(ModelBuilder modelBuilder)
        {
        //  //   Map entities to tables  
        //    modelBuilder.Entity<Users>().ToTable("users");
        //    modelBuilder.Entity<Movies>().ToTable("movies");
        //    modelBuilder.Entity<MovieSchedules>().ToTable("movie_schedules");
        //    modelBuilder.Entity<MovieTags>().ToTable("movie_tags");
        //    modelBuilder.Entity<Tags>().ToTable("tags");
        //    modelBuilder.Entity<Studios>().ToTable("studios");
        //    modelBuilder.Entity<Orders>().ToTable("orders");
        //    modelBuilder.Entity<OrderItems>().ToTable("order_items");

        //    // Configure Primary Keys  
        //    modelBuilder.Entity<Users>().HasKey(u => u.id).HasName("PK_users");
        //    modelBuilder.Entity<Movies>().HasKey(u => u.id).HasName("PK_movies");
        //    modelBuilder.Entity<MovieSchedules>().HasKey(u => u.id).HasName("PK_movie_schedules");
        //    modelBuilder.Entity<MovieTags>().HasKey(u => u.id).HasName("PK_movie_tags");
        //    modelBuilder.Entity<Tags>().HasKey(u => u.id).HasName("PK_tags");
        //    modelBuilder.Entity<Studios>().HasKey(u => u.id).HasName("PK_studios");
        //    modelBuilder.Entity<Orders>().HasKey(u => u.id).HasName("PK_orders");
        //    modelBuilder.Entity<OrderItems>().HasKey(u => u.id).HasName("PK_order_items");

        //    // Configure columns  
        //    modelBuilder.Entity<Users>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.name).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.email).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.password).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.avatar).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.activation_key).HasColumnType("varchar(255)").IsRequired(false);
        //    modelBuilder.Entity<Users>().Property(u => u.is_admin).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.is_confirmed).HasColumnType("bool").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Users>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<Users>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    modelBuilder.Entity<Movies>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.title).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.overview).HasColumnType("text").IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.poster).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.play_until).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Movies>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<Movies>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);
           
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.movie_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.studio_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.start_time).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.end_time).HasColumnType("varchar(255)").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.price).HasColumnType("double").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.date).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<MovieSchedules>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    modelBuilder.Entity<MovieTags>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<MovieTags>().Property(u => u.movie_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<MovieTags>().Property(u => u.tag_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<MovieTags>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<MovieTags>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<MovieTags>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    modelBuilder.Entity<Tags>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<Tags>().Property(u => u.name).HasColumnType("varchar(100)").IsRequired();
        //    modelBuilder.Entity<Tags>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Tags>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<Tags>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    modelBuilder.Entity<Studios>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<Studios>().Property(u => u.studio_number).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<Studios>().Property(u => u.seat_capacity).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<Studios>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Studios>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<Studios>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    modelBuilder.Entity<Orders>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<Orders>().Property(u => u.user_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<Orders>().Property(u => u.payment_method).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<Orders>().Property(u => u.total_item_price).HasColumnType("double").IsRequired();
        //    modelBuilder.Entity<Orders>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<Orders>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<Orders>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);


        //    modelBuilder.Entity<OrderItems>().Property(u => u.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.order_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.movie_schedule_id).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.qty).HasColumnType("int").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.price).HasColumnType("double").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.sub_total_price).HasColumnType("double").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.created_at).HasColumnType("datetime").IsRequired();
        //    modelBuilder.Entity<OrderItems>().Property(u => u.updated_at).HasColumnType("datetime").IsRequired(false);
        //    modelBuilder.Entity<OrderItems>().Property(u => u.deleted_at).HasColumnType("datetime").IsRequired(false);

        //    // Configure relationships  
        //    modelBuilder.Entity<MovieSchedules>().HasOne<Studios>().WithMany().HasPrincipalKey(s => s.id).HasForeignKey(ms => ms.studio_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_movie_schedules_studios");

        //    modelBuilder.Entity<MovieSchedules>().HasOne<Movies>().WithMany().HasPrincipalKey(m => m.id).HasForeignKey(ms => ms.movie_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_movie_schedules_movies");

        //    modelBuilder.Entity<MovieTags>().HasOne<Movies>().WithMany().HasPrincipalKey(m => m.id).HasForeignKey(mt => mt.movie_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_movie_tags_movies");

        //    modelBuilder.Entity<MovieTags>().HasOne<Tags>().WithMany().HasPrincipalKey(t => t.id).HasForeignKey(mt => mt.tag_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_movie_tags_tags");

        //    modelBuilder.Entity<Orders>().HasOne<Users>().WithMany().HasPrincipalKey(u => u.id).HasForeignKey(o => o.user_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_orders_users");

        //    modelBuilder.Entity<OrderItems>().HasOne<Orders>().WithMany().HasPrincipalKey(o => o.id).HasForeignKey(oi => oi.order_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_order_items_orders");

        //    modelBuilder.Entity<OrderItems>().HasOne<MovieSchedules>().WithMany().HasPrincipalKey(ms => ms.id).HasForeignKey(oi => oi.movie_schedule_id).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_order_items_movie_schedules");

        //    // seeding datas
        //    modelBuilder.Entity<Tags>().HasData(
        //        new { id = 1, name = "Scifi", created_at = DateTime.Parse("2022-07-22T00:00:00")},
        //        new { id = 2, name = "Fantasy", created_at = DateTime.Parse("2022-07-22T00:00:00")}
        //    );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildElementsTable(modelBuilder);
        }

    }
}