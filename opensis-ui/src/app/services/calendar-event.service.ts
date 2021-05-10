import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { CalendarAddViewModel, CalendarListModel } from '../models/calendar.model';
import { CalendarEventAddViewModel, CalendarEventListViewModel } from '../models/calendar-event.model';
import { BehaviorSubject } from 'rxjs';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
    providedIn: 'root'
})
export class CalendarEventService {
    private eventSource = new BehaviorSubject(false);
    currentEvent = this.eventSource.asObservable();

    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

    addCalendarEvent(calendarEvent: CalendarEventAddViewModel) {
        calendarEvent = this.defaultValuesService.getAllMandatoryVariable(calendarEvent);
        calendarEvent.schoolCalendarEvent.schoolId = this.defaultValuesService.getSchoolID();
        calendarEvent.schoolCalendarEvent.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + calendarEvent._tenantName + "/CalendarEvent/addCalendarEvent";
        return this.http.post<CalendarEventAddViewModel>(apiurl, calendarEvent)
    }
    viewCalendarEvent(calendarEvent: CalendarEventAddViewModel) {
        calendarEvent = this.defaultValuesService.getAllMandatoryVariable(calendarEvent);
        calendarEvent.schoolCalendarEvent.schoolId = this.defaultValuesService.getSchoolID();
        calendarEvent.schoolCalendarEvent.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + calendarEvent._tenantName + "/CalendarEvent/viewCalendarEvent";
        return this.http.post<CalendarEventAddViewModel>(apiurl, calendarEvent)
    }

    updateCalendarEvent(calendarEvent: CalendarEventAddViewModel) {
        calendarEvent = this.defaultValuesService.getAllMandatoryVariable(calendarEvent);
        calendarEvent.schoolCalendarEvent.schoolId = this.defaultValuesService.getSchoolID();
        calendarEvent.schoolCalendarEvent.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + calendarEvent._tenantName + "/CalendarEvent/updateCalendarEvent";
        return this.http.put<CalendarEventAddViewModel>(apiurl, calendarEvent)
    }

    deleteCalendarEvent(calendarEvent: CalendarEventAddViewModel) {
        calendarEvent = this.defaultValuesService.getAllMandatoryVariable(calendarEvent);
        let apiurl = this.apiUrl + calendarEvent._tenantName + "/CalendarEvent/deleteCalendarEvent";
        return this.http.post<CalendarEventAddViewModel>(apiurl, calendarEvent)
    }

    getAllCalendarEvent(calendarEvent: CalendarEventListViewModel) {
        calendarEvent = this.defaultValuesService.getAllMandatoryVariable(calendarEvent);
        calendarEvent.academicYear= this.defaultValuesService.getAcademicYear();
        let apiurl = this.apiUrl + calendarEvent._tenantName + "/CalendarEvent/getAllCalendarEvent";
        return this.http.post<CalendarEventListViewModel>(apiurl, calendarEvent)
    }
    changeEvent(message: boolean) {
        this.eventSource.next(message)
    }
}
