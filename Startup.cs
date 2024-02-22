using AutoMapper;
using EasyProctor.Exam.Config;
using TheCats.Infrastructure.Connection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheCats.Config;
using TheCats.Exam.Config;
using TheCats.Injections;
using TheCats.Config.Injections;

namespace EasyProctor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string MyAllowSpecificOrigins = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Token:secret").Value);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };
            });

            ConfigureDatabase(services);

            services.ClientConfig(Configuration);
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddGlobalExceptionHandlerMiddleware();
            services.ResolveDependencies();
            services.ResolveSwagger();
            services.JsonSerializationConfig();
            services.AddAutoMapper(typeof(Startup));
            services.AddSignalR();

            //Microsservice Injections
            services.ResolveDependenciesUsuario();
            services.ResolveDependenciesOpenAi();
            services.ResolveDependenciesAuth();
            services.ResolveDependenciesConversation();
            services.ResolveDependenciesAssistant();

            services.AddHttpClient("myClient", client => client.Timeout = TimeSpan.FromMinutes(10));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwaggerConfiguration();
            app.UseGlobalExceptionHandlerMiddleware();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
        }


        private void ConfigureDatabase(IServiceCollection services)
        {
            services.Configure<DatabaseSettings>(
               Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
               sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        }
    }
}