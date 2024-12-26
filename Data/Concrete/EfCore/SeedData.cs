using System;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BlogApp.Data.Concrete.EfCore;

public static class SeedData
{
    public static void TestVerileriniDoldur(IApplicationBuilder app)
    {

        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();


        if (context != null)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Tag { Text = "web programlama" },
                    new Tag { Text = "backend" },
                    new Tag { Text = "frontend" },
                    new Tag { Text = "fullstack" },
                    new Tag { Text = "php" }
                );
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { UserName = "emrahaltuner" },
                    new User { UserName = "nanotanitim" }
                );
                context.SaveChanges();
            }
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                    new Post
                    {
                        Title = "Asp.Net Core",
                        Content = "Asp.Net Core dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-10),
                        Tags = context.Tags.Take(3).ToList(),
                        UserId = 1
                    },
                                            new Post
                                            {
                                                Title = "Javascript Kursu",
                                                Content = "Javascript dersleri",
                                                IsActive = true,
                                                PublishedOn = DateTime.Now.AddDays(-20),
                                                Tags = context.Tags.Take(2).ToList(),
                                                UserId = 2
                                            },
                                            new Post
                                            {
                                                Title = "React.Js Kursu",
                                                Content = "React.Js dersleri",
                                                IsActive = false,
                                                PublishedOn = DateTime.Now.AddDays(-80),
                                                Tags = context.Tags.Take(4).ToList(),
                                                UserId = 1
                                            }
                    );
                context.SaveChanges();
            }
        }


    }
}
