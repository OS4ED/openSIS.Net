import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'filter'
})
export class NgForFilterPipe implements PipeTransform {

    transform(value: any, searchTerm: any) {
        if (value?.length === 0) {
            return value;
        }
        if (searchTerm) {
            return value.filter((term) => {
                return term.courseSection.courseSectionName?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.courseSection.mpTitle?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.courseSection.seats.toString().indexOf(searchTerm.toString())>-1
            })
        } else {
            return value;
        }


    }

}