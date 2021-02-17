import { CommonField } from "../models/commonField";

export class MarkingPeriodListModel  extends CommonField{
    public schoolYearsView: [];
    public tenantId: string;
    public schoolId: number;
    public academicYear:number;
    constructor() {
        super(); 
        this.schoolYearsView = null;      
        this.tenantId= sessionStorage.getItem("tenantId");
        this.academicYear= +sessionStorage.getItem("academicyear");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}

export class TableSchoolYear {
    public tenantId: string;
    public schoolId: number;
    public markingPeriodId: number;
    public academicYear: number;
    public title: string;
    public shortName: string;
    public sortOrder: number;
    public startDate: string;
    public endDate: string;
    public postStartDate: string;
    public postEndDate: string;
    public doesGrades: boolean;
    public doesExam:  boolean;
    public doesComments:  boolean;
    public rolloverId: number;
    public lastUpdated: string;
    public updatedBy: string;

    constructor() {
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
    }
}
export class MarkingPeriodAddModel  extends CommonField{
    public tableSchoolYears:TableSchoolYear; 
    public schoolMaster:{};
    public semesters:[];
    constructor() {
        super();        
        this.tableSchoolYears= new TableSchoolYear(); 
        this.schoolMaster=null;
        this.semesters=null;
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}
export class TableSchoolSemester {
    public tenantId: string;
    public schoolId: number;
    public markingPeriodId: number;
    public academicYear: number;
    public yearId:number;
    public title: string;
    public shortName: string;
    public sortOrder: number;
    public startDate: string;
    public endDate: string;
    public postStartDate: string;
    public postEndDate: string;
    public doesGrades: boolean;
    public doesExam:  boolean;
    public doesComments:  boolean;
    public rolloverId: number;
    public lastUpdated: string;
    public updatedBy: string;

    constructor() {
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
    }
}
export class SemesterAddModel  extends CommonField{
    public tableSemesters:TableSchoolSemester; 
    public schoolMaster:{};
    public schoolYears:{};
    public quarters:[];
    constructor() {
        super();        
        this.tableSemesters= new TableSchoolSemester(); 
        this.schoolMaster=null;
        this.schoolYears=null;
        this.quarters=null;
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}


export class TableQuarter {
    public tenantId: string;
    public schoolId: number;
    public markingPeriodId: number;
    public academicYear: number;
    public semesterId:number;
    public title: string;
    public shortName: string;
    public sortOrder: number;
    public startDate: string;
    public endDate: string;
    public postStartDate: string;
    public postEndDate: string;
    public doesGrades: boolean;
    public doesExam:  boolean;
    public doesComments:  boolean;
    public rolloverId: number;
    public lastUpdated: string;
    public updatedBy: string;

    constructor() {
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
    }
}

export class QuarterAddModel  extends CommonField{
    public tableQuarter:TableQuarter; 
    public schoolMaster:{};
    public semesters:{};
    public progressPeriods:[];
    constructor() {
        super();        
        this.tableQuarter= new TableQuarter(); 
        this.schoolMaster=null;
        this.semesters=null;
        this.progressPeriods=null;
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}
export class TableProgressPeriod {
    public tenantId: string;
    public schoolId: number;
    public markingPeriodId: number;
    public academicYear: number;
    public quarterId:number;
    public title: string;
    public shortName: string;
    public sortOrder: number;
    public startDate: string;
    public endDate: string;
    public postStartDate: string;
    public postEndDate: string;
    public doesGrades: boolean;
    public doesExam:  boolean;
    public doesComments:  boolean;
    public rolloverId: number;
    public lastUpdated: string;
    public updatedBy: string;

    constructor() {
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
        
    }
}

export class ProgressPeriodAddModel  extends CommonField{
    public tableProgressPeriods:TableProgressPeriod; 
   
  
    constructor() {
        super();        
        this.tableProgressPeriods= new TableProgressPeriod(); 
           
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}

export class GetAcademicYearListModel extends CommonField{
    academicYears:[];
    schoolId: number;
    tenantId: string;
    constructor(){
        super();
        this.tenantId=sessionStorage.getItem("tenantId");
        this._tenantName=sessionStorage.getItem('tenant');
        this._token=sessionStorage.getItem("token");
    }
}

export class GetMarkingPeriodTitleListModel extends CommonField{
        schoolId: number;
        tenantId: string;
        academicYear: number;
        period: []
        constructor() {
            super();
            this.tenantId=sessionStorage.getItem("tenantId");
            this._tenantName=sessionStorage.getItem('tenant');
            this._token=sessionStorage.getItem("token");
        }
}

export class GetAllMarkingPeriodTitle extends CommonField{
    schoolId: number;
    tenantId: string;
    academicYear: number;
    getMarkingPeriodView: []
    constructor() {
        super();
        this.tenantId=sessionStorage.getItem("tenantId");
        this._tenantName=sessionStorage.getItem('tenant');
        this._token=sessionStorage.getItem("token");
    }
}