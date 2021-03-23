import{CommonField} from '../models/commonField';
import { CourseSection } from './courseSectionModel';
import { StudentMasterModel } from './studentModel';


export class StudentCourseSectionScheduleAddViewModel extends CommonField{

    public courseSectionList: CourseSection[];
    public studentMasterList : StudentMasterModel[];
    public createdBy : string;
    public updatedBy : string;
    constructor() {
        super();
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }

}