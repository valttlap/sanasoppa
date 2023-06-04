import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { SharedModule } from '@app/shared';
import { RouterModule } from '@angular/router';
import { SplitTextDirective } from '@app/core';

@NgModule({
  declarations: [HomeComponent, SplitTextDirective],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([{ path: '', component: HomeComponent }]),
  ],
})
export class HomeModule {}
