import{CommonField} from './common-field.model'
export class StudentCommentsModel{
    tenantId: string;
    schoolId: number;
    studentId: number;
    commentId: number;
    comment: string;
    updatedBy: string;
    lastUpdated: string;
    constructor(){
      this.studentId = 0;
      this.commentId = 0;
      this.comment = null;
      this.lastUpdated = null;
    }
  }
export class StudentCommentsAddView extends CommonField{
      studentComments: StudentCommentsModel;
      constructor(){
          super();
      }
  }
export class StudentCommentsListViewModel extends CommonField {
    public studentCommentsList: [StudentCommentsModel];
    public schoolId: number;
    public studentId: number;
    constructor() {
        super();
        this.studentId = 0;
    }
}