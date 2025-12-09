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
    
            // --- LINTER BAIT BELOW ---
    
            // Unused variable
            var unusedValue = 123;
            
            // Dead code + unreachable statement
            if (false)
            {
                Console.WriteLine("This will never run");
            }
            
            // Empty catch block
            try
            {
                throw new InvalidOperationException("boom");
            }
            catch
            {
            }
            
            // Async method called without await (generates warning)
            static async Task<string> FetchDataAsync()
            {
                await Task.Delay(10);
                return "data";
            }
            var result = FetchDataAsync(); // not awaited
            
            // Magic number + unused private method
            static int DoSomething(int x) => x * 42;
            DoSomething(7);
            
            // --- END LINTER BAIT ---
    
    
            app.Run();
        }
    }
