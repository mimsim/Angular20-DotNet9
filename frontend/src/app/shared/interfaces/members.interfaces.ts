export interface IMember {
    // id: string;
    // displayName: string;
    email: string;

    id: string;
    userId: string;
    dateOfBirth: string;
    imageUrl: string;
    displayName: string;
    created: string;
    lastActive: string;
    gender: string;
    description?: string;
    city: string;
    country: string;
}

export interface IPhoto{
    id: string;
    url: string;
    publicId: string;
    memberId: string;   
}