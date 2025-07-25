import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ListTarefasComponent } from './components/tarefas/list-tarefas/list-tarefas.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { 
      path: 'tarefas', 
      component: ListTarefasComponent,
      canActivate: [authGuard]
    }
  ];
