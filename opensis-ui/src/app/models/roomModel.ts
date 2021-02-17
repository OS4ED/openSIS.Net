import { CommonField } from "../models/commonField";



export class RoomModel  {

     tenantId: string
     schoolId: number
     roomId: number
     title: string
     capacity : number
     description: string
     sortOrder: number
     isActive:boolean
     lastUpdated: string
     updatedBy: string
    constructor() {
        
        this.tenantId =sessionStorage.getItem("tenantId");
        this.schoolId =+sessionStorage.getItem("selectedSchoolId");
        this.roomId=0;
        this.title = null
        this.capacity=null;
        this.description=null;
        this.sortOrder=null;
        this.isActive=null;
        this.lastUpdated=null;
        this.updatedBy=sessionStorage.getItem('email');

    }
}
export class RoomAddView extends CommonField{
    tableRoom:RoomModel

    constructor() {
        super();
        this._tenantName=sessionStorage.getItem("tenant")
        this._token=sessionStorage.getItem("token")
        this.tableRoom=new RoomModel()
    }
}
export class RoomListViewModel extends CommonField {
    public tableroomList: [RoomModel];
    public tenantId=sessionStorage.getItem('tenantId');
    public schoolId: number;
    constructor() {
        super();
        this.schoolId=+sessionStorage.getItem("selectedSchoolId")
        this._tenantName=sessionStorage.getItem("tenant")
        this.tenantId=sessionStorage.getItem("tenantId")
        this._token=sessionStorage.getItem("token")
    }
}
