import { CommonField } from './common-field.model';

class EffortGradeScale {
    tenantId: string;
    schoolId: number;
    effortGradeScaleId: number;
    gradeScaleValue: number;
    gradeScaleComment: string;
    sortOrder: number;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
    constructor() {
        this.createdBy = sessionStorage.getItem("email");
        this.updatedBy = sessionStorage.getItem("email");
    }
}

export class EffortGradeScaleModel extends CommonField {
    effortGradeScale: EffortGradeScale;
    constructor() {
        super();
        this.effortGradeScale=new EffortGradeScale();
    }
}

export class GetAllEffortGradeScaleListModel extends CommonField {
    tenantId: string;
    schoolId: number;
    pageNumber: number;
    pageSize: number;
    sortingModel: sorting;
    filterParams: filterParams;
    _tenantName: string;
    _token: string;
    _failure: boolean;
    _message: string;
    constructor() {
        super();
        this.pageNumber=1;
        this.pageSize=10;
        this.sortingModel=new sorting();
        this.filterParams=null;
    }
}

class GradeScaleForView{
    tenantId: string;
    schoolId: number;
    effortGradeScaleId: number;
    gradeScaleValue: number;
    gradeScaleComment: string;
    sortOrder: number;
}

export class EffortGradeScaleListModel{
    getEffortGradeScaleForView:[GradeScaleForView];
    effortGradeScaleList:[GradeScaleForView];
    tenantId: string;
    schoolId: number;
    totalCount: number;
    pageNumber:number;
    _pageSize: number;
    _tenantName: string;
    _token: string;
    _failure: boolean;
    _message: string;
}

export class UpdateEffortGradeScaleSortOrderModel extends CommonField{
  tenantId: string;
  schoolId: number;
  previousSortOrder: number;
  currentSortOrder: number;
  constructor() {
    super();
}
}

class sorting {
    sortColumn: string;
    sortDirection: string;
    constructor() {
        this.sortColumn = "";
        this.sortDirection = "";
    }
}

export class filterParams {
    columnName: string;
    filterValue: string;
    filterOption: number;
    constructor() {
        this.columnName = null;
        this.filterOption = 3;
        this.filterValue = null;
    }
}
export class GradeModel{
    tenantId: string
    schoolId: number
    gradeScaleId:number
    gradeId: number
    title: string
    breakoff: number
    weightedGpValue: number
    unweightedGpValue: number
    comment: string
    sortOrder: number
    createdBy: string
    createdOn: string
    updatedBy: string
    updatedOn: string
    constructor(){
        this.gradeId=0;
        this.title="";
        this.breakoff=0;
        this.weightedGpValue=0;
        this.unweightedGpValue=0;
        this.comment="";
        this.sortOrder=0;
        this.createdBy=sessionStorage.getItem('email');
        this.createdOn=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.updatedOn=null;
    }
}
export class GradeAddViewModel extends CommonField{
    grade:GradeModel;
    constructor(){
        super()
        this.grade= new GradeModel();
    }
}
export class GradeListView extends CommonField{
    public gradeList: [GradeModel];
    schoolId: number;
    tenantId: string;
    constructor(){
        super();
    }
}
export class GradeDragDropModel extends CommonField{
    tenantId:string;
    schoolId:number;
    previousSortOrder: number;
    currentSortOrder:number;
    gradeScaleId:number;
    constructor(){
        super();
        this.previousSortOrder=0;
        this.currentSortOrder=0;
        this.gradeScaleId=0;
    }


}
export class GradeScaleModel{
    tenantId: string
    schoolId:number
    gradeScaleId: number
    gradeScaleName: string
    gradeScaleValue: number
    gradeScaleComment: string
    calculateGpa: boolean
    useAsStandardGradeScale: boolean
    sortOrder: number
    createdBy: string
    createdOn: string
    updatedBy: string
    updatedOn: string
    grade:[GradeModel]
    constructor(){
        this.gradeScaleId=0;
        this.calculateGpa=true;
        this.createdBy=sessionStorage.getItem('email');
        this.createdOn=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.updatedOn=null;
    }
}
export class GradeScaleAddViewModel extends CommonField{
    gradeScale:GradeScaleModel;
    constructor(){
        super();
        this.gradeScale = new GradeScaleModel();
    }
}
export class GradeScaleListView extends CommonField{
    public gradeScaleList: GradeScaleModel[];
    schoolId: number;
    tenantId: string;
    constructor(){
        super();
    }
}


export class SchoolSpecificStandarModel extends CommonField{
    gradeUsStandard:GradeUsStandard;

    constructor(){
        super();
        this.gradeUsStandard = new GradeUsStandard();
    }
}

export class GradeUsStandard{
    tenantId: string;
    schoolId: number;
    standardRefNo: string;
    gradeStandardId: number;
    gradeLevel: string;
    domain: string;
    subject: string;
    course: string;
    topic: string;
    standardDetails: string;
    isSchoolSpecific: boolean;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
    courseStandard:[]

    constructor(){
        this.createdBy=sessionStorage.getItem('email');
        this.createdOn=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.updatedOn=null;
    }
}

export class GradeStandardSubjectCourseListModel extends CommonField{
    gradeUsStandardList:[StandardView];
    tenantId: string;
    schoolId: number;
    totalCount: number;
    pageNumber: number;
    _pageSize: number;

    constructor(){
        super();
        this.gradeUsStandardList=[new StandardView()]
        this.pageNumber=0;
        this._pageSize=0;
    }

}

export class StandardView{
    tenantId: string;
    schoolId: number;
    standardRefNo: string;
    gradeStandardId: number;
    gradeLevel: string;
    domain: string;
    subject: string;
    course: string;
    topic: string;
    standardDetails: string
}

export class GetAllSchoolSpecificListModel extends CommonField {
    gradeUsStandardList:[StandardView];
    tenantId: string;
    schoolId: number;
    pageNumber: number;
    totalCount:number;
    pageSize: number;
    _pageSize:number;
    sortingModel: sorting;
    filterParams: filterParams;
   
    constructor() {
        super();
        this.pageNumber=1;
        this.pageSize=10;
        this.sortingModel=new sorting();
        this.filterParams=null;
    }
}

export class CheckStandardRefNoModel extends CommonField{
    tenantId: string;
    schoolId: number;
    standardRefNo: string;
    isValidStandardRefNo: boolean;

    constructor(){
        super();
    }
  
}

export class EffortGradeLibraryCategoryItemModel{
    tenantId: string;
    schoolId: number;
    effortItemId: number;
    effortCategoryId: number;
    effortItemTitle: string;
    sortOrder: number;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;

    constructor(){
        this.effortItemId=0;
        this.effortCategoryId=0;
        this.sortOrder=0;
        this.createdBy=sessionStorage.getItem('email');
        this.createdOn=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.updatedOn=null;
    }

}
export class  EffortGradeLibraryCategoryModel {
    tenantId: string;
    schoolId: number;
    effortCategoryId: number;
    categoryName: string;
    sortOrder: number;
    createdBy: string;
    createdOn: string;
    updatedBy: string;
    updatedOn: string;
    effortGradeLibraryCategoryItem:EffortGradeLibraryCategoryItemModel[]
    constructor(){
        this.effortCategoryId=0;
        this.sortOrder=0;
        this.createdBy=sessionStorage.getItem('email');
        this.createdOn=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.updatedOn=null;
        this.effortGradeLibraryCategoryItem=[];
    }
  }
  export class EffortGradeLibraryCategoryAddViewModel extends CommonField{
    effortGradeLibraryCategory:EffortGradeLibraryCategoryModel;
      constructor(){
        super();
        this.effortGradeLibraryCategory= new EffortGradeLibraryCategoryModel();
      }
  }
  export class EffortGradeLibraryCategoryListView extends CommonField{
    public effortGradeLibraryCategoryList:[EffortGradeLibraryCategoryModel]
      tenantId:string;
      schoolId:number;
      constructor(){
          super();
      }
  }
export class EffortGradeLibraryCategoryItemAddViewModel extends CommonField{
    effortGradeLibraryCategoryItem:EffortGradeLibraryCategoryItemModel
    constructor(){
        super();
        this.effortGradeLibraryCategoryItem=new EffortGradeLibraryCategoryItemModel();
    }
}
export class EffortGradeLlibraryDragDropModel extends CommonField{
        tenantId: string;
        schoolId: number;
        effortCategoryId: number;
        previousSortOrder: number;
        currentSortOrder: number;
        constructor(){
            super();
            this.effortCategoryId=0;
            this.previousSortOrder=0;
            this.currentSortOrder=0;
        }
}
export class HonorRollModel {
      tenantId: string
      schoolId: number
      markingPeriodId: number
      honorRollId: number
      honorRoll: string
      breakoff: number
      createdBy:string
      createdOn: string
      updatedBy: string
      updatedOn: string
      constructor(){
          this.markingPeriodId=+sessionStorage.getItem("markingPeriodId");
          this.honorRollId=0;
          this.createdBy=sessionStorage.getItem('email');
          this.createdOn=null;
          this.updatedBy=sessionStorage.getItem('email');
          this.updatedOn=null;
      }
  }
  export class HonorRollAddViewModel extends CommonField{
    honorRolls:HonorRollModel;
    constructor(){
        super();
        this.honorRolls=new HonorRollModel();
    }
  }
  export class HonorRollListModel extends CommonField {
    honorRollList:[];
    tenantId: string;
    schoolId: number;
    pageNumber: number;
    totalCount:number;
    pageSize: number;
    sortingModel: sorting;
    filterParams: filterParams;
   
    constructor() {
        super();
        this.pageNumber=1;
        this.pageSize=10;
        this.sortingModel=new sorting();
        this.filterParams=null;
    }
}
export class GetHonorRollModel extends CommonField {
    tenantId:string;
    pageNumber: number;
    pageSize: number;
    sortingModel:sorting;
    filterParams:filterParams;
    constructor() {
        super()
        this.pageNumber=1;
        this.pageSize=10;
        this.sortingModel=new sorting();
        this.filterParams=null;
        this._failure=false;
        this._message="";
    }
}
