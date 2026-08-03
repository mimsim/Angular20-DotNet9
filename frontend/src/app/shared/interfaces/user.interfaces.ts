export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    Id: string;
    // добави други полета според твоя backend отговор
}

export interface RegisterRequest {
    displayName: string;
    email: string;
    password: string;
}

export interface RegisterResponse {
    token: string;
    userId: string;
    // добави други полета според твоя backend отговор
}
