using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using opensis.core.School.Interfaces;
using opensis.core.School.Services;
using opensis.core.User.Services;
using opensis.core.User.Interfaces;
using opensis.data.Factory;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.Repository;
using opensisAPI.TenantDbMappingMiddleWare;
using Swashbuckle.AspNetCore.Swagger;
using opensis.core.Common.Interfaces;
using opensis.core.Common.Services;
using opensis.core.GradeLevel.Interfaces;
using opensis.core.GradeLevel.Services;
using opensis.core.Room.Interfaces;
using opensis.core.Room.Services;
using opensis.core.Section.Interfaces;
using opensis.core.Section.Services;
using opensis.core.MarkingPeriods.Interfaces;
using opensis.core.MarkingPeriods.Services;
using opensis.core.SchoolPeriod.Interfaces;
using opensis.core.SchoolPeriod.Services;
using opensis.core.Calender.Services;
using opensis.core.Calender.Interfaces;
using opensis.core.CalendarEvents.Interfaces;
using opensis.core.CalendarEvents.Services;
using opensis.core.AttendanceCode.Interfaces;
using opensis.core.AttendanceCode.Services;
using opensis.core.StudentEnrollmentCodes.Interfaces;
using opensis.core.StudentEnrollmentCodes.Services;
using opensis.core.Student.Interfaces;
using opensis.core.Student.Services;
using opensis.core.CustomField.Interfaces;
using opensis.core.CustomField.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using opensis.core.ParentInfo.Interfaces;
using opensis.core.ParentInfo.Services;
using opensis.core.Staff.Services;
using opensis.core.Staff.Interfaces;
using opensis.core.CourseManager.Services;
using opensis.core.CourseManager.Interfaces;
using opensis.core.Period.Interfaces;
using opensis.core.Period.Services;
using opensis.core.Grade.Interfaces;
using opensis.core.Grade.Services;
using opensis.core.RoleBasedAccess.Interfaces;
using opensis.core.RoleBasedAccess.Services;
using opensis.core.StudentSchedule.Interfaces;
using opensis.core.StudentSchedule.Services;
using opensis.core.StaffSchedule.Interfaces;
using opensis.core.StaffSchedule.Services;
using opensis.core.StudentAttendances.Interfaces;
using opensis.core.StudentAttendances.Services;
using opensis.core.InputFinalGrade.Services;
using opensis.core.InputFinalGrade.Interfaces;
using opensis.core.ReportCard.Services;
using opensis.core.ReportCard.Interfaces;
using opensis.core.StudentEffortGrade.Services;
using opensis.core.StudentEffortGrade.Interfaces;

namespace opensisAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<INoticeService, NoticeService>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<ISchoolRegisterService, SchoolRegister>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IGradelevelService, GradelevelService>();
            services.AddScoped<IGradelevelRepository, GradeLevelRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomRegisterService, RoomRegister>();
            services.AddScoped<ISectionRepositiory, SectionRepository>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IMarkingperiodRepository, MarkingPeriodRepository>();
            services.AddScoped<IMarkingPeriodService, MarkingPeriodService>();
            services.AddScoped<ISchoolPeriodService, SchoolPeriodService>();
            services.AddScoped<ISchoolPeriodRepository, SchoolPeriodRepository>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.AddScoped<ICalendarEventService, CalendarEventService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentEnrollmentCodeService, StudentEnrollmentCodeService>();
            services.AddScoped<IStudentEnrollmentCodeRepository, StudentEnrollmentCodeRepository>();
            services.AddScoped<IAttendanceCodeRegisterService, AttendanceCodeRegister>();
            services.AddScoped<IAttendanceCodeRepository, AttendanceCodeRepository>();
            services.AddScoped<ICustomFieldService, CustomFieldService>();
            services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
            services.AddScoped<IParentInfoRegisterService, ParentInfoRegister>();
            services.AddScoped<IParentInfoRepository, ParentInfoRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ICourseManagerRepository, CourseManagerRepository>();
            services.AddScoped<ICourseManagerService, CourseManagerService>();
            services.AddScoped<IPeriodRepository, PeriodRepository>();
            services.AddScoped<IPeriodService, PeriodService>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IRoleBasedAccessRepository, RoleBasedAccessRepository>();
            services.AddScoped<IRoleBasedAccessService, RoleBasedAccessService>();
            
            services.AddScoped<IStudentScheduleRepository, StudentScheduleRepository>();
            services.AddScoped<IStudentScheduleService, StudentScheduleService>();

            services.AddScoped<IStaffScheduleRepository, StaffScheduleRepository>();
            services.AddScoped<IStaffScheduleService, StaffScheduleServices>();
            services.AddScoped<IStudentAttendanceRepository, StudentAttendanceRepository>();
            services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();

            services.AddScoped<IReportCardRepository, ReportCardRepository>();
            services.AddScoped<IReportCardService, ReportCardService>();

            services.AddScoped<IInputFinalGradeRepository, InputFinalGradeRepository>();
            services.AddScoped<IInputFinalGradeService, InputFinalGradeService>();

            services.AddScoped<IStudentEffortGradeRepository, StudentEffortGradeRepository>();
            services.AddScoped<IStudentEffortGradeService, StudentEffortGradeService>();



            if (Configuration["dbtype"] == "sqlserver")
            {
                services.AddScoped<IDbContextFactory, DbContextFactory>(serviceProvider => new DbContextFactory(Configuration["ConnectionStringTemplate"]));
            }
            else if (Configuration["dbtype"] == "mysql")
            {
                services.AddScoped<IDbContextFactory, MySQLContextFactory>(serviceProvider => new MySQLContextFactory(Configuration["ConnectionStringTemplateMySQL"]));
            }

            


            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", builder =>
                {
                    builder.WithOrigins("*")
                        .SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();
                });
            });

            ////services.AddDbContext<catalogDBContext>(ServiceLifetime.Scoped);
            ////services.AddDbContext<opensisContext>(ServiceLifetime.Scoped);

            ////services.AddTransient<ITenantProvider, DatabaseTenantProvider>();
            ////services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenSIS2 API", Version = "V1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseTenantDBMapper();

            app.UseCors(options => options.AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenSIS2WebAPI V1");
            });

            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

        }
    }
}
