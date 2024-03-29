import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { GameHubService } from '@app/core';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
})
export class CreateComponent implements OnInit {
  gamehub!: HubConnection;
  gameName?: string;
  constructor(private gamehubService: GameHubService, private router: Router) {}

  ngOnInit(): void {
    this.gamehub = this.gamehubService.hubConnection;
  }

  createGame() {
    this.gamehub
      .invoke('CreateGame', this.gameName)
      .then((gameName: string) => {
        this.router.navigate(['/lobby', gameName]);
      })
      .catch((e: unknown) => {
        console.error(e);
      });
  }
}
