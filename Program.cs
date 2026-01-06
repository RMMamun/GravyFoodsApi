
using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.Mappings;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;



if (builder.Environment.IsDevelopment())
{
    GlobalVariable.ConnString = builder.Configuration.GetConnectionString("serverconn");
    //GlobalVariable.ConnString = builder.Configuration.GetConnectionString("laptopconn");
    //GlobalVariable.ConnString = builder.Configuration.GetConnectionString("desktopconn");
}
else
{
    GlobalVariable.ConnString = builder.Configuration.GetConnectionString("serverconn");
}




builder.Services.AddCors();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", policy =>
//    {
//        policy
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials()
//            .AllowAnyOrigin();

//        //.WithOrigins(
//        //"https://goooget.com",
//        //"https://gravyfoods.goooget.com",
//        //"http://gravyfoods.goooget.com",
//        //"https://localhost:7065") // frontend origin


//    });
//});

//              .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader();

//.WithOrigins(
//"https://gravyfoods.goooget.com",
//"http://gravyfoods.goooget.com",

//"https://localhost:7065") // frontend origin

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<TodoContext>(opt =>
//    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDbContext<MasjidDBContext>(options =>
options.UseSqlServer(GlobalVariable.ConnString,
    builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(60), null)));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<IPOSSubscription, POSSubscriptionService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
////Gravy Foods/ POS ingegration ->
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<IBrandRepository, BrandService>();
builder.Services.AddScoped<IProductUnitRepository, ProductUnitService>();
builder.Services.AddScoped<IProductCategoryRepository,  ProductCategoryService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageService>();
builder.Services.AddScoped<ICustomerInfoService, CustomerInfoService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IExpenseHeadService, ExpenseHeadService>();
builder.Services.AddScoped<IExpenseInfoService, ExpenseInfoService>();
builder.Services.AddScoped<ISupplierRepository, SupplierService>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseService>();
builder.Services.AddScoped<INavMenuRepository, NavMenuService>();
builder.Services.AddScoped<IUserWiseMenuItemRepository, UserWiseMenuItemService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<ICompanyInfoService, CompanyInfoService>();
builder.Services.AddScoped<IBranchInfoRepository,  BranchInfoService>();

builder.Services.AddScoped<IProductStockRepository, ProductStockService>();
builder.Services.AddScoped<IUnitConversionRepository, UnitConversionService>();
builder.Services.AddScoped<IProductSerialRepository, ProductSerialService>();

builder.Services.AddScoped<IWarehouseRepository, WarehouseService>();
builder.Services.AddScoped<IAppOptionsRepository, AppOptionsService>();





//2028 08 21 <-
//Gravy Foods/ POS ingegration <-

////2025 09 26 ->
//// Add authentication 
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

//builder.Services.AddAuthorization();
////2025 09 26 <-

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GravyFoodsApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GravyFoodsApi v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// in general

//app.MapGet("/health", () => Results.Ok("Healthy"));
//app.MapGet("/test-cors", (HttpContext context) =>
//{
//    return Results.Ok("CORS works!");
//}).RequireCors("AllowFrontend");


// Diagnostic endpoint - add temporarily near the end of Program.cs before app.Run()
//https://localhost:7065/dbg/menu-query
//app.MapGet("/dbg/menu-query", async (MasjidDBContext db) =>
//{
//    // Use exact inputs from your failing case
//    var userId = "Mamun";
//    var companyId = "1";
//    var branchId = "1";

//    var provider = db.Database.ProviderName;
//    var assignedMenuIds = await db.UserWiseMenuAssignment
//        .Where(a => a.UserId == userId && a.CompanyId == companyId && a.BranchId == branchId)
//        .Select(a => a.MenuId)
//        .ToListAsync();

//    var q = db.NavMenuItems
//        .Where(m => assignedMenuIds.Contains(m.MenuId) && m.IsActive)
//        .OrderBy(m => m.DisplayOrder);

//    // ToQueryString is safe to call in server code to inspect generated SQL text
//    var sql = q.ToQueryString();

//    return Results.Ok(new { provider, assignedMenuIdsCount = assignedMenuIds.Count, sql });
//});




app.UseCors("AllowFrontend");  // must be before auth & MapControllers
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseRouting();



// Serve ProductImages as static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "ProductImages")),
    RequestPath = $"/{GlobalVariable.StaticFileDir}"
});


//app.UseCors(builder =>
//{
//    builder
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader();
//});

//app.UseCors("AllowFrontend");  // must be before auth & MapControllers

//app.UseCors(builder =>
//{
//    builder
//    .WithOrigins("http://127.0.0.1:5500/Classthirteen.html", "http://127.0.0.1:5500/MasjidInfo.html")
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader();
//});


app.UseAuthentication();    //2025 08 21
app.UseAuthorization();

app.MapControllers();

////2025 09 26 ->
//// Login endpoint -> generate token
//app.MapPost("/login", async (LoginRequest login, MasjidDBContext db) =>
//{
//    var user = await db.UserInfo.FirstOrDefaultAsync(u => u.UserName == login.Username);

//    if (user == null )
//        return Results.BadRequest(new { error = "Invalid user name or password" });

//    if (login.Username == user.UserName && login.Password == user.Password)
//    {
//        var claims = new[]
//        {
//            new System.Security.Claims.Claim("name", login.Username),
//            new System.Security.Claims.Claim("role", "Admin")
//        };

//        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
//            issuer: builder.Configuration["Jwt:Issuer"],
//            audience: builder.Configuration["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddHours(1),
//            signingCredentials: new SigningCredentials(
//                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//                SecurityAlgorithms.HmacSha256)
//        );

//        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

//        return Results.Ok(new { token = tokenString });
//    }
//    return Results.Unauthorized();
//});

//// Protected endpoint
//app.MapGet("/profile", [Authorize] (HttpContext context) =>
//{
//    var name = context.User.Identity?.Name ?? "Unknown";
//    return Results.Ok(new { message = $"Hello {name}, this is a protected resource!" });
//});
////2025 09 26 <-


app.Run();