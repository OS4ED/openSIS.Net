import { CommonField } from '../models/commonField';
import { CourseSection } from './courseSectionModel';
import { StudentMasterModel } from './studentModel';


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
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this.tenantId = sessionStorage.getItem("tenantId");
    }

}

export class ScheduleStudentListViewModel extends CommonField {
    public scheduleStudentForView: ScheduleStudentForView[];
    public tenantId: string;
    public schoolId: number;
    public courseSectionId: number;
    public totalCount: number;
    public pageNumber: number;
    public _pageSize: number;
    constructor() {
        super()
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this.tenantId = sessionStorage.getItem("tenantId");
        this.pageNumber=1;
        this._pageSize= 10;
    }
}

export class ScheduleStudentForView {
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public alternateId: string;
    public gradeLevel: string;
    public section: string;
    public phoneNumber: string;
    public action: string;
    public checked:boolean;
}


export class ScheduledStudentDropModel extends CommonField {
    public studentCoursesectionScheduleList: StudentCoursesectionSchedule[];
    public courseSectionId: number;
    public effectiveDropDate: string;
    constructor() {
        super()
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
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
    public scheduleReport : any;
    constructor(){
        super()
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }
}
