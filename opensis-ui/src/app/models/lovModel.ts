import { CommonField } from './commonField';


export class lovModel{
    id: number;
    tenantId: string;
    schoolId: number;
    lovName: string;
    lovColumnValue: string;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
    constructor(){
        this.id=0;
        this.tenantId=sessionStorage.getItem("tenantId");
        this.schoolId=+sessionStorage.getItem("selectedSchoolId");
        this.createdOn=null;
        this.updatedOn=null;
    }
}

export class LovAddView extends CommonField{
    dropdownValue:lovModel;
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token");
        this.dropdownValue= new lovModel();
    }
}

export class LovList extends CommonField{
    public dropdownList:[lovModel];
    public schoolId: number;
    public lovName: string;
    public tenantId:string;
    constructor(){
        super();
        this.schoolId=+sessionStorage.getItem("selectedSchoolId");
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this.tenantId=sessionStorage.getItem("tenantId");
        this._token=sessionStorage.getItem("token");
    }
}