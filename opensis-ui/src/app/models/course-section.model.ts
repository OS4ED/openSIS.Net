import { CommonField } from "./common-field.model";
import { CalendarModel } from "./calendar.model";
import { GradeScaleModel } from "./grades.model";
import {AttendanceCodeCategories} from './attendance-code.model';
import { RoomModel } from "./room.model";
import { BlockPeriod, GetBlockListForView } from "./school-period.model";
import { StaffMasterModel } from "./staff.model";
import { tableLanguage } from "./language.model";

export class FixedSchedulingCourseSectionAddModel extends CommonField {
    public courseSection: CourseSection;
    public courseFixedSchedule: CourseFixedSchedule;
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseFixedSchedule = new CourseFixedSchedule();  
    } 
}

export class CourseFixedSchedule {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseSectionId: number;
    public meetingDays:string;
    public gradeScaleId: number;
    public serial: number;
    public roomId: number;
    public blockId:number;
    public rooms:RoomModel;
    public blockPeriod:BlockPeriod;
    public attendanceTaken: boolean;
    public periodId: number;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    constructor() {      
        this.attendanceTaken=false;    
        this.createdBy= sessionStorage.getItem("email");
    }
}

export class VariableSchedulingCourseSectionAddModel extends CommonField{
    public courseSection : CourseSection; 
    public courseFixedSchedule:[];
    public courseVariableScheduleList:Array<CourseVariableScheduleListModel>;
    public courseCalendarSchedule:[];
    public courseBlockScheduleList:[];
    public duration: string;

    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseVariableScheduleList=[new CourseVariableScheduleListModel];
        this.courseFixedSchedule=null;          
        this.courseCalendarSchedule=null;  
        this.courseBlockScheduleList=null;   
    }
}

export class CourseVariableScheduleListModel {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public serial: number;
    public day: string;
    public periodId: number;
    public roomId: number;
    public takeAttendance: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public rooms: string;
    public schoolPeriods: string;
    constructor() {
        this.rooms = null;
        this.schoolPeriods=null;
        this.day=null;
        this.periodId=null;
        this.roomId=null;
        this.serial=0;
        this.takeAttendance=false;
        this.createdBy= sessionStorage.getItem("email");
        this.updatedBy=sessionStorage.getItem("email");
    }
}


export class CalendarSchedulingCourseSectionAddModel extends CommonField {
    public courseSection: CourseSection;
    public courseCalendarScheduleList: CourseCalendarSchedule[];
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseCalendarScheduleList = [];
    }
}

export class CourseCalendarSchedule {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public blockId: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public serial: number;
    public date: string;
    public periodId: number;
    public roomId: number;
    public takeAttendance: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public blockPeriod: BlockPeriod;
    public rooms: RoomModel;
    constructor() {
        this.courseSectionId = 0;
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = null;
        this.createdOn = null;
        this.updatedOn = null;
        this.rooms= new RoomModel();
    }
}

export class BlockedSchedulingCourseSectionAddModel extends CommonField {
    public courseSection: CourseSection;
    public courseFixedSchedule: [];
    public courseVariableScheduleList: [];
    public courseCalendarSchedule: [];
    public courseBlockScheduleList: Array<CourseBlockSchedule>;
    public duration: string;
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseFixedSchedule = null;
        this.courseVariableScheduleList = null;
        this.courseCalendarSchedule = null;
        this.courseBlockScheduleList = [new CourseBlockSchedule()];
        this.duration = null;
    }
}

export class CourseBlockSchedule {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public serial: number;
    public blockId: number;
    public periodId: number;
    public block:GetBlockListForView;
    public blockPeriod:BlockPeriod;
    public rooms:RoomModel;
    public roomId: number;
    public takeAttendance: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;

    constructor(){
        this.blockId=null;
        this.periodId=null;
        this.roomId=null;
        this.takeAttendance=false;
    }
}

export class CourseSection {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public gradeScaleType: string;
    public courseSectionName: string;
    public calendarId: number;
    public attendanceCategoryId: number;
    public creditHours: number;
    public academicYear: number;
    public seats: number;
    public allowStudentConflict: boolean;
    public allowTeacherConflict: boolean;
    public isWeightedCourse: boolean;
    public affectsClassRank: boolean;
    public affectsHonorRoll: boolean;
    public onlineClassRoom: boolean;
    public onlineClassroomUrl: string;
    public onlineClassroomPassword: string;
    public useStandards: boolean;
    public standardGradeScaleId: number;
    public durationBasedOnPeriod: boolean;
    public yrMarkingPeriodId: number;
    public smstrMarkingPeriodId: number;
    public qtrMarkingPeriodId: number;
    public durationStartDate: string;
    public durationEndDate: string;
    public scheduleType: string;
    public meetingDays: string;
    public attendanceTaken: boolean;
    public isActive: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public attendanceCodeCategories: AttendanceCodeCategories;
    public course: string;
    public gradeScale: GradeScaleModel;
    public schoolCalendars: CalendarModel;
    public schoolMaster: string;
    public quarters:markingPeriodTitle;
    public schoolYears:markingPeriodTitle;
    public semesters:markingPeriodTitle;
    public mpTitle:string; //[marking period title]This key is only used for front end view to extract mp title, this is not related to backend.
    public mpStartDate:string;
    public mpEndDate:string;
    constructor() {
        this.durationStartDate = null;
        this.durationEndDate = null;
        
        this.isActive=true;
        this.durationBasedOnPeriod=true;
    }
}
export class markingPeriodTitle{
    title;
    startDate;
    endDate;
}
export class GetAllCourseSectionModel extends CommonField {
    public getCourseSectionForView: [CourseSectionAddViewModel];
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public academicYear: number;
}

export interface SearchCourseSection {
    staffSelected: boolean;
    course: string;
    courseSectionName: string;
    markingPeriod: string;
    startDate: string;
    endDate: string;
    scheduledTeacher: boolean;
}

export class OutputEmitDataFormat{
    scheduleType:string;
    roomList: any;
    scheduleDetails:any;
    error:boolean;
}

export class CourseVariableSchedule {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public serial: number;
    public day: number | string;
    public blockId: number;
    public periodId: number;
    public roomId: number;
    public takeAttendance: boolean;
    public rooms:RoomModel;
    public blockPeriod:BlockPeriod;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;

    constructor() {
        this.day=null;
        this.periodId=null;
        this.roomId=null;
        this.serial=0;
    }
}


export class CourseSectionAddViewModel extends CommonField {
    public courseSection: CourseSection;
    public courseFixedSchedule: CourseFixedSchedule;
    public courseVariableScheduleList: CourseVariableSchedule[];
    public courseCalendarScheduleList: CourseCalendarSchedule[];
    public courseBlockScheduleList: CourseBlockSchedule[];
// Below three for View
    public courseVariableSchedule: CourseVariableSchedule[];
    public courseCalendarSchedule: CourseCalendarSchedule[];
    public courseBlockSchedule: CourseBlockSchedule[];

    public markingPeriodId: string;
    public markingPeriod: string;
    public standardGradeScaleName:string;
    public availableSeat:number;
    public totalStudentSchedule:number;
    public totalStaffSchedule:number;
    public staffName:string;
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseFixedSchedule = new CourseFixedSchedule;
        this.courseVariableScheduleList = [new CourseVariableSchedule()]
        this.courseCalendarScheduleList = null;
        this.courseBlockScheduleList = [new CourseBlockSchedule()];
    }

}


export class GetAllCourseStandardForCourseSectionModel extends CommonField{
    tenantId: string;
    schoolId: number;
    courseId: number;
    getCourseStandardForCourses:[GetCourseStandardForCoursesModel]
}

class GetCourseStandardForCoursesModel{
        standardRefNo: string;
        gradeStandardId: number   
}

export class CourseSectionDataTransferModel{
    courseSectionCount:number;
    showCourse:boolean;
    courseId:number;
    constructor(){
        this.showCourse=false;
    }
}

export class DeleteCourseSectionSchedule extends CommonField{
    schoolId: number;
    courseId: number;
    courseSectionId: number;
    scheduleType: string;
    serial: number;
    tenantId:string;
}


export class ScheduledStaffForCourseSection extends CommonField{
    courseSectionsList:ScheduledStaffCourseSection[]
    schoolId: number;
    courseId: number;
    courseSectionId: number;
    tenantId:string;
    reScheduleStaffId:number;
    conflictIndexNo:string;
    createdBy:string;
    constructor(){
        super();
        this.courseSectionsList=[]
        this.createdBy=sessionStorage.getItem('email');
    }

}

class ScheduledStaffCourseSection extends CourseSection{
    staffCoursesectionSchedule:StaffList[]
}

class StaffList{
    staffId:number;
    courseId:number;
    courseSectionId:number;
    courseSectionName:string;
    staffMaster:StaffModel;
    checked:boolean; //This is for Front End Uses
    conflict:boolean; //This is for Front End Uses
}

class StaffModel extends StaffMasterModel{
    staffEmail:string; //This is only for Frontend Uses
    firstLanguageNavigation:tableLanguage;
    secondLanguageNavigation:tableLanguage;
    thirdLanguageNavigation:tableLanguage;
    firstLanguageName:string; // Front End Uses
    secondLanguageName:string; // Front End Uses
    thirdLanguageName:string; // Front End Uses
}
export class ClassRoomURLInCourseSectionModel extends CommonField{
    courseSection: CourseSection;
    constructor(){
        super();
        this.courseSection = new CourseSection();
    }
}
