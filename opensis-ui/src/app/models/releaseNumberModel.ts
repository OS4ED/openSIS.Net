import { CommonField } from "./commonField";

export class ReleaseNumber {
    tenantId: string;
    schoolId: number;
    releaseNumber1: string;
    releaseDate: string;
    constructor() {
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
    }
}

export class ReleaseNumberAddViewModel extends CommonField {

    public releaseNumber : ReleaseNumber;
    constructor() {
        super();
        this.releaseNumber = new ReleaseNumber();
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token");
        
    }

}