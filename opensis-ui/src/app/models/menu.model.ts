export interface MenuModel {
    type?: string;
    label?: string;
    icon?: any;
    route?: string;
    children?: MenuModel[];
}