import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameRoutingModule } from './game-routing.module';
import { LobbyComponent } from './lobby/lobby.component';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [LobbyComponent],
  imports: [CommonModule, SharedModule, GameRoutingModule],
})
export class GameModule {}
