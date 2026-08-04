import { Component, OnInit, inject, signal } from '@angular/core';
import { MembersService } from '../shared/services/members-service';
import { IMember } from '../shared/interfaces/members.interfaces';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-members-component',
  imports: [JsonPipe],
  standalone: true,
  templateUrl: './members-component.html',
  styleUrl: './members-component.css',
})
export class MembersComponent implements OnInit {
  private membersService = inject(MembersService);
  members = signal<IMember[]>([]);

  ngOnInit(): void {
    this.membersService.getMembers().subscribe(data => {
      this.members.set(data);
    });
  }
}
