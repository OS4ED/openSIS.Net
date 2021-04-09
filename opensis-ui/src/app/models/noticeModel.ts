import { CommonField } from './commonField';

export class NoticeModel {
    public tenantId: string;
    public schoolId: number;
    public noticeId: number;
    public targetMembershipIds: string;
    public title: string;
    public body: string;
    public validFrom: string;
    public validTo: string;
    public createdBy: string;
    constructor() {
        this.schoolId =  + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this.createdBy = sessionStorage.getItem("email");
       
    }
}

export class NoticeAddViewModel extends CommonField {
    public notice: NoticeModel;
    constructor() {
        super();
        this.notice = new NoticeModel();
        this._tenantName = sessionStorage.getItem('tenant');
        this._token= sessionStorage.getItem("token");
    }
}


export class NoticeListViewModel extends CommonField {
    public noticeList: NoticeModel[];
    public tenantId: string;
    public schoolId: number;
    constructor() {
        super();
        this.noticeList = [];
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
    }
}

