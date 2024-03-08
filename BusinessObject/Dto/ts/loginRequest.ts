export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginRequestValidator extends AbstractValidator<LoginRequest> {

}