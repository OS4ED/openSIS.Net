import { CommonField } from "../models/commonField";

export class GetAllGradeLevelsModel extends CommonField{
    public tableGradelevelList:[];
    public tenantId: string;
    public schoolId: number;
    public _tenantName: string;
    public _token: string;
    public _failure: true;
    public _message: string;   
    
    constructor(){
        super();
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
        this._tenantName = sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this._token = sessionStorage.getItem("token");
        this.tenantId = sessionStorage.getItem("tenantId");
    }
}

class tblGradelevel {
    public tenantId: string;
    public schoolId: number;
    public gradeId: number;
    public shortName: string;
    public title: string;
    public equivalencyId:string;
    public ageRangeId:number;
    public iscedCode:number;
    public nextGrade: string;
    public nextGradeId:number;
    public sortOrder: number;
    public lastUpdated: string;
    public updatedBy: string

    constructor(){
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this.updatedBy = sessionStorage.getItem('email');
    }
}
export class AddGradeLevelModel extends CommonField{
        public tblGradelevel: tblGradelevel
        constructor(){
            super();
            this.tblGradelevel = new tblGradelevel();
            this._userName = sessionStorage.getItem("user");
        }

}


class gradeEquivalencyList{
    equivalencyId: string;
    gradeLevelEquivalency: string;
    gradelevels:[]
}

export class GelAllGradeEquivalencyModel extends CommonField{
    gradeEquivalencyList:gradeEquivalencyList;
      constructor(){
        super();
        this._tenantName=sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
      }
}