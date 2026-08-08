import { Component, inject,  signal } from '@angular/core';
import { IMessage } from '../../shared/interfaces/messages.interface';
import { MessageService } from '../../shared/services/message-service';
import { MaterialModule } from '../../material.module';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-messages-component',
  imports: [MaterialModule, DatePipe],
  templateUrl: './messages-component.html',
  styleUrl: './messages-component.scss',
})
export class MessagesComponent {
  private readonly messageService = inject(MessageService);

  protected messages = signal<IMessage[]>([]);
  protected isLoading = signal(false);
  protected container = 'Inbox';

  protected pageNumber = 1;
  protected pageSize = 10;
  protected totalCount = 0;
  get displayedColumns(): string[] {
    const nameColumn = this.container === 'Outbox' ? 'recipientDisplayName' : 'senderDisplayName';
    return [nameColumn, 'content', 'messageSent', 'dateRead'];
  }

  tabs = [
    { label: 'Inbox', value: 'Inbox' },
    { label: 'Outbox', value: 'Outbox' },
    { label: 'Unread', value: 'Unread' },
  ];

  ngOnInit(): void {
    this.loadMessages();
  }

  onTabChange(index: number) {
    this.container = this.tabs[index].value;
    this.pageNumber = 1;
    this.loadMessages();
  }

  // onPageChange(event: any) {
  //   this.pageNumber = event.pageIndex + 1;
  //   this.pageSize = event.pageSize;
  //   this.loadMessages();
  // }

  loadMessages() {
    this.isLoading.set(true);

    this.messageService.getMessages(this.container, this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.messages.set(result.items);
        this.totalCount = result.totalCount;
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  }
}
