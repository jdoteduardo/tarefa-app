import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ListTarefasComponent } from './components/tarefas/list-tarefas/list-tarefas.component';
import { authGuard } from './guards/auth.guard';
import { FormTarefasComponent } from './components/tarefas/form-tarefas/form-tarefas.component';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { 
      path: 'tarefas', 
      component: ListTarefasComponent,
      canActivate: [authGuard]
    },
    { 
      path: 'tarefas/nova', 
      component: FormTarefasComponent,
      canActivate: [authGuard]
    },
    {
      path: 'tarefas/:id', 
      component: FormTarefasComponent,
      canActivate: [authGuard]
    }
  ];
