export interface FlowerBouquetResponse {
    flowerBouquetId: number;
    categoryId: number;
    flowerBouquetName: string;
    description: string;
    unitPrice: number;
    unitsInStock: number;
    flowerBouquetStatus: number | null;
    supplierId: number | null;
    morphology: string | null;
    category: CategoryResponse;
    supplier: SupplierResponse | null;
}