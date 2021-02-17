import { Pipe, PipeTransform } from '@angular/core';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import {SharedFunction} from '../../shared/shared-function'
const moment =  _rollupMoment || _moment;
@Pipe({
  name: 'transform24to12Pipe'
})
export class Transform24to12Pipe implements PipeTransform {
 constructor(public commonfunction:SharedFunction ){}
  transform(value: string): string {
    if(value!==null && value!==undefined){   
        let time:any = value.toString ().match (/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [value];

        if (time.length > 1) { // If time format correct
          time = time.slice (1);  // Remove full string match value
          time[5] = +time[0] < 12 ? 'AM' : 'PM'; // Set AM/PM
          time[0] = +time[0] % 12 || 12; // Adjust hours
        }
       
      return time.join ('');;
        
    }
    else{
      return "-"
    }
  }

}