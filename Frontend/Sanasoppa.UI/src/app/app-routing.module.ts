import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from './components/error/error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { CreateComponent } from './game/create/create.component';
import { ListComponent } from './game/list/list.component';
import { LobbyComponent } from './game/lobby/lobby.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [() => inject(AuthGuard).canActivate()],
    children: [
      { path: 'list', component: ListComponent },
      { path: 'create', component: CreateComponent },
      { path: 'lobby/:name', component: LobbyComponent },
    ],
  },
  {
    path: 'error/:message',
    component: ErrorComponent,
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
