import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { ListComponent } from './game/list/list.component';
import { TestComponent } from './features/test/test.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () =>
      import('./features/home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'callback',
    loadChildren: () =>
      import('./features/callback/callback.module').then(m => m.CallbackModule),
  },
  {
    path: 'list',
    component: ListComponent,
  },
  {
    path: 'test',
    component: TestComponent,
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
