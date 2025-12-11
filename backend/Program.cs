    using Dapper;
    using FluentValidation;
    using Newtonsoft.Json;
    using Polly;
    using Serilog;
    
    namespace SbomDemo.Api;
    
    record User(string Name, string Email);
    
    class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
    
            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .MinimumLevel.Information());
    
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
    
            var app = builder.Build();
    
            app.UseSwagger();
            app.UseSwaggerUI();
    
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(3, _ => TimeSpan.FromMilliseconds(100));
    
            app.MapGet("/user", () =>
            {
                var user = new User("test user", "test@example.com");
                var json = JsonConvert.SerializeObject(user);
    
                return Results.Ok(json);
            });
    
    
    
            app.Run();
        }
    }
