


class tableLanguage {
    langId: number;
    lcid: string;
    locale: string;
    languageCode: string;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
    userMaster: []
}

export class LanguageAddModel {    
    public language:tableLanguage;   
    public _failure: boolean;
    public _message:string;
    public _tenantName:string;
    public _token:string;
    
    constructor() {       
        this.language =new tableLanguage();
        this._failure=false;
        this._message="";
        this._tenantName="";
        this._token="";
          
    }
}

export class LanguageModel  {
    public tableLanguage : [tableLanguage];
    public isLanguageAvailable:boolean;
    public _failure: boolean;
    public _message:string;
    public _tenantName:string;
    public _token:string;
    constructor() {       
        this.tableLanguage=null;   
        this.isLanguageAvailable=false;
        this._failure=false;
        this._message="";
        this._tenantName="";
        this._token="";  
    }
}


