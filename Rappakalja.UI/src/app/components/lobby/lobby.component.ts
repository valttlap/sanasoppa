import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { GameHubService } from 'src/app/services/gamehub.service';
import { HubConnection } from '@microsoft/signalr';
import { IPlayer } from 'src/app/models/IPlayer';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
})
export class LobbyComponent implements OnInit {
  lobbyId?: string;
  gameHub!: HubConnection;
  players: IPlayer[] = [];

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
    this.gameHub.on('PlayerJoined', (player: string) => this.addPlayer(player));
  }

  addPlayer(playerName: string) {
    const newPlayer: IPlayer = {
      name: playerName,
    };
    this.players.push(newPlayer);
  }
}
