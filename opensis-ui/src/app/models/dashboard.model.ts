import { CalendarEventModel } from "./calendar-event.model";
import { CalendarModel } from "./calendar.model";
import { CommonField } from "./common-field.model";


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
}