import { CommonField } from "./common-field.model";
import {
  CourseBlockSchedule,
  CourseCalendarSchedule,
  CourseFixedSchedule,
  CourseSection,
  CourseVariableSchedule,
  markingPeriodTitle,
} from "./course-section.model";
import { StaffMasterModel } from "./staff.model";
import { StudentMasterModel } from "./student.model";

export class StudentCourseSectionScheduleAddViewModel extends CommonField {
  public courseSectionList: CourseSection[];
  public studentMasterList: StudentMasterModel[];
  public tenantId: string;
  public schoolId: number;
  public createdBy: string;
  public updatedBy: string;
  public conflictMessage: string;
  public _conflictFailure: boolean;
  constructor() {
    super();
  }
}

export class ScheduleStudentListViewModel extends CommonField {
  public scheduleStudentForView: ScheduleStudentForView[];
  public tenantId: string;
  public includeInactive: boolean;
  public schoolId: number;
  public courseSectionId: number;
  public filterParams: filterParams;
  public pageSize: number;
  public pageNumber: number;
  public sortingModel: Sorting;
  public totalCount: number;
  public profilePhoto: boolean;
  public _pageSize: number; // this is from response.
  constructor() {
    super();
    this.pageNumber = 1;
    this._pageSize = 10;
    this.sortingModel = new Sorting();
    this.filterParams = null;
  }
}

export class filterParams {
  columnName: string;
  filterValue: string;
  filterOption: number;
  constructor() {
    this.columnName = null;
    this.filterOption = 1;
    this.filterValue = null;
  }
}

export class ScheduleStudentForView {
  public tenantId: string;
  public schoolId: number;
  public studentId: number;
  public gradeId: number;
  public gradeScaleId: number;
  public firstGivenName: string;
  public lastFamilyName: string;
  public alternateId: string;
  public gradeLevel: string;
  public section: string;
  public phoneNumber: string;
  public action: string;
  public checked: boolean;
  public scheduleDate: string;
}

export class ScheduledStudentDropModel extends CommonField {
  public studentCoursesectionScheduleList: StudentCoursesectionSchedule[];
  public courseSectionId: number;
  public effectiveDropDate: string;
  public studentId: number;
  public updatedBy: string;
  constructor() {
    super();
  }
}
export class StudentCoursesectionSchedule {
  public tenantId: string;
  public schoolId: number;
  public studentId: number;
  public alternateId: string;
  public gradeLevel: string;
  public section: string;
  public phoneNumber: string;
  public action: string;

  public studentGuid: string;
  public studentInternalId: string;
  public firstGivenName: string;
  public middleName: string;
  public lastFamilyName: string;
  public firstLanguageId: number;
  public gradeId: number;
  public courseId: number;
  public courseSectionId: number;
  public academicYear: number;
  public gradeScaleId: number;
  public courseSectionName: string;
  public calendarId: number;
  public isDropped: true;
  public effectiveDropDate: string;
  public createdBy: string;
  public createdOn: string;
  public updatedBy: string;
  public updatedOn: string;
}

export class StudentScheduleReportViewModel extends CommonField {
  public tenantId: string;
  public schoolId: number;
  public scheduleReport: any;
  constructor() {
    super();
  }
}

export class ScheduleCoursesForStudent360Model extends CommonField {
  scheduleCourseSectionForView: ScheduleCourseSectionForViewModel[];
  studentId: number;
  isDropped: boolean;
  constructor() {
    super();
  }
}
class Sorting {
  sortColumn: string;
  sortDirection: string;
  constructor() {
      this.sortColumn = '';
      this.sortDirection = '';
  }
}

export class ScheduleCourseSectionForViewModel extends CommonField {
  tenantId: string;
  schoolId: number;
  courseId: number;
  courseSectionId: number;
  courseTitle: string;
  courseSectionName: string;
  markingPeriodTitle: boolean; // This is for Front End Uses
  mpStartDate: string;
  yrMarkingPeriodId: number;
  schoolYears: markingPeriodTitle;
  smstrMarkingPeriodId: number;
  semesters: markingPeriodTitle;
  qtrMarkingPeriodId: number;
  quarters: markingPeriodTitle;
  enrolledDate: string;
  effectiveDropDate: string;
  dayOfWeek: string;
  createdBy: string;
  createdOn: string;
  updatedBy: string;
  updatedOn: string;
  isDropped: boolean;
  courseSectionDurationStartDate: string;
  courseSectionDurationEndDate: string;
  isAssociationship: boolean;
  takeInput: boolean; // This is for Front End Uses
  staffMasterList: StaffMasterModel[];
  public courseFixedSchedule: CourseFixedSchedule;
  public courseVariableScheduleList: CourseVariableSchedule[];
  public courseCalendarScheduleList: CourseCalendarSchedule[];
  public courseBlockScheduleList: CourseBlockSchedule[];
}




export class RoutineViewModel {
  routineView: RoutineView[];
}
export class RoutineView {
  periodId: number;
  blockId: number;
  periodName: number;
  periodStartTime: string;
  periodEndTime: string;
  events: RoutineViewEvent[];
  filteredEvents: RoutineViewEvent[];
}

export class RoutineViewEvent {
  date: string;
  courseSectionName: string;
  staffName: string;
  color: string;
}

export class ScheduledCourseSectionListForStudent360Model extends CommonField{
  scheduleCourseSectionForView:[];
  studentId: number;
  isDropped: boolean;
  durationStartDate: string;
  durationEndDate: string;
  
  constructor(){
    super();
  }
}

export class WeekEventsModel{
  takeAttendance: boolean;
  blockId:number;
  periodTitle:string;
  periodId:number;
  courseId:number;
  courseSectionId:number;
  day:number;
  attendanceList:{};
  takenAttendanceList:[]
}
export class AttendanceWeekViewModel {
  attendanceWeekView: WeeklyAttendanceList[];
}
export class WeeklyAttendanceList {
  periodId: number;
  attendanceTaken: boolean;
  courseId:number;
  courseSectionId:number;
  blockId: number;
  periodTitle: string;
  days:string;
  attendanceList: {};
  takenAttendanceDays: string;
  cloneTakenAttendanceDays: number[];
  takenAttendanceList:[]
}
