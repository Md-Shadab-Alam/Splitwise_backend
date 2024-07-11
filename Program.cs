using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
            "Enter 'Bearer' [space and then your token in the text input below. \r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
});
});

builder.Services.AddDbContext<SplitwiseDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SplitwiseConnectionString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SplitwiseDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(u =>
{
    u.RequireHttpsMetadata = false;
    u.SaveToken = true;
    u.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});


//Adding Services
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<RabbitMQService>();

//builder.Services.AddHostedService<RabbitMQConsumerService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","My API v1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OktaAuthWebBackend", Version = "v1" });
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        Type = SecuritySchemeType.OAuth2,
//        Flows = new OpenApiOAuthFlows
//        {
//            AuthorizationCode = new OpenApiOAuthFlow
//            {
//                AuthorizationUrl = new Uri($"{builder.Configuration["Okta:OktaDomain"]}/oauth2/default/v1/authorize"),
//                //TokenUrl = new Uri($"{builder.Configuration["Okta:OktaDomain"]}/oauth2/default/v1/token"),
//                Scopes = new Dictionary<string, string>
//                {
//                    //{"api_scope", "Access to API" }
//                    {"openid","OpenID Connect scope"},
//                    {"profile","Profile scope" },
//                    {"email", "Email scope" }
//                }
//            }
//        }
//    });
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id= "oauth2"
//                }
//            },
//            new[] {"openid","profile","email"}
//        }
//    });
//}
////    options =>
////{
////    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
////    {
////        Description=
////            "JWT Authorization header using the Bearer scheme. \r\n\r\n"+
////            "Enter 'Bearer' [space and then your token in the text input below. \r\n\r\n" +
////            "Example: \"Bearer 12345abcdef\"",
////        Name="Authorization",
////        In=Microsoft.OpenApi.Models.ParameterLocation.Header,
////        Scheme = JwtBearerDefaults.AuthenticationScheme 
////    });
////    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
////    {
////        {
////            new OpenApiSecurityScheme
////            {
////                Reference = new OpenApiReference
////                    {
////                        Type = ReferenceType.SecurityScheme,
////                        Id = "Bearer"
////                    },
////                Scheme = "oauth2",
////                Name = "Bearer",
////                In = ParameterLocation.Header
////            },
////            new List<string>()
////        }
////}
////);
////}
//);

//builder.Services.AddDbContext<SplitwiseDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("SplitwiseConnectionString")));
////builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SplitwiseDbContext>();

////builder.Services.Configure<IdentityOptions>(options =>
////{
////    options.Password.RequireDigit = false;
////    options.Password.RequireLowercase = false;
////    options.Password.RequireUppercase = false;
////    options.Password.RequiredLength = 1;
////    options.Password.RequireNonAlphanumeric = false;
////});

////var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

////builder.Services.AddAuthentication(options =>
//////{
//////    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
//////    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
//////})
//////    .AddOktaWebApi(new OktaWebApiOptions
//////    {
//////        OktaDomain = builder.Configuration["Okta:OktaDomain"],
//////        Audience = "api://default"
//////    });


////{
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
////}).AddJwtBearer(u =>
////{
////    u.RequireHttpsMetadata = false;
////    u.SaveToken = true;
////    u.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuerSigningKey = true,
////        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
////        ValidateIssuer = false,
////        ValidateAudience = false,
////    };
////});


////Adding Services
//builder.Services.AddScoped<IExpenseService, ExpenseService>();
//builder.Services.AddScoped<IUsersService, UsersService>();
//builder.Services.AddScoped<IGroupService, GroupService>();
//builder.Services.AddScoped<IBalanceService, BalanceService>();

//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy("CorsPolicy", policy =>
//    {
//        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
//    });
//});




//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
//   // options.DefaultSignInScheme = OktaDefaults.MvcAuthenticationScheme;
//    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
//})
//    //.AddJwtBearer(options =>
//    //{
//    //    options.Authority = builder.Configuration["Okta:OktaDomain"] + "/oauth2/default";
//    //    options.Audience = "api://default";
//    //});

//    .AddOktaWebApi(new Okta.AspNetCore.OktaWebApiOptions
//    {
//        OktaDomain = builder.Configuration["Okta:OktaDomain"],
//        Audience = "api://default"
//        //ClientId = builder.Configuration["Okta:ClientId"],
//        //ClientSecret = builder.Configuration["Okta:ClientSecret"]
//    });

//builder.Services.AddAuthorization();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OktaAuthWebBackend V1");
//        c.OAuthClientId(builder.Configuration["Okta:ClientId"]);
//       // c.OAuthClientSecret(builder.Configuration["Okta:ClientSecret"]);
//        c.OAuthUsePkce();
//    }

//        );
// //   app.UseDeveloperExceptionPage();
//}

////app.UseRouting();

//app.UseHttpsRedirection();

//app.UseCors("CorsPolicy");

//app.UseAuthentication();

//app.UseAuthorization();

//// app.UseEndpoints(endpoints =>
//// {
////     endpoints.MapControllers();
//// });
//app.MapControllers();

//app.Run();