import { Component, OnInit, inject, signal } from '@angular/core';

import { RouterModule } from '@angular/router';
import { MaterialModule } from '../../material.module';
import { IMember } from '../../shared/interfaces/members.interfaces';
import { MembersService } from '../../shared/services/members-service';


@Component({
  selector: 'app-members-component',
  imports: [ MaterialModule, RouterModule],
  standalone: true,
  templateUrl: './members-component.html',
  styleUrl: './members-component.scss',
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
