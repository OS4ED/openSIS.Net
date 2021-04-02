import { CommonField } from "../models/commonField";
import { CourseCalendarSchedule, CourseFixedSchedule } from "./courseSectionModel";

export class StaffScheduleViewModel extends CommonField{
    staffScheduleViewList:StaffScheduleView[];
    courseSectionViewList:CourseSectionList[];
    tenantId:string;
    schoolId:number;
    constructor(){
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");         
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");

    }
}

export class StaffScheduleView{
    courseSectionViewList:CourseSectionList[];
    staffId: number;
    staffInternalId:string;
    staffFullName: string;
    staffEmail: string;
    homeroomTeacher: boolean;
    allCourseSectionChecked:boolean; //This is only for front end uses.
    conflictStaff:boolean;
    allCourseSectionConflicted:boolean; //This is only for front end uses.
    oneOrMoreCourseSectionChecked:boolean; //This is only for front end uses.
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
    conflictCourseSection:boolean;
  }