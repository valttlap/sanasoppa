import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Router } from '@angular/router';
import { GameHubService } from 'src/app/services/gamehub.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
})
export class WelcomeComponent implements OnInit {
  username?: string;
  private gameHub!: signalR.HubConnection;

  constructor(private router: Router, private gameHubService: GameHubService) {}

  ngOnInit() {
    try {
      this.gameHub = this.gameHubService.getHubConnection();
    } catch (e) {
      this.router.navigate(['/error', e]);
    }
  }

  createGame() {
    if (!this.username) return;
    this.gameHub
      .invoke('CreateGame', this.username)
      .then((gameId: string) => {
        this.router.navigate(['/lobby', gameId]);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }

  joinGame() {
    const gameId = prompt('Enter game ID:');
    this.gameHub
      .invoke('JoinGame', gameId, this.username)
      .then(() => {
        this.router.navigate(['/lobby', gameId]);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }
}
