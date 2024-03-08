export interface SupplierRequest {
    supplierName: string | null;
    supplierAddress: string | null;
    telephone: string | null;
}

export interface SupplierValidator extends AbstractValidator<SupplierRequest> {

}