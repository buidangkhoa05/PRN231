export interface OrderRequest {
    orderDetails: OrderDetailRequest[];
}

export interface OrderReqValidator extends AbstractValidator<OrderRequest> {

}

export interface OrderDetailRequest {
    flowerBouquetId: number;
    quantity: number;
    discount: number;
}

export interface OrderDetailReqValidator extends AbstractValidator<OrderDetailRequest> {

}