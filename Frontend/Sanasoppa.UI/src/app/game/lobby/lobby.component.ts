import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { GameHubService } from 'src/app/_services/gamehub.service';
import { HubConnection } from '@microsoft/signalr';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { IPlayer } from 'src/app/_models/IPlayer';
import { Game } from 'src/app/_models/game';

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
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
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
    this.gameHubService.getGame(this.name).subscribe({
      next: game => {
        this.game = game;
      },
      error: () => {
        this.router.navigate(['/notfound']);
      },
    });
    try {
      this.gameHubService.startConnection(this.user, this.name);
      this.gameHub = this.gameHubService.getHubConnection();
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
    this.gameHubService.getPlayersInGame(this.name).subscribe({
      next: players => (this.players = players),
      error: error => console.log(error),
    });
  }
}
