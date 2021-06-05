import { CommonField } from "./common-field.model";

export class AddCourseCommentCategoryModel extends CommonField{
    courseCommentCategory:ReportCardCommentModel[];
    constructor(){
        super();
        this.courseCommentCategory=[]
    }
    
}

export class DeleteCourseCommentCategoryModel extends CommonField{
    courseId: number;
    courseCommentId: number;
    constructor(){
        super();
    }
}

export class UpdateSortOrderForCourseCommentCategoryModel extends CommonField{
    courseId: number;
    previousSortOrder: number;
    currentSortOrder: number;
    constructor(){
        super();
    }
}

export class GetAllCourseCommentCategoryModel extends CommonField{
    courseCommentCategories:ReportCardCommentModel[];
    constructor(){
        super();
        this.courseCommentCategories=[]
    }
}

export class ReportCardCommentModel extends CommonField{
    courseCommentId: number;
    courseId: number;
    courseName: string;
    applicableAllCourses: boolean;
    comments: string;
    sortOrder: number;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;

    constructor(){
        super();
    }
}

export class DistinctGetAllReportCardModel extends CommonField{
    courseCommentCategories:DistinctReportCardModel[];
    constructor(){
        super();
        this.courseCommentCategories=[]
    }
}
export class DistinctReportCardModel{
    static: boolean; //This is only for Front End Uses
    courseCommentId: number;
    courseId: number;
    courseName: string;
    applicableAllCourses: boolean;
    comments: CommentModel[];
    sortOrder: number;
    createdBy?: string;
    createdOn?: string;
    updatedBy?: string;
    updatedOn?: string;
}

export class CommentModel{
    takeInput:boolean; //This is only for Front End Uses
    courseId: number;
    courseName: string;
    applicableAllCourses:boolean;
    courseCommentId:number;
    comment: string;
    sortOrder: number;
}

export class StudentDetails{
    studentId: number
}

export class AddReportCardPdf extends CommonField{
    academicYear: number;
    markingPeriods: string;
    teacherName: boolean;
    teacherComments: boolean;
    parcentage: boolean;
    gpa: boolean;
    yearToDateDailyAbsences: boolean;
    dailyAbsencesThisMarkingPeriod: boolean;
    otherAttendanceCodeYearToDate: boolean;
    studentsReportCardViewModelList: StudentDetails[];

    constructor() {
        super();
        this.studentsReportCardViewModelList = [];
        this.teacherName = true;
        this.teacherComments = true;
        this.parcentage = true;
        this.gpa = true;
        this.yearToDateDailyAbsences = true;
        this.dailyAbsencesThisMarkingPeriod = true;
        this.otherAttendanceCodeYearToDate = true;
    }
}

