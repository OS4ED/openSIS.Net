import { CommonField } from './commonField';


export class AttendanceCodeCategoryModel extends CommonField{
    attendanceCodeCategories:AttendanceCodeCategories;
    constructor(){
        super();
        this.attendanceCodeCategories=new AttendanceCodeCategories();
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName=sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token"); 
    }
}

export class GetAllAttendanceCategoriesListModel extends CommonField{
    public attendanceCodeCategoriesList:[];
    public tenantId:string;
    public schoolId:number;
    constructor(){
        super();
        this.attendanceCodeCategoriesList=null;
        this.tenantId=sessionStorage.getItem("tenantId");
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName=sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token"); 
    }
}

export class AttendanceCodeModel extends CommonField{
    public attendanceCode:AttendanceCode;
    constructor(){
        super()
        this.attendanceCode=new AttendanceCode();
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName=sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token"); 
    }
}

export class GetAllAttendanceCodeModel extends CommonField{
    public attendanceCodeList:[];
    public tenantId:string;
    public schoolId:number;
    public attendanceCategoryId:number;
    constructor(){
        super();
        this.attendanceCodeList=null;
        this.tenantId=sessionStorage.getItem("tenantId");
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName=sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token"); 
    }
}


export class AttendanceCodeCategories{
    public tenantId:string;
    public schoolId:number;
    public attendanceCategoryId:number;
    public academicYear:number;
    public title:string;
    public lastUpdated:string;
    public updatedBy:string;

    constructor(){
        this.tenantId = sessionStorage.getItem("tenantId");
        this.updatedBy = sessionStorage.getItem('email');
    }
}

class AttendanceCode{
    tenantId:string;
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
    lastUpdated:string;
    updatedBy: string;

    constructor(){
        this.tenantId = sessionStorage.getItem("tenantId");
        this.updatedBy = sessionStorage.getItem('email');
    }
}