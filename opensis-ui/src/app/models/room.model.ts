import { CommonField } from "./common-field.model";



export class RoomModel  {

     tenantId: string;
     schoolId: number;
     roomId: number;
     title: string;
     capacity: number;
     description: string;
     sortOrder: number;
     isActive: boolean;
     lastUpdated: string;
     updatedBy: string;
    constructor() {
        this.roomId = 0;
        this.title = null;
        this.capacity = null;
        this.description = null;
        this.sortOrder = null;
        this.isActive = null;
        this.lastUpdated = null;
        this.updatedBy = sessionStorage.getItem('email');

    }
}
export class RoomAddView extends CommonField{
    tableRoom: RoomModel;

    constructor() {
        super();
        this.tableRoom = new RoomModel();
    }
}
export class RoomListViewModel extends CommonField {
    public tableroomList: [RoomModel];
    public schoolId: number;
    constructor() {
        super();
    }
}
