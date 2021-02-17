import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
@Pipe({
    name: 'age'
})
export class AgePipe implements PipeTransform {

    transform(value): any {
        if(value!=null){
            let dob = new Date(value);
            if (!dob) return dob;
              return moment().diff(dob, 'years')+" Years";
        }else{
            return "0 Years"
        }
    
    }

}