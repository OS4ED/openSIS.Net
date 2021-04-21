import { CommonField } from "./common-field.model";

export class TableCityModel {    

    public id: number;
    public name: string;
    public stateId: number;
   
    constructor(){
        this.id= null;
        this.name=null;
        this.stateId=null;
        
    }
    
}


export class CityModel extends CommonField {
    
    public tableCity : [TableCityModel];
    public stateId:number;
}