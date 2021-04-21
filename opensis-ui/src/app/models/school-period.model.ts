import { CommonField } from "./common-field.model";


export class Block {
    tenantId: string;
    schoolId: number;
    blockId: number;
    blockTitle: string;
    blockSortOrder: number;
    createdBy: string;
    updatedBy: string;
    blockPeriod: BlockPeriod[];
    createdOn: string;
    updatedOn: string;
    constructor() {
        this.blockId = 0;
        this.blockTitle = "";
        this.blockSortOrder = 0;
        this.updatedBy = null;
        this.blockPeriod = [];
    }

}

export class BlockAddViewModel extends CommonField {
    block: Block;
    constructor() {
        super();
        this.block = new Block();
    }
}


export class GetBlockListForView {
    tenantId: string;
    schoolId: number;
    blockId: number;
    blockTitle: string;
    blockSortOrder: number;
    createdBy: string;
    updatedBy: string;
    blockPeriod: BlockPeriod[];
    createdOn: string;
    updatedOn: string;
    constructor() {
        this.blockId = 0;
        this.blockTitle = "";
        this.blockSortOrder = 0;
        this.updatedBy = null;
        this.blockPeriod = [];
    }

}

export class BlockListViewModel extends CommonField {
    public getBlockListForView: GetBlockListForView[];
    public tenantId: string;
    public schoolId: number;
    constructor() {
        super();
    }
}


export class BlockPeriod {
    tenantId: string;
    schoolId: number;
    blockId: number;
    periodId: number;
    periodTitle: string;
    periodShortName: string;
    periodStartTime: string;
    periodEndTime: string;
    periodSortOrder: number;
    calculateAttendance: boolean;
    createdBy: string;
    updatedBy: string;
    createdOn: string;
    updatedOn: string;
    constructor() {
        this.blockId = 0;
        this.periodId = 0;
        this.periodTitle = "";
        this.periodSortOrder = 0;
        this.calculateAttendance= false;
        this.updatedBy = null;
    }

}

export class BlockPeriodAddViewModel extends CommonField {
    blockPeriod: BlockPeriod;
    constructor() {
        super();
        this.blockPeriod = new BlockPeriod();
    }
}


export class BlockPeriodSortOrderViewModel extends CommonField {
    tenantId: string;
    schoolId: number;
    previousSortOrder: number;
    currentSortOrder: number;
    blockId: number;
    constructor() {
        super();
        this.previousSortOrder = 0;
        this.currentSortOrder = 0;
        this.blockId = 0;
    }


}
