import { LobbyHubService } from './../../_services/lobbyhub.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { take } from 'rxjs';
import { Game } from 'src/app/_models/game';
import { GameHubService } from 'src/app/_services/gamehub.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService, User } from '@auth0/auth0-angular';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
})
export class ListComponent implements OnInit, OnDestroy {
  lobbyHub!: HubConnection;
  gameName?: string;
  players: string[] = [];
  user?: User;
  games: Game[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private lobbyHubService: LobbyHubService,
    private gameHubService: GameHubService,
    private auth: AuthService,
    private modalService: NgbModal
  ) {
    this.auth.user$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      },
    });
  }
  ngOnDestroy(): void {
    this.lobbyHubService.disconnect();
  }

  ngOnInit(): void {
    if (!this.user) {
      this.router.navigate(['/']);
      return;
    }
    this.lobbyHubService.startConnection();
    this.lobbyHub = this.lobbyHubService.getHubConnection();
    this.lobbyHub.on('GameListUpdated', (games: Game[]) =>
      this.updateGames(games)
    );
    this.gameHubService.getNotStartedGames().subscribe({
      next: games => this.updateGames(games),
      error: error => console.error(error),
    });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  open(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then((result: string) => {
        this.createGame(result);
      });
  }

  updateGames(games: Game[]) {
    this.games = games;
  }

  joinGame(game: string) {
    this.router.navigate(['lobby', game]);
  }

  createGame(name: string) {
    this.lobbyHub
      .invoke('CreateGame', name)
      .then((name: string) => {
        this.router.navigate(['lobby', name]);
      })
      .catch(error => {
        console.error(error);
      });
  }
}
