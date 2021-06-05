import { CommonField } from "./common-field.model";
import { GradeUsStandard } from "./grades.model";

export class GetAllSubjectModel extends CommonField {
    public subjectList: [];
    public tenantId: string;
    public schoolId: number;
}
export class SubjectModel {
    public tenantId: string;
    public schoolId: number;
    public subjectId: number;
    public subjectName: string;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    constructor() {
        this.subjectId = 0;
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = sessionStorage.getItem("email");
    }
}



export class AddSubjectModel extends CommonField {
    public subjectList: [SubjectModel];
    constructor() {
        super();
        this.subjectList = [new SubjectModel()];
    }
}

export class UpdateSubjectModel extends CommonField {
    public subjectList: [SubjectModel];
    constructor() {
        super();
        this.subjectList = [new SubjectModel()];
    }
}
export class MassUpdateSubjectModel extends CommonField {
    public subjectList: [{}];
    constructor() {
        super();
        this.subjectList = [{}];
    }
}
export class DeleteSubjectModel extends CommonField {
    public subject: SubjectModel;
    constructor() {
        super();
        this.subject = new SubjectModel();
    }
}

export class ProgramsModel {
    public tenantId: string;
    public schoolId: number;
    public programId: number;
    public programName: string;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    constructor() {
        this.programId = 0;
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = sessionStorage.getItem("email");
    }
}

export class GetAllProgramModel extends CommonField {
    public programList: [];
    public tenantId: string;
    public schoolId: number;
}
export class AddProgramModel extends CommonField {
    public programList: [ProgramsModel];
    constructor() {
        super();
        this.programList = [new ProgramsModel()];
    }
}

export class UpdateProgramModel extends CommonField {
    public programList: [ProgramsModel];
    constructor() {
        super();
        this.programList = [new ProgramsModel()];
    }
}

export class MassUpdateProgramModel extends CommonField {
    public programList: [{}];
    constructor() {
        super();
        this.programList = [{}];
    }
}
export class DeleteProgramModel extends CommonField {
    public programs: ProgramsModel;
    constructor() {
        super();
        this.programs = new ProgramsModel();
    }
}

export class GetAllCourseListModel extends CommonField {
    public courseViewModelList: [];
    public tenantId: string;
    public schoolId: number;
}
export class CourseStandardModel {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public standardRefNo: string;
    public gradeUsStandard:GradeUsStandard;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    constructor() {
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = sessionStorage.getItem("email");
    }
}

export class CourseStandardForCourseViewModel extends CommonField{
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseStandards: CourseStandardModel[];
}
export class CourseModel {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseTitle: string;
    public courseShortName: string;
    public courseGradeLevel: string;
    public courseProgram: string;
    public courseSubject: string;
    public courseCategory: number;
    public creditHours: number;
    public courseDescription: string;
    public isCourseActive: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public courseStandard: [CourseStandardModel];
    constructor() {
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = sessionStorage.getItem("email");
        this.courseStandard = [new CourseStandardModel()]
    }
}
export class AddCourseModel extends CommonField {
    public course: CourseModel;
    public programId: number;
    public subjectId: number;
    constructor() {
        super();
        this.course = new CourseModel();
    }
}

export class CourseListFilterModel {
    subject: string;
    program: string;
    gradeLevel: string;

    constructor() {
        this.subject = '',
            this.program = '',
            this.gradeLevel = ''
    }
}

export class SearchCourseSectionViewModel extends CommonField {
    allCourseSectionViewList: AllCourseSectionView[];
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public blockId: number;
    public courseSubject: string;
    public courseProgram: string;
    public markingPeriodId: string;
    public forStaff: boolean;
    public forStudent: boolean;
    public markingPeriodStartDate: string;
}

export class AllCourseSectionView {
    public tenantId: string;
    public schoolId: number;
    public courseId: number;
    public courseTitle: string;
    public courseProgram: string;
    public courseSubject: string;
    public academicYear: number;
    public courseSectionId: number;
    public gradeScaleId: number;
    public courseSectionName: string;
    public seats: number;
    public durationStartDate: string;
    public durationEndDate: string;
    public yrMarkingPeriodId: number;
    public smstrMarkingPeriodId: number;
    public qtrMarkingPeriodId: number;
    public scheduleType: string;
    public fixedDays: string;
    public fixedRoomId: number;
    public fixedPeriodId: number;
    public varDay: string;
    public varPeriodId: number;
    public varRoomId: number;
    public calDate: string;
    public calPeriodId: number;
    public calRoomId: number;
    public blockPeriodId: number;
    public attendanceTaken:boolean;
    public blockRoomId: number;
    public isActive: boolean;
    public markingPeriodTitle: string; // This is only for Front End to extract the marking period title based on ID
}


export class SearchCourseForScheduleModel extends CommonField {
    course: CourseModel[]
    courseSubject: string;
    courseProgram: string;
}