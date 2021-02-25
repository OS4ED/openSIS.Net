import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'courseFilter'
})
export class CourseNgForFilterPipe implements PipeTransform {

    transform(value: any, searchTerm: any,filterFlag:boolean) {
        if (value?.length === 0) {
            return value;
        }
        if (searchTerm) {
            return value.filter((term) => { 
                if(filterFlag){
                    return term.courseTitle?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.courseSubject?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.courseProgram?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                }else{
                    return term.course.courseTitle?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.course.courseSubject?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                    || term.course.courseProgram?.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
                }                        
                
            })
        } else {
            return value;
        }


    }

}