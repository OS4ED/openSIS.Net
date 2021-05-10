
import { CommonField } from './common-field.model'

export class StudentFinalGrade {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public commentId: number;
    public studentFinalGradeSrlno: number;
    public basedOnStandardGrade: boolean;
    public courseId: number;
    public gradeId: number;
    public gradeScaleId: number;
    public academicYear: number;
    public calendarId: number;
    public yrMarkingPeriodId: number;
    public smstrMarkingPeriodId: number;
    public qtrMarkingPeriodId: number;
    public isPercent: boolean;
    public percentMarks: number;
    public gradeObtained: string;
    public teacherComment: string;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public studentFinalGradeComments: StudentFinalGradeComments[];
    public studentFinalGradeStandard: StudentFinalGradeStandard[];

    constructor() {
        this.studentFinalGradeStandard = [];
    }

}

export class StudentFinalGradeComments {
    courseCommentId: number;
}

export class StudentFinalGradeStandard {
    standardGradeScaleId: number;
    gradeObtained: number;
}

export class AddUpdateStudentFinalGradeModel extends CommonField {
    studentFinalGradeList = [];
    courseStandardList: [];
    tenantId: string;
    schoolId: number;
    calendarId: number;
    courseSectionId: number;
    standardGradeScaleId: number;
    courseId: number;
    isPercent: boolean;
    markingPeriodId: string;
    academicYear: number;
    createdOrUpdatedBy: string;

}