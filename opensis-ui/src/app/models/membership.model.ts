import { CommonField } from './common-field.model';
import { SchoolMasterModel } from './school-master.model';
import { UserViewModel } from './user.model';

export class Membership {
    public tenantId: string;
    public schoolId: number;
    public membershipId: number;
    public profile: string;
    public Title: string;
    public Access: string;
    public WeeklyUpdate: boolean;
    public LastUpdated: string;
    public UpdatedBy: string;
    public schoolMaster:SchoolMasterModel;
    public UserMaster: UserViewModel;
    public profileType:string;
    public isActive: boolean;
    public isSystem: boolean;
    public isSuperAdmin:boolean;
    public description: string;
    public createdBy:string;
    public createdOn: string;
    public updatedBy:string;
    public updatedOn: string;
    constructor() {
    }
}

export class GetAllMembersList extends CommonField {
    public getAllMemberList: Membership[];
    public tenantId: string;
    public schoolId: number;
    constructor() {
        super();
        this.getAllMemberList = [];
    }
}

export class AddMembershipModel extends CommonField {
    public membership: Membership;
    constructor() {
        super();
        this.membership = new Membership();
    }
}
