import { GameService } from './../../core/services/game.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { take } from 'rxjs';
import { Game, GameHubService, LobbyHubService } from '@app/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService, User } from '@auth0/auth0-angular';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
})
export class ListComponent implements OnInit {
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
    private gameService: GameService,
    private auth: AuthService,
    private modalService: NgbModal
  ) {
    this.auth.user$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      },
    });
  }

  async ngOnInit(): Promise<void> {
    if (!this.user) {
      this.router.navigate(['/']);
      return;
    }
    await this.lobbyHubService.startConnection();
    this.lobbyHub = this.lobbyHubService.hubConnection;
    this.lobbyHub.on('GameListUpdated', (games: Game[]) =>
      this.updateGames(games)
    );
    this.gameService.getNotStartedGames().subscribe({
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
    if (!this.lobbyHub) console.error('LobbyHub is not initialized');
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
