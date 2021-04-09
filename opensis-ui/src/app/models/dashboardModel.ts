import { CalendarEventModel } from "./calendarEventModel";
import { CalendarModel } from "./calendarModel";
import { CommonField } from "./commonField";


export class DashboardViewModel extends CommonField{
    public tenantId: string;
    public schoolId: number;
    public superAdministratorName: string;
    public academicYear: number;
    public schoolName: string;
    public totalStudent: number;
    public totalStaff: number;
    public totalParent: number;
    public noticeTitle: string;
    public noticeBody :string;
    public schoolCalendar : CalendarModel;
    public calendarEventList : CalendarEventModel[];
   
    constructor() {
        super();
        this.schoolId = + sessionStorage.getItem('selectedSchoolId');
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem('tenant');
        this._userName = sessionStorage.getItem("user");
        this._token= sessionStorage.getItem("token");
        this.academicYear = +sessionStorage.getItem("academicyear");
        
        
    }
}