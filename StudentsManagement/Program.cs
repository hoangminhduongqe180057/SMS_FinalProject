using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsManagement.Client.Pages;
using StudentsManagement.Client.Repository;
using StudentsManagement.Client.Services;
using StudentsManagement.Components;
using StudentsManagement.Components.Account;
using StudentsManagement.Data;
using StudentsManagement.Services;
using StudentsManagement.Shared.StudentRepository;
using StudentsManagement.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity.UI.Services;
using StudentsManagement.Utilities;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System;


var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
// Add services to the container.
//builder.Services.AddRazorComponents()  // vừa comment
//    .AddInteractiveServerComponents()
//    .AddInteractiveWebAssemblyComponents();
builder.Services.AddRazorComponents().AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 64 * 1024);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddRazorPages(); // Thêm dòng này để đăng ký Razor Pages (1)
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10); // Adjust as needed
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(10); // JS Interop call timeout
    });
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>                  // vừa comment (4)
//    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddDbContext<ApplicationDbContext>(options =>                
   options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1024 * 1024 * 15; // Max 15MB
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ISystemCodeRepository, SystemCodeRepository>();
builder.Services.AddScoped<ISystemCodeDetailRepository, SystemCodeDetailRepository>();
builder.Services.AddScoped<IParentRepository, ParentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IBookIssuanceRepository, BookIssuanceRepository>();
builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
builder.Services.AddScoped<IPaginationRepository, PaginationRepository>();
builder.Services.AddScoped<IHostelRepository, HostelRepository>();
builder.Services.AddScoped<IHostelRoomRepository, HostelRoomRepository>();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IComplaintNoteRepository, ComplaintNoteRepository>();
builder.Services.AddScoped<SystemCodeDetailService>();
builder.Services.AddScoped<SystemCodeDetailRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<HostelService>();
builder.Services.AddScoped<HostelRoomService>();
builder.Services.AddScoped<AdminMessageService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();


builder.Services.AddTransient<IEmailSender, AuthMessageSender>(); // send Email

// Đăng ký RoleManager
builder.Services.AddScoped<RoleManager<ApplicationRole>>();

builder.Services.AddScoped(http => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddress").Value!)
});


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax; // mới thay đổi None -> Lax
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.Cookie.SameSite = SameSiteMode.None;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures cookies are only sent over HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax;

    // Adjust the expiry based on your requirements
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Idle timeout of 30 minutes

    // Require re-login after browser is closed by not issuing a persistent cookie
    options.SlidingExpiration = true; // Reset expiration time if there's activity during the session
    options.LoginPath = "/Account/Login"; // Path to login page
});

builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, x => x.Cookie.SameSite = SameSiteMode.None);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication();
builder.Services.AddApiAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseStaticFiles();

app.UseRouting();  /*vừa thêm (3)*/

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseCors("AllowAll");


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()   /*vừa commnet 31/10 (1)*/
    //.AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.MapControllers();
app.MapBlazorHub(config =>
{
    config.CloseOnAuthenticationExpiration = true;
}).WithOrder(-1);
app.MapAdditionalIdentityEndpoints();
app.MapFallbackToPage("/_Host");

// Add additional endpoints required by the Identity /Account Razor components.

app.Run();