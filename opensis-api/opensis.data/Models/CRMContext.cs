using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using opensis.data.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
    public class CRMContext : DbContext
    {
        private DbContextOptions contextOptions;


        public CRMContext() : base()
        {
        }

        public CRMContext(DbContextOptions options) : base(options)
        {
            this.contextOptions = options;
        }


        public virtual DbSet<AttendanceCode> AttendanceCode { get; set; }
        public virtual DbSet<AttendanceCodeCategories> AttendanceCodeCategories { get; set; }
        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<BlockPeriod> BlockPeriod { get; set; }
        public virtual DbSet<CalendarEvents> CalendarEvents { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseBlockSchedule> CourseBlockSchedule { get; set; }
        public virtual DbSet<CourseCalendarSchedule> CourseCalendarSchedule { get; set; }
        public virtual DbSet<CourseFixedSchedule> CourseFixedSchedule { get; set; }
        public virtual DbSet<CourseSection> CourseSection { get; set; }
        public virtual DbSet<CourseStandard> CourseStandard { get; set; }
        public virtual DbSet<CourseVariableSchedule> CourseVariableSchedule { get; set; }
        public virtual DbSet<CustomFields> CustomFields { get; set; }
        public virtual DbSet<CustomFieldsValue> CustomFieldsValue { get; set; }
        public virtual DbSet<DpdownValuelist> DpdownValuelist { get; set; }
        public virtual DbSet<EffortGradeLibraryCategory> EffortGradeLibraryCategory { get; set; }
        public virtual DbSet<EffortGradeLibraryCategoryItem> EffortGradeLibraryCategoryItem { get; set; }
        public virtual DbSet<EffortGradeScale> EffortGradeScale { get; set; }
        public virtual DbSet<FieldsCategory> FieldsCategory { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<GradeAgeRange> GradeAgeRange { get; set; }
        public virtual DbSet<GradeEducationalStage> GradeEducationalStage { get; set; }
        public virtual DbSet<GradeEquivalency> GradeEquivalency { get; set; }
        public virtual DbSet<GradeScale> GradeScale { get; set; }
        public virtual DbSet<GradeUsStandard> GradeUsStandard { get; set; }   
        public virtual DbSet<Gradelevels> Gradelevels { get; set; }
        public virtual DbSet<HonorRolls> HonorRolls { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<ParentAddress> ParentAddress { get; set; }
        public virtual DbSet<ParentAssociationship> ParentAssociationship { get; set; }
        public virtual DbSet<ParentInfo> ParentInfo { get; set; }
        public virtual DbSet<PermissionCategory> PermissionCategory { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }
        public virtual DbSet<PermissionSubcategory> PermissionSubcategory { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<Programs> Programs { get; set; }
        public virtual DbSet<ProgressPeriods> ProgressPeriods { get; set; }
        public virtual DbSet<Quarters> Quarters { get; set; }
        public virtual DbSet<ReleaseNumber> ReleaseNumber { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<SchoolCalendars> SchoolCalendars { get; set; }
        public virtual DbSet<SchoolDetail> SchoolDetail { get; set; }
        public virtual DbSet<SchoolMaster> SchoolMaster { get; set; }
        public virtual DbSet<SchoolPeriodsObsolete> SchoolPeriodsObsolete { get; set; }
        public virtual DbSet<SchoolYears> SchoolYears { get; set; }
        public virtual DbSet<SearchFilter> SearchFilter { get; set; }
        public virtual DbSet<Sections> Sections { get; set; }
        public virtual DbSet<Semesters> Semesters { get; set; }
        public virtual DbSet<StaffCertificateInfo> StaffCertificateInfo { get; set; }
        public virtual DbSet<StaffMaster> StaffMaster { get; set; }
        public virtual DbSet<StaffSchoolInfo> StaffSchoolInfo { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StudentComments> StudentComments { get; set; }
        public virtual DbSet<StudentCoursesectionSchedule> StudentCoursesectionSchedule { get; set; }
        public virtual DbSet<StudentDocuments> StudentDocuments { get; set; }
        public virtual DbSet<StudentEnrollment> StudentEnrollment { get; set; }
        public virtual DbSet<StudentEnrollmentCode> StudentEnrollmentCode { get; set; }
        public virtual DbSet<StudentMaster> StudentMaster { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<UserSecretQuestions> UserSecretQuestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AttendanceCode>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.AttendanceCategoryId, e.AttendanceCode1 });

                entity.ToTable("attendance_code");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");
                entity.Property(e => e.AttendanceCategoryId).HasColumnName("attendance_category_id");

                entity.Property(e => e.AttendanceCode1).HasColumnName("attendance_code");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.AllowEntryBy)
                    .HasColumnName("allow_entry_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultCode).HasColumnName("default_code");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.StateCode)
                    .HasColumnName("state_code")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AttendanceCodeCategories)
                    .WithMany(p => p.AttendanceCode)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.AttendanceCategoryId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_attendance_code_attendance_code_categories");
            });


            modelBuilder.Entity<AttendanceCodeCategories>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.AttendanceCategoryId });

                entity.ToTable("attendance_code_categories");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.AttendanceCategoryId).HasColumnName("attendance_category_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.AttendanceCodeCategories)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_attendance_code_categories_school_master");
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.BlockId });

                entity.ToTable("block");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.BlockSortOrder).HasColumnName("block_sort_order");

                entity.Property(e => e.BlockTitle)
                    .HasColumnName("block_title")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.Block)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_block_school_master");
            });

            modelBuilder.Entity<BlockPeriod>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.BlockId, e.PeriodId });

                entity.ToTable("block_period");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.CalculateAttendance).HasColumnName("calculate_attendance");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.PeriodEndTime)
                    .HasColumnName("period_end_time")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PeriodShortName)
                    .HasColumnName("period_short_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodSortOrder).HasColumnName("period_sort_order");

                entity.Property(e => e.PeriodStartTime)
                    .HasColumnName("period_start_time")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PeriodTitle)
                    .HasColumnName("period_title")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolMaster)
                   .WithMany(p => p.BlockPeriod)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_block_period_school_master");
            });


            modelBuilder.Entity<CalendarEvents>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CalendarId, e.EventId });

                entity.ToTable("calendar_events");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CalendarId).HasColumnName("calendar_id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.EventColor)
                    .HasColumnName("event_color")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasComment("will contain HEX code e.g. #5175bc.");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.SchoolDate)
                    .HasColumnName("school_date")
                    .HasColumnType("date");

                entity.Property(e => e.StartDate)
                   .HasColumnName("start_date")
                   .HasColumnType("date");

                entity.Property(e => e.SystemWideEvent)
                    .HasColumnName("system_wide_event")
                    .HasComment("event applicable to all calenders within academic year");


                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VisibleToMembershipId)
                    .HasColumnName("visible_to_membership_id")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("membershipids separated by comma");
            });


            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");
                entity.Property(e => e.Id).HasColumnName("id")
               ;
                entity.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

                entity.Property(e => e.StateId).HasColumnName("stateid");

                entity.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");


                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                   
                    .HasConstraintName("FK_city_state");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryCode)
                    .HasColumnName("countrycode")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId });

                entity.ToTable("course");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseCategory)
                    .HasColumnName("course_category")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("'Core' or 'Elective'");

                entity.Property(e => e.CourseDescription)
                    .HasColumnName("course_description")
                    .IsUnicode(false);

                entity.Property(e => e.CourseGradeLevel)
                    .HasColumnName("course_grade_level")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseProgram)
                    .HasColumnName("course_program")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CourseShortName)
                    .HasColumnName("course_short_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseSubject)
                    .HasColumnName("course_subject")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CourseTitle)
                    .HasColumnName("course_title")
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreditHours)
                    .HasColumnName("credit_hours");
     
                entity.Property(e => e.IsCourseActive).HasColumnName("is_course_active");

                entity.Property(e => e.Standard)
                    .HasColumnName("standard")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("choose between US Common Core library or school specific standards library.");

                entity.Property(e => e.StandardRefNo)
                    .HasColumnName("standard_ref_no")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseBlockSchedule>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId, e.Serial })
                    .HasName("PK_course_block_schedule_1");

                entity.ToTable("course_block_schedule");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");
               

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Serial).HasColumnName("serial");

                entity.Property(e => e.TakeAttendance).HasColumnName("take_attendance");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.CourseBlockSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.BlockId })
                    .HasConstraintName("FK_course_block_schedule_block");

               

                entity.HasOne(d => d.Rooms)
                    .WithMany(p => p.CourseBlockSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.RoomId })
                    .HasConstraintName("FK_course_block_schedule_rooms");

                entity.HasOne(d => d.BlockPeriod)
                    .WithMany(p => p.CourseBlockSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.BlockId, d.PeriodId })
                    .HasConstraintName("FK_course_block_schedule_block_periods");
            });

            modelBuilder.Entity<CourseCalendarSchedule>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId, e.Serial })
                     .HasName("PK_course_calendar_schedule_1");

                entity.ToTable("course_calendar_schedule");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");

                entity.Property(e => e.Serial).HasColumnName("serial");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.TakeAttendance).HasColumnName("take_attendance");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Rooms)
                    .WithMany(p => p.CourseCalendarSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.RoomId })
                    .HasConstraintName("FK_course_calendar_schedule_rooms");

                entity.HasOne(d => d.BlockPeriod)
                    .WithMany(p => p.CourseCalendarSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.BlockId, d.PeriodId })
                    .HasConstraintName("FK_course_calendar_schedule_block_periods");
            });

            modelBuilder.Entity<CourseFixedSchedule>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId, e.Serial })
                     .HasName("PK_course_fixed_schedule_1");

                entity.ToTable("course_fixed_schedule");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");

                entity.Property(e => e.Serial).HasColumnName("serial");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Rooms)
                    .WithMany(p => p.CourseFixedSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.RoomId })
                    .HasConstraintName("FK_course_fixed_schedule_rooms");

                entity.HasOne(d => d.BlockPeriod)
                    .WithMany(p => p.CourseFixedSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.BlockId, d.PeriodId })
                    .HasConstraintName("FK_course_fixed_schedule_block_periods");
            });

            modelBuilder.Entity<CourseSection>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId })
                    .HasName("PK_course_section_1");

                entity.ToTable("course_section");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");

                entity.Property(e => e.AcademicYear)
                   .HasColumnName("academic_year")
                   .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.AffectsClassRank).HasColumnName("affects_class_rank");

                entity.Property(e => e.AffectsHonorRoll).HasColumnName("affects_honor_roll");

                entity.Property(e => e.AttendanceCategoryId).HasColumnName("attendance_category_id");

                entity.Property(e => e.AttendanceTaken).HasColumnName("attendance_taken");

                entity.Property(e => e.CalendarId).HasColumnName("calendar_id");

                entity.Property(e => e.CourseSectionName)
                    .HasColumnName("course_section_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreditHours)
                    .HasColumnName("credit_hours")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.DurationBasedOnPeriod).HasColumnName("duration_based_on_period");

                entity.Property(e => e.DurationEndDate)
                    .HasColumnName("duration_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.DurationStartDate)
                    .HasColumnName("duration_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.GradeScaleType)
                    .HasColumnName("grade_scale_type")
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("'Ungraded','Numeric','School_Scale','Teacher_Scale'");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsWeightedCourse).HasColumnName("is_weighted_course");

                entity.Property(e => e.MeetingDays)
                    .HasColumnName("meeting_days")
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Starting Sunday as 0, 0|1|2|3|4|5|6");

                entity.Property(e => e.OnlineClassRoom).HasColumnName("online_class_room");

                entity.Property(e => e.OnlineClassroomPassword)
                    .HasColumnName("online_classroom_password")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OnlineClassroomUrl)
                    .HasColumnName("online_classroom_url")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.QtrMarkingPeriodId).HasColumnName("qtr_marking_period_id");

                entity.Property(e => e.ScheduleType)
                    .HasColumnName("schedule_type")
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("Fixed Schedule (1) / Variable Schedule (2) / Calendar Days (3) / Bell schedule (4)");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.SmstrMarkingPeriodId).HasColumnName("smstr_marking_period_id");

                entity.Property(e => e.StandardGradeScaleId).HasColumnName("standard_grade_scale_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.UseStandards).HasColumnName("use_standards");

                entity.Property(e => e.YrMarkingPeriodId).HasColumnName("yr_marking_period_id");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_course_section_school_master");

                entity.HasOne(d => d.AttendanceCodeCategories)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.AttendanceCategoryId })
                    .HasConstraintName("FK_course_section_attendance_code_categories");

                entity.HasOne(d => d.SchoolCalendars)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CalendarId })
                    .HasConstraintName("FK_course_section_school_calendars");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CourseId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_course_section_course");

                entity.HasOne(d => d.GradeScale)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.GradeScaleId })
                    
                    .HasConstraintName("FK_course_section_grade_scale");

                entity.HasOne(d => d.Quarters)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.QtrMarkingPeriodId })
                    .HasConstraintName("FK_course_section_quarters");

                entity.HasOne(d => d.Semesters)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.SmstrMarkingPeriodId })
                    .HasConstraintName("FK_course_section_semesters");

                entity.HasOne(d => d.SchoolYears)
                    .WithMany(p => p.CourseSection)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.YrMarkingPeriodId })
                    .HasConstraintName("FK_course_section_school_years");
            });

            modelBuilder.Entity<CourseStandard>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.StandardRefNo });

                entity.ToTable("course_standard");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.StandardRefNo)
                    .HasColumnName("standard_ref_no")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Course)
                   .WithMany(p => p.CourseStandard)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CourseId })
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_course_standard_course");

                entity.HasOne(d => d.GradeUsStandard)
                    .WithMany(p => p.CourseStandard)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.StandardRefNo })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_course_standard_grade_us_standard");
            });

            modelBuilder.Entity<CourseVariableSchedule>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId, e.Serial })
                    .HasName("PK_course_variable_schedule_1");

                entity.ToTable("course_variable_schedule");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");

                entity.Property(e => e.Serial).HasColumnName("serial");

                entity.Property(e => e.BlockId).HasColumnName("block_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.TakeAttendance).HasColumnName("take_attendance");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Rooms)
                    .WithMany(p => p.CourseVariableSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.RoomId })
                    .HasConstraintName("FK_course_variable_schedule_rooms");

                entity.HasOne(d => d.BlockPeriod)
                    .WithMany(p => p.CourseVariableSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.BlockId, d.PeriodId })
                    .HasConstraintName("FK_course_variable_schedule_block_periods");
            });
            modelBuilder.Entity<CustomFields>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CategoryId, e.FieldId });

                entity.ToTable("custom_fields");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");
                entity.Property(e => e.CategoryId)
                   .HasColumnName("category_id")
                   .HasComment("Take categoryid from custom_category table");

                entity.Property(e => e.FieldId).HasColumnName("field_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.DefaultSelection)
                    .HasColumnName("default_selection")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("default value selection on form load");

                entity.Property(e => e.Hide)
                .HasColumnName("hide")
                .HasComment("hide the custom field on UI");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("datetime");

                entity.Property(e => e.Module)
                     .IsRequired()
                     .HasColumnName("module")
                     .HasMaxLength(10)
                     .IsUnicode(false)
                     .IsFixedLength()
                     .HasComment("module like \"school\", \"student\" etc.");

                entity.Property(e => e.Required)
                    .HasColumnName("required")
                    .HasComment("Whether value input is required");

                entity.Property(e => e.Search).HasColumnName("search");

                entity.Property(e => e.SelectOptions)
                    .HasColumnName("select_options")
                    .IsUnicode(false)
                    .HasComment("LOV for dropdown separated by | character.");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.SystemField)
                .HasColumnName("system_field")
                .HasComment("wheher it is applicable throughput all forms");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Field Name");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Datatype");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.CustomFields)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_custom_fields_school_master");

                entity.HasOne(d => d.FieldsCategory)
                   .WithMany(p => p.CustomFields)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CategoryId })
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_custom_fields_fields_category");
            });

            modelBuilder.Entity<CustomFieldsValue>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CategoryId, e.FieldId, e.TargetId, e.Module });

                entity.ToTable("custom_fields_value");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.FieldId).HasColumnName("field_id");

                entity.Property(e => e.TargetId)
                    .HasColumnName("target_id")
                    .HasComment("Target_is school/student/staff id for whom custom field value is entered. For School module it will be always school id.");

                entity.Property(e => e.Module)
                    .HasColumnName("module")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("'Student' | 'School' | 'Staff'");

                entity.Property(e => e.CustomFieldTitle)
                    .HasColumnName("custom_field_title")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldType)
                    .HasColumnName("custom_field_type")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("'Select' or 'Text'");

                entity.Property(e => e.CustomFieldValue)
                    .HasColumnName("custom_field_value")
                    .IsUnicode(false)
                    .HasComment("User input value...Textbox->textvalue, Select-->Value separated by '|', Date --> Date in string");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CustomFields)
                    .WithMany(p => p.CustomFieldsValue)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CategoryId, d.FieldId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_custom_fields_value_custom_fields");
            });

            modelBuilder.Entity<DpdownValuelist>(entity =>
            {
                entity.ToTable("dpdown_valuelist");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.LovColumnValue)
                    .IsRequired()
                    .HasColumnName("lov_column_value")
                    .IsUnicode(false);

                entity.Property(e => e.LovName)
                    .IsRequired()
                    .HasColumnName("lov_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.DpdownValuelist)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dpdown_valuelist_school_master");
            });

            modelBuilder.Entity<EffortGradeLibraryCategory>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.EffortCategoryId })
                    .HasName("PK_effort_category");

                entity.ToTable("effort_grade_library_category");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.EffortCategoryId).HasColumnName("effort_category_id");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EffortGradeLibraryCategoryItem>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.EffortCategoryId, e.EffortItemId })
                    .HasName("PK_effort_category_item");

                entity.ToTable("effort_grade_library_category_item");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.EffortCategoryId).HasColumnName("effort_category_id");

                entity.Property(e => e.EffortItemId).HasColumnName("effort_item_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.EffortItemTitle)
                .HasColumnName("effort_item_title")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.EffortGradeLibraryCategory)
                    .WithMany(p => p.EffortGradeLibraryCategoryItem)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.EffortCategoryId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_effort_category_item_effort_category");
            });

            modelBuilder.Entity<EffortGradeScale>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.EffortGradeScaleId });

                entity.ToTable("effort_grade_scale");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.EffortGradeScaleId).HasColumnName("effort_grade_scale_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.GradeScaleComment)
                    .HasColumnName("grade_scale_comment")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.GradeScaleValue).HasColumnName("grade_scale_value");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<FieldsCategory>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CategoryId })
                    .HasName("PK_custom_category");

                entity.ToTable("fields_category");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Hide).HasColumnName("hide");

                entity.Property(e => e.IsSystemCategory).HasColumnName("is_system_category");

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("last_update")
                    .HasColumnType("datetime");

                entity.Property(e => e.Module)
                    .HasColumnName("module")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("module like \"school\", \"student\" etc.");

                entity.Property(e => e.Required).HasColumnName("required");

                entity.Property(e => e.Search).HasColumnName("search");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.FieldsCategory)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_custom_category_school_master");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.GradeScaleId, e.GradeId });

                entity.ToTable("grade");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.Breakoff).HasColumnName("breakoff");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .IsUnicode(false);

                entity.Property(e => e.UnweightedGpValue)
                    .HasColumnName("unweighted_gp_value")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.WeightedGpValue)
                    .HasColumnName("weighted_gp_value")
                    .HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.GradeScale)
                    .WithMany(p => p.Grade)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.GradeScaleId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_grade_grade_scale");
            });

            modelBuilder.Entity<GradeAgeRange>(entity =>
            {
                entity.HasKey(e => e.AgeRangeId);

                entity.ToTable("grade_age_range");

                entity.Property(e => e.AgeRangeId)
                    .HasColumnName("age_range_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AgeRange)
                    .HasColumnName("age_range")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GradeEducationalStage>(entity =>
            {
                entity.HasKey(e => e.IscedCode);

                entity.ToTable("grade_educational_stage");

                entity.Property(e => e.IscedCode)
                    .HasColumnName("isced_code")
                    .ValueGeneratedNever();

                entity.Property(e => e.EducationalStage)
                    .HasColumnName("educational_stage")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GradeEquivalency>(entity =>
            {
                entity.HasKey(e => e.EquivalencyId)
                    .HasName("PK_grade_equivalency_1");

                entity.ToTable("grade_equivalency");

                entity.Property(e => e.EquivalencyId)
                    .HasColumnName("equivalency_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GradeLevelEquivalency)
                    .IsRequired()
                    .HasColumnName("grade_level_equivalency")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GradeScale>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.GradeScaleId });

                entity.ToTable("grade_scale");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.CalculateGpa).HasColumnName("calculate_gpa");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.GradeScaleComment)
                    .HasColumnName("grade_scale_comment")
                    .IsUnicode(false);

                entity.Property(e => e.GradeScaleName)
                    .HasColumnName("grade_scale_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GradeScaleValue)
                    .HasColumnName("grade_scale_value")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.UseAsStandardGradeScale).HasColumnName("use_as_standard_grade_scale");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.GradeScale)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_grade_scale_school_master");
            });

            modelBuilder.Entity<GradeUsStandard>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StandardRefNo });

                entity.ToTable("grade_us_standard");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StandardRefNo)
                    .HasColumnName("standard_ref_no")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Course)
                    .HasColumnName("course")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Domain)
                    .HasColumnName("domain")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GradeStandardId).HasColumnName("grade_standard_id");

                entity.Property(e => e.IsSchoolSpecific).HasColumnName("is_school_specific");


                entity.Property(e => e.GradeLevel)
                    .HasColumnName("grade_level")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandardDetails)
                    .HasColumnName("standard_details")
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Topic)
                    .HasColumnName("topic")
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Gradelevels>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.GradeId })
                    .HasName("pk_gradelevels");

                entity.ToTable("gradelevels");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.AgeRangeId).HasColumnName("age_range_id");

                entity.Property(e => e.EquivalencyId).HasColumnName("equivalency_id");

                entity.Property(e => e.IscedCode).HasColumnName("isced_code");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.NextGradeId).HasColumnName("next_grade_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AgeRange)
                    .WithMany(p => p.Gradelevels)
                    .HasForeignKey(d => d.AgeRangeId)
                    .HasConstraintName("FK_gradelevels_grade_age_range");

                entity.HasOne(d => d.Equivalency)
                    .WithMany(p => p.Gradelevels)
                    .HasForeignKey(d => d.EquivalencyId)
                    .HasConstraintName("FK_gradelevels_grade_equivalency");

                entity.HasOne(d => d.IscedCodeNavigation)
                    .WithMany(p => p.Gradelevels)
                    .HasForeignKey(d => d.IscedCode)
                    .HasConstraintName("FK_gradelevels_grade_educational_stage");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.Gradelevels)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_gradelevels_school_master");
            });

            modelBuilder.Entity<HonorRolls>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.HonorRollId });

                entity.ToTable("honor_rolls");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.HonorRollId).HasColumnName("honor_roll_id");

                entity.Property(e => e.Breakoff).HasColumnName("breakoff");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.HonorRoll)
                    .HasColumnName("honor_roll")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MarkingPeriodId).HasColumnName("marking_period_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolYears)
                    .WithMany(p => p.HonorRolls)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.MarkingPeriodId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_honor_rolls_honor_rolls");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("language");
                entity.HasKey(e => e.LangId)
                    .HasName("pk_table_language");

                entity.Property(e => e.LangId)
                    .HasColumnName("lang_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.LanguageCode)
                    .HasColumnName("language_code")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Lcid)
                    .HasColumnName("lcid")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Locale)
                    .HasColumnName("locale")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.MembershipId })
                    .HasName("pk_table_membership_1");

                entity.ToTable("membership");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.MembershipId)
                    .HasColumnName("membership_id")
                    .HasComment("can be considered as profileid of Opensis1");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                 .HasColumnName("description")
                 .HasMaxLength(250)
                 .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsSuperadmin).HasColumnName("is_superadmin");

                entity.Property(e => e.IsSystem).HasColumnName("is_system");

                entity.Property(e => e.Profile)
                    .IsRequired()
                    .HasColumnName("profile")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("E.g. admin,student,teacher");

                entity.Property(e => e.ProfileType)
                  .HasColumnName("profile_type")
                  .HasMaxLength(30)
                  .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.Membership)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table_membership_table_school_master");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("notice");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.NoticeId })
                    .HasName("pk_table_notice");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.NoticeId).HasColumnName("notice_id");

                entity.Property(e => e.Body)
                    .HasColumnName("body")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isactive).HasColumnName("isactive");

                entity.Property(e => e.TargetMembershipIds)
                    .IsRequired()
                    .HasColumnName("target_membership_ids")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Signifies group of user for whom notice is visible. to be saved as comma separated values. if user's membership_id falls in any of the value, he can see the notice.");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom)
                    .HasColumnName("valid_from")
                    .HasColumnType("date");

                entity.Property(e => e.ValidTo)
                    .HasColumnName("valid_to")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<ParentAddress>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.ParentId, e.StudentId })
                    .HasName("PK_parent_address_1");

                entity.ToTable("parent_address");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.AddressLineOne)
                    .HasColumnName("address_line_one")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLineTwo)
                    .HasColumnName("address_line_two")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentAddressSame).HasColumnName("student_address_same");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParentInfo)
                    .WithMany(p => p.ParentAddress)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.ParentId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_parent_address_parent_info");

               
            });

            modelBuilder.Entity<ParentAssociationship>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.ParentId, e.StudentId });

                entity.ToTable("parent_associationship");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Associationship)
                    .HasColumnName("associationship")
                    .HasComment("tenantid#schoolid#studentid | tenantid#schoolid#studentid | ....");

                entity.Property(e => e.ContactType)
                   .HasColumnName("contact_type")
                   .HasMaxLength(9)
                   .IsUnicode(false)
                   .HasComment("Primary | Secondary | Other");

                entity.Property(e => e.IsCustodian).HasColumnName("is_custodian");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Relationship)
                    .HasColumnName("relationship")
                    .HasMaxLength(30)
                    .IsUnicode(false);


                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<ParentInfo>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.ParentId })
                    .HasName("PK_parent_info_1");

                entity.ToTable("parent_info");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.ParentGuid).HasColumnName("parent_guid");

                entity.Property(e => e.BusDropoff).HasColumnName("bus_dropoff");

                entity.Property(e => e.BusNo)
                    .HasColumnName("bus_No")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.BusPickup).HasColumnName("bus_pickup");

                entity.Property(e => e.ParentPhoto).HasColumnName("parent_photo");
                entity.Property(e => e.PersonalEmail)
                    .HasColumnName("personal_email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                   .HasColumnName("middlename")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IsPortalUser).HasColumnName("is_portal_user");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LoginEmail)
                    .HasColumnName("login_email")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("emailaddress mapped to user_master");

                

                entity.Property(e => e.Salutation)
                    .HasColumnName("salutation")
                    .HasMaxLength(20)
                    .IsUnicode(false);

               

                entity.Property(e => e.Suffix)
                   .HasColumnName("suffix")
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfile)
                    .HasColumnName("user_profile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkEmail)
                    .HasColumnName("work_email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.WorkPhone)
                    .HasColumnName("work_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PermissionCategory>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.PermissionCategoryId })
                    .HasName("PK_permission_category_1");

                entity.ToTable("permission_category");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.PermissionCategoryId).HasColumnName("permission_category_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    ;

                entity.Property(e => e.EnableAdd).HasColumnName("enable_add");

                entity.Property(e => e.EnableDelete).HasColumnName("enable_delete");

                entity.Property(e => e.EnableEdit).HasColumnName("enable_edit");

                entity.Property(e => e.EnableView).HasColumnName("enable_view");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionCategoryName)
                    .HasColumnName("permission_category_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionGroupId).HasColumnName("permission_group_id");

                entity.Property(e => e.ShortCode)
                    .HasColumnName("short_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    ;

                entity.HasOne(d => d.PermissionGroup)
                    .WithMany(p => p.PermissionCategory)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PermissionGroupId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_permission_category_permission_group");
            });

            modelBuilder.Entity<PermissionGroup>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.PermissionGroupId });

                entity.ToTable("permission_group");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.PermissionGroupId).HasColumnName("permission_group_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BadgeType)
                    .HasColumnName("badgeType")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BadgeValue)
                    .HasColumnName("badgeValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasColumnName("icon")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IconType)
                    .HasColumnName("icon_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsSystem).HasColumnName("is_system");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionGroupName)
                    .HasColumnName("permission_group_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.PermissionGroup)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_permission_group_school_master");
            });

            modelBuilder.Entity<PermissionSubcategory>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.PermissionSubcategoryId })
                    .HasName("PK_permission_subcategory_1");

                entity.ToTable("permission_subcategory");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.PermissionCategoryId).HasColumnName("permission_category_id");

                entity.Property(e => e.PermissionSubcategoryId).HasColumnName("permission_subcategory_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnableAdd).HasColumnName("enable_add");

                entity.Property(e => e.EnableDelete).HasColumnName("enable_delete");

                entity.Property(e => e.EnableEdit).HasColumnName("enable_edit");

                entity.Property(e => e.EnableView).HasColumnName("enable_view");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionGroupId).HasColumnName("permission_group_id");

                entity.Property(e => e.PermissionSubcategoryName)
                    .HasColumnName("permission_subcategory_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShortCode)
                    .HasColumnName("short_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.PermissionCategory)
                    .WithMany(p => p.PermissionSubcategory)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PermissionCategoryId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_permission_subcategory_permission_category");
            });

            modelBuilder.Entity<Plans>(entity =>
            {
                entity.ToTable("plans");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.PlanId })
                    .HasName("pk_table_plans");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.PlanId).HasColumnName("plan_id");

                entity.Property(e => e.Features).HasColumnName("features");

                entity.Property(e => e.MaxApiChecks).HasColumnName("max_api_checks");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Programs>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.ProgramId });

                entity.ToTable("programs");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ProgramId).HasColumnName("program_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProgramName)
                    .HasColumnName("program_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ProgressPeriods>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.MarkingPeriodId, e.AcademicYear, e.QuarterId })
                    .HasName("pk_table_progress_periods");

                entity.ToTable("progress_periods");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.MarkingPeriodId).HasColumnName("marking_period_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.QuarterId).HasColumnName("quarter_id");

                entity.Property(e => e.DoesComments).HasColumnName("does_comments");

                entity.Property(e => e.DoesExam).HasColumnName("does_exam");

                entity.Property(e => e.DoesGrades).HasColumnName("does_grades");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.PostEndDate)
                    .HasColumnName("post_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.PostStartDate)
                    .HasColumnName("post_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Quarters)
                    .WithMany(p => p.ProgressPeriods)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.QuarterId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_progress_periods_quarters");
            });

            modelBuilder.Entity<Quarters>(entity =>
            {
                entity.ToTable("quarters");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.MarkingPeriodId })
                    .HasName("pk_table_quarters");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.MarkingPeriodId).HasColumnName("marking_period_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.DoesComments).HasColumnName("does_comments");

                entity.Property(e => e.DoesExam).HasColumnName("does_exam");

                entity.Property(e => e.DoesGrades).HasColumnName("does_grades");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.PostEndDate)
                    .HasColumnName("post_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.PostStartDate)
                    .HasColumnName("post_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.SemesterId).HasColumnName("semester_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.Quarters)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_quarters_school_master");

                entity.HasOne(d => d.Semesters)
                    .WithMany(p => p.Quarters)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.SemesterId })
                    .HasConstraintName("FK_quarters_semesters");
            });

            modelBuilder.Entity<ReleaseNumber>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.ReleaseNumber1 });

                entity.ToTable("release_number");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ReleaseNumber1)
                    .HasColumnName("release_number")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("999.99.99");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.RolePermissionId });

                entity.ToTable("role_permission");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.RolePermissionId).HasColumnName("role_permission_id");

                entity.Property(e => e.CanAdd).HasColumnName("can_add");

                entity.Property(e => e.CanDelete).HasColumnName("can_delete");

                entity.Property(e => e.CanEdit).HasColumnName("can_edit");

                entity.Property(e => e.CanView).HasColumnName("can_view");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    ;

                entity.Property(e => e.PermissionGroupId).HasColumnName("permission_group_id");

                entity.Property(e => e.PermissionCategoryId).HasColumnName("permission_category_id");

                entity.Property(e => e.PermissionSubcategoryId).HasColumnName("permission_subcategory_id");

                entity.Property(e => e.MembershipId).HasColumnName("membership_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    ;

                entity.HasOne(d => d.PermissionCategory)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PermissionCategoryId })
                    .HasConstraintName("FK_role_permission_permission_category");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.MembershipId })
                    .HasConstraintName("FK_role_permission_membership");

                entity.HasOne(d => d.PermissionSubcategory)
                   .WithMany(p => p.RolePermission)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PermissionSubcategoryId })
                   .HasConstraintName("FK_role_permission_permission_subcategory");

                entity.HasOne(d => d.PermissionGroup)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PermissionGroupId })
                    .HasConstraintName("FK_role_permission_permission_groupId");

            });

           
            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.ToTable("rooms");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.RoomId })
                    .HasName("pk_table_rooms");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.IsActive).HasColumnName("isactive");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchoolCalendars>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.CalenderId })
                    .HasName("pk_table_school_calendars");

                entity.ToTable("school_calendars");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.CalenderId).HasColumnName("calender_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.Days)
                    .HasColumnName("days")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultCalender).HasColumnName("default_calender");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false); 

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VisibleToMembershipId)
                    .HasColumnName("visible_to_membership_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.SchoolCalendars)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_school_calendars_school_master");
            });

            modelBuilder.Entity<SchoolDetail>(entity =>
            {
                entity.ToTable("school_detail");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Affiliation)
                    .HasColumnName("affiliation")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Associations)
                .HasColumnName("associations")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.CommonToiletAccessibility)
                    .HasColumnName("common_toilet_accessibility")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ComonToiletType)
                    .HasColumnName("comon_toilet_type")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.CurrentlyAvailable).HasColumnName("currently_available");

                entity.Property(e => e.DateSchoolClosed)
                    .HasColumnName("date_school_closed")
                    .HasColumnType("date");

                entity.Property(e => e.DateSchoolOpened)
                    .HasColumnName("date_school_opened")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Facebook)
                .HasColumnName("facebook")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Fax)
                .HasColumnName("fax")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.FemaleToiletAccessibility)
                    .HasColumnName("female_toilet_accessibility")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FemaleToiletType)
                    .HasColumnName("female_toilet_type")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Gender)
                .HasColumnName("gender")
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.HandwashingAvailable).HasColumnName("handwashing_available");

                entity.Property(e => e.HighestGradeLevel)
                    .HasColumnName("highest_grade_level")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.HygeneEducation)
                    .HasColumnName("hygene_education")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Instagram)
                .HasColumnName("instagram")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LinkedIn)
                .HasColumnName("linkedin")
                    .HasMaxLength(100)
                    .IsFixedLength();
                entity.Property(e => e.Status)
                .HasColumnName("status");
                entity.Property(e => e.Internet)
                .HasColumnName("internet");

                entity.Property(e => e.Electricity)
                .HasColumnName("electricity");

                entity.Property(e => e.Locale)
                .HasColumnName("locale")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LowestGradeLevel)
                    .HasColumnName("lowest_grade_level")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.MainSourceOfDrinkingWater)
                    .HasColumnName("main_source_of_drinking_water")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.MaleToiletAccessibility)
                    .HasColumnName("male_toilet_accessibility")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.MaleToiletType)
                    .HasColumnName("male_toilet_type")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.NameOfAssistantPrincipal)
                    .HasColumnName("name_of_assistant_principal")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.NameOfPrincipal)
                    .HasColumnName("name_of_principal")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.RunningWater).HasColumnName("running_water");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.SchoolLogo)
                    .HasColumnName("school_logo");
                   

                entity.Property(e => e.SoapAndWaterAvailable).HasColumnName("soap_and_water_available");

                entity.Property(e => e.Telephone)
                .HasColumnName("telephone")
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.TotalCommonToilets).HasColumnName("total_common_toilets");

                entity.Property(e => e.TotalCommonToiletsUsable).HasColumnName("total_common_toilets_usable");

                entity.Property(e => e.TotalFemaleToilets).HasColumnName("total_female_toilets");

                entity.Property(e => e.TotalFemaleToiletsUsable).HasColumnName("total_female_toilets_usable");

                entity.Property(e => e.TotalMaleToilets).HasColumnName("total_male_toilets");

                entity.Property(e => e.TotalMaleToiletsUsable).HasColumnName("total_male_toilets_usable");

                entity.Property(e => e.Twitter)
                .HasColumnName("twitter")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Website)
                .HasColumnName("website")
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.Youtube)
                .HasColumnName("youtube")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.SchoolDetail)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .HasConstraintName("FK_school_detail_school_master");
            });

            modelBuilder.Entity<SchoolMaster>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId })
                    .HasName("pk_table_school_master");

                entity.ToTable("school_master");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.SchoolGuid).HasColumnName("school_guid");

                entity.Property(e => e.AlternateName)
                    .HasColumnName("alternate_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Longitude)
                .HasColumnName("longitude");
                            entity.Property(e => e.Latitude)
                .HasColumnName("latitude");

                entity.Property(e => e.City)
                .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Country)
                .HasColumnName("country")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.County)
                .HasColumnName("county")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CurrentPeriodEnds)
                    .HasColumnName("current_period_ends")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModifed)
                    .HasColumnName("date_modifed")
                    .HasColumnType("datetime");

                entity.Property(e => e.District)
                .HasColumnName("district")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Division)
                .HasColumnName("division")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Features).HasColumnName("features").IsUnicode(false);

                entity.Property(e => e.MaxApiChecks).HasColumnName("max_api_checks");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PlanId).HasColumnName("plan_id");

                entity.Property(e => e.SchoolAltId)
                    .HasColumnName("school_alt_id")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SchoolClassification)
                    .HasColumnName("school_classification")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.SchoolDistrictId)
                    .HasColumnName("school_district_id")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.SchoolInternalId)
                    .HasColumnName("school_internal_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolLevel)
                    .HasColumnName("school_level")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasMaxLength(100);

                entity.Property(e => e.SchoolStateId)
                    .HasColumnName("school_state_id")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.State)
                .HasColumnName("state")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StreetAddress1)
                    .HasColumnName("street_address_1")
                    .HasMaxLength(150);

                entity.Property(e => e.StreetAddress2)
                    .HasColumnName("street_address_2")
                    .HasMaxLength(150);

                entity.Property(e => e.Zip)
                .HasColumnName("zip")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Plans)
                    .WithMany(p => p.SchoolMaster)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.PlanId })
                    .HasConstraintName("FK_school_master_plans");
            });

            modelBuilder.Entity<SchoolPeriodsObsolete>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.PeriodId })
                    .HasName("pk_table_school_periods");

                entity.ToTable("school_periods_obsolete");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.PeriodId).HasColumnName("period_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.Attendance).HasColumnName("attendance");

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.IgnoreScheduling)
                    .HasColumnName("ignore_scheduling")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Length)
                    .HasColumnName("length")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchoolYears>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.MarkingPeriodId })
                    .HasName("pk_table_school_years");

                entity.ToTable("school_years");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.MarkingPeriodId).HasColumnName("marking_period_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.DoesComments).HasColumnName("does_comments");

                entity.Property(e => e.DoesExam).HasColumnName("does_exam");

                entity.Property(e => e.DoesGrades).HasColumnName("does_grades");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.PostEndDate)
                    .HasColumnName("post_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.PostStartDate)
                    .HasColumnName("post_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.SchoolYears)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_school_years_school_master");
            });

            modelBuilder.Entity<SearchFilter>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.Module, e.FilterId });

                entity.ToTable("search_filter");

                entity.HasIndex(e => new { e.TenantId, e.SchoolId, e.FilterName })
                    .HasName("IX_search_filter");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.Module)
                    .HasColumnName("module")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FilterId).HasColumnName("filter_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModifed)
                    .HasColumnName("date_modifed")
                    .HasColumnType("datetime");

                entity.Property(e => e.Emailaddress)
                    .HasColumnName("emailaddress")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FilterName)
                    .HasColumnName("filter_name")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.JsonList)
                    .HasColumnName("json_list")
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.SearchFilter)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_search_filter_school_master");

                entity.HasOne(d => d.UserMaster)
                    .WithMany(p => p.SearchFilter)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.Emailaddress })
                    .HasConstraintName("FK_search_filter_user_master");
            });

            modelBuilder.Entity<Sections>(entity =>
            {
                entity.ToTable("sections");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.SectionId })
                    .HasName("pk_table_sections");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Semesters>(entity =>
            {
                entity.ToTable("semesters");
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.MarkingPeriodId })
                    .HasName("pk_table_semesters");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.MarkingPeriodId).HasColumnName("marking_period_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.DoesComments).HasColumnName("does_comments");

                entity.Property(e => e.DoesExam).HasColumnName("does_exam");

                entity.Property(e => e.DoesGrades).HasColumnName("does_grades");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.PostEndDate)
                    .HasColumnName("post_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.PostStartDate)
                    .HasColumnName("post_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.RolloverId).HasColumnName("rollover_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.YearId).HasColumnName("year_id");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.Semesters)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_semesters_school_master");

                entity.HasOne(d => d.SchoolYears)
                    .WithMany(p => p.Semesters)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.YearId })
                    .HasConstraintName("FK_semesters_school_years");
            });

            modelBuilder.Entity<StaffCertificateInfo>(entity =>
            {
                

                entity.ToTable("staff_certificate_info");
                entity.Property(e => e.Id)
                   .HasColumnName("id")
                   .ValueGeneratedNever();


                entity.Property(e => e.CertificationCode)
                    .HasColumnName("certification_code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CertificationDate)
                    .HasColumnName("certification_date")
                    .HasColumnType("date");

                entity.Property(e => e.CertificationDescription)
                    .HasColumnName("certification_description")
                    .IsUnicode(false);

                entity.Property(e => e.CertificationExpiryDate)
                    .HasColumnName("certification_expiry_date")
                    .HasColumnType("date");

                entity.Property(e => e.CertificationName)
                    .HasColumnName("certification_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryCertification).HasColumnName("primary_certification");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StaffMaster)
                    .WithMany(p => p.StaffCertificateInfo)
                    .HasForeignKey(d => new { d.TenantId, d.StaffId })
                    .HasConstraintName("FK_staff_certificate_info_staff_master");
            });

            modelBuilder.Entity<StaffMaster>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.StaffId })
                    .HasName("PK_staff_master_1");

                entity.ToTable("staff_master");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.StaffGuid).HasColumnName("staff_guid");


                entity.Property(e => e.AlternateId)
                    .HasColumnName("alternate_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BusDropoff).HasColumnName("bus_dropoff");

                entity.Property(e => e.BusNo)
                    .HasColumnName("bus_no")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.BusPickup).HasColumnName("bus_pickup");


                entity.Property(e => e.CountryOfBirth).HasColumnName("country_of_birth");

                entity.Property(e => e.DisabilityDescription)
                    .HasColumnName("disability_description")
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DistrictId)
                    .HasColumnName("district_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.EmergencyEmail)
                    .HasColumnName("emergency_email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyFirstName)
                    .HasColumnName("emergency_first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyHomePhone)
                    .HasColumnName("emergency_home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyLastName)
                    .HasColumnName("emergency_last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyMobilePhone)
                    .HasColumnName("emergency_mobile_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyWorkPhone)
                    .HasColumnName("emergency_work_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Ethnicity)
                    .HasColumnName("ethnicity")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .IsUnicode(false);

                entity.Property(e => e.FirstGivenName)
                    .HasColumnName("first_given_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLanguage).HasColumnName("first_language");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressCity)
                    .HasColumnName("home_address_city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressCountry)
                    .HasColumnName("home_address_country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressLineOne)
                    .HasColumnName("home_address_line_one")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressLineTwo)
                    .HasColumnName("home_address_line_two")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressState)
                    .HasColumnName("home_address_state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressZip)
                    .HasColumnName("home_address_zip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.HomeroomTeacher).HasColumnName("homeroom_teacher");

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .IsUnicode(false);

                entity.Property(e => e.JobTitle)
                    .HasColumnName("job_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JoiningDate)
                    .HasColumnName("joining_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastFamilyName)
                    .HasColumnName("last_family_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("last_updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Linkedin)
                    .HasColumnName("linkedin")
                    .IsUnicode(false);

                entity.Property(e => e.LoginEmailAddress)
                    .HasColumnName("login_email_address")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressCity)
                    .HasColumnName("mailing_address_city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressCountry)
                    .HasColumnName("mailing_address_country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressLineOne)
                    .HasColumnName("mailing_address_line_one")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressLineTwo)
                    .HasColumnName("mailing_address_line_two")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressSameToHome)
                    .HasColumnName("mailing_address_same_to_home")
                    .HasComment("if true, home address will be replicated to mailing");

                entity.Property(e => e.MailingAddressState)
                    .HasColumnName("mailing_address_state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressZip)
                    .HasColumnName("mailing_address_zip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .HasColumnName("marital_status")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobile_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality).HasColumnName("nationality");

                entity.Property(e => e.OfficePhone)
                    .HasColumnName("office_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OtherGovtIssuedNumber)
                    .HasColumnName("other_govt_issued_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OtherGradeLevelTaught)
                    .HasColumnName("other_grade_level_taught")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OtherSubjectTaught)
                    .HasColumnName("other_subject_taught")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalEmail)
                    .HasColumnName("personal_email")
                    .IsUnicode(false);

                entity.Property(e => e.PhysicalDisability).HasColumnName("physical_disability");

                entity.Property(e => e.PortalAccess).HasColumnName("portal_access");

                entity.Property(e => e.PreferredName)
                    .HasColumnName("preferred_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousName)
                    .HasColumnName("previous_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryGradeLevelTaught)
                    .HasColumnName("primary_grade_level_taught")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimarySubjectTaught)
                    .HasColumnName("primary_subject_taught")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Profile)
                    .HasColumnName("profile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RelationshipToStaff)
                    .HasColumnName("relationship_to_staff")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salutation)
                    .HasColumnName("salutation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolEmail)
                    .HasColumnName("school_email")
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.SecondLanguage).HasColumnName("second_language");

                entity.Property(e => e.SocialSecurityNumber)
                    .HasColumnName("social_security_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StaffInternalId)
                    .HasColumnName("staff_internal_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StaffPhoto).HasColumnName("staff_photo");

                entity.Property(e => e.StateId)
                    .HasColumnName("state_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Suffix)
                    .HasColumnName("suffix")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThirdLanguage).HasColumnName("third_language");

                entity.Property(e => e.Twitter)
                    .HasColumnName("twitter")
                    .IsUnicode(false);

                entity.Property(e => e.Youtube)
                    .HasColumnName("youtube")
                    .IsUnicode(false);

                entity.HasOne(d => d.FirstLanguageNavigation)
                    .WithMany(p => p.StaffMasterFirstLanguageNavigation)
                    .HasForeignKey(d => d.FirstLanguage)
                    .HasConstraintName("FK_staff_master_language");

                entity.HasOne(d => d.SecondLanguageNavigation)
                    .WithMany(p => p.StaffMasterSecondLanguageNavigation)
                    .HasForeignKey(d => d.SecondLanguage)
                    .HasConstraintName("FK_staff_master_language1");

                entity.HasOne(d => d.ThirdLanguageNavigation)
                    .WithMany(p => p.StaffMasterThirdLanguageNavigation)
                    .HasForeignKey(d => d.ThirdLanguage)
                    .HasConstraintName("FK_staff_master_language2");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.StaffMaster)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_staff_master_school_master");
            });

            modelBuilder.Entity<StaffSchoolInfo>(entity =>
            {
               

                entity.ToTable("staff_school_info");

                entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .ValueGeneratedNever();

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Profile)
                    .HasColumnName("profile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolAttachedId).HasColumnName("school_attached_id");

                entity.Property(e => e.SchoolAttachedName)
                    .HasColumnName("school_attached_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StaffMaster)
                    .WithMany(p => p.StaffSchoolInfo)
                    .HasForeignKey(d => new { d.TenantId, d.StaffId })
                    .HasConstraintName("FK_staff_school_info_staff_master");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryId).HasColumnName("countryid");

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");


                entity.HasOne(d => d.Country)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_state_country");

            });

            modelBuilder.Entity<StudentComments>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StudentId, e.CommentId });

                entity.ToTable("student_comments");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                   .HasColumnName("last_updated")
                   .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StudentMaster)
                    .WithMany(p => p.StudentComments)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.StudentId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_comments_student_master");
            });

            modelBuilder.Entity<StudentCoursesectionSchedule>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StudentId, e.CourseId, e.CourseSectionId });

                entity.ToTable("student_coursesection_schedule");

                entity.HasIndex(e => new { e.TenantId, e.SchoolId, e.StudentId })
                    .HasName("IX_student_coursesection_schedule");

                entity.HasIndex(e => new { e.TenantId, e.SchoolId, e.CourseId, e.CourseSectionId })
                    .HasName("IX_student_coursesection_schedule_1");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseSectionId).HasColumnName("course_section_id");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.AlternateId)
                    .HasColumnName("alternate_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CalendarId).HasColumnName("calendar_id");

                entity.Property(e => e.CourseSectionName)
                    .HasColumnName("course_section_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstGivenName)
                    .IsRequired()
                    .HasColumnName("first_given_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLanguageId).HasColumnName("first_language_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.GradeScaleId).HasColumnName("grade_scale_id");

                entity.Property(e => e.LastFamilyName)
                    .IsRequired()
                    .HasColumnName("last_family_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentGuid).HasColumnName("student_guid");

                entity.Property(e => e.StudentInternalId)
                    .HasColumnName("student_internal_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.StudentCoursesectionSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_coursesection_schedule_school_master");

                entity.HasOne(d => d.StudentMaster)
                    .WithMany(p => p.StudentCoursesectionSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.StudentId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_coursesection_schedule_student_master");

                entity.HasOne(d => d.CourseSection)
                    .WithMany(p => p.StudentCoursesectionSchedule)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.CourseId, d.CourseSectionId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_coursesection_schedule_course_section");
            });

            modelBuilder.Entity<StudentDocuments>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StudentId, e.DocumentId });

                entity.ToTable("student_documents");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.FileUploaded).HasColumnName("file_uploaded");

                entity.Property(e => e.Filename)
                  .HasColumnName("filename")
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.Filetype)
                    .HasColumnName("filetype")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UploadedBy)
                    .HasColumnName("uploaded_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UploadedOn)
                    .HasColumnName("uploaded_on")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.StudentMaster)
                    .WithMany(p => p.StudentDocuments)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.StudentId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_documents_student_master");
            });

            modelBuilder.Entity<StudentEnrollment>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StudentId, e.EnrollmentId });

                entity.ToTable("student_enrollment");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.StudentGuid).HasColumnName("student_guid");


                entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");

                entity.Property(e => e.CalenderId).HasColumnName("calender_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");


                entity.Property(e => e.EnrollmentCode)
                    .HasColumnName("enrollment_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnName("enrollment_date")
                    .HasColumnType("date");

                entity.Property(e => e.ExitCode)
                    .HasColumnName("exit_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExitDate)
                    .HasColumnName("exit_date")
                    .HasColumnType("date");

                entity.Property(e => e.GradeLevelTitle)
                    .HasColumnName("grade_level_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.RollingOption)
                    .HasColumnName("rolling_option")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("LOV of N/A, Transferred In,Rolled Over,New");

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolTransferred)
                    .HasColumnName("school_transferred")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TransferredGrade)
                    .HasColumnName("transferred_grade")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransferredSchoolId).HasColumnName("transferred_school_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gradelevels)
                   .WithMany(p => p.StudentEnrollment)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.GradeId })
                   .HasConstraintName("FK_student_enrollment_gradelevels");

                entity.HasOne(d => d.StudentMaster)
                    .WithMany(p => p.StudentEnrollment)
                    .HasPrincipalKey(p => new { p.TenantId, p.SchoolId, p.StudentGuid })
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.StudentGuid })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_enrollment_student_master");

            });

            modelBuilder.Entity<StudentEnrollmentCode>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.EnrollmentCode })
                    .HasName("PK_student_enrollment_codes");

                entity.ToTable("student_enrollment_code");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.EnrollmentCode).HasColumnName("enrollment_code");

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("academic_year")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.StudentEnrollmentCode)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_enrollment_code_school_master1");
            });

            modelBuilder.Entity<StudentMaster>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.StudentId })
                    .HasName("PK_student_master_1");

                entity.ToTable("student_master");

                entity.HasIndex(e => new { e.TenantId, e.SchoolId, e.StudentGuid })
                   .HasName("IX_student_master")
                   .IsUnique();

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");
                entity.Property(e => e.StudentGuid).HasColumnName("student_guid");

                entity.Property(e => e.AdmissionNumber)
                    .HasColumnName("admission_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AlertDescription)
                    .HasColumnName("alert_description")
                    .IsUnicode(false);

                entity.Property(e => e.AlternateId)
                    .HasColumnName("alternate_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Associationship)
                    .HasColumnName("associationship")
                    .IsUnicode(false)
                    .HasComment("tenantid#schoolid#studentid | tenantid#schoolid#studentid | ....");

                entity.Property(e => e.BusNo)
                    .HasColumnName("bus_no")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CountryOfBirth).HasColumnName("country_of_birth");

                entity.Property(e => e.CriticalAlert)
                    .HasColumnName("critical_alert")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Dentist)
                    .HasColumnName("dentist")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DentistPhone)
                    .HasColumnName("dentist_phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DistrictId)
                    .HasColumnName("district_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.EconomicDisadvantage).HasColumnName("economic_disadvantage");

                entity.Property(e => e.Eligibility504).HasColumnName("eligibility_504");

                entity.Property(e => e.EnrollmentType)
                   .HasColumnName("enrollment_type")
                   .HasMaxLength(8)
                   .IsUnicode(false)
                   .IsFixedLength()
                   .HasComment("\"Internal\" or \"External\". Default \"Internal\"");


                entity.Property(e => e.EstimatedGradDate)
                    .HasColumnName("estimated_grad_date")
                    .HasColumnType("date");

                entity.Property(e => e.Ethnicity)
                    .HasColumnName("ethnicity")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .IsUnicode(false);

                entity.Property(e => e.FirstGivenName)
                    .HasColumnName("first_given_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLanguageId)
                .HasColumnName("first_language_id")
                .HasComment("Plan is language will be displayed in dropdown from language table and selected corresponding id will be stored into table.");

                entity.Property(e => e.FreeLunchEligibility).HasColumnName("free_lunch_eligibility");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressCity)
                    .HasColumnName("home_address_city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressLineOne)
                    .HasColumnName("home_address_line_one")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressLineTwo)
                    .HasColumnName("home_address_line_two")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressCountry)
                    .HasColumnName("home_address_country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressState)
                    .HasColumnName("home_address_state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressZip)
                    .HasColumnName("home_address_zip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceCompany)
                     .HasColumnName("insurance_company")
                     .HasMaxLength(200)
                     .IsUnicode(false);

                entity.Property(e => e.InsuranceCompanyPhone)
                    .HasColumnName("insurance_company_phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LastFamilyName)
                    .HasColumnName("last_family_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                   .HasColumnName("last_updated")
                   .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LepIndicator).HasColumnName("lep_indicator");

                entity.Property(e => e.Linkedin)
                    .HasColumnName("linkedin")
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressCity)
                    .HasColumnName("mailing_address_city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressCountry)
                      .HasColumnName("mailing_address_country")
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.MailingAddressLineOne)
                    .HasColumnName("mailing_address_line_one")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressLineTwo)
                    .HasColumnName("mailing_address_line_two")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressSameToHome)
                    .HasColumnName("mailing_address_same_to_home")
                    .HasComment("if true, home address will be replicated to mailing");

                entity.Property(e => e.MailingAddressState)
                    .HasColumnName("mailing_address_state")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailingAddressZip)
                    .HasColumnName("mailing_address_zip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .HasColumnName("marital_status")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MedicalFacility)
                   .HasColumnName("medical_facility")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.MedicalFacilityPhone)
                    .HasColumnName("medical_facility_phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobile_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality).HasColumnName("nationality");

                entity.Property(e => e.OtherGovtIssuedNumber)
                    .HasColumnName("other_govt_issued_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalEmail)
                    .HasColumnName("personal_email")
                    .IsUnicode(false);

                entity.Property(e => e.PolicyHolder)
                   .HasColumnName("policy_holder")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.PolicyNumber)
                    .HasColumnName("policy_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreferredName)
                    .HasColumnName("preferred_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousName)
                    .HasColumnName("previous_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryCarePhysician)
                   .HasColumnName("primary_care_physician")
                   .HasMaxLength(200)
                   .IsUnicode(false);

                entity.Property(e => e.PrimaryCarePhysicianPhone)
                    .HasColumnName("primary_care_physician_phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RollNumber)
                    .HasColumnName("roll_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salutation)
                    .HasColumnName("salutation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolBusDropOff).HasColumnName("school_bus_drop_off");

                entity.Property(e => e.SchoolBusPickUp).HasColumnName("school_bus_pick_up");

                entity.Property(e => e.SchoolEmail)
                    .HasColumnName("school_email")
                    .IsUnicode(false);

                entity.Property(e => e.SecondLanguageId)
                .HasColumnName("second_language_id")
                .HasComment("Plan is language will be displayed in dropdown from language table and selected corresponding id will be stored into table.");

                entity.Property(e => e.SectionId).HasColumnName("section_id");


                entity.Property(e => e.SocialSecurityNumber)
                    .HasColumnName("social_security_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialEducationIndicator).HasColumnName("special_education_indicator");

                entity.Property(e => e.StateId)
                    .HasColumnName("state_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentInternalId)
                    .HasColumnName("student_internal_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentPhoto).HasColumnName("student_photo");

                entity.Property(e => e.StudentPortalId)
                    .HasColumnName("student_portal_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Suffix)
                    .HasColumnName("suffix")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThirdLanguageId)
                .HasColumnName("third_language_id")
                .HasComment("Plan is language will be displayed in dropdown from language table and selected corresponding id will be stored into table.");


                entity.Property(e => e.Twitter)
                    .HasColumnName("twitter")
                    .IsUnicode(false);

                entity.Property(e => e.Vision)
                    .HasColumnName("vision")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VisionPhone)
                    .HasColumnName("vision_phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Youtube)
                    .HasColumnName("youtube")
                    .IsUnicode(false);

                entity.HasOne(d => d.FirstLanguage)
                   .WithMany(p => p.StudentMasterFirstLanguage)
                   .HasForeignKey(d => d.FirstLanguageId)
                   .HasConstraintName("FK_student_master_language");

                entity.HasOne(d => d.SecondLanguage)
                    .WithMany(p => p.StudentMasterSecondLanguage)
                    .HasForeignKey(d => d.SecondLanguageId)
                    .HasConstraintName("FK_student_master_language1");

                entity.HasOne(d => d.ThirdLanguage)
                    .WithMany(p => p.StudentMasterThirdLanguage)
                    .HasForeignKey(d => d.ThirdLanguageId)
                    .HasConstraintName("FK_student_master_language2");

                entity.HasOne(d => d.SchoolMaster)
                    .WithMany(p => p.StudentMaster)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_student_master_school_master");

                entity.HasOne(d => d.Sections)
                   .WithMany(p => p.StudentMaster)
                   .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.SectionId })
                   .HasConstraintName("FK_student_master_sections");

            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.SubjectId });

                entity.ToTable("subject");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubjectName)
                    .HasColumnName("subject_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.EmailAddress })
                   .HasName("PK_user_master_1");

                entity.ToTable("user_master");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.EmailAddress)
                .HasColumnName("emailaddress")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(250)
                .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LangId)
                    .HasColumnName("lang_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.MembershipId).HasColumnName("membership_id");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.PasswordHash)
                .HasColumnName("passwordhash")
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.UserMaster)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_master_language");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.UserMaster)
                    .HasForeignKey(d => new { d.TenantId, d.SchoolId, d.MembershipId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_master_membership");
                   
            });

            modelBuilder.Entity<UserSecretQuestions>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.SchoolId, e.Emailaddress });

                entity.ToTable("user_secret_questions");

                entity.Property(e => e.TenantId).HasColumnName("tenant_id");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.Emailaddress)
                    .HasColumnName("emailaddress")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Book)
                    .HasColumnName("book")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cartoon)
                    .HasColumnName("cartoon")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Hero)
                    .HasColumnName("hero")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Movie)
                    .HasColumnName("movie")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.UserMaster)
                    .WithOne(p => p.UserSecretQuestions)
                    .HasForeignKey<UserSecretQuestions>(d => new { d.TenantId, d.SchoolId, d.Emailaddress })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_secret_questions_user_master");
            });
            LanguageSeedData(modelBuilder);
            CountrySeedData(modelBuilder);
            GradeEquivalencySeedData(modelBuilder);
            GradeEducationalStageSeedData(modelBuilder);
            GradeAgeRangeSeedData(modelBuilder);
           // StateSeedData(modelBuilder);
           //CitySeedData(modelBuilder);
           //DemoRegisterData(modelBuilder);
        }

        private void LanguageSeedData(ModelBuilder mb)
        {
            mb.Entity<Language>().HasData(new Language { LangId = 1, Lcid = "af", Locale = "Afrikaans", LanguageCode = "af" },
                new Language { LangId = 2, Lcid = "sq", Locale = "Albanian", LanguageCode = "sq" },
                new Language { LangId = 3, Lcid = "am", Locale = "Amharic", LanguageCode = "am" },
                new Language { LangId = 4, Lcid = "ar-dz", Locale = "Arabic - Algeria", LanguageCode = "ar" },
                new Language { LangId = 5, Lcid = "ar-bh", Locale = "Arabic - Bahrain", LanguageCode = "ar" },
                new Language { LangId = 6, Lcid = "ar-eg", Locale = "Arabic - Egypt", LanguageCode = "ar" },
                new Language { LangId = 7, Lcid = "ar-iq", Locale = "Arabic - Iraq", LanguageCode = "ar" },
                new Language { LangId = 8, Lcid = "ar-jo", Locale = "Arabic - Jordan", LanguageCode = "ar" },
                new Language { LangId = 9, Lcid = "ar-kw", Locale = "Arabic - Kuwait", LanguageCode = "ar" },
                new Language { LangId = 10, Lcid = "ar-lb", Locale = "Arabic - Lebanon", LanguageCode = "ar" },
                new Language { LangId = 11, Lcid = "ar-ly", Locale = "Arabic - Libya", LanguageCode = "ar" },
                new Language { LangId = 12, Lcid = "ar-ma", Locale = "Arabic - Morocco", LanguageCode = "ar" },
                new Language { LangId = 13, Lcid = "ar-om", Locale = "Arabic - Oman", LanguageCode = "ar" },
                new Language { LangId = 14, Lcid = "ar-qa", Locale = "Arabic - Qatar", LanguageCode = "ar" },
                new Language { LangId = 15, Lcid = "ar-sa", Locale = "Arabic - Saudi Arabia", LanguageCode = "ar" },
                new Language { LangId = 16, Lcid = "ar-sy", Locale = "Arabic - Syria", LanguageCode = "ar" },
                new Language { LangId = 17, Lcid = "ar-tn", Locale = "Arabic - Tunisia", LanguageCode = "ar" },
                new Language { LangId = 18, Lcid = "ar-ae", Locale = "Arabic - United Arab Emirates", LanguageCode = "ar" },
                new Language { LangId = 19, Lcid = "ar-ye", Locale = "Arabic - Yemen", LanguageCode = "ar" },
                new Language { LangId = 20, Lcid = "hy", Locale = "Armenian", LanguageCode = "hy" },
                new Language { LangId = 21, Lcid = "as", Locale = "Assamese", LanguageCode = "as" },
                new Language { LangId = 22, Lcid = "az-az", Locale = "Azeri - Cyrillic", LanguageCode = "az" },
                new Language { LangId = 23, Lcid = "az-az", Locale = "Azeri - Latin", LanguageCode = "az" },
                new Language { LangId = 24, Lcid = "eu", Locale = "Basque", LanguageCode = "eu" },
                new Language { LangId = 25, Lcid = "be", Locale = "Belarusian", LanguageCode = "be" },
                new Language { LangId = 26, Lcid = "bn", Locale = "Bengali - Bangladesh", LanguageCode = "bn" },
                new Language { LangId = 27, Lcid = "bn", Locale = "Bengali - India", LanguageCode = "bn" },
                new Language { LangId = 28, Lcid = "bs", Locale = "Bosnian", LanguageCode = "bs" },
                new Language { LangId = 29, Lcid = "bg", Locale = "Bulgarian", LanguageCode = "bg" },
                new Language { LangId = 30, Lcid = "my", Locale = "Burmese", LanguageCode = "my" },
                new Language { LangId = 31, Lcid = "ca", Locale = "Catalan", LanguageCode = "ca" },
                new Language { LangId = 32, Lcid = "zh-cn", Locale = "Chinese - China", LanguageCode = "zh" },
                new Language { LangId = 33, Lcid = "zh-hk", Locale = "Chinese - Hong Kong SAR", LanguageCode = "zh" },
                new Language { LangId = 34, Lcid = "zh-mo", Locale = "Chinese - Macau SAR", LanguageCode = "zh" },
                new Language { LangId = 35, Lcid = "zh-sg", Locale = "Chinese - Singapore", LanguageCode = "zh" },
                new Language { LangId = 36, Lcid = "zh-tw", Locale = "Chinese - Taiwan", LanguageCode = "zh" },
                new Language { LangId = 37, Lcid = "hr", Locale = "Croatian", LanguageCode = "hr" },
                new Language { LangId = 38, Lcid = "cs", Locale = "Czech", LanguageCode = "cs" },
                new Language { LangId = 39, Lcid = "da", Locale = "Danish", LanguageCode = "da" },
                new Language { LangId = 40, Lcid = "Maldivian", Locale = "Divehi", LanguageCode = "Dhivehi" },
                new Language { LangId = 41, Lcid = "nl-be", Locale = "Dutch - Belgium", LanguageCode = "nl" },
                new Language { LangId = 42, Lcid = "nl-nl", Locale = "Dutch - Netherlands", LanguageCode = "nl" },
                new Language { LangId = 43, Lcid = "en-au", Locale = "English - Australia", LanguageCode = "en" },
                new Language { LangId = 44, Lcid = "en-bz", Locale = "English - Belize", LanguageCode = "en" },
                new Language { LangId = 45, Lcid = "en-ca", Locale = "English - Canada", LanguageCode = "en" },
                new Language { LangId = 46, Lcid = "en-cb", Locale = "English - Caribbean", LanguageCode = "en" },
                new Language { LangId = 47, Lcid = "en-gb", Locale = "English - Great Britain", LanguageCode = "en" },
                new Language { LangId = 48, Lcid = "en-in", Locale = "English - India", LanguageCode = "en" },
                new Language { LangId = 49, Lcid = "en-ie", Locale = "English - Ireland", LanguageCode = "en" },
                new Language { LangId = 50, Lcid = "en-jm", Locale = "English - Jamaica", LanguageCode = "en" },
                new Language { LangId = 51, Lcid = "en-nz", Locale = "English - New Zealand", LanguageCode = "en" },
                new Language { LangId = 52, Lcid = "en-ph", Locale = "English - Philippines", LanguageCode = "en" },
                new Language { LangId = 53, Lcid = "en-za", Locale = "English - Southern Africa", LanguageCode = "en" },
                new Language { LangId = 54, Lcid = "en-tt", Locale = "English - Trinidad", LanguageCode = "en" },
                new Language { LangId = 55, Lcid = "en-us", Locale = "English - United States", LanguageCode = "en" },
                new Language { LangId = 56, Lcid = "et", Locale = "Estonian", LanguageCode = "et" },
                new Language { LangId = 57, Lcid = "mk", Locale = "FYRO Macedonia", LanguageCode = "mk" },
                new Language { LangId = 58, Lcid = "fo", Locale = "Faroese", LanguageCode = "fo" },
                new Language { LangId = 59, Lcid = "fa", Locale = "Farsi - Persian", LanguageCode = "fa" },
                new Language { LangId = 60, Lcid = "fi", Locale = "Finnish", LanguageCode = "fi" },
                new Language { LangId = 61, Lcid = "fr-be", Locale = "French - Belgium", LanguageCode = "fr" },
                new Language { LangId = 62, Lcid = "fr-ca", Locale = "French - Canada", LanguageCode = "fr" },
                new Language { LangId = 63, Lcid = "fr-fr", Locale = "French - France", LanguageCode = "fr" },
                new Language { LangId = 64, Lcid = "fr-lu", Locale = "French - Luxembourg", LanguageCode = "fr" },
                new Language { LangId = 65, Lcid = "fr-ch", Locale = "French - Switzerland", LanguageCode = "fr" },
                new Language { LangId = 66, Lcid = "gd-ie", Locale = "Gaelic - Ireland", LanguageCode = "gd" },
                new Language { LangId = 67, Lcid = "gd", Locale = "Gaelic - Scotland", LanguageCode = "gd" },
                new Language { LangId = 68, Lcid = "de-at", Locale = "German - Austria", LanguageCode = "de" },
                new Language { LangId = 69, Lcid = "de-de", Locale = "German - Germany", LanguageCode = "de" },
                new Language { LangId = 70, Lcid = "de-li", Locale = "German - Liechtenstein", LanguageCode = "de" },
                new Language { LangId = 71, Lcid = "de-lu", Locale = "German - Luxembourg", LanguageCode = "de" },
                new Language { LangId = 72, Lcid = "de-ch", Locale = "German - Switzerland", LanguageCode = "de" },
                new Language { LangId = 73, Lcid = "el", Locale = "Greek", LanguageCode = "el" },
                new Language { LangId = 74, Lcid = "gn", Locale = "Guarani - Paraguay", LanguageCode = "gn" },
                new Language { LangId = 75, Lcid = "gu", Locale = "Gujarati", LanguageCode = "gu" },
                new Language { LangId = 76, Lcid = "he", Locale = "Hebrew", LanguageCode = "he" },
                new Language { LangId = 77, Lcid = "hi", Locale = "Hindi", LanguageCode = "hi" },
                new Language { LangId = 78, Lcid = "hu", Locale = "Hungarian", LanguageCode = "hu" },
                new Language { LangId = 79, Lcid = "is", Locale = "Icelandic", LanguageCode = "is" },
                new Language { LangId = 80, Lcid = "id", Locale = "Indonesian", LanguageCode = "id" },
                new Language { LangId = 81, Lcid = "it-it", Locale = "Italian - Italy", LanguageCode = "it" },
                new Language { LangId = 82, Lcid = "it-ch", Locale = "Italian - Switzerland", LanguageCode = "it" },
                new Language { LangId = 83, Lcid = "ja", Locale = "Japanese", LanguageCode = "ja" },
                new Language { LangId = 84, Lcid = "kn", Locale = "Kannada", LanguageCode = "kn" },
                new Language { LangId = 85, Lcid = "ks", Locale = "Kashmiri", LanguageCode = "ks" },
                new Language { LangId = 86, Lcid = "kk", Locale = "Kazakh", LanguageCode = "kk" },
                new Language { LangId = 87, Lcid = "km", Locale = "Khmer", LanguageCode = "km" },
                new Language { LangId = 88, Lcid = "ko", Locale = "Korean", LanguageCode = "ko" },
                new Language { LangId = 89, Lcid = "lo", Locale = "Lao", LanguageCode = "lo" },
                new Language { LangId = 90, Lcid = "la", Locale = "Latin", LanguageCode = "la" },
                new Language { LangId = 91, Lcid = "lv", Locale = "Latvian", LanguageCode = "lv" },
                new Language { LangId = 92, Lcid = "lt", Locale = "Lithuanian", LanguageCode = "lt" },
                new Language { LangId = 93, Lcid = "ms-bn", Locale = "Malay - Brunei", LanguageCode = "ms" },
                new Language { LangId = 94, Lcid = "ms-my", Locale = "Malay - Malaysia", LanguageCode = "ms" },
                new Language { LangId = 95, Lcid = "ml", Locale = "Malayalam", LanguageCode = "ml" },
                new Language { LangId = 96, Lcid = "mt", Locale = "Maltese", LanguageCode = "mt" },
                new Language { LangId = 97, Lcid = "mi", Locale = "Maori", LanguageCode = "mi" },
                new Language { LangId = 98, Lcid = "mr", Locale = "Marathi", LanguageCode = "mr" },
                new Language { LangId = 99, Lcid = "mn", Locale = "Mongolian", LanguageCode = "mn" },
                new Language { LangId = 100, Lcid = "mn", Locale = "Mongolian", LanguageCode = "mn" },
                new Language { LangId = 101, Lcid = "ne", Locale = "Nepali", LanguageCode = "ne" },
                new Language { LangId = 102, Lcid = "no-no", Locale = "Norwegian - Bokml", LanguageCode = "nb" },
                new Language { LangId = 103, Lcid = "no-no", Locale = "Norwegian - Nynorsk", LanguageCode = "nn" },
                new Language { LangId = 104, Lcid = "or", Locale = "Oriya", LanguageCode = "or" },
                new Language { LangId = 105, Lcid = "pl", Locale = "Polish", LanguageCode = "pl" },
                new Language { LangId = 106, Lcid = "pt-br", Locale = "Portuguese - Brazil", LanguageCode = "pt" },
                new Language { LangId = 107, Lcid = "pt-pt", Locale = "Portuguese - Portugal", LanguageCode = "pt" },
                new Language { LangId = 108, Lcid = "pa", Locale = "Punjabi", LanguageCode = "pa" },
                new Language { LangId = 109, Lcid = "rm", Locale = "Raeto-Romance", LanguageCode = "rm" },
                new Language { LangId = 110, Lcid = "ro-mo", Locale = "Romanian - Moldova", LanguageCode = "ro" },
                new Language { LangId = 111, Lcid = "ro", Locale = "Romanian - Romania", LanguageCode = "ro" },
                new Language { LangId = 112, Lcid = "ru", Locale = "Russian", LanguageCode = "ru" },
                new Language { LangId = 113, Lcid = "ru-mo", Locale = "Russian - Moldova", LanguageCode = "ru" },
                new Language { LangId = 114, Lcid = "sa", Locale = "Sanskrit", LanguageCode = "sa" },
                new Language { LangId = 115, Lcid = "sr-sp", Locale = "Serbian - Cyrillic", LanguageCode = "sr" },
                new Language { LangId = 116, Lcid = "sr-sp", Locale = "Serbian - Latin", LanguageCode = "sr" },
                new Language { LangId = 117, Lcid = "tn", Locale = "Setsuana", LanguageCode = "tn" },
                new Language { LangId = 118, Lcid = "sd", Locale = "Sindhi", LanguageCode = "sd" },
                new Language { LangId = 119, Lcid = "si", Locale = "Sinhala", LanguageCode = "Sinhalese" },
                new Language { LangId = 120, Lcid = "sk", Locale = "Slovak", LanguageCode = "sk" },
                new Language { LangId = 121, Lcid = "sl", Locale = "Slovenian", LanguageCode = "sl" },
                new Language { LangId = 122, Lcid = "so", Locale = "Somali", LanguageCode = "so" },
                new Language { LangId = 123, Lcid = "sb", Locale = "Sorbian", LanguageCode = "sb" },
                new Language { LangId = 124, Lcid = "es-ar", Locale = "Spanish - Argentina", LanguageCode = "es" },
                new Language { LangId = 125, Lcid = "es-bo", Locale = "Spanish - Bolivia", LanguageCode = "es" },
                new Language { LangId = 126, Lcid = "es-cl", Locale = "Spanish - Chile", LanguageCode = "es" },
                new Language { LangId = 127, Lcid = "es-co", Locale = "Spanish - Colombia", LanguageCode = "es" },
                new Language { LangId = 128, Lcid = "es-cr", Locale = "Spanish - Costa Rica", LanguageCode = "es" },
                new Language { LangId = 129, Lcid = "es-do", Locale = "Spanish - Dominican Republic", LanguageCode = "es" },
                new Language { LangId = 130, Lcid = "es-ec", Locale = "Spanish - Ecuador", LanguageCode = "es" },
                new Language { LangId = 131, Lcid = "es-sv", Locale = "Spanish - El Salvador", LanguageCode = "es" },
                new Language { LangId = 132, Lcid = "es-gt", Locale = "Spanish - Guatemala", LanguageCode = "es" },
                new Language { LangId = 133, Lcid = "es-hn", Locale = "Spanish - Honduras", LanguageCode = "es" },
                new Language { LangId = 134, Lcid = "es-mx", Locale = "Spanish - Mexico", LanguageCode = "es" },
                new Language { LangId = 135, Lcid = "es-ni", Locale = "Spanish - Nicaragua", LanguageCode = "es" },
                new Language { LangId = 136, Lcid = "es-pa", Locale = "Spanish - Panama", LanguageCode = "es" },
                new Language { LangId = 137, Lcid = "es-py", Locale = "Spanish - Paraguay", LanguageCode = "es" },
                new Language { LangId = 138, Lcid = "es-pe", Locale = "Spanish - Peru", LanguageCode = "es" },
                new Language { LangId = 139, Lcid = "es-pr", Locale = "Spanish - Puerto Rico", LanguageCode = "es" },
                new Language { LangId = 140, Lcid = "es-es", Locale = "Spanish - Spain (Traditional)", LanguageCode = "es" },
                new Language { LangId = 141, Lcid = "es-uy", Locale = "Spanish - Uruguay", LanguageCode = "es" },
                new Language { LangId = 142, Lcid = "es-ve", Locale = "Spanish - Venezuela", LanguageCode = "es" },
                new Language { LangId = 143, Lcid = "sw", Locale = "Swahili", LanguageCode = "sw" },
                new Language { LangId = 144, Lcid = "sv-fi", Locale = "Swedish - Finland", LanguageCode = "sv" },
                new Language { LangId = 145, Lcid = "sv-se", Locale = "Swedish - Sweden", LanguageCode = "sv" },
                new Language { LangId = 146, Lcid = "tg", Locale = "Tajik", LanguageCode = "tg" },
                new Language { LangId = 147, Lcid = "ta", Locale = "Tamil", LanguageCode = "ta" },
                new Language { LangId = 148, Lcid = "tt", Locale = "Tatar", LanguageCode = "tt" },
                new Language { LangId = 149, Lcid = "te", Locale = "Telugu", LanguageCode = "te" },
                new Language { LangId = 150, Lcid = "th", Locale = "Thai", LanguageCode = "th" },
                new Language { LangId = 151, Lcid = "bo", Locale = "Tibetan", LanguageCode = "bo" },
                new Language { LangId = 152, Lcid = "ts", Locale = "Tsonga", LanguageCode = "ts" },
                new Language { LangId = 153, Lcid = "tr", Locale = "Turkish", LanguageCode = "tr" },
                new Language { LangId = 154, Lcid = "tk", Locale = "Turkmen", LanguageCode = "tk" },
                new Language { LangId = 155, Lcid = "uk", Locale = "Ukrainian", LanguageCode = "uk" },
                new Language { LangId = 157, Lcid = "ur", Locale = "Urdu", LanguageCode = "ur" },
                new Language { LangId = 158, Lcid = "uz-uz", Locale = "Uzbek - Cyrillic", LanguageCode = "uz" },
                new Language { LangId = 159, Lcid = "uz-uz", Locale = "Uzbek - Latin", LanguageCode = "uz" },
                new Language { LangId = 160, Lcid = "vi", Locale = "Vietnamese", LanguageCode = "vi" },
                new Language { LangId = 161, Lcid = "cy", Locale = "Welsh", LanguageCode = "cy" },
                new Language { LangId = 162, Lcid = "xh", Locale = "Xhosa", LanguageCode = "xh" },
                new Language { LangId = 163, Lcid = "yi", Locale = "Yiddish", LanguageCode = "yi" },
                new Language { LangId = 164, Lcid = "zu", Locale = "Zulu", LanguageCode = "zu" }
                );
          }




        private void CountrySeedData(ModelBuilder mb)
        {
            mb.Entity<Country>().HasData(new Country { Id = 1, Name = "Afghanistan", CountryCode = "AF" },
                new Country { Id = 2, Name = "Albania", CountryCode = "AL" },
                new Country { Id = 3, Name = "Algeria", CountryCode = "DZ" },
                new Country { Id = 4, Name = "American Samoa", CountryCode = "AS" },
                new Country { Id = 5, Name = "Andorra", CountryCode = "AD" },
                new Country { Id = 6, Name = "Angola", CountryCode = "AO" },
                new Country { Id = 7, Name = "Anguilla", CountryCode = "AI" },
                new Country { Id = 8, Name = "Antarctica", CountryCode = "AQ" },
                new Country { Id = 9, Name = "Antigua And Barbuda", CountryCode = "AG" },
                new Country { Id = 10, Name = "Argentina", CountryCode = "AR" },
                new Country { Id = 11, Name = "Armenia", CountryCode = "AM" },
                new Country { Id = 12, Name = "Aruba", CountryCode = "AW" },
                new Country { Id = 13, Name = "Australia", CountryCode = "AU" },
                new Country { Id = 14, Name = "Austria", CountryCode = "AT" },
                new Country { Id = 15, Name = "Azerbaijan", CountryCode = "AZ" },
                new Country { Id = 16, Name = "Bahamas The", CountryCode = "BS" },
                new Country { Id = 17, Name = "Bahrain", CountryCode = "BH" },
                new Country { Id = 18, Name = "Bangladesh", CountryCode = "BD" },
                new Country { Id = 19, Name = "Barbados", CountryCode = "BB" },
                new Country { Id = 20, Name = "Belarus", CountryCode = "BY" },
                new Country { Id = 21, Name = "Belgium", CountryCode = "BE" },
                new Country { Id = 22, Name = "Belize", CountryCode = "BZ" },
                new Country { Id = 23, Name = "Benin", CountryCode = "BJ" },
                new Country { Id = 24, Name = "Bermuda", CountryCode = "BM" },
                new Country { Id = 25, Name = "Bhutan", CountryCode = "BT" },
                new Country { Id = 26, Name = "Bolivia", CountryCode = "BO" },
                new Country { Id = 27, Name = "Bosnia and Herzegovina", CountryCode = "BA" },
                new Country { Id = 28, Name = "Botswana", CountryCode = "BW" },
                new Country { Id = 29, Name = "Bouvet Island", CountryCode = "BV" },
                new Country { Id = 30, Name = "Brazil", CountryCode = "BR" },
                new Country { Id = 31, Name = "British Indian Ocean Territory", CountryCode = "IO" },
                new Country { Id = 32, Name = "Brunei", CountryCode = "BN" },
                new Country { Id = 33, Name = "Bulgaria", CountryCode = "BG" },
                new Country { Id = 34, Name = "Burkina Faso", CountryCode = "BF" },
                new Country { Id = 35, Name = "Burundi", CountryCode = "BI" },
                new Country { Id = 36, Name = "Cambodia", CountryCode = "KH" },
                new Country { Id = 37, Name = "Cameroon", CountryCode = "CM" },
                new Country { Id = 38, Name = "Canada", CountryCode = "CA" },
                new Country { Id = 39, Name = "Cape Verde", CountryCode = "CV" },
                new Country { Id = 40, Name = "Cayman Islands", CountryCode = "KY" },
                new Country { Id = 41, Name = "Central African Republic", CountryCode = "CF" },
                new Country { Id = 42, Name = "Chad", CountryCode = "TD" },
                new Country { Id = 43, Name = "Chile", CountryCode = "CL" },
                new Country { Id = 44, Name = "China", CountryCode = "CN" },
                new Country { Id = 45, Name = "Christmas Island", CountryCode = "CX" },
                new Country { Id = 46, Name = "Cocos (Keeling) Islands", CountryCode = "CC" },
                new Country { Id = 47, Name = "Colombia", CountryCode = "CO" },
                new Country { Id = 48, Name = "Comoros", CountryCode = "KM" },
                new Country { Id = 49, Name = "Congo", CountryCode = "CG" },
                new Country { Id = 50, Name = "Congo The Democratic Republic Of The", CountryCode = "CD" },
                new Country { Id = 51, Name = "Cook Islands", CountryCode = "CK" },
                new Country { Id = 52, Name = "Costa Rica", CountryCode = "CR" },
                new Country { Id = 53, Name = "Cote D'Ivoire (Ivory Coast)", CountryCode = "CI" },
                new Country { Id = 54, Name = "Croatia (Hrvatska)", CountryCode = "HR" },
                new Country { Id = 55, Name = "Cuba", CountryCode = "CU" },
                new Country { Id = 56, Name = "Cyprus", CountryCode = "CY" },
                new Country { Id = 57, Name = "Czech Republic", CountryCode = "CZ" },
                new Country { Id = 58, Name = "Denmark", CountryCode = "DK" },
                new Country { Id = 59, Name = "Djibouti", CountryCode = "DJ" },
                new Country { Id = 60, Name = "Dominica", CountryCode = "DM" },
                new Country { Id = 61, Name = "Dominican Republic", CountryCode = "DO" },
                new Country { Id = 62, Name = "East Timor", CountryCode = "TP" },
                new Country { Id = 63, Name = "Ecuador", CountryCode = "EC" },
                new Country { Id = 64, Name = "Egypt", CountryCode = "EG" },
                new Country { Id = 65, Name = "El Salvador", CountryCode = "SV" },
                new Country { Id = 66, Name = "Equatorial Guinea", CountryCode = "GQ" },
                new Country { Id = 67, Name = "Eritrea", CountryCode = "ER" },
                new Country { Id = 68, Name = "Estonia", CountryCode = "EE" },
                new Country { Id = 69, Name = "Ethiopia", CountryCode = "ET" },
                new Country { Id = 70, Name = "External Territories of Australia", CountryCode = "XA" },
                new Country { Id = 71, Name = "Falkland Islands", CountryCode = "FK" },
                new Country { Id = 72, Name = "Faroe Islands", CountryCode = "FO" },
                new Country { Id = 73, Name = "Fiji Islands", CountryCode = "FJ" },
                new Country { Id = 74, Name = "Finland", CountryCode = "FI" },
                new Country { Id = 75, Name = "France", CountryCode = "FR" },
                new Country { Id = 76, Name = "French Guiana", CountryCode = "GF" },
                new Country { Id = 77, Name = "French Polynesia", CountryCode = "PF" },
                new Country { Id = 78, Name = "French Southern Territories", CountryCode = "TF" },
                new Country { Id = 79, Name = "Gabon", CountryCode = "GA" },
                new Country { Id = 80, Name = "Gambia The", CountryCode = "GM" },
                new Country { Id = 81, Name = "Georgia", CountryCode = "GE" },
                new Country { Id = 82, Name = "Germany", CountryCode = "DE" },
                new Country { Id = 83, Name = "Ghana", CountryCode = "GH" },
                new Country { Id = 84, Name = "Gibraltar", CountryCode = "GI" },
                new Country { Id = 85, Name = "Greece", CountryCode = "GR" },
                new Country { Id = 86, Name = "Greenland", CountryCode = "GL" },
                new Country { Id = 87, Name = "Grenada", CountryCode = "GD" },
                new Country { Id = 88, Name = "Guadeloupe", CountryCode = "GP" },
                new Country { Id = 89, Name = "Guam", CountryCode = "GU" },
                new Country { Id = 90, Name = "Guatemala", CountryCode = "GT" },
                new Country { Id = 91, Name = "Guernsey and Alderney", CountryCode = "XU" },
                new Country { Id = 92, Name = "Guinea", CountryCode = "GN" },
                new Country { Id = 93, Name = "Guinea-Bissau", CountryCode = "GW" },
                new Country { Id = 94, Name = "Guyana", CountryCode = "GY" },
                new Country { Id = 95, Name = "Haiti", CountryCode = "HT" },
                new Country { Id = 96, Name = "Heard and McDonald Islands", CountryCode = "HM" },
                new Country { Id = 97, Name = "Honduras", CountryCode = "HN" },
                new Country { Id = 98, Name = "Hong Kong S.A.R.", CountryCode = "HK" },
                new Country { Id = 99, Name = "Hungary", CountryCode = "HU" },
                new Country { Id = 100, Name = "Iceland", CountryCode = "IS" },
                new Country { Id = 101, Name = "India", CountryCode = "IN" },
                new Country { Id = 102, Name = "Indonesia", CountryCode = "ID" },
                new Country { Id = 103, Name = "Iran", CountryCode = "IR" },
                new Country { Id = 104, Name = "Iraq", CountryCode = "IQ" },
                new Country { Id = 105, Name = "Ireland", CountryCode = "IE" },
                new Country { Id = 106, Name = "Israel", CountryCode = "IL" },
                new Country { Id = 107, Name = "Italy", CountryCode = "IT" },
                new Country { Id = 108, Name = "Jamaica", CountryCode = "JM" },
                new Country { Id = 109, Name = "Japan", CountryCode = "JP" },
                new Country { Id = 110, Name = "Jersey", CountryCode = "XJ" },
                new Country { Id = 111, Name = "Jordan", CountryCode = "JO" },
                new Country { Id = 112, Name = "Kazakhstan", CountryCode = "KZ" },
                new Country { Id = 113, Name = "Kenya", CountryCode = "KE" },
                new Country { Id = 114, Name = "Kiribati", CountryCode = "KI" },
                new Country { Id = 115, Name = "Korea North", CountryCode = "KP" },
                new Country { Id = 116, Name = "Korea South", CountryCode = "KR" },
                new Country { Id = 117, Name = "Kuwait", CountryCode = "KW" },
                new Country { Id = 118, Name = "Kyrgyzstan", CountryCode = "KG" },
                new Country { Id = 119, Name = "Laos", CountryCode = "LA" },
                new Country { Id = 120, Name = "Latvia", CountryCode = "LV" },
                new Country { Id = 121, Name = "Lebanon", CountryCode = "LB" },
                new Country { Id = 122, Name = "Lesotho", CountryCode = "LS" },
                new Country { Id = 123, Name = "Liberia", CountryCode = "LR" },
                new Country { Id = 124, Name = "Libya", CountryCode = "LY" },
                new Country { Id = 125, Name = "Liechtenstein", CountryCode = "LI" },
                new Country { Id = 126, Name = "Lithuania", CountryCode = "LT" },
                new Country { Id = 127, Name = "Luxembourg", CountryCode = "LU" },
                new Country { Id = 128, Name = "Macau S.A.R.", CountryCode = "MO" },
                new Country { Id = 129, Name = "Macedonia", CountryCode = "MK" },
                new Country { Id = 130, Name = "Madagascar", CountryCode = "MG" },
                new Country { Id = 131, Name = "Malawi", CountryCode = "MW" },
                new Country { Id = 132, Name = "Malaysia", CountryCode = "MY" },
                new Country { Id = 133, Name = "Maldives", CountryCode = "MV" },
                new Country { Id = 134, Name = "Mali", CountryCode = "ML" },
                new Country { Id = 135, Name = "Malta", CountryCode = "MT" },
                new Country { Id = 136, Name = "Man (Isle of)", CountryCode = "XM" },
                new Country { Id = 137, Name = "Marshall Islands", CountryCode = "MH" },
                new Country { Id = 138, Name = "Martinique", CountryCode = "MQ" },
                new Country { Id = 139, Name = "Mauritania", CountryCode = "MR" },
                new Country { Id = 140, Name = "Mauritius", CountryCode = "MU" },
                new Country { Id = 141, Name = "Mayotte", CountryCode = "YT" },
                new Country { Id = 142, Name = "Mexico", CountryCode = "MX" },
                new Country { Id = 143, Name = "Micronesia", CountryCode = "FM" },
                new Country { Id = 144, Name = "Moldova", CountryCode = "MD" },
                new Country { Id = 145, Name = "Monaco", CountryCode = "MC" },
                new Country { Id = 146, Name = "Mongolia", CountryCode = "MN" },
                new Country { Id = 147, Name = "Montserrat", CountryCode = "MS" },
                new Country { Id = 148, Name = "Morocco", CountryCode = "MA" },
                new Country { Id = 149, Name = "Mozambique", CountryCode = "MZ" },
                new Country { Id = 150, Name = "Myanmar", CountryCode = "MM" },
                new Country { Id = 151, Name = "Namibia", CountryCode = "NA" },
                new Country { Id = 152, Name = "Nauru", CountryCode = "NR" },
                new Country { Id = 153, Name = "Nepal", CountryCode = "NP" },
                new Country { Id = 154, Name = "Netherlands Antilles", CountryCode = "AN" },
                new Country { Id = 155, Name = "Netherlands The", CountryCode = "NL" },
                new Country { Id = 156, Name = "New Caledonia", CountryCode = "NC" },
                new Country { Id = 157, Name = "New Zealand", CountryCode = "NZ" },
                new Country { Id = 158, Name = "Nicaragua", CountryCode = "NI" },
                new Country { Id = 159, Name = "Niger", CountryCode = "NE" },
                new Country { Id = 160, Name = "Nigeria", CountryCode = "NG" },
                new Country { Id = 161, Name = "Niue", CountryCode = "NU" },
                new Country { Id = 162, Name = "Norfolk Island", CountryCode = "NF" },
                new Country { Id = 163, Name = "Northern Mariana Islands", CountryCode = "MP" },
                new Country { Id = 164, Name = "Norway", CountryCode = "NO" },
                new Country { Id = 165, Name = "Oman", CountryCode = "OM" },
                new Country { Id = 166, Name = "Pakistan", CountryCode = "PK" },
                new Country { Id = 167, Name = "Palau", CountryCode = "PW" },
                new Country { Id = 168, Name = "Palestinian Territory Occupied", CountryCode = "PS" },
                new Country { Id = 169, Name = "Panama", CountryCode = "PA" },
                new Country { Id = 170, Name = "Papua new Guinea", CountryCode = "PG" },
                new Country { Id = 171, Name = "Paraguay", CountryCode = "PY" },
                new Country { Id = 172, Name = "Peru", CountryCode = "PE" },
                new Country { Id = 173, Name = "Philippines", CountryCode = "PH" },
                new Country { Id = 174, Name = "Pitcairn Island", CountryCode = "PN" },
                new Country { Id = 175, Name = "Poland", CountryCode = "PL" },
                new Country { Id = 176, Name = "Portugal", CountryCode = "PT" },
                new Country { Id = 177, Name = "Puerto Rico", CountryCode = "PR" },
                new Country { Id = 178, Name = "Qatar", CountryCode = "QA" },
                new Country { Id = 179, Name = "Reunion", CountryCode = "RE" },
                new Country { Id = 180, Name = "Romania", CountryCode = "RO" },
                new Country { Id = 181, Name = "Russia", CountryCode = "RU" },
                new Country { Id = 182, Name = "Rwanda", CountryCode = "RW" },
                new Country { Id = 183, Name = "Saint Helena", CountryCode = "SH" },
                new Country { Id = 184, Name = "Saint Kitts And Nevis", CountryCode = "KN" },
                new Country { Id = 185, Name = "Saint Lucia", CountryCode = "LC" },
                new Country { Id = 186, Name = "Saint Pierre and Miquelon", CountryCode = "PM" },
                new Country { Id = 187, Name = "Saint Vincent And The Grenadines", CountryCode = "VC" },
                new Country { Id = 188, Name = "Samoa", CountryCode = "WS" },
                new Country { Id = 189, Name = "San Marino", CountryCode = "SM" },
                new Country { Id = 190, Name = "Sao Tome and Principe", CountryCode = "ST" },
                new Country { Id = 191, Name = "Saudi Arabia", CountryCode = "SA" },
                new Country { Id = 192, Name = "Senegal", CountryCode = "SN" },
                new Country { Id = 193, Name = "Serbia", CountryCode = "RS" },
                new Country { Id = 194, Name = "Seychelles", CountryCode = "SC" },
                new Country { Id = 195, Name = "Sierra Leone", CountryCode = "SL" },
                new Country { Id = 196, Name = "Singapore", CountryCode = "SG" },
                new Country { Id = 197, Name = "Slovakia", CountryCode = "SK" },
                new Country { Id = 198, Name = "Slovenia", CountryCode = "SI" },
                new Country { Id = 199, Name = "Smaller Territories of the UK", CountryCode = "XG" },
                new Country { Id = 200, Name = "Solomon Islands", CountryCode = "SB" },
                new Country { Id = 201, Name = "Somalia", CountryCode = "SO" },
                new Country { Id = 202, Name = "South Africa", CountryCode = "ZA" },
                new Country { Id = 203, Name = "South Georgia", CountryCode = "GS" },
                new Country { Id = 204, Name = "South Sudan", CountryCode = "SS" },
                new Country { Id = 205, Name = "Spain", CountryCode = "ES" },
                new Country { Id = 206, Name = "Sri Lanka", CountryCode = "LK" },
                new Country { Id = 207, Name = "Sudan", CountryCode = "SD" },
                new Country { Id = 208, Name = "Suriname", CountryCode = "SR" },
                new Country { Id = 209, Name = "Svalbard And Jan Mayen Islands", CountryCode = "SJ" },
                new Country { Id = 210, Name = "Swaziland", CountryCode = "SZ" },
                new Country { Id = 211, Name = "Sweden", CountryCode = "SE" },
                new Country { Id = 212, Name = "Switzerland", CountryCode = "CH" },
                new Country { Id = 213, Name = "Syria", CountryCode = "SY" },
                new Country { Id = 214, Name = "Taiwan", CountryCode = "TW" },
                new Country { Id = 215, Name = "Tajikistan", CountryCode = "TJ" },
                new Country { Id = 216, Name = "Tanzania", CountryCode = "TZ" },
                new Country { Id = 217, Name = "Thailand", CountryCode = "TH" },
                new Country { Id = 218, Name = "Togo", CountryCode = "TG" },
                new Country { Id = 219, Name = "Tokelau", CountryCode = "TK" },
                new Country { Id = 220, Name = "Tonga", CountryCode = "TO" },
                new Country { Id = 221, Name = "Trinidad And Tobago", CountryCode = "TT" },
                new Country { Id = 222, Name = "Tunisia", CountryCode = "TN" },
                new Country { Id = 223, Name = "Turkey", CountryCode = "TR" },
                new Country { Id = 224, Name = "Turkmenistan", CountryCode = "TM" },
                new Country { Id = 225, Name = "Turks And Caicos Islands", CountryCode = "TC" },
                new Country { Id = 226, Name = "Tuvalu", CountryCode = "TV" },
                new Country { Id = 227, Name = "Uganda", CountryCode = "UG" },
                new Country { Id = 228, Name = "Ukraine", CountryCode = "UA" },
                new Country { Id = 229, Name = "United Arab Emirates", CountryCode = "AE" },
                new Country { Id = 230, Name = "United Kingdom", CountryCode = "GB" },
                new Country { Id = 231, Name = "United States", CountryCode = "US" },
                new Country { Id = 232, Name = "United States Minor Outlying Islands", CountryCode = "UM" },
                new Country { Id = 233, Name = "Uruguay", CountryCode = "UY" },
                new Country { Id = 234, Name = "Uzbekistan", CountryCode = "UZ" },
                new Country { Id = 235, Name = "Vanuatu", CountryCode = "VU" },
                new Country { Id = 236, Name = "Vatican City State (Holy See)", CountryCode = "VA" },
                new Country { Id = 237, Name = "Venezuela", CountryCode = "VE" },
                new Country { Id = 238, Name = "Vietnam", CountryCode = "VN" },
                new Country { Id = 239, Name = "Virgin Islands (British)", CountryCode = "VG" },
                new Country { Id = 240, Name = "Virgin Islands (US)", CountryCode = "VI" },
                new Country { Id = 241, Name = "Wallis And Futuna Islands", CountryCode = "WF" },
                new Country { Id = 242, Name = "Western Sahara", CountryCode = "EH" },
                new Country { Id = 243, Name = "Yemen", CountryCode = "YE" },
                new Country { Id = 244, Name = "Yugoslavia", CountryCode = "YU" },
                new Country { Id = 245, Name = "Zambia", CountryCode = "ZM" },
                new Country { Id = 246, Name = "Zimbabwe", CountryCode = "ZW" }

            );
        }

        //private void StateSeedData(ModelBuilder mb)
        //{
        //    var statejsonList = StateData.StateJSON;
        //    var states = JsonConvert.DeserializeObject<List<State>>(statejsonList);
        //    mb.Entity<State>().HasData(states);
        //}

        //private void CitySeedData(ModelBuilder mb)
        //{
        //    //var dataText = System.IO.File.ReadAllText(@"weatherdataseed.json");
        //    var cityjsonList= CityData.CityJSON;
        //    var cities = JsonConvert.DeserializeObject<List<City>>(cityjsonList);
        //    mb.Entity<City>().HasData(cities);
        //}


        private void GradeEquivalencySeedData(ModelBuilder mb)
        {

            mb.Entity<GradeEquivalency>().HasData(
                new GradeEquivalency { EquivalencyId = -1, GradeLevelEquivalency = "Pre-Kindergarten" },
                new GradeEquivalency { EquivalencyId = 0, GradeLevelEquivalency = "Kindergarten" },
                new GradeEquivalency { EquivalencyId = 1, GradeLevelEquivalency = "1st Grade" },
                new GradeEquivalency { EquivalencyId = 2, GradeLevelEquivalency = "2nd Grade" },
                new GradeEquivalency { EquivalencyId = 3, GradeLevelEquivalency = "3rd Grade" },
                new GradeEquivalency { EquivalencyId = 4, GradeLevelEquivalency = "4th Grade" },
                new GradeEquivalency { EquivalencyId = 5, GradeLevelEquivalency = "5th Grade" },
                new GradeEquivalency { EquivalencyId = 6, GradeLevelEquivalency = "6th Grade" },
                new GradeEquivalency { EquivalencyId = 7, GradeLevelEquivalency = "7th Grade" },
                new GradeEquivalency { EquivalencyId = 8, GradeLevelEquivalency = "8th Grade" },
                new GradeEquivalency { EquivalencyId = 9, GradeLevelEquivalency = "9th Grade" },
                new GradeEquivalency { EquivalencyId = 10, GradeLevelEquivalency = "10th Grade" },
                new GradeEquivalency { EquivalencyId = 11, GradeLevelEquivalency = "11th Grade" },
                new GradeEquivalency { EquivalencyId = 12, GradeLevelEquivalency = "12th Grade" },
                new GradeEquivalency { EquivalencyId = 13, GradeLevelEquivalency = "1st Year College" },
                new GradeEquivalency { EquivalencyId = 14, GradeLevelEquivalency = "2nd Year College" },
                new GradeEquivalency { EquivalencyId = 15, GradeLevelEquivalency = "3rd Year College" },
                new GradeEquivalency { EquivalencyId = 16, GradeLevelEquivalency = "4th Year College" },
                new GradeEquivalency { EquivalencyId = 17, GradeLevelEquivalency = "5th Year College" },
                new GradeEquivalency { EquivalencyId = 18, GradeLevelEquivalency = "6th Year College" },
                new GradeEquivalency { EquivalencyId = 19, GradeLevelEquivalency = "7th Year College" },
                new GradeEquivalency { EquivalencyId = 20, GradeLevelEquivalency = "8th Year College" }
                );
        }

        private void GradeEducationalStageSeedData(ModelBuilder mb)
        {

            mb.Entity<GradeEducationalStage>().HasData(
                new GradeEducationalStage { IscedCode = 0, EducationalStage = "Early childhood Education" },
                new GradeEducationalStage { IscedCode = 1, EducationalStage = "Primary education" },
                new GradeEducationalStage { IscedCode = 2, EducationalStage = "Lower secondary education" },
                new GradeEducationalStage { IscedCode = 3, EducationalStage = "Upper secondary education" },
                new GradeEducationalStage { IscedCode = 4, EducationalStage = "Post-secondary non-tertiary education" },
                new GradeEducationalStage { IscedCode = 5, EducationalStage = "Short-cycle tertiary education" },
                new GradeEducationalStage { IscedCode = 6, EducationalStage = "Bachelor's degree or equivalent" },
                new GradeEducationalStage { IscedCode = 7, EducationalStage = "Master's degree or equivalent" },
                new GradeEducationalStage { IscedCode = 8, EducationalStage = "Doctoral degree or equivalent" }
                );
        }

        private void GradeAgeRangeSeedData(ModelBuilder mb)
        {

            mb.Entity<GradeAgeRange>().HasData(
               new GradeAgeRange { AgeRangeId = 0, AgeRange = "Below 5" },
               new GradeAgeRange { AgeRangeId = 1, AgeRange = "5–6" },
               new GradeAgeRange { AgeRangeId = 2, AgeRange = "6–7" },
               new GradeAgeRange { AgeRangeId = 3, AgeRange = "7–8" },
               new GradeAgeRange { AgeRangeId = 4, AgeRange = "8–9" },
               new GradeAgeRange { AgeRangeId = 5, AgeRange = "9–10" },
               new GradeAgeRange { AgeRangeId = 6, AgeRange = "10–11" },
               new GradeAgeRange { AgeRangeId = 7, AgeRange = "11–12" },
               new GradeAgeRange { AgeRangeId = 8, AgeRange = "12–13" },
               new GradeAgeRange { AgeRangeId = 9, AgeRange = "13–14" },
               new GradeAgeRange { AgeRangeId = 10, AgeRange = "14–15" },
               new GradeAgeRange { AgeRangeId = 11, AgeRange = "15–16" },
               new GradeAgeRange { AgeRangeId = 12, AgeRange = "16–17" },
               new GradeAgeRange { AgeRangeId = 13, AgeRange = "17–18" },
               new GradeAgeRange { AgeRangeId = 14, AgeRange = "18+" }
                );
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string[] tenants = new string[] { "TenantA" };
                string connectionString = "Server=SABYA\\SQLEXPRESS;Database={tenant};User Id=sa; Password=admin@123;MultipleActiveResultSets=true";
                optionsBuilder.UseSqlServer(connectionString.Replace("{tenant}", "opensisv2_Dev"));

                //foreach (string tenant in tenants)
                //{
                //    optionsBuilder.UseSqlServer(connectionString.Replace("{tenant}", "TenantA"));
                //}
            }
        }
    }
}
