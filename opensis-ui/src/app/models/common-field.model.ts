export class CommonField  {
    public _failure: boolean;
    public _message:string;
    public _tenantName:string;
    public _token:string;
    public language:string;
    public _userName:string;
    public schoolId:number | string;
    public tenantId:string;
    constructor(){
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");         
        this._tenantName = sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this._token = sessionStorage.getItem("token");
        this.language ="en";
    }
}