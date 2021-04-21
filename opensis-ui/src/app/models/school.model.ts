import { CommonField } from "./common-field.model";
export class SchoolModel {
    public school_id : number;
    public tenant_id: string;
    public school_name: string;
    public school_address: string;
    public isactive: boolean;
    constructor() {
        this.school_id=0;
        this.tenant_id="";
        this.school_name="";
        this.school_address="";
        this.isactive=true;
    }
}
export class SchoolViewModel extends CommonField {
    public school: SchoolModel;
    constructor() {
        super();
        this.school= new SchoolModel();
    }
}

export class SchoolListViewModel extends CommonField {
    public schoolList: SchoolModel[]  ;
    constructor() {
        super();
        this.schoolList= [];
    }
}