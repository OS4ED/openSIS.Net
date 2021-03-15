import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'EmtyValueCheckPipe'
})
export class EmtyValueCheckPipe implements PipeTransform {

  transform(value: string,type?: string): string {
    if(type==="0.00"){
      if(value!==null && value!==undefined){
        if(value.trim()!==""){
          return value.trim();
        }else{
          return "0.00";
        }
      }
      else{
        return "0.00"
      }
    }
    else{
      if(value!==null && value!==undefined){
        if(value.trim()!==""){
          return value.trim();
        }else{
          return "-";
        }
      }
      else{
        return "-"
      }
    }
  }
}
