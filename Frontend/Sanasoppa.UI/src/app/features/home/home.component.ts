import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { take } from 'rxjs';
import { GameHubService } from 'src/app/_services/gamehub.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  user$ = this.auth.user$;
  constructor(private auth: AuthService, private game: GameHubService) {}

  getGames(): void {
    this.game
      .getNotStartedGames()
      .pipe(take(1))
      .subscribe(game => console.log(game));
  }
}
