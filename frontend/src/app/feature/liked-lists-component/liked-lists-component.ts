import { Component, inject, signal } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { IMember } from '../../shared/interfaces/members.interfaces';
import { LikesService } from '../../shared/services/likes-service';

@Component({
  selector: 'app-liked-lists-component',
  imports: [CommonModule, RouterLink, MaterialModule],
  templateUrl: './liked-lists-component.html',
  styleUrl: './liked-lists-component.scss',
})
export class LikedListsComponent {
  private readonly likesService = inject(LikesService);

  protected members = signal<IMember[]>([]);
  protected predicate = 'liked';
  protected isLoading = signal(false);

  tabs = [
    { label: 'Liked', value: 'liked' },
    { label: 'Liked me', value: 'likedBy' },
    { label: 'Mutual', value: 'mutual' },
  ];

  ngOnInit(): void {
    this.loadLikes();
  }

  onTabChange(index: number) {
    const tab = this.tabs[index];
    if (this.predicate === tab.value) return;
    this.predicate = tab.value;
    this.loadLikes();
  }

  loadLikes() {
    this.isLoading.set(true);

    this.likesService.getLikes(this.predicate).subscribe({
      next: (members) => {
        this.members.set(members);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  }
}
