import { CommonField } from './common-field.model'

export class StudentEffortGradeMaster {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public studentFinalGradeSrlno: number;
    public courseId: number;
    public courseSectionId: number;
    public academicYear: number;
    public calendarId: number;
    public yrMarkingPeriodId: number;
    public smstrMarkingPeriodId: number;
    public qtrMarkingPeriodId: number;
    public teacherComment: string;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public studentEffortGradeDetail: StudentEffortGradeDetail[];
}

export class StudentEffortGradeDetail {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public commentId: number;
    public studentFinalGradeSrlno: number;
    public id: number;
    public effortCategoryId: number;
    public effortItemId: number;
    public effortGradeScaleId: number;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
}

export class StudentEffortGradeListModel extends CommonField {
    studentEffortGradeList = [];
    tenantId: string;
    schoolId: number;
    calendarId: number;
    courseSectionId: number;
    courseId: number;
    markingPeriodId: string;
    academicYear: number;
    createdOrUpdatedBy: string;
}