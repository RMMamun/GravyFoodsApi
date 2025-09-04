
using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

GlobalVariable.ConnString = builder.Configuration.GetConnectionString("myconn");

builder.Services.AddCors();

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

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

//app.UseCors(builder =>
//{
//    builder
//    .WithOrigins("http://127.0.0.1:5500/Classthirteen.html", "http://127.0.0.1:5500/MasjidInfo.html")
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader();
//});


// Serve ProductImages as static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "ProductImages")),
    RequestPath = $"/{GlobalVariable.StaticFileDir}"
});
//app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();    //2028 08 21

app.MapControllers();

app.Run();