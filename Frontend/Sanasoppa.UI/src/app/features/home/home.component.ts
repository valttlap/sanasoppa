import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { take } from 'rxjs';
import { GameHubService, GameService } from '@app/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  user$ = this.auth.user$;
  constructor(
    private auth: AuthService,
    private gameHubService: GameHubService,
    private gameService: GameService
  ) {}

  getGames(): void {
    this.gameService
      .getNotStartedGames()
      .pipe(take(1))
      .subscribe(game => console.log(game));
  }
}
