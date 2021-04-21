import { CommonField } from './common-field.model';
import { CourseSection } from './course-section.model';
import { StudentMasterModel } from './student.model';


export class StudentCourseSectionScheduleAddViewModel extends CommonField {

    public courseSectionList: CourseSection[];
    public studentMasterList: StudentMasterModel[];
    public tenantId: string;
    public schoolId: number;
    public createdBy: string;
    public updatedBy: string;
    public conflictMessage: string;
    constructor() {
        super();
    }

}

export class ScheduleStudentListViewModel extends CommonField {
    public scheduleStudentForView: ScheduleStudentForView[];
    public tenantId: string;
    public schoolId: number;
    public courseSectionId: number;
    public filterParams: filterParams;
    public pageSize: number;
    public pageNumber: number;
    public totalCount: number;
    public _pageSize: number; // this is from response.
    constructor() {
        super();
        this.pageNumber = 1;
        this._pageSize = 10;
    }
}

export class filterParams{
    columnName: string;
    filterValue: string;
    filterOption: number;
 constructor(){
     this.columnName=null;
     this.filterOption=1;
     this.filterValue=null;
 }
}

export class ScheduleStudentForView {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public firstGivenName: string;
    public lastFamilyName: string;
    public alternateId: string;
    public gradeLevel: string;
    public section: string;
    public phoneNumber: string;
    public action: string;
    public checked: boolean;
    public scheduleDate: string;
}


export class ScheduledStudentDropModel extends CommonField {
    public studentCoursesectionScheduleList: StudentCoursesectionSchedule[];
    public courseSectionId: number;
    public effectiveDropDate: string;
    constructor() {
        super();
    }
}
export class StudentCoursesectionSchedule {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public alternateId: string;
    public gradeLevel: string;
    public section: string;
    public phoneNumber: string;
    public action: string;
}

export class StudentScheduleReportViewModel extends CommonField{
    public tenantId: string;
    public schoolId: number;
    public scheduleReport: any;
    constructor(){
        super();
    }
}
