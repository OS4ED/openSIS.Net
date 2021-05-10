export class CertificateModel{
    id: number;
    tenantId: string;
    schoolId: number;
    staffId: number;
    certificationName: string;
    shortName: string;
    certificationCode: string;
    primaryCertification: boolean;
    certificationDate: string;
    certificationExpiryDate: string;
    certificationDescription: string;
    updatedBy: string;
    updatedAt: string;
    constructor(){
        this.id = 0;
        this.staffId = 0;
        this.updatedAt = null;
    }
}
