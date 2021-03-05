import { CommonField } from "../models/commonField";

export class TableSectionList {
    public tenantId: string;
    public schoolId: number;
    public sectionId: number;
    public name: string;
    public sortOrder: number;
    public lastUpdated: string;
    public updatedBy: string;   

}


export class GetAllSectionModel extends CommonField{
    public tableSectionsList: [TableSectionList];
    public tenantId: string;
    public schoolId: number;
    public isSectionAvailable:boolean;
    constructor() {
        super(); 
        this.tableSectionsList = null;      
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
        this.isSectionAvailable=false;
    }
}

export class TableSection  {
    public tenantId: string;
    public schoolId: number;
    public sectionId: number;
    public name: string;
    public sortOrder: number;
    public lastUpdated: string;
    public updatedBy: string;
    constructor() {     
        this.tenantId= sessionStorage.getItem("tenantId");
        this.schoolId= +sessionStorage.getItem("selectedId");      
       
    }

}


export class SectionAddModel extends CommonField {
   public tableSections:TableSection
    constructor() {
        super(); 
        this.tableSections= new TableSection();
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }

}




