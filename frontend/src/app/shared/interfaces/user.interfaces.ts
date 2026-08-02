export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    Id: string;
    // добави други полета според твоя backend отговор
}
