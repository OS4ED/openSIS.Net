import { CommonField } from "./common-field.model";
import { CourseCalendarSchedule, CourseFixedSchedule } from "./course-section.model";

export class StaffScheduleViewModel extends CommonField {
    staffScheduleViewList: StaffScheduleView[];
    courseSectionViewList: CourseSectionList[];
    tenantId: string;
    schoolId: number;
    createdBy: string;
    existingStaff: number;
    constructor() {
        super();
        this.createdBy = sessionStorage.getItem('email');
    }
}

export class StaffScheduleView {
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

export class CourseSectionList {
    courseSectionId: number;
    courseId: number;
    calendarId: number;
    gradeScaleId: number;
    standardGradeScaleId: number;
    courseTitle: string;
    courseSectionName: string;
    durationStartDate: string;
    durationEndDate: string;
    yrMarkingPeriodId: number;
    qtrMarkingPeriodId: number;
    smstrMarkingPeriodId: number;
    scheduleType: string;
    meetingDays: string;
    courseFixedSchedule: CourseFixedSchedule;
    courseBlockSchedule: [];
    courseCalendarSchedule: CourseCalendarSchedule[];
    courseVariableSchedule: [];
    scheduledStaff: string;
    takeAttendanceForFixedSchedule: boolean;
    weekDays: string;
    gradeScaleType: string;
    usedStandard:boolean;

    dayOfWeek: string; // This key is used for student 360
    createdBy: string; // This key is used for student 360
    createdOn: string; // This key is used for student 360
    updatedBy: string; // This key is used for student 360
    updatedOn: string; // This key is used for student 360
    mpStartDate: string; // This key is used for student 360

    markingPeriodTitle: string; // This is only used for front end.
    checked: boolean; // This is only used for front end.
    cloneMeetingDays: string; // This is only used for front end.
    conflictCourseSection: boolean;
}

export class AllScheduledCourseSectionForStaffModel extends CommonField {
    courseSectionViewList: CourseSectionList[];
    staffId: number;
    constructor() {
        super();
    }
}


