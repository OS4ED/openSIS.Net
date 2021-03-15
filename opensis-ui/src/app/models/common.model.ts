import { CommonField } from "./commonField";

export class AgeRangeList extends CommonField{
    gradeAgeRangeList:[GradeRange]
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem('tenant');
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
        this._token=sessionStorage.getItem('token');
    }
}

class EducationalStageList{
    iscedCode:number;
    educationalStage:string;
}