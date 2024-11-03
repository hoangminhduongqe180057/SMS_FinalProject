using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentsManagement.Client;
using StudentsManagement.Client.Repository;
using StudentsManagement.Client.Services;
using StudentsManagement.Shared.StudentRepository;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddScoped<IStudentRepository, StudentService>();
builder.Services.AddScoped<ICountryRepository, CountryService>();
builder.Services.AddScoped<ISystemCodeRepository, SystemCodeService>();
builder.Services.AddScoped<ISystemCodeDetailRepository, SystemCodeDetailService>();
builder.Services.AddScoped<IParentRepository, ParentService>();
builder.Services.AddScoped<ITeacherRepository, TeacherService>();
builder.Services.AddScoped<ISubjectRepository, SubjectService>();
builder.Services.AddScoped<IBookRepository, BookService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentService>();
builder.Services.AddScoped<IBookIssuanceRepository, BookIssuanceService>();
builder.Services.AddScoped<IAcademicYearRepository, AcademicYearService>();
builder.Services.AddScoped<IPaginationRepository, PaginationService>();
builder.Services.AddScoped<IHostelRepository, HostelService>();
builder.Services.AddScoped<IHostelRoomRepository, HostelRoomService>();
builder.Services.AddScoped<IComplaintRepository, ComplaintService>();
builder.Services.AddScoped<IComplaintNoteRepository, ComplaintNoteService>();
builder.Services.AddScoped<StudentService>();

builder.Services.AddApiAuthorization(); // Nếu bạn sử dụng Identity Server
builder.Services.AddOptions();
builder.Services.AddScoped(http => new HttpClient // Cho phép sử dụng [Authorize]
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

await builder.Build().RunAsync();
