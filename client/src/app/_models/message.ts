export interface Message {
    id: number
    senderId: number
    senderPhotoUrl: string
    senderUsername: string
    recipientId: number
    recipientUsername: string
    recipientPhotoUrl: string
    content: string
    dateRead: Date
    messageSent: Date
  }