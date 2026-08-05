import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        loadComponent: () =>
            import('./feature/home-component/home-component').then(m => m.HomeComponent)
    },
    {
        path: 'login',
        loadComponent: () =>
            import('./feature/account/login-component/login-component').then(m => m.LoginComponent)
    },
    {
        path: 'members',
        loadComponent: () =>
            import('./members-component/members-component').then(m => m.MembersComponent)
    },
    {
        path: 'members/:id',
        loadComponent: () =>
            import('./feature/member-card-component/member-card-component').then(m => m.MemberCardComponent)
    }
];
