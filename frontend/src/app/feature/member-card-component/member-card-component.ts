import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IMember } from '../../shared/interfaces/members.interfaces';
import { AuthService } from '../../shared/services/auth-service';
import { MembersService } from '../../shared/services/members-service';
import { LikesService } from '../../shared/services/likes-service';



@Component({
  selector: 'app-member-card-component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './member-card-component.html',
  styleUrl: './member-card-component.scss',
})
export class MemberCardComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly membersService = inject(MembersService);
  private readonly authService = inject(AuthService);
  private readonly fb = inject(FormBuilder);
  private readonly likesService = inject(LikesService);
  readonly member = signal<IMember | null>(null);
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditing = signal(false);
  readonly isSaving = signal(false);
  protected hasLiked = computed(() =>
    this.likesService.likeIds().includes(this.member()?.userId ?? '')
  );
  // Едит бутон се вижда само ако разглеждаш собствения си профил
  readonly isOwnProfile = computed(() => {
    const currentUser = this.authService.currentUser();
    const viewedMember = this.member();
    return !!currentUser && !!viewedMember && currentUser.id === viewedMember.userId;
  });

  editForm: FormGroup = this.fb.group({
    displayName: ['', [Validators.required, Validators.minLength(2)]],
    dateOfBirth: ['', Validators.required],
    gender: ['', Validators.required],
    description: [''],
    city: ['', Validators.required],
    country: ['', Validators.required],
    imageUrl: [''],
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage.set('Липсва ID на потребител.');
      this.isLoading.set(false);
      return;
    }

    this.membersService.getMemberById(id).subscribe({
      next: (member) => {
        this.member.set(member);
        this.isLoading.set(false);
        console.log('member', member);
      },
      error: () => {
        this.errorMessage.set('Неуспешно зареждане на профила.');
        this.isLoading.set(false);
      },
    });
  }

  onStartEdit() {
    const m = this.member();
    if (!m) return;

    this.editForm.patchValue({
      displayName: m.displayName,
      dateOfBirth: m.dateOfBirth,
      gender: m.gender,
      description: m.description,
      city: m.city,
      country: m.country,
      imageUrl: m.imageUrl,
    });

    this.isEditing.set(true);
  }

  onCancelEdit() {
    this.isEditing.set(false);
  }

  onSave() {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }

    this.isSaving.set(true);

    const formValue = this.editForm.getRawValue() as Partial<IMember>;

    this.membersService.updateMember(formValue).subscribe({
      next: () => {
        // ъпдейтваме локалния signal веднага, без нужда от нов GET
        const updatedMember = { ...(this.member() ?? {} as IMember), ...formValue } as IMember;
        this.member.set(updatedMember);
        this.isSaving.set(false);
        this.isEditing.set(false);
        console.log('success', formValue)
      },
      error: () => {
        this.isSaving.set(false);
        this.errorMessage.set('Неуспешно запазване на промените.');
      },
    });
  }

//   onLikeMember() {
//     const memberId = this.member()?.userId
// ;
//     if (!memberId) return;

//     this.likesService.toggleLike(memberId).subscribe({
//       next: () => {
//         this.likesService.getLikeIds();
//       },
//       error: () => {
//         this.errorMessage.set('Неуспешно харесване на потребителя.');
//       },
//     });
//   }


  toggleLike(targetMemberId: string) {
    this.likesService.toggleLike(targetMemberId).subscribe(() => {
      const current = this.likesService.likeIds();
      const isLiked = current.includes(targetMemberId);

      this.likesService.likeIds.set(
        isLiked
          ? current.filter(id => id !== targetMemberId)
          : [...current, targetMemberId]
      );
    });
  }
}