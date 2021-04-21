import { CommonField } from './common-field.model';


export class AttendanceCodeCategoryModel extends CommonField {
    attendanceCodeCategories: AttendanceCodeCategories;
    constructor() {
        super();
        this.attendanceCodeCategories = new AttendanceCodeCategories();
    }
}

export class GetAllAttendanceCategoriesListModel extends CommonField {
    public attendanceCodeCategoriesList: [];
    public tenantId: string;
    public schoolId: number;
    constructor() {
        super();
        this.attendanceCodeCategoriesList = null;
    }
}

export class AttendanceCodeModel extends CommonField {
    public attendanceCode: AttendanceCode;
    constructor() {
        super()
        this.attendanceCode = new AttendanceCode();
    }
}

export class GetAllAttendanceCodeModel extends CommonField {
    public attendanceCodeList: AttendanceCode[];
    public tenantId: string;
    public schoolId: number;
    public attendanceCategoryId: number;
    constructor() {
        super();
        this.attendanceCodeList = null;
    }
}

export class AttendanceCodeDragDropModel extends CommonField{
    tenantId:string;
    schoolId:number;
    previousSortOrder: number;
    currentSortOrder:number;
    attendanceCategoryId:number;
    constructor(){
        super();
        this.previousSortOrder=0;
        this.currentSortOrder=0;
        this.attendanceCategoryId=0;
    }


}


export class AttendanceCodeCategories {
    public tenantId: string;
    public schoolId: number;
    public attendanceCategoryId: number;
    public academicYear: number;
    public title: string;
    public lastUpdated: string;
    public updatedBy: string;

    constructor() {
        this.updatedBy = sessionStorage.getItem('email');
    }
}

export class AttendanceCode {
    tenantId: string;
    schoolId: number;
    attendanceCategoryId: number;
    attendanceCode1: number;
    academicYear: number;
    title: string;
    shortName: string;
    type: string;
    stateCode: string;
    defaultCode: boolean;
    allowEntryBy: string;
    sortOrder: number;
    lastUpdated: string;
    updatedBy: string;

    constructor() {
        this.updatedBy = sessionStorage.getItem('email');
    }
}