import { CommonField } from "./common-field.model";

export class AgeRangeList extends CommonField{
    gradeAgeRangeList:[GradeRange]
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem('token');
    }
}

class GradeRange{
    ageRangeId:number;
    ageRange:string;
}

export class EducationalStage extends CommonField{
    gradeEducationalStageList:[EducationalStageList]
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem('token');
    }
}

export class UserMasterModel {
    public emailAddress: string;
    public passwordHash: string;
    public tenantId: string;
    public schoolId: number;
    public userId: number;

    constructor() {
    }
}

export class ResetPasswordModel extends CommonField {
    public userMaster: UserMasterModel;
    constructor() {
        super();
        this.userMaster = new UserMasterModel();
    }
}

class EducationalStageList{
    iscedCode:number;
    educationalStage:string;
}

export class ExportExcel extends CommonField{
    module: string;
    constructor(){
        super();
    }
}