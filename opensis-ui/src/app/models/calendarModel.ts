import { CommonField } from './commonField';
import { SchoolMasterModel } from './schoolMasterModel';

export class CalendarModel {
    public tenantId: string;
    public schoolId: number;
    public calenderId: number;
    public title: string;
    public academicYear: number;
    public defaultCalender: boolean;
    public days: string;
    public rolloverId: number;
    public lastUpdated: string;
    public updatedBy: string;
    public visibleToMembershipId :string;
    public startDate :string;
    public endDate : string;
   
    constructor() {
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this.updatedBy = sessionStorage.getItem("email");
        
    }
}
export interface Weeks {
    name: string;
    id: number;
  }

export class CalendarAddViewModel extends CommonField {
    public schoolCalendar: CalendarModel;
    constructor() {
        super();
        this.schoolCalendar = new CalendarModel();
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
    }
}


export class CalendarListModel extends CommonField {
    public calendarList: CalendarModel[];
    public tenantId: string;
    public schoolId: number;
    public academicYear :number;
    constructor() {
        super();
        this.calendarList = [];
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
        this.academicYear = +sessionStorage.getItem("academicyear");
    }
}