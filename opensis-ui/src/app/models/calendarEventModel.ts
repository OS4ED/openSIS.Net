import { CommonField } from './commonField';


export class CalendarEventModel {
    public tenantId: string;
    public schoolId: number;
    public calendarId: number;
    public eventId :number;
    public title: string;
    public academicYear: number;
    public schoolDate: string;
    public description: string;
    public lastUpdated: string;
    public updatedBy: string;
    public visibleToMembershipId :string;
    public eventColor :string;
    public systemWideEvent :boolean;
    public startDate :string;
    public endDate : string;
   
    constructor() {
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this.updatedBy = sessionStorage.getItem("email");
    }
}

export class CalendarEventAddViewModel extends CommonField {
    public schoolCalendarEvent: CalendarEventModel;
    constructor() {
        super();
        this.schoolCalendarEvent = new CalendarEventModel();
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName=sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
    }
}


export class CalendarEventListViewModel extends CommonField {
    public calendarEventList: CalendarEventModel[];
    public tenantId: string;
    public schoolId: number;
    public calendarId: number;
    public academicYear: number;
    constructor() {
        super();
        this.calendarEventList = [];
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
        this.academicYear = +sessionStorage.getItem("academicyear");
    }
}

export interface colors {
    name: string;
    value: string;
    
  }