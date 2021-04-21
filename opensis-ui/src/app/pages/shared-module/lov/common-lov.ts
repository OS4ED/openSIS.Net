import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { DefaultValuesService } from '../../../common/default-values.service';
import { LovList } from '../../../models/lov.model';
import { CommonService } from '../../../services/common.service';
import { SchoolService } from '../../../services/school.service';

@Injectable({
    providedIn: 'root'
})
export class CommonLOV {
    lovList: LovList = new LovList();

    constructor(private commonService: CommonService,
        private schoolService:SchoolService, private defaultValuesService:DefaultValuesService) { }

      getLovByName(LovName) {
        let schoolId=this.defaultValuesService.getSchoolID();
        if(schoolId!=null){
            this.lovList.schoolId=+schoolId;
            this.lovList._token = this.defaultValuesService.getToken();
        }else{
            this.lovList.schoolId = this.defaultValuesService.getSchoolID();
            this.lovList._token = this.defaultValuesService.getToken();
        }
        this.lovList.lovName = LovName;
        return this.commonService.getAllDropdownValues(this.lovList)
            .pipe(
                map((res:LovList) => {
                    if(LovName!=='Grade Level'){
                        res.dropdownList?.sort((a, b) => {return a.lovColumnValue < b.lovColumnValue ? -1 : 1;} )   
                    }
              return res.dropdownList;
            }))
    }
}
