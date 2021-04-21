import { CommonField } from "./common-field.model";
import { Membership } from "./membership.model";


export class RolePermission {
    public tenantId: string;
    public schoolId: number;
    public rolePermissionId: number;
    public membershipId: number;
    public permissionCategoryId: number;
    public canView: boolean;
    public canAdd: boolean;
    public canEdit: boolean;
    public canDelete: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public membership: Membership;
    public permissionCategory: PermissionCategory;

}

export class PermissionCategory {
    public tenantId: string;
    public schoolId: number;
    public permissionCategoryId: number;
    public permissionGroupId: number;
    public permissionCategoryName: string;
    public permissionSubcategoryId: number;
    public permissionSubcategoryName: string;
    public icon;
    public shortCode: string;
    public path: string;
    public title: string;
    public type: string;
    public enableView: boolean;
    public enableAdd: boolean;
    public enableEdit: boolean;
    public enableDelete: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public permissionGroup: PermissionGroup;
    public rolePermission: RolePermission[];
    public permissionSubcategory: PermissionSubCategory[];

    constructor() {
        this.rolePermission = [new RolePermission];
        this.permissionSubcategory = [new PermissionSubCategory];
    }
}
export class PermissionSubCategory {
    public tenantId: string;
    public schoolId: number;
    public permissionSubcategoryId: number;
    public permissionGroupId: number;
    public permissionSubcategoryName: string;
    public shortCode: string;
    public path: string;
    public title: string;
    public Type: string;
    public enableView: boolean;
    public enableAdd: boolean;
    public enableEdit: boolean;
    public enableDelete: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public permissionGroup: PermissionGroup;
    public rolePermission: RolePermission[];
    constructor() {
        this.rolePermission = [new RolePermission];
    }
}

export class PermissionGroup {
    public tenantId: string;
    public schoolId: number;
    public permissionGroupId: number;
    public permissionGroupName: string;
    public shortName: string;
    public isActive: boolean;
    public isSystem: boolean;
    public title: string;
    public icon: string;
    public iconType: string;
    public sortOrder: number;
    public type: string;
    public path: string;
    public badgeType: string;
    public badgeValue: string;
    public active: boolean;
    public createdBy: string;
    public createdOn: string;
    public updatedBy: string;
    public updatedOn: string;
    public permissionCategory: PermissionCategory[];
    public rolePermission: RolePermission[];
    constructor() {
        this.permissionCategory = [new PermissionCategory];
        this.rolePermission = [new RolePermission];
    }
}


export class PermissionGroupListViewModel extends CommonField {
    public permissionGroupList: PermissionGroup[];
    public tenantId: string;
    public schoolId: number;
    constructor() {
        super()
        this.permissionGroupList = [new PermissionGroup];
    }
}

export class RolePermissionListViewModel extends CommonField {
    public permissionList: RolePermissionViewModel[];
    public tenantId: string;
    public schoolId: number;
    public membershipId: number;
    constructor() {
        super()
        this.membershipId = 1;
        this.permissionList = [new RolePermissionViewModel]
        this.membershipId = +sessionStorage.getItem("userMembershipID");
    }
}

export class RolePermissionViewModel extends CommonField {
    public permissionGroup: PermissionGroup;
    constructor() {
        super()
        this.permissionGroup = new PermissionGroup;
    }
}

export class PermissionGroupAddViewModel extends CommonField {
    public permissionGroup: PermissionGroup;
}