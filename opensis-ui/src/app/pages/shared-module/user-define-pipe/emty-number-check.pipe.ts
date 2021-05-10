import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'EmtyNumberCheckPipe'
})
export class EmtyNumberCheckPipe implements PipeTransform {

  transform(value: number): string {
    if (value){
      return value.toString();
    }
    else{
      return '-';
    }
  }

}
