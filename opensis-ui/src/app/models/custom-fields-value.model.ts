import { CommonField } from "./common-field.model";
export class CustomFieldsValueModel {
    tenantId: string;
    schoolId: number;
    categoryId: number;
    fieldId: number;
    targetId: number;
    module:string;
    customFieldType: string;
    customFieldTitle: string;
    customFieldValue: string;
    lastUpdate: string;
    updatedBy: string;
    constructor(){
        this.tenantId=sessionStorage.getItem('tenantId');
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
        this.fieldId=0;
        this.customFieldType="";
        this.customFieldTitle="";
        this.module="";
        this.customFieldValue="";
        this.categoryId=0;
        this.lastUpdate="2020-10-28T09:03:47";
        this.updatedBy="";
    }
  }