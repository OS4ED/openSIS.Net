import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'weekDay'
})
export class WeekDayPipe implements PipeTransform {
    weeks = [
        { name: 'SUN',altName:'S',fullName:'Sunday', id: 0 },
        { name: 'MON',altName:'M',fullName:'Monday', id: 1 },
        { name: 'TUE',altName:'T',fullName:'Tuesday', id: 2 },
        { name: 'WED',altName:'W',fullName:'Wednesday', id: 3 },
        { name: 'THU',altName:'H',fullName:'Thursday', id: 4 },
        { name: 'FRI',altName:'F',fullName:'Friday', id: 5 },
        { name: 'SAT',altName:'S',fullName:'Saturday', id: 6 }
    ];

    transform(value,scheduling?:boolean,toolTip?:boolean): number | string {
        if (value) {
            if(scheduling){
                let altName;
                value = value.split('|').join('')
                for (let day of this.weeks) {
                    if(day.id==+value){
                        altName=day.altName;
                        break;
                    }
                }
                return altName;
            }else if(toolTip){
                let fullName;
                value = value.split('|').join('')
                for (let day of this.weeks) {
                    if(day.id==+value){
                        fullName=day.fullName;
                        break;
                    }
                }
                return fullName;
            }else{
                let finalString = [];
                value = value.split('|');
                for (let j = 0; j < this.weeks.length; j++) {
                    for (let i = 0; i < value.length; i++) {
                        if (value[i] == this.weeks[j].fullName.toLowerCase()) {
                            finalString.push(this.weeks[j].name);
                        }
                    }
                }
                return finalString.join(' - ');
            }
        
        } else {
            return value;
        }

    }

}