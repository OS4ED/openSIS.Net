import { RolePermissionListViewModel } from "./roll-based-access.model";

export interface MenuModel {
    type?: string;
    label?: string;
    icon?: any;
    route?: string;
    children?: MenuModel[];
}

// export class MenuStatusModel{
//     available?:boolean;
//     store?:RolePermissionListViewModel;
//     constructor(){
//         this.available=false;
//     }
// }