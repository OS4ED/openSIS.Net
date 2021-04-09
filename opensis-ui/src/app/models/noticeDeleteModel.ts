import { CommonField } from './commonField';

export class NoticeDeleteModel extends CommonField {
    public NoticeId: number;
    public schoolId: number;
    public tenantId: string
    constructor() {
        super();
        this.NoticeId = 0;
        this._tenantName=sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem('token');
        this.schoolId =  + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
    }
}
