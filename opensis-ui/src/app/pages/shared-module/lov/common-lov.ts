import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { LovList } from '../../../models/lovModel';
import { CommonService } from '../../../services/common.service';
import { SchoolService } from '../../../services/school.service';

@Injectable({
    providedIn: 'root'
})
export class CommonLOV {
    lovList: LovList = new LovList();

    constructor(private commonService: CommonService,
        private schoolService:SchoolService) { }

      getLovByName(LovName) {
        let schoolId=this.schoolService.getSchoolId()
        if(schoolId!=null){
            this.lovList.schoolId=+schoolId;
            this.lovList._token = sessionStorage.getItem('token');
        }else{
            this.lovList.schoolId = +sessionStorage.getItem("selectedSchoolId");
            this.lovList._token = sessionStorage.getItem('token');
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
