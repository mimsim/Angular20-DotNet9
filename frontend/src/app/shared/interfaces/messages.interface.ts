export interface IMessage {
    id: string;
    senderId: string;
    senderDisplayName: string;
    senderImageUrl: string | null;
    recipientId: string;
    recipientDisplayName: string;
    recipientImageUrl: string | null;
    content: string;
    dateRead: string | null;
    messageSent: string;
}

export interface IPaginatedResult<T> {
    items: T[];
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
}