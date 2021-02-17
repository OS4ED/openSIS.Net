import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'EmtyBooleanCheckPipe'
})
export class EmtyBooleanCheckPipe implements PipeTransform {

  transform(value: boolean,type?: string): string {
    if(type=="status"){
      if(value===false){
        return "Inactive";
        
      }
      else if (value===true){
        return "Active";
      }
      else if(value ===null){
        return "-";
      }
     
      else{
        return "-";
      }
    }
    else{
      if(value===false){
        return "No";
        
      }
      else if (value===true){
        return "Yes";
      }
      else if(value ===null){
        return "-";
      }
     
      else{
        return "-";
      }
    }
 
  }

}
