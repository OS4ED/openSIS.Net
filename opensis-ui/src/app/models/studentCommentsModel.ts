import{CommonField} from '../models/commonField'
export class StudentCommentsModel{
    tenantId: string;
    schoolId: number;
    studentId: number;
    commentId: number;
    comment: string;
    updatedBy: string;
    lastUpdated: string;
    constructor(){
      this.tenantId=sessionStorage.getItem("tenantId");
      this.schoolId=+sessionStorage.getItem("selectedSchoolId");
      this.studentId= 0;
      this.commentId= 0;
      this.comment=null;
      this.lastUpdated=null;
      this.updatedBy=sessionStorage.getItem("email");;
    }
  }
  export class StudentCommentsAddView extends CommonField{
      studentComments:StudentCommentsModel;
      constructor(){
          super();
          this.studentComments= new StudentCommentsModel();
          this._tenantName=sessionStorage.getItem("tenant");
          this._userName = sessionStorage.getItem("user");
          this._token=sessionStorage.getItem('token');
      }
  }
  export class StudentCommentsListViewModel extends CommonField {
    public studentCommentsList: [StudentCommentsModel];
    public tenantId=sessionStorage.getItem('tenantId');
    public schoolId: number;
    public studentId:number;
    constructor() {
        super();
        this.schoolId=+sessionStorage.getItem("selectedSchoolId");;
        this.studentId=0;
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this.tenantId=sessionStorage.getItem("tenantId");
        this._token=sessionStorage.getItem("token");
    }
}
  