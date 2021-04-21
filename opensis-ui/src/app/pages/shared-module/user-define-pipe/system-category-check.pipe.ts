import { Pipe, PipeTransform } from '@angular/core';
import { CustomFieldModel } from 'src/app/models/custom-field.model';

@Pipe({
    name: 'systemCategoryCheckPipe'
})
export class SystemCategoryCheckPipe implements PipeTransform {
    transform(items: any[]): any {
        if (!items) {
            return items;
        }
        // filter items array, items which match and return true will be
        // kept, false will be filtered out
        return items.filter(item => !item.systemField);
    }
}