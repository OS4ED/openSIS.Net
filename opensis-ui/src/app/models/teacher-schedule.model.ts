import { CommonField } from "./common-field.model";
import { CourseCalendarSchedule, CourseFixedSchedule } from "./course-section.model";

export class StaffScheduleViewModel extends CommonField{
    staffScheduleViewList:StaffScheduleView[];
    courseSectionViewList:CourseSectionList[];
    tenantId:string;
    schoolId:number;
    createdBy: string;
    existingStaff: number;
    constructor(){
        super();
        this.createdBy = sessionStorage.getItem('email');
    }
}

export class StaffScheduleView{
    courseSectionViewList: CourseSectionList[];
    staffId: number;
    staffInternalId: string;
    staffFullName: string;
    staffEmail: string;
    homeroomTeacher: boolean;
    allCourseSectionChecked: boolean; // This is only for front end uses.
    conflictStaff: boolean;
    allCourseSectionConflicted: boolean; // This is only for front end uses.
    oneOrMoreCourseSectionChecked: boolean; // This is only for front end uses.
}

 export class CourseSectionList{
    courseSectionId: number;
    courseTitle: string;
    courseSectionName: string;
    durationStartDate: string;
    durationEndDate: string;
    yrMarkingPeriodId: number;
    qtrMarkingPeriodId: number;
    smstrMarkingPeriodId: number;
    scheduleType: string;
    meetingDays: string;
    courseFixedSchedule:CourseFixedSchedule;
    courseBlockSchedule:[];
    courseCalendarSchedule:CourseCalendarSchedule[];
    courseVariableSchedule:[];
    scheduledStaff:string;
    takeAttendanceForFixedSchedule:boolean;
    weekDays:string;
    markingPeriodTitle:string; //This is only used for front end.
    checked:boolean; //This is only used for front end.
    cloneMeetingDays:string; //This is only used for front end.
    conflictCourseSection:boolean;
  }

  export class AllScheduledCourseSectionForStaffModel extends CommonField{
    courseSectionViewList:CourseSectionList[];
    staffId: number;
    constructor(){
        super();
    }
  }

  
