import { CommonField } from "../models/commonField";
import { CalendarModel } from "./calendarModel";
import { GradeScaleModel } from "./grades.model";
import {AttendanceCodeCategories} from './attendanceCodeModel';
import { RoomModel } from "./roomModel";
import { BlockPeriod, GetBlockListForView } from "./schoolPeriodModel";

export class FixedSchedulingCourseSectionAddModel extends CommonField {
    public courseSection: CourseSection;
    public courseFixedSchedule: CourseFixedSchedule;
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseFixedSchedule = new CourseFixedSchedule();         
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
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
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId"); 
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
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
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
        this.createdBy= sessionStorage.getItem("email");
        this.updatedBy=sessionStorage.getItem("email");
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId"); 
    }
}


export class CalendarSchedulingCourseSectionAddModel extends CommonField {
    public courseSection: CourseSection;
    public courseCalendarScheduleList: CourseCalendarSchedule[];
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseCalendarScheduleList = [];
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
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
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
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
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
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
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
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
    public courseSectionName: string;
    public calendarId: number;
    public attendanceCategoryId: number;
    public creditHours: number;
    public seats: number;
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
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this.durationStartDate = null;
        this.durationEndDate = null;
        
        this.isActive=true;
        this.durationBasedOnPeriod=true;
    }
}
class markingPeriodTitle{
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
    constructor() {
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");        
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }
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
    public day: number;
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
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
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
    public standardRefNo:string;
    constructor() {
        super();
        this.courseSection = new CourseSection();
        this.courseFixedSchedule = new CourseFixedSchedule;
        this.courseVariableScheduleList = [new CourseVariableSchedule()]
        this.courseCalendarScheduleList = null;
        this.courseBlockScheduleList = [new CourseBlockSchedule()];
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }

}


export class GetAllCourseStandardForCourseSectionModel extends CommonField{
    tenantId: string;
    schoolId: number;
    courseId: number;
    getCourseStandardForCourses:[GetCourseStandardForCoursesModel]
    constructor(){
        super();
        this.schoolId=+sessionStorage.getItem("selectedSchoolId");
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
        this.tenantId = sessionStorage.getItem("tenantId");
    }
}

class GetCourseStandardForCoursesModel{
        standardRefNo: string;
        gradeStandardId: number   
}