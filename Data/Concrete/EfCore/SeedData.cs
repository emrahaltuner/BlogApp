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
                    new Tag { Text = "web programlama", Url = "web-programlama" },
                    new Tag { Text = "backend programlama", Url = "backend-programlama" },
                    new Tag { Text = "frontend", Url = "frontend" },
                    new Tag { Text = "fullstack", Url = "fullstack" },
                    new Tag { Text = "php", Url = "php" }
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
                        Url = "Asp-Net-Core",
                        Image = "1.jpg",
                        PublishedOn = DateTime.Now.AddDays(-10),
                        Tags = context.Tags.Take(3).ToList(),
                        UserId = 1
                    },
                                            new Post
                                            {
                                                Title = "Javascript Kursu",
                                                Content = "Javascript dersleri",
                                                IsActive = true,
                                                Url = "javascript-kursu",
                                                Image = "2.jpg",
                                                PublishedOn = DateTime.Now.AddDays(-20),
                                                Tags = context.Tags.Take(2).ToList(),
                                                UserId = 2
                                            },
                                            new Post
                                            {
                                                Title = "React.Js Kursu",
                                                Content = "React.Js dersleri",
                                                IsActive = false,
                                                Url = "react-js-kursu",
                                                Image = "3.jpg",
                                                PublishedOn = DateTime.Now.AddDays(-80),
                                                Tags = context.Tags.Take(4).ToList(),
                                                UserId = 1
                                            },
                                            new Post
                                            {
                                                Title = "Vue Js Kursu",
                                                Content = "Vue.Js dersleri",
                                                IsActive = true,
                                                Url = "vue-js-kursu",
                                                Image = "1.jpg",
                                                PublishedOn = DateTime.Now.AddDays(-40),
                                                Tags = context.Tags.Take(4).ToList(),
                                                UserId = 2
                                            },
                                            new Post
                                            {
                                                Title = "Php Laravel Kursu",
                                                Content = "Php laravel dersleri",
                                                IsActive = false,
                                                Url = "php-laravel-kursu",
                                                Image = "2.jpg",
                                                PublishedOn = DateTime.Now.AddDays(-60),
                                                Tags = context.Tags.Take(4).ToList(),
                                                UserId = 2
                                            },
                                            new Post
                                            {
                                                Title = "Angular Js Kursu",
                                                Content = "Angular Js dersleri",
                                                IsActive = true,
                                                Url = "angular-js-kursu",
                                                Image = "3.jpg",
                                                PublishedOn = DateTime.Now.AddDays(-50),
                                                Tags = context.Tags.Take(4).ToList(),
                                                UserId = 1
                                            }
                    );
                context.SaveChanges();
            }
        }


    }
}
