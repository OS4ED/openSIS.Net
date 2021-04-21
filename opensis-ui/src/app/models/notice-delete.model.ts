import { CommonField } from './common-field.model';

export class NoticeDeleteModel extends CommonField {
    public NoticeId: number;
    public schoolId: number;
    public tenantId: string;
    constructor() {
        super();
        this.NoticeId = 0;
    }
}
