
using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;


GlobalVariable.ConnString = builder.Configuration.GetConnectionString("myconn");

//builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy

            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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


//2028 08 21 <-
////Gravy Foods/ POS ingegration <-

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

app.MapGet("/health", () => Results.Ok("Healthy"));
app.MapGet("/test-cors", (HttpContext context) =>
{
    return Results.Ok("CORS works!");
}).RequireCors("AllowFrontend");


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

app.UseCors("AllowFrontend");  // must be before auth & MapControllers

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

app.Run();