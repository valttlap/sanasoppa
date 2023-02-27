import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { take } from 'rxjs';
import { Game } from 'src/app/_models/game';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { GameHubService } from 'src/app/_services/gamehub.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
})
export class ListComponent implements OnInit {
  gameHub!: HubConnection;
  players: string[] = [];
  user?: User;
  games: Game[] = [];

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
    this.gameHubService.startConnection(this.user);
    this.gameHub = this.gameHubService.getHubConnection();
    this.gameHubService.getNotStartedGames().subscribe({
      next: games => this.updateGames(games),
      error: error => console.log(error),
    });
    this.gameHub.on('GameListUpdated', (games: Game[]) =>
      this.updateGames(games)
    );
  }

  updateGames(games: Game[]) {
    this.games = games;
  }

  joinGame(gameName: string) {
    this.gameHub
      .invoke('JoinGame', gameName)
      .then(() => {
        this.router.navigate(['/lobby', gameName]);
      })
      .catch((e: unknown) => {
        console.log(e);
      });
  }
}
