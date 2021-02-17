import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'weekDay'
})
export class WeekDayPipe implements PipeTransform {
    weeks = [
        { name: 'SUN', id: 0 },
        { name: 'MON', id: 1 },
        { name: 'TUE', id: 2 },
        { name: 'WED', id: 3 },
        { name: 'THU', id: 4 },
        { name: 'FRI', id: 5 },
        { name: 'SAT', id: 6 }
    ];

    transform(value): number | string {
        if (value) {
            let finalString = [];
            value = value.split('|').join('')
            for (let j = 0; j < this.weeks.length; j++) {
                for (let i = 0; i < value.length; i++) {
                    if (value[i] == this.weeks[j].id) {
                        finalString.push(this.weeks[j].name);
                    }
                }
            }
            return finalString.join(' - ');
        } else {
            return value;
        }

    }

}