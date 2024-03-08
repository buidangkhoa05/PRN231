export interface CategoryRequest {
    categoryName: string;
    categoryDescription: string | null;
    categoryNote: string | null;
}

export interface CategoryValidator extends AbstractValidator<CategoryRequest> {

}