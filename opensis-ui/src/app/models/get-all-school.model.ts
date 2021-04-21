import { SchoolMasterModel } from "./school-master.model";

export class GetAllSchoolModel {
    tenantId: string;
    pageNumber: number;
    pageSize: number;
    sortingModel: sorting;
    filterParams: filterParams;
    _tenantName: string;
    _token: string;
    _failure: boolean;
    _message: string;
    _userName: string;
    includeInactive: boolean;
    constructor() {
        this.pageNumber = 1;
        this.pageSize = 10;
        this.sortingModel = new sorting();
        this.filterParams = null;
        this.includeInactive = false;
        this._failure = false;
        this._message = "";
    }
}

export class AllSchoolListModel {
    schoolMaster: [SchoolMasterModel];
    totalCount: number;
    pageNumber: number;
    _pageSize: number;
    _tenantName: string;
    _token: string;
    _failure: boolean;
    _message: string;
}

class OnlySchoolList {
    schoolId: number;
    tenantId: string;
    schoolName: string;
    dateSchoolOpened: string;
    dateSchoolClosed: string;
    streetAddress1: string;
    nameOfPrincipal: string;
    telephone: string;
    status: boolean;
}

export class OnlySchoolListModel {
    schoolMaster: [SchoolMasterModel];
    tenantId: string;
    totalCount: number;
    pageNumber: number;
    _pageSize: number;
    _tenantName: string;
    _userName: string;
    _token: string;
    _failure: boolean;
    _message: string
}

class sorting {
    sortColumn: string;
    sortDirection: string;
    constructor() {
        this.sortColumn = "";
        this.sortDirection = "";
    }
}

export class filterParams {
    columnName: string;
    filterValue: string;
    filterOption: number;
    constructor() {
        this.columnName = null;
        this.filterOption = 3;
        this.filterValue = null;
    }
}


