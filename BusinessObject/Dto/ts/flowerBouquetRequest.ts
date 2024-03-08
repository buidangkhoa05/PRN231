export interface FlowerBouquetRequest {
    categoryId: number;
    flowerBouquetName: string;
    description: string;
    unitPrice: number;
    unitsInStock: number;
    flowerBouquetStatus: number | null;
    supplierId: number | null;
    morphology: string | null;
}

export interface FlowerBouquetValidator extends AbstractValidator<FlowerBouquetRequest> {

}