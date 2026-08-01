import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'members',
        pathMatch: 'full'
    },
    {
        path: 'members',
        loadComponent: () =>
            import('./members-component/members-component').then(m => m.MembersComponent)  
    },
    // {
    //     path: 'members/:id',
    //     loadComponent: () =>
    //         import('./members/member-detail/member-detail.component').then(m => m.MemberDetail)
    // }
];
