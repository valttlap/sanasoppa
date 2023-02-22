import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { GameHubService } from 'src/app/services/gamehub.service';
import { HubConnection } from '@microsoft/signalr';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
})
export class LobbyComponent implements OnInit {
  lobbyId?: string;
  gameHub!: HubConnection;
  players: string[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private gameHubService: GameHubService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }
    this.lobbyId = id;
    try {
      this.gameHub = this.gameHubService.getHubConnection();
    } catch (e) {
      this.router.navigate(['/error', e]);
    }
    this.gameHub
      .invoke('GetPlayers', this.lobbyId)
      .then((players: string[]) => (this.players = players));
    this.gameHub.on(
      'PlayerJoined',
      (players: string[]) => (this.players = players)
    );
  }
}
