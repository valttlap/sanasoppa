import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CallbackRoutingModule } from './callback-routing.module';
import { CallbackComponent } from './callback.component';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [CallbackComponent],
  imports: [CommonModule, CallbackRoutingModule, SharedModule],
})
export class CallbackModule {}
