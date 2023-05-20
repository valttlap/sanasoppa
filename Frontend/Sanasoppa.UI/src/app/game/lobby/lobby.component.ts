import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { GameHubService, GameService } from '@app/core';
import { HubConnection } from '@microsoft/signalr';
import { take } from 'rxjs';
import { IPlayer } from 'src/app/_models/IPlayer';
import { Game } from 'src/app/_models/game';
import { AuthService, User } from '@auth0/auth0-angular';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
})
export class LobbyComponent implements OnInit {
  name!: string;
  gameHub!: HubConnection;
  players: IPlayer[] = [];
  user?: User;
  game?: Game;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private gameHubService: GameHubService,
    private gameService: GameService,
    private auth: AuthService
  ) {
    this.auth.user$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      },
    });
  }

  ngOnInit(): void {
    if (!this.user) {
      this.router.navigate(['/']);
      return;
    }
    const name = this.route.snapshot.paramMap.get('name');
    if (!name) {
      this.router.navigate(['/notfound']);
      return;
    }
    this.name = name;
    this.gameService.getGame(this.name).subscribe({
      next: game => {
        this.game = game;
      },
      error: () => {
        this.router.navigate(['/notfound']);
      },
    });
    try {
      this.gameHubService.startConnection(this.name);
      this.gameHub = this.gameHubService.hubConnection;
    } catch (e) {
      this.router.navigate(['/error', e]);
    }
    this.refeshPlayers();
    this.gameHub.on(
      'PlayerJoined',
      (players: IPlayer[]) => (this.players = players)
    );
  }

  refeshPlayers() {
    this.gameService.getPlayersInGame(this.name).subscribe({
      next: players => (this.players = players),
      error: error => console.error(error),
    });
  }
}
