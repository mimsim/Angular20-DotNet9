import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IMember } from '../../shared/interfaces/members.interfaces';
import { AuthService } from '../../shared/services/auth-service';
import { MembersService } from '../../shared/services/members-service';



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

  readonly member = signal<IMember | null>(null);
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditing = signal(false);
  readonly isSaving = signal(false);

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

  ngOnInit(): void {
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
      },
      error: () => {
        this.errorMessage.set('Неуспешно зареждане на профила.');
        this.isLoading.set(false);
      },
    });
  }

  onStartEdit(): void {
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

  onCancelEdit(): void {
    this.isEditing.set(false);
  }

  onSave(): void {
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
}